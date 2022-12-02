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
});
//# sourceMappingURL=ItemModules.js.map