import {IForm} from "Interfaces.Forms";
import Mof = require("Mof");


export interface IFormField
{
    // Defines the field to be used to create the dom for the field
    field: Mof.DmObject;

    isReadOnly: boolean;

    form: IForm;

    // Creates the dom depending on the given field and the internal object
    createDom(dmElement: Mof.DmObject) : JQuery<HTMLElement>;

    // Evaluates the result of the user and injects it into the given element
    evaluateDom(dmElement: Mof.DmObject);
}

export class BaseField
{
    field: Mof.DmObject;
    isReadOnly: boolean;
    form: IForm;
}
