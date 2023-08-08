import * as Form from "../forms/ObjectForm.js";
import { loadDefaultModules } from "../modules/DefaultLoader.js";
export function init(workspace, itemUri) {
    loadDefaultModules();
    const detailForm = new Form.ObjectFormCreatorForItem({
        itemContainer: $("#form_view"),
        viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
        formSelectorContainer: $("#form_selection_container"),
        storeCurrentFormBtn: $("#dm-store-current-form-btn"),
        statusContainer: $(".dm-status-text-container")
    });
    const _ = detailForm.createForm(workspace, itemUri);
}
//# sourceMappingURL=Item.js.map