define(["require", "exports", "./Settings"], function (require, exports, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadNameByUri = exports.loadNameOf = void 0;
    function loadNameOf(elementPosition) {
        return $.ajax(Settings.baseUrl +
            "api/elements/get_name/" +
            encodeURIComponent(elementPosition.workspace) + "/" +
            encodeURIComponent(elementPosition.extentUri) + "/" +
            encodeURIComponent(elementPosition.item));
    }
    exports.loadNameOf = loadNameOf;
    function loadNameByUri(elementUri) {
        return $.ajax(Settings.baseUrl +
            "api/elements/get_name/" +
            encodeURIComponent(elementUri));
    }
    exports.loadNameByUri = loadNameByUri;
});
//# sourceMappingURL=NameLoader.js.map