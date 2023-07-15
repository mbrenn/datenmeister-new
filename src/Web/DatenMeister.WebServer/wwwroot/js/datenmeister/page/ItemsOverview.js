import * as CollectionForm from "../forms/CollectionForm.js";
import { loadDefaultModules } from "../modules/DefaultLoader.js";
import { ElementBreadcrumb } from "../controls/ElementBreadcrumb.js";
export async function init(workspace, extentUri) {
    loadDefaultModules();
    $("#items_collection_uri").text(extentUri);
    let listForm = new CollectionForm.CollectionFormCreator();
    await listForm.createCollectionForRootElements({
        itemContainer: $("#dm-items"),
        viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
        createNewItemWithMetaClassBtn: $("#dm-btn-create-item-with-metaclass"),
        createNewItemWithMetaClassContainer: $("#dm-btn-create-item-metaclass"),
        storeCurrentFormBtn: $("#dm-store-current-form-btn"),
        formSelectorContainer: $("#form_selection_container")
    }, workspace, extentUri, { isReadOnly: true });
    let breadcrumb = new ElementBreadcrumb($(".dm-breadcrumb-page"));
    const _ = breadcrumb.createForExtent(workspace, extentUri);
}
//# sourceMappingURL=ItemsOverview.js.map