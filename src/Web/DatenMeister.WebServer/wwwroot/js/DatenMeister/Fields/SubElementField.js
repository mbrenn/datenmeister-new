define(["require", "exports", "../Interfaces.Fields", "../Website"], function (require, exports, Interfaces_Fields_1, Website_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            this._list = $("<ul class='list-unstyled'></ul>");
            const fieldName = this.field.get('name');
            const value = dmElement.get(fieldName);
            for (var m in value) {
                if (Object.prototype.hasOwnProperty.call(value, m)) {
                    let innerValue = value[m];
                    let item = $("<li><a></a></li>");
                    const link = $("a", item);
                    link.text(innerValue.get('name'));
                    link.attr('href', (0, Website_1.getItemDetailUri)(innerValue));
                    this._list.append(item);
                }
            }
            return this._list;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=SubElementField.js.map