define(["require", "exports", "../FormActions", "../Interfaces.Fields"], function (require, exports, FormActions_1, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            const tthis = this;
            const title = this.field.get('title');
            const action = this.field.get('actionName');
            const result = $("<button class='btn btn-secondary' type='button'></button>");
            result.text(title);
            result.on('click', () => {
                FormActions_1.DetailFormActions.execute(action, tthis.form, tthis.itemUrl, dmElement);
            });
            return result;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=ActionField.js.map