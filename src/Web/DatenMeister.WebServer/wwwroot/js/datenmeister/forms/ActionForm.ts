import {IFormConfiguration} from "./IFormConfiguration.js";
import {DmObject, DmObjectWithSync, ObjectType} from "../Mof.js";
import {debugElementToDom} from "../DomHelper.js";
import * as Forms from "./Forms.js";
import * as ObjectForm from "./ObjectForm.js";
import * as FormActions from "../FormActions.js";
import * as ClientForms from '../client/Forms.js'
import * as ClientElements from '../client/Elements.js'
import * as ClientItems from '../client/Items.js'
import * as MofSync from "../MofSync.js"
import {StatusFieldControl} from "../controls/StatusFieldControl.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";

export async function createActionFormForEmptyObject(
    parent: JQuery<HTMLElement>,
    metaClass: string,
    configuration: IFormConfiguration,
    actionName: string) {
    
    const statusOverview = new StatusFieldControl();
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
        configuration.refreshForm = async () => {
            await createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
        }
    }

    const creator = new ObjectForm.ObjectFormCreator(
        {
            itemContainer: parent
        });

    configuration.onSubmit = async (element, method) => {

        statusOverview.setListStatus("Sync Object with Server", false);
        // Stores the most recent changes on the server        
        await MofSync.sync(element);
        statusOverview.setListStatus("Sync Object with Server", true);
        
        let loadedElement = await ClientItems.getObjectByUri(element.workspace, element.uri);

        statusOverview.setListStatus("Execute Action", false);
        // Executes the detail form
        const result = await FormActions.execute(
            actionName,
            creator,
            loadedElement,
            undefined, // The action form cannot provide additional parameters as the ActionButton
            method);
        statusOverview.setListStatus("Execute Action", true);

        if (result !== undefined) {

            statusOverview.setListStatus("Execute Client-Action", false);


            // Checks, if we are having a client-actions responded back from the server
            const resultAsMof = result as DmObject;
            const clientActions = resultAsMof.get(_DatenMeister._Actions._ActionResult.clientActions, ObjectType.Array);
            if (clientActions !== undefined) {

                for (let n in clientActions) {

                    // Try to find the module and execute the client action
                    const clientAction = clientActions[n] as DmObject;
                    FormActions.executeClientAction(clientAction, creator);
                }                
            }

            statusOverview.setListStatus("Execute Client-Action", true);
        }
    };

    /* Loads the object being used as a base for the new action.
    * Usually context information from GET-Query are retrieved. Or some default fields are filled out
    */
    statusOverview.setListStatus("Load Object", false);
    let element = await module.loadObject();
    statusOverview.setListStatus("Load Object", true);
    
    if (element === undefined) {
        statusOverview.setListStatus("Create Temporary Object", false);
        const temporaryElement = 
            await ClientElements.createTemporaryElement(metaClass);
        statusOverview.setListStatus("Create Temporary Object", true);
        element = DmObjectWithSync.createFromReference(temporaryElement.workspace, temporaryElement.uri);
        

        // Sets the metaclass and workspace id upon url, if not created by Modules
        let p = new URLSearchParams(window.location.search);
        const metaclass = p.get('metaclass');
        const metaclassWorkspace = p.get('metaclassworkspace') ?? 'Types';
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
    statusOverview.setListStatus("Load Form", false);
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
            form = await ClientForms.getDefaultObjectForMetaClass(metaClass);
        }
    }
    
    statusOverview.setListStatus("Load Form", true);

    // Creates the object as being provided by the uri
    creator.element = element;
    creator.formElement = form;
    creator.workspace = element.workspace;
    creator.itemUrl = element.uri;
    creator.extentUri = creator.element.extentUri;
    configuration.viewMode = "ViewMode.DataManipulation";
    
    configuration.submitName = module.actionVerb;

    // Finally, we have everything together, create the form
    statusOverview.setListStatus("Create Form By Object", false);
    await creator.createFormByObject(configuration);
    statusOverview.setListStatus("Create Form By Object", true);

    // Asks the detail form actions, whether we have a form for the action itself
    statusOverview.setListStatus("Prepare Page", false);
    await module.preparePage(creator.element, form);
    statusOverview.setListStatus("Prepare Page", true);

    debugElementToDom(form, "#debug_formelement");
}