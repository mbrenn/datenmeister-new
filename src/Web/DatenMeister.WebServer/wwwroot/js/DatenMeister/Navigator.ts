﻿import * as Settings from "./Settings";

export function navigateToWorkspaces() {
    document.location.href =
        Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
}

export function navigateToWorkspace(workspace: string) {
    document.location.href =
        Settings.baseUrl + "Item/Management/dm%3A%2F%2F%2F_internal%2Fworkspaces/" +
        encodeURIComponent(workspace);
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
        encodeURIComponent(extentUri) + "/" +
        encodeURIComponent(itemId);
}