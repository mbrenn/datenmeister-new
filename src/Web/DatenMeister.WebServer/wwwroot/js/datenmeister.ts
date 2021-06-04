
module Settings
{
    export let baseUrl = "/";
}

module ApiModels
{
    export namespace In {
        export interface IElementPosition {
            workspace: string;
            extentUri: string;
            item: string;
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

