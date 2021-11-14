define(["require", "exports", "./NameLoader"], function (require, exports, NameLoader) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.convertToDom = exports.debugElementToDom = exports.injectNameByUri = exports.injectName = void 0;
    function injectName(domElement, elementPosition) {
        NameLoader.loadNameOf(elementPosition).done(x => {
            domElement.text(x.name);
        });
    }
    exports.injectName = injectName;
    function injectNameByUri(domElement, elementUri) {
        NameLoader.loadNameByUri(elementUri).done(x => {
            if (x.extentUri !== undefined && x.workspace !== undefined
                && x.extentUri !== "" && x.workspace !== ""
                && x.itemId !== "" && x.itemId !== undefined) {
                const linkElement = $("<a></a>");
                linkElement.text(x.name);
                linkElement.attr("href", "/Item/" + encodeURIComponent(x.workspace) +
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
        }
        else if ((typeof mofElement === "object" || typeof mofElement === "function") && (mofElement !== null)) {
            const asElement = mofElement;
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
        }
        else {
            var span = $("<span></span>");
            span.text(mofElement);
            return span;
        }
    }
    exports.convertToDom = convertToDom;
});
//# sourceMappingURL=DomHelper.js.map