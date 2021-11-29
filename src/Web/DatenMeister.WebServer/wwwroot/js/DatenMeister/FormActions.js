define(["require", "exports", "./Settings", "./ApiConnection", "./Navigator", "./Mof"], function (require, exports, Settings, ApiConnection, Navigator, Mof_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.FormActions = exports.DetailFormActions = void 0;
    var DetailFormActions;
    (function (DetailFormActions) {
        function requiresConfirmation(actionName) {
            if (actionName === "Item.Delete" || actionName === "ExtentsList.DeleteItem") {
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
            switch (actionName) {
                case "Extent.NavigateTo":
                    extentUri = element.get('uri');
                    workspaceId = element.get('workspaceId');
                    FormActions.extentNavigateTo(workspaceId, extentUri);
                    break;
                case "Extent.CreateItem":
                    let p = new URLSearchParams(window.location.search);
                    if (!p.has("extent") || !p.has("workspace")) {
                        alert('There is no extent given');
                    }
                    else {
                        const extentUri = p.get('extent');
                        const workspace = p.get('extent');
                        FormActions.extentCreateItem(workspace, extentUri, element);
                    }
                    alert(window.location.search);
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
        static extentCreateItem(workspace, extentUri, element, metaClass) {
            alert(workspace);
            alert(extentUri);
            const json = (0, Mof_1.createJsonFromObject)(element);
            ApiConnection.post(Settings.baseUrl + "create_in_extent/" + encodeURI(workspace) + "/" + encodeURI(extentUri), {
                metaClass: metaClass === undefined ? "" : metaClass,
                properties: json
            }).done(() => {
                document.location.href = Settings.baseUrl
                    + "ItemsOverview/" + encodeURI(workspace) +
                    "/" + encodeURI(extentUri);
            });
        }
        static extentNavigateTo(workspace, extentUri) {
            document.location.href =
                Settings.baseUrl + "ItemsOverview/" +
                    encodeURIComponent(workspace) + "/" +
                    encodeURIComponent(extentUri);
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