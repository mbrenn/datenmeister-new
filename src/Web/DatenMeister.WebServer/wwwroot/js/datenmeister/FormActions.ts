import * as Settings from "./Settings";
import * as ApiConnection from "./ApiConnection";
import * as Navigator from "./Navigator";
import {createJsonFromObject, DmObject} from "./Mof";
import * as IIForms from "./forms/Interfaces";
import * as FormClient from "./client/Forms";
import * as ActionClient from "./client/Actions";
import * as DatenMeisterModel from "./models/DatenMeister.class";
import {SubmitMethod} from "./forms/RowForm";
import {
    moveItemInExtentDown,
    moveItemInExtentUp
} from "./client/Actions.Items";

/**
 * This interface describes one module being used for the action form
 * One module hosts exactly one action, a plugin can add multiple modules 
 * to the manager, if one plugin covers multiple modules. 
 */
export interface IItemFormActionModule
{
    /**
     * Defines the name of the action. This name is used to look up the action 
     */
    actionName: string;

    /**
     * Defines the verb of the action. This information is used to fill the button
     */
    actionVerb?: string;

    /**
     * Loads the object for a certain action. 
     * Can be undefined, if a default object can be used
     */
    loadObject(): Promise<DmObject> | undefined;

    /**
     * Loads a certain form fitting to the action
     * Can be undefined, if a default form shall be generated 
     */
    loadForm(): Promise<DmObject> | undefined;

    /**
     * Will be called to execute the action 
     * @param form The form that has been used to trigger the action by the 
     * user. It contains additional information of the workspace and extent
     * @param element The element itself which has been clicked
     * @param parameter The parameters which are provided by the server for this
     * specific action. These parameters are typically set by the configuration
     * of the action button
     * @param submitMethod The type of the submit action being clicked by the
     * user. 
     */
    execute(
        form: IIForms.IFormNavigation,
        element: DmObject,
        parameter?: DmObject,
        submitMethod?: SubmitMethod) : Promise<void>;

    /**
     * Contains a flag, whether the action is a 'dangerous' action
     * and a reconfirmation by the user is expected.
     */
    requiresConfirmation: boolean | undefined;
}

/**
 * Defines the base implementation which can be overridden
 */
export class ItemFormActionModuleBase implements IItemFormActionModule
{
    constructor(actionName?:string) {
        this.actionName = actionName;
    }
    
    actionName: string;
    actionVerb: string;
    requiresConfirmation: boolean | undefined;

    execute(form: IIForms.IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        return Promise.resolve(undefined);
    }

    loadForm(): Promise<DmObject> | undefined {
        return Promise.resolve(undefined);
    }

    loadObject(): Promise<DmObject> | undefined {
        return Promise.resolve(undefined);
    }    
}

let modules: Array<IItemFormActionModule> = new Array<IItemFormActionModule>();

export function addModule(module: IItemFormActionModule) {
    // Checks, if there is already a module register. If yes, throw an exception
    if (getModule(module.actionName) !== undefined) {
        throw "A module with action name " + module.actionName + " is already registered";
    }

    // Adds the module
    modules.push(module);
}

export function getModule(actionName:string): IItemFormActionModule | undefined {
    for (let n in modules) {
        const module = modules[n];
        if (module.actionName === actionName) {
            return module;
        }
    }

    return undefined;
}

export async function loadObjectForAction(actionName: string): Promise<DmObject> | undefined {
    const foundModule = getModule(actionName);
    if (foundModule !== undefined) {
        return foundModule.loadObject();
    }

    /* Nothing has been found, so return an undefined */
    return Promise.resolve(undefined);
}

/* Finds the best form fitting for the action */
export async function loadFormForAction(actionName: string) {
    const foundModule = getModule(actionName);
    if (foundModule !== undefined) {
        return foundModule.loadForm();
    }
    
    if (actionName === 'Forms.Create.ByMetaClass') {
        return await FormClient.getForm("dm:///_internal/forms/internal#Forms.Create.ByMetaClass");
    }

    return Promise.resolve(undefined);
}

export function requiresConfirmation(actionName: string): boolean {

    const foundModule = getModule(actionName);
    if (foundModule !== undefined) {
        return foundModule.requiresConfirmation === true;
    }
    
    if (actionName === "ExtentsList.DeleteItem") {
        return true;
    } else {
        return false;
    }
}

