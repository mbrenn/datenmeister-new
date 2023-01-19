import * as Settings from "./Settings";

export function getLinkForNavigateToWorkspaces() {
    return Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
}

export function navigateToWorkspaces() {
    document.location.href =
        getLinkForNavigateToWorkspaces();
}

export function getLinkForNavigateToWorkspace(workspace: string) {
    return Settings.baseUrl + "Item/Management/" +
        encodeURIComponent("dm:///_internal/workspaces#" + workspace);
}

export function navigateToWorkspace(workspace: string) {
    document.location.href =
        getLinkForNavigateToWorkspace(workspace);
}

export interface NavigationToExtentItemsParameter{
    /**
     * Metaclass to which the shown items will be filtered
     */
    metaClass?: string; 
}

export function getLinkForNavigateToExtentItems(workspace: string, extentUri: string, parameter?: NavigationToExtentItemsParameter) {
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

export function navigateToExtentItems(workspace: string, extentUri: string, parameter?: NavigationToExtentItemsParameter) {
    document.location.href =
        getLinkForNavigateToExtentItems(workspace, extentUri, parameter);
}

export function navigateToExtentProperties(workspace: string, extentUri: string) {
    document.location.href =
        Settings.baseUrl + "Item/Management/" +
        encodeURIComponent(workspace) + "_" +
        encodeURIComponent(extentUri);
}

export interface INavigateToItemParams
{
    /**
     * Defines whether the user shall move to the edit mode
     */
    editMode?: boolean;
}

export function getLinkForNavigateToItem(workspace: string, extentUri: string, itemId: string, param?: INavigateToItemParams) {
    return Settings.baseUrl + "Item/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(extentUri + "#" + itemId) +
        parseNavigateToItemParam(param);
}

export function navigateToItem(workspace: string, extentUri: string, itemId: string, param?: INavigateToItemParams) {
    document.location.href =
        getLinkForNavigateToItem(workspace, extentUri, itemId, param);
}

export function getLinkForNavigateToItemByUrl(workspace: string, itemUrl: string, param?: INavigateToItemParams) {
    return Settings.baseUrl + "Item/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(itemUrl) +
        parseNavigateToItemParam(param);
}

export function navigateToItemByUrl(workspace: string, itemUrl: string, param?: INavigateToItemParams) {
    document.location.href =
        getLinkForNavigateToItemByUrl(workspace, itemUrl, param);
}

function parseNavigateToItemParam(param? : INavigateToItemParams) {
    if (param === undefined) {
        return "";
    }

    let result = '';
    let ampersand = '?';
    if (param.editMode === true) {
        result += ampersand + "edit=true"
        ampersand = '&';
    }

    return result;
}

export function getLinkForNavigateToCreateNewItemInExtent(workspace: string, extentUri: string, metaclass: string) {
    return Settings.baseUrl +
        "ItemAction/Extent.CreateItem?workspace=" +
        encodeURIComponent(workspace) +
        "&extent=" +
        encodeURIComponent(extentUri) +
        "&metaclass=" +
        encodeURIComponent(metaclass);
}

export function navigateToCreateNewItemInExtent(workspace: string, extentUri: string, metaclass: string) {
    document.location.href = getLinkForNavigateToCreateNewItemInExtent(workspace, extentUri, metaclass);
}

export function getLinkForNavigateToAction(parameter: any, actionName: string, formUri: string) {
    let urlParameter = "";

    if (parameter !== undefined) {
        urlParameter = "?";
        let ampersand = "";

        for (let key in parameter) {
            var value = parameter[key];

            urlParameter += ampersand + encodeURIComponent(key) + "=" + encodeURIComponent(value);
            ampersand = "&";
        }
    }

    return `${Settings.baseUrl}ItemAction/${actionName}/${encodeURIComponent(formUri ?? "")}${urlParameter}`;
}

export function navigateToAction(actionName: string, formUri?: string, parameter?: any) {
    document.location.href = getLinkForNavigateToAction(parameter, actionName, formUri);
}

export function getLinkForNavigateToCreateItemInProperty(workspace: string, 
                                                         itemUrl: string,
                                                         metaclass: string,
                                                         propertyName: string) {
    return Settings.baseUrl +
        "ItemAction/Extent.CreateItemInProperty?workspace=" +
        encodeURIComponent(workspace) +
        "&itemUrl=" +
        encodeURIComponent(itemUrl) +
        "&metaclass=" +
        encodeURIComponent(metaclass) +
        "&property=" +
        encodeURIComponent(propertyName);
}


export function navigateToCreateItemInProperty(workspace: string,
                                                         itemUrl: string,
                                                         metaclass: string,
                                                         propertyName: string) {
    document.location.href = getLinkForNavigateToCreateItemInProperty(workspace, itemUrl, metaclass, propertyName);
}