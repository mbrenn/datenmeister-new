import * as NameLoader from "./NameLoader";
import * as ApiModels from "./ApiModels";


export function injectName(domElement: JQuery<HTMLElement>, elementPosition: ApiModels.In.IElementPosition) {

    NameLoader.loadNameOf(elementPosition).done(x => {
        domElement.text(x.name);
    });
}
export function injectNameByUri(domElement: JQuery<HTMLElement>, elementUri: string) {

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