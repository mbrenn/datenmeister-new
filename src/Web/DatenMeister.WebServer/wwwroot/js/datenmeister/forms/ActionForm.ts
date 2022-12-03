import {IFormConfiguration} from "./IFormConfiguration";
import {DmObject} from "../Mof";
import {debugElementToDom} from "../DomHelper";
import * as Forms from "./Forms";
import * as ObjectForm from "./ObjectForm";
import * as FormActions from "../FormActions";
import * as ClientForms from '../client/Forms'
import * as ClientElements from '../client/Elements'
import * as ClientItems from '../client/Items'
import * as DataLoader from "../client/Items";

export async function createActionFormForEmptyObject(
    parent: JQuery<HTMLElement>,
    metaClass: string,
    configuration: IFormConfiguration,
    actionName: string) {
    
    const module = FormActions.getModule(actionName);
    if (module === undefined)
    {
        parent.text("Unknown action: " + actionName);
        return;
    }

    configuration.submitName = "Perform Action";
    configuration.showCancelButton = false;
    configuration.allowAddingNewProperties = false;

    if (configuration.formUri === "") {
        configuration.formUri = undefined;
    }

    if (configuration.refreshForm === undefined) {
        configuration.refreshForm = () => {
            createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
        }
    }

    const creator = new ObjectForm.ObjectFormCreator();

    configuration.onSubmit = async (element, method) => {

        // Stores the most recent changes on the server
        await DataLoader.setProperties("Data", temporaryElement.uri, element);
        let loadedElement = await ClientItems.getObjectByUri("Data", temporaryElement.uri);
        
        // Executes the detail form
        await FormActions.execute(
            actionName,
            creator,
            loadedElement,
            undefined, // The action form cannot provide additional parameters as the ActionButton
            method);
    };

    /* Loads the object being used as a base for the new action.
    * Usually context information from GET-Query are retrieved. Or some default fields are filled out
    */
    let element = await module.loadObject();
    if (element === undefined) {
        
        element = new DmObject();
        
        // Sets the metaclass and workspace id upon url, if not created by Modules
        let p = new URLSearchParams(window.location.search);
        const metaclass = p.get('metaclass');
        if (metaclass !== undefined && metaclass !== null) {
            element.setMetaClassByUri(metaclass);
        }

        const workspaceId = p.get('workspaceId');
        if (workspaceId !== undefined) {
            element.set('workspaceId', workspaceId);
        }
    }

    // If, we have created the element, we will now have to create the temporary object on the server
    const temporaryElement = await ClientElements.createTemporaryElement(element.metaClass?.uri);
    await ClientItems.setProperties("Data", temporaryElement.uri, element);

    /* Now find the right form */

    let form;

    // After having loaded the object, load the form
    if (metaClass === undefined && element.metaClass?.uri !== undefined) {
        // If the returned element has a metaclass, then set the metaClass being used to 
        // find the right form to the one by the element
        metaClass = element.metaClass.uri;
    } else if (element.metaClass === undefined) {
        // Updates the metaclass, if the metaclass is not set by the element itself
        element.setMetaClassByUri(metaClass);
    }

    // Asks the detail form actions, whether we have a form for the action itself
    form = await module.loadForm(metaClass);
    if (form === undefined) {
        // Defines the form
        if (configuration.formUri !== undefined) {
            // Ok, we already have an explicit form
            form = await ClientForms.getForm(configuration.formUri);
        } else if (metaClass === undefined) {
            // If there is no metaclass set, create a total empty form object...
            form = Forms.FormModel.createEmptyFormObject();
        } else {
            form = await ClientForms.getObjectFormForMetaClass(metaClass);
        }
    }

    // Creates the object as being provided by the uri
    creator.element = await ClientItems.getObjectByUri("Data", temporaryElement.uri);
    creator.formElement = form;
    creator.workspace = "Data";
    creator.extentUri = creator.element.extentUri;
    
    configuration.submitName = module.actionVerb;

    // Finally, we have everything together, create the form
    await creator.createFormByObject(
        {
            itemContainer: parent
        }, configuration);

    debugElementToDom(form, "#debug_formelement");
}