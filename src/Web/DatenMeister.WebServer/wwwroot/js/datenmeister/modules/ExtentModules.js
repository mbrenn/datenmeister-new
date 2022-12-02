var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../FormActions", "../Mof", "../forms/RowForm", "../ApiConnection", "../Settings", "../client/Extents", "../client/Items", "../Navigator", "../client/Actions.Items", "../Mof", "../client/Actions", "../models/DatenMeister.class"], function (require, exports, FormActions, Mof_1, RowForm_1, ApiConnection, Settings, ECClient, ItemClient, Navigator, Actions_Items_1, Mof, Actions, DatenMeister_class_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadModules = void 0;
    var _StoreExtentAction = DatenMeister_class_1._DatenMeister._Actions._StoreExtentAction;
    function loadModules() {
        FormActions.addModule(new ExtentPropertiesUpdateAction());
        FormActions.addModule(new ExtentCreateItemAction());
        FormActions.addModule(new ExtentDeleteAction());
        FormActions.addModule(new ExtentNavigateToAction());
        FormActions.addModule(new ExtentPropertiesAction());
        FormActions.addModule(new ExtentCreateItemInPropertyAction());
        FormActions.addModule(new ExtentsListViewItemAction());
        FormActions.addModule(new ExtentsListDeleteItemAction());
        FormActions.addModule(new ExtentsListMoveUpItemAction());
        FormActions.addModule(new ExtentsListMoveDownItemAction());
        FormActions.addModule(new ExtentsStoreAction());
    }
    exports.loadModules = loadModules;
    class ExtentPropertiesUpdateAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.Properties.Update");
        }
        loadObject() {
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                const workspace = p.get('workspace');
                const extentUri = p.get('extent');
                return yield ECClient.getProperties(workspace, extentUri);
            });
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                if (!p.has("extent") || !p.has("workspace")) {
                    alert('There is no extent given');
                }
                else {
                    const workspace = p.get('workspace');
                    const extentUri = p.get('extent');
                    yield ECClient.setProperties(workspace, extentUri, element);
                    Navigator.navigateToExtent(workspace, extentUri);
                }
            });
        }
    }
    class ExtentCreateItemAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.CreateItem");
            this.actionVerb = "Create Item";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                if (!p.has("extent") || !p.has("workspace")) {
                    alert('There is no extent given');
                }
                else {
                    const workspace = p.get('workspace');
                    const extentUri = p.get('extent');
                    yield this.extentCreateItem(workspace, extentUri, element, undefined, submitMethod);
                }
            });
        }
        extentCreateItem(workspace, extentUri, element, metaClass, submitMethod) {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
                if (metaClass === undefined) {
                    metaClass = (_a = element.metaClass) === null || _a === void 0 ? void 0 : _a.uri;
                }
                const json = (0, Mof_1.createJsonFromObject)(element);
                const newItem = yield ItemClient.createItemInExtent(workspace, extentUri, {
                    metaClass: metaClass === undefined ? "" : metaClass,
                    properties: element
                });
                if (submitMethod === RowForm_1.SubmitMethod.Save) {
                    // If user has clicked on the save button (without closing), the form shall just be updated
                    Navigator.navigateToItemByUrl(workspace, newItem.itemId);
                }
                else {
                    // Else, move to the overall items overview
                    document.location.href = Settings.baseUrl +
                        "ItemsOverview/" +
                        encodeURIComponent(workspace) +
                        "/" +
                        encodeURIComponent(extentUri);
                }
            });
        }
    }
    class ExtentCreateItemInPropertyAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.CreateItemInProperty");
            this.actionVerb = "Create Item";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                if (!p.has("itemUrl") || !p.has("workspace") || !p.has("property")) {
                    alert('There is no itemUrl given');
                }
                else {
                    const workspace = p.get('workspace');
                    const itemUrl = p.get('itemUrl');
                    const property = p.get('property');
                    const metaclass = p.get('metaclass');
                    const json = (0, Mof_1.createJsonFromObject)(element);
                    yield ApiConnection.post(Settings.baseUrl + "api/items/create_child/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(itemUrl), {
                        metaClass: (metaclass === undefined || metaclass === null) ? "" : metaclass,
                        property: property,
                        asList: true,
                        properties: json
                    });
                    Navigator.navigateToItemByUrl(workspace, itemUrl);
                }
            });
        }
    }
    class ExtentDeleteAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.DeleteExtent");
            this.actionVerb = "Delete Extent";
            this.requiresConfirmation = true;
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                let extentUri = element.get('uri');
                let workspaceId = element.get('workspaceId');
                const deleteParameter = {
                    workspace: workspaceId,
                    extentUri: extentUri
                };
                yield ApiConnection.deleteRequest(Settings.baseUrl + "api/extent/delete", deleteParameter);
                Navigator.navigateToWorkspace(workspaceId);
            });
        }
    }
    class ExtentNavigateToAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.NavigateTo");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                let extentUri = element.get('uri');
                let workspaceId = element.get('workspaceId');
                Navigator.navigateToExtent(workspaceId, extentUri);
            });
        }
    }
    class ExtentPropertiesAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.Properties");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                let extentUri = element.get('uri');
                let workspaceId = element.get('workspaceId');
                document.location.href =
                    Settings.baseUrl +
                        "ItemAction/Extent.Properties.Navigate/" +
                        encodeURIComponent("dm:///_internal/forms/internal#DatenMeister.Extent.Properties") +
                        "?workspace=" +
                        encodeURIComponent(workspaceId) +
                        "&extent=" +
                        encodeURIComponent(extentUri);
            });
        }
    }
    class ExtentsListViewItemAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("ExtentsList.ViewItem");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                Navigator.navigateToItemByUrl(form.workspace, element.uri);
            });
        }
    }
    class ExtentsListDeleteItemAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("ExtentsList.DeleteItem");
            this.actionVerb = "Delete Item";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                const data = yield ApiConnection.deleteRequest(Settings.baseUrl + "api/items/delete/"
                    + encodeURIComponent(form.workspace) + "/" +
                    encodeURIComponent(element.uri), {});
                const success = data.success;
                if (success) {
                    document.location.reload();
                }
                else {
                    alert('Deletion was not successful.');
                }
            });
        }
    }
    class ExtentsListMoveUpItemAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("ExtentsList.MoveUpItem");
            this.actionVerb = "Move Up";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                yield (0, Actions_Items_1.moveItemInExtentUp)(form.workspace, element.extentUri, element.uri);
                document.location.reload();
            });
        }
    }
    class ExtentsListMoveDownItemAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("ExtentsList.MoveDownItem");
            this.actionVerb = "Move Down";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                yield (0, Actions_Items_1.moveItemInExtentDown)(form.workspace, element.extentUri, element.uri);
                document.location.reload();
            });
        }
    }
    class ExtentsStoreAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.Store");
            this.actionVerb = "Store Extent";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                const action = new Mof.DmObject(DatenMeister_class_1._DatenMeister._Actions.__StoreExtentAction_Uri);
                action.set(_StoreExtentAction.workspaceId, element.get(DatenMeister_class_1._DatenMeister._Management._Extent.workspaceId));
                action.set(_StoreExtentAction.extentUri, element.get(DatenMeister_class_1._DatenMeister._Management._Extent.uri));
                const actionParams = {
                    parameter: action
                };
                yield Actions.executeActionDirectly("Execute", actionParams);
                alert('Extent has been stored.');
            });
        }
    }
});
//# sourceMappingURL=ExtentModules.js.map