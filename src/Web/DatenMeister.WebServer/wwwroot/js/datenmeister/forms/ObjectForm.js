import * as FormFactory from "./FormFactory.js";
import { SubmitMethod } from "./RowForm.js";
import * as ClientItems from "../client/Items.js";
import * as Navigator from "../Navigator.js";
import { navigateToExtentItems, navigateToItemByUrl } from "../Navigator.js";
import * as VML from "./ViewModeLogic.js";
import * as ClientForms from "../client/Forms.js";
import { debugElementToDom } from "../DomHelper.js";
import { ViewModeSelectionControl } from "../controls/ViewModeSelectionControl.js";
import { FormSelectionControl } from "../controls/FormSelectionControl.js";
import * as Mof from "../Mof.js";
import * as MofSync from "../MofSync.js";
import { FormMode } from "./Forms.js";
import * as IForm from "./Interfaces.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
export class ObjectFormHtmlElements {
}
/**
 * Defines the form creator which also performs the connect to the webserver itself.
 * The input for this type of form is a single element
 *
 * This method handles all allowed form types.
 */
export class ObjectFormCreator {
    constructor() {
        this.formType = IForm.FormType.Object;
    }
    async createFormByObject(htmlElements, configuration) {
        // First, store the parent and the configuration
        this.domContainer = htmlElements.itemContainer;
        this.htmlItemContainer = configuration;
        await this.createFormForItem();
    }
    async createFormForItem() {
        const configuration = this.htmlItemContainer;
        const tthis = this;
        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createFormForItem();
            };
        }
        if (this.element == null)
            this.element = await MofSync.createTemporaryDmObject();
        const tabs = this.formElement.getAsArray("tab");
        for (let n in tabs) {
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }
            let form = $("<div />");
            const tab = tabs[n];
            const factoryFunction = FormFactory.getObjectFormFactory(tab.metaClass.uri);
            if (factoryFunction !== undefined) {
                const detailForm = factoryFunction();
                detailForm.workspace = this.workspace;
                detailForm.extentUri = this.extentUri;
                detailForm.itemUrl = this.itemUrl;
                detailForm.formElement = tab;
                detailForm.element = this.element;
                await detailForm.createFormByObject(form, configuration);
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
            this.domContainer.append(form);
        }
    }
}
export class ObjectFormCreatorForItem {
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
        // Check the edit parameter
        let p = new URLSearchParams(window.location.search);
        if (p.get('edit') === 'true') {
            this.formMode = FormMode.EditMode;
        }
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
                onSubmit: async (element, method) => {
                    await MofSync.sync(element);
                    if (method === SubmitMethod.Save) {
                        tthis.switchToMode(FormMode.ViewMode);
                    }
                    if (method === SubmitMethod.SaveAndClose) {
                        const containers = await ClientItems.getContainer(tthis.workspace, tthis.itemUri);
                        if (containers !== undefined && containers.length > 0) {
                            const parentWorkspace = containers[0].workspace;
                            if (containers.length === 2) {
                                // If user has selected would move to an extent, he should move to the items enumeration
                                navigateToExtentItems(parentWorkspace, containers[0].uri);
                            }
                            else {
                                navigateToItemByUrl(parentWorkspace, containers[0].uri);
                            }
                        }
                        else {
                            alert('Something wrong happened. I cannot retrieve the parent...');
                        }
                    }
                }
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
        const defer1 = ClientItems.getObjectByUri(this.workspace, this.itemUri);
        // Load the form
        const defer2 = this._overrideFormUrl === undefined ?
            ClientForms.getObjectFormForItem(this.workspace, this.itemUri, configuration.viewMode) :
            ClientForms.getForm(this._overrideFormUrl, IForm.FormType.Object);
        // Wait for both
        Promise.all([defer1, defer2]).then(async ([element1, form]) => {
            // First the debug information
            debugElementToDom(element1, "#debug_mofelement");
            debugElementToDom(form, "#debug_formelement");
            // Now created the object form
            this.htmlElements.itemContainer.empty();
            const objectFormCreator = new ObjectFormCreator();
            objectFormCreator.workspace = this.workspace;
            objectFormCreator.itemUrl = this.itemUri;
            objectFormCreator.element = element1;
            objectFormCreator.extentUri = element1.extentUri;
            objectFormCreator.formElement = form;
            if (this.formMode === FormMode.ViewMode) {
                const domEditButton = $('<a class="btn btn-primary" ">Edit Item</a>');
                domEditButton.on('click', () => tthis.switchToMode(FormMode.EditMode));
                this.htmlElements.itemContainer.append(domEditButton);
            }
            await objectFormCreator.createFormByObject(tthis.htmlElements, configuration);
            // Creates the form selection
            if (this.htmlElements.formSelectorContainer !== undefined
                && this.htmlElements.formSelectorContainer !== null) {
                this.htmlElements.formSelectorContainer.empty();
                const formControl = new FormSelectionControl();
                formControl.formSelected.addListener(selectedItem => {
                    this._overrideFormUrl = selectedItem.selectedForm.uri;
                    this.rebuildForm();
                });
                formControl.formResetted.addListener(() => {
                    this._overrideFormUrl = undefined;
                    this.rebuildForm();
                });
                let formUrl;
                if (this._overrideFormUrl !== undefined) {
                    formUrl = {
                        workspace: "Management",
                        uri: this._overrideFormUrl
                    };
                }
                else {
                    const byForm = form.get(_DatenMeister._Forms._Form.originalUri, Mof.ObjectType.String);
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
                const _ = formControl.createControl(this.htmlElements.formSelectorContainer);
            }
        });
        // Creates the viewmode Selection field
        if (this.htmlElements.viewModeSelectorContainer !== undefined
            && this.htmlElements.viewModeSelectorContainer !== null) {
            this.htmlElements.viewModeSelectorContainer.empty();
            const viewModeForm = new ViewModeSelectionControl();
            const htmlViewModeForm = viewModeForm.createForm();
            viewModeForm.viewModeSelected.addListener(_ => configuration.refreshForm());
            this.htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
        }
        /*
         * Creates the handler for the automatic creation of forms for extent
         */
        if (this.htmlElements.storeCurrentFormBtn !== undefined) {
            this.htmlElements.storeCurrentFormBtn.on('click', async () => {
                const result = await ClientForms.createObjectFormForItem(this.workspace, this.itemUri, configuration.viewMode);
                Navigator.navigateToItemByUrl(result.createdForm.workspace, result.createdForm.uri);
            });
        }
        /*
         * Introduces the loading text
         */
        this.htmlElements.itemContainer.empty();
        this.htmlElements.itemContainer.text("Loading content and form...");
    }
}
//# sourceMappingURL=ObjectForm.js.map