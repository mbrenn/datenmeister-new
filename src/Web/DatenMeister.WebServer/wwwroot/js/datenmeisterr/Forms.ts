import * as Mof from "./Mof";
import * as DataLoader from "./Client.Items";
import * as ApiConnection from "./ApiConnection";
import * as Settings from "./Settings";
import * as DetailForm from "./Forms.DetailForm";
import * as IForm from "./Forms.Interfaces";
import {ListForm} from "./Forms.ListForm";
import {debugElementToDom} from "./DomHelper";
import {IFormConfiguration} from "./IFormConfiguration";
import DmObject = Mof.DmObject;

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
        const defer1 = DataLoader.loadRootElementsFromExtent(workspace, extentUri);

        // Load the form
        const defer2 = getDefaultFormForExtent(workspace, extentUri, "");

        // Wait for both
        $.when(defer1, defer2).then((elements, form) => {
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

/* 
    Defines the form creator which also performs the connect to the webserver itself. 
    The input for this type of form is a single element
    
    This method handles all allowed form types.  
 */
export class DetailFormCreator implements IForm.IFormNavigation {

    element: DmObject;
    extentUri: string;
    formElement: DmObject;
    itemId: string;
    workspace: string;

    createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration) {
        const tthis = this;

        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createFormByObject(parent, configuration);
            }
        }

        if (this.element == null) this.element = new DmObject();

        parent.empty();
        const creatingElements = $("<div>Creating elements...</div>");
        parent.append(creatingElements);

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

            parent.append(form);
        }

        // Removes the loading information
        creatingElements.remove();
    }

    createViewForm(parent: JQuery<HTMLElement>, workspace: string, uri: string) {
        this.createForm(parent, workspace, uri, {isReadOnly: true});
    }

    createEditForm(parent: JQuery<HTMLElement>, workspace: string, uri: string) {
        const tthis = this;

        this.createForm(parent, workspace, uri,
            {
                isReadOnly: false,
                onCancel: () => {
                    tthis.createViewForm(parent, tthis.workspace, tthis.itemId);
                },
                onSubmit: (element) => {
                    DataLoader.storeObjectByUri(tthis.workspace, tthis.itemId, element).done(
                        () => {
                            tthis.createViewForm(parent, tthis.workspace, tthis.itemId);
                        }
                    );
                }
            });
    }

    createForm(parent: JQuery<HTMLElement>,
               workspace: string,
               itemUrl: string,
               configuration: IFormConfiguration) {
        const tthis = this;


        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createForm(parent, workspace, itemUrl, configuration);
            }
        }

        // Load the object
        const defer1 = DataLoader.loadObjectByUri(workspace, itemUrl);

        // Load the form
        const defer2 = getDefaultFormForItem(workspace, itemUrl, "");

        // Wait for both
        $.when(defer1, defer2).then(function (element, form) {
            tthis.element = element;
            tthis.formElement = form;
            tthis.workspace = workspace;
            tthis.itemId = itemUrl;

            debugElementToDom(element, "#debug_mofelement");
            debugElementToDom(form, "#debug_formelement");

            tthis.createFormByObject(parent, configuration);
        });

        parent.empty();
        parent.text("Loading content and form...");
    }

}

export function getForm(formUri: string): JQuery.Deferred<Mof.DmObject, never, never> {
    const r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/get/" +
        encodeURIComponent(formUri)
    ).done(x => {
        const dmObject =
            Mof.convertJsonObjectToDmObject(x);
        r.resolve(dmObject);
    });

    return r;
}

/*
    Gets the default form for a certain item by the webserver
 */
export function getDefaultFormForItem(workspace: string, item: string,  viewMode: string): JQuery.Deferred<Mof.DmObject, never, never> {
    const r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/default_for_item/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(item) +
        "/" +
        encodeURIComponent(viewMode)
    ).done(x => {
        const dmObject =
            Mof.convertJsonObjectToDmObject(x);
        r.resolve(dmObject);
    });

    return r;
}

/*
    Gets the default form for an extent uri by the webserver
 */
export function getDefaultFormForExtent(workspace: string, extentUri: string, viewMode: string): JQuery.Deferred<Mof.DmObject, never, never> {
    const r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/default_for_extent/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri) +
        "/" +
        encodeURIComponent(viewMode)
    ).done(x => {
        const dmObject =
            Mof.convertJsonObjectToDmObject(x);
        r.resolve(dmObject);
    });

    return r;
}

/*
    Gets the default form for an extent uri by the webserver
 */
export function getDefaultFormForMetaClass(metaClassUri: string): JQuery.Deferred<Mof.DmObject, never, never> {
    const r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/default_for_metaclass/" +
        encodeURIComponent(metaClassUri)
    ).done(x => {
        const dmObject =
            Mof.convertJsonObjectToDmObject(x);
        r.resolve(dmObject);
    });

    return r;
}