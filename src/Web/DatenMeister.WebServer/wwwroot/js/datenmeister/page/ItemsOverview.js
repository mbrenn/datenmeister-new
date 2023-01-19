define(["require", "exports", "../forms/CollectionForm", "../modules/DefaultLoader", "../controls/ElementBreadcrumb"], function (require, exports, CollectionForm, DefaultLoader_1, ElementBreadcrumb_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.init = void 0;
    function init(workspace, extentUri) {
        (0, DefaultLoader_1.loadDefaultModules)();
        $("#items_collection_uri").text(extentUri);
        let listForm = new CollectionForm.CollectionFormCreator();
        listForm.createCollectionForRootElements({
            itemContainer: $("#dm-items"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
            createNewItemWithMetaClassBtn: $("#dm-btn-create-item-with-metaclass"),
            createNewItemWithMetaClassContainer: $("#dm-btn-create-item-metaclass"),
            storeCurrentFormBtn: $("#dm-store-current-form-btn"),
            formSelectorContainer: $("#form_selection_container")
        }, workspace, extentUri, { isReadOnly: true });
        let breadcrumb = new ElementBreadcrumb_1.ElementBreadcrumb($(".dm-breadcrumb-page"));
        const _ = breadcrumb.createForExtent(workspace, extentUri);
    }
    exports.init = init;
});
//# sourceMappingURL=ItemsOverview.js.map