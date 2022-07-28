define(["require", "exports", "../forms/CollectionForm"], function (require, exports, Form) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.init = void 0;
    function init(workspace, extentUri) {
        let listForm = new Form.CollectionFormCreator();
        listForm.createCollectionForRootElements({
            itemContainer: $("#dm-items"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
            createNewItemWithMetaClassBtn: $("#dm-btn-create-item-with-metaclass"),
            createNewItemWithMetaClassContainer: $("#dm-btn-create-item-metaclass"),
            storeCurrentFormBtn: $("dm-store-current-form-btn"),
            formSelectorContainer: $("#form_selection_container")
        }, workspace, extentUri, { isReadOnly: true });
    }
    exports.init = init;
});
//# sourceMappingURL=ItemsOverview.js.map