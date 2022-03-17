define(["require", "exports", "../Interfaces.Fields"], function (require, exports, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            const fieldName = this.field.get('name');
            let value = dmElement.get(fieldName);
            if (Array.isArray(value)) {
                if (value.length === 1) {
                    value = value[0];
                }
                else {
                    return $("<em>The value is an array and not supported by the referencefield</em>");
                }
            }
            if (this.isReadOnly === true) {
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