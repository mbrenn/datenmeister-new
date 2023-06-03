import * as Settings from "../Settings.js"
import * as ApiConnection from "../ApiConnection.js"
import * as Mof from "../Mof.js";

export interface ExecuteActionParams
{
    parameter: Mof.DmObject;    
}

export interface ExecuteActionResult
{
    success: boolean;
    reason: string;
    stackTrace: string;
}

export async function executeActionDirectly(actionName: string, parameter: ExecuteActionParams)
{
    let url = Settings.baseUrl +
        "api/action/execute_directly/" +
        encodeURIComponent(actionName);

    return await ApiConnection.post<ExecuteActionResult>(
        url,
        {parameter: Mof.createJsonFromObject(parameter.parameter)});
}

export async function executeAction(workspaceId: string, itemUri: string) {
    let url = Settings.baseUrl +
        "api/action/execute/" +
        encodeURIComponent(workspaceId) + "/" +
        encodeURIComponent(itemUri);

    return await ApiConnection.post<ExecuteActionResult>(
        url,
        {});
}