import {DmObject} from "../Mof";
import {SubmitMethod} from "./DetailForm";

export interface IFormConfiguration {
    allowAddingNewProperties?: boolean;
    isReadOnly?: boolean;
    isNewItem?: boolean;

    // Form Uri to be set, if the caller wants to have an explicit form
    formUri?: string;
    
    onCancel?: () => void;
    onSubmit?: (element: DmObject, method: SubmitMethod) => void;

    refreshForm?: () => void;
}