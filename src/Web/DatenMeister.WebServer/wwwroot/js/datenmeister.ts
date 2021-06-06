
module Settings
{
    export let baseUrl = "/";
}

module ApiConnection {
    export function post<T>(uri: string, data: object): JQuery.jqXHR<T>
    {
        return $.ajax(
            {
                url: uri,
                data: JSON.stringify(data), 
                dataType: "json",
                contentType: "application/json",
                method: "POST"
            }
        );
    }
}

module ApiModels
{
    export namespace In {
        export interface IElementPosition {
            workspace: string;
            extentUri: string;
            item: string;
        }
        
        export interface IDeleteItemParams
        {
            workspace: string;
            extentUri: string;
            itemId: string;
        }

        export interface IDeleteExtentParams
        {
            workspace: string;
            extentUri: string;
        }

        export interface IDeleteWorkspaceParams
        {
            workspace: string;
        }
    }
    export namespace Out {
        export interface INamedElement {
            name: string;
            extentUri?: string;
            workspace?: string;
            itemId?: string;
        }
    }
}

class NameLoader {
    static loadNameOf(elementPosition: ApiModels.In.IElementPosition): JQuery.jqXHR<ApiModels.Out.INamedElement> {
        return $.ajax(
            Settings.baseUrl + 
            "api/elements/get_name/" + 
            encodeURIComponent(elementPosition.workspace) + "/" +
            encodeURIComponent(elementPosition.extentUri) + "/" +
            encodeURIComponent(elementPosition.item));
    }
    
    static loadNameByUri(elementUri:string): JQuery.jqXHR<ApiModels.Out.INamedElement> {
        return $.ajax(
            Settings.baseUrl +
            "api/elements/get_name/" +
            encodeURIComponent(elementUri));
    }
}

module DatenMeister {

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
                    function (data) {
                        document.location.reload();
                    });
        }
        
        static createItem(workspace: string, extentUri: string) {
            ApiConnection.post(
                Settings.baseUrl + "api/items/create",
                {
                    workspace: workspace,
                    extentUri: extentUri
                })
                .done(
                    function (data) {
                        document.location.reload();
                    });
        }
        
        static deleteItem(workspace:string, extentUri: string, itemId:string) {
            ApiConnection.post(
                Settings.baseUrl + "api/items/delete",
                {
                    workspace: workspace,
                    extentUri: extentUri,
                    item: itemId
                })
                .done(
                    function (data) {
                        Navigator.navigateToExtent(workspace, extentUri);
                    });
        }
    }

    export class Navigator {

        static navigateToWorkspaces() {
            document.location.href =
                Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
        }
        
        static navigateToWorkspace(workspace: string) {
            document.location.href =
                Settings.baseUrl + "Item/Management/dm%3A%2F%2F%2F_internal%2Fworkspaces/" + 
                encodeURIComponent(workspace);
        }
        
        static navigateToExtent(workspace: string, extentUri: string) {
            document.location.href =
                Settings.baseUrl + "ItemsOverview/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri);
        }
        
        static navigateToItem(workspace: string, extentUri: string, itemId: string) {
            document.location.href =
                Settings.baseUrl + "Item/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri) + "/" +
                encodeURIComponent(itemId);
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
                if(
                    x.extentUri !== undefined && x.workspace !== undefined
                    && x.extentUri !== "" && x.workspace !== ""
                    && x.itemId !== "" && x.itemId !== undefined)
                {
                    var linkElement = $("<a></a>");
                    linkElement.text(x.name);
                    linkElement.attr(
                        "href", 
                        "/Item/" + encodeURIComponent(x.workspace) + 
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
}

