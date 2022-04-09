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
    exports.convertToDom = exports.debugElementToDom = exports.injectNameByUri = void 0;
    function injectNameByUri(domElement, workspaceId, elementUri) {
        return __awaiter(this, void 0, void 0, function* () {
            const x = yield ElementClient.loadNameByUri(workspaceId, elementUri);
            if (x.extentUri !== undefined && x.workspace !== undefined
                && x.extentUri !== "" && x.workspace !== ""
                && x.id !== "" && x.id !== undefined) {
                const linkElement = $("<a></a>");
                linkElement.text(x.name);
                linkElement.attr("href", "/Item/" + encodeURIComponent(x.workspace) +
                    "/" + encodeURIComponent(x.extentUri + "#" + x.id));
                domElement.empty();
                domElement.append(linkElement);
            }
            else {
                domElement.text(x.name);
            }
        });
    }
    exports.injectNameByUri = injectNameByUri;
    function debugElementToDom(mofElement, domSelector) {
        const domElement = $(domSelector);
        if (domElement.length > 0) {
            domElement.append(convertToDom(mofElement));
        }
    }
    exports.debugElementToDom = debugElementToDom;
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