// Calls to execute the form actions.
// actionName: Name of the action to be executed. This is a simple string describing the action
// form: The form which was used to trigger the action
// itemUrl: The url of the item whose action will be executed
// element: The element which is reflected within the form
// parameter: These parameter are retrieved from the actionForm definition from the server and are forwarded
//    This supports the server to provide additional parameter for an action button
// submitMethod: Describes which button the user has clicked
export async function execute(
    actionName: string,
    form: IIForms.IFormNavigation,
    element: DmObject,
    parameter?: DmObject,
    submitMethod?: SubmitMethod) {

    const foundModule = getModule(actionName);
    if (foundModule !== undefined) {
        return foundModule.execute(form, element, parameter, submitMethod);
    }
    
    let workspaceId;
    let extentUri;
    let p = new URLSearchParams(window.location.search);
    switch (actionName) {
        case "Extent.CreateItemInProperty":
            if (!p.has("itemUrl") || !p.has("workspace") || !p.has("property")) {
                alert('There is no itemUrl given');
            } else {
                const workspace = p.get('workspace');
                const itemUrl = p.get('itemUrl');
                const property = p.get('property');
                const metaclass = p.get('metaclass');
                await FormActions.extentCreateItemInProperty(workspace, itemUrl, property, element, metaclass);
            }
            break;
        case "ExtentsList.ViewItem":
            FormActions.itemNavigateTo(form.workspace, element.uri);
            break;
        case "ExtentsList.DeleteItem":
            await FormActions.extentsListDeleteItem(form.workspace, form.extentUri, element.uri);
            break;
        case "ExtentsList.MoveUpItem":
            await FormActions.extentsListMoveUpItem(form.workspace, form.extentUri, element.uri);
            break;
        case "ExtentsList.MoveDownItem":
            await FormActions.extentsListMoveDownItem(form.workspace, form.extentUri, element.uri);
            break;

        case "Forms.Create.ByMetaClass": {
            const extentCreationParameter = new DmObject();
            extentCreationParameter.set('configuration', element);
            extentCreationParameter.setMetaClassByUri(
                DatenMeisterModel._DatenMeister._Actions.__CreateFormByMetaClass_Uri
            );

            const result = await ActionClient.executeActionDirectly(
                "Execute",
                {
                    parameter: extentCreationParameter
                }
            );

            if (result.success !== true) {
                alert('Form was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
            } else {
                alert('Form was created successfully');
            }

            break;
        }

        case "JSON.Item.Alert":
            alert(JSON.stringify(createJsonFromObject(element)));
            break;
        
        case "Action.Execute":
            // Executes the action directly
            const result = await ActionClient.executeAction(
                element.workspace,
                element.uri
            );

            if (result.success) {
                alert('Success');
            } else {
                alert('Failure');
            }

            break;

        default:
            alert("Unknown action type: " + actionName);
            break;
    }
}

interface IDeleteCallbackData {
    success: boolean;
}

export class FormActions {

    static workspaceNavigateTo(workspace: string) {
        document.location.href =
            Settings.baseUrl + "Item/Management/dm:%2F%2F%2F_internal%2Fworkspaces/" + encodeURIComponent(workspace);
    }

    static async extentCreateItemInProperty(workspace: string, itemUrl: string, property: string, element: DmObject, metaClass?: string) {
        const json = createJsonFromObject(element);
        await ApiConnection.post(
            Settings.baseUrl + "api/items/create_child/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(itemUrl),
            {
                metaClass: (metaClass === undefined || metaClass === null) ? "" : metaClass,
                property: property,
                asList: true,
                properties: json
            }
        );

        Navigator.navigateToItemByUrl(workspace, itemUrl);
    }
    
    // Performs the navigation to the given item. The ItemUrl may be a uri or just the id
    static itemNavigateTo(workspace: string, itemUrl: string) {
        Navigator.navigateToItemByUrl(
            workspace,
            itemUrl);
    }

    static async extentsListDeleteItem(workspace: string, extentUri: string, itemId: string) {

        const data = await ApiConnection.deleteRequest<IDeleteCallbackData>(
            Settings.baseUrl + "api/items/delete/"
            + encodeURIComponent(workspace) + "/" +
            encodeURIComponent(itemId),
            {}
        );

        const success = data.success;
        if (success) {
            document.location.reload();
        } else {
            alert('Deletion was not successful.');
        }
    }

    static async extentsListMoveUpItem(workspace: string, extentUri: string, itemId: string) {
        await moveItemInExtentUp(workspace, extentUri, itemId);
        document.location.reload();
    }

    static async extentsListMoveDownItem(workspace: string, extentUri: string, itemId: string) {
        await moveItemInExtentDown(workspace, extentUri, itemId);
        document.location.reload();
    }
}