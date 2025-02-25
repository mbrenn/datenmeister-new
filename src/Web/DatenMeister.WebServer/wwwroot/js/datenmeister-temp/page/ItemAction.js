import * as Form from "../forms/ActionForm.js";
import { loadDefaultModules } from "../actions/DefaultLoader.js";
export async function init(actionName, metaClass, formUri) {
    loadDefaultModules();
    await Form.createActionFormForEmptyObject($("#form_view"), metaClass, { isReadOnly: false, allowAddingNewProperties: true, formUri: formUri }, actionName);
}
//# sourceMappingURL=ItemAction.js.map