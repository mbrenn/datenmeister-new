//
// This module contains the handling of the viewmode. 
// The user can select a view mode and this will be applied throughout the session
// When the user changes the viewmode, it will be kept during the session
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Forms"], function (require, exports, FormsClient) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getViewModesFromServer = exports.clearCurrentViewMode = exports.setCurrentViewMode = exports.getCurrentViewMode = void 0;
    const sessionPropertyName = "dm_current_viewmode";
    /**
     * Gets the current view mode. It is 'Default', when no viewmode has been selected by the user.
     */
    function getCurrentViewMode() {
        let viewMode = sessionStorage.getItem(sessionPropertyName);
        if (viewMode === null) {
            return "Default";
        }
        return viewMode;
    }
    exports.getCurrentViewMode = getCurrentViewMode;
    /**
     * Sets the current view mode as the active view.
     * @param viewModeId Id of the view ode
     */
    function setCurrentViewMode(viewModeId) {
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
    exports.setCurrentViewMode = setCurrentViewMode;
    /**
     * Clears the current view mode
     */
    function clearCurrentViewMode() {
        sessionStorage.removeItem(sessionPropertyName);
    }
    exports.clearCurrentViewMode = clearCurrentViewMode;
    /**
     *  Loads the viewModes
     */
    function getViewModesFromServer() {
        return __awaiter(this, void 0, void 0, function* () {
            const serverResult = yield FormsClient.getViewModes();
            return serverResult.viewModes;
        });
    }
    exports.getViewModesFromServer = getViewModesFromServer;
});
//# sourceMappingURL=ViewModeLogic.js.map