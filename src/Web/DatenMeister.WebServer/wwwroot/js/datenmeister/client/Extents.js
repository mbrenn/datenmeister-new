import * as Settings from "../Settings.js";
import * as ApiConnection from "../ApiConnection.js";
import * as Mof from "../Mof.js";
export async function exists(workspaceId, extent) {
    let url = Settings.baseUrl +
        "api/extent/exists/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(extent);
    return await ApiConnection.get(url);
}
export class ExportXmiResult {
}
export async function exportXmi(workspace, extentUri) {
    let url = Settings.baseUrl +
        "api/extent/export_xmi/"
        + encodeURIComponent(workspace) + "/"
        + encodeURIComponent(extentUri);
    return await ApiConnection.get(url);
}
export class ImportXmiResult {
}
export async function importXmi(workspace, extentUri, xmi) {
    let url = Settings.baseUrl +
        "api/extent/import_xmi/"
        + encodeURIComponent(workspace) + "/"
        + encodeURIComponent(extentUri);
    return await ApiConnection.post(url, { xmi: xmi });
}
export function createXmi(params) {
    return new Promise((resolve, reject) => {
        let url = Settings.baseUrl +
            "api/extent/create_xmi";
        ApiConnection.post(url, params).then((result) => {
            resolve(result);
        });
    });
}
export function deleteExtent(params) {
    return new Promise((resolve, reject) => {
        let url = Settings.baseUrl +
            "api/extent/delete";
        ApiConnection.deleteRequest(url, params).then((result) => {
            resolve(result);
        });
    });
}
export function clearExtent(params) {
    return new Promise((resolve, reject) => {
        let url = Settings.baseUrl +
            "api/extent/clear";
        ApiConnection.post(url, params).then((result) => {
            resolve(result);
        });
    });
}
export function setProperties(workspace, extentUri, properties) {
    return ApiConnection.post(Settings.baseUrl + "api/extent/set_properties/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri), Mof.createJsonFromObject(properties));
}
export function getProperties(workspace, extentUri) {
    return new Promise(resolve => {
        ApiConnection.get(Settings.baseUrl + "api/extent/get_properties/"
            + encodeURIComponent(workspace)
            + "/" + encodeURIComponent(extentUri))
            .then(x => {
            const dmObject = Mof.convertJsonObjectToDmObject(x);
            resolve(dmObject);
        });
    });
}
//# sourceMappingURL=Extents.js.map