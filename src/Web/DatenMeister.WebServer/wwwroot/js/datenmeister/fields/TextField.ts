import * as Mof from "../Mof.js";
import {BaseField, IFormField} from "./Interfaces.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import _TextFieldData = _DatenMeister._Forms._TextFieldData;
import {truncateText} from "../../burnsystems/StringManipulation.js";
import { injectNameByUri } from "../DomHelper.js";
import { TableForm } from "../forms/TableForm.js";

export class Field extends BaseField implements IFormField
{
    // Gets or sets the method which allows to override the method to 
    // retrieve the property key
    OverridePropertyValue: () => string;
    
    _textBox: JQuery<HTMLElement>;

    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {
        const fieldName = this.field.get('name')?.toString() ?? "";

        /* Returns a list element in case an array is given */
        let value = dmElement.get(fieldName, Mof.ObjectType.String) ?? "";
        const originalValue = value;
        
        // If we are in a table view, then reduce the length of the text to 100 
        // characters. 
        if ((this.form as TableForm).tableParameter?.shortenFullText === true) {
            value = truncateText(value, {
                useWordBoundary: true,
                maxLines: 3,
                maxLength: 100                
            });
        }
        else {
            const shortenTextLength = this.field.get(_DatenMeister._Forms._TextFieldData.shortenTextLength, Mof.ObjectType.Number);
            if (shortenTextLength !== undefined && shortenTextLength > 0) {
                value = truncateText(value, {useWordBoundary: true, maxLength: shortenTextLength});
            }
        }
        
        // Checks, if we are having an array, then we will just show the 
        // enumeration
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
            const divContainer = $("<div class='dm-textfield-container'></div>");
            
            if (this.field.get(_DatenMeister._Forms._TextFieldData.supportClipboardCopy, Mof.ObjectType.Boolean)) {
                const button = $("<button class='btn btn-secondary'>Copy to Clipboard</button>");
                button.on('click', () =>
                {
                    navigator.clipboard.writeText(originalValue);
                    alert('Text copied to clipboard');
                });
                
                divContainer.append(button);
            }
            
            const div = $("<div class='dm-textfield'/>");
            if (value === undefined) {
                div.append($("<em class='dm-undefined'>undefined</em>"));

            } else {
                div.text(value ?? "undefined");
            }

            if (fieldName === 'name') {
                // If the text field is of a certain typename, then tranform is to an injected property            
                let _ = injectNameByUri(div, dmElement.workspace, dmElement.uri, {});
            }

            divContainer.append(div);
            return divContainer;
        } else {
            const lineHeight = this.field.get(_TextFieldData.lineHeight, Mof.ObjectType.Number);
            const width = this.field.get(_TextFieldData.width, Mof.ObjectType.Number);
            
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

    async evaluateDom(dmElement: Mof.DmObject) : Promise<void> {
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