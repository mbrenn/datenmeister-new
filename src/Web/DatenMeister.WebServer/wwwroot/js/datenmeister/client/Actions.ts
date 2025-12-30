import * as Settings from "../Settings.js"
import * as ApiConnection from "../ApiConnection.js"
import * as Mof from "../Mof.js";
import * as _DatenMeister from '../models/DatenMeister.class.js';

export interface ExecuteActionParams
{
    parameter: Mof.DmObject;    
}

export interface ExecuteActionResult
{
    success: boolean;
    reason: string;
    stackTrace: string;
    
    /**
     * Defines the result text which can be returned
     */
    result: string;
}

export interface ExecuteActionWithDmObjectResult
extends ExecuteActionResult{
    resultAsDmObject: Mof.DmObject;
}

export async function executeActionDirectly(actionName: string, parameter: ExecuteActionParams)
{
    let url = Settings.baseUrl +
        "api/action/execute_directly/" +
        encodeURIComponent(actionName);

    const result =  await ApiConnection.post<ExecuteActionResult>(
        url,
        {parameter: Mof.createJsonFromObject(parameter.parameter)});
    
    const resultAsDmObject: ExecuteActionWithDmObjectResult =
        {
            success: result.success,
            result: result.result,
            reason: result.reason,
            stackTrace: result.stackTrace,
            resultAsDmObject: Mof.convertJsonObjectToDmObject(result.result)
        };

    return resultAsDmObject;
}

export async function executeAction(workspaceId: string, itemUri: string) {
    let url = Settings.baseUrl +
        "api/action/execute/" +
        encodeURIComponent(workspaceId) + "/" +
        encodeURIComponent(itemUri);

    const result =
        await ApiConnection.post<ExecuteActionResult>(
            url, {});
    const resultAsDmObject: ExecuteActionWithDmObjectResult =
        {
            success: result.success,
            result: result.result,
            reason: result.reason,
            stackTrace: result.stackTrace,
            resultAsDmObject: Mof.convertJsonObjectToDmObject(result.result)
        };

    return resultAsDmObject;
}

export async function refreshTypeIndex(waitForRefresh: boolean) {
    const action = new Mof.DmObject(_DatenMeister._Actions.__RefreshTypeIndexAction_Uri);
    action.set(_DatenMeister._Actions._RefreshTypeIndexAction.waitForRefresh, waitForRefresh);

    const parameter: ExecuteActionParams =
        {
            parameter: action
        };

    await executeActionDirectly("Execute", parameter);
}