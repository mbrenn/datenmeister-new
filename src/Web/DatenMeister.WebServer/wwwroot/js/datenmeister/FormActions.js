var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.execute = exports.requiresConfirmation = exports.loadFormForAction = exports.loadObjectForAction = exports.getModule = exports.addModule = exports.ItemFormActionModuleBase = void 0;
    /**
     * Defines the base implementation which can be overridden
     */
    class ItemFormActionModuleBase {
        constructor(actionName) {
            this.actionName = actionName;
        }
        execute(form, element, parameter, submitMethod) {
            return Promise.resolve(undefined);
        }
        loadForm(metaClass) {
            return Promise.resolve(undefined);
        }
        loadObject() {
            return Promise.resolve(undefined);
        }
    }
    exports.ItemFormActionModuleBase = ItemFormActionModuleBase;
    let modules = new Array();
    function addModule(module) {
        // Checks, if there is already a module register. If yes, throw an exception
        if (getModule(module.actionName) !== undefined) {
            throw "A module with action name " + module.actionName + " is already registered";
        }
        // Adds the module
        modules.push(module);
    }
    exports.addModule = addModule;
    function getModule(actionName) {
        for (let n in modules) {
            const module = modules[n];
            if (module.actionName === actionName) {
                return module;
            }
        }
        return undefined;
    }
    exports.getModule = getModule;
    /*
        Supporting methods
     */
    function loadObjectForAction(actionName) {
        return __awaiter(this, void 0, void 0, function* () {
            const foundModule = getModule(actionName);
            if (foundModule !== undefined) {
                return foundModule.loadObject();
            }
            /* Nothing has been found, so return an undefined */
            return Promise.resolve(undefined);
        });
    }
    exports.loadObjectForAction = loadObjectForAction;
    /* Finds the best form fitting for the action */
    function loadFormForAction(actionName) {
        return __awaiter(this, void 0, void 0, function* () {
            const foundModule = getModule(actionName);
            if (foundModule !== undefined) {
                return foundModule.loadForm();
            }
            return Promise.resolve(undefined);
        });
    }
    exports.loadFormForAction = loadFormForAction;
    function requiresConfirmation(actionName) {
        const foundModule = getModule(actionName);
        if (foundModule !== undefined) {
            return foundModule.requiresConfirmation === true;
        }
        return false;
    }
    exports.requiresConfirmation = requiresConfirmation;
    // Calls to execute the form actions.
    // actionName: Name of the action to be executed. This is a simple string describing the action
    // form: The form which was used to trigger the action
    // itemUrl: The url of the item whose action will be executed
    // element: The element which is reflected within the form
    // parameter: These parameter are retrieved from the actionForm definition from the server and are forwarded
    //    This supports the server to provide additional parameter for an action button
    // submitMethod: Describes which button the user has clicked
    function execute(actionName, form, element, parameter, submitMethod) {
        return __awaiter(this, void 0, void 0, function* () {
            const foundModule = getModule(actionName);
            if (foundModule !== undefined) {
                return foundModule.execute(form, element, parameter, submitMethod);
            }
            alert("Unknown action type: " + actionName);
        });
    }
    exports.execute = execute;
});
//# sourceMappingURL=FormActions.js.map