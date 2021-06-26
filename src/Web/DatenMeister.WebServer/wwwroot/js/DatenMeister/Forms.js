define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.TextField = exports.Form = void 0;
    class Form {
    }
    exports.Form = Form;
    class TextField {
        createDom(parent, dmElement) {
            var fieldName = this.Field['name'];
            this._textBox = $("<input />");
            this._textBox.val(dmElement.get(fieldName).toString());
        }
        evaluateDom(dmElement) {
        }
    }
    exports.TextField = TextField;
});
//# sourceMappingURL=Forms.js.map