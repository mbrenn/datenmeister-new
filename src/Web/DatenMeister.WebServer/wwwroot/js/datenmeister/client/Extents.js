var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Settings", "../ApiConnection", "../Mof"], function (require, exports, Settings, ApiConnection, Mof) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getProperties = exports.setProperties = exports.deleteExtent = exports.createXmi = exports.importXmi = exports.ImportXmiResult = exports.exportXmi = exports.ExportXmiResult = exports.exists = void 0;
    function exists(workspaceId, extent) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/extent/exists/"
                + encodeURIComponent(workspaceId) + "/"
                + encodeURIComponent(extent);
            return yield ApiConnection.get(url);
        });
    }
    exports.exists = exists;
    class ExportXmiResult {
    }
    exports.ExportXmiResult = ExportXmiResult;
    function exportXmi(workspace, extentUri) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/extent/export_xmi/"
                + encodeURIComponent(workspace) + "/"
                + encodeURIComponent(extentUri);
            return yield ApiConnection.get(url);
        });
    }
    exports.exportXmi = exportXmi;
    class ImportXmiResult {
    }
    exports.ImportXmiResult = ImportXmiResult;
    function importXmi(workspace, extentUri, xmi) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/extent/import_xmi/"
                + encodeURIComponent(workspace) + "/"
                + encodeURIComponent(extentUri);
            return yield ApiConnection.post(url, { xmi: xmi });
        });
    }
    exports.importXmi = importXmi;
    function createXmi(params) {
        return new Promise((resolve, reject) => {
            let url = Settings.baseUrl +
                "api/extent/create_xmi";
            ApiConnection.post(url, params).then((result) => {
                resolve(result);
            });
        });
    }
    exports.createXmi = createXmi;
    function deleteExtent(params) {
        return new Promise((resolve, reject) => {
            let url = Settings.baseUrl +
                "api/extent/delete";
            ApiConnection.deleteRequest(url, params).then((result) => {
                resolve(result);
            });
        });
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
//# sourceMappingURL=Extents.js.map