import * as Form from "../forms/ObjectForm.js";
import { loadDefaultModules } from "../modules/DefaultLoader.js";
import { ElementBreadcrumb } from "../controls/ElementBreadcrumb.js";
export function init(workspace, itemUri) {
    loadDefaultModules();
    const detailForm = new Form.ObjectFormCreatorForItem();
    detailForm.createForm({
        itemContainer: $("#form_view"),
        viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
        formSelectorContainer: $("#form_selection_container"),
        storeCurrentFormBtn: $("#dm-store-current-form-btn")
    }, workspace, itemUri);
    let breadcrumb = new ElementBreadcrumb($(".dm-breadcrumb-page"));
    const _ = breadcrumb.createForItem(workspace, itemUri);
}
//# sourceMappingURL=Item.js.map