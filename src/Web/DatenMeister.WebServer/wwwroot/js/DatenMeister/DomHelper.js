define(["require", "exports", "./NameLoader"], function (require, exports, NameLoader) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.injectNameByUri = exports.injectName = void 0;
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
});
//# sourceMappingURL=DomHelper.js.map