var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Interfaces"], function (require, exports, Interfaces_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        constructor(unknownFieldUri) {
            super();
            this.unknownFieldUri = unknownFieldUri;
        }
        createDom(dmElement) {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
                const result = $("<em></em>");
                result.text((_a = this.unknownFieldUri) !== null && _a !== void 0 ? _a : "unknown");
                return result;
            });
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=UnknownField.js.map