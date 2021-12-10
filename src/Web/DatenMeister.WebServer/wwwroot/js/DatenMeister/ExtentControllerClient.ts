import * as Mof from "./Mof";
import * as ApiConnection from "./ApiConnection"
import * as Settings from "./Settings"

export function setProperties(workspace: string, extentUri: string, properties:Mof.DmObject): JQuery.jqXHR
{
    return ApiConnection.post(
        Settings.baseUrl + "api/extent/set_properties/" + encodeURIComponent(workspace)+ "/" + encodeURIComponent(extentUri),
        Mof.createJsonFromObject(properties)
    );
}