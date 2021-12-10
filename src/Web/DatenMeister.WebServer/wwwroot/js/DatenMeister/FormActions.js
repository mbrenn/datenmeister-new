define(["require", "exports", "./Settings", "./ApiConnection", "./Navigator", "./Mof", "./ExtentControllerClient"], function (require, exports, Settings, ApiConnection, Navigator, Mof_1, ECClient) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.FormActions = exports.DetailFormActions = void 0;
    var DetailFormActions;
    (function (DetailFormActions) {
        function requiresConfirmation(actionName) {
            if (actionName === "Item.Delete"
                || actionName === "ExtentsList.DeleteItem"
                || actionName === "Extent.DeleteExtent") {
                return true;
            }
            else {
                return false;
            }
        }
        DetailFormActions.requiresConfirmation = requiresConfirmation;
        function execute(actionName, form, itemUrl, element) {
            let workspaceId;
            let extentUri;
            let p = new URLSearchParams(window.location.search);
            switch (actionName) {
                case "Extent.NavigateTo":
                    extentUri = element.get('uri');
                    workspaceId = element.get('workspaceId');
                    FormActions.extentNavigateTo(workspaceId, extentUri);
                    break;
                case "Extent.DeleteExtent":
                    extentUri = element.get('uri');
                    workspaceId = element.get('workspaceId');
                    FormActions.extentDelete(workspaceId, extentUri);
                    break;
                case "Extent.Properties":
                    extentUri = element.get('uri');
                    workspaceId = element.get('workspaceId');
                    FormActions.extentNavigateToProperties(workspaceId, extentUri);
                    break;
                case "Extent.Properties.Update":
                    if (!p.has("extent") || !p.has("workspace")) {
                        alert('There is no extent given');
                    }
                    else {
                        const workspace = p.get('workspace');
                        const extentUri = p.get('extent');
                        FormActions.extentUpdateExtentProperties(workspace, extentUri, element);
                    }
                    break;
                case "Extent.CreateItem":
                    if (!p.has("extent") || !p.has("workspace")) {
                        alert('There is no extent given');
                    }
                    else {
                        const workspace = p.get('workspace');
                        const extentUri = p.get('extent');
                        FormActions.extentCreateItem(workspace, extentUri, element);
                    }
                    break;
                case "ExtentsList.ViewItem":
                    FormActions.itemNavigateTo(form.workspace, form.extentUri, element.uri);
                    break;
                case "ExtentsList.DeleteItem":
                    FormActions.extentsListDeleteItem(form.workspace, form.extentUri, itemUrl);
                    break;
                case "Item.Delete":
                    FormActions.itemDelete(form.workspace, form.extentUri, itemUrl);
                    break;
                case "ZipExample.CreateExample":
                    const id = element.get('id');
                    ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: id })
                        .done(data => {
                        document.location.reload();
                    });
                    break;
                case "Workspace.Extent.Xmi.Create":
                    ApiConnection.post(Settings.baseUrl + "api/action/Workspace.Extent.Xmi.Create", { Parameter: (0, Mof_1.createJsonFromObject)(element) })
                        .done(data => {
                        document.location.href = Settings.baseUrl
                            + "ItemsOverview/" + encodeURIComponent(element.get("workspaceId")) +
                            "/" + encodeURIComponent(element.get("extentUri"));
                    })
                        .fail(() => {
                        alert('fail');
                    });
                    break;
                case "JSON.Item.Alert":
                    alert(JSON.stringify((0, Mof_1.createJsonFromObject)(element)));
                    break;
                case "Zipcode.Test":
                    alert(element.get('zip').toString());
                    break;
                default:
                    alert("Unknown action type: " + actionName);
                    break;
            }
        }
        DetailFormActions.execute = execute;
    })(DetailFormActions = exports.DetailFormActions || (exports.DetailFormActions = {}));
    class FormActions {
        static workspaceNavigateTo(workspace) {
            document.location.href =
                Settings.baseUrl + "Item/Management/dm:%2F%2F%2F_internal%2Fworkspaces/" + encodeURIComponent(workspace);
        }
        static extentCreateItem(workspace, extentUri, element, metaClass) {
            const json = (0, Mof_1.createJsonFromObject)(element);
            ApiConnection.post(Settings.baseUrl + "api/items/create_in_extent/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri), {
                metaClass: metaClass === undefined ? "" : metaClass,
                properties: json
            }).done(() => {
                document.location.href = Settings.baseUrl +
                    "ItemsOverview/" +
                    encodeURIComponent(workspace) +
                    "/" +
                    encodeURIComponent(extentUri);
            });
        }
        static extentNavigateTo(workspace, extentUri) {
            document.location.href =
                Settings.baseUrl + "ItemsOverview/" +
                    encodeURIComponent(workspace) + "/" +
                    encodeURIComponent(extentUri);
        }
        static extentNavigateToProperties(workspace, extentUri) {
            document.location.href =
                Settings.baseUrl +
                    "ItemAction/Extent.Properties.Update/" +
                    "?workspace=" +
                    encodeURIComponent(workspace) +
                    "&extent=" +
                    encodeURIComponent(extentUri);
        }
        static extentUpdateExtentProperties(workspace, extentUri, element) {
            ECClient.setProperties(workspace, extentUri, element);
        }
        static extentDelete(workspace, extentUri) {
            const parameter = {
                workspace: workspace,
                extentUri: extentUri
            };
            ApiConnection.deleteRequest(Settings.baseUrl + "api/extent/delete", parameter).done(() => {
                FormActions.workspaceNavigateTo(workspace);
            });
        }
        static createZipExample(workspace) {
            ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: workspace })
                .done(data => {
                document.location.reload();
            });
        }
        // Performs the navigation to the given item. The ItemUrl may be a uri or just the id
        static itemNavigateTo(workspace, extent, itemUrl) {
            document.location.href = Settings.baseUrl + "Item/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extent) + "/" +
                encodeURIComponent(itemUrl);
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
            ApiConnection.deleteRequest(Settings.baseUrl + "api/items/delete/"
                + encodeURIComponent(workspace) + "/" +
                encodeURIComponent(itemUri), {})
                .done(data => {
                const success = data.success;
                if (success) {
                    Navigator.navigateToExtent(workspace, extentUri);
                }
                else {
                    alert('Deletion was not successful.');
                }
            });
        }
        static extentsListViewItem(workspace, extentUri, itemId) {
            document.location.href = Settings.baseUrl + "Item/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri) + "/" +
                encodeURIComponent(itemId);
        }
        static extentsListDeleteItem(workspace, extentUri, itemId) {
            ApiConnection.deleteRequest(Settings.baseUrl + "api/items/delete/"
                + encodeURIComponent(workspace) + "/" +
                encodeURIComponent(itemId), {})
                .done(data => {
                const success = data.success;
                if (success) {
                    document.location.reload();
                }
                else {
                    alert('Deletion was not successful.');
                }
            });
        }
    }
    exports.FormActions = FormActions;
});
//# sourceMappingURL=FormActions.js.map