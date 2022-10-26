define(["require", "exports", "../forms/ActionForm", "../modules/DefaultLoader"], function (require, exports, Form, DefaultLoader_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.init = void 0;
    function init(actionName, metaClass, formUri) {
        (0, DefaultLoader_1.loadDefaultModules)();
        Form.createActionFormForEmptyObject($("#form_view"), metaClass, { isReadOnly: false, allowAddingNewProperties: true, formUri: formUri }, actionName);
    }
    exports.init = init;
});
//# sourceMappingURL=ItemAction.js.map