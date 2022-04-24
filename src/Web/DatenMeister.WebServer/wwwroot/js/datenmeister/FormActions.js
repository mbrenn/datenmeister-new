var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Settings", "./ApiConnection", "./Navigator", "./Mof", "./client/Extents", "./client/Items", "./forms/DetailForm"], function (require, exports, Settings, ApiConnection, Navigator, Mof_1, ECClient, ItemClient, DetailForm_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.FormActions = exports.DetailFormActions = void 0;
    var DetailFormActions;
    (function (DetailFormActions) {
        // Loads the object being used for the action. 
        function loadObjectForAction(actionName) {
            let p = new URLSearchParams(window.location.search);
            if (actionName === "Extent.Properties.Update") {
                const workspace = p.get('workspace');
                const extentUri = p.get('extent');
                return ECClient.getProperties(workspace, extentUri);
            }
            if (actionName === "Extent.CreateItem" || actionName === "Extent.CreateItemInProperty") {
                const metaclass = p.get('metaclass');
                return new Promise(resolve => {
                    const result = new Mof_1.DmObject();
                    if (metaclass !== undefined && metaclass !== null) {
                        result.setMetaClassByUri(metaclass);
                    }
                    resolve(result);
                });
            }
            if (actionName === "Zipcode.Test") {
                return new Promise(resolve => {
                    const result = new Mof_1.DmObject();
                    result.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
                    resolve(result);
                });
            }
            if (actionName === "Workspace.Extent.Xmi.Create") {
                return new Promise(resolve => {
                    const result = new Mof_1.DmObject();
                    result.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");
                    result.set("workspaceId", p.get('workspaceId'));
                    resolve(result);
                });
            }
            return undefined;
        }
        DetailFormActions.loadObjectForAction = loadObjectForAction;
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
        // Calls to execute the form actions.
        // actionName: Name of the action to be executed. This is a simple string describing the action
        // form: The form which was used to trigger the action
        // itemUrl: The url of the item whose action will be executed
        // element: The element which is reflected within the form
        // parameter: These parameter are retrieved from the actionForm definition from the server and are forwarded
        //    This supports the server to provide additional paramater for an action button
        // submitMethod: Describes which button the user has clicked
        function execute(actionName, form, itemUrl, element, parameter, submitMethod) {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
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
                            yield FormActions.extentCreateItem(workspace, extentUri, element, undefined, submitMethod);
                        }
                        break;
                    case "Extent.CreateItemInProperty":
                        if (!p.has("itemUrl") || !p.has("workspace") || !p.has("property")) {
                            alert('There is no itemUrl given');
                        }
                        else {
                            const workspace = p.get('workspace');
                            const itemUrl = p.get('itemUrl');
                            const property = p.get('property');
                            yield FormActions.extentCreateItemInProperty(workspace, itemUrl, property, element);
                        }
                        break;
                    case "ExtentsList.ViewItem":
                        FormActions.itemNavigateTo(form.workspace, element.uri);
                        break;
                    case "ExtentsList.DeleteItem":
                        FormActions.extentsListDeleteItem(form.workspace, form.extentUri, itemUrl);
                        break;
                    case "Item.Delete":
                        FormActions.itemDelete(form.workspace, form.extentUri, itemUrl);
                        break;
                    case "ZipExample.CreateExample":
                        const id = element.get('id');
                        yield ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: id })
                            .then(data => {
                            document.location.reload();
                        });
                        break;
                    case "Workspace.Extent.Xmi.Create.Navigate":
                        const workspaceIdParameter = (_a = parameter === null || parameter === void 0 ? void 0 : parameter.get('workspaceId')) !== null && _a !== void 0 ? _a : "";
                        FormActions.workspaceExtentCreateXmiNavigateTo(workspaceIdParameter);
                        break;
                    case "Workspace.Extent.Xmi.Create":
                        yield ApiConnection.post(Settings.baseUrl + "api/action/Workspace.Extent.Xmi.Create", { Parameter: (0, Mof_1.createJsonFromObject)(element) })
                            .then(data => {
                            if (data.success) {
                                document.location.href = Settings.baseUrl
                                    + "ItemsOverview/" + encodeURIComponent(element.get("workspaceId")) +
                                    "/" + encodeURIComponent(element.get("extentUri"));
                            }
                            else {
                                alert(data.reason);
                            }
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
            });
        }
        DetailFormActions.execute = execute;
    })(DetailFormActions = exports.DetailFormActions || (exports.DetailFormActions = {}));
    class FormActions {
        static workspaceExtentCreateXmiNavigateTo(workspaceId) {
            document.location.href =
                Settings.baseUrl + "ItemAction/Workspace.Extent.Xmi.Create?workspaceId=" + encodeURIComponent(workspaceId);
        }
        static extentCreateItem(workspace, extentUri, element, metaClass, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                if (metaClass === undefined) {
                    metaClass = element.metaClass.uri;
                }
                const json = (0, Mof_1.createJsonFromObject)(element);
                const newItem = yield ItemClient.createItemInExtent(workspace, extentUri, {
                    metaClass: metaClass === undefined ? "" : metaClass,
                    properties: element
                });
                if (submitMethod === DetailForm_1.SubmitMethod.Save) {
                    // If user has clicked on the save button (without closing), the form shall just be updated
                    Navigator.navigateToItemByUrl(workspace, newItem.itemId);
                }
                else {
                    // Else, move to the overall items overview
                    document.location.href = Settings.baseUrl +
                        "ItemsOverview/" +
                        encodeURIComponent(workspace) +
                        "/" +
                        encodeURIComponent(extentUri);
                }
            });
        }
        static workspaceNavigateTo(workspace) {
            document.location.href =
                Settings.baseUrl + "Item/Management/dm:%2F%2F%2F_internal%2Fworkspaces/" + encodeURIComponent(workspace);
        }
        static extentCreateItemInProperty(workspace, itemUrl, property, element, metaClass) {
            return __awaiter(this, void 0, void 0, function* () {
                if (metaClass === undefined) {
                    metaClass = element.metaClass.uri;
                }
                const json = (0, Mof_1.createJsonFromObject)(element);
                yield ApiConnection.post(Settings.baseUrl + "api/items/create_child/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(itemUrl), {
                    metaClass: (metaClass === undefined || metaClass === undefined) ? "" : metaClass,
                    property: property,
                    asList: true,
                    properties: json
                });
                Navigator.navigateToItemByUrl(workspace, itemUrl);
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
                    "ItemAction/Extent.Properties.Navigate/" +
                    encodeURIComponent("dm:///_internal/forms/internal#DatenMeister.Extent.Properties") +
                    "?workspace=" +
                    encodeURIComponent(workspace) +
                    "&extent=" +
                    encodeURIComponent(extentUri);
        }
        static extentUpdateExtentProperties(workspace, extentUri, element) {
            ECClient.setProperties(workspace, extentUri, element).then(() => FormActions.extentNavigateTo(workspace, extentUri));
        }
        static extentDelete(workspace, extentUri) {
            const parameter = {
                workspace: workspace,
                extentUri: extentUri
            };
            ApiConnection.deleteRequest(Settings.baseUrl + "api/extent/delete", parameter).then(() => {
                FormActions.workspaceNavigateTo(workspace);
            });
        }
        static createZipExample(workspace) {
            ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: workspace })
                .then(data => {
                document.location.reload();
            });
        }
        // Performs the navigation to the given item. The ItemUrl may be a uri or just the id
        static itemNavigateTo(workspace, itemUrl) {
            Navigator.navigateToItemByUrl(workspace, itemUrl);
        }
        static itemNew(workspace, extentUri) {
            ApiConnection.post(Settings.baseUrl + "api/items/create", {
                workspace: workspace,
                extentUri: extentUri
            })
                .then(data => {
                document.location.reload();
            });
        }
        static itemDelete(workspace, extentUri, itemUri) {
            return __awaiter(this, void 0, void 0, function* () {
                const data = yield ItemClient.deleteItem(workspace, itemUri);
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
                .then(data => {
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