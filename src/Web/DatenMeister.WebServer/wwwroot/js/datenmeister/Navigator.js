define(["require", "exports", "./Settings"], function (require, exports, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.navigateToAction = exports.navigateToCreateNewItemInExtent = exports.navigateToItemByUrl = exports.navigateToItem = exports.navigateToExtentProperties = exports.navigateToExtent = exports.navigateToWorkspace = exports.navigateToWorkspaces = void 0;
    function navigateToWorkspaces() {
        document.location.href =
            Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
    }
    exports.navigateToWorkspaces = navigateToWorkspaces;
    function navigateToWorkspace(workspace) {
        document.location.href =
            Settings.baseUrl + "Item/Management/" +
                encodeURIComponent("dm:///_internal/workspaces#" + workspace);
    }
    exports.navigateToWorkspace = navigateToWorkspace;
    function navigateToExtent(workspace, extentUri) {
        document.location.href =
            Settings.baseUrl + "ItemsOverview/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri);
    }
    exports.navigateToExtent = navigateToExtent;
    function navigateToExtentProperties(workspace, extentUri) {
        document.location.href =
            Settings.baseUrl + "Item/Management/" +
                encodeURIComponent(workspace) + "_" +
                encodeURIComponent(extentUri);
    }
    exports.navigateToExtentProperties = navigateToExtentProperties;
    function navigateToItem(workspace, extentUri, itemId, param) {
        document.location.href =
            Settings.baseUrl + "Item/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri + "#" + itemId) +
                parseNavigateToItemParam(param);
    }
    exports.navigateToItem = navigateToItem;
    function navigateToItemByUrl(workspace, itemUrl, param) {
        document.location.href =
            Settings.baseUrl + "Item/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(itemUrl) +
                parseNavigateToItemParam(param);
    }
    exports.navigateToItemByUrl = navigateToItemByUrl;
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
        return result;
    }
    function navigateToCreateNewItemInExtent(workspace, extentUri, metaclass) {
        document.location.href = Settings.baseUrl +
            "ItemAction/Extent.CreateItem?workspace=" +
            encodeURIComponent(workspace) +
            "&extent=" +
            encodeURIComponent(extentUri) +
            "&metaclass=" +
            encodeURIComponent(metaclass);
    }
    exports.navigateToCreateNewItemInExtent = navigateToCreateNewItemInExtent;
    function navigateToAction(actionName, formUri, parameter) {
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
        document.location.href =
            `${Settings.baseUrl}ItemAction/${actionName}/${encodeURIComponent(formUri !== null && formUri !== void 0 ? formUri : "")}${urlParameter}`;
    }
    exports.navigateToAction = navigateToAction;
});
//# sourceMappingURL=Navigator.js.map