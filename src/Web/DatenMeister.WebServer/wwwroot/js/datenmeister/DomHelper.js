import * as ElementClient from "./client/Elements.js";
import * as Navigator from "./Navigator.js";
export async function injectNameByObject(domElement, element) {
    return injectNameByUri(domElement, element.workspace, element.uri);
}
export async function injectNameByUri(domElement, workspaceId, elementUri, parameter) {
    const x = await ElementClient.loadNameByUri(workspaceId, elementUri);
    domElement.empty();
    const paramCall = {};
    if (parameter !== undefined) {
        paramCall.onClick = parameter.onClick;
    }
    domElement.append(convertItemWithNameAndIdToDom(x, paramCall));
}
export async function convertDmObjectToDom(item, params) {
    const x = await ElementClient.loadNameByUri(item.workspace, item.uri);
    return convertItemWithNameAndIdToDom(x, params);
}
export function debugElementToDom(mofElement, domSelector) {
    const domElement = $(domSelector);
    domElement.empty();
    if (domElement.length > 0) {
        domElement.append(convertToDom(mofElement));
    }
}
/*
 * Converts an ItemWithNameAndId to a Dom Element which reflects the content
 */
export function convertItemWithNameAndIdToDom(item, params) {
    let result = $("<span></span>");
    // Checks, if we have valid link information, so the user can click on the item to move to it
    // The link will only be shown when the parameter does inhibit this. The inhibition might be required
    // in case the calling element wants to include its own action.
    const validLinkInformation = item.uri !== undefined && item.workspace !== undefined &&
        item.uri !== "" && item.workspace !== "";
    const inhibitLink = params !== undefined && params.inhibitItemLink;
    const inhibitEditLink = params !== undefined && params.inhibitEditItemLink;
    if (validLinkInformation) {
        // The inhibition link
        if (!inhibitLink) {
            const linkElement = $("<a></a>");
            linkElement.text(item.name);
            if (params?.onClick !== undefined) {
                // There is a special click handler, so we execute that one instead of a generic uri
                linkElement.attr('href', '#');
                linkElement.on('click', () => {
                    params.onClick(item);
                    return false;
                });
            }
            else {
                linkElement.attr("href", Navigator.getLinkForNavigateToItemByUrl(item.workspace, item.uri));
                linkElement.on('click', () => {
                    Navigator.navigateToItemByUrl(item.workspace, item.uri);
                });
            }
            result.append(linkElement);
        }
        else {
            const linkElement = $("<span></span>");
            linkElement.text(item.name);
            result.append(linkElement);
        }
        // The Edit link
        if (!inhibitEditLink) {
            const linkElement = $("<a class='dm-item-edit-button'>✒️</a>");
            linkElement.attr('href', Navigator.getLinkForNavigateToItemByUrl(item.workspace, item.uri, { editMode: true }));
            linkElement.on('click', () => {
                Navigator.navigateToItemByUrl(item.workspace, item.uri, { editMode: true });
                return false;
            });
            result.append(linkElement);
        }
    }
    else {
        result.text(item.name);
    }
    // Add the metaclass
    if (item.metaClassName !== undefined && item.metaClassName !== null) {
        const metaClassText = $("<span class='dm-metaclass'></span>");
        metaClassText
            .attr('title', item.metaClassUri)
            .text(" [" + item.metaClassName + "]");
        result.append(metaClassText);
    }
    return result;
}
export function convertToDom(mofElement) {
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
    }
    else if ((typeof mofElement === "object" || typeof mofElement === "function") && (mofElement !== null)) {
        const asElement = mofElement;
        const list = $("<ul></ul>");
        if (asElement.metaClass !== undefined && asElement.metaClass.fullName !== undefined) {
            const row = $("<li><em></em></li>");
            $("em", row)
                .attr('title', asElement.metaClass.uri)
                .text("[[MetaClass: " + asElement.metaClass.fullName + "]]");
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
        if (asElement.getPropertyValues !== undefined) {
            for (let n in asElement.getPropertyValues()) {
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
    }
    else {
        const span = $("<span class='dm-debug-property-value'></span>");
        span.text(mofElement);
        return span;
    }
}
//# sourceMappingURL=DomHelper.js.map