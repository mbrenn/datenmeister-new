import * as CollectionForm from "../forms/CollectionForm.js";
import { loadDefaultModules } from "../modules/DefaultLoader.js";
export async function init(workspace, extentUri) {
    loadDefaultModules();
    let listForm = new CollectionForm.CollectionFormCreator({
        itemContainer: $("#dm-items"),
        viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
        createNewItemWithMetaClassBtn: $("#dm-btn-create-item-with-metaclass"),
        createNewItemWithMetaClassContainer: $("#dm-btn-create-item-metaclass"),
        storeCurrentFormBtn: $("#dm-store-current-form-btn"),
        formSelectorContainer: $("#form_selection_container")
    });
    await listForm.createCollectionForRootElements(workspace, extentUri, { isReadOnly: true });
}
//# sourceMappingURL=ItemsOverview.js.map