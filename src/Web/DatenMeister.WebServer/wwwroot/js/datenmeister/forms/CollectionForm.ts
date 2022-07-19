﻿
/* 
    Defines the html fields which will be used for layouting.
 */
import {IFormConfiguration} from "./IFormConfiguration";
import * as VML from "./ViewModeLogic";
import * as DataLoader from "../client/Items";
import * as ClientForms from "../client/Forms";
import {debugElementToDom} from "../DomHelper";
import {ViewModeSelectionForm} from "./ViewModeSelectionForm";
import * as IForm from "./Interfaces";
import * as Mof from "../Mof";
import {TableForm} from "./TableForm";
import * as SIC from "../controls/SelectItemControl";
import * as Settings from "../Settings";
import {_DatenMeister} from "../models/DatenMeister.class";

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
    viewModeSelectorContainer: JQuery;
    
    /*
     Defines the JQuery instance which reflects the button which can be clicked
     by the user. After clicking the button, a selection field will be created in which the user
     can select the metaClass
     */
    createNewItemWithMetaClassBtn: JQuery;
    
    /*
     Within this container, the user can select the metaclass.
     This container is filled upon 'createNewItemWithMetaClass'.
     */
    createNewItemWithMetaClassContainer: JQuery;

    /**
     * Contains the button which can be clicked by the user to create a form out of the currently autogenerated form
     */
    storeCurrentFormBtn: JQuery;
}

/*
    Creates a form containing a collection of items. 
    The input for this type is a collection of elements
*/
export class CollectionFormCreator implements IForm.IFormNavigation {
    extentUri: string;
    formElement: Mof.DmObject;
    workspace: string;

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

        // Load the object
        const defer1 = DataLoader.getRootElements(workspace, extentUri);

        // Load the form
        const defer2 = ClientForms.getCollectionFormForExtent(workspace, extentUri, configuration.viewMode);

        // Wait for both
        Promise.all([defer1, defer2]).then(([elements, form]) => {
            tthis.formElement = form;
            tthis.workspace = workspace;
            tthis.extentUri = extentUri;

            debugElementToDom(elements, "#debug_mofelement");
            debugElementToDom(form, "#debug_formelement");

            tthis.createFormByCollection(htmlElements, elements, configuration);
        });

        /* 
         Creates the form for the View Mode Selection
         */
        htmlElements.viewModeSelectorContainer?.empty();
        if (htmlElements.viewModeSelectorContainer !== undefined && htmlElements.viewModeSelectorContainer !== null) {
            const viewModeForm = new ViewModeSelectionForm();
            const htmlViewModeForm = viewModeForm.createForm();
            viewModeForm.viewModeSelected.addListener(
                _ => configuration.refreshForm());

            htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
        }

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

        htmlElements.itemContainer.empty()
            .text("Loading content and form...");
    }

    createFormByCollection(htmlElements: CollectionFormHtmlElements, elements: Array<Mof.DmObject>, configuration: IFormConfiguration) {

        const itemContainer = htmlElements.itemContainer;

        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }

        const tthis = this;

        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createFormByCollection(htmlElements, elements, configuration);
            }
        }

        itemContainer.empty();
        const creatingElements = $("<div>Creating elements...</div>");
        itemContainer.append(creatingElements);

        const tabs = this.formElement.get("tab") as Array<Mof.DmObject>;

        let tabCount = Array.isArray(tabs) ? tabs.length : 0;
        for (let n in tabs) {
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }

            // Do it asynchronously. 
            window.setTimeout(() => {
                let form = $("<div />");
                const tab = tabs[n] as Mof.DmObject;
                if (tab.metaClass.uri === _DatenMeister._Forms.__TableForm_Uri) {
                    const listForm = new TableForm();
                    listForm.elements = elements;
                    listForm.formElement = tab;
                    listForm.workspace = this.workspace;
                    listForm.extentUri = this.extentUri;
                    listForm.createFormByCollection(form, configuration);
                } else {
                    alert('Unknown tab: ' + tab.metaClass.uri);
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


export function createMetaClassSelectionButtonForNewItem(buttonDiv: JQuery, containerDiv: JQuery, workspace: string, extentUri: string) {
    const tthis = this;

    buttonDiv.on('click', () => {
        containerDiv.empty();
        const selectItem = new SIC.SelectItemControl();
        const settings = new SIC.Settings();
        settings.showWorkspaceInBreadcrumb = true;
        settings.showExtentInBreadcrumb = true;
        selectItem.setWorkspaceById('Types');
        selectItem.setExtentByUri("dm:///_internal/types/internal");
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

        selectItem.init(containerDiv, settings);
    });
}