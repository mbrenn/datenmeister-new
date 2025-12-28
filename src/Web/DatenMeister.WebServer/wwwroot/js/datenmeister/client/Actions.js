import * as Settings from "../Settings.js";
import * as ApiConnection from "../ApiConnection.js";
import * as Mof from "../Mof.js";
import * as _DatenMeister from '../models/DatenMeister.class.js';
export async function executeActionDirectly(actionName, parameter) {
    let url = Settings.baseUrl +
        "api/action/execute_directly/" +
        encodeURIComponent(actionName);
    const result = await ApiConnection.post(url, { parameter: Mof.createJsonFromObject(parameter.parameter) });
    const resultAsDmObject = {
        success: result.success,
        result: result.result,
        reason: result.reason,
        stackTrace: result.stackTrace,
        resultAsDmObject: Mof.convertJsonObjectToDmObject(result.result)
    };
    return resultAsDmObject;
}
export async function executeAction(workspaceId, itemUri) {
    let url = Settings.baseUrl +
        "api/action/execute/" +
        encodeURIComponent(workspaceId) + "/" +
        encodeURIComponent(itemUri);
    const result = await ApiConnection.post(url, {});
    const resultAsDmObject = {
        success: result.success,
        result: result.result,
        reason: result.reason,
        stackTrace: result.stackTrace,
        resultAsDmObject: Mof.convertJsonObjectToDmObject(result.result)
    };
    return resultAsDmObject;
}
export async function refreshTypeIndex(waitForRefresh) {
    const action = new Mof.DmObject(_DatenMeister._Actions.__RefreshTypeIndexAction_Uri);
    action.set(_DatenMeister._Actions._RefreshTypeIndexAction.waitForRefresh, waitForRefresh);
    const parameter = {
        parameter: action
    };
    await executeActionDirectly("Execute", parameter);
}
//# sourceMappingURL=Actions.js.map