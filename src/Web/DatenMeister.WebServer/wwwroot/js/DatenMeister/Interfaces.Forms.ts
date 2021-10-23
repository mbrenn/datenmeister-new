import * as Mof from "./Mof";

export interface IForm
{
    workspace: string;
    extentUri: string;
    formElement: Mof.DmObject;
}

export interface IDetailForm extends IForm {
    itemId: string;
}