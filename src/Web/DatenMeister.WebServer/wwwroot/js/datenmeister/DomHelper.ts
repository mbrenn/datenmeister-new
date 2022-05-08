import * as ElementClient from "./client/Elements"
import * as ApiModels from "./ApiModels";
import {DmObject} from "./Mof";
import {loadNameByUri} from "./client/Elements";

export async function injectNameByUri(domElement: JQuery<HTMLElement>, workspaceId: string, elementUri: string) {

    const x = await ElementClient.loadNameByUri(workspaceId, elementUri);

    domElement.empty();
    domElement.append(convertItemWithNameAndIdToDom(x));
    
}

export async function convertDmObjectToDom(item: DmObject, params?: IConvertItemWithNameAndIdParameters) {
    const x = await ElementClient.loadNameByUri(item.workspace, item.uri);
    return convertItemWithNameAndIdToDom(x, params);
}

export function debugElementToDom(mofElement: any, domSelector: string) {
    const domElement = $(domSelector);
    domElement.empty();
    if (domElement.length > 0) {
        domElement.append(convertToDom(mofElement));
    }
}

export interface IConvertItemWithNameAndIdParameters{
    inhibitItemLink?: boolean;
}

/*
 * Converts an ItemWithNameAndId to a Dom Element which reflects the content
 */
export function convertItemWithNameAndIdToDom(item: any, params?: IConvertItemWithNameAndIdParameters): JQuery {
    let result = $("<span></span>");
    
    // Checks, if we have valid link information, so the user can click on the item to move to it
    // The link will only be shown when the parameter does inhibit this. The inhibition might be required
    // in case the calling element wants to include its own action.
    const validLinkInformation = 
        item.extentUri !== undefined && item.workspace !== undefined && 
        item.extentUri !== "" && item.workspace !== "" &&
        item.id !== "" && item.id !== undefined;
    
    const inhibitLink = params !== undefined && params.inhibitItemLink;
    
    if (validLinkInformation && !inhibitLink) {

        const linkElement = $("<a></a>");
        linkElement.text(item.name);
        linkElement.attr(
            "href",
            "/Item/" + encodeURIComponent(item.workspace) +
            "/" + encodeURIComponent(item.extentUri + "#" + item.id));
        result.append(linkElement);
        
    } else {
        result.text(item.name);
    }
    
    // Add the metaclass
    if (item.typeName !== undefined && item.typeName !== null) {
        const metaClassText = $("<span class='dm-metaclass'></span>");
        metaClassText.text(" [" + item.typeName + "]");
        result.append(metaClassText);
    }

    return result;
}

export function convertToDom(mofElement: any): JQuery {
    if (Array.isArray(mofElement)) {
        let count = 0;
        const arrayList = $("<ol></ol>");

        for (let m in mofElement) {
            const li = $("<li></li>");
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
        const span = $("<span class='dm-debug-property-value'></span>");
        span.text(mofElement);
        return span;
    }
}