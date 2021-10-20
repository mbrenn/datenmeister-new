import * as Mof from "./Mof";
import * as DataLoader from "./DataLoader";
import DmObject = Mof.DmObject;
import * as ApiConnection from "./ApiConnection";
import * as ApiModels from "./ApiModels";
import * as Settings from "./Settings";
import IFields = require("./Interfaces.Fields");
import DetailForm = require("./DetailForm");
import IForm = require("./Interfaces.Forms");

export class Form {
    viewMode: string;
}

/* 
    Defines the form creator which also performs the connect to the webserver itself. 
    
    This method handles all allowed form types.  
 */
export class FormCreator implements IForm.IForm {

    element: DmObject;
    extentUri: string;
    formElement: DmObject;
    itemId: string;
    workspace: string;

    createFormByObject(parent: JQuery<HTMLElement>, isReadOnly: boolean) {
        const tthis = this;
        parent.empty();

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

                detailForm.createFormByObject(parent, isReadOnly);

                detailForm.onCancel = () => {
                    tthis.createViewForm(parent, tthis.workspace, tthis.extentUri, tthis.itemId);
                }

                detailForm.onChange = (element) => {
                    DataLoader.storeObjectByUri(tthis.workspace, tthis.itemId, tthis.element).done(
                        () => {
                            tthis.createViewForm(form, tthis.workspace, tthis.extentUri, tthis.itemId);
                        }
                    );
                }
            } else if (tab.metaClass.id === "DatenMeister.Models.Forms.ListForm") {

            }
            // DetailForm
            else {
                form = $("<div>Unknown Formtype:<span class='id'></span></div> ");
                $(".id", form).text(tab.metaClass.id);
            }

            parent.append(form);
        }
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
            tthis.itemId = itemId;
            tthis.createFormByObject(parent, isReadOnly);
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

    ApiConnection.get<ApiModels.Out.IItem>(
        Settings.baseUrl +
        "api/forms/default_for_item/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(item) +
        "/" +
        encodeURIComponent(viewMode)
    ).done(x => {
        const dmObject =
            Mof.createObjectFromJson(x.item, x.metaClass);
        r.resolve(dmObject);
    });

    return r;
}