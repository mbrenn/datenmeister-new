import * as DropDownBaseField from "./DropDownBaseField.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
export class Field extends DropDownBaseField.DropDownBaseField {
    constructor() {
        super();
        this.fieldType = DropDownBaseField.FieldType.Strings;
    }
    async loadFields() {
        const values = this.field.get('values');
        if (Array.isArray(values)) {
            return values.map(x => {
                return {
                    title: x.get(_DatenMeister._Forms._ValuePair._name_).toString(),
                    value: x.get(_DatenMeister._Forms._ValuePair.value).toString()
                };
            });
        }
        else {
            return [];
        }
    }
}
//# sourceMappingURL=DropDownField.js.map