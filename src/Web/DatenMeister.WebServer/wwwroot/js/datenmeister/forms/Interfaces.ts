import * as Mof from "../Mof";
import {SubmitMethod} from "./RowForm";
import {IFormConfiguration} from "./IFormConfiguration";


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

    /**
     * Stores the values of the form into the DOM, this is an optional method
     * @param reuseExistingElement This parameter decides whether the already provided element shall be 
     * directly returned or if a completely new element shall be created to only consider the diff
     */
    storeFormValuesIntoDom?(reuseExistingElement?: boolean): Promise<Mof.DmObject>;
}

export interface IForm extends IFormNavigation
{
    // Just performs a refresh of the form
    refreshForm(): void;
}

export interface IObjectFormElement extends IForm {
    /**
     * The element which is requried to be shown
     */
    element: Mof.DmObject;

    createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration) : Promise<void>;
}

/**
 * Defines the instance which will be added to an overall CollectionForm
 * The elements will receive a set of elements and a reference to its parent item ('itemUrl'). 
 * Usually, the task of the implementation is to show the collections
 */
export interface ICollectionFormElement extends IForm {
    /**
     * Elements which are required to shown
     */
    elements: Array<Mof.DmObject>;

    createFormByCollection(parent: JQuery<HTMLElement>, configuration: IFormConfiguration, refresh?: boolean): Promise<void>
}