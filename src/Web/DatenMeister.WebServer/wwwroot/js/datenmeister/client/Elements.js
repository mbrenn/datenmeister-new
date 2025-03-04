import * as ApiConnection from "../ApiConnection.js";
import * as Settings from "../Settings.js";
import * as Mof from "../Mof.js";
import { convertToMofObjects } from "./Items.js";
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
    let url = "/api/elements/get_composites";
    if (workspaceId !== undefined && workspaceId !== null) {
        url += '/' + encodeURIComponent(workspaceId);
        if (itemUri !== undefined && itemUri !== null) {
            url += '/' + encodeURIComponent(itemUri);
        }
    }
    return await ApiConnection.get(url);
}
export async function loadNameOf(workspaceId, extentUri, itemUri) {
    return await ApiConnection.get(Settings.baseUrl +
        "api/elements/get_name/" +
        encodeURIComponent(workspaceId) + "/" +
        encodeURIComponent(extentUri) + "/" +
        encodeURIComponent(itemUri));
}
export async function loadNameByUri(workspaceId, elementUri) {
    if (workspaceId === undefined) {
        workspaceId = "_";
    }
    const getUrl = Settings.baseUrl +
        "api/elements/get_name/" +
        encodeURIComponent(workspaceId) + "/" +
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
export async function queryObject(query, timeout) {
    var json = Mof.createJsonFromObject(query);
    var parameters = {
        query: json
    };
    if (timeout !== undefined && timeout !== null && timeout > 0) {
        parameters.timeout = timeout;
    }
    var result = await ApiConnection.post(Settings.baseUrl +
        "api/elements/query_object", parameters);
    return {
        result: convertToMofObjects(result.result)
    };
}
//# sourceMappingURL=Elements.js.map