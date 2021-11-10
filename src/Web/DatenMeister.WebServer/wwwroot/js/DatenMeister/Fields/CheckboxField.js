define(["require", "exports", "../Interfaces.Fields"], function (require, exports, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            this._checkbox = $("<input type='checkbox'/>");
            const fieldName = this.field.get('name').toString();
            if (dmElement.get(fieldName)) {
                this._checkbox.prop('checked', true);
            }
            if (this.isReadOnly) {
                this._checkbox.prop('disabled', 'disabled');
            }
            return this._checkbox;
        }
        evaluateDom(dmElement) {
            if (this._checkbox !== undefined && this._checkbox !== null) {
                const fieldName = this.field.get('name').toString();
                dmElement.set(fieldName, this._checkbox.prop('checked'));
            }
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=CheckboxField.js.map