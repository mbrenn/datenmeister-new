import { BaseField, IFormField } from "./Interfaces.js";
import {DmObject} from "../Mof.js";

export class Field extends BaseField implements IFormField {
    
    // Defines the name of the field name which is not known
    unknownFieldUri: string;

    constructor(unknownFieldUri:string) {
        super();
        this.unknownFieldUri = unknownFieldUri;
    }

    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
        const result = $("<em></em>");
        result.text(this.unknownFieldUri ?? "unknown");
        return result;
    }

    async evaluateDom(dmElement: DmObject) : Promise<void> {
    }
}