import {IFormConfiguration} from "./IFormConfiguration.js";
import {DmObjectWithSync} from "../Mof.js";
import {debugElementToDom} from "../DomHelper.js";
import * as Forms from "./Forms.js";
import * as ObjectForm from "./ObjectForm.js";
import * as FormActions from "../FormActions.js";
import * as ClientForms from '../client/Forms.js'
import * as ClientElements from '../client/Elements.js'
import * as ClientItems from '../client/Items.js'
import * as MofSync from "../MofSync.js"

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
        await MofSync.sync(element);
        let loadedElement = await ClientItems.getObjectByUri(element.workspace, element.uri);
        
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

        const temporaryElement = await ClientElements.createTemporaryElement(metaClass);
        element = DmObjectWithSync.createFromReference(temporaryElement.workspace, temporaryElement.uri);

        // Sets the metaclass and workspace id upon url, if not created by Modules
        let p = new URLSearchParams(window.location.search);
        const metaclass = p.get('metaclass');
        const metaclassWorkspace = p.get('metaclassworkspace');
        if (metaclass !== undefined && metaclass !== null) {
            element.setMetaClassByUri(metaClass, metaclassWorkspace);
        }

        const workspaceId = p.get('workspaceId');
        if (workspaceId !== undefined) {
            element.set('workspaceId', workspaceId);
        }
    }
    else {
        // Checks whether the object has a copy on the server and checks the type of the object
        if (element.uri === undefined || element.workspace === undefined || element.propertiesSet === undefined) {
            throw "Element is not linked to the server or is not of Type MofObjectWithSync";
        }
    }

    /* Now find the right form */
    let form;

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
    creator.element = element;
    creator.formElement = form;
    creator.workspace = "Data";
    creator.extentUri = creator.element.extentUri;
    
    configuration.submitName = module.actionVerb;

    // Finally, we have everything together, create the form
    await creator.createFormByObject(
        {
            itemContainer: parent
        }, configuration);

    // Asks the detail form actions, whether we have a form for the action itself
    await module.preparePage(creator.element, form);

    debugElementToDom(form, "#debug_formelement");
}