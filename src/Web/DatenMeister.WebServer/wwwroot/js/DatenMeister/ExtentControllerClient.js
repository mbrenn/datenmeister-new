define(["require", "exports", "./Mof", "./ApiConnection", "./Settings"], function (require, exports, Mof, ApiConnection, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {value: true});
    exports.getProperties = exports.setProperties = void 0;
    function setProperties(workspace, extentUri, properties) {
        return ApiConnection.post(Settings.baseUrl + "api/extent/set_properties/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri), Mof.createJsonFromObject(properties));
    }
    exports.setProperties = setProperties;
    function getProperties(workspace, extentUri) {
        const r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl + "api/extent/get_properties/"
            + encodeURIComponent(workspace)
            + "/" + encodeURIComponent(extentUri))
            .done(x => {
                const dmObject = Mof.convertJsonObjectToDmObject(x);
                r.resolve(dmObject);
            });
        return r;
    }
    exports.getProperties = getProperties;
});
//# sourceMappingURL=ExtentControllerClient.js.map