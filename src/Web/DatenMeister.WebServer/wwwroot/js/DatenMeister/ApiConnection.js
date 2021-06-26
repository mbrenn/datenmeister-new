define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ApiConnection = void 0;
    class ApiConnection {
        static post(uri, data) {
            return $.ajax({
                url: uri,
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json",
                method: "POST"
            });
        }
    }
    exports.ApiConnection = ApiConnection;
});
//# sourceMappingURL=ApiConnection.js.map