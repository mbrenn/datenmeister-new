import * as FormFactory from "./FormFactory.js";
import * as VML from "./ViewModeLogic.js";
import * as ClientItems from "../client/Items.js";
import * as ClientForms from "../client/Forms.js";
import { debugElementToDom } from "../DomHelper.js";
import { ViewModeSelectionControl } from "../controls/ViewModeSelectionControl.js";
import * as Mof from "../Mof.js";
import { DmObject, ObjectType } from "../Mof.js";
import * as SIC from "../controls/SelectItemControl.js";
import * as Navigator from "../Navigator.js";
import * as Settings from "../Settings.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import { FormSelectionControl } from "../controls/FormSelectionControl.js";
var _TableForm = _DatenMeister._Forms._TableForm;
import { FormType } from "./Interfaces.js";
import * as ActionField from "../fields/ActionField.js";
export class CollectionFormHtmlElements {
}
/*
    Creates a form containing a collection of root items of an extent
    The input for this type is a collection of elements
*/
export class CollectionFormCreator {
    constructor() {
        this.formType = FormType.Collection;
    }
    async createCollectionForRootElements(htmlElements, workspace, extentUri, configuration) {
        if (htmlElements.itemContainer === undefined || htmlElements.itemContainer === null) {
            throw "htmlElements.itemContainer is not set";
        }
        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }
        if (configuration.viewMode === undefined || configuration.viewMode === null) {
            /*
            Gets the default viewmode for the extent to be shown
             */
            configuration.viewMode = await VML.getDefaultViewModeIfNotSet(workspace, extentUri);
        }
        const tthis = this;
        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createCollectionForRootElements(htmlElements, workspace, extentUri, configuration);
            };
        }
        // Load the form
        const defer2 = this._overrideFormUrl === undefined ?
            ClientForms.getCollectionFormForExtent(workspace, extentUri, configuration.viewMode) :
            ClientForms.getForm(this._overrideFormUrl, FormType.Collection);
        // Wait for both
        Promise.all([defer2]).then(async ([form]) => {
            tthis.formElement = form;
            tthis.workspace = workspace;
            tthis.extentUri = extentUri;
            tthis.itemUrl = extentUri;
            debugElementToDom(form, "#debug_formelement");
            await tthis.createFormByCollection(htmlElements, configuration);
            /*
             Creates the form for the View Mode Selection
             */
            htmlElements.viewModeSelectorContainer?.empty();
            if (htmlElements.viewModeSelectorContainer !== undefined && htmlElements.viewModeSelectorContainer !== null) {
                const viewModeForm = new ViewModeSelectionControl();
                const htmlViewModeForm = await viewModeForm.createForm();
                viewModeForm.viewModeSelected.addListener(_ => {
                    configuration.viewMode = VML.getCurrentViewMode();
                    configuration.refreshForm();
                });
                htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
            }
            /*
             *  Creates the form selection in which the user can manually select a form
             */
            if (htmlElements.formSelectorContainer !== undefined
                && htmlElements.formSelectorContainer !== null) {
                // Empty the container for the formselector
                htmlElements.formSelectorContainer.empty();
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
                if (htmlElements.storeCurrentFormBtn !== undefined) {
                    htmlElements.storeCurrentFormBtn.on('click', () => {
                    });
                }
                // Sets the current formurl and creates the control
                formControl.setCurrentFormUrl(formUrl);
                await formControl.createControl(htmlElements.formSelectorContainer);
            }
        });
        /*
         Creates the form for the creation of Metaclasses
         */
        if (htmlElements.createNewItemWithMetaClassBtn !== undefined &&
            htmlElements.createNewItemWithMetaClassContainer !== undefined) {
            createMetaClassSelectionButtonForNewItem($("#dm-btn-create-item-with-metaclass"), $("#dm-btn-create-item-metaclass"), workspace, extentUri);
        }
        /*
         * Creates the handler for the automatic creation of forms for extent
         */
        if (htmlElements.storeCurrentFormBtn !== undefined) {
            htmlElements.storeCurrentFormBtn.on('click', async () => {
                const result = await ClientForms.createCollectionFormForExtent(workspace, extentUri, configuration.viewMode);
                Navigator.navigateToItemByUrl(result.createdForm.workspace, result.createdForm.uri);
            });
        }
        /*
         * Introduces the loading text
         */
        htmlElements.itemContainer.empty()
            .text("Loading content and form...");
    }
    /**
     * Creates the actual html for a specific form
     * @param htmlElements
     * @param configuration
     */
    async createFormByCollection(htmlElements, configuration) {
        const itemContainer = htmlElements.itemContainer;
        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }
        const tthis = this;
        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = async () => {
                await tthis.createFormByCollection(htmlElements, configuration);
            };
        }
        itemContainer.empty();
        const creatingElements = $("<div>Creating elements...</div>");
        itemContainer.append(creatingElements);
        // Create the action fields
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
        }
        // Create the tabs
        const tabs = this.formElement.get(_DatenMeister._Forms._CollectionForm.tab, Mof.ObjectType.Array);
        let tabCount = Array.isArray(tabs) ? tabs.length : 0;
        for (let n in tabs) {
            const tab = tabs[n];
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }
            // The function which is capable to create the content of the tab
            // This function must be indirectly created since it works in the enumeration value
            const tabCreationFunction = function (tab, form) {
                return async () => {
                    const parameter = {};
                    const viewNodeUrl = tab.get(_TableForm.viewNode, ObjectType.Single);
                    if (viewNodeUrl !== undefined) {
                        parameter.viewNode = viewNodeUrl.uri;
                    }
                    // Load the object for the specific form
                    const elements = await ClientItems.getRootElements(tthis.workspace, tthis.extentUri, parameter);
                    const formFactory = FormFactory.getCollectionFormFactory(tab.metaClass.uri);
                    if (formFactory !== undefined) {
                        const tableForm = formFactory();
                        tableForm.elements = elements;
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
                    tabCount--;
                    if (tabCount === 0) {
                        // Removes the loading information
                        creatingElements.remove();
                    }
                };
            };
            let tabFormContainer = $("<div />");
            itemContainer.append(tabFormContainer);
            // Do it asynchronously. 
            window.setTimeout(tabCreationFunction(tab, tabFormContainer));
        }
    }
}
export function createMetaClassSelectionButtonForNewItem(buttonDiv, containerDiv, workspace, extentUri) {
    const tthis = this;
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