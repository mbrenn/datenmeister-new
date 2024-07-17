import {DmObject, DmObjectWithSync} from "./Mof.js";
import * as IIForms from "./forms/Interfaces.js";
import { SubmitMethod } from "./forms/Forms.js";

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
    loadObject(): Promise<DmObjectWithSync> | undefined;

    /**
     * Loads a certain form fitting to the action
     * Can be undefined, if a default form shall be generated 
     */
    loadForm(metaClass?: string): Promise<DmObject> | undefined;

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
        submitMethod?: SubmitMethod): Promise<DmObject | void>;


    /**
     * Will be called when the form has been shown and the item to be shown is loaded. 
     * This event is only called, when the action represents a full form as by the page 'ItemAction'
     * @param element Element to be shown
     * @param form Form that was used. 
     */
    preparePage(element: DmObject, form: IIForms.IFormNavigation): Promise<void>

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

/**
 * Defines the base implementation which can be overridden
 */
export class ItemFormActionModuleBase implements IItemFormActionModule {
    constructor(actionName?: string) {
        this.actionName = actionName;
    }

    actionName: string;
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

    execute(form: IIForms.IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<DmObject | void> {
        return Promise.resolve(undefined);
    }

    loadForm(metaClass?: string): Promise<DmObject> | undefined {
        return Promise.resolve(undefined);
    }

    preparePage(element: DmObject, form: IIForms.IFormNavigation): Promise<void> | undefined {
        return Promise.resolve(undefined);
    }

    loadObject(): Promise<DmObjectWithSync> | undefined {
        if (this.defaultMetaClassUri !== undefined) {
            return Promise.resolve(
                new DmObjectWithSync(this.defaultMetaClassUri)
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
    submitMethod?: SubmitMethod): Promise<DmObject | void> {

    const foundModule = getModule(actionName);
    if (foundModule !== undefined) {
        return foundModule.execute(form, element, parameter, submitMethod);
    }

    alert("Unknown action type: " + actionName);
}