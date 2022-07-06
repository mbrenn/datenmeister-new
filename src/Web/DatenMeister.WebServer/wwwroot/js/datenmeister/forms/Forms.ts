import * as Mof from "../Mof";
import * as DataLoader from "../client/Items";
import * as ClientForms from "../client/Forms";
import * as DetailForm from "./DetailForm";
import {SubmitMethod} from "./DetailForm";
import * as IForm from "./Interfaces";
import {ListForm} from "./ListForm";
import {debugElementToDom} from "../DomHelper";
import {IFormConfiguration} from "./IFormConfiguration";
import {navigateToExtent, navigateToItemByUrl} from "../Navigator";
import DmObject = Mof.DmObject;
import {ViewModeSelectionForm} from "./ViewModeSelectionForm";
import * as VML from "./ViewModeLogic"

export namespace FormModel {
    export function createEmptyFormWithDetail() {
        const form = new Mof.DmObject();
        const detailForm = new Mof.DmObject();
        detailForm.metaClass =
            {
                id: "DatenMeister.Models.Forms.DetailForm"
            };

        form.set('tab', [detailForm]);

        return form;
    }
}

/* 
    Defines the html fields which will be used for layouting.
 */
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
}

/*
    Creates a form containing a collection of items. 
    The input for this type is a collection of elements
*/
export class CollectionFormCreator implements IForm.IFormNavigation {
    extentUri: string;
    formElement: DmObject;
    workspace: string;

    createListForRootElements(htmlElements: CollectionFormHtmlElements, workspace: string, extentUri: string, configuration: IFormConfiguration) {
        if (htmlElements.itemContainer === undefined || htmlElements.itemContainer === null) {
            throw "htmlElements.itemContainer is not set";
        }

        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }
        
        if(configuration.viewMode === undefined || configuration.viewMode === null) {
            configuration.viewMode = VML.getCurrentViewMode();
        }

        const tthis = this;

        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createListForRootElements(htmlElements, workspace, extentUri, configuration);
            }
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

            debugElementToDom(elements, "#debug_mofelement");
            debugElementToDom(form, "#debug_formelement");

            tthis.createFormByCollection(htmlElements, elements, configuration);
        });

        htmlElements.viewModeSelectorContainer?.empty();
        if (htmlElements.viewModeSelectorContainer !== undefined && htmlElements.viewModeSelectorContainer !== null ) {
            const viewModeForm = new ViewModeSelectionForm();
            const htmlViewModeForm = viewModeForm.createForm();
            viewModeForm.viewModeSelected.addListener(
                _ => configuration.refreshForm());

            htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
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
                const tab = tabs[n] as DmObject;
                if (tab.metaClass.id === "DatenMeister.Models.Forms.ListForm") {
                    const listForm = new ListForm();
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


// Defines the possible viewmode of a form
export enum FormMode
{
    // The user can not edit the fields and just views the information
    ViewMode,
    // The user can edit the fields and submit these changes
    EditMode
};

/* 
    Defines the html fields which will be used for layouting.
 */
export class DetailFormHtmlElements
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
}


/* 
    Defines the form creator which also performs the connect to the webserver itself. 
    The input for this type of form is a single element
    
    This method handles all allowed form types.  
 */
export class DetailFormCreator implements IForm.IFormNavigation {

    element: DmObject;
    extentUri: string;
    formElement: DmObject;
    domContainer: JQuery;
    htmlItemContainer: IFormConfiguration;
    itemId: string;
    workspace: string;

    createFormByObject(htmlElements: DetailFormHtmlElements, configuration: IFormConfiguration) {
        // First, store the parent and the configuration
        this.domContainer = htmlElements.itemContainer;
        this.htmlItemContainer = configuration;
        
        this.createFormForItem();
    }

    private createFormForItem() {
        const configuration = this.htmlItemContainer;
        const tthis = this;

        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createFormForItem();
            }
        }

        if (this.element == null) this.element = new DmObject();

        const tabs = this.formElement.getAsArray("tab");
        for (let n in tabs) {
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }

            let form = $("<div />");
            const tab = tabs[n] as DmObject;
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
            } else if (tab.metaClass.id === "DatenMeister.Models.Forms.ListForm") {
                const listForm = new ListForm();
                listForm.workspace = this.workspace;
                listForm.extentUri = this.extentUri;
                listForm.itemId = this.itemId;
                listForm.formElement = tab;
                listForm.elements = this.element.get(tab.get("property"));

                listForm.createFormByCollection(form, {isReadOnly: true});
            } else {
                form = $("<div>Unknown Formtype:<span class='id'></span></div> ");
                $(".id", form).text(tab.metaClass.id);
            }

            this.domContainer.append(form);
        }

    }    
}

export class ItemDetailFormCreator {
    formMode: FormMode = FormMode.ViewMode;
    itemUri: string;
    workspace: string;
    private htmlElements: DetailFormHtmlElements;

    switchToMode(formMode: FormMode) {
        this.formMode = formMode;
        this.rebuildForm();
    }

    createForm(htmlElements: DetailFormHtmlElements, workspace: string, itemUri: string) {
        this.htmlElements = htmlElements;
        this.workspace = workspace;
        this.itemUri = itemUri;

        this.rebuildForm();
    }

    rebuildForm() {
        const tthis = this;

        let configuration;
        if (this.formMode === FormMode.ViewMode) {
            configuration = {isReadOnly: true};
        } else {
            configuration = {
                isReadOnly: false,
                onCancel: () => {
                    tthis.switchToMode(FormMode.ViewMode);
                },
                onSubmit: async (element, method) => {
                    await DataLoader.setProperties(tthis.workspace, tthis.itemUri, element);

                    if (method === SubmitMethod.Save) {
                        tthis.switchToMode(FormMode.ViewMode);
                    }

                    if (method === SubmitMethod.SaveAndClose) {
                        const containers = await DataLoader.getContainer(tthis.workspace, tthis.itemUri);
                        if (containers !== undefined && containers.length > 0) {
                            const parentWorkspace = containers[0].workspace;
                            if (containers.length === 2) {
                                // If user has selected would move to an extent, he should move to the items enumeration
                                navigateToExtent(parentWorkspace, containers[0].uri);
                            } else {
                                navigateToItemByUrl(parentWorkspace, containers[0].uri);
                            }
                        }
                        else{
                            alert ('Something wrong happened. I cannot retrieve the parent...');
                        }
                    }
                }
            };
        }

        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.rebuildForm();
            }
        }

        // Defines the viewmode, if not already defined by the caller
        if(configuration.viewMode === undefined || configuration.viewMode === null) {
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

            debugElementToDom(element1, "#debug_mofelement");
            debugElementToDom(form, "#debug_formelement");

        });

        this.htmlElements.itemContainer.empty();
        this.htmlElements.itemContainer.text("Loading content and form...");


        // Creates the viewmode Selection field
        if (this.htmlElements.viewModeSelectorContainer !== undefined 
            && this.htmlElements.viewModeSelectorContainer !== null ) {
            this.htmlElements.viewModeSelectorContainer.empty();
            
            const viewModeForm = new ViewModeSelectionForm();
            const htmlViewModeForm = viewModeForm.createForm();
            viewModeForm.viewModeSelected.addListener(_ => configuration.refreshForm());

            this.htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
        }
    }
}