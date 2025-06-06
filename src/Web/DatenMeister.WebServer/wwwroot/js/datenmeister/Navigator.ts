import * as Settings from "./Settings.js";
import * as Mof from "./Mof";

export function getLinkForNavigateToWorkspaces() {
    return Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
}

export function navigateToWorkspaces() {
    document.location.href =
        getLinkForNavigateToWorkspaces();
}

export function getLinkForNavigateToWorkspace(workspace: string) {
    return Settings.baseUrl + "Item/Management/" +
        encodeURIComponent("dm:///_internal/workspaces#" + encodeURIComponent(workspace));
}

export function navigateToWorkspace(workspace: string) {
    document.location.href =
        getLinkForNavigateToWorkspace(workspace);
}


export function getLinkForNavigateToDefineAction(actionType: string) {
    return Settings.baseUrl + "Item/Management/" +
        encodeURIComponent("dm:///_internal/workspaces#" + actionType);
}

export function navigateToDefineAction(workspace: string) {
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
    document.location.href = getLinkForNavigateToExtentProperties(workspace, extentUri);
}

export function getLinkForNavigateToExtentProperties(workspace: string, extentUri: string) {
    return Settings.baseUrl + "Item/Management/" +
        encodeURIComponent("dm:///_internal/workspaces#" +
            encodeURIComponent(workspace).replace('_', '__') + "_" +
            encodeURIComponent(extentUri).replace('_', '__'));
}

export interface INavigateToItemParams
{
    /**
     * Defines whether the user shall move to the edit mode
     */
    editMode?: boolean;

    /**
     * Defines the form uri to be used to show the item itself
     */
    formUri?: string;
}

export function getLinkForNavigateToMofItem(item: Mof.DmObject, param?: INavigateToItemParams) {
    return getLinkForNavigateToItemByUrl(item.workspace, item.uri, param);
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

    // Adds the formUri to the result uri
    if (param.formUri !== undefined && param.formUri !== null && param.formUri !== "") {
        result += ampersand + "formUri=" + encodeURIComponent(param.formUri);
        ampersand = '&';
    }

    return result;
}

export function getLinkForNavigateToCreateNewItemInExtent(workspace: string,
                                                          extentUri: string,
                                                          metaclass: string,
                                                          metaClassWorkspace: string) {
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
export function navigateToCreateNewItemInExtent(workspace: string, 
                                                extentUri: string,
                                                metaclass: string,
                                                metaClassWorkspace?: string) {
    document.location.href = getLinkForNavigateToCreateNewItemInExtent(
        workspace, extentUri, metaclass, metaClassWorkspace);
}

export function getLinkForNavigateToAction(parameter: any, actionName: string, formUri: string) {
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

export function navigateToAction(actionName: string, formUri?: string, parameter?: any) {
    document.location.href = getLinkForNavigateToAction(parameter, actionName, formUri);
}

export function getLinkForNavigateToCreateItemInProperty(workspace: string, 
                                                         itemUrl: string,
                                                         metaclass: string,
                                                         metaclassWorkspace: string, 
                                                         propertyName: string) {
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


export function navigateToCreateItemInProperty(workspace: string,
                                                         itemUrl: string,
                                                         metaclass: string,
                                                         metaclassWorkspace: string,
                                                         propertyName: string) {
    document.location.href = 
        getLinkForNavigateToCreateItemInProperty(workspace, itemUrl, metaclass, metaclassWorkspace, propertyName);
}