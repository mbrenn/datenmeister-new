define(["require", "exports", "./Mof", "./DataLoader", "./ApiConnection", "./Settings", "./Forms.DetailForm", "./Forms.ListForm"], function (require, exports, Mof, DataLoader, ApiConnection, Settings, DetailForm, Forms_ListForm_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getDefaultFormForItem = exports.FormCreator = exports.Form = void 0;
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
                    detailForm.createFormByObject(form, isReadOnly);
                    detailForm.onCancel = () => {
                        tthis.createViewForm(form, tthis.workspace, tthis.extentUri, tthis.itemId);
                    };
                    detailForm.onChange = (element) => {
                        DataLoader.storeObjectByUri(tthis.workspace, tthis.itemId, tthis.element).done(() => {
                            tthis.createViewForm(form, tthis.workspace, tthis.extentUri, tthis.itemId);
                        });
                    };
                }
                else if (tab.metaClass.id === "DatenMeister.Models.Forms.ListForm") {
                    const listForm = new Forms_ListForm_1.ListForm();
                    listForm.workspace = this.workspace;
                    listForm.extentUri = this.extentUri;
                    listForm.itemId = this.itemId;
                    listForm.formElement = tab;
                    listForm.elements = this.element.get(tab.get("property"));
                    listForm.createFormByCollection(form, true);
                }
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