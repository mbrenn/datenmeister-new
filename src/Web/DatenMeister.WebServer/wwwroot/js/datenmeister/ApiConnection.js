define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.get = exports.put = exports.deleteRequest = exports.post = void 0;
    function serverError(x) {
        alert("Error during Web-API Connection: " + x.toString());
    }
    function post(uri, data) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: uri,
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json",
                method: "POST"
            }).fail(x => {
                serverError(x.responseText);
                reject();
            }).done(x => resolve(x));
        });
    }
    exports.post = post;
    function deleteRequest(uri, data) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: uri,
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json",
                method: "DELETE"
            }).fail(x => {
                serverError(x.responseText);
                reject();
            }).done(x => resolve(x));
        });
    }
    exports.deleteRequest = deleteRequest;
    function put(uri, data) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: uri,
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json",
                method: "PUT"
            }).fail(x => {
                serverError(x.responseText);
                reject();
            }).done(x => resolve(x));
        });
    }
    exports.put = put;
    function get(uri) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: uri,
                method: "GET"
            }).fail(x => {
                serverError(x.responseText);
                reject();
            }).done(x => resolve(x));
        });
    }
    exports.get = get;
});
//# sourceMappingURL=ApiConnection.js.map