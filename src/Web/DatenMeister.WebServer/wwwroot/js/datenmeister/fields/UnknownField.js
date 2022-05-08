define(["require", "exports", "./Interfaces"], function (require, exports, Interfaces_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        constructor(unknownFieldName) {
            super();
            this.unknownFieldName = unknownFieldName;
        }
        createDom(dmElement) {
            var _a;
            const result = $("<em></em>");
            result.text((_a = this.unknownFieldName) !== null && _a !== void 0 ? _a : "unknown");
            return result;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=UnknownField.js.map