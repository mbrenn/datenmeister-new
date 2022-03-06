import { BaseField, IFormField } from "../Interfaces.Fields";
import {DmObject} from "../Mof";

export class Field extends BaseField implements IFormField {
    
    // Defines the name of the field name which is not known
    unknownFieldName: string;
    
    constructor(unknownFieldName:string) {
        super();
        this.unknownFieldName = unknownFieldName;
    }
    
    createDom(dmElement: DmObject): JQuery<HTMLElement> {

        const result = $("<em></em>");
        result.text(this.unknownFieldName ?? "unknown");
        return result;
    }

    evaluateDom(dmElement: DmObject) {
    }
}