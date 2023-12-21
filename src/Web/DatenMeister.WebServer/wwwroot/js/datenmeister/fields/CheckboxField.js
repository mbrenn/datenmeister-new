import { BaseField } from "./Interfaces.js";
import { ObjectType } from "../Mof.js";
export class Field extends BaseField {
    async createDom(dmElement) {
        this._checkbox = $("<input type='checkbox'/>");
        const fieldName = this.field.get('name').toString();
        if (dmElement.get(fieldName, ObjectType.Boolean)) {
            this._checkbox.prop('checked', true);
        }
        if (this.isReadOnly) {
            this._checkbox.prop('disabled', 'disabled');
        }
        return this._checkbox;
    }
    async evaluateDom(dmElement) {
        if (this._checkbox !== undefined && this._checkbox !== null) {
            const fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._checkbox.prop('checked'));
        }
    }
}
//# sourceMappingURL=CheckboxField.js.map