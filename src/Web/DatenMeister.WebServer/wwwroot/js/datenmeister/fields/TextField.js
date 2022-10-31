var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Mof", "../Mof", "./Interfaces", "../models/DatenMeister.class", "../forms/Interfaces", "../../burnsystems/StringManipulation"], function (require, exports, Mof, Mof_1, Interfaces_1, DatenMeister_class_1, Interfaces_2, StringManipulation_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    var _TextFieldData = DatenMeister_class_1._DatenMeister._Forms._TextFieldData;
    class Field extends Interfaces_1.BaseField {
        createDom(dmElement) {
            var _a, _b, _c;
            return __awaiter(this, void 0, void 0, function* () {
                const fieldName = (_b = (_a = this.field.get('name')) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
                /* Returns a list element in case an array is given */
                let value = (_c = dmElement.get(fieldName, Mof_1.ObjectType.String)) !== null && _c !== void 0 ? _c : "";
                // If we are in a table view, then reduce the length of the text to 100 
                // characters. 
                if (this.form.formType === Interfaces_2.FormType.Table) {
                    value = (0, StringManipulation_1.truncateText)(value, {
                        useWordBoundary: true,
                        maxLines: 3,
                        maxLength: 100
                    });
                }
                // Checks, if we are having an array, then we will just show the 
                // enumeration
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
                    const div = $("<div class='dm-textfield'/>");
                    if (value === undefined) {
                        div.append($("<em class='dm-undefined'>undefined</em>"));
                    }
                    else {
                        div.text(value !== null && value !== void 0 ? value : "undefined");
                    }
                    return div;
                }
                else {
                    const lineHeight = this.field.get(_TextFieldData.lineHeight, Mof_1.ObjectType.Number);
                    const width = this.field.get(_TextFieldData.width, Mof_1.ObjectType.Number);
                    if (lineHeight === undefined || Number.isNaN(lineHeight) || lineHeight <= 1) {
                        this._textBox = $("<input class='dm-textfield' />");
                        if (width !== undefined && !Number.isNaN(width) && width > 0) {
                            this._textBox.attr('size', width);
                        }
                        else {
                            this._textBox.attr('size', 70);
                        }
                    }
                    else {
                        this._textBox = $("<textarea class='dm-textfield' />");
                        this._textBox.attr('rows', lineHeight);
                        if (width !== undefined && !Number.isNaN(width) && width > 0) {
                            this._textBox.attr('cols', width);
                        }
                        else {
                            this._textBox.attr('cols', 80);
                        }
                    }
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