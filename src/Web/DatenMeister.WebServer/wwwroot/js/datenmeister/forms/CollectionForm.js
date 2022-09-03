var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./ViewModeLogic", "../client/Items", "../client/Forms", "../client/Forms", "../DomHelper", "../controls/ViewModeSelectionControl", "../Mof", "./TableForm", "../controls/SelectItemControl", "../Settings", "../models/DatenMeister.class", "../controls/FormSelectionControl"], function (require, exports, VML, DataLoader, ClientForms, Forms_1, DomHelper_1, ViewModeSelectionControl_1, Mof, TableForm_1, SIC, Settings, DatenMeister_class_1, FormSelectionControl_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createMetaClassSelectionButtonForNewItem = exports.CollectionFormCreator = exports.CollectionFormHtmlElements = void 0;
    class CollectionFormHtmlElements {
    }
    exports.CollectionFormHtmlElements = CollectionFormHtmlElements;
    /*
        Creates a form containing a collection of items.
        The input for this type is a collection of elements
    */
    class CollectionFormCreator {
        createCollectionForRootElements(htmlElements, workspace, extentUri, configuration) {
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
                    tthis.createCollectionForRootElements(htmlElements, workspace, extentUri, configuration);
                };
            }
            // Load the object
            const defer1 = DataLoader.getRootElements(workspace, extentUri);
            // Load the form
            const defer2 = this._overrideFormUrl === undefined ?
                ClientForms.getCollectionFormForExtent(workspace, extentUri, configuration.viewMode) :
                ClientForms.getForm(this._overrideFormUrl, Forms_1.FormType.Collection);
            // Wait for both
            Promise.all([defer1, defer2]).then(([elements, form]) => __awaiter(this, void 0, void 0, function* () {
                var _a;
                tthis.formElement = form;
                tthis.workspace = workspace;
                tthis.extentUri = extentUri;
                (0, DomHelper_1.debugElementToDom)(elements, "#debug_mofelement");
                (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
                tthis.createFormByCollection(htmlElements, elements, configuration);
                /*
                 Creates the form for the View Mode Selection
                 */
                (_a = htmlElements.viewModeSelectorContainer) === null || _a === void 0 ? void 0 : _a.empty();
                if (htmlElements.viewModeSelectorContainer !== undefined && htmlElements.viewModeSelectorContainer !== null) {
                    const viewModeForm = new ViewModeSelectionControl_1.ViewModeSelectionControl();
                    const htmlViewModeForm = viewModeForm.createForm();
                    viewModeForm.viewModeSelected.addListener(_ => configuration.refreshForm());
                    htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
                    // Creates the form selection
                    if (htmlElements.formSelectorContainer !== undefined
                        && htmlElements.formSelectorContainer !== null) {
                        htmlElements.formSelectorContainer.empty();
                        const formControl = new FormSelectionControl_1.FormSelectionControl();
                        formControl.formSelected.addListener(selectedItem => {
                            this._overrideFormUrl = selectedItem.selectedForm.uri;
                            configuration.refreshForm();
                        });
                        formControl.formResetted.addListener(() => {
                            this._overrideFormUrl = undefined;
                            configuration.refreshForm();
                        });
                        let formUrl;
                        if (this._overrideFormUrl !== undefined) {
                            formUrl = {
                                workspace: "Management",
                                uri: this._overrideFormUrl
                            };
                        }
                        else {
                            const byForm = form.get(DatenMeister_class_1._DatenMeister._Forms._Form.originalUri, Mof.ObjectType.String);
                            if (form.uri !== undefined && byForm === undefined) {
                                formUrl = {
                                    workspace: form.workspace,
                                    uri: form.uri
                                };
                            }
                            else if (byForm !== undefined) {
                                formUrl = {
                                    workspace: "Management",
                                    uri: byForm
                                };
                            }
                        }
                        formControl.setCurrentFormUrl(formUrl);
                        yield formControl.createControl(htmlElements.formSelectorContainer);
                    }
                }
            }));
            /*
             Creates the form for the creation of Metaclasses
             */
            if (htmlElements.createNewItemWithMetaClassBtn !== undefined &&
                htmlElements.createNewItemWithMetaClassContainer !== undefined) {
                createMetaClassSelectionButtonForNewItem($("#dm-btn-create-item-with-metaclass"), $("#dm-btn-create-item-metaclass"), workspace, extentUri);
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
                    if (tab.metaClass.uri === DatenMeister_class_1._DatenMeister._Forms.__TableForm_Uri) {
                        const listForm = new TableForm_1.TableForm();
                        listForm.elements = elements;
                        listForm.formElement = tab;
                        listForm.workspace = this.workspace;
                        listForm.extentUri = this.extentUri;
                        listForm.createFormByCollection(form, configuration);
                    }
                    else {
                        form.addClass('alert alert-warning');
                        const nameValue = tab.get('name', Mof.ObjectType.String);
                        let name = tab.metaClass.uri;
                        if (nameValue !== undefined) {
                            name = `${nameValue} (${tab.metaClass.uri})`;
                        }
                        form.text('Unknown tab: ' + name);
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
    function createMetaClassSelectionButtonForNewItem(buttonDiv, containerDiv, workspace, extentUri) {
        const tthis = this;
        buttonDiv.on('click', () => {
            containerDiv.empty();
            const selectItem = new SIC.SelectItemControl();
            const settings = new SIC.Settings();
            settings.showWorkspaceInBreadcrumb = true;
            settings.showExtentInBreadcrumb = true;
            selectItem.itemSelected.addListener(selectedItem => {
                if (selectedItem === undefined) {
                    document.location.href =
                        Settings.baseUrl +
                            "ItemAction/Extent.CreateItem?workspace=" +
                            encodeURIComponent(workspace) +
                            "&extent=" +
                            encodeURIComponent(extentUri);
                }
                else {
                    document.location.href =
                        Settings.baseUrl +
                            "ItemAction/Extent.CreateItem?workspace=" +
                            encodeURIComponent(workspace) +
                            "&extent=" +
                            encodeURIComponent(extentUri) +
                            "&metaclass=" +
                            encodeURIComponent(selectedItem.uri);
                }
            });
            selectItem.setWorkspaceById('Types');
            selectItem.setExtentByUri("Types", "dm:///_internal/types/internal");
            selectItem.init(containerDiv, settings);
        });
    }
    exports.createMetaClassSelectionButtonForNewItem = createMetaClassSelectionButtonForNewItem;
});
//# sourceMappingURL=CollectionForm.js.map