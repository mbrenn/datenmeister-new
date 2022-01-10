define(["require", "exports", "./Mof", "./Settings", "./ApiConnection"], function (require, exports, Mof, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {value: true});
    exports.setMetaclass = exports.storeObjectByUri = exports.loadRootElementsFromExtent = exports.loadObjectByUri = exports.loadObject = void 0;

    function loadObject(workspace, extent, id) {
        const r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl +
            "api/items/get/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(extent) +
            "/" +
            encodeURIComponent(id)).done(x => {
            const dmObject = Mof.convertJsonObjectToDmObject(x);
            r.resolve(dmObject);
        });
        return r;
    }

    exports.loadObject = loadObject;

    function loadObjectByUri(workspace, url) {
        const r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl +
            "api/items/get/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(url)).done(x => {
            const dmObject = Mof.convertJsonObjectToDmObject(x);
            r.resolve(dmObject);
        });
        return r;
    }

    exports.loadObjectByUri = loadObjectByUri;

    function loadRootElementsFromExtent(workspace, extentUri) {
        const r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl +
            "api/items/get_root_elements/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(extentUri)).done(text => {
            const x = JSON.parse(text);
            let result = [];
            for (let n in x) {
                if (Object.prototype.hasOwnProperty.call(x, n)) {
                    const v = x[n];
                    result.push(Mof.convertJsonObjectToDmObject(v));
                }
            }
            r.resolve(result);
        });
        return r;
    }

    exports.loadRootElementsFromExtent = loadRootElementsFromExtent;

    function storeObjectByUri(workspace, url, element) {
        const r = jQuery.Deferred();
        const result = Mof.createJsonFromObject(element);
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

    function setMetaclass(workspaceId, itemUrl, newMetaClass) {
        let url = Settings.baseUrl +
            "api/items/set_metaclass/" +
            encodeURIComponent(workspaceId) +
            "/" +
            encodeURIComponent(itemUrl);
        return ApiConnection.post(url, {metaclass: newMetaClass});
    }

    exports.setMetaclass = setMetaclass;
});
//# sourceMappingURL=Client.Items.js.map