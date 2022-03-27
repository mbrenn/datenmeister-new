define(["require", "exports", "./Settings", "./ApiConnection", "./Mof"], function (require, exports, Settings, ApiConnection, Mof) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getProperties = exports.setProperties = exports.deleteExtent = exports.createXmi = void 0;
    function createXmi(params) {
        return new Promise((resolve, reject) => {
            let url = Settings.baseUrl +
                "api/extent/create_xmi";
            ApiConnection.post(url, params).then((result) => {
                resolve({ success: result.success });
            });
        });
    }
    exports.createXmi = createXmi;
    function deleteExtent(params) {
        const r = new Promise((resolve, reject) => {
            let url = Settings.baseUrl +
                "api/extent/delete";
            ApiConnection.deleteRequest(url, params).then((result) => {
                resolve({ success: result.success });
            });
        });
        return r;
    }
    exports.deleteExtent = deleteExtent;
    function setProperties(workspace, extentUri, properties) {
        return ApiConnection.post(Settings.baseUrl + "api/extent/set_properties/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri), Mof.createJsonFromObject(properties));
    }
    exports.setProperties = setProperties;
    function getProperties(workspace, extentUri) {
        return new Promise(resolve => {
            ApiConnection.get(Settings.baseUrl + "api/extent/get_properties/"
                + encodeURIComponent(workspace)
                + "/" + encodeURIComponent(extentUri))
                .then(x => {
                const dmObject = Mof.convertJsonObjectToDmObject(x);
                resolve(dmObject);
            });
        });
    }
    exports.getProperties = getProperties;
});
//# sourceMappingURL=Client.Extents.js.map