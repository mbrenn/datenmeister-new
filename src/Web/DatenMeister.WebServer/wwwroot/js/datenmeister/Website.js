define(["require", "exports", "./Settings"], function (require, exports, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getItemDetailUri = void 0;
    function getItemDetailUri(element) {
        return Settings.baseUrl + "Item/"
            + encodeURIComponent(element.workspace) + "/"
            + encodeURIComponent(element.uri);
    }
    exports.getItemDetailUri = getItemDetailUri;
});
//# sourceMappingURL=Website.js.map