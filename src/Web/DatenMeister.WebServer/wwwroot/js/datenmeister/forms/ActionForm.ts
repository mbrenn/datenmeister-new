import {IFormConfiguration} from "./IFormConfiguration";
import {DmObject} from "../Mof";
import {debugElementToDom} from "../DomHelper";
import * as Forms from "./Forms";
import {DetailFormActions} from "../FormActions";
import * as ClientForms from '../client/Forms'
import * as ClientElements from '../client/Elements'
import * as ClientItems from '../client/Items'

export async function createActionFormForEmptyObject(
    parent: JQuery<HTMLElement>,
    metaClass: string,
    configuration: IFormConfiguration,
    actionName: string) {
    
    configuration.submitName = "Perform Action";
    configuration.showCancelButton = false;
    configuration.allowAddingNewProperties = false;

    if (configuration.refreshForm === undefined) {
        configuration.refreshForm = () => {            
            createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
        }
    }

    const creator = new Forms.DetailFormCreator();

    configuration.onSubmit = async (element, method) => {
        const loadedElement = await ClientItems.getObjectByUri("Data", creator.element.uri);
        
        await DetailFormActions.execute(
            actionName,
            creator,
            undefined,
            loadedElement,
            undefined, // The action form cannot provide additional parameters as the ActionButton
            method);
    };

    /* Loads the object being used as a base for the new action.
    * Usually context information from GET-Query are retrieved. Or some default fields are filled out
    */
    let element = await DetailFormActions.loadObjectForAction(actionName);
    if (element === undefined) {
        element = new DmObject();
    }
    
    // If, we have created the element, we will now have to create the temporary object on the server
    const temporaryElement = await ClientElements.createTemporaryElement();
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
    form = await DetailFormActions.loadFormForAction(actionName);
    if (form === undefined) {
        // Defines the form
        if (configuration.formUri !== undefined) {
            // Ok, we already have an explicit form
            form = await ClientForms.getForm(configuration.formUri);
        } else if (metaClass === undefined) {
            // If there is no metaclass set, create a total empty form object...
            form = Forms.FormModel.createEmptyFormWithDetail();
        } else {
            form = await ClientForms.getDefaultFormForMetaClass(metaClass);
        }
    }
    
    creator.element = await ClientItems.getObjectByUri("Data", temporaryElement.uri);
    creator.formElement = form;
    
    // Finally, we have everything together, create the form
    creator.createFormByObject(parent, configuration);

    debugElementToDom(form, "#debug_formelement");
}