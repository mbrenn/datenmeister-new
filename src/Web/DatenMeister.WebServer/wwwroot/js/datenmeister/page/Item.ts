
import * as Form from "../forms/ObjectForm"
import {loadDefaultModules} from "../modules/DefaultLoader"

export function init(workspace: string, itemUri: string) {
    loadDefaultModules();
    
    const detailForm = new Form.ObjectFormCreatorForItem();
    detailForm.createForm(
        {
            itemContainer: $("#form_view"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
            formSelectorContainer: $("#form_selection_container")
        },
        workspace,
        itemUri);
}