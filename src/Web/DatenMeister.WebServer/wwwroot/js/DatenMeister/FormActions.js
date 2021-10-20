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
define(["require", "exports", "./Settings", "./ApiConnection", "./Navigator"], function (require, exports, Settings, ApiConnection, Navigator) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.FormActions = exports.DetailFormActions = void 0;
    Settings = __importStar(Settings);
    ApiConnection = __importStar(ApiConnection);
    Navigator = __importStar(Navigator);
    var DetailFormActions;
    (function (DetailFormActions) {
        function execute(actionName, form, element) {
            let workspaceId;
            let extentUri;
            switch (actionName) {
                case "Extent.NavigateTo":
                    extentUri = element.get('uri');
                    workspaceId = element.get('workspaceId');
                    FormActions.extentNavigateTo(workspaceId, extentUri);
                    break;
                case "Item.Delete":
                    FormActions.itemDelete(form.workspace, form.extentUri, form.itemId);
                    break;
                case "ZipExample.CreateExample":
                    const id = element.get('id');
                    ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: id })
                        .done(data => {
                        document.location.reload();
                    });
                    break;
                default:
                    alert("Unknown action type: " + actionName);
                    break;
            }
        }
        DetailFormActions.execute = execute;
    })(DetailFormActions = exports.DetailFormActions || (exports.DetailFormActions = {}));
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
        static itemDelete(workspace, extentUri, itemUri) {
            ApiConnection.post(Settings.baseUrl + "api/items/delete", {
                workspace: workspace,
                itemUri: itemUri
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
});
//# sourceMappingURL=FormActions.js.map