define(["require", "exports", "../FormActions", "./Interfaces"], function (require, exports, FormActions_1, Interfaces_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        createDom(dmElement) {
            const tthis = this;
            const title = this.field.get('title');
            const action = this.field.get('actionName');
            const parameter = this.field.get('parameter');
            this.inConfirmation = false;
            const requireConfirmation = FormActions_1.DetailFormActions.requiresConfirmation(action);
            this.button = $("<button class='btn btn-secondary' type='button'></button>");
            this.button.text(title);
            this.button.on('click', () => {
                // There is the option whether a form action requires a separate confirmation
                // If this is the case, then the button itself is asking for confirmation upon the first 
                // click. Only then, the DetailForm itself is executed. 
                if (!requireConfirmation || tthis.inConfirmation) {
                    FormActions_1.DetailFormActions.execute(action, tthis.form, tthis.itemUrl, dmElement, parameter);
                }
                if (requireConfirmation && !tthis.inConfirmation) {
                    this.button.text("Are you sure?");
                    tthis.inConfirmation = true;
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