import {IFormField} from "./Interfaces.js";
import { DmObject } from "../Mof.js";
import * as DropDownBaseField from "./DropDownBaseField.js";
import {_DatenMeister} from "../models/DatenMeister.class.js";

export class Field extends DropDownBaseField.DropDownBaseField implements IFormField {
    constructor() {
        super();
        this.fieldType = DropDownBaseField.FieldType.Strings;
    }
    
    async loadFields(): Promise<DropDownBaseField.DropDownOptionField[]> {
        const values = this.field.get('values') as Array<DmObject>;
        if (Array.isArray(values)) {
            return values.map(x => {
                return {
                    title: x.get(_DatenMeister._Forms._ValuePair._name_).toString(),
                    value: x.get(_DatenMeister._Forms._ValuePair.value).toString()
                };
            });
        } else {
            return [];
        }
    }
}