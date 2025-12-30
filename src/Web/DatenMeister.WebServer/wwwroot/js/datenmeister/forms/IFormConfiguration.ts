import * as Mof from "../Mof.js";
import { SubmitMethod } from "./Forms.js";

export interface IFormConfiguration {
    allowAddingNewProperties?: boolean;
    isReadOnly?: boolean;
    isNewItem?: boolean;
    submitName?: string;
    showCancelButton?: boolean;
    formElement?: Mof.DmObject;

    // Form Uri to be set, if the caller wants to have an explicit form
    formUri?: string;
    
    onCancel?: () => void;
    onSubmit?: (element: Mof.DmObjectWithSync, method: SubmitMethod) => void;

    refreshForm?: () => Promise<void>;

    /**
     * Defines the view mode for the configuration
     */
    viewMode?: string;
}