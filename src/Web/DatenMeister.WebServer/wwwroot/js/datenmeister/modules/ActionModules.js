import * as FormActions from "../FormActions.js";
import * as Mof from "../Mof.js";
import * as ActionClient from "../client/Actions.js";
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
    async execute(form, element, parameter, submitMethod) {
        alert(JSON.stringify(Mof.createJsonFromObject(element)));
    }
}
class ActionExecuteAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Action.Execute");
    }
    async execute(form, element, parameter, submitMethod) {
        // Executes the action directly
        const result = await ActionClient.executeAction(element.workspace, element.uri);
        if (result.success) {
            alert('Success: ' + result.result);
        }
        else {
            alert("Unfortunately, the action failed: \n\n" + result.reason);
        }
    }
}
//# sourceMappingURL=ActionModules.js.map