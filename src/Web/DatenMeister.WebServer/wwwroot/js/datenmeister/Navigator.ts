import * as Settings from "./Settings";

export function navigateToWorkspaces() {
    document.location.href =
        Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
}

export function navigateToWorkspace(workspace: string) {
    document.location.href =
        Settings.baseUrl + "Item/Management/" +
        encodeURIComponent("dm:///_internal/workspaces#" + workspace);
}

export function navigateToExtent(workspace: string, extentUri: string) {
    document.location.href =
        Settings.baseUrl + "ItemsOverview/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(extentUri);
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
export function navigateToItem(workspace: string, extentUri: string, itemId: string, param?: INavigateToItemParams) {
    document.location.href =
        Settings.baseUrl + "Item/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(extentUri + "#" + itemId) + 
        parseNavigateToItemParam(param);
}

export function navigateToItemByUrl(workspace: string, itemUrl: string, param?: INavigateToItemParams) {
    document.location.href =
        Settings.baseUrl + "Item/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(itemUrl) +
        parseNavigateToItemParam(param);
}

function parseNavigateToItemParam(param? : INavigateToItemParams)
{
    if ( param === undefined) {
        return "";
    }
    
    let result = '';
    let ampersand = '?';
    if ( param.editMode === true)
    {
        result += ampersand + "edit=true"
        ampersand = '&';
    }
    
    return result;
    
}

export function navigateToCreateNewItemInExtent(workspace: string, extentUri: string, metaclass: string) {
    document.location.href = Settings.baseUrl +
        "ItemAction/Extent.CreateItem?workspace=" +
        encodeURIComponent(workspace) +
        "&extent=" +
        encodeURIComponent(extentUri) +
        "&metaclass=" +
        encodeURIComponent(metaclass);
}

export function navigateToAction(actionName: string, formUri?: string, parameter?: any) {
    let urlParameter = "";
    
    if ( parameter !== undefined) {
        urlParameter = "?";
        let ampersand = "";

        for (let key in parameter) {
            var value = parameter[key];
            
            urlParameter += ampersand + encodeURIComponent(key) + "=" + encodeURIComponent(value);
            ampersand = "&";
        }
    }
    
    document.location.href =
        `${Settings.baseUrl}ItemAction/${actionName}/${encodeURIComponent(formUri ?? "")}${urlParameter}`;
}