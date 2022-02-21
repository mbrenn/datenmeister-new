define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.get = exports.put = exports.deleteRequest = exports.post = void 0;
    function serverError(x) {
        alert("Error during Web-API Connection: " + x.toString());
    }
    function post(uri, data) {
        return $.ajax({
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "POST"
        }).fail(x => serverError(x.responseText));
    }
    exports.post = post;
    function deleteRequest(uri, data) {
        return $.ajax({
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "DELETE"
        }).fail(x => serverError(x.responseText));
    }
    exports.deleteRequest = deleteRequest;
    function put(uri, data) {
        return $.ajax({
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "POST"
        }).fail(x => serverError(x.responseText));
    }
    exports.put = put;
    function get(uri) {
        return $.ajax({
            url: uri,
            method: "GET"
        }).fail(x => serverError(x.responseText));
    }
    exports.get = get;
});
//# sourceMappingURL=ApiConnection.js.map