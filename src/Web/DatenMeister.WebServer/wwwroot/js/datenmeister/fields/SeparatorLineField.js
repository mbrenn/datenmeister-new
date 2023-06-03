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
}
//# sourceMappingURL=SeparatorLineField.js.map