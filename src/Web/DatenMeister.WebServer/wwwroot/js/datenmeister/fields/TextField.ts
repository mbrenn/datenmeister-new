import * as Mof from "../Mof";

import {BaseField, IFormField} from "./Interfaces";

export class Field extends BaseField implements IFormField
{
    // Gets or sets the method which allows to override the method to 
    // retrieve the property key
    OverridePropertyValue: () => string;
    
    _textBox: JQuery<HTMLElement>;

    createDom(dmElement: Mof.DmObject) {
        const fieldName = this.field.get('name')?.toString() ?? "";

        /* Returns a list element in case an array is given */
        const value = dmElement.get(fieldName);
        if (Array.isArray(value)) {
            let enumeration = $("<ul class='list-unstyled'></ul>");
            for (let m in value) {
                if (Object.prototype.hasOwnProperty.call(value, m)) {
                    let innerValue = value[m];

                    let item = $("<li></li>");
                    item.text(Mof.getName(innerValue));
                    enumeration.append(item);
                }
            }

            return enumeration;
        }

        /* Otherwise just create the correct field type. */
        if (this.isReadOnly) {
            const div = $("<div />");
            const value = dmElement.get(fieldName);
            if (value === undefined) {
                div.append($("<em>unknown</em>"));
            } else {
                div.text(dmElement.get(fieldName)?.toString() ?? "unknown");
            }
            return div;
        } else {
            const value = dmElement.get(fieldName)?.toString() ?? "";
            this._textBox = $("<input />");
            this._textBox.val(value);

            return this._textBox;
        }
    }

    evaluateDom(dmElement: Mof.DmObject) {
        if (this._textBox !== undefined && this._textBox !== null)
        {
            let fieldName: string;
            
            if (this.OverridePropertyValue === undefined) {
                fieldName = this.field.get('name').toString();
            }
            else{
                fieldName = this.OverridePropertyValue();
            }
            
            dmElement.set(fieldName, this._textBox.val());
        }
    }
}
