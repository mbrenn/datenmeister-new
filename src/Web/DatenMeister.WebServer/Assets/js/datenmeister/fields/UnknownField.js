import { BaseField } from "./Interfaces.js";
export class Field extends BaseField {
    // Defines the name of the field name which is not known
    unknownFieldUri;
    constructor(unknownFieldUri) {
        super();
        this.unknownFieldUri = unknownFieldUri;
    }
    async createDom(dmElement) {
        const result = $("<em></em>");
        result.text(this.unknownFieldUri ?? "unknown");
        return result;
    }
    async evaluateDom(dmElement) {
    }
}
//# sourceMappingURL=UnknownField.js.map