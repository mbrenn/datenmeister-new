import { DmObjectWithSync } from "./Mof.js";
/**
 * Defines the base implementation which can be overridden
 */
export class ItemFormActionModuleBase {
    constructor(actionName) {
        this.actionName = actionName;
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
            return Promise.resolve(new DmObjectWithSync(this.defaultMetaClassUri));
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
//# sourceMappingURL=FormActions.js.map