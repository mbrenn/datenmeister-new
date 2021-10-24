import * as Mof from "../Mof";

import {BaseField, IFormField} from "../Interfaces.Fields";

export class Field extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLElement>;

    createDom(dmElement: Mof.DmObject) {
        const fieldName = this.field.get('name').toString();

        /* Returns a list element in case an array is given */
        const value = dmElement.get(fieldName);
        if (Array.isArray(value)) {
            let enumeration = $("<ul class='list-unstyled'></ul>");
            for (let m in value) {
                if (Object.prototype.hasOwnProperty.call(value, m)) {
                    let innerValue = value[m];

                    let item = $("<li></li>");
                    item.text(innerValue.get('name'));
                    enumeration.append(item);
                }
            }
            
            return enumeration;
        }
        
        /* Otherwise just create the correct field type. */
        if (this.isReadOnly) {
            const div = $("<div />");
            div.text(dmElement.get(fieldName)?.toString() ?? "unknown");
            return div;
        } else {
            this._textBox = $("<input />");
            this._textBox.val(dmElement.get(fieldName)?.toString() ?? "unknown");

            return this._textBox;
        }
    }

    evaluateDom(dmElement: Mof.DmObject) {
        if (this._textBox !== undefined && this._textBox !== null)
        {
            const fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._textBox.val());
        }
    }
}
