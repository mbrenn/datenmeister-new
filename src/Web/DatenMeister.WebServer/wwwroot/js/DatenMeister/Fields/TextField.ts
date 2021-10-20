import * as Mof from "../Mof";

import {BaseField, IFormField} from "../Interfaces.Fields";

export class Field extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLElement>;

    createDom(dmElement: Mof.DmObject) {
        const fieldName = this.field.get('name').toString();
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
