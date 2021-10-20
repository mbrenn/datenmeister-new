define(["require", "exports", "../Interfaces.Fields"], function (require, exports, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            var _a, _b, _c, _d;
            const fieldName = this.field.get('name').toString();
            if (this.isReadOnly) {
                const div = $("<div />");
                div.text((_b = (_a = dmElement.get(fieldName)) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "unknown");
                return div;
            }
            else {
                this._textBox = $("<input />");
                this._textBox.val((_d = (_c = dmElement.get(fieldName)) === null || _c === void 0 ? void 0 : _c.toString()) !== null && _d !== void 0 ? _d : "unknown");
                return this._textBox;
            }
        }
        evaluateDom(dmElement) {
            if (this._textBox !== undefined && this._textBox !== null) {
                const fieldName = this.field.get('name').toString();
                dmElement.set(fieldName, this._textBox.val());
            }
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=TextField.js.map