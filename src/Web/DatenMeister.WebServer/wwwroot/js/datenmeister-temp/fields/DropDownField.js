import * as DropDownBaseField from "./DropDownBaseField.js";
/*
export class Field extends BaseField implements IFormField {
    private selectBox: JQuery<HTMLElement>;

    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
        const fieldName = this.field.get('name').toString();
        const selectedValue = dmElement.get(fieldName);
        const values = this.field.get('values') as Array<DmObject>;

        this.selectBox = $("<select></select>");

        if (Array.isArray(values)) {
            for (const value of values) {
                const option = $("<option></option>");
                option.val(value.get('value').toString());
                option.text(value.get('name').toString());
                this.selectBox.append(option);
            }
        } else {
            const option = $("<option></option>");
            option.text("No values given...");
            this.selectBox.append(option);
        }

        this.selectBox.val(selectedValue);
        if (this.isReadOnly) {
            this.selectBox.prop('disabled', 'disabled');
        }

        return this.selectBox;
    }

    async evaluateDom(dmElement: DmObject) : Promise<void> {
        const fieldName = this.field.get('name').toString();
        dmElement.set(fieldName, this.selectBox.val());
    }
}*/
export class Field extends DropDownBaseField.DropDownBaseField {
    constructor() {
        super();
        this.fieldType = DropDownBaseField.FieldType.Strings;
    }
    async loadFields() {
        const values = this.field.get('values');
        if (Array.isArray(values)) {
            return await Promise.resolve(values.map(x => {
                return {
                    title: x.get('name').toString(),
                    value: x.get('value').toString()
                };
            }));
        }
        else {
            return [];
        }
    }
}
//# sourceMappingURL=DropDownField.js.map