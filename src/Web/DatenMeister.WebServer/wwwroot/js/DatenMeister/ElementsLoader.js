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
define(["require", "exports", "./ApiConnection"], function (require, exports, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getAllChildItems = exports.getAllRootItems = exports.getAllExtents = exports.getAllWorkspaces = void 0;
    ApiConnection = __importStar(ApiConnection);
    function getAllWorkspaces() {
        return load(undefined, undefined, undefined);
    }
    exports.getAllWorkspaces = getAllWorkspaces;
    function getAllExtents(workspaceId) {
        return load(workspaceId, undefined, undefined);
    }
    exports.getAllExtents = getAllExtents;
    function getAllRootItems(workspaceId, extent) {
        return load(workspaceId, extent, undefined);
    }
    exports.getAllRootItems = getAllRootItems;
    function getAllChildItems(workspaceId, extent, itemId) {
        return load(workspaceId, extent, itemId);
    }
    exports.getAllChildItems = getAllChildItems;
    function load(workspaceId, extent, itemId) {
        const r = jQuery.Deferred();
        let url = '/api/elements/get_composites';
        if (workspaceId !== undefined && workspaceId !== null) {
            url += '/' + encodeURIComponent(workspaceId);
            if (extent !== undefined && extent !== null) {
                if (itemId !== undefined && itemId !== null) {
                    url += '/' + encodeURIComponent(extent + '#' + itemId);
                }
                else {
                    url += '/' + encodeURIComponent(extent);
                }
            }
        }
        ApiConnection.get(url).done(items => {
            r.resolve(items);
        });
        return r;
    }
});
//# sourceMappingURL=ElementsLoader.js.map