﻿/* 
    Defines the html fields which will be used for layouting.
 */
import {IFormConfiguration} from "./IFormConfiguration";
import * as VML from "./ViewModeLogic";
import * as ClientItems from "../client/Items";
import * as ClientForms from "../client/Forms";
import {debugElementToDom} from "../DomHelper";
import {ViewModeSelectionControl} from "../controls/ViewModeSelectionControl";
import * as IForm from "./Interfaces";
import * as Mof from "../Mof";
import {DmObject, ObjectType} from "../Mof";
import {TableForm} from "./TableForm";
import * as SIC from "../controls/SelectItemControl";
import * as Navigator from "../Navigator";
import * as Settings from "../Settings";
import {_DatenMeister} from "../models/DatenMeister.class";
import {FormSelectionControl} from "../controls/FormSelectionControl";
import {ItemLink} from "../ApiModels";
import _TableForm = _DatenMeister._Forms._TableForm;
import {FormType} from "./Interfaces";

export class CollectionFormHtmlElements
{
    /*
    Here, the items themselves will be added.
    This element should be a 'div' or another container item which is capable to hold a table
     */
    itemContainer: JQuery;
    /*
    Here, the options for selection will be added. 
    This element shall be 'div' which is capable to store the select element
     */
    viewModeSelectorContainer?: JQuery;
    
    /*
     Defines the JQuery instance which reflects the button which can be clicked
     by the user. After clicking the button, a selection field will be created in which the user
     can select the metaClass
     */
    createNewItemWithMetaClassBtn?: JQuery;
    
    /*
     Within this container, the user can select the metaclass.
     This container is filled upon 'createNewItemWithMetaClass'.
     */
    createNewItemWithMetaClassContainer?: JQuery;

    /**
     * Contains the button which can be clicked by the user to create a form out of the currently autogenerated form
     */
    storeCurrentFormBtn?: JQuery;

    /**
     * Here, the user can select the form to be explicitly used for the current form.
     * When the user selects a certain form, then it will override the automatically selected form.
     */
    formSelectorContainer?: JQuery;
}

/*
    Creates a form containing a collection of root items of an extent 
    The input for this type is a collection of elements
*/
export class CollectionFormCreator implements IForm.IFormNavigation {
    extentUri: string;
    formElement: Mof.DmObject;
    workspace: string;
    itemUrl: string;
    formType: FormType = FormType.Collection;

    /**
     * Defines the form url being used to select the form for the object form.
     * If the value is undefined, the standard form as being provided
     * by the server will be chosen.
     *
     * If this property is set the item will be retrieved within the Management Workspace
     * @private
     */
    private _overrideFormUrl?: string;

