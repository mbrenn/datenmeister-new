define(["require", "exports", "./datenmeister-clientinterface"], function (require, exports, DMC) {
    "use strict";
    exports.__esModule = true;
    var ClientApi;
    (function (ClientApi) {
        function getPlugins() {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/client/get_plugins",
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
        ClientApi.getPlugins = getPlugins;
    })(ClientApi = exports.ClientApi || (exports.ClientApi = {}));
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
        function createWorkspace(model) {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/workspace/create",
                contentType: "application/json",
                method: "POST",
                data: JSON.stringify(model),
                cache: false,
                success: function (data) {
                    callback.resolve(true);
                },
                error: function (data) {
                    callback.reject(false);
                }
            });
            return callback;
        }
        WorkspaceApi.createWorkspace = createWorkspace;
        function deleteWorkspace(workspace) {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/workspace/delete",
                contentType: "application/json",
                method: "POST",
                data: JSON.stringify({ name: workspace }),
                cache: false,
                success: function (data) {
                    callback.resolve(true);
                },
                error: function (data) {
                    callback.reject(false);
                }
            });
            return callback;
        }
        WorkspaceApi.deleteWorkspace = deleteWorkspace;
    })(WorkspaceApi = exports.WorkspaceApi || (exports.WorkspaceApi = {}));
    var ExtentApi;
    (function (ExtentApi) {
        function getExtents(ws) {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
                cache: false,
                success: function (data) {
                    callback.resolve(data);
                },
                error: function (data) {
                    callback.reject(false);
                }
            });
            return callback;
        }
        ExtentApi.getExtents = getExtents;
        function createItem(ws, extentUrl, metaclass) {
            var callback = $.Deferred();
            var postModel = new DMC.Out.ItemCreateModel();
            postModel.ws = ws;
            postModel.ext = extentUrl;
            postModel.metaclass = metaclass;
            $.ajax({
                url: "/api/datenmeister/extent/item_create",
                contentType: "application/json",
                data: JSON.stringify(postModel),
                method: "POST",
                success: function (data) { callback.resolve(data); },
                error: function (data) { callback.reject(false); }
            });
            return callback;
        }
        ExtentApi.createItem = createItem;
        function createItemAsSubElement(ws, extentUrl, parentItem, parentProperty, metaclass) {
            var callback = $.Deferred();
            var postModel = new DMC.Out.ItemCreateModel();
            postModel.ws = ws;
            postModel.ext = extentUrl;
            postModel.metaclass = metaclass;
            postModel.parentItem = parentItem;
            postModel.parentProperty = parentProperty;
            $.ajax({
                url: "/api/datenmeister/extent/item_create",
                contentType: "application/json",
                data: JSON.stringify(postModel),
                method: "POST",
                success: function (data) { callback.resolve(data); },
                error: function (data) { callback.reject(false); }
            });
            return callback;
        }
        ExtentApi.createItemAsSubElement = createItemAsSubElement;
        /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
        function deleteItem(ws, extent, item) {
            var callback = $.Deferred();
            var postModel = new DMC.Out.ItemDeleteModel();
            postModel.ws = ws;
            postModel.ext = extent;
            postModel.item = item;
            $.ajax({
                url: "/api/datenmeister/extent/item_delete",
                contentType: "application/json",
                data: JSON.stringify(postModel),
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
                if (query.view !== undefined && query.view !== null) {
                    url += "&view=" + encodeURIComponent(query.view.toString());
                }
            }
            return $.ajax({
                url: url,
                cache: false
            });
        }
        function deleteExtent(ws, extent) {
            var callback = $.Deferred();
            var postModel = new DMC.Out.ExtentReferenceModel();
            postModel.ws = ws;
            postModel.ext = extent;
            $.ajax({
                url: "/api/datenmeister/extent/extent_delete",
                contentType: "application/json",
                data: JSON.stringify(postModel),
                method: "POST",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.reject(false); }
            });
            return callback;
        }
        ExtentApi.deleteExtent = deleteExtent;
        function createExtent(extentData) {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/extent_create",
                contentType: "application/json",
                data: JSON.stringify(extentData),
                method: "POST",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.reject(false); }
            });
            return callback;
        }
        ExtentApi.createExtent = createExtent;
        function addExtent(extentData) {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/extent_add",
                contentType: "application/json",
                data: JSON.stringify(extentData),
                method: "POST",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.reject(false); }
            });
            return callback;
        }
        ExtentApi.addExtent = addExtent;
        function getCreatableTypes(ws, extent) {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/get_creatable_types?ws=" + encodeURIComponent(ws)
                    + "&extent=" + encodeURIComponent(extent),
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
        ExtentApi.getCreatableTypes = getCreatableTypes;
        function getViews(ws, extent, item) {
            var callback = $.Deferred();
            var uri = "/api/datenmeister/extent/get_views?ws=" + encodeURIComponent(ws) + "&extent=" + encodeURIComponent(extent);
            if (item !== null && item !== undefined) {
                uri += "&item=" + encodeURIComponent(item);
            }
            $.ajax({
                url: uri,
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
        ExtentApi.getViews = getViews;
    })(ExtentApi = exports.ExtentApi || (exports.ExtentApi = {}));
    var ItemApi;
    (function (ItemApi) {
        function getItem(ws, extentUrl, itemUrl) {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/item?ws=" + encodeURIComponent(ws) + "&extent=" + encodeURIComponent(extentUrl) + "&item=" + encodeURIComponent(itemUrl),
                cache: false,
                success: function (data) {
                    // Adds the necessary information into the ItemContentModel
                    data.ws = ws;
                    data.ext = extentUrl;
                    data.uri = itemUrl;
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
            var postModel = new DMC.Out.ItemUnsetPropertyModel();
            postModel.ws = ws;
            postModel.ext = extent;
            postModel.item = item;
            postModel.property = property;
            $.ajax({
                url: "/api/datenmeister/extent/item_unset_property",
                data: JSON.stringify(postModel),
                method: "POST",
                contentType: "application/json",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.resolve(false); }
            });
            return callback;
        }
        ItemApi.deleteProperty = deleteProperty;
        function setProperty(ws, extentUrl, itemUrl, property, newValue) {
            var callback = $.Deferred();
            var postModel = new DMC.Out.ItemSetPropertyModel();
            postModel.ws = ws;
            postModel.ext = extentUrl;
            postModel.item = itemUrl;
            postModel.property = property;
            postModel.newValue = newValue;
            $.ajax({
                url: "/api/datenmeister/extent/item_set_property",
                contentType: "application/json",
                data: postModel,
                method: "POST",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.resolve(false); }
            });
            return callback;
        }
        ItemApi.setProperty = setProperty;
        function setProperties(ws, extentUrl, itemUrl, item) {
            var callback = $.Deferred();
            var postModel = new DMC.Out.ItemSetPropertiesModel();
            postModel.ws = ws;
            postModel.ext = extentUrl;
            postModel.item = itemUrl;
            postModel.v = new Array();
            for (var k in item.v) {
                var value = item.v[k];
                var property = {
                    Key: k,
                    Value: value
                };
                postModel.v[postModel.v.length] = property;
            }
            $.ajax({
                url: "/api/datenmeister/extent/item_set_properties",
                data: JSON.stringify(postModel),
                method: "POST",
                contentType: "application/json",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.resolve(false); }
            });
            return callback;
        }
        ItemApi.setProperties = setProperties;
    })(ItemApi = exports.ItemApi || (exports.ItemApi = {}));
    var ExampleApi;
    (function (ExampleApi) {
        function addZipCodes(workspace) {
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/example/addzipcodes",
                data: JSON.stringify({ ws: workspace }),
                method: "POST",
                contentType: "application/json",
                success: function (data) { callback.resolve(true); },
                error: function (data) { callback.resolve(false); }
            });
            return callback;
        }
        ExampleApi.addZipCodes = addZipCodes;
    })(ExampleApi = exports.ExampleApi || (exports.ExampleApi = {}));
});
//# sourceMappingURL=datenmeister-client.js.map