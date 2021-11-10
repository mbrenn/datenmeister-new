import * as Settings from "./Settings";
import * as ApiConnection from "./ApiConnection";
import * as Navigator from "./Navigator";
import {DmObject} from "./Mof";
import * as IIForms from "./Interfaces.Forms";
import {deleteRequest} from "./ApiConnection";



export module DetailFormActions {
    export function requiresConfirmation(actionName: string): boolean {
        if (actionName === "Item.Delete" || actionName === "ExtentsList.DeleteItem") {
            return true;
        } else {
            return false;
        }
    }
    
    export function execute(actionName: string, form: IIForms.IForm, itemUrl: string, element: DmObject) {
        let workspaceId;
        let extentUri;
        switch (actionName) {
            case "Extent.NavigateTo":
                extentUri = element.get('uri');
                workspaceId = element.get('workspaceId');
                FormActions.extentNavigateTo(workspaceId, extentUri);                
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
    static extentNavigateTo(workspace: string, extentUri: string):void {
        document.location.href = Settings.baseUrl + "ItemsOverview/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri);
    }

    static createZipExample(workspace:string) {
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

    static itemDelete(workspace:string, extentUri: string, itemUri: string) {
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

    static extentsListViewItem(workspace:string, extentUri: string, itemId:string) {
        document.location.href = Settings.baseUrl + "Item/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri) + "/" +
            encodeURIComponent(itemId);
    }

    static extentsListDeleteItem(workspace:string, extentUri: string, itemId:string) {

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