import * as FormFactory from "./FormFactory.js";
import * as VML from "./ViewModeLogic.js";
import * as ClientItems from "../client/Items.js";
import * as ClientForms from "../client/Forms.js";
import { debugElementToDom } from "../DomHelper.js";
import { ViewModeSelectionControl } from "../controls/ViewModeSelectionControl.js";
import * as IForm from "./Interfaces.js";
import * as Mof from "../Mof.js";
import { DmObject, ObjectType } from "../Mof.js";
import * as SIC from "../controls/SelectItemControl.js";
import * as Navigator from "../Navigator.js";
import * as Settings from "../Settings.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import { FormSelectionControl } from "../controls/FormSelectionControl.js";
var _TableForm = _DatenMeister._Forms._TableForm;
import * as ActionField from "../fields/ActionField.js";
import { StatusFieldControl } from "../controls/StatusFieldControl.js";
import { ElementBreadcrumb } from "../controls/ElementBreadcrumb.js";
export class CollectionFormHtmlElements {
}
/*
    Creates a form containing a collection of root items of an extent
    The input for this type is a collection of elements
*/
export class CollectionFormCreator {
    constructor(htmlElements) {
        this.htmlElements = htmlElements;
        this.statusTextControl = new StatusFieldControl(htmlElements.statusContainer);
        this.pageNavigation = this;
    }
    async refreshForm() {
        await this.createCollectionForRootElements(this.workspace, this.extentUri, this.configuration);
    }
    async switchFormUrl(newFormUrl) {
        this._overrideFormUrl = newFormUrl;
        await this.refreshForm();
    }
    async createCollectionForRootElements(workspace, extentUri, configuration) {
        const tthis = this;
        tthis.workspace = workspace;
        tthis.extentUri = extentUri;
        tthis.itemUrl = extentUri;
        this.configuration = configuration;
        if (this.htmlElements.itemContainer === undefined || this.htmlElements.itemContainer === null) {
            throw "htmlElements.itemContainer is not set";
        }
        // Sets the refresh callback
        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = async () => {
                await this.refreshForm();
            };
        }
        // Empties the overview
        this.htmlElements.itemContainer.empty();
        this.htmlElements.itemContainer.append("Load Collection Form");
        // Set Read-Only as default
        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }
        // Load default viewmode
        this.statusTextControl.setListStatus("Loading Default Viewmode", false);
        if (configuration.viewMode === undefined || configuration.viewMode === null) {
            /*
            Gets the default viewmode for the extent to be shown
             */
            configuration.viewMode = await VML.getDefaultViewModeIfNotSet(workspace, extentUri);
        }
        this.statusTextControl.setListStatus("Loading Default Viewmode", true);
        // Load Breadcrumb
        this.statusTextControl.setListStatus("Loading Breadcrumb", false);
        let breadcrumb = new ElementBreadcrumb($(".dm-breadcrumb-page"));
        await breadcrumb.createForExtent(workspace, extentUri);
        this.statusTextControl.setListStatus("Loading Breadcrumb", true);
        // Loads the form
        this.statusTextControl.setListStatus("Loading Form", false);
        // Load the form
        const form = this._overrideFormUrl === undefined ?
            await ClientForms.getCollectionFormForExtent(workspace, extentUri, configuration.viewMode) :
            await ClientForms.getForm(this._overrideFormUrl, IForm.FormType.Collection);
        this.statusTextControl.setListStatus("Loading Form", true);
        // Wait for both
        tthis.formElement = form;
        debugElementToDom(form, "#debug_formelement");
        this.statusTextControl.setListStatus("Create Form", false);
        await tthis.createFormByCollection(configuration);
        this.statusTextControl.setListStatus("Create Form", true);
        /*
         Creates the form for the View Mode Selection
         */
        this.htmlElements.viewModeSelectorContainer?.empty();
        if (this.htmlElements.viewModeSelectorContainer !== undefined
            && this.htmlElements.viewModeSelectorContainer !== null) {
            this.statusTextControl.setListStatus("Create Viewmode Selection", false);
            const viewModeForm = new ViewModeSelectionControl();
            const htmlViewModeForm = await viewModeForm.createForm();
            viewModeForm.viewModeSelected.addListener(viewMode => {
                configuration.viewMode = viewMode;
                configuration.refreshForm();
            });
            this.htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
            this.statusTextControl.setListStatus("Create Viewmode Selection", true);
        }
        /*
         *  Creates the form selection in which the user can manually select a form
         */
        if (this.htmlElements.formSelectorContainer !== undefined
            && this.htmlElements.formSelectorContainer !== null) {
            this.statusTextControl.setListStatus("Create Form Selection", false);
            // Empty the container for the formselector
            this.htmlElements.formSelectorContainer.empty();
            const formControl = new FormSelectionControl();
            formControl.formSelected.addListener(selectedItem => {
                this._overrideFormUrl = selectedItem.selectedForm.uri;
                configuration.refreshForm();
            });
            formControl.formResetted.addListener(() => {
                this._overrideFormUrl = undefined;
                configuration.refreshForm();
            });
            let formUrl;
            // Tries to retrieve the current form uri
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
            /*
             * Handles the store auto-generated form button
             */
            if (this.htmlElements.storeCurrentFormBtn !== undefined) {
                this.htmlElements.storeCurrentFormBtn.on('click', () => {
                });
            }
            // Sets the current formurl and creates the control
            formControl.setCurrentFormUrl(formUrl);
            await formControl.createControl(this.htmlElements.formSelectorContainer);
            this.statusTextControl.setListStatus("Create Form Selection", true);
        }
        /*
         Creates the form for the creation of Metaclasses
         */
        if (this.htmlElements.createNewItemWithMetaClassBtn !== undefined &&
            this.htmlElements.createNewItemWithMetaClassContainer !== undefined) {
            createMetaClassSelectionButtonForNewItem($("#dm-btn-create-item-with-metaclass"), $("#dm-btn-create-item-metaclass"), workspace, extentUri);
        }
        /*
         * Creates the handler for the automatic creation of forms for extent
         */
        if (this.htmlElements.storeCurrentFormBtn !== undefined) {
            this.htmlElements.storeCurrentFormBtn.on('click', async () => {
                const result = await ClientForms.createCollectionFormForExtent(workspace, extentUri, configuration.viewMode);
                Navigator.navigateToItemByUrl(result.createdForm.workspace, result.createdForm.uri);
            });
        }
        this.statusTextControl.setStatusText("");
    }
    /**
     * Creates the actual html for a specific form
     * @param configuration
     */
    async createFormByCollection(configuration) {
        const itemContainer = this.htmlElements.itemContainer;
        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }
        const tthis = this;
        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = async () => {
                await tthis.createFormByCollection(configuration);
            };
        }
        // Create the action fields for the collection field
        this.statusTextControl.setListStatus("Actionfields", false);
        const fields = this.formElement.get(_DatenMeister._Forms._CollectionForm.field, Mof.ObjectType.Array);
        if (fields !== undefined) {
            const actionFields = $("<div></div>");
            for (const n in fields) {
                const field = fields[n];
                if (field.metaClass.uri === _DatenMeister._Forms.__ActionFieldData_Uri) {
                    const actionField = new ActionField.Field();
                    actionField.field = field;
                    actionFields.append(await actionField.createDom(DmObject.createFromReference(this.workspace, this.extentUri)));
                }
                else {
                    actionFields.append($("<div>Unsupported Field Type: " + field.metaClass.uri + "</div>"));
                }
            }
            itemContainer.append(actionFields);
            this.statusTextControl.setListStatus("Actionfields", true);
        }
        this.statusTextControl.setListStatus("Create Tabs", false);
        // Create the tabs
        const tabs = this.formElement.get(_DatenMeister._Forms._CollectionForm.tab, Mof.ObjectType.Array);
        var firstTab = true;
        for (let n in tabs) {
            const tab = tabs[n];
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }
            // The function which is capable to create the content of the tab
            // This function must be indirectly created since it works in the enumeration value
            const tabCreationFunction = async function (tab, form) {
                const parameter = {};
                const viewNodeUrl = tab.get(_TableForm.viewNode, ObjectType.Single);
                if (viewNodeUrl !== undefined) {
                    parameter.viewNode = viewNodeUrl.uri;
                }
                const callbackLoadItems = async (query) => {
                    parameter.filterByFreetext = query.filterByFreetext;
                    parameter.filterByProperties = query.filterByProperties;
                    parameter.orderBy = query.orderBy;
                    parameter.orderByDescending = query.orderByDescending;
                    // Load the object for the specific form
                    return await ClientItems.getRootElements(tthis.workspace, tthis.extentUri, parameter);
                };
                const formFactory = FormFactory.getCollectionFormFactory(tab.metaClass.uri);
                if (formFactory !== undefined) {
                    const tableForm = formFactory();
                    tableForm.pageNavigation = this;
                    tableForm.callbackLoadItems = callbackLoadItems;
                    tableForm.formElement = tab;
                    tableForm.workspace = tthis.workspace;
                    tableForm.extentUri = tthis.extentUri;
                    await tableForm.createFormByCollection(form, configuration);
                }
                else {
                    form.addClass('alert alert-warning');
                    const nameValue = tab.get('name', Mof.ObjectType.String);
                    let name = tab.metaClass.uri;
                    if (nameValue !== undefined) {
                        name = `${nameValue} (${tab.metaClass.uri})`;
                    }
                    form.text('Unknown form type for tab: ' + name);
                }
            };
            let tabFormContainer = $("<div></div>");
            this.statusTextControl.setListStatus("Create tab " + n, false);
            // Do it asynchronously. 
            await tabCreationFunction(tab, tabFormContainer);
            if (firstTab) {
                // Empties the loading information on first tab
                itemContainer.empty();
                firstTab = false;
            }
            itemContainer.append(tabFormContainer);
            this.statusTextControl.setListStatus("Create tab " + n, true);
        }
        this.statusTextControl.setListStatus("Create Tabs", true);
    }
}
export function createMetaClassSelectionButtonForNewItem(buttonDiv, containerDiv, workspace, extentUri) {
    buttonDiv.on('click', async () => {
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
        await selectItem.setWorkspaceById('Types');
        await selectItem.setExtentByUri("Types", "dm:///_internal/types/internal");
        selectItem.init(containerDiv, settings);
    });
}
//# sourceMappingURL=CollectionForm.js.map