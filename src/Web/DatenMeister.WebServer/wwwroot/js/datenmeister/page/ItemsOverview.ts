
import * as Form from "../forms/CollectionForm"

export function init(workspace: string, extentUri: string)
{       
    let listForm = new Form.CollectionFormCreator();
    listForm.createCollectionForRootElements(
        {
            itemContainer: $("#dm-items"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
            createNewItemWithMetaClassBtn: $("#dm-btn-create-item-with-metaclass"),
            createNewItemWithMetaClassContainer: $("#dm-btn-create-item-metaclass"),
            storeCurrentFormBtn: $("dm-store-current-form-btn")
            
        },
        workspace, 
        extentUri,
        {isReadOnly: true});
}