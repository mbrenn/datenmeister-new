import * as ApiConnection from "../ApiConnection.js";
import * as Settings from "../Settings.js";
import * as Mof from "../Mof.js";
export function getAllWorkspaces() {
    return load(undefined, undefined);
}
export function getAllExtents(workspaceId) {
    return load(workspaceId, undefined);
}
export function getAllRootItems(workspaceId, extentUri) {
    return load(workspaceId, extentUri);
}
export function getAllChildItems(workspaceId, itemUrl) {
    return load(workspaceId, itemUrl);
}
/*
 * Loads the items from the server.
 * The ItemUri may be an extent (then the root items will be loaded)
 * or may be an item (then the composite children will be loaded)
 */
async function load(workspaceId, itemUri) {
    let url = "/api/elements/get_composites?";
    if (workspaceId !== undefined && workspaceId !== null) {
        url += "w=" + encodeURIComponent(workspaceId);
        if (itemUri !== undefined && itemUri !== null) {
            url += "&u=" + encodeURIComponent(itemUri);
        }
    }
    else if (itemUri !== undefined && itemUri !== null) {
        url += "u=" + encodeURIComponent(itemUri);
    }
    return await ApiConnection.get(url);
}
export async function loadNameOf(workspaceId, extentUri, itemUri) {
    return await ApiConnection.get(Settings.baseUrl +
        "api/elements/get_name?w=" +
        encodeURIComponent(workspaceId) + "&e=" +
        encodeURIComponent(extentUri) + "&i=" +
        encodeURIComponent(itemUri));
}
export async function loadNameByUri(workspaceId, elementUri) {
    if (workspaceId === undefined) {
        workspaceId = "_";
    }
    const getUrl = Settings.baseUrl +
        "api/elements/get_name_by_uri?w=" +
        encodeURIComponent(workspaceId) + "&u=" +
        encodeURIComponent(elementUri);
    return await ApiConnection.get(getUrl);
}
export async function createTemporaryElement(metaClassUri) {
    return await ApiConnection.put(Settings.baseUrl +
        "api/elements/create_temporary_element", {
        metaClassUri: metaClassUri
    });
}
export function findBySearchString(searchString) {
    return ApiConnection.get(Settings.baseUrl +
        "api/elements/find_by_searchstring?search=" +
        encodeURIComponent(searchString));
}
/**
 * Calls the server to query the object according the provided parameters
 * @param query The query that is being used to query the object
 * @param parameters Additional parameters, the query object will be included into that element
 */
export async function queryObject(query, parameters) {
    if (parameters === undefined) {
        parameters = {};
    }
    if (parameters.query === undefined || parameters.query === null) {
        parameters.query = Mof.createJsonFromObject(query);
    }
    const resultFromServer = await ApiConnection.post(Settings.baseUrl +
        "api/elements/query_object", parameters);
    const result = new Array();
    for (const n in resultFromServer.result) {
        const v = resultFromServer.result[n];
        result.push(Mof.convertJsonObjectToDmObject(v));
    }
    return {
        result: result
    };
}
//# sourceMappingURL=Elements.js.map