import * as Settings from "./Settings";
import * as ApiConnection from "./ApiConnection";
import * as Navigator from "./Navigator";
import {createJsonFromObject, DmObject} from "./Mof";
import * as IIForms from "./Forms.Interfaces";
import * as ECClient from "./ExtentControllerClient";

export module DetailFormActions {

    // Loads the object being used for the action. 
    export function loadObjectForAction(actionName: string): JQueryDeferred<DmObject> | undefined {

        let p = new URLSearchParams(window.location.search);

        if (actionName === "Extent.Properties.Update") {

            const workspace = p.get('workspace');
            const extentUri = p.get('extent');

            return ECClient.getProperties(workspace, extentUri);
        }

        if (actionName === "Extent.CreateItem" || actionName === "Extent.CreateItemInProperty") {
            const metaclass = p.get('metaclass');
            const deferLoadObjectForAction = $.Deferred<DmObject>();
            const result = new DmObject();

            if (metaclass !== undefined) {
                result.setMetaClassByUri(metaclass);
            }

            deferLoadObjectForAction.resolve(result);
            return deferLoadObjectForAction;
        }

        if (actionName === "Zipcode.Test") {
            const deferLoadObjectForAction = $.Deferred<DmObject>();
            const result = new DmObject();

            result.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
            deferLoadObjectForAction.resolve(result);
            return deferLoadObjectForAction;
        }

        return undefined;
    }

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
                } else {
                    const workspace = p.get('workspace');
                    const extentUri = p.get('extent');
                    FormActions.extentUpdateExtentProperties(workspace, extentUri, element);
                }
                break;
            case "Extent.CreateItem":
                if (!p.has("extent") || !p.has("workspace")) {
                    alert('There is no extent given');
                } else {
                    const workspace = p.get('workspace');
                    const extentUri = p.get('extent');
                    FormActions.extentCreateItem(workspace, extentUri, element);
                }
                break;
            case "Extent.CreateItemInProperty":
                if (!p.has("itemUrl") || !p.has("workspace") || !p.has("property")) {
                    alert('There is no itemUrl given');
                } else {
                    const workspace = p.get('workspace');
                    const itemUrl = p.get('itemUrl');
                    const property = p.get('property');
                    FormActions.extentCreateItemInProperty(workspace, itemUrl, property, element);
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
                ApiConnection.post(
                    Settings.baseUrl + "api/zip/create",
                    {workspace: id})
                    .done(
                        data => {
                            document.location.reload();
                        });
                break;
            case "Workspace.Extent.Xmi.Create":
                ApiConnection.post<any>(
                    Settings.baseUrl + "api/action/Workspace.Extent.Xmi.Create",
                    {Parameter: createJsonFromObject(element)})
                    .done(data => {
                        if (data.success) {
                            document.location.href = Settings.baseUrl
                                + "ItemsOverview/" + encodeURIComponent(element.get("workspaceId")) +
                                "/" + encodeURIComponent(element.get("extentUri"))
                        } else {
                            alert(data.reason);
                        }
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
        if (metaClass === undefined) {
            metaClass = element.metaClass.uri
        }

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

    static extentCreateItemInProperty(workspace: string, itemUrl: string, property: string, element: DmObject, metaClass?: string) {
        if (metaClass === undefined) {
            metaClass = element.metaClass.uri
        }

        const json = createJsonFromObject(element);
        ApiConnection.post(
            Settings.baseUrl + "api/items/create_child/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(itemUrl),
            {
                metaClass: metaClass === undefined ? "" : metaClass,
                property: property,
                asList: true,
                properties: json
            }
        ).done(() => {
            Navigator.navigateToItemByUrl(workspace, itemUrl);
        });
    }

    static extentNavigateTo(workspace: string, extentUri: string): void {
        document.location.href =
            Settings.baseUrl + "ItemsOverview/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri);
    }

    static extentNavigateToProperties(workspace: string, extentUri: string) {
        document.location.href =
            Settings.baseUrl +
            "ItemAction/Extent.Properties.Navigate/" +
            encodeURIComponent("dm:///_internal/forms/internal#DatenMeister.Extent.Properties") +
            "?workspace=" +
            encodeURIComponent(workspace) +
            "&extent=" +
            encodeURIComponent(extentUri);
    }
    
    static extentUpdateExtentProperties(workspace: string, extentUri: string, element: DmObject): void{
        ECClient.setProperties(workspace, extentUri, element).done(() => FormActions.extentNavigateTo(workspace, extentUri));
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
    static itemNavigateTo(workspace: string, itemUrl: string) {
        Navigator.navigateToItemByUrl(
            workspace,
            itemUrl);
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