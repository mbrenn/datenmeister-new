define(["require", "exports", "../forms/ObjectForm"], function (require, exports, Form) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.init = void 0;
    function init(workspace, itemUri) {
        const detailForm = new Form.ObjectFormCreatorForItem();
        detailForm.createForm({
            itemContainer: $("#form_view"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container")
        }, workspace, itemUri);
    }
    exports.init = init;
});
//# sourceMappingURL=Item.js.map