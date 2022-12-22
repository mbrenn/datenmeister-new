var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../FormActions", "../Mof", "../client/Forms", "../client/Actions", "../Settings", "../models/DatenMeister.class", "../client/Items", "../Navigator", "../client/Actions.Items"], function (require, exports, FormActions, Mof_1, FormClient, ActionClient, Settings, DatenMeister_class_1, ItemClient, Navigator, Actions_Items_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadModules = void 0;
    var _MoveOrCopyAction = DatenMeister_class_1._DatenMeister._Actions._MoveOrCopyAction;
    function loadModules() {
        FormActions.addModule(new ItemMoveOrCopyActionNavigate());
        FormActions.addModule(new ItemMoveOrCopyAction());
        FormActions.addModule(new ItemDeleteAction());
        FormActions.addModule(new ItemMoveUpItemAction());
        FormActions.addModule(new ItemMoveDownItemAction());
        FormActions.addModule(new ItemXmiExportNavigate());
        FormActions.addModule(new ItemXmiExport());
        FormActions.addModule(new ItemXmiImportNavigate());
        FormActions.addModule(new ItemXmiImport());
    }
    exports.loadModules = loadModules;
    class ItemMoveOrCopyActionNavigate extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Item.MoveOrCopy.Navigate");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                document.location.href =
                    Settings.baseUrl + "ItemAction/Item.MoveOrCopy?workspaceId=" + encodeURIComponent(element.workspace)
                        + "&itemUri=" + encodeURIComponent(element.uri);
            });
        }
    }
    class ItemMoveOrCopyAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Item.MoveOrCopy");
            this.actionVerb = "Move/Copy Item";
        }
        loadObject() {
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                const result = new Mof_1.DmObject();
                result.setMetaClassByUri(DatenMeister_class_1._DatenMeister._Actions.__MoveOrCopyAction_Uri);
                // TODO: Set Result
                const sourceWorkspace = p.get('workspaceId');
                const sourceItemUri = p.get('itemUri');
                const source = Mof_1.DmObject.createFromReference(sourceWorkspace, sourceItemUri);
                result.set(_MoveOrCopyAction.source, source);
                return Promise.resolve(result);
            });
        }
        loadForm() {
            return __awaiter(this, void 0, void 0, function* () {
                return yield FormClient.getForm("dm:///_internal/forms/internal#Item.MoveOrCopy");
            });
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                // Executes the action directly
                const result = yield ActionClient.executeAction(element.workspace, element.uri);
                if (result.success) {
                    alert('Success');
                }
                else {
                    alert('Failure: \r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
                }
            });
        }
    }
    class ItemDeleteAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Item.Delete");
            this.requiresConfirmation = true;
            this.actionVerb = "Delete Item";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                const data = yield ItemClient.deleteItem(form.workspace, element.uri);
                const success = data.success;
                if (success) {
                    Navigator.navigateToWorkspace(form.workspace);
                }
                else {
                    alert('Deletion was not successful.');
                }
            });
        }
    }
    class ItemMoveDownItemAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Item.MoveDownItem");
            this.actionVerb = "Move Up";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                yield (0, Actions_Items_1.moveItemInCollectionDown)(form.workspace, form.itemUrl, form.formElement.get(DatenMeister_class_1._DatenMeister._Forms._TableForm.property), element.uri);
                document.location.reload();
            });
        }
    }
    class ItemMoveUpItemAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Item.MoveUpItem");
            this.actionVerb = "Move Down";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                yield (0, Actions_Items_1.moveItemInCollectionUp)(form.workspace, form.itemUrl, form.formElement.get(DatenMeister_class_1._DatenMeister._Forms._TableForm.property), element.uri);
                document.location.reload();
            });
        }
    }
    class ItemXmiExportNavigate extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Item.ExportXmi.Navigate");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                Navigator.navigateToAction("Item.ExportXmi", "dm:///_internal/forms/internal#DatenMeister.Export.Xmi", {
                    workspace: element.workspace,
                    itemUri: element.uri
                });
            });
        }
    }
    class ItemXmiExport extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Item.ExportXmi");
        }
        loadObject() {
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                if (!p.has("itemUri") || !p.has("workspace")) {
                    alert('There is no workspace and extentUri given');
                    throw 'There is no workspace and extentUri given';
                }
                else {
                    const workspace = p.get('workspace');
                    const itemUri = p.get('itemUri');
                    // Export the Xmi and stores it into the element
                    const exportedXmi = yield ItemClient.exportXmi(workspace, itemUri);
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
    class ItemXmiImportNavigate extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Item.ImportXmi.Navigate");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                Navigator.navigateToAction("Item.ImportXmi", "dm:///_internal/forms/internal#DatenMeister.Import.Item.Xmi", {
                    workspace: element.workspace,
                    itemUri: element.uri,
                    metaClass: DatenMeister_class_1._DatenMeister._CommonTypes._Default.__XmiImportContainer_Uri
                });
            });
        }
    }
    class ItemXmiImport extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Item.ImportXmi");
            this.actionVerb = "Perform Import";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                alert('Now, we do the import');
                let p = new URLSearchParams(window.location.search);
                if (!p.has("itemUri") || !p.has("workspace")) {
                    alert('There is no workspace and extentUri given');
                    throw 'There is no workspace and extentUri given';
                }
                else {
                    const workspace = p.get('workspace');
                    const itemUri = p.get('itemUri');
                    // Export the Xmi and stores it into the element
                    const importedXmi = yield ItemClient.importXmi(workspace, itemUri, element.get(DatenMeister_class_1._DatenMeister._CommonTypes._Default._XmiImportContainer.property, Mof_1.ObjectType.String), element.get(DatenMeister_class_1._DatenMeister._CommonTypes._Default._XmiImportContainer.addToCollection, Mof_1.ObjectType.Boolean), element.get(DatenMeister_class_1._DatenMeister._CommonTypes._Default._XmiImportContainer.xmi, Mof_1.ObjectType.String));
                    if (importedXmi.success) {
                        Navigator.navigateToItemByUrl(workspace, itemUri);
                    }
                    else {
                        alert('Something failed');
                    }
                }
            });
        }
    }
});
//# sourceMappingURL=ItemModules.js.map