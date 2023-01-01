
import * as Form from "../forms/ObjectForm"
import {loadDefaultModules} from "../modules/DefaultLoader"
import {ElementBreadcrumb} from "../controls/ElementBreadcrumb";

export function init(workspace: string, itemUri: string) {
    loadDefaultModules();
    
    const detailForm = new Form.ObjectFormCreatorForItem();
    detailForm.createForm(
        {
            itemContainer: $("#form_view"),
            viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
            formSelectorContainer: $("#form_selection_container"),
            storeCurrentFormBtn: $("#dm-store-current-form-btn")
        },
        workspace,
        itemUri);

    let breadcrumb = new ElementBreadcrumb($(".dm-breadcrumb-page"));
    const _ = breadcrumb.createForItem(workspace, itemUri);
}