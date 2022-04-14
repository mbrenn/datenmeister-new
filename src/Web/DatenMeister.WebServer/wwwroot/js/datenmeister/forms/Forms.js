define(["require", "exports", "../Mof", "../client/Items", "../client/Forms", "./DetailForm", "./ListForm", "../DomHelper"], function (require, exports, Mof, DataLoader, ClientForms, DetailForm, ListForm_1, DomHelper_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.DetailFormCreator = exports.CollectionFormCreator = exports.FormModel = void 0;
    var DmObject = Mof.DmObject;
    var FormModel;
    (function (FormModel) {
        function createEmptyFormWithDetail() {
            const form = new Mof.DmObject();
            const detailForm = new Mof.DmObject();
            detailForm.metaClass =
                {
                    id: "DatenMeister.Models.Forms.DetailForm"
                };
            form.set('tab', [detailForm]);
            return form;
        }
        FormModel.createEmptyFormWithDetail = createEmptyFormWithDetail;
    })(FormModel = exports.FormModel || (exports.FormModel = {}));
    /*
        Creates a form containing a collection of items.
        The input for this type is a collection of elements
    */
    class CollectionFormCreator {
        createListForRootElements(parent, workspace, extentUri, configuration) {
            if (configuration.isReadOnly === undefined) {
                configuration.isReadOnly = true;
            }
            const tthis = this;
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    tthis.createListForRootElements(parent, workspace, extentUri, configuration);
                };
            }
            // Load the object
            const defer1 = DataLoader.getRootElements(workspace, extentUri);
            // Load the form
            const defer2 = ClientForms.getDefaultFormForExtent(workspace, extentUri, "");
            // Wait for both
            Promise.all([defer1, defer2]).then(([elements, form]) => {
                tthis.formElement = form;
                tthis.workspace = workspace;
                tthis.extentUri = extentUri;
                (0, DomHelper_1.debugElementToDom)(elements, "#debug_mofelement");
                (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
                tthis.createFormByCollection(parent, elements, configuration);
            });
            parent.empty();
            parent.text("Loading content and form...");
        }
        createFormByCollection(parent, elements, configuration) {
            if (configuration.isReadOnly === undefined) {
                configuration.isReadOnly = true;
            }
            const tthis = this;
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    tthis.createFormByCollection(parent, elements, configuration);
                };
            }
            parent.empty();
            const creatingElements = $("<div>Creating elements...</div>");
            parent.append(creatingElements);
            const tabs = this.formElement.get("tab");
            let tabCount = Array.isArray(tabs) ? tabs.length : 0;
            for (let n in tabs) {
                if (!tabs.hasOwnProperty(n)) {
                    continue;
                }
                // Do it asynchronously. 
                window.setTimeout(() => {
                    let form = $("<div />");
                    const tab = tabs[n];
                    if (tab.metaClass.id === "DatenMeister.Models.Forms.ListForm") {
                        const listForm = new ListForm_1.ListForm();
                        listForm.elements = elements;
                        listForm.formElement = tab;
                        listForm.workspace = this.workspace;
                        listForm.extentUri = this.extentUri;
                        listForm.createFormByCollection(form, configuration);
                    }
                    parent.append(form);
                    tabCount--;
                    if (tabCount === 0) {
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
        createFormByObject(parent, configuration) {
            const tthis = this;
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    tthis.createFormByObject(parent, configuration);
                };
            }
            if (this.element == null)
                this.element = new DmObject();
            parent.empty();
            const creatingElements = $("<div>Creating elements...</div>");
            parent.append(creatingElements);
            const tabs = this.formElement.getAsArray("tab");
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
                    detailForm.createFormByObject(form, configuration);
                    if (configuration.onCancel !== undefined) {
                        detailForm.onCancel = configuration.onCancel;
                    }
                    if (configuration.onSubmit !== undefined) {
                        detailForm.onChange = configuration.onSubmit;
                    }
                }
                else if (tab.metaClass.id === "DatenMeister.Models.Forms.ListForm") {
                    const listForm = new ListForm_1.ListForm();
                    listForm.workspace = this.workspace;
                    listForm.extentUri = this.extentUri;
                    listForm.itemId = this.itemId;
                    listForm.formElement = tab;
                    listForm.elements = this.element.get(tab.get("property"));
                    listForm.createFormByCollection(form, { isReadOnly: true });
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
        createViewForm(parent, workspace, uri) {
            this.createForm(parent, workspace, uri, { isReadOnly: true });
        }
        createEditForm(parent, workspace, uri) {
            const tthis = this;
            this.createForm(parent, workspace, uri, {
                isReadOnly: false,
                onCancel: () => {
                    tthis.createViewForm(parent, tthis.workspace, tthis.itemId);
                },
                onSubmit: (element) => {
                    DataLoader.setProperties(tthis.workspace, tthis.itemId, element).then(() => {
                        tthis.createViewForm(parent, tthis.workspace, tthis.itemId);
                    });
                }
            });
        }
        createForm(parent, workspace, itemUrl, configuration) {
            const tthis = this;
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    tthis.createForm(parent, workspace, itemUrl, configuration);
                };
            }
            // Load the object
            const defer1 = DataLoader.getObjectByUri(workspace, itemUrl);
            // Load the form
            const defer2 = ClientForms.getDefaultFormForItem(workspace, itemUrl, "");
            // Wait for both
            Promise.all([defer1, defer2]).then(([element1, form]) => {
                tthis.element = element1;
                tthis.formElement = form;
                tthis.workspace = workspace;
                tthis.itemId = itemUrl;
                (0, DomHelper_1.debugElementToDom)(element1, "#debug_mofelement");
                (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
                tthis.createFormByObject(parent, configuration);
            });
            parent.empty();
            parent.text("Loading content and form...");
        }
    }
    exports.DetailFormCreator = DetailFormCreator;
});
//# sourceMappingURL=Forms.js.map