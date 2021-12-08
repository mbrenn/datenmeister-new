define(["require", "exports", "./ApiConnection", "./Settings"], function (require, exports, ApiConnection, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.setMetaclass = exports.getAllChildItems = exports.getAllRootItems = exports.getAllExtents = exports.getAllWorkspaces = void 0;
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
    function setMetaclass(workspaceId, itemUrl, newMetaClass) {
        let url = Settings.baseUrl +
            "api/items/set_metaclass/" +
            encodeURIComponent(workspaceId) +
            "/" +
            encodeURIComponent(itemUrl);
        return ApiConnection.post(url, { metaclass: newMetaClass });
    }
    exports.setMetaclass = setMetaclass;
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