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

export function navigateToItem(workspace: string, extentUri: string, itemId: string) {
    document.location.href =
        Settings.baseUrl + "Item/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(extentUri + "#" + itemId);
}

export function navigateToItemByUrl(workspace: string, itemUrl: string) {
    document.location.href =
        Settings.baseUrl + "Item/" +
        encodeURIComponent(workspace) + "/" +
        encodeURIComponent(itemUrl);
}