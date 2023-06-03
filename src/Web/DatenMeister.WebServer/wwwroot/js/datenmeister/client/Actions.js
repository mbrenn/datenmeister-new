import * as Settings from "../Settings.js";
import * as ApiConnection from "../ApiConnection.js";
import * as Mof from "../Mof.js";
export async function executeActionDirectly(actionName, parameter) {
    let url = Settings.baseUrl +
        "api/action/execute_directly/" +
        encodeURIComponent(actionName);
    return await ApiConnection.post(url, { parameter: Mof.createJsonFromObject(parameter.parameter) });
}
export async function executeAction(workspaceId, itemUri) {
    let url = Settings.baseUrl +
        "api/action/execute/" +
        encodeURIComponent(workspaceId) + "/" +
        encodeURIComponent(itemUri);
    return await ApiConnection.post(url, {});
}
//# sourceMappingURL=Actions.js.map