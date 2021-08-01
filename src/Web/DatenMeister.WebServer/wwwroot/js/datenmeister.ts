import * as ApiConnection from "./DatenMeister/ApiConnection";
import * as ApiModels from "./DatenMeister/ApiModels"
import * as Settings from "./DatenMeister/Settings"
import * as NameLoader from "./DatenMeister/NameLoader"
import * as Navigator from "./DatenMeister/Navigator"

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
    
    static itemDelete(workspace:string, extentUri: string, itemId:string) {
        ApiConnection.post(
            Settings.baseUrl + "api/items/delete",
            {
                workspace: workspace,
                extentUri: extentUri,
                itemId: itemId
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


export class DomHelper {
    static injectName(domElement: JQuery<HTMLElement>, elementPosition: ApiModels.In.IElementPosition) {

        NameLoader.loadNameOf(elementPosition).done(x => {
            domElement.text(x.name);
        });
    }

    static injectNameByUri(domElement: JQuery<HTMLElement>, elementUri: string) {

        NameLoader.loadNameByUri(elementUri).done(x => {
            if (
                x.extentUri !== undefined && x.workspace !== undefined
                && x.extentUri !== "" && x.workspace !== ""
                && x.itemId !== "" && x.itemId !== undefined) {
                const linkElement = $("<a></a>");
                linkElement.text(x.name);
                linkElement.attr(
                    "href",
                    "/Item/" + encodeURIComponent(x.workspace) +
                    "/" + encodeURIComponent(x.extentUri) +
                    "/" + encodeURIComponent(x.itemId));
                domElement.empty();
                domElement.append(linkElement);
            } else {
                domElement.text(x.name);
            }
        });
    }
}