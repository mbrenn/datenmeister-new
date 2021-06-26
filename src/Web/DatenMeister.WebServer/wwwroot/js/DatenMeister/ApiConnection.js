define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.post = void 0;
    function post(uri, data) {
        return $.ajax({
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "POST"
        });
    }
    exports.post = post;
});
//# sourceMappingURL=ApiConnection.js.map