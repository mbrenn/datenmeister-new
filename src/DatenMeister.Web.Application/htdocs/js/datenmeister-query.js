"use strict";
var DMClient = require("datenmeister-client");
var ItemsFromExtentProvider = (function () {
    function ItemsFromExtentProvider(ws, extent) {
        this.ws = ws;
        this.extent = extent;
    }
    ItemsFromExtentProvider.prototype.performQuery = function (query) {
        return DMClient.ExtentApi.getItems(this.ws, this.extent, query);
    };
    return ItemsFromExtentProvider;
}());
exports.ItemsFromExtentProvider = ItemsFromExtentProvider;
