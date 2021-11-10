define(["require", "exports", "./Mof", "./DataLoader", "./ApiConnection", "./Settings", "./Forms.DetailForm", "./Forms.ListForm"], function (require, exports, Mof, DataLoader, ApiConnection, Settings, DetailForm, Forms_ListForm_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getDefaultFormForExtent = exports.getDefaultFormForItem = exports.DetailFormCreator = exports.CollectionFormCreator = exports.Form = void 0;
    class Form {
    }
    exports.Form = Form;
    /*
        Creates a form containing a collection of items.
        The input for this type is a collection of elements
    */
    class CollectionFormCreator {
        createListForRootElements(parent, workspace, extentUri, isReadOnly) {
            const tthis = this;
            // Load the object
            const defer1 = DataLoader.loadRootElementsFromExtent(workspace, extentUri);
            // Load the form
            const defer2 = getDefaultFormForExtent(workspace, extentUri, "");
            // Wait for both
            $.when(defer1, defer2).then((elements, form) => {
                tthis.formElement = form;
                tthis.workspace = workspace;
                tthis.extentUri = extentUri;
                tthis.createFormByCollection(parent, elements, isReadOnly);
            });
            parent.empty();
            parent.text("Loading content and form...");
        }
        createFormByCollection(parent, elements, isReadOnly) {
            const tthis = this;
            parent.empty();
            const creatingElements = $("<div>Creating elements...</div>");
            parent.append(creatingElements);
            const tabs = this.formElement.get("tab");
            let tabCount = tabs.length;
            for (let n in tabs) {
                if (!tabs.hasOwnProperty(n)) {
                    continue;
                }
                // Do it asynchronously. 
                window.setTimeout(() => {
                    let form = $("<div />");
                    const tab = tabs[n];
                    if (tab.metaClass.id === "DatenMeister.Models.Forms.ListForm") {
                        const listForm = new Forms_ListForm_1.ListForm();
                        listForm.elements = elements;
                        listForm.formElement = tab;
                        listForm.workspace = this.workspace;
                        listForm.extentUri = this.extentUri;
                        listForm.createFormByCollection(form, isReadOnly);
                    }
                    parent.append(form);
                    tabCount--;
                    if (tabCount == 0) {
                        // Removes the loading information
                        creatingElements.remove();
                    }
                });
            }
        }
    }
    exports.CollectionFormCreator = CollectionFormCreator;
    /*
        Defines the form creator which also performs the connect to the webserver itself.
        The input for this type of form is a single element
        
        This method handles all allowed form types.
     */
    class DetailFormCreator {
        createFormByObject(parent, isReadOnly) {
            const tthis = this;
            parent.empty();
            const creatingElements = $("<div>Creating elements...</div>");
            parent.append(creatingElements);
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
            // Removes the loading information
            creatingElements.remove();
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
                tthis.extentUri = extentUri;
                tthis.itemId = itemId;
                tthis.createFormByObject(parent, isReadOnly);
            });
            parent.empty();
            parent.text("Loading content and form...");
        }
    }
    exports.DetailFormCreator = DetailFormCreator;
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
            const dmObject = Mof.convertJsonObjectToDmObject(x);
            r.resolve(dmObject);
        });
        return r;
    }
    exports.getDefaultFormForItem = getDefaultFormForItem;
    /*
        Gets the default form for an extent uri by the webserver
     */
    function getDefaultFormForExtent(workspace, extentUri, viewMode) {
        const r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl +
            "api/forms/default_for_extent/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(extentUri) +
            "/" +
            encodeURIComponent(viewMode)).done(x => {
            const dmObject = Mof.convertJsonObjectToDmObject(x);
            r.resolve(dmObject);
        });
        return r;
    }
    exports.getDefaultFormForExtent = getDefaultFormForExtent;
});
//# sourceMappingURL=Forms.js.map