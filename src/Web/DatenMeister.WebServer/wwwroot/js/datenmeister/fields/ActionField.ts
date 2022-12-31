
import * as FormActions from "../FormActions";
import {BaseField, IFormField} from "./Interfaces";
import {DmObject} from "../Mof";
import * as ClientItems from "../client/Items";

export class Field extends BaseField implements IFormField {

    button: JQuery;

    private inConfirmation: boolean;

    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {

        const tthis = this;
        const title = this.field.get('title');
        const action = this.field.get('actionName');
        
        const parameter = this.field.get('parameter');

        const module = FormActions.getModule(action);
        this.inConfirmation = false;
        const requireConfirmation = module?.requiresConfirmation === true;

        this.button = $("<button class='btn btn-secondary' type='button'></button>");
        this.button.text(title);

        this.button.on('click',
            async () => {
                // There is the option whether a form action requires a separate confirmation
                // If this is the case, then the button itself is asking for confirmation upon the first 
                // click. Only then, the DetailForm itself is executed. 
                if (!requireConfirmation || tthis.inConfirmation) {
                    if (tthis.form.storeFormValuesIntoDom !== undefined) {
                        tthis.form.storeFormValuesIntoDom(true);
                    }

                    if (module?.skipSaving !== true) {
                        // We need to set the properties of the item, so the action handler can directly work on the item
                        await ClientItems.setProperties(dmElement.workspace, dmElement.uri, dmElement);
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

    evaluateDom(dmElement: DmObject) {

    }
}