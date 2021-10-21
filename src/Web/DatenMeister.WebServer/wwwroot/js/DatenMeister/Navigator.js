define(["require", "exports", "./Settings"], function (require, exports, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.navigateToItem = exports.navigateToExtent = exports.navigateToWorkspace = exports.navigateToWorkspaces = void 0;
    function navigateToWorkspaces() {
        document.location.href =
            Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
    }
    exports.navigateToWorkspaces = navigateToWorkspaces;
    function navigateToWorkspace(workspace) {
        document.location.href =
            Settings.baseUrl + "Item/Management/dm%3A%2F%2F%2F_internal%2Fworkspaces/" +
                encodeURIComponent(workspace);
    }
    exports.navigateToWorkspace = navigateToWorkspace;
    function navigateToExtent(workspace, extentUri) {
        document.location.href =
            Settings.baseUrl + "ItemsOverview/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri);
    }
    exports.navigateToExtent = navigateToExtent;
    function navigateToItem(workspace, extentUri, itemId) {
        document.location.href =
            Settings.baseUrl + "Item/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri) + "/" +
                encodeURIComponent(itemId);
    }
    exports.navigateToItem = navigateToItem;
});
//# sourceMappingURL=Navigator.js.map