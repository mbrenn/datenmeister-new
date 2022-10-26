define(["require", "exports", "../forms/ObjectForm", "../modules/DefaultLoader"], function (require, exports, Form, DefaultLoader_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.init = void 0;
    function init(workspace, itemUri) {
        (0, DefaultLoader_1.loadDefaultModules)();
        const detailForm = new Form.ObjectFormCreatorForItem();
        detailForm.createForm({
            itemContainer: $("#form_view"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
            formSelectorContainer: $("#form_selection_container")
        }, workspace, itemUri);
    }
    exports.init = init;
});
//# sourceMappingURL=Item.js.map