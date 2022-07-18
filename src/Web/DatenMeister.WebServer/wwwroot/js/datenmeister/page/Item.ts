
import * as Form from "../forms/ObjectForm"

export function init(workspace: string, itemUri: string) {
    const detailForm = new Form.ObjectFormCreatorForItem();
    detailForm.createForm(
        {
            itemContainer: $("#form_view"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container")
        },
        workspace,
        itemUri);
}