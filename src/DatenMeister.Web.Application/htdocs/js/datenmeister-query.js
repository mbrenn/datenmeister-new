define(["require", "exports", "./datenmeister-client"], function (require, exports, DMClient) {
    "use strict";
    exports.__esModule = true;
    var ItemsFromExtentProvider = (function () {
        function ItemsFromExtentProvider(ws, extent) {
            this.ws = ws;
            this.ext = extent;
        }
        ItemsFromExtentProvider.prototype.performQuery = function (query) {
            return DMClient.ExtentApi.getItems(this.ws, this.ext, query);
        };
        return ItemsFromExtentProvider;
    }());
    exports.ItemsFromExtentProvider = ItemsFromExtentProvider;
});
//# sourceMappingURL=datenmeister-query.js.map