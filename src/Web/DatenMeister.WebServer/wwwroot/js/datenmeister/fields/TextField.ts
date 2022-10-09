import * as Mof from "../Mof";
import {DmObject, ObjectType} from "../Mof";

import {BaseField, IFormField} from "./Interfaces";
import {_DatenMeister} from "../models/DatenMeister.class";
import _TextFieldData = _DatenMeister._Forms._TextFieldData;

export class Field extends BaseField implements IFormField
{
    // Gets or sets the method which allows to override the method to 
    // retrieve the property key
    OverridePropertyValue: () => string;
    
    _textBox: JQuery<HTMLElement>;

    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
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
            const div = $("<div class='dm-textfield'/>");
            const value = dmElement.get(fieldName);
            if (value === undefined) {
                div.append($("<em class='dm-undefined'>undefined</em>"));
            } else {
                div.text(dmElement.get(fieldName)?.toString() ?? "undefined");
            }
            return div;
        } else {
            const value = dmElement.get(fieldName)?.toString() ?? "";
            const lineHeight = this.field.get(_TextFieldData.lineHeight, ObjectType.Number);
            const width = this.field.get(_TextFieldData.width, ObjectType.Number);
            
            if (lineHeight === undefined || Number.isNaN(lineHeight) || lineHeight <= 1 ) {
                this._textBox = $("<input class='dm-textfield' />");

                if (width !== undefined && !Number.isNaN(width) && width > 0) {
                    this._textBox.attr('size', width);
                } else {
                    this._textBox.attr('size', 70);
                }
            }
            else {
                this._textBox = $("<textarea class='dm-textfield' />");
                this._textBox.attr('rows', lineHeight);

                if (width !== undefined && !Number.isNaN(width) && width > 0) {
                    this._textBox.attr('cols', width);
                } else {
                    this._textBox.attr('cols', 80);
                }
            }
                
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