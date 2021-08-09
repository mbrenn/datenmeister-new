import * as Mof from "./Mof";
import * as DataLoader from "./DataLoader";


import DmObject = Mof.DmObject;
import * as ApiConnection from "./ApiConnection";
import * as ApiModels from "./ApiModels";
import * as Settings from "./Settings";

export class Form {
    viewMode: string;
}

export class DetailForm {
    createViewForm(parent: JQuery<HTMLElement>, workspace: string, uri: string) {
        var tthis = this;

        // Load the object
        var defer1 = DataLoader.loadObjectByUri(workspace, uri);

        // Load the form
        var defer2 = getDefaultFormForItem(workspace, uri, "");

        // Wait for both
        $.when(defer1, defer2).then(function (element, form) {
            tthis.createFormByObject(parent, element, form, true);
        });

        parent.empty();
        parent.text("createViewForm");
    }

    createFormByObject(parent: JQuery<HTMLElement>, element: DmObject, form: DmObject, isReadOnly: boolean) {
        parent.text("createViewFormByObject");

        var table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");

        const tabs = form.get("tab");
        for (let n in tabs) {
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }

            const tab = tabs[n] as DmObject;

            var fields = tab.get("field");
            for (let m in fields) {
                var tr = $("<tr><td class='key'></td><td class='value'></td></tr>");

                if (!fields.hasOwnProperty(m)) {
                    continue;
                }

                var field = fields[m] as DmObject;
                
                const name = field.get("name") as any as string;

                $(".key", tr).text(name);
                
                var fieldMetaClassId = field.metaClass.id;
                let fieldElement;
                if (fieldMetaClassId === "DatenMeister.Models.Forms.TextFieldData") {
                    fieldElement = new TextField();
                }
                else if (fieldMetaClassId === "DatenMeister.Models.Forms.MetaClassElementFieldData") {
                    fieldElement = new MetaClassElementFieldData();
                }
                else {
                    fieldElement = $("<em></em>");
                    fieldElement.text(fieldMetaClassId ?? "unknown");
                    $(".value", tr).append(fieldElement);
                    table.append(tr);
                    continue;
                }

                fieldElement.field = field;
                fieldElement.isReadOnly = isReadOnly;
                
                let htmlElement = fieldElement.createDom(element);
                $(".value", tr).append(htmlElement);

                table.append(tr);
            }
        }

        parent.append(table);
    }
}

export function getDefaultFormForItem(workspace: string, item: string,  viewMode: string): JQuery.Deferred<Mof.DmObject, never, never> {
    var r = jQuery.Deferred<Mof.DmObject, never, never>();

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

export interface IFormField
{
    // Defines the field to be used to create the dom for the field
    field: DmObject;
    
    isReadOnly: boolean;

    // Creates the dom depending on the given field and the internal object
    createDom(dmElement: DmObject) : JQuery<HTMLElement>;

    // Evaluates the result of the user and injects it into the given element
    evaluateDom(dmElement:DmObject);
}

export class BaseField
{
    field: DmObject;
    isReadOnly: boolean;
}

export class TextField extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    createDom(dmElement: Mof.DmObject) {
        var fieldName = this.field.get('name').toString();
        if (this.isReadOnly) {
            const div = $("<div />");
            div.text(dmElement.get(fieldName)?.toString() ?? "unknown");
            return div;
        } else {
            this._textBox = $("<input />");
            this._textBox.val(dmElement.get(fieldName)?.toString() ?? "unknown");
            
            return this._textBox;
        }
    }

    evaluateDom(dmElement: Mof.DmObject) {
    }
}

export class MetaClassElementFieldData extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    createDom(dmElement: Mof.DmObject) {

        const div = $("<div />");
        if ( dmElement.metaClass !== undefined && dmElement.metaClass !== null)
        {
            div.text(dmElement.metaClass.id);
            
        }
        else
        {
            div.text("unknown");
        }
        return div;
        
    }

    evaluateDom(dmElement: Mof.DmObject) {
    }
}
