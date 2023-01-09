
import * as CollectionForm from "../forms/CollectionForm"
import {loadDefaultModules} from "../modules/DefaultLoader";
import {ElementBreadcrumb} from "../controls/ElementBreadcrumb";

export function init(workspace: string, extentUri: string) {
    loadDefaultModules();

    $("#items_collection_uri").text(extentUri);
    
    let listForm = new CollectionForm.CollectionFormCreator();
    listForm.createCollectionForRootElements(
        {
            itemContainer: $("#dm-items"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
            createNewItemWithMetaClassBtn: $("#dm-btn-create-item-with-metaclass"),
            createNewItemWithMetaClassContainer: $("#dm-btn-create-item-metaclass"),
            storeCurrentFormBtn: $("#dm-store-current-form-btn"),
            formSelectorContainer: $("#form_selection_container")
        },
        workspace,
        extentUri,
        {isReadOnly: true});
    
    let breadcrumb = new ElementBreadcrumb($(".dm-breadcrumb-page"));
    const _ = breadcrumb.createForExtent(workspace, extentUri);
    
}