import * as ElementClient from "./Client.Elements"
import * as ApiModels from "./ApiModels";
import {DmObject} from "./Mof";


export function injectName(domElement: JQuery<HTMLElement>, elementPosition: ApiModels.In.IElementPosition) {

    ElementClient.loadNameOf(elementPosition).done(x => {
        domElement.text(x.name);
    });
}

export function injectNameByUri(domElement: JQuery<HTMLElement>, workspaceId: string, elementUri: string) {

    ElementClient.loadNameByUri(workspaceId, elementUri).done(x => {
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

export function debugElementToDom(mofElement: any, domSelector: string) {
    const domElement = $(domSelector);
    if (domElement.length > 0) {
        domElement.append(convertToDom(mofElement));
    }
}

export function convertToDom(mofElement: any): JQuery {
    if (Array.isArray(mofElement)) {
        let count = 0;
        const arrayList = $("<ol></ol>");

        for (var m in mofElement) {
            var li = $("<li></li>");
            if (count > 50) {
                li.text("... (total: " + mofElement.length + ")");
                arrayList.append(li);
                break;
            }

            li.append(convertToDom(mofElement[m]));
            arrayList.append(li);
            count++;
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

        if (asElement.uri !== undefined) {
            const row = $("<li></li>");
            const span = $("<span></span>");
            span.html("<em>Uri</em>: ");
            span.append(convertToDom(asElement.uri));
            row.append(span);

            list.append(row);
        }
        if (asElement.values !== undefined) {
            for (let n in asElement.values) {
                let value = asElement.get(n);

                const row = $("<li></li>");
                const span = $("<span></span>");
                span.text(n + ": ");
                span.append(convertToDom(value));
                row.append(span);

                list.append(row);
            }
        }

        return list;
    } else {
        var span = $("<span class='dm-debug-property-value'></span>");
        span.text(mofElement);
        return span;
    }
}