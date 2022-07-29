import { BaseField, IFormField } from "./Interfaces";
import {DmObject} from "../Mof";

export class Field extends BaseField implements IFormField {
    
    // Defines the name of the field name which is not known
    unknownFieldUri: string;
    
    constructor(unknownFieldUri:string) {
        super();
        this.unknownFieldUri = unknownFieldUri;
    }
    
    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        const result = $("<em></em>");
        result.text(this.unknownFieldUri ?? "unknown");
        return result;
    }

    evaluateDom(dmElement: DmObject) {
    }
}