define(["require", "exports", "./Mof", "./ApiConnection", "./Settings"], function (require, exports, Mof, ApiConnection, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {value: true});
    exports.setProperties = void 0;
    function setProperties(workspace, extentUri, properties) {
        return ApiConnection.post(Settings.baseUrl + "api/extent/set_properties/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri), Mof.createJsonFromObject(properties));
    }
    exports.setProperties = setProperties;
});
//# sourceMappingURL=ExtentControllerClient.js.map