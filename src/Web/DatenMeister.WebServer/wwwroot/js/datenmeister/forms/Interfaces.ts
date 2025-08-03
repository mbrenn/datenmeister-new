import * as Mof from "../Mof.js";
import { IFormConfiguration } from "./IFormConfiguration.js";

export enum FormType {
    Object = "object",
    Collection = "collection"
}

/**
 * Contains the information which is used to navigate on the page.
 */
export interface IPageNavigation {
    /**
     * Switches the active form to a new url
     * @param newFormUrl The url to be handled
     */
    switchFormUrl(newFormUrl: string) : Promise<void>;
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
     * Stores the values of the form into the DOM, this is an optional method
     * @param reuseExistingElement This parameter decides whether the already provided element shall be 
     * directly returned or if a completely new element shall be created to only consider the diff
     */
    storeFormValuesIntoDom?(reuseExistingElement?: boolean): Promise<Mof.DmObject>;
}

export interface IForm extends IFormNavigation
{
    // Just performs a refresh of the form
    refreshForm(): Promise<void>;
}

export interface IPageForm extends IForm {

    /** Stores instance to the page to allow navigation */
    pageNavigation: IPageNavigation;
}
export interface IObjectFormElement extends IPageForm {
    /**
     * The element which is required to be shown
     */
    element: Mof.DmObject;

    createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration) : Promise<void>;
}

/**
 * Defines the instance which will be added to an overall CollectionForm
 * The elements will receive a set of elements and a reference to its parent item ('itemUrl'). 
 * Usually, the task of the implementation is to show the collections
 */
export interface ICollectionFormElement extends IPageForm {
    /**
     * Elements which are required to shown
     */
    callbackLoadItems: (query: QueryFilterParameter) => Promise<Array<Mof.DmObject>>;

    createFormByCollection(parent: JQuery<HTMLElement>, configuration: IFormConfiguration, refresh?: boolean): Promise<void>
}

export interface IQueryFilterResult
{
    message?: string
    elements: Array<Mof.DmObject>;
}

/**
 * Defines the filter parameters when a query shall be executed upon a set of items to allow
 * filtering, sorting or other data transformations directly on server-side
 */
export class QueryFilterParameter {
    orderBy?: string; // Property to which the ordering shall be done
    orderByDescending?: boolean; // Flag, whether ordering shall be done by descending
    filterByProperties?: Array<string>; // Property filters. Key is Propertyname, Value is textfilter
    filterByFreetext?: string; // Additional freetext
}