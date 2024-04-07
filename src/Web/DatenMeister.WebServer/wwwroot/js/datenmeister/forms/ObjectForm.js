import * as FormFactory from "./FormFactory.js";
import { SubmitMethod } from "./Forms.js";
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
import { ElementBreadcrumb } from "../controls/ElementBreadcrumb.js";
import { StatusFieldControl } from "../controls/StatusFieldControl.js";
export class ObjectFormHtmlElements {
}
/**
 * Defines the form creator which also performs the connect to the webserver itself.
 * The input for this type of form is a single element
 *
 * This method handles all allowed form types.
 */
export class ObjectFormCreator {
    constructor(htmlElements) {
        this.formType = IForm.FormType.Object;
        this.htmlElements = htmlElements;
        this.statusTextControl = new StatusFieldControl(htmlElements.statusContainer);
    }
    async createFormByObject(configuration) {
        // First, store the parent and the configuration
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
        this.statusTextControl.setListStatus("Temporary Object", false);
        if (this.element == null)
            this.element = await MofSync.createTemporaryDmObject();
        this.statusTextControl.setListStatus("Temporary Object", true);
        const tabs = this.formElement.getAsArray("tab");
        for (let n in tabs) {
            try {
                if (!tabs.hasOwnProperty(n)) {
                    continue;
                }
                this.statusTextControl.setListStatus("Create Tab " + n, false);
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
                this.htmlElements.itemContainer.append(form);
                this.statusTextControl.setListStatus("Create Tab " + n, true);
            }
            catch (error) {
                const errorMessage = $("<div>An Exception has occured during the creation: <span></span></div>");
                $("span", errorMessage).text(error);
                this.htmlElements.itemContainer.append(errorMessage);
            }
        }
    }
}
export class ObjectFormCreatorForItem {
    constructor(htmlElements) {
        this.formMode = FormMode.ViewMode;
        this.htmlElements = htmlElements;
        this.statusTextControl = new StatusFieldControl(htmlElements.statusContainer);
    }
    async switchToMode(formMode) {
        this.formMode = formMode;
        await this.rebuildForm();
    }
    async createForm(workspace, itemUri) {
        this.workspace = workspace;
        this.itemUri = itemUri;
        // Check the edit parameter
        let p = new URLSearchParams(window.location.search);
        if (p.get('edit') === 'true') {
            this.formMode = FormMode.EditMode;
        }
        try {
            await this.rebuildForm();
        }
        catch (error) {
            this.htmlElements.itemContainer.text("An error occured during 'createForm': " + error);
        }
    }
    async rebuildForm() {
        // First, clear the page to have a fast reaction, otherwise the user will be confused
        this.htmlElements.itemContainer.empty();
        this.htmlElements.itemContainer.append($("<div>Loading Data and Form</div>"));
        // Creates the breadcrumb
        this.statusTextControl.setListStatus("Create Breadcrumb ", false);
        let breadcrumb = new ElementBreadcrumb($(".dm-breadcrumb-page"));
        await breadcrumb.createForItem(this.workspace, this.itemUri);
        this.statusTextControl.setListStatus("Create Breadcrumb ", true);
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
                        await tthis.switchToMode(FormMode.ViewMode);
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
            configuration.refreshForm = async () => {
                await tthis.rebuildForm();
            };
        }
        this.statusTextControl.setListStatus("Get Current Viewmode", false);
        // Defines the viewmode, if not already defined by the caller
        if (configuration.viewMode === undefined || configuration.viewMode === null) {
            configuration.viewMode = await VML.getDefaultViewModeIfNotSet(this.workspace, this.itemUri);
        }
        this.statusTextControl.setListStatus("Get Current Viewmode", true);
        this.statusTextControl.setListStatus("Load Object", false);
        // Load the object
        const defer1 = ClientItems.getObjectByUri(this.workspace, this.itemUri);
        this.statusTextControl.setListStatus("Load Form", false);
        // Load the form
        const defer2 = this._overrideFormUrl === undefined ?
            ClientForms.getObjectFormForItem(this.workspace, this.itemUri, configuration.viewMode) :
            ClientForms.getForm(this._overrideFormUrl, IForm.FormType.Object);
        // Wait for both
        Promise.all([defer1, defer2]).then(async ([element1, form]) => {
            this.statusTextControl.setListStatus("Load Object", true);
            this.statusTextControl.setListStatus("Load Form", true);
            // First the debug information
            debugElementToDom(element1, "#debug_mofelement");
            debugElementToDom(form, "#debug_formelement");
            // Now created the object form
            this.htmlElements.itemContainer.empty();
            const objectFormCreator = new ObjectFormCreator(this.htmlElements);
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
            this.statusTextControl.setListStatus("Create Form", false);
            await objectFormCreator.createFormByObject(configuration);
            this.statusTextControl.setListStatus("Create Form", true);
            // Creates the form selection
            if (this.htmlElements.formSelectorContainer !== undefined
                && this.htmlElements.formSelectorContainer !== null) {
                this.statusTextControl.setListStatus("Create Form Selection", false);
                this.htmlElements.formSelectorContainer.empty();
                const formControl = new FormSelectionControl();
                formControl.formSelected.addListener(async (selectedItem) => {
                    this._overrideFormUrl = selectedItem.selectedForm.uri;
                    await this.rebuildForm();
                });
                formControl.formResetted.addListener(async () => {
                    this._overrideFormUrl = undefined;
                    await this.rebuildForm();
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
                await formControl.createControl(this.htmlElements.formSelectorContainer);
                this.statusTextControl.setListStatus("Create Form Selection", true);
            }
        });
        // Creates the viewmode Selection field
        if (this.htmlElements.viewModeSelectorContainer !== undefined
            && this.htmlElements.viewModeSelectorContainer !== null) {
            this.htmlElements.viewModeSelectorContainer.empty();
            this.statusTextControl.setListStatus("Create Viewmode Selection", false);
            const viewModeForm = new ViewModeSelectionControl();
            const htmlViewModeForm = await viewModeForm.createForm(configuration.viewMode);
            viewModeForm.viewModeSelected.addListener(_ => configuration.refreshForm());
            this.htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
            this.statusTextControl.setListStatus("Create Viewmode Selection", true);
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
    }
}
//# sourceMappingURL=ObjectForm.js.map