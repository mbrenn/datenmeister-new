import { BaseField } from "./Interfaces.js";
export class Field extends BaseField {
    constructor(unknownFieldUri, context) {
        super(context);
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