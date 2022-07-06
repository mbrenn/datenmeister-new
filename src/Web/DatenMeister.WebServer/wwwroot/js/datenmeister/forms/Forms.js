var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Mof", "../client/Items", "../client/Forms", "./DetailForm", "./DetailForm", "./ListForm", "../DomHelper", "../Navigator", "./ViewModeSelectionForm", "./ViewModeLogic"], function (require, exports, Mof, DataLoader, ClientForms, DetailForm, DetailForm_1, ListForm_1, DomHelper_1, Navigator_1, ViewModeSelectionForm_1, VML) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ItemDetailFormCreator = exports.DetailFormCreator = exports.DetailFormHtmlElements = exports.FormMode = exports.CollectionFormCreator = exports.CollectionFormHtmlElements = exports.FormModel = void 0;
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
        Defines the html fields which will be used for layouting.
     */
    class CollectionFormHtmlElements {
    }
    exports.CollectionFormHtmlElements = CollectionFormHtmlElements;
    /*
        Creates a form containing a collection of items.
        The input for this type is a collection of elements
    */
    class CollectionFormCreator {
        createListForRootElements(htmlElements, workspace, extentUri, configuration) {
            var _a;
            if (htmlElements.itemContainer === undefined || htmlElements.itemContainer === null) {
                throw "htmlElements.itemContainer is not set";
            }
            if (configuration.isReadOnly === undefined) {
                configuration.isReadOnly = true;
            }
            if (configuration.viewMode === undefined || configuration.viewMode === null) {
                configuration.viewMode = VML.getCurrentViewMode();
            }
            const tthis = this;
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    tthis.createListForRootElements(htmlElements, workspace, extentUri, configuration);
                };
            }
            // Load the object
            const defer1 = DataLoader.getRootElements(workspace, extentUri);
            // Load the form
            const defer2 = ClientForms.getDefaultFormForExtent(workspace, extentUri, configuration.viewMode);
            // Wait for both
            Promise.all([defer1, defer2]).then(([elements, form]) => {
                tthis.formElement = form;
                tthis.workspace = workspace;
                tthis.extentUri = extentUri;
                (0, DomHelper_1.debugElementToDom)(elements, "#debug_mofelement");
                (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
                tthis.createFormByCollection(htmlElements, elements, configuration);
            });
            (_a = htmlElements.viewModeSelectorContainer) === null || _a === void 0 ? void 0 : _a.empty();
            if (htmlElements.viewModeSelectorContainer !== undefined && htmlElements.viewModeSelectorContainer !== null) {
                const viewModeForm = new ViewModeSelectionForm_1.ViewModeSelectionForm();
                const htmlViewModeForm = viewModeForm.createForm();
                viewModeForm.viewModeSelected.addListener(_ => configuration.refreshForm());
                htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
            }
            htmlElements.itemContainer.empty()
                .text("Loading content and form...");
        }
        createFormByCollection(htmlElements, elements, configuration) {
            const itemContainer = htmlElements.itemContainer;
            if (configuration.isReadOnly === undefined) {
                configuration.isReadOnly = true;
            }
            const tthis = this;
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    tthis.createFormByCollection(htmlElements, elements, configuration);
                };
            }
            itemContainer.empty();
            const creatingElements = $("<div>Creating elements...</div>");
            itemContainer.append(creatingElements);
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
                    itemContainer.append(form);
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
    // Defines the possible viewmode of a form
    var FormMode;
    (function (FormMode) {
        // The user can not edit the fields and just views the information
        FormMode[FormMode["ViewMode"] = 0] = "ViewMode";
        // The user can edit the fields and submit these changes
        FormMode[FormMode["EditMode"] = 1] = "EditMode";
    })(FormMode = exports.FormMode || (exports.FormMode = {}));
    ;
    /*
        Defines the html fields which will be used for layouting.
     */
    class DetailFormHtmlElements {
    }
    exports.DetailFormHtmlElements = DetailFormHtmlElements;
    /*
        Defines the form creator which also performs the connect to the webserver itself.
        The input for this type of form is a single element
        
        This method handles all allowed form types.
     */
    class DetailFormCreator {
        createFormByObject(htmlElements, configuration) {
            // First, store the parent and the configuration
            this.domContainer = htmlElements.itemContainer;
            this.htmlItemContainer = configuration;
            this.createFormForItem();
        }
        createFormForItem() {
            const configuration = this.htmlItemContainer;
            const tthis = this;
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    tthis.createFormForItem();
                };
            }
            if (this.element == null)
                this.element = new DmObject();
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
                this.domContainer.append(form);
            }
        }
    }
    exports.DetailFormCreator = DetailFormCreator;
    class ItemDetailFormCreator {
        constructor() {
            this.formMode = FormMode.ViewMode;
        }
        switchToMode(formMode) {
            this.formMode = formMode;
            this.rebuildForm();
        }
        createForm(htmlElements, workspace, itemUri) {
            this.htmlElements = htmlElements;
            this.workspace = workspace;
            this.itemUri = itemUri;
            this.rebuildForm();
        }
        rebuildForm() {
            const tthis = this;
            let configuration;
            if (this.formMode === FormMode.ViewMode) {
                configuration = { isReadOnly: true };
            }
            else {
                configuration = {
                    isReadOnly: false,
                    onCancel: () => {
                        tthis.switchToMode(FormMode.ViewMode);
                    },
                    onSubmit: (element, method) => __awaiter(this, void 0, void 0, function* () {
                        yield DataLoader.setProperties(tthis.workspace, tthis.itemUri, element);
                        if (method === DetailForm_1.SubmitMethod.Save) {
                            tthis.switchToMode(FormMode.ViewMode);
                        }
                        if (method === DetailForm_1.SubmitMethod.SaveAndClose) {
                            const containers = yield DataLoader.getContainer(tthis.workspace, tthis.itemUri);
                            if (containers !== undefined && containers.length > 0) {
                                const parentWorkspace = containers[0].workspace;
                                if (containers.length === 2) {
                                    // If user has selected would move to an extent, he should move to the items enumeration
                                    (0, Navigator_1.navigateToExtent)(parentWorkspace, containers[0].uri);
                                }
                                else {
                                    (0, Navigator_1.navigateToItemByUrl)(parentWorkspace, containers[0].uri);
                                }
                            }
                            else {
                                alert('Something wrong happened. I cannot retrieve the parent...');
                            }
                        }
                    })
                };
            }
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    tthis.rebuildForm();
                };
            }
            // Defines the viewmode, if not already defined by the caller
            if (configuration.viewMode === undefined || configuration.viewMode === null) {
                configuration.viewMode = VML.getCurrentViewMode();
            }
            // Load the object
            const defer1 = DataLoader.getObjectByUri(this.workspace, this.itemUri);
            // Load the form
            const defer2 = ClientForms.getDefaultFormForItem(this.workspace, this.itemUri, configuration.viewMode);
            // Wait for both
            Promise.all([defer1, defer2]).then(([element1, form]) => {
                this.htmlElements.itemContainer.empty();
                const detailForm = new DetailFormCreator();
                detailForm.workspace = this.workspace;
                detailForm.itemId = this.itemUri;
                detailForm.element = element1;
                detailForm.formElement = form;
                if (this.formMode === FormMode.ViewMode) {
                    const domEditButton = $('<a class="btn btn-primary" ">Edit Item</a>');
                    domEditButton.on('click', () => tthis.switchToMode(FormMode.EditMode));
                    this.htmlElements.itemContainer.append(domEditButton);
                }
                detailForm.createFormByObject(tthis.htmlElements, configuration);
                (0, DomHelper_1.debugElementToDom)(element1, "#debug_mofelement");
                (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
            });
            this.htmlElements.itemContainer.empty();
            this.htmlElements.itemContainer.text("Loading content and form...");
            // Creates the viewmode Selection field
            if (this.htmlElements.viewModeSelectorContainer !== undefined
                && this.htmlElements.viewModeSelectorContainer !== null) {
                this.htmlElements.viewModeSelectorContainer.empty();
                const viewModeForm = new ViewModeSelectionForm_1.ViewModeSelectionForm();
                const htmlViewModeForm = viewModeForm.createForm();
                viewModeForm.viewModeSelected.addListener(_ => configuration.refreshForm());
                this.htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
            }
        }
    }
    exports.ItemDetailFormCreator = ItemDetailFormCreator;
});
//# sourceMappingURL=Forms.js.map