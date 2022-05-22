var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../ApiConnection", "../Settings"], function (require, exports, ApiConnection, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.findBySearchString = exports.createTemporaryElement = exports.loadNameByUri = exports.loadNameOf = exports.getAllChildItems = exports.getAllRootItems = exports.getAllExtents = exports.getAllWorkspaces = void 0;
    function getAllWorkspaces() {
        return load(undefined, undefined);
    }
    exports.getAllWorkspaces = getAllWorkspaces;
    function getAllExtents(workspaceId) {
        return load(workspaceId, undefined);
    }
    exports.getAllExtents = getAllExtents;
    function getAllRootItems(workspaceId, extentUri) {
        return load(workspaceId, extentUri);
    }
    exports.getAllRootItems = getAllRootItems;
    function getAllChildItems(workspaceId, itemUrl) {
        return load(workspaceId, itemUrl);
    }
    exports.getAllChildItems = getAllChildItems;
    /*
     * Loads the items from the server.
     * The ItemUri may be an extent (then the root items will be loaded)
     * or may be an item (then the composite children will be loaded)
     */
    function load(workspaceId, itemUri) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = "/api/elements/get_composites";
            if (workspaceId !== undefined && workspaceId !== null) {
                url += '/' + encodeURIComponent(workspaceId);
                if (itemUri !== undefined && itemUri !== null) {
                    url += '/' + encodeURIComponent(itemUri);
                }
            }
            return yield ApiConnection.get(url);
        });
    }
    function loadNameOf(workspaceId, extentUri, itemUri) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield ApiConnection.get(Settings.baseUrl +
                "api/elements/get_name/" +
                encodeURIComponent(workspaceId) + "/" +
                encodeURIComponent(extentUri) + "/" +
                encodeURIComponent(itemUri));
        });
    }
    exports.loadNameOf = loadNameOf;
    function loadNameByUri(workspaceId, elementUri) {
        return __awaiter(this, void 0, void 0, function* () {
            if (workspaceId === undefined) {
                workspaceId = "_";
            }
            return yield ApiConnection.get(Settings.baseUrl +
                "api/elements/get_name/" +
                encodeURIComponent(workspaceId) + "/" +
                encodeURIComponent(elementUri));
        });
    }
    exports.loadNameByUri = loadNameByUri;
    function createTemporaryElement(metaClassUri) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield ApiConnection.put(Settings.baseUrl +
                "api/elements/create_temporary_element", {
                metaClassUri: metaClassUri
            });
        });
    }
    exports.createTemporaryElement = createTemporaryElement;
    function findBySearchString(searchString) {
        return ApiConnection.get(Settings.baseUrl +
            "api/elements/find_by_searchstring?search=" +
            encodeURIComponent(searchString));
    }
    exports.findBySearchString = findBySearchString;
});
//# sourceMappingURL=Elements.js.map