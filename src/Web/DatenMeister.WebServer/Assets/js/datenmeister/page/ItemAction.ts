import * as Form from "../forms/ActionForm.js"
import * as FormActions from "../FormActions.js"
import {loadDefaultModules} from "../actions/DefaultLoader.js";
import {FormType} from "../forms/Interfaces.js";

export async function init(actionName: string, metaClass: string, formUri: undefined | string) {
    loadDefaultModules();
    
    const module = await Form.createActionFormForEmptyObject(
        $("#form_view"),
        metaClass,  
        {isReadOnly: false, allowAddingNewProperties: true, formUri: formUri, formType: FormType.Object },
        actionName);
    
    if(module !== undefined) {
        window.document.title = "Action - '" + FormActions.getActionHeading(module) + "' - Der DatenMeister";
    }
    else
    {
        window.document.title = "Action - Unknown Action - Der DatenMeister";
    }
}