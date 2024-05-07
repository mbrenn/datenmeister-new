import * as FormActions from "../FormActions.js";
import { BaseField } from "./Interfaces.js";
import * as MofSync from "../MofSync.js";
export class Field extends BaseField {
    async createDom(dmElement) {
        const tthis = this;
        const title = this.field.get('title');
        const action = this.field.get('actionName');
        const parameter = this.field.get('parameter');
        const module = FormActions.getModule(action);
        this.inConfirmation = false;
        const requireConfirmation = module?.requiresConfirmation === true;
        this.button = $("<button class='btn btn-secondary' type='button'></button>");
        this.button.text(title);
        this.button.on('click', async () => {
            // There is the option whether a form action requires a separate confirmation
            // If this is the case, then the button itself is asking for confirmation upon the first 
            // click. Only then, the DetailForm itself is executed. 
            if (!requireConfirmation || tthis.inConfirmation) {
                if (tthis?.form?.storeFormValuesIntoDom !== undefined) {
                    await tthis.form.storeFormValuesIntoDom(true);
                }
                const mofWithSync = dmElement;
                if (module?.skipSaving !== true && mofWithSync.propertiesSet !== undefined) {
                    // We need to set the properties of the item, so the action handler can directly work on the item
                    await MofSync.sync(mofWithSync);
                }
                await FormActions.execute(action, tthis.form, dmElement, parameter);
            }
            if (requireConfirmation && !tthis.inConfirmation) {
                this.button.text("Are you sure?");
                tthis.inConfirmation = true;
            }
        });
        return this.button;
    }
    async evaluateDom(dmElement) {
    }
}
//# sourceMappingURL=ActionField.js.map