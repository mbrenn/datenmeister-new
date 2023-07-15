//
// This module contains the handling of the viewmode. 
// The user can select a view mode and this will be applied throughout the session
// When the user changes the viewmode, it will be kept during the session
import { ObjectType } from "../Mof.js";
import * as FormsClient from "../client/Forms.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
const sessionPropertyName = "dm_current_viewmode";
/**
 * Gets the current view mode. It is 'Default', when no viewmode has been selected by the user.
 */
export function getCurrentViewMode() {
    const viewMode = sessionStorage.getItem(sessionPropertyName);
    if (viewMode === null) {
        return "ViewMode.Default";
    }
    return viewMode;
}
export function isCurrentViewModeSet() {
    const viewMode = sessionStorage.getItem(sessionPropertyName);
    return viewMode !== null;
}
/**
 * Sets the current view mode as the active view.
 * @param viewModeId Id of the view ode
 */
export function setCurrentViewMode(viewModeId) {
    try {
        sessionStorage.setItem(sessionPropertyName, viewModeId);
    }
    catch (exc) {
        // Should not happen, but I have no clue of how to capture that
        if (exc.code === DOMException.QUOTA_EXCEEDED_ERR) {
            alert('Quota exceeded');
        }
    }
}
/**
 * Clears the current view mode
 */
export function clearCurrentViewMode() {
    sessionStorage.removeItem(sessionPropertyName);
}
/**
 *  Loads the viewModes
 */
export async function getViewModesFromServer() {
    const serverResult = await FormsClient.getViewModes();
    return serverResult.viewModes;
}
export async function getDefaultViewModeIfNotSet(workspaceId, extentUri) {
    if (isCurrentViewModeSet())
        return getCurrentViewMode();
    const viewMode = await FormsClient.getDefaultViewMode(workspaceId, extentUri);
    return viewMode.viewMode.get(_DatenMeister._Forms._ViewMode._name_, ObjectType.String);
}
//# sourceMappingURL=ViewModeLogic.js.map