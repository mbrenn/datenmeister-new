define(["require", "exports", "./Mof", "./Settings", "./ApiConnection"], function (require, exports, Mof, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.storeObjectByUri = exports.loadObjectByUri = exports.loadObject = void 0;
    function loadObject(workspace, extent, id) {
        const r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl +
            "api/items/get/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(extent) +
            "/" +
            encodeURIComponent(id)).done(x => {
            var dmObject = Mof.createObjectFromJson(x.item);
            r.resolve(dmObject);
        });
        return r;
    }
    exports.loadObject = loadObject;
    function loadObjectByUri(workspace, url) {
        var r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl +
            "api/items/get/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(url)).done(x => {
            var dmObject = Mof.createObjectFromJson(x.item, x.metaClass);
            r.resolve(dmObject);
        });
        return r;
    }
    exports.loadObjectByUri = loadObjectByUri;
    function storeObjectByUri(workspace, url, element) {
        var r = jQuery.Deferred();
        var result = Mof.createJsonFromObject(element);
        ApiConnection.put(Settings.baseUrl +
            "api/items/set/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(url), result).done(x => {
            r.resolve();
        });
        return r;
    }
    exports.storeObjectByUri = storeObjectByUri;
});
//# sourceMappingURL=DataLoader.js.map