define(["require", "exports", "./Settings", "./ApiConnection"], function (require, exports, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.deleteExtent = exports.createXmi = void 0;
    function createXmi(params) {
        const r = jQuery.Deferred();
        let url = Settings.baseUrl +
            "api/extent/create_xmi";
        ApiConnection.post(url, params).done((result) => {
            r.resolve({ success: result.success });
        });
        return r;
    }
    exports.createXmi = createXmi;
    function deleteExtent(params) {
        const r = jQuery.Deferred();
        let url = Settings.baseUrl +
            "api/extent/delete";
        ApiConnection.deleteRequest(url, params).done((result) => {
            r.resolve({ success: result.success });
        });
        return r;
    }
    exports.deleteExtent = deleteExtent;
});
//# sourceMappingURL=Client.Extents.js.map