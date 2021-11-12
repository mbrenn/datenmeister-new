import { DetailFormActions } from "../FormActions";
import { BaseField, IFormField } from "../Interfaces.Fields";
import { DmObject } from "../Mof";

export class Field extends BaseField implements IFormField {
    
    button: JQuery;
    
    _inConfirmation: boolean;
    
    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        
        const tthis = this;
        const title = this.field.get('title');
        const action = this.field.get('actionName');

        this._inConfirmation = false;
        const requireConfirmation = DetailFormActions.requiresConfirmation(action);

        this.button = $("<button class='btn btn-secondary' type='button'></button>");
        this.button.text(title);

        this.button.on('click',
            () => {
                if(!requireConfirmation || tthis._inConfirmation) {
                    DetailFormActions.execute(action, tthis.form, tthis.itemUrl, dmElement);
                }
                
                if (requireConfirmation) {
                    this.button.text("Are you sure?");
                    tthis._inConfirmation = true;
                }
            });

        return this.button;
    }

    evaluateDom(dmElement: DmObject) {

    }
}