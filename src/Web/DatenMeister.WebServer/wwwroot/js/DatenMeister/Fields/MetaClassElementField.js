define(["require", "exports", "../DomHelper", "../Interfaces.Fields"], function (require, exports, DomHelper_1, Interfaces_Fields_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        createDom(dmElement) {
            const div = $("<div />");
            if (dmElement.metaClass !== undefined && dmElement.metaClass !== null) {
                div.text(dmElement.metaClass.id);
            }
            else {
                div.text("unknown");
            }
            (0, DomHelper_1.injectNameByUri)(div, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
            return div;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=MetaClassElementField.js.map