/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var DatenMeister;
(function (DatenMeister) {
    ;
    ;
    var PostModels;
    (function (PostModels) {
        /** This class is used to reference a single object within the database */
        var ItemReferenceModel = (function () {
            function ItemReferenceModel() {
            }
            return ItemReferenceModel;
        })();
        PostModels.ItemReferenceModel = ItemReferenceModel;
        var ItemUnsetPropertyModel = (function (_super) {
            __extends(ItemUnsetPropertyModel, _super);
            function ItemUnsetPropertyModel() {
                _super.apply(this, arguments);
            }
            return ItemUnsetPropertyModel;
        })(ItemReferenceModel);
        PostModels.ItemUnsetPropertyModel = ItemUnsetPropertyModel;
        var ItemDeleteModel = (function (_super) {
            __extends(ItemDeleteModel, _super);
            function ItemDeleteModel() {
                _super.apply(this, arguments);
            }
            return ItemDeleteModel;
        })(ItemReferenceModel);
        PostModels.ItemDeleteModel = ItemDeleteModel;
    })(PostModels = DatenMeister.PostModels || (DatenMeister.PostModels = {}));
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
    DatenMeister.WorkspaceLogic = WorkspaceLogic;
    ;
    var Models;
    (function (Models) {
        var ItemReferenceModel = (function () {
            function ItemReferenceModel() {
            }
            return ItemReferenceModel;
        })();
        Models.ItemReferenceModel = ItemReferenceModel;
        var ItemDeleteModel = (function (_super) {
            __extends(ItemDeleteModel, _super);
            function ItemDeleteModel() {
                _super.apply(this, arguments);
            }
            return ItemDeleteModel;
        })(ItemReferenceModel);
        Models.ItemDeleteModel = ItemDeleteModel;
    })(Models = DatenMeister.Models || (DatenMeister.Models = {}));
    var ExtentLogic = (function () {
        function ExtentLogic() {
        }
        /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
        ExtentLogic.prototype.deleteItem = function (ws, extent, item) {
            var callback = $.Deferred();
            var postModel = new PostModels.ItemDeleteModel();
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
            var postModel = new PostModels.ItemUnsetPropertyModel();
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
        ExtentLogic.prototype.loadAndCreateHtmlForExtents = function (container, ws) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
                cache: false,
                success: function (data) {
                    tthis.createHtmlForExtent(container, ws, data);
                    callback.resolve(null);
                },
                error: function (data) {
                    callback.reject(null);
                }
            });
            return callback;
        };
        ExtentLogic.prototype.createHtmlForExtent = function (container, ws, data) {
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
        ExtentLogic.prototype.loadAndCreateHtmlForItems = function (container, ws, extentUrl) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/items?ws=" + encodeURIComponent(ws)
                    + "&extent=" + encodeURIComponent(extentUrl),
                cache: false,
                success: function (data) {
                    tthis.createHtmlForItems(container, ws, extentUrl, data);
                    callback.resolve(null);
                },
                error: function (data) {
                    callback.reject(null);
                }
            });
            return callback;
        };
        ExtentLogic.prototype.createHtmlForItems = function (container, ws, extentUrl, data) {
            var tthis = this;
            var configuration = new GUI.DataTableConfiguration();
            configuration.editFunction = function (url) {
                if (tthis.onItemSelected !== undefined) {
                    tthis.onItemSelected(ws, extentUrl, url);
                }
                return false;
            };
            configuration.deleteFunction = function (url, domRow) {
                var callback = tthis.deleteItem(ws, extentUrl, url);
                callback
                    .done(function () {
                    // tthis.loadAndCreateHtmlForItems(container, ws, extentUrl);
                    domRow.find("td").fadeOut(500, function () { domRow.remove(); });
                })
                    .fail(function () { alert("FAILED"); });
                return false;
            };
            var table = new GUI.ItemListTable(data.items, data.columns, configuration);
            table.show(container);
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
            var configuration = new GUI.ItemContentConfiguration();
            configuration.deleteFunction = function (url, property, domRow) {
                tthis.deleteProperty(ws, extentUrl, itemUrl, property).done(function () { return domRow.find("td").fadeOut(500, function () { domRow.remove(); }); });
                return false;
            };
            var table = new GUI.ItemContentTable(data, configuration);
            table.show(jQuery);
        };
        return ExtentLogic;
    })();
    DatenMeister.ExtentLogic = ExtentLogic;
    var Helper;
    (function (Helper) {
        // Helper function out of http://stackoverflow.com/questions/901115/how-can-i-get-query-string-values-in-javascript
        function getParameterByNameFromHash(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&#]" + name + "=([^&#]*)"), results = regex.exec(location.hash);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
        Helper.getParameterByNameFromHash = getParameterByNameFromHash;
    })(Helper = DatenMeister.Helper || (DatenMeister.Helper = {}));
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
            var location = document.location.hash;
            alert(location);
            var ws = Helper.getParameterByNameFromHash("ws");
            var extentUrl = Helper.getParameterByNameFromHash("ext");
            var itemUrl = Helper.getParameterByNameFromHash("item");
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
            var workbenchLogic = new DatenMeister.WorkspaceLogic();
            workbenchLogic.onWorkspaceSelected = function (id) {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                history.pushState({}, '', "#ws=" + encodeURIComponent(id));
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
            var extentLogic = new DatenMeister.ExtentLogic();
            extentLogic.onExtentSelected = function (ws, extentUrl) {
                history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                    + "&ext=" + encodeURIComponent(extentUrl));
                loadExtent(ws, extentUrl);
                return false;
            };
            extentLogic.loadAndCreateHtmlForExtents($(".container_data"), workspaceId)
                .done(function (data) {
                createTitle(workspaceId);
            })
                .fail(function () {
            });
        }
        GUI.loadExtents = loadExtents;
        function loadExtent(workspaceId, extentUrl) {
            var extentLogic = new DatenMeister.ExtentLogic();
            extentLogic.onItemSelected = function (ws, extentUrl, itemUrl) {
                history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                    + "&ext=" + encodeURIComponent(extentUrl)
                    + "&item=" + encodeURIComponent(itemUrl));
                loadItem(ws, extentUrl, itemUrl);
            };
            extentLogic.loadAndCreateHtmlForItems($(".container_data"), workspaceId, extentUrl).done(function (data) {
                createTitle(workspaceId, extentUrl);
            });
        }
        GUI.loadExtent = loadExtent;
        function loadItem(workspaceId, extentUrl, itemUrl) {
            var extentLogic = new DatenMeister.ExtentLogic();
            createTitle(workspaceId, extentUrl, itemUrl);
            extentLogic.loadAndCreateHtmlForItem($(".container_data"), workspaceId, extentUrl, itemUrl);
        }
        GUI.loadItem = loadItem;
        var DataTableConfiguration = (function () {
            function DataTableConfiguration() {
                this.editFunction = function (url, domRow) { return false; /*Ignoring*/ };
                this.deleteFunction = function (url, domRow) { return false; /*Ignoring*/ };
            }
            return DataTableConfiguration;
        })();
        GUI.DataTableConfiguration = DataTableConfiguration;
        /*
         * Used to show a lot of items in a database. The table will use an array of MofObjects
         * as the datasource
         */
        var ItemListTable = (function () {
            function ItemListTable(items, columns, configuration) {
                this.items = items;
                this.columns = columns;
                this.configuration = configuration;
            }
            // Replaces the content at the dom with the created table
            ItemListTable.prototype.show = function (dom) {
                var tthis = this;
                dom.empty();
                var domTable = $("<table class='table'></table>");
                // First the headline
                var domRow = $("<tr><th>ID</th></tr>");
                for (var c in this.columns) {
                    var column = this.columns[c];
                    var domColumn = $("<th></th>");
                    domColumn.text(column.title);
                    domRow.append(domColumn);
                }
                // Creates the edit and delete column
                var domEditColumn = $("<th>EDIT</th>");
                domRow.append(domEditColumn);
                var domDeleteColumn = $("<th>DELETE</th>");
                domRow.append(domDeleteColumn);
                domTable.append(domRow);
                // Now, the items
                for (var i in this.items) {
                    var item = this.items[i];
                    // Gets the id of the item
                    var id = item.uri;
                    var hashIndex = item.uri.indexOf('#');
                    if (hashIndex !== -1) {
                        id = item.uri.substring(hashIndex + 1);
                    }
                    domRow = $("<tr></tr>");
                    var domColumn = $("<td></td>");
                    domColumn.text(id);
                    domRow.append(domColumn);
                    for (var c in this.columns) {
                        var column = this.columns[c];
                        domColumn = $("<td></td>");
                        domColumn.text(item.v[column.name]);
                        domRow.append(domColumn);
                    }
                    // Add Edit link
                    domEditColumn = $("<td class='hl'><a href='#'>EDIT</a></td>");
                    domEditColumn.click((function (url, iDomRow) {
                        return function () {
                            return tthis.configuration.editFunction(url, iDomRow);
                        };
                    })(item.uri, domRow));
                    domRow.append(domEditColumn);
                    domDeleteColumn = $("<td class='hl'><a href='#'>DELETE</a></td>");
                    var domA = $("a", domDeleteColumn);
                    domDeleteColumn.click((function (url, innerDomRow, innerDomA) {
                        return function () {
                            if (innerDomA.data("wasClicked") === true) {
                                return tthis.configuration.deleteFunction(url, innerDomRow);
                            }
                            else {
                                innerDomA.data("wasClicked", true);
                                innerDomA.text("CONFIRM");
                                return false;
                            }
                        };
                    })(item.uri, domRow, domA));
                    domRow.append(domDeleteColumn);
                    domTable.append(domRow);
                }
                dom.append(domTable);
            };
            return ItemListTable;
        })();
        GUI.ItemListTable = ItemListTable;
        var ItemContentConfiguration = (function () {
            function ItemContentConfiguration() {
                this.editFunction = function (url, property, domRow) { return false; };
                this.deleteFunction = function (url, property, domRow) { return false; };
            }
            return ItemContentConfiguration;
        })();
        GUI.ItemContentConfiguration = ItemContentConfiguration;
        var ItemContentTable = (function () {
            function ItemContentTable(item, configuration) {
                this.item = item;
                this.configuration = configuration;
            }
            ItemContentTable.prototype.show = function (dom) {
                var tthis = this;
                dom.empty();
                var domTable = $("<table class='table'></table>");
                // First the headline
                var domRow = $("<tr><th>Title</th><th>Value</th><th>EDIT</th><th>DELETE</th></tr>");
                domTable.append(domRow);
                // Now, the items
                for (var property in this.item.v) {
                    domRow = $("<tr></tr>");
                    var value = this.item.v[property];
                    var domColumn = $("<td></td>");
                    domColumn.text(property);
                    domRow.append(domColumn);
                    domColumn = $("<td></td>");
                    domColumn.text(value);
                    domRow.append(domColumn);
                    // Add Edit link
                    var domEditColumn = $("<td class='hl'><a href='#'>EDIT</a></td>");
                    domEditColumn.click((function (url, property, idomRow, idomA) {
                        return function () {
                            return tthis.configuration.editFunction(url, property, idomRow);
                        };
                    })(this.item.uri, property, domRow, domA));
                    domRow.append(domEditColumn);
                    var domDeleteColumn = $("<td class='hl'><a href='#'>DELETE</a></td>");
                    var domA = $("a", domDeleteColumn);
                    domDeleteColumn.click((function (url, property, idomRow, idomA) {
                        return function () {
                            if (idomA.data("wasClicked") === true) {
                                return tthis.configuration.deleteFunction(url, property, idomRow);
                            }
                            else {
                                idomA.data("wasClicked", true);
                                idomA.text("CONFIRM");
                                return false;
                            }
                        };
                    })(this.item.uri, property, domRow, domA));
                    domRow.append(domDeleteColumn);
                    domTable.append(domRow);
                }
                dom.append(domTable);
            };
            return ItemContentTable;
        })();
        GUI.ItemContentTable = ItemContentTable;
    })(GUI = DatenMeister.GUI || (DatenMeister.GUI = {}));
})(DatenMeister || (DatenMeister = {}));
;
