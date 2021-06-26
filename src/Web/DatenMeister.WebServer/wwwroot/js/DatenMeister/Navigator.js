var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
define(["require", "exports", "./Settings"], function (require, exports, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.navigateToItem = exports.navigateToExtent = exports.navigateToWorkspace = exports.navigateToWorkspaces = void 0;
    Settings = __importStar(Settings);
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