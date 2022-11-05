
import * as Form from "../forms/ActionForm"
import {loadDefaultModules} from "../modules/DefaultLoader";

export async function init(actionName: string, metaClass: undefined | string, formUri: undefined | string) {
    loadDefaultModules();
    
    await Form.createActionFormForEmptyObject(
        $("#form_view"),
        metaClass,
        {isReadOnly: false, allowAddingNewProperties: true, formUri: formUri},
        actionName);
}