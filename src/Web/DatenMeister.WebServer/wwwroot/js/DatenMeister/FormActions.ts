import * as Settings from "./Settings";
import * as ApiConnection from "./ApiConnection";
import * as Navigator from "./Navigator";
import {DmObject} from "./Mof";
import IIForms = require("./Interfaces.Forms");

export module DetailFormActions {
    export function execute(actionName: string, form: IIForms.IForm, element: DmObject) {
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
        ApiConnection.post(
            Settings.baseUrl + "api/items/delete",
            {
                workspace: workspace,
                itemUri: itemUri
            })
            .done(
                data => {
                    Navigator.navigateToExtent(workspace, extentUri);
                });
    }

    static extentsListViewItem(workspace:string, extentUri: string, itemId:string) {
        document.location.href = Settings.baseUrl + "Item/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri) + "/" +
            encodeURIComponent(itemId);
    }

    static extentsListDeleteItem(workspace:string, extentUri: string, itemId:string) {
        ApiConnection.post(
            Settings.baseUrl + "api/items/delete_from_extent",
            {
                workspace: workspace,
                extentUri: extentUri,
                itemId: itemId
            })
            .done(
                data => {
                    document.location.reload();
                });
    }
}