define(["require", "exports", "../forms/ObjectForm", "../modules/DefaultLoader", "../controls/ElementBreadcrumb"], function (require, exports, Form, DefaultLoader_1, ElementBreadcrumb_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.init = void 0;
    function init(workspace, itemUri) {
        (0, DefaultLoader_1.loadDefaultModules)();
        const detailForm = new Form.ObjectFormCreatorForItem();
        detailForm.createForm({
            itemContainer: $("#form_view"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
            formSelectorContainer: $("#form_selection_container"),
            storeCurrentFormBtn: $("#dm-store-current-form-btn")
        }, workspace, itemUri);
        let breadcrumb = new ElementBreadcrumb_1.ElementBreadcrumb($(".dm-breadcrumb-page"));
        const _ = breadcrumb.createForItem(workspace, itemUri);
    }
    exports.init = init;
});
//# sourceMappingURL=Item.js.map