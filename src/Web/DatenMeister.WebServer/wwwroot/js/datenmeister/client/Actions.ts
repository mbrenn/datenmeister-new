import * as Settings from "../Settings"
import * as ApiConnection from "../ApiConnection"
import * as Mof from "../Mof";

export interface ExecuteActionParams
{
    parameter: Mof.DmObject;    
}

export interface ExecuteActionResult
{
    success: boolean;
    reason: string;
}

export async function executeAction(actionName: string, parameter: ExecuteActionParams)
{
    let url = Settings.baseUrl +
        "api/action/" +
        encodeURIComponent(actionName);

    return await ApiConnection.post<ExecuteActionResult>(
        url,
        {parameter: Mof.createJsonFromObject(parameter.parameter)});
}