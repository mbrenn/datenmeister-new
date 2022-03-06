define(["require", "exports", "../Interfaces.Fields"], function (require, exports, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            if (this.isReadOnly === true) {
                const fieldName = this.field.get('name');
                const value = dmElement.get(fieldName);
                this._list = $("<span></span>");
                if (value === undefined) {
                    this._list.html("<em>undefined</em>");
                }
                else {
                    this._list.text(value.get('name'));
                }
                return this._list;
            }
            else {
                const fieldName = this.field.get('name');
                const value = dmElement.get(fieldName);
                this._list = $("<span></span>");
                this._list.text(value.get('name'));
                return this._list;
            }
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=ReferenceField.js.map