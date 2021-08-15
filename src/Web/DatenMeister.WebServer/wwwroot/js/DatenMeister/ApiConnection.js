define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.get = exports.put = exports.post = void 0;
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
    function put(uri, data) {
        return $.ajax({
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "POST"
        });
    }
    exports.put = put;
    function get(uri) {
        return $.ajax({
            url: uri,
            method: "GET"
        });
    }
    exports.get = get;
});
//# sourceMappingURL=ApiConnection.js.map