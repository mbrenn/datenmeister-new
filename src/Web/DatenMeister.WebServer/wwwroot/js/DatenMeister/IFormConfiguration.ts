import {DmObject} from "./Mof";

export interface IFormConfiguration {
    allowAddingNewProperties?: boolean;
    isReadOnly?: boolean;
    
    onCancel?: () => void;
    onSubmit?: (element: DmObject) => void;
}