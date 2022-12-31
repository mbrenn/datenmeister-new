import * as FormActions from "../FormActions"
import {createJsonFromObject, DmObject} from "../Mof";
import {SubmitMethod} from "../forms/RowForm";
import {IFormNavigation} from "../forms/Interfaces";
import * as ActionClient from "../client/Actions";


export function loadModules() {
    FormActions.addModule(new JsonItemAlertAction());
    FormActions.addModule(new ActionExecuteAction());
}

class JsonItemAlertAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("JSON.Item.Alert");
        this.actionVerb = "Trigger JSON Alert";
        this.skipSaving = true;
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        alert(JSON.stringify(createJsonFromObject(element)));
    }
}

class ActionExecuteAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Action.Execute");
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        // Executes the action directly
        const result = await ActionClient.executeAction(
            element.workspace,
            element.uri
        );

        if (result.success) {
            alert('Success');
        } else {
            alert("Unfortunately, the action failed: \n\n" + result.reason);
        }
    }
}