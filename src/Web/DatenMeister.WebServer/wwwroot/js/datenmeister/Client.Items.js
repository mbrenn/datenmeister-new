define(["require", "exports", "./Mof", "./Settings", "./ApiConnection"], function (require, exports, Mof, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.unsetProperty = exports.setProperty = exports.getProperty = exports.removeReferenceFromCollection = exports.addReferenceToCollection = exports.setMetaclass = exports.storeObjectByUri = exports.loadRootElementsFromExtent = exports.loadObjectByUri = exports.loadObject = void 0;
    function loadObject(workspace, extent, id) {
        return new Promise((resolve, reject) => {
            ApiConnection.get(Settings.baseUrl +
                "api/items/get/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(extent) +
                "/" +
                encodeURIComponent(id)).then(x => {
                const dmObject = Mof.convertJsonObjectToDmObject(x);
                resolve(dmObject);
            });
        });
    }
    exports.loadObject = loadObject;
    function loadObjectByUri(workspace, url) {
        return new Promise((resolve, reject) => {
            ApiConnection.get(Settings.baseUrl +
                "api/items/get/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(url)).then(x => {
                const dmObject = Mof.convertJsonObjectToDmObject(x);
                resolve(dmObject);
            });
        });
    }
    exports.loadObjectByUri = loadObjectByUri;
    function loadRootElementsFromExtent(workspace, extentUri) {
        return new Promise((resolve, reject) => {
            ApiConnection.get(Settings.baseUrl +
                "api/items/get_root_elements/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(extentUri)).then(text => {
                const x = JSON.parse(text);
                let result = new Array();
                for (let n in x) {
                    if (Object.prototype.hasOwnProperty.call(x, n)) {
                        const v = x[n];
                        result.push(Mof.convertJsonObjectToDmObject(v));
                    }
                }
                resolve(result);
            });
        });
    }
    exports.loadRootElementsFromExtent = loadRootElementsFromExtent;
    function storeObjectByUri(workspace, url, element) {
        return new Promise((resolve, reject) => {
            const result = Mof.createJsonFromObject(element);
            ApiConnection.put(Settings.baseUrl +
                "api/items/set/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(url), result).then(x => {
                resolve();
            });
        });
    }
    exports.storeObjectByUri = storeObjectByUri;
    function setMetaclass(workspaceId, itemUrl, newMetaClass) {
        let url = Settings.baseUrl +
            "api/items/set_metaclass/" +
            encodeURIComponent(workspaceId) +
            "/" +
            encodeURIComponent(itemUrl);
        return ApiConnection.post(url, { metaclass: newMetaClass });
    }
    exports.setMetaclass = setMetaclass;
    function addReferenceToCollection(workspaceId, itemUrl, parameter) {
        let url = Settings.baseUrl +
            "api/items/add_ref_to_collection/" +
            encodeURIComponent(workspaceId) +
            "/" +
            encodeURIComponent(itemUrl);
        return ApiConnection.post(url, {
            property: parameter.property,
            workspaceId: parameter.referenceWorkspaceId,
            referenceUri: parameter.referenceUri
        });
    }
    exports.addReferenceToCollection = addReferenceToCollection;
    function removeReferenceFromCollection(workspaceId, itemUrl, parameter) {
        let url = Settings.baseUrl +
            "api/items/remove_ref_to_collection/" +
            encodeURIComponent(workspaceId) +
            "/" +
            encodeURIComponent(itemUrl);
        return ApiConnection.post(url, {
            property: parameter.property,
            workspaceId: parameter.referenceWorkspaceId,
            referenceUri: parameter.referenceUri
        });
    }
    exports.removeReferenceFromCollection = removeReferenceFromCollection;
    function getProperty(workspaceId, itemUrl, property) {
        const r = new Promise((resolve, reject) => {
            let url = Settings.baseUrl +
                "api/items/get_property/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl) +
                "?property=" +
                encodeURIComponent(property);
            const result = ApiConnection.get(url);
            result.then(x => {
                const dmObject = Mof.convertJsonObjectToObjects(x.v);
                resolve(dmObject);
            });
        });
        return r;
    }
    exports.getProperty = getProperty;
    function setProperty(workspaceId, itemUrl, property, value) {
        return new Promise(resolve => {
            let url = Settings.baseUrl +
                "api/items/set_property/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl);
            const result = ApiConnection.put(url, { key: property, value: value });
            result.then(x => {
                resolve(true);
            });
        });
    }
    exports.setProperty = setProperty;
    function unsetProperty(workspaceId, itemUrl, property) {
        return new Promise(resolve => {
            let url = Settings.baseUrl +
                "api/items/unset_property/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl);
            const result = ApiConnection.put(url, { property: property });
            result.then(x => {
                resolve(true);
            });
        });
    }
    exports.unsetProperty = unsetProperty;
});
//# sourceMappingURL=Client.Items.js.map