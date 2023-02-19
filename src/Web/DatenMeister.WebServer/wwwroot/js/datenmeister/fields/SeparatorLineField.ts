import {BaseField, IFormField} from "./Interfaces";
import {DmObject} from "../Mof";

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