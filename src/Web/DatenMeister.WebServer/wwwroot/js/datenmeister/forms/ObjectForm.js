var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./RowForm", "./TableForm", "../client/Items", "./RowForm", "../Navigator", "./ViewModeLogic", "../client/Forms", "../DomHelper", "./ViewModeSelectionForm", "../Mof", "./Forms"], function (require, exports, DetailForm, TableForm_1, DataLoader, RowForm_1, Navigator_1, VML, ClientForms, DomHelper_1, ViewModeSelectionForm_1, Mof, Forms_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ObjectFormCreatorForItem = exports.ObjectFormCreator = exports.ObjectFormHtmlElements = void 0;
    class ObjectFormHtmlElements {
    }
    exports.ObjectFormHtmlElements = ObjectFormHtmlElements;
    /*
        Defines the form creator which also performs the connect to the webserver itself.
        The input for this type of form is a single element
        
        This method handles all allowed form types.
     */
    class ObjectFormCreator {
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
                this.element = new Mof.DmObject();
            const tabs = this.formElement.getAsArray("tab");
            for (let n in tabs) {
                if (!tabs.hasOwnProperty(n)) {
                    continue;
                }
                let form = $("<div />");
                const tab = tabs[n];
                if (tab.metaClass.id === "DatenMeister.Models.Forms.RowForm") {
                    const detailForm = new DetailForm.RowForm();
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
                else if (tab.metaClass.id === "DatenMeister.Models.Forms.TableForm") {
                    const listForm = new TableForm_1.TableForm();
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
    exports.ObjectFormCreator = ObjectFormCreator;
    class ObjectFormCreatorForItem {
        constructor() {
            this.formMode = Forms_1.FormMode.ViewMode;
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
            if (this.formMode === Forms_1.FormMode.ViewMode) {
                configuration = { isReadOnly: true };
            }
            else {
                configuration = {
                    isReadOnly: false,
                    onCancel: () => {
                        tthis.switchToMode(Forms_1.FormMode.ViewMode);
                    },
                    onSubmit: (element, method) => __awaiter(this, void 0, void 0, function* () {
                        yield DataLoader.setProperties(tthis.workspace, tthis.itemUri, element);
                        if (method === RowForm_1.SubmitMethod.Save) {
                            tthis.switchToMode(Forms_1.FormMode.ViewMode);
                        }
                        if (method === RowForm_1.SubmitMethod.SaveAndClose) {
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
            const defer2 = ClientForms.getObjectFormForItem(this.workspace, this.itemUri, configuration.viewMode);
            // Wait for both
            Promise.all([defer1, defer2]).then(([element1, form]) => {
                this.htmlElements.itemContainer.empty();
                const detailForm = new ObjectFormCreator();
                detailForm.workspace = this.workspace;
                detailForm.itemId = this.itemUri;
                detailForm.element = element1;
                detailForm.formElement = form;
                if (this.formMode === Forms_1.FormMode.ViewMode) {
                    const domEditButton = $('<a class="btn btn-primary" ">Edit Item</a>');
                    domEditButton.on('click', () => tthis.switchToMode(Forms_1.FormMode.EditMode));
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
    exports.ObjectFormCreatorForItem = ObjectFormCreatorForItem;
});
//# sourceMappingURL=ObjectForm.js.map