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
define(["require", "exports", "./DatenMeister/ApiConnection", "./DatenMeister/Settings", "./DatenMeister/NameLoader", "./DatenMeister/Navigator"], function (require, exports, ApiConnection, Settings, NameLoader, Navigator) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.DomHelper = exports.FormActions = void 0;
    ApiConnection = __importStar(ApiConnection);
    Settings = __importStar(Settings);
    NameLoader = __importStar(NameLoader);
    Navigator = __importStar(Navigator);
    class FormActions {
        static extentNavigateTo(workspace, extentUri) {
            document.location.href = Settings.baseUrl + "ItemsOverview/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri);
        }
        static createZipExample(workspace) {
            ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: workspace })
                .done(data => {
                document.location.reload();
            });
        }
        static itemNew(workspace, extentUri) {
            ApiConnection.post(Settings.baseUrl + "api/items/create", {
                workspace: workspace,
                extentUri: extentUri
            })
                .done(data => {
                document.location.reload();
            });
        }
        static itemDelete(workspace, extentUri, itemId) {
            ApiConnection.post(Settings.baseUrl + "api/items/delete", {
                workspace: workspace,
                extentUri: extentUri,
                itemId: itemId
            })
                .done(data => {
                Navigator.navigateToExtent(workspace, extentUri);
            });
        }
        static extentsListViewItem(workspace, extentUri, itemId) {
            document.location.href = Settings.baseUrl + "Item/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri) + "/" +
                encodeURIComponent(itemId);
        }
        static extentsListDeleteItem(workspace, extentUri, itemId) {
            ApiConnection.post(Settings.baseUrl + "api/items/delete_from_extent", {
                workspace: workspace,
                extentUri: extentUri,
                itemId: itemId
            })
                .done(data => {
                document.location.reload();
            });
        }
    }
    exports.FormActions = FormActions;
    class DomHelper {
        static injectName(domElement, elementPosition) {
            NameLoader.loadNameOf(elementPosition).done(x => {
                domElement.text(x.name);
            });
        }
        static injectNameByUri(domElement, elementUri) {
            NameLoader.loadNameByUri(elementUri).done(x => {
                if (x.extentUri !== undefined && x.workspace !== undefined
                    && x.extentUri !== "" && x.workspace !== ""
                    && x.itemId !== "" && x.itemId !== undefined) {
                    const linkElement = $("<a></a>");
                    linkElement.text(x.name);
                    linkElement.attr("href", "/Item/" + encodeURIComponent(x.workspace) +
                        "/" + encodeURIComponent(x.extentUri) +
                        "/" + encodeURIComponent(x.itemId));
                    domElement.empty();
                    domElement.append(linkElement);
                }
                else {
                    domElement.text(x.name);
                }
            });
        }
    }
    exports.DomHelper = DomHelper;
});
//# sourceMappingURL=datenmeister.js.map