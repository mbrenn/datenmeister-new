define(["require", "exports", "../DomHelper", "../Interfaces.Fields"], function (require, exports, DomHelper_1, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            var _a;
            const div = $("<div />");
            if (dmElement !== undefined && dmElement.metaClass !== undefined && dmElement.metaClass !== null) {
                if (dmElement.metaClass.uri !== null) {
                    div.text((_a = dmElement.metaClass.id) !== null && _a !== void 0 ? _a : dmElement.metaClass.uri);
                    (0, DomHelper_1.injectNameByUri)(div, encodeURIComponent(dmElement.metaClass.uri));
                }
                else if (dmElement.metaClass.id !== null && dmElement.metaClass.extentUri !== null) {
                    div.text(dmElement.metaClass.id);
                    (0, DomHelper_1.injectNameByUri)(div, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
                }
                else {
                    div.append($("<em>unknown</em>"));
                }
            }
            else {
                div.append($("<em>unknown</em>"));
            }
            return div;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=MetaClassElementField.js.map