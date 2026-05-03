import { BaseField } from "./Interfaces.js";
export class Field extends BaseField {
    async createDom(dmElement) {
        return $("<hr class='dm-separatorline'/>");
    }
    async evaluateDom(dmElement) {
    }
    showNameField() {
        return true;
    }
    showValue() {
        return false;
    }
}
//# sourceMappingURL=SeparatorLineField.js.map