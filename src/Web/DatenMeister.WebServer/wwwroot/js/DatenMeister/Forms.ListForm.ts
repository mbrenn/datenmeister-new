
import * as InterfacesForms from "./Interfaces.Forms";
import {DmObject} from "./Mof";
import * as Mof from "./Mof";

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
                    const field = fields[n] as Mof.DmObject;

                    const name = field.get("name");
                    let cell = $("<td></td>");

                    let value = element.get(name);
                    if (Array.isArray(value)) {
                        let enumeration = $("<ul class='list-unstyled'></ul>");
                        for (let m in value) {
                            if (Object.prototype.hasOwnProperty.call(value, m)) {
                                let innerValue = value[m];

                                let item = $("<li></li>");
                                item.text(innerValue.get('name'));
                                enumeration.append(item);
                            }
                        }

                        cell.append(enumeration);
                    } else {
                        cell.text(element.get(name));
                    }

                    row.append(cell);
                }

                table.append(row);
            }
        }

        parent.append(table);
    }
}