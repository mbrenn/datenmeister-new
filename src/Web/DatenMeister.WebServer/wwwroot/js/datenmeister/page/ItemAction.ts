
import * as Form from "../forms/ActionForm"

export function init(actionName: string, metaClass: undefined | string, formUri: undefined | string) {
    Form.createActionFormForEmptyObject(
        $("#form_view"),
        metaClass,
        {isReadOnly: false, allowAddingNewProperties: true, formUri: formUri},
        actionName);
}