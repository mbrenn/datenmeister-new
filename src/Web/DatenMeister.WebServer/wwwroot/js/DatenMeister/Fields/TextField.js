define(["require", "exports", "../Interfaces.Fields"], function (require, exports, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            var _a, _b, _c, _d, _e, _f;
            const fieldName = (_b = (_a = this.field.get('name')) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
            /* Returns a list element in case an array is given */
            const value = dmElement.get(fieldName);
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
                return enumeration;
            }
            /* Otherwise just create the correct field type. */
            if (this.isReadOnly) {
                const div = $("<div />");
                const value = dmElement.get(fieldName);
                if (value === undefined) {
                    div.append($("<em>unknown</em>"));
                }
                else {
                    div.text((_d = (_c = dmElement.get(fieldName)) === null || _c === void 0 ? void 0 : _c.toString()) !== null && _d !== void 0 ? _d : "unknown");
                }
                return div;
            }
            else {
                const value = (_f = (_e = dmElement.get(fieldName)) === null || _e === void 0 ? void 0 : _e.toString()) !== null && _f !== void 0 ? _f : "";
                this._textBox = $("<input />");
                this._textBox.val(value);
                return this._textBox;
            }
        }
        evaluateDom(dmElement) {
            if (this._textBox !== undefined && this._textBox !== null) {
                let fieldName;
                if (this.OverridePropertyValue === undefined) {
                    fieldName = this.field.get('name').toString();
                }
                else {
                    fieldName = this.OverridePropertyValue();
                }
                dmElement.set(fieldName, this._textBox.val());
            }
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=TextField.js.map