
import {BaseField, IFormField} from "../Interfaces.Fields";
import { DmObject } from "../Mof";

export class Field extends BaseField implements IFormField {
    _selectBox: JQuery<HTMLElement>;

    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        const fieldName = this.field.get('name').toString();
        const selectedValue = dmElement.get(fieldName);
        const values = this.field.get('values') as Array<DmObject>;

        this._selectBox = $("<select></select>");
        for (const value of values) {
            const option = $("<option></option>");
            option.val(value.get('value').toString());
            option.text(value.get('name').toString());
            this._selectBox.append(option);
        }

        this._selectBox.val(selectedValue);
        if (this.isReadOnly) {
            this._selectBox.prop('disabled', 'disabled');
        }

        return this._selectBox;
    }

    evaluateDom(dmElement: DmObject) {

        const fieldName = this.field.get('name').toString();
        dmElement.set(fieldName, this._selectBox.val());
    }
}