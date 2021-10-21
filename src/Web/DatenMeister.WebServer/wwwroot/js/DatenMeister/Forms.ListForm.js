define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ListForm = void 0;
    class ListForm {
        createFormByCollection(parent, isReadOnly) {
            var _a;
            let table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
            const fields = this.formElement.get("field");
            const headerRow = $("<thead><tr></tr></thead>");
            const innerRow = $("tr", headerRow);
            for (let n in fields) {
                if (!fields.hasOwnProperty(n))
                    continue;
                const field = fields[n];
                let cell = $("<th>");
                cell.text((_a = field.get("title")) !== null && _a !== void 0 ? _a : field.get("name"));
                innerRow.append(cell);
            }
            for (let n in this.elements) {
                let element = this.elements[n];
                const row = $("tr", headerRow);
                for (let n in fields) {
                    if (!fields.hasOwnProperty(n))
                        continue;
                    const field = fields[n];
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
    exports.ListForm = ListForm;
});
//# sourceMappingURL=Forms.ListForm.js.map