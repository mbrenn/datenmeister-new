import * as Mof from "../Mof.js";
import * as Settings from "../Settings.js";
import * as ApiConnection from "../ApiConnection.js";
/*
    Gets the default form for an extent uri by the webserver
 */
export async function getCollectionFormForExtent(workspace, extentUri, viewMode) {
    const resultFromServer = await ApiConnection.get(Settings.baseUrl +
        "api/forms/default_for_extent/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri) +
        "/" +
        encodeURIComponent(viewMode));
    return Mof.convertJsonObjectToDmObject(resultFromServer);
}
/*
    Gets the default form for an extent uri by the webserver
 */
export async function getDefaultObjectForMetaClass(metaClassUri, viewMode) {
    if (metaClassUri === undefined || metaClassUri === null || metaClassUri === "") {
        metaClassUri = "_";
    }
    const viewModeUri = viewMode === undefined || viewMode === "" ?
        "" :
        "/" + encodeURI(viewMode);
    const resultFromServer = await ApiConnection.get(Settings.baseUrl +
        "api/forms/default_object_for_metaclass/" +
        encodeURIComponent(metaClassUri) +
        viewModeUri);
    return Mof.convertJsonObjectToDmObject(resultFromServer);
}
export async function getForm(formUri, formType) {
    const resultFromServer = await ApiConnection.get(Settings.baseUrl +
        "api/forms/get/" +
        encodeURIComponent(formUri) +
        (formType === undefined ? "" : "?formtype=" + encodeURIComponent(formType)));
    return Mof.convertJsonObjectToDmObject(resultFromServer);
}
/*
    Gets the default form for a certain item by the webserver
 */
export async function getObjectFormForItem(workspace, item, viewMode) {
    const resultFromServer = await ApiConnection.get(Settings.baseUrl +
        "api/forms/default_for_item/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(item) +
        "/" +
        encodeURIComponent(viewMode));
    return Mof.convertJsonObjectToDmObject(resultFromServer);
}
export async function createCollectionFormForExtent(workspace, extentUri, viewMode) {
    return await ApiConnection.post(Settings.baseUrl +
        "api/forms/create_collection_form_for_extent/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri) +
        "/" +
        encodeURIComponent(viewMode ?? ""), {});
}
export async function createObjectFormForItem(workspace, extentUri, viewMode) {
    return await ApiConnection.post(Settings.baseUrl +
        "api/forms/create_object_form_for_item/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri) +
        "/" +
        encodeURIComponent(viewMode ?? ""), {});
}
export async function getViewModes() {
    const resultFromServer = await ApiConnection.get(Settings.baseUrl +
        "api/forms/get_viewmodes");
    const result = {
        viewModes: new Array()
    };
    for (let n in resultFromServer.viewModes) {
        const value = resultFromServer.viewModes[n];
        result.viewModes.push(Mof.convertJsonObjectToDmObject(value));
    }
    return result;
}
export async function getDefaultViewMode(workspace, extentUri) {
    const apiResult = await ApiConnection.get(Settings.baseUrl +
        "api/forms/get_default_viewmode/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri));
    return {
        viewMode: Mof.convertJsonObjectToDmObject(apiResult.viewMode)
    };
}
//# sourceMappingURL=Forms.js.map