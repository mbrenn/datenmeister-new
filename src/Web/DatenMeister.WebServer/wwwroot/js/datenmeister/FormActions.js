import * as Mof from "./Mof.js";
import { SubmitMethod } from "./forms/Forms.js";
import { _DatenMeister } from "./models/DatenMeister.class.js";
import * as ActionClient from "./client/Actions.js";
export function getActionHeading(module) {
    if (module.actionHeading !== undefined) {
        return module.actionHeading;
    }
    return module.actionVerb;
}
/**
 * Defines the base implementation which can be overridden
 */
export class ItemFormActionModuleBase {
    constructor(actionName, actionMetaClassUri) {
        this.actionName = actionName;
        this.actionMetaClassUri = actionMetaClassUri;
    }
    execute(form, element, parameter, submitMethod) {
        return Promise.resolve(undefined);
    }
    loadForm(metaClass) {
        return Promise.resolve(undefined);
    }
    preparePage(element, form) {
        return Promise.resolve(undefined);
    }
    loadObject() {
        if (this.defaultMetaClassUri !== undefined) {
            return Promise.resolve(new Mof.DmObjectWithSync(this.defaultMetaClassUri));
        }
        return Promise.resolve(undefined);
    }
}
let modules = new Array();
export function addModule(module) {
    // Checks, if there is already a module register. If yes, throw an exception
    if (getModule(module.actionName) !== undefined) {
        throw "A module with action name " + module.actionName + " is already registered";
    }
    // Adds the module
    modules.push(module);
}
export function getModule(actionName) {
    for (let n in modules) {
        const module = modules[n];
        if (module.actionName === actionName) {
            return module;
        }
    }
    return undefined;
}
export function getModuleByUri(actionMetaClassUri) {
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
export async function execute(actionName, form, element, parameter, submitMethod) {
    const foundModule = getModule(actionName);
    if (foundModule !== undefined) {
        return foundModule.execute(form, element, parameter, submitMethod);
    }
    alert("Unknown action type: " + actionName);
}
export async function executeClientAction(clientAction, form, parameter, submitMethod) {
    submitMethod ?? (submitMethod = SubmitMethod.Save);
    const moduleName = clientAction.get(_DatenMeister._Actions._ClientActions._ClientAction.actionName);
    let module = getModule(moduleName);
    if (module === undefined) {
        module = getModuleByUri(clientAction.metaClass?.uri);
    }
    if (module === undefined) {
        alert('Unknown action: ' + moduleName + clientAction.metaClass?.uri);
    }
    else {
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
export async function executeAction(action, form, element, parameter, submitMethod) {
    var module = getModuleByUri(action.metaClass?.uri);
    if (module === undefined) {
        alert("Unknown action: " + action.metaClass?.uri);
        return;
    }
    const result = await module.execute(form, element, parameter, submitMethod);
    if (result !== undefined) {
        // Checks, if we are having a client-actions responded back from the server
        const resultAsMof = result;
        const clientActions = resultAsMof.get(_DatenMeister._Actions._ActionResult.clientActions, Mof.ObjectType.Array);
        if (clientActions !== undefined) {
            for (let n in clientActions) {
                // Try to find the module and execute the client action
                const clientAction = clientActions[n];
                executeClientAction(clientAction, form);
            }
        }
    }
    return result;
}
export async function executeActionOnServer(action, form) {
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
                    const clientAction = clientActions[n];
                    executeClientAction(clientAction, form);
                }
            }
        }
    }
    return result;
}
//# sourceMappingURL=FormActions.js.map