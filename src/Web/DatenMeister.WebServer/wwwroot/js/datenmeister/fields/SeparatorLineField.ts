import {BaseField, IFormField} from "./Interfaces.js";
import {DmObject} from "../Mof.js";

export class Field extends BaseField implements IFormField
{
    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
        return $("<hr class='dm-separatorline'/>");
    }

    async evaluateDom(dmElement: DmObject) : Promise<void> {
    }
    
    showNameField(): boolean {
        return true;
    }
}