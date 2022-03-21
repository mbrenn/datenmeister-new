define(["require", "exports", "./Settings", "./ApiConnection"], function (require, exports, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.deleteWorkspace = exports.createWorkspace = void 0;
    function createWorkspace(id, annotation, param) {
        const r = jQuery.Deferred();
        let url = Settings.baseUrl +
            "api/workspace/create";
        const data = {
            id: id,
            annotation: annotation
        };
        if ((param === null || param === void 0 ? void 0 : param.skipIfExisting) !== undefined) {
            data.skipIfExisting = param.skipIfExisting;
        }
        ApiConnection.put(url, data).done((result) => {
            r.resolve({ success: result.success });
        });
        return r;
    }
    exports.createWorkspace = createWorkspace;
    function deleteWorkspace(id, param) {
        const r = jQuery.Deferred();
        let url = Settings.baseUrl +
            "api/workspace/delete";
        const data = {
            id: id
        };
        if ((param === null || param === void 0 ? void 0 : param.skipIfExisting) !== undefined) {
            data.skipIfExisting = param.skipIfExisting;
        }
        ApiConnection.deleteRequest(url, data).done((result) => {
            r.resolve({ success: result.success });
        });
        return r;
    }
    exports.deleteWorkspace = deleteWorkspace;
});
//# sourceMappingURL=Client.Workspace.js.map