
import { BaseField, IFormField } from "./Interfaces.js";
import * as Mof from "../Mof.js";

export class Field extends BaseField implements IFormField {
    _checkbox: JQuery<HTMLElement>;

    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {

        this._checkbox = $("<input type='checkbox'/>");

        const fieldName = this.field.get('name').toString();
        if (dmElement.get(fieldName, Mof.ObjectType.Boolean)) {
            this._checkbox.prop('checked', true);
        }

        if (this.isReadOnly) {
            this._checkbox.prop('disabled', 'disabled');
        }

        return this._checkbox;
    }

    async evaluateDom(dmElement: Mof.DmObject) : Promise<void> {
        if (this._checkbox !== undefined && this._checkbox !== null) {
            const fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._checkbox.prop('checked'));
        }
    }
}