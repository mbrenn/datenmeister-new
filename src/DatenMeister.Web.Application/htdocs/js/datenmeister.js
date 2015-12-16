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
            $.ajax("/api/datenmeister/workspace/all").
                done(function (data) {
                tthis.createHtmlForWorkbenchs(container, data);
                callback.resolve(null);
            })
                .fail(function (data) {
                callback.reject(null);
            });
            return callback;
        };
        WorkspaceLogic.prototype.createHtmlForWorkbenchs = function (container, data) {
            var tthis = this;
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click((function (localEntry) {
                    return function () {
                        var workspaceId = localEntry.id;
                        if (tthis.onWorkspaceSelected != null) {
                            tthis.onWorkspaceSelected(localEntry.id);
                        }
                    };
                }(entry)));
                container.append(dom);
            }
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
            $.ajax("/api/datenmeister/extent/item_delete", {
                data: postModel,
                method: "POST"
            })
                .done(function (data) { callback.resolve(true); })
                .fail(function (data) { callback.resolve(false); });
            return callback;
        };
        ExtentLogic.prototype.deleteProperty = function (ws, extent, item, property) {
            var callback = $.Deferred();
            var postModel = new PostModels.ItemUnsetPropertyModel();
            postModel.ws = ws;
            postModel.extent = extent;
            postModel.item = item;
            postModel.property = property;
            $.ajax("/api/datenmeister/extent/item_unset_property", {
                data: postModel,
                method: "POST"
            })
                .done(function (data) { callback.resolve(true); })
                .fail(function (data) { callback.resolve(false); });
            return callback;
        };
        ExtentLogic.prototype.loadAndCreateHtmlForExtents = function (container, ws) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws))
                .done(function (data) {
                tthis.createHtmlForExtent(container, ws, data);
                callback.resolve(null);
            })
                .fail(function (data) {
                callback.reject(null);
            });
            return callback;
        };
        ExtentLogic.prototype.createHtmlForExtent = function (container, ws, data) {
            var tthis = this;
            container.empty();
            if (data.length === 0) {
                container.html("<tr><td>No extents were found</td></tr>");
            }
            else {
                var compiled = _.template($("#template_extent").html());
                for (var n in data) {
                    var entry = data[n];
                    var line = compiled(entry);
                    var dom = $(line);
                    $(".data", dom).click((function (localEntry) {
                        return function () {
                            if (tthis.onExtentSelected !== null) {
                                tthis.onExtentSelected(ws, localEntry.uri);
                            }
                        };
                    }(entry)));
                    container.append(dom);
                }
            }
        };
        ExtentLogic.prototype.loadAndCreateHtmlForItems = function (container, ws, extentUrl) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/items?ws=" + encodeURIComponent(ws)
                + "&extent=" + encodeURIComponent(extentUrl)).
                done(function (data) {
                tthis.createHtmlForItems(container, ws, extentUrl, data);
                callback.resolve(null);
            })
                .fail(function (data) {
                callback.reject(null);
            });
            return callback;
        };
        ExtentLogic.prototype.createHtmlForItems = function (container, ws, extentUrl, data) {
            var tthis = this;
            var configuration = new GUI.DataTableConfiguration();
            configuration.editFunction = function (url) {
                if (tthis.onItemSelected !== null) {
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
            var table = new GUI.DataTable(data.items, data.columns, configuration);
            table.show(container);
        };
        ExtentLogic.prototype.loadAndCreateHtmlForItem = function (container, ws, extentUrl, itemUrl) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/item?ws=" + encodeURIComponent(ws)
                + "&extent=" + encodeURIComponent(extentUrl)
                + "&item=" + encodeURIComponent(itemUrl)).
                done(function (data) {
                tthis.createHtmlForItem(container, ws, extentUrl, itemUrl, data);
                callback.resolve(null);
            })
                .fail(function (data) {
                callback.reject(null);
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
    var GUI;
    (function (GUI) {
        function start() {
            $(document).ready(function () { loadWorkspaces(); });
        }
        GUI.start = start;
        function loadWorkspaces() {
            var workbenchLogic = new DatenMeister.WorkspaceLogic();
            workbenchLogic.onWorkspaceSelected = function (id) {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                loadExtents(id);
            };
            $(".container_title").text("Workspaces");
            workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".container_data"))
                .done(function (data) {
            })
                .fail(function () {
            });
        }
        GUI.loadWorkspaces = loadWorkspaces;
        function loadExtents(workspaceId) {
            var extentLogic = new DatenMeister.ExtentLogic();
            var containerTitle = $(".container_title");
            containerTitle.html("<a href='#' class='link_workspace'>Workspaces</a> - Extents");
            $(".link_workspace", containerTitle).click(function () {
                loadWorkspaces();
                return false;
            });
            extentLogic.loadAndCreateHtmlForExtents($(".container_data"), workspaceId)
                .done(function (data) {
            })
                .fail(function () {
            });
        }
        GUI.loadExtents = loadExtents;
        function loadExtent(workspaceId, extentUrl) {
            var extentLogic = new DatenMeister.ExtentLogic();
            $(".container_title").text("Items");
            extentLogic.loadAndCreateHtmlForItems($(".container_data"), workspaceId, extentUrl);
        }
        GUI.loadExtent = loadExtent;
        function loadItem(workspaceId, extentUrl, itemUrl) {
            var extentLogic = new DatenMeister.ExtentLogic();
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
        var DataTable = (function () {
            function DataTable(items, columns, configuration) {
                this.items = items;
                this.columns = columns;
                this.configuration = configuration;
            }
            // Replaces the content at the dom with the created table
            DataTable.prototype.show = function (dom) {
                var tthis = this;
                dom.empty();
                var domTable = $("<table></table>");
                // First the headline
                var domRow = $("<tr></tr>");
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
                    domRow = $("<tr></tr>");
                    for (var c in this.columns) {
                        var column = this.columns[c];
                        var domColumn = $("<td></td>");
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
                    domDeleteColumn.click((function (url, idomRow, idomA) {
                        return function () {
                            if (idomA.data("wasClicked") === true) {
                                return tthis.configuration.deleteFunction(url, idomRow);
                            }
                            else {
                                idomA.data("wasClicked", true);
                                idomA.text("CONFIRM");
                            }
                        };
                    })(item.uri, domRow, domA));
                    domRow.append(domDeleteColumn);
                    domTable.append(domRow);
                }
                dom.append(domTable);
            };
            return DataTable;
        })();
        GUI.DataTable = DataTable;
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
                var domTable = $("<table></table>");
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
