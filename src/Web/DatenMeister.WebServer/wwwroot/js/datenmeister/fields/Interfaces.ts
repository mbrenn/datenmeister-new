import {IForm, IFormNavigation} from "../forms/Interfaces";
import * as Mof from "Mof";
import { IFormConfiguration } from "../forms/IFormConfiguration";


export interface IFormField
{
    configuration: IFormConfiguration;

    // Defines the field to be used to create the dom for the field
    field: Mof.DmObject;

    isReadOnly: boolean;

    // Is connected to the item url of the element being connected to that element
    itemUrl: string;
    
    form: IFormNavigation;

    // Creates the dom depending on the given field and the internal object
    createDom(dmElement: Mof.DmObject) : Promise<JQuery<HTMLElement>>;

    // Evaluates the result of the user and injects it into the given element
    evaluateDom(dmElement: Mof.DmObject);

    /**
     * Gets a value whether the name of the corresponding field shall be shown.
     * This can be used for RowForms when we need a content cell which covers
     * the name column, but also the value column.
     */
    showNameField?(): boolean;
}

export class BaseField
{
    configuration: IFormConfiguration;
    field: Mof.DmObject;
    isReadOnly: boolean;
    form: IFormNavigation;
    itemUrl: string;
}