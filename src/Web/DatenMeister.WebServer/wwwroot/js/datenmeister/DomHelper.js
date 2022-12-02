var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./client/Elements"], function (require, exports, ElementClient) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.convertToDom = exports.convertItemWithNameAndIdToDom = exports.debugElementToDom = exports.convertDmObjectToDom = exports.injectNameByUri = void 0;
    function injectNameByUri(domElement, workspaceId, elementUri, parameter) {
        return __awaiter(this, void 0, void 0, function* () {
            const x = yield ElementClient.loadNameByUri(workspaceId, elementUri);
            domElement.empty();
            const paramCall = {};
            if (parameter !== undefined) {
                paramCall.onClick = parameter.onClick;
            }
            domElement.append(convertItemWithNameAndIdToDom(x, paramCall));
        });
    }
    exports.injectNameByUri = injectNameByUri;
    function convertDmObjectToDom(item, params) {
        return __awaiter(this, void 0, void 0, function* () {
            const x = yield ElementClient.loadNameByUri(item.workspace, item.uri);
            return convertItemWithNameAndIdToDom(x, params);
        });
    }
    exports.convertDmObjectToDom = convertDmObjectToDom;
    function debugElementToDom(mofElement, domSelector) {
        const domElement = $(domSelector);
        domElement.empty();
        if (domElement.length > 0) {
            domElement.append(convertToDom(mofElement));
        }
    }
    exports.debugElementToDom = debugElementToDom;
    /*
     * Converts an ItemWithNameAndId to a Dom Element which reflects the content
     */
    function convertItemWithNameAndIdToDom(item, params) {
        let result = $("<span></span>");
        // Checks, if we have valid link information, so the user can click on the item to move to it
        // The link will only be shown when the parameter does inhibit this. The inhibition might be required
        // in case the calling element wants to include its own action.
        const validLinkInformation = item.uri !== undefined && item.workspace !== undefined &&
            item.uri !== "" && item.workspace !== "";
        const inhibitLink = params !== undefined && params.inhibitItemLink;
        if (validLinkInformation && !inhibitLink) {
            const linkElement = $("<a></a>");
            linkElement.text(item.name);
            if ((params === null || params === void 0 ? void 0 : params.onClick) !== undefined) {
                // There is a special click handler, so we execute that one instead of a generic uri
                linkElement.attr('href', '#');
                linkElement.on('click', () => { params.onClick(item); return false; });
            }
            else {
                linkElement.attr("href", "/Item/" + encodeURIComponent(item.workspace) +
                    "/" + encodeURIComponent(item.uri));
            }
            result.append(linkElement);
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
    exports.convertItemWithNameAndIdToDom = convertItemWithNameAndIdToDom;
    function convertToDom(mofElement) {
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
    exports.convertToDom = convertToDom;
});
//# sourceMappingURL=DomHelper.js.map