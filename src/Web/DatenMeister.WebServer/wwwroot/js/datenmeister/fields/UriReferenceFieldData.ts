import * as Mof from "../Mof.js";
import {DmObject, ObjectType} from "../Mof.js";
import {BaseField, IFormField} from "./Interfaces.js";
import {_DatenMeister} from "../models/DatenMeister.class.js";
import _UriReferenceFieldData = _DatenMeister._Forms._UriReferenceFieldData;

export class Field extends BaseField implements IFormField
{
   
    _textBox: JQuery<HTMLElement>;

    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
        const fieldName = this.field.get('name')?.toString() ?? "";

        /* Returns a list element in case an array is given */
        let value = dmElement.get(fieldName, ObjectType.String) ?? "";
        const originalValue = value;        

        /* Otherwise just create the correct field type. */
        if (this.isReadOnly) {
            const divContainer = $("<div class='dm-urireference-container-readonly'></div>");
                        
            const div = $("<div class='dm-textfield'/>");
            if (value === undefined) {
                div.append($("<em class='dm-undefined'>undefined</em>"));

            } else {
                div.text(value ?? "undefined");
            }

            divContainer.append(div);
            return divContainer;
        } else {
            const domContainer = $("<div class='dm-urireference-container'></div>");
            const textUri = "<div>Uri:</div>";
            domContainer.append(textUri);
            
            const defaultExtent = this.field.get(_UriReferenceFieldData.defaultExtent, ObjectType.String);
            const defaultWorkspace = this.field.get(_UriReferenceFieldData.defaultWorkspace, ObjectType.String);
            
            this._textBox = $("<input size='80' class='dm-textfield' />");

            
            this._textBox.val(value);
            domContainer.append(this._textBox);
            return domContainer;
        }
    }

    async evaluateDom(dmElement: DmObject) : Promise<void> {
        if (this._textBox !== undefined && this._textBox !== null)
        {
            let fieldName: string;            
            dmElement.set(fieldName, this._textBox.val());
        }
    }
}