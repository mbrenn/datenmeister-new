import { DetailFormActions } from "../FormActions";
import { BaseField, IFormField } from "../Interfaces.Fields";
import { DmObject } from "../Mof";

export class Field extends BaseField implements IFormField {
    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        const tthis = this;
        const title = this.field.get('title');
        const action = this.field.get('actionName');

        const result = $("<button class='btn btn-secondary' type='button'></button>");
        result.text(title);

        result.on('click',
            () => {
                DetailFormActions.execute(action, tthis.form, dmElement);
            });

        return result;
    }

    evaluateDom(dmElement: DmObject) {

    }
}
