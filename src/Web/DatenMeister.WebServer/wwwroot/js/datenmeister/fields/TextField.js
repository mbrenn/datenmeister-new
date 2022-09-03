var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Mof", "./Interfaces"], function (require, exports, Mof, Interfaces_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        createDom(dmElement) {
            var _a, _b, _c, _d, _e, _f;
            return __awaiter(this, void 0, void 0, function* () {
                const fieldName = (_b = (_a = this.field.get('name')) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
                /* Returns a list element in case an array is given */
                const value = dmElement.get(fieldName);
                if (Array.isArray(value)) {
                    let enumeration = $("<ul class='list-unstyled'></ul>");
                    for (let m in value) {
                        if (Object.prototype.hasOwnProperty.call(value, m)) {
                            let innerValue = value[m];
                            let item = $("<li></li>");
                            item.text(Mof.getName(innerValue));
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
                        div.append($("<em class='dm-undefined'>undefined</em>"));
                    }
                    else {
                        div.text((_d = (_c = dmElement.get(fieldName)) === null || _c === void 0 ? void 0 : _c.toString()) !== null && _d !== void 0 ? _d : "undefined");
                    }
                    return div;
                }
                else {
                    const value = (_f = (_e = dmElement.get(fieldName)) === null || _e === void 0 ? void 0 : _e.toString()) !== null && _f !== void 0 ? _f : "";
                    this._textBox = $("<input />");
                    this._textBox.val(value);
                    return this._textBox;
                }
            });
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