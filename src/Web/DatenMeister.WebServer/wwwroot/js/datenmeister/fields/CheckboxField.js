var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Interfaces"], function (require, exports, Interfaces_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        createDom(dmElement) {
            return __awaiter(this, void 0, void 0, function* () {
                this._checkbox = $("<input type='checkbox'/>");
                const fieldName = this.field.get('name').toString();
                if (dmElement.get(fieldName)) {
                    this._checkbox.prop('checked', true);
                }
                if (this.isReadOnly) {
                    this._checkbox.prop('disabled', 'disabled');
                }
                return this._checkbox;
            });
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