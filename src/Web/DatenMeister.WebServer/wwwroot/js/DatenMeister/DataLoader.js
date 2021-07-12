var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
define(["require", "exports", "./Mof", "./Settings", "./ApiConnection"], function (require, exports, Mof, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ObjectLoader = void 0;
    Mof = __importStar(Mof);
    Settings = __importStar(Settings);
    ApiConnection = __importStar(ApiConnection);
    class ObjectLoader {
        loadObject(workspace, extent, id) {
            var r = jQuery.Deferred();
            ApiConnection.get(Settings.baseUrl +
                "api/items/get/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(extent) +
                "/" +
                encodeURIComponent(id)).done(x => {
                var dmObject = Mof.createObjectFromJson(x.item);
                r.resolve(dmObject);
            });
            return r;
        }
        loadObjectByUri(workspace, item) {
            var r = jQuery.Deferred();
            ApiConnection.get(Settings.baseUrl +
                "api/items/get/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(item)).done(x => {
                var dmObject = Mof.createObjectFromJson(x.item);
                r.resolve(dmObject);
            });
            return r;
        }
    }
    exports.ObjectLoader = ObjectLoader;
});
//# sourceMappingURL=DataLoader.js.map