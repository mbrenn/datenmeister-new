var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
define(["require", "exports", "./Mof", "./DataLoader", "./ApiConnection", "./Settings", "./DetailForm"], function (require, exports, Mof, DataLoader, ApiConnection, Settings, DetailForm) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getDefaultFormForItem = exports.FormCreator = exports.Form = void 0;
    Mof = __importStar(Mof);
    DataLoader = __importStar(DataLoader);
    ApiConnection = __importStar(ApiConnection);
    Settings = __importStar(Settings);
    class Form {
    }
    exports.Form = Form;
    /*
        Defines the form creator which also performs the connect to the webserver itself.
        
        This method handles all allowed form types.
     */
    class FormCreator {
        createFormByObject(parent, isReadOnly) {
            const tthis = this;
            parent.empty();
            const tabs = this.formElement.get("tab");
            for (let n in tabs) {
                if (!tabs.hasOwnProperty(n)) {
                    continue;
                }
                let form = $("<div />");
                const tab = tabs[n];
                if (tab.metaClass.id === "DatenMeister.Models.Forms.DetailForm") {
                    const detailForm = new DetailForm.DetailForm();
                    detailForm.workspace = this.workspace;
                    detailForm.extentUri = this.extentUri;
                    detailForm.itemId = this.itemId;
                    detailForm.formElement = tab;
                    detailForm.element = this.element;
                    detailForm.createFormByObject(parent, isReadOnly);
                    detailForm.onCancel = () => {
                        tthis.createViewForm(parent, tthis.workspace, tthis.extentUri, tthis.itemId);
                    };
                    detailForm.onChange = (element) => {
                        DataLoader.storeObjectByUri(tthis.workspace, tthis.itemId, tthis.element).done(() => {
                            tthis.createViewForm(form, tthis.workspace, tthis.extentUri, tthis.itemId);
                        });
                    };
                }
                else if (tab.metaClass.id === "DatenMeister.Models.Forms.ListForm") {
                }
                // DetailForm
                else {
                    form = $("<div>Unknown Formtype:<span class='id'></span></div> ");
                    $(".id", form).text(tab.metaClass.id);
                }
                parent.append(form);
            }
        }
        createViewForm(parent, workspace, extentUri, uri) {
            this.createForm(parent, workspace, extentUri, uri, true);
        }
        createEditForm(parent, workspace, extentUri, uri) {
            this.createForm(parent, workspace, extentUri, uri, false);
        }
        createForm(parent, workspace, extentUri, itemId, isReadOnly) {
            const tthis = this;
            // Load the object
            const defer1 = DataLoader.loadObjectByUri(workspace, itemId);
            // Load the form
            const defer2 = getDefaultFormForItem(workspace, itemId, "");
            // Wait for both
            $.when(defer1, defer2).then(function (element, form) {
                tthis.element = element;
                tthis.formElement = form;
                tthis.workspace = workspace;
                tthis.itemId = itemId;
                tthis.createFormByObject(parent, isReadOnly);
            });
            parent.empty();
            parent.text("Loading content and form...");
        }
    }
    exports.FormCreator = FormCreator;
    /*
        Gets the default form for a certain item by the webserver
     */
    function getDefaultFormForItem(workspace, item, viewMode) {
        const r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl +
            "api/forms/default_for_item/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(item) +
            "/" +
            encodeURIComponent(viewMode)).done(x => {
            const dmObject = Mof.createObjectFromJson(x.item, x.metaClass);
            r.resolve(dmObject);
        });
        return r;
    }
    exports.getDefaultFormForItem = getDefaultFormForItem;
});
//# sourceMappingURL=Forms.js.map