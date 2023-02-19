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
                const fieldName = this.field.get('name').toString();
                const selectedValue = dmElement.get(fieldName);
                const values = this.field.get('values');
                this.selectBox = $("<select></select>");
                if (Array.isArray(values)) {
                    for (const value of values) {
                        const option = $("<option></option>");
                        option.val(value.get('value').toString());
                        option.text(value.get('name').toString());
                        this.selectBox.append(option);
                    }
                }
                else {
                    const option = $("<option></option>");
                    option.text("No values given...");
                    this.selectBox.append(option);
                }
                this.selectBox.val(selectedValue);
                if (this.isReadOnly) {
                    this.selectBox.prop('disabled', 'disabled');
                }
                return this.selectBox;
            });
        }
        evaluateDom(dmElement) {
            return __awaiter(this, void 0, void 0, function* () {
                const fieldName = this.field.get('name').toString();
                dmElement.set(fieldName, this.selectBox.val());
            });
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=DropDownField.js.map