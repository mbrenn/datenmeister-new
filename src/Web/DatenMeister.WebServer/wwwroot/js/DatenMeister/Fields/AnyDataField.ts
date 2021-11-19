import {BaseField, IFormField} from "../Interfaces.Fields";
import { DmObject } from "../Mof";

export class Field extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLElement>;
    
    createDom(dmElement: DmObject): JQuery<HTMLElement> {

        const result = $("<div>");

        const headLine = $(
            "<div class='dm-anydatafield-headline'><a class='dm-anydatafield-headline-value active'>Value</a> " +
            "| <a class='dm-anydatafield-headline-collection'>Collection</a> " +
            "| <a class='dm-anydatafield-headline-reference'>Reference</a></div>");

        const aValue = $(".dm-anydatafield-headline-value", headLine);
        const aCollection = $(".dm-anydatafield-headline-collection", headLine);
        const aReference = $(".dm-anydatafield-headline-reference", headLine);

        aValue.on('click', () => {
            alert('Only Values supported up to now')
        });
        aCollection.on('click', () => {
            alert('Only Values supported up to now')
        });
        aReference.on('click', () => {
            alert('Only Values supported up to now')
        });

        result.append(headLine);

        const fieldName = this.field.get('name').toString();
        const value = dmElement.get(fieldName);

        /* Otherwise just create the correct field type. */
        if (this.isReadOnly) {
            const div = $("<div />");
            div.text(dmElement.get(fieldName)?.toString() ?? "unknown");
            result.append(div);
            return result;
        } else {
            this._textBox = $("<input />");
            this._textBox.val(dmElement.get(fieldName)?.toString() ?? "unknown");
            result.append(this._textBox)
            return result;
        }
    }

    evaluateDom(dmElement: DmObject) {
        if (this._textBox !== undefined && this._textBox !== null)
        {
            const fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._textBox.val());
        }
    }
    
}