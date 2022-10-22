import * as Mof from "../Mof";

export interface IFormNavigation {
    workspace: string;
    extentUri: string;

    /**
     * This is the item for which the form is currently displayed. It may be the same as the extent
     * in case the extent is being shown 
     */
    itemUrl: string;  
     
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