import * as Mof from "../Mof";
import * as DataLoader from "../client/Items";
import * as ClientForms from "../client/Forms";
import * as DetailForm from "./DetailForm";
import * as IForm from "./Interfaces";
import {ListForm} from "./ListForm";
import {debugElementToDom} from "../DomHelper";
import {IFormConfiguration} from "./IFormConfiguration";
import DmObject = Mof.DmObject;
import {SubmitMethod} from "./DetailForm";

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
    Creates a form containing a collection of items. 
    The input for this type is a collection of elements
*/
export class CollectionFormCreator implements IForm.IFormNavigation {
    extentUri: string;
    formElement: DmObject;
    workspace: string;

    createListForRootElements(parent: JQuery<HTMLElement>, workspace: string, extentUri: string, configuration: IFormConfiguration) {
        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }

        const tthis = this;

        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createListForRootElements(parent, workspace, extentUri, configuration);
            }
        }

        // Load the object
        const defer1 = DataLoader.getRootElements(workspace, extentUri);

        // Load the form
        const defer2 = ClientForms.getDefaultFormForExtent(workspace, extentUri, "");

        // Wait for both
        Promise.all([defer1, defer2]).then(([elements, form]) => {
            tthis.formElement = form;
            tthis.workspace = workspace;
            tthis.extentUri = extentUri;

            debugElementToDom(elements, "#debug_mofelement");
            debugElementToDom(form, "#debug_formelement");

            tthis.createFormByCollection(parent, elements, configuration);
        });

        parent.empty();
        parent.text("Loading content and form...");
    }

    createFormByCollection(parent: JQuery<HTMLElement>, elements: Array<Mof.DmObject>, configuration: IFormConfiguration) {
        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }

        const tthis = this;

        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createFormByCollection(parent, elements, configuration);
            }
        }

        parent.empty();
        const creatingElements = $("<div>Creating elements...</div>");
        parent.append(creatingElements);

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

                parent.append(form);
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
    Defines the form creator which also performs the connect to the webserver itself. 
    The input for this type of form is a single element
    
    This method handles all allowed form types.  
 */
export class DetailFormCreator implements IForm.IFormNavigation {

    element: DmObject;
    extentUri: string;
    formElement: DmObject;
    domContainer: JQuery;
    configuration: IFormConfiguration;
    itemId: string;
    workspace: string;

    createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration) {
        // First, store the parent and the configuration
        this.domContainer = parent;
        this.configuration = configuration;
        
        this.createFormForItem();
    }

    private createFormForItem() {
        const configuration = this.configuration;
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
    private domContainer: JQuery;

    switchToMode(formMode: FormMode) {
        this.formMode = formMode;
        this.rebuildForm();
    }

    createForm(parent: JQuery, workspace: string, itemUri: string) {
        this.domContainer = parent;
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

                    }
                }
            };
        }

        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.rebuildForm();
            }
        }

        // Load the object
        const defer1 = DataLoader.getObjectByUri(this.workspace, this.itemUri);

        // Load the form
        const defer2 = ClientForms.getDefaultFormForItem(this.workspace, this.itemUri, "");

        // Wait for both
        Promise.all([defer1, defer2]).then(([element1, form]) => {
            
            this.domContainer.empty();
            
            const detailForm = new DetailFormCreator();
            detailForm.workspace = this.workspace;
            detailForm.itemId = this.itemUri;
            detailForm.element = element1;
            detailForm.formElement = form;

            if (this.formMode === FormMode.ViewMode) {
                const domEditButton = $('<a class="btn btn-primary" ">Edit Item</a>');
                domEditButton.on('click', () => tthis.switchToMode(FormMode.EditMode));
                this.domContainer.append(domEditButton);
            }

            detailForm.createFormByObject(tthis.domContainer, configuration);

            debugElementToDom(element1, "#debug_mofelement");
            debugElementToDom(form, "#debug_formelement");

        });

        this.domContainer.empty();
        this.domContainer.text("Loading content and form...");
    }
}