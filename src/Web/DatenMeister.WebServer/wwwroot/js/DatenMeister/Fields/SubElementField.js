define(["require", "exports", "../Interfaces.Fields", "../Mof", "../Forms.FieldFactory", "../Website"], function (require, exports, Interfaces_Fields_1, Mof_1, FieldFactory, Website_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {value: true});
    exports.Field = void 0;

    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            var _a;
            const fieldName = this.field.get('name');
            const value = dmElement.get(fieldName);
            if (this.isReadOnly) {
                this._list = $("<ul class='list-unstyled'></ul>");
                let foundElements = 0;
                for (let m in value) {
                    if (Object.prototype.hasOwnProperty.call(value, m)) {
                        let innerValue = value[m];
                        let item = $("<li><a></a></li>");
                        const link = $("a", item);
                        link.text(innerValue.get('name'));
                        link.attr('href', (0, Website_1.getItemDetailUri)(innerValue));
                        this._list.append(item);
                        foundElements++;
                    }
                }
                if (foundElements === 0) {
                    this._list = $("<em>No items</em>");
                }
            } else {
                this._list = $("<div></div>");
                const table = $("<table><tbody></tbody></table>");
                this._list.append(table);
                let fields = (_a = this.field.get('form')) === null || _a === void 0 ? void 0 : _a.get('field');
                let fieldsData = [];
                if (fields === undefined) {
                    const nameField = new Mof_1.DmObject();
                    nameField.setMetaClassById("DatenMeister.Models.Forms.TextFieldData");
                    nameField.set('name', 'name');
                    nameField.set('title', 'Name');
                    nameField.set('isReadOnly', true);
                    fieldsData.push(nameField);
                }
                const tBody = $("tbody", table);
                const tr = $("<tr></tr>");
                for (let fieldDataKey in fieldsData) {
                    let fieldData = fieldsData[fieldDataKey];
                    let header = $("<th></th>");
                    header.text(fieldData.get('title'));
                    tr.append(header);
                }
                tBody.append(tr);
                for (let m in value) {
                    if (Object.prototype.hasOwnProperty.call(value, m)) {
                        const tr = $("<tr></tr>");
                        for (let fieldDataKey in fieldsData) {
                            const td = $("<td></td>");
                            let innerValue = value[m];
                            let fieldData = fieldsData[fieldDataKey];
                            const field = FieldFactory.createField(fieldData.metaClass.id, {
                                field: fieldData,
                                form: this.form,
                                isReadOnly: true,
                                itemUrl: innerValue.uri,
                                configuration: {}
                            });
                            const dom = field.createDom(innerValue);
                            td.append(dom);
                            tr.append(td);
                        }
                        table.append(tr);
                    }
                }
                const newItem = $("<a href='#'>Attach new Item</a>");
                newItem.on('click', () => {
                    alert('YES');
                    return false;
                });
                this._list.append(newItem);
            }
            return this._list;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=SubElementField.js.map