
import * as Form from "../forms/ObjectForm.js"
import {loadDefaultModules} from "../actions/DefaultLoader.js"
import * as UmlHelper from "./../UmlHelper.js"

export async function init(workspace: string, itemUri: string) {
    loadDefaultModules();

    const objectForm = new Form.ObjectFormCreatorForItem({
        itemContainer: $("#form_view"),
        viewModeSelectorContainer: $("#dm-viewmode-selection-container"),
        formSelectorContainer: $("#form_selection_container"),
        storeCurrentFormBtn: $("#dm-store-current-form-btn"),
        statusContainer: $(".dm-status-text-container")
    });

    const _ = await objectForm.createForm(
        workspace,
        itemUri);

    const name = await UmlHelper.NamedElement.getName(
        objectForm.element);
    if (name !== undefined) {
        window.document.title = "Item - '" + name + "' - Der DatenMeister";
    }
}