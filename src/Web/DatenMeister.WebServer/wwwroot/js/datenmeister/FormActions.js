var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Settings", "./ApiConnection", "./Navigator", "./Mof", "./client/Items", "./client/Forms", "./client/Actions", "./models/DatenMeister.class", "./models/DatenMeister.class", "./client/Actions.Items"], function (require, exports, Settings, ApiConnection, Navigator, Mof_1, ItemClient, FormClient, ActionClient, DatenMeisterModel, DatenMeister_class_1, Actions_Items_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.FormActions = exports.execute = exports.requiresConfirmation = exports.loadFormForAction = exports.loadObjectForAction = exports.getModule = exports.addModule = exports.ItemFormActionModuleBase = void 0;
    var _MoveOrCopyAction = DatenMeister_class_1._DatenMeister._Actions._MoveOrCopyAction;
    /**
     * Defines the base implementation which can be overridden
     */
    class ItemFormActionModuleBase {
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
            // Now the default handling
            let p = new URLSearchParams(window.location.search);
            if (actionName === "Workspace.Extent.Xmi.Create") {
                const result = new Mof_1.DmObject();
                result.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");
                result.set("workspaceId", p.get('workspaceId'));
                return Promise.resolve(result);
            }
            if (actionName === "Item.MoveOrCopy") {
                const result = new Mof_1.DmObject();
                result.setMetaClassByUri(DatenMeister_class_1._DatenMeister._Actions.__MoveOrCopyAction_Uri);
                // TODO: Set Result
                const sourceWorkspace = p.get('workspaceId');
                const sourceItemUri = p.get('itemUri');
                const source = Mof_1.DmObject.createFromReference(sourceWorkspace, sourceItemUri);
                result.set(_MoveOrCopyAction.source, source);
                return Promise.resolve(result);
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
            if (actionName === 'Workspace.Extent.LoadOrCreate') {
                return yield FormClient.getForm("dm:///_internal/forms/internal#WorkspacesAndExtents.Extent.SelectType");
            }
            if (actionName === 'Forms.Create.ByMetaClass') {
                return yield FormClient.getForm("dm:///_internal/forms/internal#Forms.Create.ByMetaClass");
            }
            if (actionName === 'Item.MoveOrCopy') {
                return yield FormClient.getForm("dm:///_internal/forms/internal#Item.MoveOrCopy");
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
        if (actionName === "Item.Delete"
            || actionName === "ExtentsList.DeleteItem") {
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
        var _a, _b, _c;
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
                case "Item.Delete":
                    yield FormActions.itemDelete(form.workspace, form.extentUri, element.uri);
                    break;
                case "Item.MoveDownItem":
                    yield FormActions.itemMoveDownItem(form.workspace, form.itemUrl, form.formElement.get(DatenMeister_class_1._DatenMeister._Forms._TableForm.property), element.uri);
                    break;
                case "Item.MoveUpItem":
                    yield FormActions.itemMoveUpItem(form.workspace, form.itemUrl, form.formElement.get(DatenMeister_class_1._DatenMeister._Forms._TableForm.property), element.uri);
                    break;
                case "Workspace.Extent.Xmi.Create.Navigate": {
                    const workspaceIdParameter = (_a = parameter === null || parameter === void 0 ? void 0 : parameter.get('workspaceId')) !== null && _a !== void 0 ? _a : "";
                    yield FormActions.workspaceExtentCreateXmiNavigateTo(workspaceIdParameter);
                    break;
                }
                case "Workspace.Extent.LoadOrCreate.Navigate": {
                    const workspaceIdParameter = (_b = p === null || p === void 0 ? void 0 : p.get('workspaceId')) !== null && _b !== void 0 ? _b : "";
                    yield FormActions.workspaceExtentLoadAndCreateNavigateTo(workspaceIdParameter);
                    break;
                }
                case "Workspace.Extent.LoadOrCreate": {
                    const workspaceIdParameter = (_c = p === null || p === void 0 ? void 0 : p.get('workspaceId')) !== null && _c !== void 0 ? _c : "";
                    const extentType = yield ItemClient.getProperty("Data", element.uri, "extentType");
                    if (extentType === null || extentType === undefined) {
                        alert('No Extent Type has been selected');
                    }
                    else {
                        document.location.href = Settings.baseUrl +
                            "ItemAction/Workspace.Extent.LoadOrCreate.Step2" +
                            "?metaclass=" + encodeURIComponent(extentType.uri) +
                            (workspaceIdParameter !== undefined
                                ? ("&workspaceId=" + encodeURIComponent(workspaceIdParameter))
                                : "");
                    }
                    break;
                }
                case "Workspace.Extent.LoadOrCreate.Step2": {
                    const extentCreationParameter = new Mof_1.DmObject();
                    extentCreationParameter.set('configuration', element);
                    extentCreationParameter.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__LoadExtentAction_Uri);
                    const result = yield ActionClient.executeActionDirectly("Execute", {
                        parameter: extentCreationParameter
                    });
                    if (result.success !== true) {
                        alert('Extent was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
                    }
                    else {
                        alert('Extent was created successfully');
                    }
                    break;
                }
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
                case "Workspace.Extent.Xmi.Create":
                    {
                        const extentCreationParameter = new Mof_1.DmObject();
                        extentCreationParameter.set('configuration', element);
                        extentCreationParameter.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__LoadExtentAction_Uri);
                        const result = yield ActionClient.executeActionDirectly("Execute", {
                            parameter: extentCreationParameter
                        });
                        if (result.success) {
                            document.location.href = Settings.baseUrl
                                + "ItemsOverview/" + encodeURIComponent(element.get("workspaceId")) +
                                "/" + encodeURIComponent(element.get("extentUri"));
                        }
                        else {
                            alert(result.reason);
                        }
                    }
                    break;
                case "Item.MoveOrCopy.Navigate":
                    yield FormActions.itemMoveOrCopyNavigateTo(element.workspace, element.uri);
                    break;
                case "JSON.Item.Alert":
                    alert(JSON.stringify((0, Mof_1.createJsonFromObject)(element)));
                    break;
                case "Item.MoveOrCopy":
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
        static workspaceExtentCreateXmiNavigateTo(workspaceId) {
            document.location.href =
                Settings.baseUrl + "ItemAction/Workspace.Extent.Xmi.Create?metaClass=" +
                    encodeURIComponent(DatenMeister_class_1._DatenMeister._ExtentLoaderConfigs.__XmiStorageLoaderConfig_Uri) +
                    "&workspaceId=" + encodeURIComponent(workspaceId);
        }
        static workspaceExtentLoadAndCreateNavigateTo(workspaceId) {
            document.location.href =
                Settings.baseUrl + "ItemAction/Workspace.Extent.LoadOrCreate?workspaceId=" + encodeURIComponent(workspaceId);
        }
        static itemMoveOrCopyNavigateTo(workspaceId, itemUri) {
            document.location.href =
                Settings.baseUrl + "ItemAction/Item.MoveOrCopy?workspaceId=" + encodeURIComponent(workspaceId)
                    + "&itemUri=" + encodeURIComponent(itemUri);
        }
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
        static extentNavigateTo(workspace, extentUri) {
            document.location.href =
                Settings.baseUrl + "ItemsOverview/" +
                    encodeURIComponent(workspace) + "/" +
                    encodeURIComponent(extentUri);
        }
        // Performs the navigation to the given item. The ItemUrl may be a uri or just the id
        static itemNavigateTo(workspace, itemUrl) {
            Navigator.navigateToItemByUrl(workspace, itemUrl);
        }
        static itemNew(workspace, extentUri) {
            ApiConnection.post(Settings.baseUrl + "api/items/create", {
                workspace: workspace,
                extentUri: extentUri
            })
                .then(data => {
                document.location.reload();
            });
        }
        static itemDelete(workspace, extentUri, itemUri) {
            return __awaiter(this, void 0, void 0, function* () {
                const data = yield ItemClient.deleteItem(workspace, itemUri);
                const success = data.success;
                if (success) {
                    Navigator.navigateToWorkspace(workspace);
                }
                else {
                    alert('Deletion was not successful.');
                }
            });
        }
        static itemMoveUpItem(workspace, parentUri, property, itemUrl) {
            return __awaiter(this, void 0, void 0, function* () {
                yield (0, Actions_Items_1.moveItemInCollectionUp)(workspace, parentUri, property, itemUrl);
                document.location.reload();
            });
        }
        static itemMoveDownItem(workspace, parentUri, property, itemUrl) {
            return __awaiter(this, void 0, void 0, function* () {
                yield (0, Actions_Items_1.moveItemInCollectionDown)(workspace, parentUri, property, itemUrl);
                document.location.reload();
            });
        }
        static extentsListViewItem(workspace, extentUri, itemId) {
            document.location.href = Settings.baseUrl + "Item/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri) + "/" +
                encodeURIComponent(itemId);
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