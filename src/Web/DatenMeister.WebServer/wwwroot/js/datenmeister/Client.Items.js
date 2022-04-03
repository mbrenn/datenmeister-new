var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Mof", "./Settings", "./ApiConnection"], function (require, exports, Mof, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.removeReferenceFromCollection = exports.addReferenceToCollection = exports.setMetaclass = exports.getProperty = exports.setProperties = exports.setPropertiesByStringValues = exports.unsetProperty = exports.setProperty = exports.getContainer = exports.getRootElements = exports.getObjectByUri = exports.getObject = exports.deleteItemFromExtent = exports.deleteItem = exports.createItemAsChild = exports.createItemInExtent = void 0;
    function createItemInExtent(workspaceId, extentUri, param) {
        return __awaiter(this, void 0, void 0, function* () {
            const evaluatedParameter = {
                metaClass: param.metaClass,
                properties: undefined
            };
            if (param.properties !== undefined && param.properties !== null) {
                evaluatedParameter.properties = Mof.createJsonFromObject(param.properties);
            }
            return yield ApiConnection.post(Settings.baseUrl + "api/items/create_in_extent/"
                + encodeURIComponent(workspaceId) + "/"
                + encodeURIComponent(extentUri), evaluatedParameter);
        });
    }
    exports.createItemInExtent = createItemInExtent;
    function createItemAsChild(workspaceId, itemUri, param) {
        var _a;
        return __awaiter(this, void 0, void 0, function* () {
            const evaluatedParameter = {
                metaClass: param.metaClass,
                property: param.property,
                asList: (_a = param.asList) !== null && _a !== void 0 ? _a : false,
                properties: undefined
            };
            if (param.properties !== undefined && param.properties !== null) {
                evaluatedParameter.properties = Mof.createJsonFromObject(param.properties);
            }
            return yield ApiConnection.post(Settings.baseUrl + "api/items/create_child/"
                + encodeURIComponent(workspaceId) + "/"
                + encodeURIComponent(itemUri), evaluatedParameter);
        });
    }
    exports.createItemAsChild = createItemAsChild;
    function deleteItem(workspaceId, itemUri) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield ApiConnection.deleteRequest(Settings.baseUrl + "api/items/create_child/"
                + encodeURIComponent(workspaceId) + "/"
                + encodeURIComponent(itemUri), {});
        });
    }
    exports.deleteItem = deleteItem;
    function deleteItemFromExtent(workspaceId, extentUri, itemId) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield ApiConnection.deleteRequest(Settings.baseUrl + "api/items/create_child/"
                + encodeURIComponent(workspaceId) + "/"
                + encodeURIComponent(extentUri) + "/"
                + encodeURIComponent(itemId), {});
        });
    }
    exports.deleteItemFromExtent = deleteItemFromExtent;
    function getObject(workspace, extent, id) {
        return __awaiter(this, void 0, void 0, function* () {
            const resultFromServer = ApiConnection.get(Settings.baseUrl +
                "api/items/get/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(extent) +
                "/" +
                encodeURIComponent(id));
            return Mof.convertJsonObjectToDmObject(resultFromServer);
        });
    }
    exports.getObject = getObject;
    function getObjectByUri(workspace, url) {
        return __awaiter(this, void 0, void 0, function* () {
            const resultFromServer = yield ApiConnection.get(Settings.baseUrl +
                "api/items/get/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(url));
            return Mof.convertJsonObjectToDmObject(resultFromServer);
        });
    }
    exports.getObjectByUri = getObjectByUri;
    function getRootElements(workspace, extentUri) {
        return __awaiter(this, void 0, void 0, function* () {
            const resultFromServer = yield ApiConnection.get(Settings.baseUrl +
                "api/items/get_root_elements/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(extentUri));
            const x = JSON.parse(resultFromServer);
            let result = new Array();
            for (let n in x) {
                if (Object.prototype.hasOwnProperty.call(x, n)) {
                    const v = x[n];
                    result.push(Mof.convertJsonObjectToDmObject(v));
                }
            }
            return result;
        });
    }
    exports.getRootElements = getRootElements;
    function getContainer(workspaceId, itemUri) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield ApiConnection.get(Settings.baseUrl + "api/items/get_container/"
                + encodeURIComponent(workspaceId) + "/"
                + encodeURIComponent(itemUri));
        });
    }
    exports.getContainer = getContainer;
    function setProperty(workspaceId, itemUrl, property, value) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/items/set_property/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl);
            return yield ApiConnection.put(url, { key: property, value: value });
        });
    }
    exports.setProperty = setProperty;
    function unsetProperty(workspaceId, itemUrl, property) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/items/unset_property/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl);
            return yield ApiConnection.put(url, { property: property });
        });
    }
    exports.unsetProperty = unsetProperty;
    function setPropertiesByStringValues(workspaceId, itemUrl, params) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/items/unset_property/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl);
            return yield ApiConnection.post(url, params);
        });
    }
    exports.setPropertiesByStringValues = setPropertiesByStringValues;
    function setProperties(workspaceId, itemUrl, properties) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/items/set/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl);
            return yield ApiConnection.put(url, Mof.createJsonFromObject(properties));
        });
    }
    exports.setProperties = setProperties;
    function getProperty(workspaceId, itemUrl, property) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/items/get_property/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl) +
                "?property=" +
                encodeURIComponent(property);
            const result = yield ApiConnection.get(url);
            return Mof.convertJsonObjectToObjects(result.v);
        });
    }
    exports.getProperty = getProperty;
    function setMetaclass(workspaceId, itemUrl, newMetaClass) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/items/set_metaclass/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl);
            return yield ApiConnection.post(url, { metaclass: newMetaClass });
        });
    }
    exports.setMetaclass = setMetaclass;
    function addReferenceToCollection(workspaceId, itemUrl, parameter) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/items/add_ref_to_collection/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl);
            yield ApiConnection.post(url, {
                property: parameter.property,
                workspaceId: parameter.referenceWorkspaceId,
                referenceUri: parameter.referenceUri
            });
        });
    }
    exports.addReferenceToCollection = addReferenceToCollection;
    function removeReferenceFromCollection(workspaceId, itemUrl, parameter) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/items/remove_ref_to_collection/" +
                encodeURIComponent(workspaceId) +
                "/" +
                encodeURIComponent(itemUrl);
            yield ApiConnection.post(url, {
                property: parameter.property,
                workspaceId: parameter.referenceWorkspaceId,
                referenceUri: parameter.referenceUri
            });
        });
    }
    exports.removeReferenceFromCollection = removeReferenceFromCollection;
});
//# sourceMappingURL=Client.Items.js.map