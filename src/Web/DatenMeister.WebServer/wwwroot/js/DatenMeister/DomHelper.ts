import * as NameLoader from "./NameLoader";
import * as ApiModels from "./ApiModels";
import { DmObject } from "./Mof";


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

export function debugElementToDom(mofElement: DmObject, domSelector: string) {
    const domElement = $(domSelector);
    if (domElement.length > 0) {
        domElement.append(convertToDom(mofElement));
    }
}

export function convertToDom(mofElement: any): JQuery{
    if (Array.isArray(mofElement)) {
        var arrayList = $("<ol></ol>");

        for (var m in mofElement) {
            var li = $("<li></li>");
            li.append(convertToDom(mofElement[m]));
            arrayList.append(li);
        }

        return arrayList;

    } else if ((typeof mofElement === "object" || typeof mofElement === "function") && (mofElement !== null)) {
        const asElement = mofElement as DmObject;
        const list = $("<ul></ul>");

        if (asElement.metaClass !== undefined && asElement.metaClass.fullName !== undefined) {
            const row = $("<li><em></em></li>");
            $("em", row).text("[[MetaClass: " + asElement.metaClass.fullName + "]]");
            list.append(row);
        }

        for (var n in mofElement.values) {
            let value = mofElement.get(n);

            const row = $("<li></li>");
            const span = $("<span></span>");
            span.text(n + ": ");
            span.append(convertToDom(value));
            row.append(span);

            list.append(row);
        }

        return list;
    } else {
        var span = $("<span></span>");
        span.text(mofElement);
        return span;
    }
}