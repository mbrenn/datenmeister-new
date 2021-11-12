define(["require", "exports", "./Forms.FieldFactory"], function (require, exports, Forms_FieldFactory_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.DetailForm = void 0;
    class DetailForm {
        createFormByObject(parent, isReadOnly) {
            var _a;
            let tr;
            let table;
            const tthis = this;
            parent.empty();
            this.fieldElements = new Array();
            const fields = this.formElement.get("field");
            table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
            const tableBody = $("<tbody><tr><th>Name</th><th>Value</th></tr>");
            table.append(tableBody);
            var itemUri = tthis.itemId.indexOf('#') === -1
                ? tthis.extentUri + "#" + tthis.itemId
                : tthis.itemId;
            for (let n in fields) {
                if (!fields.hasOwnProperty(n))
                    continue;
                const field = fields[n];
                tr = $("<tr><td class='key'></td><td class='value'></td></tr>");
                const name = (_a = field.get("title")) !== null && _a !== void 0 ? _a : field.get("name");
                $(".key", tr).text(name);
                const fieldMetaClassId = field.metaClass.id;
                let fieldElement = null; // The instance if IFormField allowing to create the dom
                let htmlElement; // The dom that had been created... 
                fieldElement = (0, Forms_FieldFactory_1.createField)(fieldMetaClassId, {
                    form: this,
                    field: field,
                    itemUrl: itemUri,
                    isReadOnly: isReadOnly
                });
                if (fieldElement === null) {
                    // No field element was created.
                    htmlElement = $("<em></em>");
                    htmlElement.text(fieldMetaClassId !== null && fieldMetaClassId !== void 0 ? fieldMetaClassId : "unknown");
                    $(".value", tr).append(fieldElement);
                }
                else {
                    fieldElement.field = field;
                    fieldElement.isReadOnly = isReadOnly;
                    fieldElement.form = this;
                    fieldElement.itemUrl = itemUri;
                    htmlElement = fieldElement.createDom(this.element);
                }
                this.fieldElements.push(fieldElement);
                $(".value", tr).append(htmlElement);
                tableBody.append(tr);
            }
            if (!isReadOnly) {
                // Add the Cancel and Submit buttons at the end of the creation to the table
                // allowing the cancelling and setting of the properties
                tr = $("<tr><td></td><td><button class='btn btn-secondary'>Cancel" +
                    "<button class='btn btn-primary'>Submit</button></td></tr>");
                tableBody.append(tr);
                $(".btn-secondary", tr).on('click', () => {
                    if (tthis.onCancel !== undefined && tthis.onCancel !== null) {
                        tthis.onCancel();
                    }
                });
                $(".btn-primary", tr).on('click', () => {
                    if (tthis.onChange !== undefined && tthis.onCancel !== null) {
                        for (let m in tthis.fieldElements) {
                            if (!tthis.fieldElements.hasOwnProperty(m))
                                continue;
                            const fieldElement = tthis.fieldElements[m];
                            fieldElement.evaluateDom(tthis.element);
                        }
                        tthis.onChange(tthis.element);
                    }
                });
            }
            parent.append(table);
        }
    }
    exports.DetailForm = DetailForm;
});
//# sourceMappingURL=Forms.DetailForm.js.map