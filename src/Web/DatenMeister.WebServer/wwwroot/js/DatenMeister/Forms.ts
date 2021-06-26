import * as Mof from "./Mof";


import DmObject = Mof.DmObject;

export class Form
{

}

export interface IFormField
{
    // Defines the field to be used to create the dom for the field
    Field: DmObject;

    // Creates the dom depending on the given field and the internal object
    createDom(parent:JQuery<HTMLElement>, dmElement: DmObject);

    // Evaluates the result of the user and injects it into the given element
    evaluateDom(dmElement:DmObject);

}

export class TextField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    Field: Mof.DmObject;

    createDom(parent: JQuery<HTMLElement>, dmElement: Mof.DmObject) {
        var fieldName = this.Field['name'];
        this._textBox = $("<input />");
        this._textBox.val(dmElement.get(fieldName).toString());
    }

    evaluateDom(dmElement: Mof.DmObject) {
    }
}