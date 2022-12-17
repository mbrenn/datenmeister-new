var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../ApiConnection", "../client/Actions", "../client/Actions.Items", "../client/Extents", "../client/Forms", "../client/Items", "../FormActions", "../forms/RowForm", "../models/DatenMeister.class", "../Mof", "../Mof", "../Navigator", "../Settings"], function (require, exports, ApiConnection, Actions, Actions_Items_1, ECClient, ClientForms, ItemClient, FormActions, RowForm_1, DatenMeister_class_1, Mof, Mof_1, Navigator, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadModules = void 0;
    var _StoreExtentAction = DatenMeister_class_1._DatenMeister._Actions._StoreExtentAction;
    var _ObjectForm = DatenMeister_class_1._DatenMeister._Forms._ObjectForm;
    var _RowForm = DatenMeister_class_1._DatenMeister._Forms._RowForm;
    var _ActionFieldData = DatenMeister_class_1._DatenMeister._Forms._ActionFieldData;
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
        FormActions.addModule(new ExtentXmiExportNavigate());
        FormActions.addModule(new ExtentXmiExport());
        FormActions.addModule(new ExtentXmiImportNavigate());
        FormActions.addModule(new ExtentXmiImport());
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
        loadForm(metaClass) {
            return __awaiter(this, void 0, void 0, function* () {
                const form = yield ClientForms.getObjectFormForMetaClass(metaClass);
                const tabs = form.get(_ObjectForm.tab, Mof_1.ObjectType.Array);
                const firstTab = tabs[0];
                const fields = firstTab.get(_RowForm.field, Mof_1.ObjectType.Array);
                const parameter = new Mof_1.DmObject();
                parameter.set('name', 'CreateItemAndAnotherOne');
                // Adds the additional button 
                const actionButton = new Mof_1.DmObject(DatenMeister_class_1._DatenMeister._Forms.__ActionFieldData_Uri);
                actionButton.set(_ActionFieldData.title, "Create Item and another one");
                actionButton.set(_ActionFieldData.parameter, parameter);
                actionButton.set(_ActionFieldData.actionName, this.actionName);
                fields.push(actionButton);
                return form;
            });
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                if ((parameter === null || parameter === void 0 ? void 0 : parameter.get('name', Mof_1.ObjectType.String)) === 'CreateItemAndAnotherOne') {
                    submitMethod = RowForm_1.SubmitMethod.UserDefined1;
                }
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
                if (submitMethod === RowForm_1.SubmitMethod.UserDefined1) {
                    // Recreate a new item
                    Navigator.navigateToCreateNewItemInExtent(workspace, extentUri, metaClass);
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
    class ExtentXmiExportNavigate extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.ExportXmi.Navigate");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                Navigator.navigateToAction("Extent.ExportXmi", "dm:///_internal/forms/internal#DatenMeister.Export.Xmi", {
                    workspace: element.get(DatenMeister_class_1._DatenMeister._Management._Extent.workspaceId),
                    extentUri: element.get(DatenMeister_class_1._DatenMeister._Management._Extent.uri)
                });
            });
        }
    }
    class ExtentXmiExport extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.ExportXmi");
        }
        loadObject() {
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                if (!p.has("extentUri") || !p.has("workspace")) {
                    alert('There is no workspace and extentUri given');
                    throw 'There is no workspace and extentUri given';
                }
                else {
                    const workspace = p.get('workspace');
                    const extentUri = p.get('extentUri');
                    // Export the Xmi and stores it into the element
                    const exportedXmi = yield ECClient.exportXmi(workspace, extentUri);
                    const result = new Mof_1.DmObject(DatenMeister_class_1._DatenMeister._CommonTypes._Default.__XmiExportContainer_Uri);
                    result.set(DatenMeister_class_1._DatenMeister._CommonTypes._Default._XmiExportContainer.xmi, exportedXmi.xmi);
                    return result;
                }
            });
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                alert('Nothing to do');
            });
        }
    }
    class ExtentXmiImportNavigate extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.ImportXmi.Navigate");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                Navigator.navigateToAction("Extent.ImportXmi", "dm:///_internal/forms/internal#DatenMeister.Import.Xmi", {
                    workspaceId: element.get(DatenMeister_class_1._DatenMeister._Management._Extent.workspaceId),
                    extentUri: element.get(DatenMeister_class_1._DatenMeister._Management._Extent.uri)
                });
            });
        }
    }
    class ExtentXmiImport extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Extent.ImportXmi");
            this.actionVerb = "Perform Import";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                alert('Now, we do the import');
                let p = new URLSearchParams(window.location.search);
                if (!p.has("extentUri") || !p.has("workspace")) {
                    alert('There is no workspace and extentUri given');
                    throw 'There is no workspace and extentUri given';
                }
                else {
                    const workspace = p.get('workspace');
                    const extentUri = p.get('extentUri');
                    // Export the Xmi and stores it into the element
                    const exportedXmi = yield ECClient.importXmi(workspace, extentUri, element.get(DatenMeister_class_1._DatenMeister._CommonTypes._Default._XmiExportContainer.xmi, Mof_1.ObjectType.String));
                }
            });
        }
    }
});
//# sourceMappingURL=ExtentModules.js.map