
import * as InterfacesForms from "./Interfaces.Forms";
import {DmObject} from "./Mof";
import * as Mof from "./Mof";
import {createField} from "./Forms.FieldFactory";

export class ListForm implements InterfacesForms.IForm {
    elements: Array<DmObject>;
    extentUri: string;
    formElement: DmObject;
    itemId: string;
    workspace: string;

    createFormByCollection(parent: JQuery<HTMLElement>, isReadOnly: boolean) {
        let table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
        const fields = this.formElement.get("field");

        const headerRow = $("<tbody><tr></tr></tbody>");
        const innerRow = $("tr", headerRow);

        for (let n in fields) {
            if (!fields.hasOwnProperty(n)) continue;
            const field = fields[n] as Mof.DmObject;

            let cell = $("<th></th>");
            cell.text(field.get("title") ?? field.get("name"));

            innerRow.append(cell);
        }

        table.append(headerRow);

        let elements = this.elements;
        for (let n in elements) {
            if (Object.prototype.hasOwnProperty.call(elements, n)) {
                let element = this.elements[n];

                const row = $("<tr></tr>");

                for (let n in fields) {
                    if (!fields.hasOwnProperty(n)) continue;
                    const field = fields[n] as DmObject;
                    let cell = $("<td></td>");
                    
                    const fieldMetaClassId = field.metaClass.id;
                    const fieldElement = createField(
                        fieldMetaClassId,
                        {
                            form: this,
                            field: field,
                            itemUrl: element.uri,
                            isReadOnly: isReadOnly
                        });
                    
                    cell.append(fieldElement.createDom(element));
                    row.append(cell);
                }

                table.append(row);
            }
        }

        parent.append(table);
    }
}