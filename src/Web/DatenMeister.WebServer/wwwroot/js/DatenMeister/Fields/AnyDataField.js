define(["require", "exports", "../Interfaces.Fields", "../DomHelper"], function (require, exports, Interfaces_Fields_1, DomHelper_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            var _a, _b;
            const result = $("<div>");
            const headLine = $("<div class='dm-anydatafield-headline'><a class='dm-anydatafield-headline-value active'>Value</a> " +
                "| <a class='dm-anydatafield-headline-collection'>Collection</a> " +
                "| <a class='dm-anydatafield-headline-reference'>Reference</a></div>");
            const aValue = $(".dm-anydatafield-headline-value", headLine);
            const aCollection = $(".dm-anydatafield-headline-collection", headLine);
            const aReference = $(".dm-anydatafield-headline-reference", headLine);
            aValue.on('click', () => {
                alert('Only Values supported up to now');
            });
            aCollection.on('click', () => {
                alert('Only Values supported up to now');
            });
            aReference.on('click', () => {
                alert('Only Values supported up to now');
            });
            result.append(headLine);
            const fieldName = this.field.get('name').toString();
            const value = dmElement.get(fieldName);
            /* Otherwise just create the correct field type. */
            if (this.isReadOnly) {
                if ((typeof value === "object" || typeof value === "function") && (value !== null)) {
                    const asDmObject = value;
                    (0, DomHelper_1.injectNameByUri)(result, asDmObject.workspace, asDmObject.uri);
                }
                else {
                    const div = $("<div />");
                    div.text((_a = value === null || value === void 0 ? void 0 : value.toString()) !== null && _a !== void 0 ? _a : "unknown");
                    result.append(div);
                }
                return result;
            }
            else {
                this._textBox = $("<input />");
                this._textBox.val((_b = value === null || value === void 0 ? void 0 : value.toString()) !== null && _b !== void 0 ? _b : "unknown");
                result.append(this._textBox);
                return result;
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
//# sourceMappingURL=AnyDataField.js.map