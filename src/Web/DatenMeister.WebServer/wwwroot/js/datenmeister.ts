
class X {

    y(): void {

    }
}

module Settings
{
    export let baseUrl = "/";
}

module ApiModels
{
    export namespace In {
        export interface ElementPosition {
            workspace: string;
            extentUri: string;
            item: string;
        }
    }
    export namespace Out {
        export interface NamedElement {
            name: string;
        }
    }
}


class NameLoader {
    static loadNameOf(elementPosition: ApiModels.In.ElementPosition): JQuery.jqXHR<ApiModels.Out.NamedElement> {
        return $.ajax(
            Settings.baseUrl + 
            "api/elements/get_name/" + 
            encodeURI(elementPosition.workspace) + "/" +
            encodeURI(elementPosition.extentUri) + "/" + 
            encodeURI(elementPosition.item));
    }
    static loadNameByUri(elementUri:string): JQuery.jqXHR<ApiModels.Out.NamedElement> {
        return $.ajax(
            Settings.baseUrl +
            "api/elements/get_name/" +
            encodeURI(elementUri));
    }
}

module DatenMeister {
    export class DomHelper {
        static injectName(domElement: JQuery<HTMLElement>, elementPosition: ApiModels.In.ElementPosition) {
            domElement.text("YES");

            NameLoader.loadNameOf(elementPosition).done(x => {
                domElement.text(x.name);
            });
        }


        static injectNameByUri(domElement: JQuery<HTMLElement>, elementUri: string) {
            domElement.text("YES");

            NameLoader.loadNameByUri(elementUri).done(x => {
                domElement.text(x.name);
            });
        }
    }
}

