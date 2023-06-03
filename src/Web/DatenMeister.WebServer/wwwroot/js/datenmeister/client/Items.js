import * as Mof from "../Mof.js";
import * as Settings from "../Settings.js";
import * as ApiConnection from "../ApiConnection.js";
export async function createItemInExtent(workspaceId, extentUri, param) {
    const evaluatedParameter = {
        metaClass: param.metaClass,
        properties: undefined
    };
    if (param.properties !== undefined && param.properties !== null) {
        evaluatedParameter.properties = Mof.createJsonFromObject(param.properties);
    }
    return await ApiConnection.post(Settings.baseUrl + "api/items/create_in_extent/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(extentUri), evaluatedParameter);
}
export async function createItemAsChild(workspaceId, itemUri, param) {
    const evaluatedParameter = {
        metaClass: param.metaClass,
        property: param.property,
        asList: param.asList ?? false,
        properties: undefined
    };
    if (param.properties !== undefined && param.properties !== null) {
        evaluatedParameter.properties = Mof.createJsonFromObject(param.properties);
    }
    return await ApiConnection.post(Settings.baseUrl + "api/items/create_child/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(itemUri), evaluatedParameter);
}
export async function deleteRootElements(workspaceId, extentUri) {
    return await ApiConnection.deleteRequest(Settings.baseUrl + "api/items/delete_root_elements/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(extentUri), {});
}
export async function deleteItem(workspaceId, itemUri) {
    return await ApiConnection.deleteRequest(Settings.baseUrl + "api/items/delete/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(itemUri), {});
}
export async function deleteItemFromExtent(workspaceId, itemUrl) {
    return await ApiConnection.deleteRequest(Settings.baseUrl + "api/items/delete_from_extent/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(itemUrl), {});
}
export async function getObject(workspace, extent, id) {
    const resultFromServer = ApiConnection.get(Settings.baseUrl +
        "api/items/get/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extent) +
        "/" +
        encodeURIComponent(id));
    return Mof.convertJsonObjectToDmObject(resultFromServer);
}
export async function getObjectByUri(workspace, url) {
    try {
        const resultFromServer = await ApiConnection.get(Settings.baseUrl +
            "api/items/get/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(url));
        return Mof.convertJsonObjectToDmObject(resultFromServer);
    }
    catch (e) {
        return undefined;
    }
}
export async function getItemWithNameAndId(workspace, url) {
    try {
        const resultFromServer = await ApiConnection.get(Settings.baseUrl +
            "api/items/get_itemwithnameandid/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(url));
        return resultFromServer;
    }
    catch (e) {
        return undefined;
    }
}
export async function getRootElements(workspace, extentUri, parameter) {
    // Handle issue that empty urls cannot be resolved by ASP.Net, so we need to include a Workspace Name
    if (workspace === undefined || workspace === "" || workspace === null) {
        workspace = "Data";
    }
    let url = Settings.baseUrl +
        "api/items/get_root_elements/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri);
    // Checks, if there is a view node being attached
    if (parameter?.viewNode !== undefined) {
        url += "?viewNode=" + encodeURIComponent(parameter.viewNode);
    }
    const resultFromServer = await ApiConnection.get(url);
    const x = JSON.parse(resultFromServer);
    let result = new Array();
    for (let n in x) {
        if (Object.prototype.hasOwnProperty.call(x, n)) {
            const v = x[n];
            result.push(Mof.convertJsonObjectToDmObject(v));
        }
    }
    return result;
}
export async function getRootElementsAsItem(workspace, extentUri, parameter) {
    // Handle issue that empty urls cannot be resolved by ASP.Net, so we need to include a Workspace Name
    if (workspace === undefined || workspace === "" || workspace === null) {
        workspace = "Data";
    }
    let url = Settings.baseUrl +
        "api/items/get_root_elements_as_item/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri);
    // Checks, if there is a view node being attached
    if (parameter?.viewNode !== undefined) {
        url += "?viewNode=" + encodeURIComponent(parameter.viewNode);
    }
    const resultFromServer = await ApiConnection.get(url);
    return resultFromServer;
}
export async function getContainer(workspaceId, itemUri, self) {
    let uri = Settings.baseUrl + "api/items/get_container/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(itemUri);
    if (self === true) {
        uri += "?self=true";
    }
    return await ApiConnection.get(uri);
}
export async function setProperty(workspaceId, itemUrl, property, value) {
    let url = Settings.baseUrl +
        "api/items/set_property/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return await ApiConnection.put(url, { key: property, value: value });
}
export async function unsetProperty(workspaceId, itemUrl, property) {
    let url = Settings.baseUrl +
        "api/items/unset_property/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return await ApiConnection.post(url, { property: property });
}
export async function setPropertiesByStringValues(workspaceId, itemUrl, params) {
    let url = Settings.baseUrl +
        "api/items/set_properties/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return await ApiConnection.post(url, params);
}
export async function setProperties(workspaceId, itemUrl, properties) {
    let url = Settings.baseUrl +
        "api/items/set/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return await ApiConnection.put(url, Mof.createJsonFromObject(properties));
}
export async function getProperty(workspaceId, itemUrl, property) {
    let url = Settings.baseUrl +
        "api/items/get_property/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl) +
        "?property=" +
        encodeURIComponent(property);
    const result = await ApiConnection.get(url);
    return Mof.convertJsonObjectToObjects(result.v);
}
export async function setMetaclass(workspaceId, itemUrl, newMetaClass) {
    let url = Settings.baseUrl +
        "api/items/set_metaclass/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return await ApiConnection.post(url, { metaclass: newMetaClass });
}
export async function addReferenceToCollection(workspaceId, itemUrl, parameter) {
    let url = Settings.baseUrl +
        "api/items/add_ref_to_collection/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    await ApiConnection.post(url, {
        property: parameter.property,
        workspaceId: parameter.workspaceId,
        referenceUri: parameter.referenceUri
    });
}
export async function setPropertyReference(workspaceId, itemUrl, parameter) {
    let url = Settings.baseUrl +
        "api/items/set_property_reference/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return await ApiConnection.post(url, {
        property: parameter.property,
        workspaceId: parameter.workspaceId,
        referenceUri: parameter.referenceUri
    });
}
export async function removeReferenceFromCollection(workspaceId, itemUrl, parameter) {
    let url = Settings.baseUrl +
        "api/items/remove_ref_to_collection/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    await ApiConnection.post(url, {
        property: parameter.property,
        workspaceId: parameter.referenceWorkspaceId,
        referenceUri: parameter.referenceUri
    });
}
export class ExportXmiResult {
}
export async function exportXmi(workspace, itemUri) {
    let url = Settings.baseUrl +
        "api/item/export_xmi/"
        + encodeURIComponent(workspace) + "/"
        + encodeURIComponent(itemUri);
    return await ApiConnection.get(url);
}
export class ImportXmiResult {
}
export async function importXmi(workspace, itemUri, property, addToCollection, xmi) {
    let url = Settings.baseUrl +
        "api/item/import_xmi/"
        + encodeURIComponent(workspace) + "/"
        + encodeURIComponent(itemUri)
        + "?property=" + encodeURIComponent(property)
        + "&addToCollection=" + (addToCollection ? "true" : "false");
    return await ApiConnection.post(url, { xmi: xmi });
}
//# sourceMappingURL=Items.js.map