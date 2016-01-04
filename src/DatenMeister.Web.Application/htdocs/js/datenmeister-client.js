define(["require", "exports", "datenmeister-interfaces"], function (require, exports, DMI) {
    var WorkspaceApi;
    (function (WorkspaceApi) {
        function getAllWorkspaces() {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/workspace/all",
                cache: false,
                success: function (data) {
                    callback.resolve(data);
                },
                error: function (data) {
                    callback.reject(data);
                }
            });
            return callback;
        }
        WorkspaceApi.getAllWorkspaces = getAllWorkspaces;
    })(WorkspaceApi = exports.WorkspaceApi || (exports.WorkspaceApi = {}));
    var ExtentApi;
    (function (ExtentApi) {
        function createItem(ws, extentUrl, container) {
            var callback = $.Deferred();
            var postModel = new DMI.PostModels.ItemCreateModel();
            postModel.ws = ws;
            postModel.extent = extentUrl;
            postModel.container = container;
            $.ajax({
                url: "/api/datenmeister/extent/item_create",
                data: postModel,
                method: "POST",
                success: function (data) { callback.resolve(data); },
                error: function (data) { callback.reject(false); }
            });
            return callback;
        }
        ExtentApi.createItem = createItem;
        /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
        function deleteItem(ws, extent, item) {
            var callback = $.Deferred();
            var postModel = new DMI.PostModels.ItemDeleteModel();
            postModel.ws = ws;
            postModel.extent = extent;
            postModel.item = item;
            $.ajax({
                url: "/api/datenmeister/extent/item_delete",
                data: postModel,
                method: "POST",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.reject(false); }
            });
            return callback;
        }
        ExtentApi.deleteItem = deleteItem;
        function getItems(ws, extentUrl, query) {
            var callback = $.Deferred();
            getAjaxForItems(ws, extentUrl, query)
                .done(function (data) {
                callback.resolve(data);
            })
                .fail(function (data) {
                callback.reject(data);
            });
            return callback;
        }
        ExtentApi.getItems = getItems;
        function getAjaxForItems(ws, extentUrl, query) {
            var url = "/api/datenmeister/extent/items?ws=" + encodeURIComponent(ws)
                + "&extent=" + encodeURIComponent(extentUrl);
            if (query !== undefined && query !== null) {
                if (query.searchString !== undefined) {
                    url += "&search=" + encodeURIComponent(query.searchString);
                }
                if (query.offset !== undefined && query.offset !== null) {
                    url += "&o=" + encodeURIComponent(query.offset.toString());
                }
                if (query.amount !== undefined && query.amount !== null) {
                    url += "&a=" + encodeURIComponent(query.amount.toString());
                }
            }
            return $.ajax({
                url: url,
                cache: false
            });
        }
    })(ExtentApi = exports.ExtentApi || (exports.ExtentApi = {}));
    var ItemApi;
    (function (ItemApi) {
        function getItem(ws, extentUrl, itemUrl) {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/item?ws=" + encodeURIComponent(ws)
                    + "&extent=" + encodeURIComponent(extentUrl)
                    + "&item=" + encodeURIComponent(itemUrl),
                cache: false,
                success: function (data) {
                    callback.resolve(data);
                },
                error: function (data) {
                    callback.reject(null);
                }
            });
            return callback;
        }
        ItemApi.getItem = getItem;
        function deleteProperty(ws, extent, item, property) {
            var callback = $.Deferred();
            var postModel = new DMI.PostModels.ItemUnsetPropertyModel();
            postModel.ws = ws;
            postModel.extent = extent;
            postModel.item = item;
            postModel.property = property;
            $.ajax({
                url: "/api/datenmeister/extent/item_unset_property",
                data: postModel,
                method: "POST",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.resolve(false); }
            });
            return callback;
        }
        ItemApi.deleteProperty = deleteProperty;
        function setProperty(ws, extentUrl, itemUrl, property, newValue) {
            var callback = $.Deferred();
            var postModel = new DMI.PostModels.ItemSetPropertyModel();
            postModel.ws = ws;
            postModel.extent = extentUrl;
            postModel.item = itemUrl;
            postModel.property = property;
            postModel.newValue = newValue;
            $.ajax({
                url: "/api/datenmeister/extent/item_set_property",
                data: postModel,
                method: "POST",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.resolve(false); }
            });
            return callback;
        }
        ItemApi.setProperty = setProperty;
    })(ItemApi = exports.ItemApi || (exports.ItemApi = {}));
});
//# sourceMappingURL=datenmeister-client.js.map