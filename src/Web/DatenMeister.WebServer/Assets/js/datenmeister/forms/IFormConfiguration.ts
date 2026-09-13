import * as Mof from "../Mof.js";
import {SubmitMethod} from "./Forms.js";
import {FormType, IPageNavigation} from "./Interfaces.js";

/**
 * Options for configuring form display, actions, and callbacks
 */
export interface IFormOptions {
    isReadOnly: boolean;
    isNewItem?: boolean;
    allowAddingNewProperties?: boolean;
    submitName?: string;
    showCancelButton?: boolean;
    onCancel?: () => void;
    onSubmit?: (element: Mof.DmObjectWithSync, method: SubmitMethod) => void;
    refreshForm?: () => Promise<void>;
    viewMode?: string;
}

/**
 * Execution context for the form containing data source and navigational metadata
 */
export interface IFormContext {
    workspace: string;
    extentUri?: string;
    itemUrl?: string;
    formElement?: Mof.DmObject;
    pageNavigation?: IPageNavigation;
    viewMode?: string;
    formType?: FormType;
    formUri?: string;
}

/**
 * Combined configuration interface for backward-compatibility and complete form configuration
 */
export interface IFormConfiguration extends IFormOptions {
    formType: FormType;
    formElement?: Mof.DmObject;
    formUri?: string;
    workspace?: string;
    extentUri?: string;
    itemUrl?: string;
}

/**
 * Defines the configuration for a decoupled form
 */
export interface IDecoupledFormConfiguration
{
    form: Mof.DmObject;
}
