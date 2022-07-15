var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Mof", "../Settings", "../ApiConnection"], function (require, exports, Mof, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getViewModes = exports.getDefaultFormForItem = exports.getForm = exports.getDefaultCollectionFormForMetaClass = exports.getDefaultFormForExtent = void 0;
    /*
        Gets the default form for an extent uri by the webserver
     */
    function getDefaultFormForExtent(workspace, extentUri, viewMode) {
        return __awaiter(this, void 0, void 0, function* () {
            const resultFromServer = yield ApiConnection.get(Settings.baseUrl +
                "api/forms/default_for_extent/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(extentUri) +
                "/" +
                encodeURIComponent(viewMode));
            return Mof.convertJsonObjectToDmObject(resultFromServer);
        });
    }
    exports.getDefaultFormForExtent = getDefaultFormForExtent;
    /*
        Gets the default form for an extent uri by the webserver
     */
    function getDefaultCollectionFormForMetaClass(metaClassUri) {
        return __awaiter(this, void 0, void 0, function* () {
            const resultFromServer = yield ApiConnection.get(Settings.baseUrl +
                "api/forms/default_collection_for_metaclass/" +
                encodeURIComponent(metaClassUri));
            return Mof.convertJsonObjectToDmObject(resultFromServer);
        });
    }
    exports.getDefaultCollectionFormForMetaClass = getDefaultCollectionFormForMetaClass;
    function getForm(formUri) {
        return __awaiter(this, void 0, void 0, function* () {
            const resultFromServer = yield ApiConnection.get(Settings.baseUrl +
                "api/forms/get/" +
                encodeURIComponent(formUri));
            return Mof.convertJsonObjectToDmObject(resultFromServer);
        });
    }
    exports.getForm = getForm;
    /*
        Gets the default form for a certain item by the webserver
     */
    function getDefaultFormForItem(workspace, item, viewMode) {
        return __awaiter(this, void 0, void 0, function* () {
            const resultFromServer = yield ApiConnection.get(Settings.baseUrl +
                "api/forms/default_for_item/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(item) +
                "/" +
                encodeURIComponent(viewMode));
            return Mof.convertJsonObjectToDmObject(resultFromServer);
        });
    }
    exports.getDefaultFormForItem = getDefaultFormForItem;
    function getViewModes() {
        return __awaiter(this, void 0, void 0, function* () {
            const resultFromServer = yield ApiConnection.get(Settings.baseUrl +
                "api/forms/get_viewmodes");
            const result = {
                viewModes: new Array()
            };
            for (let n in resultFromServer.viewModes) {
                const value = resultFromServer.viewModes[n];
                result.viewModes.push(Mof.convertJsonObjectToDmObject(value));
            }
            return result;
        });
    }
    exports.getViewModes = getViewModes;
});
//# sourceMappingURL=Forms.js.map