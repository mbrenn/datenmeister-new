import * as Mof from "../Mof";


/**
 * Defines the form types
 */
export enum FormType
{
    Object= "object",
    Collection = "collection",
    Row = "row",
    Table = "table"
}

export interface IFormNavigation {
    workspace: string;
    extentUri: string;

    /**
     * This is the item for which the form is currently displayed. It may be the same as the extent
     * in case the extent is being shown. 
     * 
     * For property lists, it contains the element whose properties are listed.
     */
    itemUrl: string;  
     
    formElement: Mof.DmObject;

    /**
     * Defines the form type of the current form in which the field will be embedded.
     */
    formType: FormType;
}

export interface IForm extends IFormNavigation
{

    // Just performs a refresh of the form
    refreshForm(): void;
}

export interface IDetailForm extends IForm {
    itemId: string;
}