    createCollectionForRootElements(htmlElements: CollectionFormHtmlElements, workspace: string, extentUri: string, configuration: IFormConfiguration) {
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
            }
        }

        // Load the form
        const defer2 =
            this._overrideFormUrl === undefined ?
                ClientForms.getCollectionFormForExtent(workspace, extentUri, configuration.viewMode) :
                ClientForms.getForm(this._overrideFormUrl, FormType.Collection);

        // Wait for both
        Promise.all([defer2]).then(async ([form]) => {
            tthis.formElement = form;
            tthis.workspace = workspace;
            tthis.extentUri = extentUri;
            tthis.itemUrl = extentUri;

/*            debugElementToDom(elements, "#debug_mofelement");*/
            debugElementToDom(form, "#debug_formelement");

            tthis.createFormByCollection(htmlElements, configuration);

            /* 
             Creates the form for the View Mode Selection
             */
            htmlElements.viewModeSelectorContainer?.empty();
            if (htmlElements.viewModeSelectorContainer !== undefined && htmlElements.viewModeSelectorContainer !== null) {
                const viewModeForm = new ViewModeSelectionControl();
                const htmlViewModeForm = viewModeForm.createForm();
                viewModeForm.viewModeSelected.addListener(
                    _ => {
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
                formControl.formSelected.addListener(
                    selectedItem => {
                        this._overrideFormUrl = selectedItem.selectedForm.uri;
                        configuration.refreshForm();
                    });
                formControl.formResetted.addListener(
                    () => {
                        this._overrideFormUrl = undefined;
                        configuration.refreshForm();
                    });

                let formUrl: ItemLink;

                // Tries to retrieve the current form uri
                if (this._overrideFormUrl !== undefined) {
                    formUrl = {
                        workspace: "Management",
                        uri: this._overrideFormUrl
                    };
                } else {
                    const byForm = form.get(_DatenMeister._Forms._Form.originalUri, Mof.ObjectType.String);
                    if (form.uri !== undefined && byForm === undefined) {
                        formUrl = {
                            workspace: form.workspace,
                            uri: form.uri
                        };
                    } else if (byForm !== undefined) {
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
                    htmlElements.storeCurrentFormBtn.on('click',() => {
                            
                        })
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
            createMetaClassSelectionButtonForNewItem(
                $("#dm-btn-create-item-with-metaclass"),
                $("#dm-btn-create-item-metaclass"),
                workspace,
                extentUri);
        }

        /*
         * Creates the handler for the automatic creation of forms for extent
         */
        if (htmlElements.storeCurrentFormBtn !== undefined) {
            htmlElements.storeCurrentFormBtn.click(async () => {
                const result = await ClientForms.createCollectionFormForExtent(
                    workspace,
                    extentUri,
                    configuration.viewMode
                );

                Navigator.navigateToItemByUrl(result.createdForm.workspace, result.createdForm.uri);
            });
        }

        /*
         * Introduces the loading text
         */
        htmlElements.itemContainer.empty()
            .text("Loading content and form...");
    }

    createFormByCollection(
        htmlElements: CollectionFormHtmlElements,
        configuration: IFormConfiguration) {

        const itemContainer = htmlElements.itemContainer;

        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }

        const tthis = this;

        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createFormByCollection(htmlElements, configuration);
            }
        }

        itemContainer.empty();
        const creatingElements = $("<div>Creating elements...</div>");
        itemContainer.append(creatingElements);

        const tabs = this.formElement.get("tab") as Array<Mof.DmObject>;

        let tabCount = Array.isArray(tabs) ? tabs.length : 0;
        for (let n in tabs) {

            const tab = tabs[n] as Mof.DmObject;
            
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }

            // The function which is capable to create the content of the tab
            // This function must be indirectly created since it works in the enumeration value
            const tabCreationFunction = function(tab:DmObject, form: JQuery)
            {
                return async () => {

                    const parameter = {} as ClientItems.IGetRootElementsParameter;
                    const viewNodeUrl = tab.get(_TableForm.viewNode, ObjectType.Single) as DmObject;
                    if (viewNodeUrl !== undefined) {
                        parameter.viewNode = viewNodeUrl.uri;
                    }

                    // Load the object for the specific form
                    const elements = await ClientItems.getRootElements(
                        tthis.workspace, tthis.extentUri, parameter);

                    if (tab.metaClass.uri === _DatenMeister._Forms.__TableForm_Uri) {
                        const tableForm = new TableForm();
                        tableForm.elements = elements;
                        tableForm.formElement = tab;
                        tableForm.workspace = tthis.workspace;
                        tableForm.extentUri = tthis.extentUri;
                        await tableForm.createFormByCollection(form, configuration);
                    } else {
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
            }

            let tabFormContainer = $("<div />");
            itemContainer.append(tabFormContainer);
            // Do it asynchronously. 
            window.setTimeout(tabCreationFunction(tab, tabFormContainer));
        }
    }
}

export function createMetaClassSelectionButtonForNewItem(buttonDiv: JQuery, containerDiv: JQuery, workspace: string, extentUri: string) {
    const tthis = this;

    buttonDiv.on('click', async () => {
        containerDiv.empty();
        const selectItem = new SIC.SelectItemControl();
        const settings = new SIC.Settings();
        settings.showWorkspaceInBreadcrumb = true;
        settings.showExtentInBreadcrumb = true;
        selectItem.itemSelected.addListener(
            selectedItem => {
                if (selectedItem === undefined) {
                    document.location.href =
                        Settings.baseUrl +
                        "ItemAction/Extent.CreateItem?workspace=" +
                        encodeURIComponent(workspace) +
                        "&extent=" +
                        encodeURIComponent(extentUri);
                } else {
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