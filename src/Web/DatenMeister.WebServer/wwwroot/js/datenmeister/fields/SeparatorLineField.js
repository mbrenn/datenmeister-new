define(["require", "exports", "./Interfaces"], function (require, exports, Interfaces_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        createDom(dmElement) {
            return $("<hr class='dm-separatorline'/>");
        }
        evaluateDom(dmElement) {
        }
        showNameField() {
            return true;
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=SeparatorLineField.js.map