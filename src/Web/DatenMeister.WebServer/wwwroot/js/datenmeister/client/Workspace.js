define(["require", "exports", "../Settings", "../ApiConnection"], function (require, exports, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.deleteWorkspace = exports.createWorkspace = void 0;
    function createWorkspace(id, annotation, param) {
        return new Promise(resolve => {
            let url = Settings.baseUrl +
                "api/workspace/create";
            const data = {
                id: id,
                annotation: annotation
            };
            if ((param === null || param === void 0 ? void 0 : param.skipIfExisting) !== undefined) {
                data.skipIfExisting = param.skipIfExisting;
            }
            ApiConnection.put(url, data).then((result) => {
                resolve(result);
            });
        });
    }
    exports.createWorkspace = createWorkspace;
    function deleteWorkspace(id, param) {
        return new Promise(resolve => {
            let url = Settings.baseUrl +
                "api/workspace/delete";
            const data = {
                id: id
            };
            if ((param === null || param === void 0 ? void 0 : param.skipIfExisting) !== undefined) {
                data.skipIfExisting = param.skipIfExisting;
            }
            ApiConnection.deleteRequest(url, data).then((result) => {
                resolve(result);
            });
        });
    }
    exports.deleteWorkspace = deleteWorkspace;
});
//# sourceMappingURL=Workspace.js.map