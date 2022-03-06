define(["require", "exports", "./Mof", "./Forms.FieldFactory", "./fields/TextField"], function (require, exports, Mof, Forms_FieldFactory_1, TextField) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {value: true});
    exports.DetailForm = void 0;

    class DetailForm {
        refreshForm() {
            this.createFormByObject(this.parentHtml, this.configuration);
        }

        createFormByObject(parent, configuration) {
            var _a;
            this.parentHtml = parent;
            this.configuration = configuration;
            if (configuration.isReadOnly === undefined) {
                configuration.isReadOnly = true;
            }
            if (configuration.allowAddingNewProperties === undefined) {
                configuration.allowAddingNewProperties = false;
            }
            let tr;
            let table;
            const tthis = this;
            parent.empty();
            this.fieldElements = new Array();
            const fields = this.formElement.getAsArray("field");
            table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
            const tableBody = $("<tbody><tr><th>Name</th><th>Value</th></tr></tbody>");
            table.append(tableBody);
            const itemUri = this.itemId === undefined
                ? ""
                : tthis.itemId.indexOf('#') === -1
                    ? tthis.extentUri + "#" + tthis.itemId
                    : tthis.itemId;
            // Creates the fields for the item
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
                    configuration: configuration,
                    form: this,
                    field: field,
                    itemUrl: itemUri,
                    isReadOnly: configuration.isReadOnly
                });
                if (fieldElement === null) {
                    // No field element was created.
                    htmlElement = $("<em></em>");
                    htmlElement.text(fieldMetaClassId !== null && fieldMetaClassId !== void 0 ? fieldMetaClassId : "unknown");
                    $(".value", tr).append(fieldElement);
                }
                else {
                    fieldElement.field = field;
                    fieldElement.isReadOnly = configuration.isReadOnly;
                    fieldElement.form = this;
                    fieldElement.itemUrl = itemUri;
                    htmlElement = fieldElement.createDom(this.element);
                }
                this.fieldElements.push(fieldElement);
                $(".value", tr).append(htmlElement);
                tableBody.append(tr);
            }
            // Checks, if user may add additional properties, if yes, include a button and create the corresponding
            // logic
            if (!configuration.isReadOnly && configuration.allowAddingNewProperties) {
                tr = $("<tr class='dm-row-newproperty'><td></td><td><button class='btn btn-secondary'>Add new property</button></td></tr>");
                tableBody.append(tr);
                const button = $("button", tr);
                button.on('click', () => {
                    const newRow = $("<tr><td><input class='dm-textfield-key' type='text' /></td><td class='dm-row-value'></td></tr>");
                    const rowValue = $(".dm-row-value", newRow);
                    const propertyTextField = $(".dm-textfield-key", newRow);
                    const textField = new TextField.Field();
                    textField.field = new Mof.DmObject();
                    textField.form = tthis;
                    textField.isReadOnly = configuration.isReadOnly;
                    textField.OverridePropertyValue =
                        () => {
                            return propertyTextField.val().toString();
                        };
                    rowValue.append(textField.createDom(tthis.element));
                    tthis.fieldElements.push(textField);
                    newRow.insertBefore($('.dm-row-newproperty'));
                    propertyTextField.trigger('focus');
                });
            }
            // Checks, if the form is a read-only form. If it is not a read-only, create the Accept and Reject buttons
            if (!configuration.isReadOnly) {
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
            const tableInfo = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
            tableInfo.append($("<tr><th>URL</th><td class='dm-detail-info-uri'>U</td></tr>"));
            tableInfo.append($("<tr><th>Workspace</th><td class='dm-detail-info-workspace'>W</td></tr>"));
            tableInfo.append($("<tr><th>Extent-Uri</th><td class='dm-detail-info-extenturi'>E</td></tr>"));
            tableInfo.append($("<tr><th>Metaclass</th><td class='dm-detail-info-metaclass'>m</td></tr>"));
            $(".dm-detail-info-uri", tableInfo).text(this.element.uri);
            $(".dm-detail-info-workspace", tableInfo).text(this.element.workspace);
            $(".dm-detail-info-extenturi", tableInfo).text(this.element.extentUri);
            $(".dm-detail-info-metaclass", tableInfo).text(this.element.metaClass.fullName);
            parent.append(tableInfo);
        }
    }
    exports.DetailForm = DetailForm;
});
//# sourceMappingURL=Forms.DetailForm.js.map