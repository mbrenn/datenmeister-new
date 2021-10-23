define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ListForm = void 0;
    class ListForm {
        createFormByCollection(parent, isReadOnly) {
            var _a;
            let table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
            const fields = this.formElement.get("field");
            const headerRow = $("<tbody><tr></tr></tbody>");
            const innerRow = $("tr", headerRow);
            for (let n in fields) {
                if (!fields.hasOwnProperty(n))
                    continue;
                const field = fields[n];
                let cell = $("<th></th>");
                cell.text((_a = field.get("title")) !== null && _a !== void 0 ? _a : field.get("name"));
                innerRow.append(cell);
            }
            table.append(headerRow);
            let elements = this.elements;
            for (let n in elements) {
                if (Object.prototype.hasOwnProperty.call(elements, n)) {
                    let element = this.elements[n];
                    const row = $("<tr></tr>");
                    for (let n in fields) {
                        if (!fields.hasOwnProperty(n))
                            continue;
                        const field = fields[n];
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
                        }
                        else {
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
    exports.ListForm = ListForm;
});
//# sourceMappingURL=Forms.ListForm.js.map