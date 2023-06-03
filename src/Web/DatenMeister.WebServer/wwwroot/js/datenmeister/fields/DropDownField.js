import { BaseField } from "./Interfaces.js";
export class Field extends BaseField {
    async createDom(dmElement) {
        const fieldName = this.field.get('name').toString();
        const selectedValue = dmElement.get(fieldName);
        const values = this.field.get('values');
        this.selectBox = $("<select></select>");
        if (Array.isArray(values)) {
            for (const value of values) {
                const option = $("<option></option>");
                option.val(value.get('value').toString());
                option.text(value.get('name').toString());
                this.selectBox.append(option);
            }
        }
        else {
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
    async evaluateDom(dmElement) {
        const fieldName = this.field.get('name').toString();
        dmElement.set(fieldName, this.selectBox.val());
    }
}
//# sourceMappingURL=DropDownField.js.map