import * as Mof from "./Mof";
import * as ApiConnection from "./ApiConnection"
import * as Settings from "./Settings"

export function setProperties(workspace: string, extentUri: string, properties: Mof.DmObject): JQuery.jqXHR {
    return ApiConnection.post(
        Settings.baseUrl + "api/extent/set_properties/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri),
        Mof.createJsonFromObject(properties)
    );
}

export function getProperties(workspace: string, extentUri: string): JQuery.Deferred<Mof.DmObject, never, never> {
    const r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<object>(
        Settings.baseUrl + "api/extent/get_properties/"
        + encodeURIComponent(workspace)
        + "/" + encodeURIComponent(extentUri))
        .done(x => {
            const dmObject =
                Mof.convertJsonObjectToDmObject(x);
            r.resolve(dmObject);
        })

    return r;
}