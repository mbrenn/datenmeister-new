import {BaseField, IFieldRenderContext, IFormField} from "./Interfaces.js";
import * as Mof from "../Mof.js";

export class Field extends BaseField implements IFormField
{
    constructor(context: IFieldRenderContext) {
        super(context);
    }

    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {
        return $("<hr class='dm-separatorline'/>");
    }

    async evaluateDom(dmElement: Mof.DmObject) : Promise<void> {
    }
    
    showNameField(): boolean {
        return true;
    }

    showValue(): boolean {
        return false;
    }
}
