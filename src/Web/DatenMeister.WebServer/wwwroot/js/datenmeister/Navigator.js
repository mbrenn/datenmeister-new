import * as Settings from "./Settings.js";
export function getLinkForNavigateToWorkspaces() {
    return Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
}
export function navigateToWorkspaces() {
    document.location.href =
        getLinkForNavigateToWorkspaces();
}
export function getLinkForNavigateToWorkspace(workspace) {
    return Settings.baseUrl + "Item/Management/" +
        encodeURIComponent("dm:///_internal/workspaces#" + encodeURIComponent(workspace));
}
export function navigateToWorkspace(workspace) {
    document.location.href =
        getLinkForNavigateToWorkspace(workspace);
}
export function getLinkForNavigateToDefineAction(actionType) {
    return Settings.baseUrl + "Item/Management/" +
        encodeURIComponent("dm:///_internal/workspaces#" + actionType);
}
export function navigateToDefineAction(workspace) {
    document.location.href =
        getLinkForNavigateToWorkspace(workspace);
}
export function getLinkForNavigateToExtentItems(workspace, extentUri, parameter) {
    if (workspace === undefined || extentUri === undefined) {
        return null;
    }
    let urlParameter = "";
    let ampersand = '?';
    // Trim extentUri to remove the parameters
    const asUrl = new URL(extentUri);
    asUrl.searchParams.delete('metaclass');
    extentUri = asUrl.href;
    if (parameter?.metaClass !== undefined) {
        urlParameter += ampersand + "metaclass=" + encodeURIComponent(parameter.metaClass);
        ampersand = '&';
    }
    return Settings.baseUrl + "ItemsOverview/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(extentUri + urlParameter);
}
export function navigateToExtentItems(workspace, extentUri, parameter) {
    const link = getLinkForNavigateToExtentItems(workspace, extentUri, parameter);
    if (link !== null) {
        document.location.href = link;
    }
}
export function navigateToExtentProperties(workspace, extentUri) {
    document.location.href = getLinkForNavigateToExtentProperties(workspace, extentUri);
}
export function getLinkForNavigateToExtentProperties(workspace, extentUri) {
    return Settings.baseUrl + "Item/Management/" +
        encodeURIComponent("dm:///_internal/workspaces#" +
            encodeURIComponent(workspace).replace('_', '__') + "_" +
            encodeURIComponent(extentUri).replace('_', '__'));
}
export function getLinkForNavigateToMofItem(item, param) {
    return getLinkForNavigateToItemByUrl(item.workspace, item.uri, param);
}
export function getLinkForNavigateToItem(workspace, extentUri, itemId, param) {
    return Settings.baseUrl + "Item/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(extentUri + "#" + itemId) +
        parseNavigateToItemParam(param);
}
export function navigateToItem(workspace, extentUri, itemId, param) {
    document.location.href =
        getLinkForNavigateToItem(workspace, extentUri, itemId, param);
}
export function getLinkForNavigateToItemByUrl(workspace, itemUrl, param) {
    return Settings.baseUrl + "Item/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(itemUrl) +
        parseNavigateToItemParam(param);
}
export function navigateToItemByUrl(workspace, itemUrl, param) {
    document.location.href =
        getLinkForNavigateToItemByUrl(workspace, itemUrl, param);
}
function parseNavigateToItemParam(param) {
    if (param === undefined) {
        return "";
    }
    let result = '';
    let ampersand = '?';
    if (param.editMode === true) {
        result += ampersand + "edit=true";
        ampersand = '&';
    }
    // Adds the formUri to the result uri
    if (param.formUri !== undefined && param.formUri !== null && param.formUri !== "") {
        result += ampersand + "formUri=" + encodeURIComponent(param.formUri);
        ampersand = '&';
    }
    return result;
}
export function getLinkForNavigateToCreateNewItemInExtent(workspace, extentUri, metaclass, metaClassWorkspace) {
    return Settings.baseUrl +
        "ItemAction/Extent.CreateNewItem" +
        "?workspace=" + encodeURIComponent(workspace) +
        "&item=" + encodeURIComponent(extentUri) +
        (metaclass !== undefined
            ? "&metaclass=" + encodeURIComponent(metaclass)
            : "") +
        (metaClassWorkspace !== undefined
            ? "&metaclassworkspace=" + encodeURIComponent(metaClassWorkspace)
            : "");
}
export function navigateToCreateNewItemInExtent(workspace, extentUri, metaclass, metaClassWorkspace) {
    document.location.href = getLinkForNavigateToCreateNewItemInExtent(workspace, extentUri, metaclass, metaClassWorkspace);
}
export function getLinkForNavigateToAction(parameter, actionName, formUri) {
    let urlParameter = "";
    if (parameter !== undefined) {
        urlParameter = "?";
        let ampersand = "";
        for (let key in parameter) {
            const value = parameter[key];
            urlParameter += ampersand + encodeURIComponent(key) + "=" + encodeURIComponent(value);
            ampersand = "&";
        }
    }
    return `${Settings.baseUrl}ItemAction/${actionName}/${encodeURIComponent(formUri ?? "")}${urlParameter}`;
}
export function navigateToAction(actionName, formUri, parameter) {
    document.location.href = getLinkForNavigateToAction(parameter, actionName, formUri);
}
export function getLinkForNavigateToCreateItemInProperty(workspace, itemUrl, metaclass, metaclassWorkspace, propertyName) {
    return Settings.baseUrl +
        "ItemAction/Extent.CreateNewItem" +
        "?workspace=" + encodeURIComponent(workspace) +
        "&item=" + encodeURIComponent(itemUrl) +
        (metaclass !== undefined
            ? "&metaclass=" + encodeURIComponent(metaclass)
            : "") +
        (metaclassWorkspace !== undefined
            ? "&metaclassworkspace=" + encodeURIComponent(metaclassWorkspace)
            : "") +
        "&property=" + encodeURIComponent(propertyName);
}
export function navigateToCreateItemInProperty(workspace, itemUrl, metaclass, metaclassWorkspace, propertyName) {
    document.location.href =
        getLinkForNavigateToCreateItemInProperty(workspace, itemUrl, metaclass, metaclassWorkspace, propertyName);
}
//# sourceMappingURL=Navigator.js.map