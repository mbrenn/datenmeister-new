var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../FormActions", "./Interfaces"], function (require, exports, FormActions_1, Interfaces_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        createDom(dmElement) {
            return __awaiter(this, void 0, void 0, function* () {
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
                        FormActions_1.DetailFormActions.execute(action, tthis.form, dmElement, parameter);
                    }
                    if (requireConfirmation && !tthis.inConfirmation) {
                        this.button.text("Are you sure?");
                        tthis.inConfirmation = true;
                    }
                });
                return this.button;
            });
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=ActionField.js.map