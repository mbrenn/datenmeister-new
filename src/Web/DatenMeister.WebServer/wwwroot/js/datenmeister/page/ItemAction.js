define(["require", "exports", "../forms/ActionForm"], function (require, exports, Form) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.init = void 0;
    function init(actionName, metaClass, formUri) {
        Form.createActionFormForEmptyObject($("#form_view"), metaClass, { isReadOnly: false, allowAddingNewProperties: true, formUri: formUri }, actionName);
    }
    exports.init = init;
});
//# sourceMappingURL=ItemAction.js.map