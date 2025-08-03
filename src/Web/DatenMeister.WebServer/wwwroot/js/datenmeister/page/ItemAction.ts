
import * as Form from "../forms/ActionForm.js"
import * as FormActions from "../FormActions.js"
import {loadDefaultModules} from "../actions/DefaultLoader.js";

export async function init(actionName: string, metaClass: undefined | string, formUri: undefined | string) {
    loadDefaultModules();
    
    const module = await Form.createActionFormForEmptyObject(
        $("#form_view"),
        metaClass,  
        {isReadOnly: false, allowAddingNewProperties: true, formUri: formUri},
        actionName);
    
    window.document.title = "Action - '" + FormActions.getActionHeading(module) + "' - Der DatenMeister";
}