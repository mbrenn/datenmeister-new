
import * as Form from "../forms/ObjectForm.js"
import {loadDefaultModules} from "../actions/DefaultLoader.js"
import {ElementBreadcrumb} from "../controls/ElementBreadcrumb.js";

export function init(workspace: string, itemUri: string) {
    loadDefaultModules();

    const objectForm = new Form.ObjectFormCreatorForItem({
        itemContainer: $("#form_view"),
        viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
        formSelectorContainer: $("#form_selection_container"),
        storeCurrentFormBtn: $("#dm-store-current-form-btn"),
        statusContainer: $(".dm-status-text-container")
    });
    
    const _ = objectForm.createForm(
        workspace,
        itemUri);

}