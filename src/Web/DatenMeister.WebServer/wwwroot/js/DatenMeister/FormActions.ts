import * as Settings from "./Settings";
import * as ApiConnection from "./ApiConnection";
import * as Navigator from "./Navigator";
import {createJsonFromObject, DmObject} from "./Mof";
import * as IIForms from "./Forms.Interfaces";

export module DetailFormActions {
    export function requiresConfirmation(actionName: string): boolean {
        if (actionName === "Item.Delete"
            || actionName === "ExtentsList.DeleteItem"
            || actionName === "Extent.DeleteExtent") {
            return true;
        } else {
            return false;
        }
    }

    export function execute(actionName: string, form: IIForms.IFormNavigation, itemUrl: string, element: DmObject) {
        let workspaceId;
        let extentUri;
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
            case "Extent.CreateItem":
                let p = new URLSearchParams(window.location.search);
                if (!p.has("extent") || !p.has("workspace")) {
                    alert('There is no extent given');
                } else {
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
                ApiConnection.post(
                    Settings.baseUrl + "api/zip/create",
                    {workspace: id})
                    .done(
                        data => {
                            document.location.reload();
                        });
                break;
            case "Workspace.Extent.Xmi.Create":
                ApiConnection.post(
                    Settings.baseUrl + "api/action/Workspace.Extent.Xmi.Create",
                    {Parameter: createJsonFromObject(element)})
                    .done(data => {
                        document.location.href = Settings.baseUrl
                            + "ItemsOverview/" + encodeURIComponent(element.get("workspaceId")) +
                            "/" + encodeURIComponent(element.get("extentUri"))
                    })
                    .fail(() => {
                        alert('fail');
                    });
                break;
            case "JSON.Item.Alert":
                alert(JSON.stringify(createJsonFromObject(element)));
                break;
            case "Zipcode.Test":
                alert(element.get('zip').toString());
                break;

            default:
                alert("Unknown action type: " + actionName);
                break;
        }
    }
}

interface IDeleteCallbackData {
    success: boolean;
}

export class FormActions {

    static workspaceNavigateTo(workspace: string) {
        document.location.href =
            Settings.baseUrl + "Item/Management/dm:%2F%2F%2F_internal%2Fworkspaces/" + encodeURIComponent(workspace);
    }

    static extentCreateItem(workspace: string, extentUri: string, element: DmObject, metaClass?: string) {
        const json = createJsonFromObject(element);
        ApiConnection.post(
            Settings.baseUrl + "api/items/create_in_extent/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri),
            {
                metaClass: metaClass === undefined ? "" : metaClass,
                properties: json
            }
        ).done(() => {
            document.location.href = Settings.baseUrl +
                "ItemsOverview/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(extentUri);
        });
    }

    static extentNavigateTo(workspace: string, extentUri: string): void {
        document.location.href =
            Settings.baseUrl + "ItemsOverview/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri);
    }

    static extentDelete(workspace: string, extentUri: string): void {
        const parameter = {
            workspace: workspace,
            extentUri: extentUri
        }

        ApiConnection.deleteRequest(
            Settings.baseUrl + "api/extent/delete",
            parameter
        ).done(() => {
            FormActions.workspaceNavigateTo(workspace);
        });
    }

    static createZipExample(workspace: string) {
        ApiConnection.post(
            Settings.baseUrl + "api/zip/create",
            {workspace: workspace})
            .done(
                data => {
                    document.location.reload();
                });
    }

    // Performs the navigation to the given item. The ItemUrl may be a uri or just the id
    static itemNavigateTo(workspace: string, extent: string, itemUrl: string) {
        document.location.href = Settings.baseUrl + "Item/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extent) + "/" +
            encodeURIComponent(itemUrl);
    }

    static itemNew(workspace: string, extentUri: string) {
        ApiConnection.post(
            Settings.baseUrl + "api/items/create",
            {
                workspace: workspace,
                extentUri: extentUri
            })
            .done(
                data => {
                    document.location.reload();
                });
    }

    static itemDelete(workspace: string, extentUri: string, itemUri: string) {
        ApiConnection.deleteRequest<IDeleteCallbackData>(
            Settings.baseUrl + "api/items/delete/"
            + encodeURIComponent(workspace) + "/" +
            encodeURIComponent(itemUri),
            {}
        )
            .done(
                data => {
                    const success = data.success;
                    if (success) {
                        Navigator.navigateToExtent(workspace, extentUri);
                    } else {
                        alert('Deletion was not successful.');
                    }
                });
    }

    static extentsListViewItem(workspace: string, extentUri: string, itemId: string) {
        document.location.href = Settings.baseUrl + "Item/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri) + "/" +
            encodeURIComponent(itemId);
    }

    static extentsListDeleteItem(workspace: string, extentUri: string, itemId: string) {

        ApiConnection.deleteRequest<IDeleteCallbackData>(
            Settings.baseUrl + "api/items/delete/"
            + encodeURIComponent(workspace) + "/" +
            encodeURIComponent(itemId),
            {}
        )
            .done(
                data => {
                    const success = data.success;
                    if (success) {
                        document.location.reload();
                    } else {
                        alert('Deletion was not successful.');
                    }
                });
    }
}