
import * as Form from "../forms/CollectionForm"

export function init(workspace: string, extentUri: string)
{
    let p = new URLSearchParams(window.location.search);

    
    
    let listForm = new Form.CollectionFormCreator();
    listForm.createCollectionForRootElements(
        {
            itemContainer: $("#dm-items"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
            createNewItemWithMetaClassBtn: $("#dm-btn-create-item-with-metaclass"),
            createNewItemWithMetaClassContainer: $("#dm-btn-create-item-metaclass")
        },
        workspace, 
        extentUri,
        {isReadOnly: true});
}