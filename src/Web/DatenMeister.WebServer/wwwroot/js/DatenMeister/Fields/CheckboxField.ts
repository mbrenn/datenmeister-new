
import { BaseField, IFormField } from "../Interfaces.Fields";
import { DmObject } from "../Mof";

export class Field extends BaseField implements IFormField {
    _checkbox: JQuery<HTMLElement>;

    createDom(dmElement: DmObject): JQuery<HTMLElement> {

        this._checkbox = $("<input type='checkbox'/>");

        const fieldName = this.field.get('name').toString();
        if (dmElement.get(fieldName)) {
            this._checkbox.prop('checked', true);
        }

        if (this.isReadOnly) {
            this._checkbox.prop('disabled', 'disabled');
        }

        return this._checkbox;
    }

    evaluateDom(dmElement: DmObject) {
        if (this._checkbox !== undefined && this._checkbox !== null) {
            const fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._checkbox.prop('checked'));
        }
    }
}