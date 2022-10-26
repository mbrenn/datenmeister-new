﻿import {createJsonFromObject, DmObject} from "./Mof";
import * as IIForms from "./forms/Interfaces";
import * as ActionClient from "./client/Actions";
import {SubmitMethod} from "./forms/RowForm";

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


/*
    Supporting methods
 */

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
    
    return Promise.resolve(undefined);
}

export function requiresConfirmation(actionName: string): boolean {

    const foundModule = getModule(actionName);
    if (foundModule !== undefined) {
        return foundModule.requiresConfirmation === true;
    }
    
    return false;
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

    alert("Unknown action type: " + actionName);
}