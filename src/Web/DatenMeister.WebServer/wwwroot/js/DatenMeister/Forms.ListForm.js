define(["require", "exports", "./Forms.FieldFactory"], function (require, exports, Forms_FieldFactory_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ListForm = void 0;
    class ListForm {
        createFormByCollection(parent, isReadOnly) {
            var _a;
            let headline = $("<h2></h2>");
            headline.text(this.formElement.get('name'));
            parent.append(headline);
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
                        let cell = $("<td></td>");
                        const fieldMetaClassId = field.metaClass.id;
                        const fieldElement = (0, Forms_FieldFactory_1.createField)(fieldMetaClassId, {
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
    exports.ListForm = ListForm;
});
//# sourceMappingURL=Forms.ListForm.js.map