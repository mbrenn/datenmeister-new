import * as Mof from "./Mof";
import * as DataLoader from "./DataLoader";


import DmObject = Mof.DmObject;
import * as ApiConnection from "./ApiConnection";
import * as ApiModels from "./ApiModels";
import * as Settings from "./Settings";

export class Form
{
    viewMode: string;
    
    
}

export class DetailForm
{
    createViewForm ( parent: JQuery<HTMLElement>, workspace: string, uri: string) {
        var tthis = this;
        
        // Load the object
        var defer1 = DataLoader.loadObjectByUri(workspace, uri);
        
        // Load the form
        var defer2 = getDefaultFormForItem(workspace, uri, "");
        
        // Wait for both
        $.when(defer1,  defer2).then(function (element, form) {
            tthis.createViewFormByObject(parent, element,form);
        });
        
        parent.empty();
        parent.text("createViewForm");
    }
    
    createViewFormByObject ( parent: JQuery<HTMLElement>, element: DmObject, form: DmObject) {
        parent.text("createViewFormByObject");
        
        var table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");

        const tabs = form.get("tab");
        for (let n in tabs)
        {
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

                var field = fields[m];
                
                const name = field.get("name") as any as string;                
                $(".key", tr).text(name);
                $(".value", tr).text(element.get(name)?.toString() ?? "unknown");
                
                parent.append(tr);
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
    Field: DmObject;

    // Creates the dom depending on the given field and the internal object
    createDom(parent:JQuery<HTMLElement>, dmElement: DmObject);

    // Evaluates the result of the user and injects it into the given element
    evaluateDom(dmElement:DmObject);

}

export class TextField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    Field: Mof.DmObject;

    createDom(parent: JQuery<HTMLElement>, dmElement: Mof.DmObject) {
        var fieldName = this.Field['name'];
        this._textBox = $("<input />");
        this._textBox.val(dmElement.get(fieldName).toString());
    }

    evaluateDom(dmElement: Mof.DmObject) {
    }
}
