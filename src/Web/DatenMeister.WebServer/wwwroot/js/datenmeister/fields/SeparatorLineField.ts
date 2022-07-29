import {BaseField, IFormField} from "./Interfaces";
import {DmObject} from "../Mof";

export class Field extends BaseField implements IFormField
{
    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        return $("<hr class='dm-separatorline'/>");
    }

    evaluateDom(dmElement: DmObject) {
    }
    
    showNameField(): boolean {
        return true;
    }
}