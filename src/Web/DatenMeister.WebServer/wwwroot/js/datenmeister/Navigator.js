define(["require", "exports", "./Settings"], function (require, exports, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.navigateToAction = exports.getLinkForNavigateToAction = exports.navigateToCreateNewItemInExtent = exports.getLinkForNavigateToCreateNewItemInExtent = exports.navigateToItemByUrl = exports.getLinkForNavigateToItemByUrl = exports.navigateToItem = exports.getLinkForNavigateToItem = exports.navigateToExtentProperties = exports.navigateToExtent = exports.getLinkForNavigateToExtent = exports.navigateToWorkspace = exports.getLinkForNavigateToWorkspace = exports.navigateToWorkspaces = exports.getLinkForNavigateToWorkspaces = void 0;
    function getLinkForNavigateToWorkspaces() {
        return Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
    }
    exports.getLinkForNavigateToWorkspaces = getLinkForNavigateToWorkspaces;
    function navigateToWorkspaces() {
        document.location.href =
            getLinkForNavigateToWorkspaces();
    }
    exports.navigateToWorkspaces = navigateToWorkspaces;
    function getLinkForNavigateToWorkspace(workspace) {
        return Settings.baseUrl + "Item/Management/" +
            encodeURIComponent("dm:///_internal/workspaces#" + workspace);
    }
    exports.getLinkForNavigateToWorkspace = getLinkForNavigateToWorkspace;
    function navigateToWorkspace(workspace) {
        document.location.href =
            getLinkForNavigateToWorkspace(workspace);
    }
    exports.navigateToWorkspace = navigateToWorkspace;
    function getLinkForNavigateToExtent(workspace, extentUri) {
        return Settings.baseUrl + "ItemsOverview/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri);
    }
    exports.getLinkForNavigateToExtent = getLinkForNavigateToExtent;
    function navigateToExtent(workspace, extentUri) {
        document.location.href =
            getLinkForNavigateToExtent(workspace, extentUri);
    }
    exports.navigateToExtent = navigateToExtent;
    function navigateToExtentProperties(workspace, extentUri) {
        document.location.href =
            Settings.baseUrl + "Item/Management/" +
                encodeURIComponent(workspace) + "_" +
                encodeURIComponent(extentUri);
    }
    exports.navigateToExtentProperties = navigateToExtentProperties;
    function getLinkForNavigateToItem(workspace, extentUri, itemId, param) {
        return Settings.baseUrl + "Item/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri + "#" + itemId) +
            parseNavigateToItemParam(param);
    }
    exports.getLinkForNavigateToItem = getLinkForNavigateToItem;
    function navigateToItem(workspace, extentUri, itemId, param) {
        document.location.href =
            getLinkForNavigateToItem(workspace, extentUri, itemId, param);
    }
    exports.navigateToItem = navigateToItem;
    function getLinkForNavigateToItemByUrl(workspace, itemUrl, param) {
        return Settings.baseUrl + "Item/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(itemUrl) +
            parseNavigateToItemParam(param);
    }
    exports.getLinkForNavigateToItemByUrl = getLinkForNavigateToItemByUrl;
    function navigateToItemByUrl(workspace, itemUrl, param) {
        document.location.href =
            getLinkForNavigateToItemByUrl(workspace, itemUrl, param);
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
    function getLinkForNavigateToCreateNewItemInExtent(workspace, extentUri, metaclass) {
        return Settings.baseUrl +
            "ItemAction/Extent.CreateItem?workspace=" +
            encodeURIComponent(workspace) +
            "&extent=" +
            encodeURIComponent(extentUri) +
            "&metaclass=" +
            encodeURIComponent(metaclass);
    }
    exports.getLinkForNavigateToCreateNewItemInExtent = getLinkForNavigateToCreateNewItemInExtent;
    function navigateToCreateNewItemInExtent(workspace, extentUri, metaclass) {
        document.location.href = getLinkForNavigateToCreateNewItemInExtent(workspace, extentUri, metaclass);
    }
    exports.navigateToCreateNewItemInExtent = navigateToCreateNewItemInExtent;
    function getLinkForNavigateToAction(parameter, actionName, formUri) {
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
        return `${Settings.baseUrl}ItemAction/${actionName}/${encodeURIComponent(formUri !== null && formUri !== void 0 ? formUri : "")}${urlParameter}`;
    }
    exports.getLinkForNavigateToAction = getLinkForNavigateToAction;
    function navigateToAction(actionName, formUri, parameter) {
        document.location.href = getLinkForNavigateToAction(parameter, actionName, formUri);
    }
    exports.navigateToAction = navigateToAction;
});
//# sourceMappingURL=Navigator.js.map