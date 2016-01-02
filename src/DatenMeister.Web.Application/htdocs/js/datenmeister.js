/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
define(["require", "exports", "datenmeister-helper", "datenmeister-interfaces", "datenmeister-tables"], function (require, exports, DMHelper, DMI, DMTables) {
    var WorkspaceLogic = (function () {
        function WorkspaceLogic() {
        }
        WorkspaceLogic.prototype.loadAndCreateHtmlForWorkbenchs = function (container) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/workspace/all",
                cache: false,
                success: function (data) {
                    tthis.createHtmlForWorkbenchs(container, data);
                    callback.resolve(null);
                },
                error: function (data) {
                    callback.reject(null);
                }
            });
            return callback;
        };
        WorkspaceLogic.prototype.createHtmlForWorkbenchs = function (container, data) {
            var tthis = this;
            container.empty();
            var compiledTable = $($("#template_workspace_table").html());
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click((function (localEntry) {
                    return function () {
                        var workspaceId = localEntry.id;
                        if (tthis.onWorkspaceSelected != undefined) {
                            tthis.onWorkspaceSelected(workspaceId);
                        }
                        return false;
                    };
                }(entry)));
                $(compiledTable).append(dom);
            }
            container.append(compiledTable);
        };
        return WorkspaceLogic;
    })();
    exports.WorkspaceLogic = WorkspaceLogic;
    var ExtentLogic = (function () {
        function ExtentLogic() {
        }
        ExtentLogic.prototype.createItem = function (ws, extentUrl, container) {
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
        };
        /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
        ExtentLogic.prototype.deleteItem = function (ws, extent, item) {
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
        };
        ExtentLogic.prototype.deleteProperty = function (ws, extent, item, property) {
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
        };
        ExtentLogic.prototype.setProperty = function (ws, extentUrl, itemUrl, property, newValue) {
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
        };
        ExtentLogic.prototype.loadAndCreateHtmlForWorkspace = function (container, ws) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
                cache: false,
                success: function (data) {
                    tthis.createHtmlForWorkspace(container, ws, data);
                    callback.resolve(null);
                },
                error: function (data) {
                    callback.reject(null);
                }
            });
            return callback;
        };
        ExtentLogic.prototype.createHtmlForWorkspace = function (container, ws, data) {
            var tthis = this;
            container.empty();
            if (data.length === 0) {
                container.html("<p>No extents were found</p>");
            }
            else {
                var compiledTable = $($("#template_extent_table").html());
                var compiled = _.template($("#template_extent").html());
                for (var n in data) {
                    var entry = data[n];
                    var line = compiled(entry);
                    var dom = $(line);
                    $(".data", dom).click((function (localEntry) {
                        return function () {
                            if (tthis.onExtentSelected !== undefined) {
                                tthis.onExtentSelected(ws, localEntry.uri);
                            }
                            return false;
                        };
                    }(entry)));
                    compiledTable.append(dom);
                }
                container.append(compiledTable);
            }
        };
        ExtentLogic.prototype.loadAndCreateHtmlForExtent = function (container, ws, extentUrl, query) {
            var tthis = this;
            var callback = $.Deferred();
            this.loadHtmlForExtent(ws, extentUrl)
                .done(function (data) {
                tthis.createHtmlForExtent(container, ws, extentUrl, data);
                callback.resolve(null);
            })
                .fail(function (data) {
                callback.reject(null);
            });
            return callback;
        };
        ExtentLogic.prototype.loadHtmlForExtent = function (ws, extentUrl, query) {
            var url = "/api/datenmeister/extent/items?ws=" + encodeURIComponent(ws)
                + "&extent=" + encodeURIComponent(extentUrl);
            if (query !== undefined) {
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
        };
        ExtentLogic.prototype.createHtmlForExtent = function (container, ws, extentUrl, data) {
            var _this = this;
            var tthis = this;
            var configuration = new DMTables.ItemTableConfiguration();
            configuration.onItemEdit = function (url) {
                if (_this.onItemSelected !== undefined) {
                    _this.onItemSelected(ws, extentUrl, url);
                }
                return false;
            };
            configuration.onItemDelete = function (url, domRow) {
                var callback = _this.deleteItem(ws, extentUrl, url);
                callback
                    .done(function () {
                    // tthis.loadAndCreateHtmlForItems(container, ws, extentUrl);
                    domRow.find("td").fadeOut(500, function () { domRow.remove(); });
                })
                    .fail(function () { alert("FAILED"); });
                return false;
            };
            var table = new DMTables.ItemListTable(container, data, configuration);
            configuration.onSearch = function (searchString) {
                tthis.loadHtmlForExtent(ws, extentUrl, { searchString: searchString })
                    .done(function (innerData) {
                    if (table.lastProcessedSearchString === innerData.search) {
                        table.updateItems(innerData);
                    }
                });
            };
            configuration.onNewItemClicked = function () {
                tthis.createItem(ws, extentUrl)
                    .done(function (innerData) {
                    tthis.onItemCreated(ws, extentUrl, innerData.newuri);
                });
            };
            var itemsPerPage = 10;
            configuration.onPageChange = function (newPage) {
                tthis.loadHtmlForExtent(ws, extentUrl, {
                    offset: itemsPerPage * (newPage - 1),
                    amount: itemsPerPage
                })
                    .done(function (innerData) {
                    table.updateItems(innerData);
                });
            };
            table.show();
        };
        ExtentLogic.prototype.loadAndCreateHtmlForItem = function (container, ws, extentUrl, itemUrl) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/item?ws=" + encodeURIComponent(ws)
                    + "&extent=" + encodeURIComponent(extentUrl)
                    + "&item=" + encodeURIComponent(itemUrl),
                cache: false,
                success: function (data) {
                    tthis.createHtmlForItem(container, ws, extentUrl, itemUrl, data);
                    callback.resolve(null);
                },
                error: function (data) {
                    callback.reject(null);
                }
            });
            return callback;
        };
        ExtentLogic.prototype.createHtmlForItem = function (jQuery, ws, extentUrl, itemUrl, data) {
            var tthis = this;
            var configuration = new DMTables.ItemContentConfiguration();
            configuration.deleteFunction = function (url, property, domRow) {
                tthis.deleteProperty(ws, extentUrl, itemUrl, property).done(function () { return domRow.find("td").fadeOut(500, function () { domRow.remove(); }); });
                return false;
            };
            configuration.onEditProperty = function (url, property, newValue) {
                tthis.setProperty(ws, extentUrl, itemUrl, property, newValue);
            };
            configuration.onNewProperty = function (url, property, newValue) {
                tthis.setProperty(ws, extentUrl, itemUrl, property, newValue);
            };
            var table = new DMTables.ItemContentTable(data, configuration);
            table.show(jQuery);
        };
        return ExtentLogic;
    })();
    exports.ExtentLogic = ExtentLogic;
    var GUI;
    (function (GUI) {
        function start() {
            $(document).ready(function () {
                window.onpopstate = function (ev) {
                    parseAndNavigateToWindowLocation();
                };
                parseAndNavigateToWindowLocation();
            });
        }
        GUI.start = start;
        function parseAndNavigateToWindowLocation() {
            var ws = DMHelper.getParameterByNameFromHash("ws");
            var extentUrl = DMHelper.getParameterByNameFromHash("ext");
            var itemUrl = DMHelper.getParameterByNameFromHash("item");
            if (ws === "") {
                loadWorkspaces();
            }
            else if (extentUrl === "") {
                loadExtents(ws);
            }
            else if (itemUrl === "") {
                loadExtent(ws, extentUrl);
            }
            else {
                loadItem(ws, extentUrl, itemUrl);
            }
        }
        GUI.parseAndNavigateToWindowLocation = parseAndNavigateToWindowLocation;
        function createTitle(ws, extentUrl, itemUrl) {
            var containerTitle = $(".container_title");
            var containerRefresh = $("<a href='#'>Refresh</a>");
            if (ws === undefined) {
                containerTitle.text("Workspaces - ");
                containerRefresh.click(function () {
                    loadWorkspaces();
                    return false;
                });
            }
            else if (extentUrl === undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - Extents - ");
                containerRefresh.click(function () {
                    loadExtents(ws);
                    return false;
                });
            }
            else if (itemUrl == undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - Items - ");
                containerRefresh.click(function () {
                    loadExtent(ws, extentUrl);
                    return false;
                });
            }
            else {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - <a href='#' class='link_items'>Items</a> - ");
                containerRefresh.click(function () {
                    loadItem(ws, extentUrl, itemUrl);
                    return false;
                });
            }
            containerTitle.append(containerRefresh);
            $(".link_workspaces", containerTitle).click(function () {
                loadWorkspaces();
                return false;
            });
            $(".link_extents", containerTitle).click(function () {
                loadExtents(ws);
                return false;
            });
            $(".link_items", containerTitle).click(function () {
                loadExtent(ws, extentUrl);
                return false;
            });
        }
        function loadWorkspaces() {
            var workbenchLogic = new WorkspaceLogic();
            workbenchLogic.onWorkspaceSelected = function (id) {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                history.pushState({}, "", "#ws=" + encodeURIComponent(id));
                loadExtents(id);
            };
            workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".container_data"))
                .done(function (data) {
                createTitle();
            })
                .fail(function () {
            });
        }
        GUI.loadWorkspaces = loadWorkspaces;
        function loadExtents(workspaceId) {
            var extentLogic = new ExtentLogic();
            extentLogic.onExtentSelected = function (ws, extentUrl) {
                history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                    + "&ext=" + encodeURIComponent(extentUrl));
                loadExtent(ws, extentUrl);
                return false;
            };
            extentLogic.loadAndCreateHtmlForWorkspace($(".container_data"), workspaceId)
                .done(function (data) {
                createTitle(workspaceId);
            })
                .fail(function () {
            });
        }
        GUI.loadExtents = loadExtents;
        function loadExtent(workspaceId, extentUrl) {
            var extentLogic = new ExtentLogic();
            extentLogic.onItemSelected = function (ws, extentUrl, itemUrl) {
                navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.onItemCreated = function (ws, extentUrl, itemUrl) {
                navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.loadAndCreateHtmlForExtent($(".container_data"), workspaceId, extentUrl).done(function (data) {
                createTitle(workspaceId, extentUrl);
            });
        }
        GUI.loadExtent = loadExtent;
        function navigateToItem(ws, extentUrl, itemUrl) {
            history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                + "&ext=" + encodeURIComponent(extentUrl)
                + "&item=" + encodeURIComponent(itemUrl));
            loadItem(ws, extentUrl, itemUrl);
        }
        GUI.navigateToItem = navigateToItem;
        function loadItem(workspaceId, extentUrl, itemUrl) {
            var extentLogic = new ExtentLogic();
            createTitle(workspaceId, extentUrl, itemUrl);
            extentLogic.loadAndCreateHtmlForItem($(".container_data"), workspaceId, extentUrl, itemUrl);
        }
        GUI.loadItem = loadItem;
    })(GUI = exports.GUI || (exports.GUI = {}));
});
//# sourceMappingURL=datenmeister.js.map