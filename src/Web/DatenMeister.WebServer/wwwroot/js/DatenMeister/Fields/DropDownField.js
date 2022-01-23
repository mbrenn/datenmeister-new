define(["require", "exports", "../Interfaces.Fields"], function (require, exports, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            const fieldName = this.field.get('name').toString();
            const selectedValue = dmElement.get(fieldName);
            const values = this.field.get('values');
            this._selectBox = $("<select></select>");
            if (Array.isArray(values)) {
                for (const value of values) {
                    const option = $("<option></option>");
                    option.val(value.get('value').toString());
                    option.text(value.get('name').toString());
                    this._selectBox.append(option);
                }
            } else {
                const option = $("<option></option>");
                option.text("No values given...");
                this._selectBox.append(option);
            }
            this._selectBox.val(selectedValue);
            if (this.isReadOnly) {
                this._selectBox.prop('disabled', 'disabled');
            }
            return this._selectBox;
        }
        evaluateDom(dmElement) {
            const fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._selectBox.val());
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=DropDownField.js.map