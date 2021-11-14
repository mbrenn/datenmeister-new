import * as Mof from "./Mof";
import * as DataLoader from "./DataLoader";
import DmObject = Mof.DmObject;
import * as ApiConnection from "./ApiConnection";
import * as Settings from "./Settings";
import * as DetailForm from "./Forms.DetailForm";
import * as IForm from "./Interfaces.Forms";
import {ListForm} from "./Forms.ListForm";
import { debugElementToDom } from "./DomHelper";

export class Form {
    viewMode: string;
}

/*
    Creates a form containing a collection of items. 
    The input for this type is a collection of elements
*/
export class CollectionFormCreator implements IForm.IForm {
    extentUri: string;
    formElement: DmObject;
    workspace: string;

    createListForRootElements(parent: JQuery<HTMLElement>, workspace: string, extentUri: string, isReadOnly: boolean) {
        const tthis = this;

        // Load the object
        const defer1 = DataLoader.loadRootElementsFromExtent(workspace, extentUri);

        // Load the form
        const defer2 = getDefaultFormForExtent(workspace, extentUri, "");

        // Wait for both
        $.when(defer1, defer2).then((elements, form) => {
            tthis.formElement = form;
            tthis.workspace = workspace;
            tthis.extentUri = extentUri;

            tthis.createFormByCollection(parent, elements, isReadOnly);
            
        });

        parent.empty();
        parent.text("Loading content and form...");
    }

    createFormByCollection(parent: JQuery<HTMLElement>, elements: Array<Mof.DmObject>, isReadOnly: boolean) {
        const tthis = this;
        parent.empty();
        const creatingElements = $("<div>Creating elements...</div>");
        parent.append(creatingElements);

        const tabs = this.formElement.get("tab") as Array<Mof.DmObject>;

        let tabCount = tabs.length;
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
                    listForm.createFormByCollection(form, isReadOnly);
                }

                parent.append(form);
                tabCount--;

                if (tabCount == 0) {
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
export class DetailFormCreator implements IForm.IForm {

    element: DmObject;
    extentUri: string;
    formElement: DmObject;
    itemId: string;
    workspace: string;

    createFormByObject(parent: JQuery<HTMLElement>, isReadOnly: boolean) {
        const tthis = this;
        parent.empty();
        const creatingElements = $("<div>Creating elements...</div>");
        parent.append(creatingElements);

        const tabs = this.formElement.get("tab");
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

                detailForm.createFormByObject(form, isReadOnly);

                detailForm.onCancel = () => {
                    tthis.createViewForm(form, tthis.workspace, tthis.extentUri, tthis.itemId);
                }

                detailForm.onChange = (element) => {
                    DataLoader.storeObjectByUri(tthis.workspace, tthis.itemId, tthis.element).done(
                        () => {
                            tthis.createViewForm(form, tthis.workspace, tthis.extentUri, tthis.itemId);
                        }
                    );
                }
            } else if (tab.metaClass.id === "DatenMeister.Models.Forms.ListForm") {
                const listForm = new ListForm();
                listForm.workspace = this.workspace;
                listForm.extentUri = this.extentUri;
                listForm.itemId = this.itemId;
                listForm.formElement = tab;
                listForm.elements = this.element.get(tab.get("property"));
                
                listForm.createFormByCollection(form, true);
            }
            else {
                form = $("<div>Unknown Formtype:<span class='id'></span></div> ");
                $(".id", form).text(tab.metaClass.id);
            }

            parent.append(form);
        }

        // Removes the loading information
        creatingElements.remove();
    }

    createViewForm(parent: JQuery<HTMLElement>, workspace: string, extentUri: string, uri: string) {
        this.createForm(parent, workspace, extentUri, uri, true);
    }

    createEditForm(parent: JQuery<HTMLElement>, workspace: string, extentUri: string, uri: string) {
        this.createForm(parent, workspace, extentUri, uri, false);
    }

    createForm(parent: JQuery<HTMLElement>, workspace: string, extentUri: string, itemId: string, isReadOnly: boolean) {
        const tthis = this;

        // Load the object
        const defer1 = DataLoader.loadObjectByUri(workspace, itemId);

        // Load the form
        const defer2 = getDefaultFormForItem(workspace, itemId, "");

        // Wait for both
        $.when(defer1, defer2).then(function (element, form) {
            tthis.element = element;
            tthis.formElement = form;
            tthis.workspace = workspace;            
            tthis.extentUri = extentUri;
            tthis.itemId = itemId;
            tthis.createFormByObject(parent, isReadOnly);

            debugElementToDom(element, ".debug_mofelement");
            debugElementToDom(form, ".debug_formelement");
        });

        parent.empty();
        parent.text("Loading content and form...");
    }
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