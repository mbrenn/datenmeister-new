import * as Mof from "./Mof.js";
import * as IIForms from "./forms/Interfaces.js";
import { SubmitMethod } from "./forms/Forms.js";
import * as _DatenMeister from "./models/DatenMeister.class.js"
import * as ActionClient from "./client/Actions.js"

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
     * Alternatively, defines the action by its metaclass uri.
     */
    actionMetaClassUri: string; 

    /**
     * Defines the verb of the action. This information is used to fill the button
     */
    actionVerb?: string;

    /**
     * Defines the headline of the action form. Can be undefined, so actionVerb is used.
     */
    actionHeading: string | undefined;

    /**
     * Loads the object for a certain action. 
     * Can be undefined, if a default object can be used
     */
    loadObject(): Promise<Mof.DmObjectWithSync> | undefined;

    /**
     * Loads a certain form fitting to the action
     * Can be undefined, if a default form shall be generated 
     */
    loadForm(metaClass?: string): Promise<Mof.DmObject> | undefined;

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
        element: Mof.DmObject,
        parameter?: Mof.DmObject,
        submitMethod?: SubmitMethod): Promise<Mof.DmObject | void>;


    /**
     * Will be called when the form has been shown and the item to be shown is loaded. 
     * This event is only called, when the action represents a full form as by the page 'ItemAction'
     * @param element Element to be shown
     * @param form Form that was used. 
     */
    preparePage(element: Mof.DmObject, form: IIForms.IFormNavigation): Promise<void>

    /**
     * Contains a flag, whether the action is a 'dangerous' action
     * and a reconfirmation by the user is expected.
     */
    requiresConfirmation: boolean | undefined;

    /**
     * Gets or sets a flag indicating, whether the action shall trigger a saving of the shown item
     * This can be set to true, in case the action is just a navigation or does not require the storage
     * of an item.
     */
    skipSaving: boolean | undefined;
}

export function getActionHeading(module: IItemFormActionModule)
{
    if (module.actionHeading !== undefined) {
        return module.actionHeading;
    }
    
    return module.actionVerb;
}

/**
 * Defines the base implementation which can be overridden
 */
export class ItemFormActionModuleBase implements IItemFormActionModule {
    constructor(actionName?: string, actionMetaClassUri?: string) {
        this.actionName = actionName;
        this.actionMetaClassUri = actionMetaClassUri;
    }

    /** 
     * Name of the action as a coded information
     */
    actionName: string;

    /** 
     * Defines the uri of the metaclass of the action which provides a unique and 
     */
    actionMetaClassUri: string;

    /**
     * Defines the headline of the action form. Can be undefined, so actionVerb is used. 
     */
    actionHeading: string | undefined;

    /**
     * Represents an action verb that is used for the button or heading, if actionHeading is not set
     */
    actionVerb: string;

    requiresConfirmation: boolean | undefined;

    /**
     * Defines the default metaclass that will be used to create an empty item in the temporary extent.
     */
    defaultMetaClassUri: string | undefined;

    /**
     * Gets or sets a flag indicating, whether the action shall trigger a saving of the shown item
     * This can be set to true, in case the action is just a navigation or does not require the storage
     * of an item.
     */
    skipSaving: boolean | undefined;
        
    execute(form: IIForms.IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<Mof.DmObject | void> {
        return Promise.resolve(undefined);
    }

    loadForm(metaClass?: string): Promise<Mof.DmObject> | undefined {
        return Promise.resolve(undefined);
    }

    preparePage(element: Mof.DmObject, form: IIForms.IFormNavigation): Promise<void> | undefined {
        return Promise.resolve(undefined);
    }

    loadObject(): Promise<Mof.DmObjectWithSync> | undefined {
        if (this.defaultMetaClassUri !== undefined) {
            return Promise.resolve(
                new Mof.DmObjectWithSync(this.defaultMetaClassUri)
            );
        }

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
export function getModuleByUri(actionMetaClassUri: string): IItemFormActionModule | undefined {
    if (actionMetaClassUri === undefined || actionMetaClassUri === "") {
        return undefined;
    }

    for (let n in modules) {
        const module = modules[n];
        if (module.actionMetaClassUri === actionMetaClassUri) {
            return module;
        }
    }

    return undefined;
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
    element: Mof.DmObject,
    parameter?: Mof.DmObject,
    submitMethod?: SubmitMethod): Promise<Mof.DmObject | void> {

    const foundModule = getModule(actionName);
    if (foundModule !== undefined) {
        return foundModule.execute(form, element, parameter, submitMethod);
    }

    alert("Unknown action type: " + actionName);
}


export async function executeClientAction(    
    clientAction: Mof.DmObject,
    form?: IIForms.IFormNavigation,
    parameter?: Mof.DmObject,
    submitMethod?: SubmitMethod): Promise<Mof.DmObject | void> {

    submitMethod ??= SubmitMethod.Save;

    const moduleName = clientAction.get(_DatenMeister._Actions._ClientActions._ClientAction.actionName);
    let module = getModule(moduleName);

    if (module === undefined) {
        module = getModuleByUri(clientAction.metaClass?.uri);
    }
    if (module === undefined) {
        alert('Unknown action: ' + moduleName + clientAction.metaClass?.uri);
    } else {
        await module.execute(form, clientAction, parameter, submitMethod);
    }
}

/**
 * 
 * Executes the action by the given action object and executes the returned client actions
 * @param action The action to be executed
 * @param form The form which was used to trigger the action
 * @param element The element which is reflected within the form
 * @param parameter Additional parameters which could be provided to the action
 * @param submitMethod The method which was used to submit the action
 * @returns The result of the action
 */
export async function executeAction(
    action: Mof.DmObject,
    form?: IIForms.IFormNavigation,
    element?: Mof.DmObject,
    parameter?: Mof.DmObject,
    submitMethod?: SubmitMethod) {

    var module = getModuleByUri(action.metaClass?.uri);
    if (module === undefined) {
        alert("Unknown action: " + action.metaClass?.uri);
        return;
    }

    const result = await module.execute(form, element, parameter, submitMethod);

    if (result !== undefined) {

        // Checks, if we are having a client-actions responded back from the server
        const resultAsMof = result as Mof.DmObject;
        const clientActions = resultAsMof.get(_DatenMeister._Actions._ActionResult.clientActions, Mof.ObjectType.Array);
        if (clientActions !== undefined) {

            for (let n in clientActions) {

                // Try to find the module and execute the client action
                const clientAction = clientActions[n] as Mof.DmObject;
                executeClientAction(clientAction, form);
            }
        }
    }

    return result
}

export async function executeActionOnServer(
    action: Mof.DmObject,
    form?: IIForms.IFormNavigation) {

    const result = await ActionClient.executeActionDirectly("Execute", {
        parameter: action
    });

    if (result !== undefined) {

        if (!result.success) {
            throw result.reason + "\r\n" + result.stackTrace;
        }

        // Checks, if we are having a client-actions responded back from the server
        const resultAsMof = result.resultAsDmObject;
        if (resultAsMof !== undefined) {
            const clientActions = resultAsMof.get(_DatenMeister._Actions._ActionResult.clientActions, Mof.ObjectType.Array);
            if (clientActions !== undefined) {

                for (let n in clientActions) {

                    // Try to find the module and execute the client action
                    const clientAction = clientActions[n] as Mof.DmObject;
                    executeClientAction(clientAction, form);
                }
            }
        }
    }

    return result;
}