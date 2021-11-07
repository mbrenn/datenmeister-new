define(["require", "exports", "../Interfaces.Fields"], function (require, exports, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            var _a, _b, _c, _d;
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
            if (this.isReadOnly) {
                /* Otherwise just create the correct field type. */
                if (this.isReadOnly) {
                    const div = $("<div />");
                    div.text((_b = (_a = dmElement.get(fieldName)) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "unknown");
                    result.append(div);
                    return result;
                }
                else {
                    this._textBox = $("<input />");
                    this._textBox.val((_d = (_c = dmElement.get(fieldName)) === null || _c === void 0 ? void 0 : _c.toString()) !== null && _d !== void 0 ? _d : "unknown");
                    result.append(this._textBox);
                    return result;
                }
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