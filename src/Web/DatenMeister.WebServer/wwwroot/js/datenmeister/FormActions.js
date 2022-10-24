var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Settings", "./ApiConnection", "./Navigator", "./Mof", "./client/Forms", "./client/Actions", "./models/DatenMeister.class", "./client/Actions.Items"], function (require, exports, Settings, ApiConnection, Navigator, Mof_1, FormClient, ActionClient, DatenMeisterModel, Actions_Items_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.FormActions = exports.execute = exports.requiresConfirmation = exports.loadFormForAction = exports.loadObjectForAction = exports.getModule = exports.addModule = exports.ItemFormActionModuleBase = void 0;
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
        loadForm() {
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
            if (actionName === 'Forms.Create.ByMetaClass') {
                return yield FormClient.getForm("dm:///_internal/forms/internal#Forms.Create.ByMetaClass");
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
        if (actionName === "ExtentsList.DeleteItem") {
            return true;
        }
        else {
            return false;
        }
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
            let workspaceId;
            let extentUri;
            let p = new URLSearchParams(window.location.search);
            switch (actionName) {
                case "Extent.CreateItemInProperty":
                    if (!p.has("itemUrl") || !p.has("workspace") || !p.has("property")) {
                        alert('There is no itemUrl given');
                    }
                    else {
                        const workspace = p.get('workspace');
                        const itemUrl = p.get('itemUrl');
                        const property = p.get('property');
                        const metaclass = p.get('metaclass');
                        yield FormActions.extentCreateItemInProperty(workspace, itemUrl, property, element, metaclass);
                    }
                    break;
                case "ExtentsList.ViewItem":
                    FormActions.itemNavigateTo(form.workspace, element.uri);
                    break;
                case "ExtentsList.DeleteItem":
                    yield FormActions.extentsListDeleteItem(form.workspace, form.extentUri, element.uri);
                    break;
                case "ExtentsList.MoveUpItem":
                    yield FormActions.extentsListMoveUpItem(form.workspace, form.extentUri, element.uri);
                    break;
                case "ExtentsList.MoveDownItem":
                    yield FormActions.extentsListMoveDownItem(form.workspace, form.extentUri, element.uri);
                    break;
                case "Forms.Create.ByMetaClass": {
                    const extentCreationParameter = new Mof_1.DmObject();
                    extentCreationParameter.set('configuration', element);
                    extentCreationParameter.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__CreateFormByMetaClass_Uri);
                    const result = yield ActionClient.executeActionDirectly("Execute", {
                        parameter: extentCreationParameter
                    });
                    if (result.success !== true) {
                        alert('Form was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
                    }
                    else {
                        alert('Form was created successfully');
                    }
                    break;
                }
                case "JSON.Item.Alert":
                    alert(JSON.stringify((0, Mof_1.createJsonFromObject)(element)));
                    break;
                case "Action.Execute":
                    // Executes the action directly
                    const result = yield ActionClient.executeAction(element.workspace, element.uri);
                    if (result.success) {
                        alert('Success');
                    }
                    else {
                        alert('Failure');
                    }
                    break;
                default:
                    alert("Unknown action type: " + actionName);
                    break;
            }
        });
    }
    exports.execute = execute;
    class FormActions {
        static workspaceNavigateTo(workspace) {
            document.location.href =
                Settings.baseUrl + "Item/Management/dm:%2F%2F%2F_internal%2Fworkspaces/" + encodeURIComponent(workspace);
        }
        static extentCreateItemInProperty(workspace, itemUrl, property, element, metaClass) {
            return __awaiter(this, void 0, void 0, function* () {
                const json = (0, Mof_1.createJsonFromObject)(element);
                yield ApiConnection.post(Settings.baseUrl + "api/items/create_child/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(itemUrl), {
                    metaClass: (metaClass === undefined || metaClass === null) ? "" : metaClass,
                    property: property,
                    asList: true,
                    properties: json
                });
                Navigator.navigateToItemByUrl(workspace, itemUrl);
            });
        }
        // Performs the navigation to the given item. The ItemUrl may be a uri or just the id
        static itemNavigateTo(workspace, itemUrl) {
            Navigator.navigateToItemByUrl(workspace, itemUrl);
        }
        static extentsListDeleteItem(workspace, extentUri, itemId) {
            return __awaiter(this, void 0, void 0, function* () {
                const data = yield ApiConnection.deleteRequest(Settings.baseUrl + "api/items/delete/"
                    + encodeURIComponent(workspace) + "/" +
                    encodeURIComponent(itemId), {});
                const success = data.success;
                if (success) {
                    document.location.reload();
                }
                else {
                    alert('Deletion was not successful.');
                }
            });
        }
        static extentsListMoveUpItem(workspace, extentUri, itemId) {
            return __awaiter(this, void 0, void 0, function* () {
                yield (0, Actions_Items_1.moveItemInExtentUp)(workspace, extentUri, itemId);
                document.location.reload();
            });
        }
        static extentsListMoveDownItem(workspace, extentUri, itemId) {
            return __awaiter(this, void 0, void 0, function* () {
                yield (0, Actions_Items_1.moveItemInExtentDown)(workspace, extentUri, itemId);
                document.location.reload();
            });
        }
    }
    exports.FormActions = FormActions;
});
//# sourceMappingURL=FormActions.js.map