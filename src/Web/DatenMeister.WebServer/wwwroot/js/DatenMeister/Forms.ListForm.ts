
import * as InterfacesForms from "./Interfaces.Forms";
import {DmObject} from "./Mof";
import * as Mof from "./Mof";

export class ListForm implements InterfacesForms.IForm{
    elements: Array<DmObject>;
    extentUri: string;
    formElement: DmObject;
    itemId: string;
    workspace: string;

    createFormByCollection(parent: JQuery<HTMLElement>, isReadOnly: boolean)
    {
        let table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
        const fields = this.formElement.get("field");
        
        const headerRow = $("<thead><tr></tr></thead>");
        const innerRow = $("tr", headerRow);

        for (let n in fields) {
            if (!fields.hasOwnProperty(n)) continue;
            const field = fields[n] as Mof.DmObject;
            
            let cell = $("<th>");
            cell.text(field.get("title") ?? field.get("name"));
            
            innerRow.append(cell);
        }        
        
        for(let n in this.elements) {
            let element = this.elements[n];
            
            const row = $("tr", headerRow);

            for (let n in fields) {
                if (!fields.hasOwnProperty(n)) continue;
                const field = fields[n] as Mof.DmObject;

                const name = field.get("name");
                let cell = $("<td>");
                cell.text(element.get(name));
                
                innerRow.append(cell);
            }
            
            table.append(row);
        }
        
        table.append(headerRow);        
    }
}