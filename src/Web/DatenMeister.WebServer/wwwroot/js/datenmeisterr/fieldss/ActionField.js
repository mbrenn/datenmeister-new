define(["require", "exports", "../FormActions", "../Interfaces.Fields"], function (require, exports, FormActions_1, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            const tthis = this;
            const title = this.field.get('title');
            const action = this.field.get('actionName');
            this._inConfirmation = false;
            const requireConfirmation = FormActions_1.DetailFormActions.requiresConfirmation(action);
            this.button = $("<button class='btn btn-secondary' type='button'></button>");
            this.button.text(title);
            this.button.on('click', () => {
                if (!requireConfirmation || tthis._inConfirmation) {
                    FormActions_1.DetailFormActions.execute(action, tthis.form, tthis.itemUrl, dmElement);
                }
                if (requireConfirmation) {
                    this.button.text("Are you sure?");
                    tthis._inConfirmation = true;
                }
            });
            return this.button;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=ActionField.js.map