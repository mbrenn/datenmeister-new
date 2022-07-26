
/* 
    Defines the html fields which will be used for layouting.
 */
import {IFormConfiguration} from "./IFormConfiguration";
import * as DetailForm from "./RowForm";
import {TableForm} from "./TableForm";
import * as DataLoader from "../client/Items";
import {SubmitMethod} from "./RowForm";
import {navigateToExtent, navigateToItemByUrl} from "../Navigator";
import * as VML from "./ViewModeLogic";
import * as ClientForms from "../client/Forms";
import {debugElementToDom} from "../DomHelper";
import {ViewModeSelectionControl} from "../controls/ViewModeSelectionControl";
import {FormSelectionControl} from "../controls/FormSelectionControl"
import * as Mof from "../Mof";
import {FormMode} from "./Forms";
import * as IForm from "./Interfaces";
import {_DatenMeister} from "../models/DatenMeister.class";

export class ObjectFormHtmlElements
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

    /**
     * Here, the user can select the form to be explicitly used for the current form.
     * When the user selects a certain form, then it will override the automatically selected form. 
     */
    formSelectorContainer?: JQuery;
    
}


/* 
    Defines the form creator which also performs the connect to the webserver itself. 
    The input for this type of form is a single element
    
    This method handles all allowed form types.  
 */
export class ObjectFormCreator implements IForm.IFormNavigation {

    element: Mof.DmObject;
    extentUri: string;
    formElement: Mof.DmObject;
    domContainer: JQuery;
    htmlItemContainer: IFormConfiguration;
    itemId: string;
    workspace: string;

    createFormByObject(htmlElements: ObjectFormHtmlElements, configuration: IFormConfiguration) {
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

        if (this.element == null) this.element = new Mof.DmObject();

        const tabs = this.formElement.getAsArray("tab");
        for (let n in tabs) {
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }

            let form = $("<div />");
            const tab = tabs[n] as Mof.DmObject;
            if (tab.metaClass.uri === _DatenMeister._Forms.__RowForm_Uri) {
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
            } else if (tab.metaClass.uri === _DatenMeister._Forms.__TableForm_Uri) {
                const listForm = new TableForm();
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

export class ObjectFormCreatorForItem {
    formMode: FormMode = FormMode.ViewMode;
    itemUri: string;
    workspace: string;
    private htmlElements: ObjectFormHtmlElements;

    switchToMode(formMode: FormMode) {
        this.formMode = formMode;
        this.rebuildForm();
    }

    createForm(htmlElements: ObjectFormHtmlElements, workspace: string, itemUri: string) {
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
                        } else {
                            alert('Something wrong happened. I cannot retrieve the parent...');
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
        if (configuration.viewMode === undefined || configuration.viewMode === null) {
            configuration.viewMode = VML.getCurrentViewMode();
        }

        // Load the object
        const defer1 = DataLoader.getObjectByUri(this.workspace, this.itemUri);

        // Load the form
        const defer2 = ClientForms.getObjectFormForItem(this.workspace, this.itemUri, configuration.viewMode);

        // Wait for both
        Promise.all([defer1, defer2]).then(([element1, form]) => {
            // First the debug information
            debugElementToDom(element1, "#debug_mofelement");
            debugElementToDom(form, "#debug_formelement");

            // Now created the object form
            this.htmlElements.itemContainer.empty();

            const objectFormCreator = new ObjectFormCreator();
            objectFormCreator.workspace = this.workspace;
            objectFormCreator.itemId = this.itemUri;
            objectFormCreator.element = element1;
            objectFormCreator.formElement = form;

            if (this.formMode === FormMode.ViewMode) {
                const domEditButton = $('<a class="btn btn-primary" ">Edit Item</a>');
                domEditButton.on('click', () => tthis.switchToMode(FormMode.EditMode));
                this.htmlElements.itemContainer.append(domEditButton);
            }

            objectFormCreator.createFormByObject(tthis.htmlElements, configuration);
        });

        this.htmlElements.itemContainer.empty();
        this.htmlElements.itemContainer.text("Loading content and form...");

        // Creates the viewmode Selection field
        if (this.htmlElements.viewModeSelectorContainer !== undefined
            && this.htmlElements.viewModeSelectorContainer !== null) {
            this.htmlElements.viewModeSelectorContainer.empty();

            const viewModeForm = new ViewModeSelectionControl();
            const htmlViewModeForm = viewModeForm.createForm();
            viewModeForm.viewModeSelected.addListener(_ => configuration.refreshForm());

            this.htmlElements.viewModeSelectorContainer.append(htmlViewModeForm);
        }

        // Creates the form selection
        if (this.htmlElements.formSelectorContainer !== undefined
            && this.htmlElements.formSelectorContainer !== null) {
            this.htmlElements.formSelectorContainer.empty();

            const formControl = new FormSelectionControl();
            formControl.formSelected.addListener(
                selectedForm => alert(selectedForm)
            );

            const _ = formControl.createControl(this.htmlElements.formSelectorContainer);
        }
    }
}