import {DetailFormActions} from "../FormActions";
import {BaseField, IFormField} from "./Interfaces";
import {DmObject} from "../Mof";

export class Field extends BaseField implements IFormField {

    button: JQuery;

    private inConfirmation: boolean;

    createDom(dmElement: DmObject): JQuery<HTMLElement> {

        const tthis = this;
        const title = this.field.get('title');
        const action = this.field.get('actionName');
        
        const parameter = this.field.get('parameter');

        this.inConfirmation = false;
        const requireConfirmation = DetailFormActions.requiresConfirmation(action);

        this.button = $("<button class='btn btn-secondary' type='button'></button>");
        this.button.text(title);

        this.button.on('click',
            () => {
                if (!requireConfirmation || tthis.inConfirmation) {
                    DetailFormActions.execute(action, tthis.form, tthis.itemUrl, dmElement, parameter);
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
