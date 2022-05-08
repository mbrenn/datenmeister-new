import * as Mof from "../Mof";

export interface IFormNavigation {
    workspace: string;
    extentUri: string;
    formElement: Mof.DmObject;
}

export interface IForm extends IFormNavigation
{

    // Just performs a refresh of the form
    refreshForm(): void;
}

export interface IDetailForm extends IForm {
    itemId: string;
}