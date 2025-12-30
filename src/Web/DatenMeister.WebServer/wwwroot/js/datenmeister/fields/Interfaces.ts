import {IForm, IFormNavigation} from "../forms/Interfaces.js";
import * as Mof from "Mof.js";
import { IFormConfiguration } from "../forms/IFormConfiguration.js";

export interface IFormField
{
    /**
     * The configuration for the form
     */
    configuration: IFormConfiguration;

    /**
     * Defines the field definition (metamodel) used to create the UI for this field
     */
    field: Mof.DmObject;

    /**
     * Indicates whether the field is in read-only mode
     */
    isReadOnly: boolean;

    /**
     * The URL of the item to which this field is connected
     */
    itemUrl: string;
    
    /**
     * The navigation interface for the parent form
     */
    form: IFormNavigation;

    /**
     * Creates the DOM elements for the field based on the provided data element
     * @param dmElement The data element containing the values to be displayed
     * @returns A promise resolving to the JQuery object representing the field's DOM
     */
    createDom(dmElement: Mof.DmObject) : Promise<JQuery<HTMLElement>>;

    /**
     * Evaluates the current state of the DOM and injects the user's input back into the given data element
     * @param dmElement The data element where the changes will be stored
     */
    evaluateDom(dmElement: Mof.DmObject) : Promise<void>;

    /**
     * Gets a value indicating whether the name/label of the field should be shown separately.
     * This can be used for RowForms when a content cell needs to cover both the name and value columns.
     * @returns True if the name field should be shown, false otherwise
     */
    showNameField?(): boolean;

    /**
     * Gets a value indicating whether the value of the field is shown.
     * In case this field is being false, the checkbox (isset) shall be hidden.
     * @returns True if the value is shown, false otherwise
     */
    showValue?(): boolean;

    /**
     * This callback will be called in case of the content of the field is changed
     * This allows creator of a field to explicitly react upon changes within the field
     */
    callbackUpdateField?: () => void;
}

/**
 * Base class for field implementations, providing the standard properties of IFormField
 */
export class BaseField
{
    /**
     * The configuration for the form
     */
    configuration: IFormConfiguration;

    /**
     * Defines the field definition (metamodel)
     */
    field: Mof.DmObject;

    /**
     * Indicates whether the field is in read-only mode
     */
    isReadOnly: boolean;

    /**
     * The navigation interface for the parent form
     */
    form: IFormNavigation;

    /**
     * The URL of the item to which this field is connected
     */
    itemUrl: string;

    /**
     * Gets a value indicating whether the value of the field is shown.
     * Default implementation returns true.
     * @returns True
     */
    showValue(): boolean {
        return true;
    }

    /**
     * This callback will be called in case of the content of the field is changed
     * This allows creator of a field to explicitly react upon changes within the field
     * Default implementation does nothing.
     */
    callbackUpdateField()
    {
        return;
    }
}