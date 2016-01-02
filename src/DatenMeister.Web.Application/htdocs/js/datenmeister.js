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
        var ExtentReferenceModel = (function () {
            function ExtentReferenceModel() {
            }
            return ExtentReferenceModel;
        })();
        PostModels.ExtentReferenceModel = ExtentReferenceModel;
        var ItemReferenceModel = (function (_super) {
            __extends(ItemReferenceModel, _super);
            function ItemReferenceModel() {
                _super.apply(this, arguments);
            }
            return ItemReferenceModel;
        })(ExtentReferenceModel);
        PostModels.ItemReferenceModel = ItemReferenceModel;
        var ItemCreateModel = (function (_super) {
            __extends(ItemCreateModel, _super);
            function ItemCreateModel() {
                _super.apply(this, arguments);
            }
            return ItemCreateModel;
        })(ExtentReferenceModel);
        PostModels.ItemCreateModel = ItemCreateModel;
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
        var ItemSetPropertyModel = (function (_super) {
            __extends(ItemSetPropertyModel, _super);
            function ItemSetPropertyModel() {
                _super.apply(this, arguments);
            }
            return ItemSetPropertyModel;
        })(ItemReferenceModel);
        PostModels.ItemSetPropertyModel = ItemSetPropertyModel;
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
        ExtentLogic.prototype.createItem = function (ws, extentUrl, container) {
            var callback = $.Deferred();
            var postModel = new PostModels.ItemCreateModel();
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
        ExtentLogic.prototype.setProperty = function (ws, extentUrl, itemUrl, property, newValue) {
            var callback = $.Deferred();
            var postModel = new PostModels.ItemSetPropertyModel();
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
        ExtentLogic.prototype.loadAndCreateHtmlForItems = function (container, ws, extentUrl, query) {
            var tthis = this;
            var callback = $.Deferred();
            this.loadHtmlForItems(ws, extentUrl)
                .done(function (data) {
                tthis.createHtmlForItems(container, ws, extentUrl, data);
                callback.resolve(null);
            })
                .fail(function (data) {
                callback.reject(null);
            });
            return callback;
        };
        ExtentLogic.prototype.loadHtmlForItems = function (ws, extentUrl, query) {
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
        ExtentLogic.prototype.createHtmlForItems = function (container, ws, extentUrl, data) {
            var _this = this;
            var tthis = this;
            var configuration = new GUI.DataTableConfiguration();
            configuration.editFunction = function (url) {
                if (_this.onItemSelected !== undefined) {
                    _this.onItemSelected(ws, extentUrl, url);
                }
                return false;
            };
            configuration.deleteFunction = function (url, domRow) {
                var callback = _this.deleteItem(ws, extentUrl, url);
                callback
                    .done(function () {
                    // tthis.loadAndCreateHtmlForItems(container, ws, extentUrl);
                    domRow.find("td").fadeOut(500, function () { domRow.remove(); });
                })
                    .fail(function () { alert("FAILED"); });
                return false;
            };
            var table = new GUI.ItemListTable(container, data, configuration);
            configuration.onSearch = function (searchString) {
                tthis.loadHtmlForItems(ws, extentUrl, { searchString: searchString })
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
            var configuration = new GUI.ItemContentConfiguration();
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
                navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.onItemCreated = function (ws, extentUrl, itemUrl) {
                navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.loadAndCreateHtmlForItems($(".container_data"), workspaceId, extentUrl).done(function (data) {
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
            var extentLogic = new DatenMeister.ExtentLogic();
            createTitle(workspaceId, extentUrl, itemUrl);
            extentLogic.loadAndCreateHtmlForItem($(".container_data"), workspaceId, extentUrl, itemUrl);
        }
        GUI.loadItem = loadItem;
        var DataTableConfiguration = (function () {
            function DataTableConfiguration() {
                this.editFunction = function (url, domRow) { return false; };
                this.deleteFunction = function (url, domRow) { return false; };
                this.supportSearchbox = true;
                this.supportNewItem = true;
            }
            return DataTableConfiguration;
        })();
        GUI.DataTableConfiguration = DataTableConfiguration;
        /*
         * Used to show a lot of items in a database. The table will use an array of MofObjects
         * as the datasource
         */
        var ItemListTable = (function () {
            function ItemListTable(dom, data, configuration) {
                this.domContainer = dom;
                this.data = data;
                this.configuration = configuration;
            }
            // Replaces the content at the dom with the created table
            ItemListTable.prototype.show = function () {
                var tthis = this;
                this.domContainer.empty();
                if (this.configuration.supportSearchbox) {
                    var domSearchBox = $("<div><input type='textbox' /></div>");
                    var domInput = $("input", domSearchBox);
                    $("input", domSearchBox).keyup(function () {
                        var searchValue = domInput.val();
                        if (tthis.configuration.onSearch !== undefined) {
                            tthis.lastProcessedSearchString = searchValue;
                            tthis.configuration.onSearch(searchValue);
                        }
                    });
                    this.domContainer.append(domSearchBox);
                }
                if (this.configuration.supportNewItem) {
                    var domNewItem = $("<div><a href='#'>Create new item</a></div>");
                    domNewItem.click(function () {
                        if (tthis.configuration.onNewItemClicked !== undefined) {
                            tthis.configuration.onNewItemClicked();
                        }
                        return false;
                    });
                    this.domContainer.append(domNewItem);
                }
                var domAmount = $("<div>Total: <span class='totalnumber'>##</span>, Filtered: <span class='filterednumber'>##</span>");
                this.domTotalNumber = $(".totalnumber", domAmount);
                this.domFilteredNumber = $(".filterednumber", domAmount);
                this.domContainer.append(domAmount);
                this.domTable = $("<table class='table'></table>");
                // First the headline
                var domRow = $("<tr><th>ID</th></tr>");
                for (var c in this.data.columns) {
                    var column = this.data.columns[c];
                    var domColumn = $("<th></th>");
                    domColumn.text(column.title);
                    domRow.append(domColumn);
                }
                // Creates the edit and delete column
                var domEditColumn = $("<th>EDIT</th>");
                domRow.append(domEditColumn);
                var domDeleteColumn = $("<th>DELETE</th>");
                domRow.append(domDeleteColumn);
                this.domTable.append(domRow);
                // Now, the items
                tthis.createRowsForData();
                this.domContainer.append(this.domTable);
            };
            ItemListTable.prototype.createRowsForData = function () {
                var tthis = this;
                this.domTotalNumber.text(this.data.totalItemCount);
                this.domFilteredNumber.text(this.data.filteredItemCount);
                // Now, the items
                for (var i in this.data.items) {
                    var item = this.data.items[i];
                    // Gets the id of the item
                    var id = item.uri;
                    var hashIndex = item.uri.indexOf("#");
                    if (hashIndex !== -1) {
                        id = item.uri.substring(hashIndex + 1);
                    }
                    var domRow = $("<tr></tr>");
                    var domColumn = $("<td></td>");
                    domColumn.text(id);
                    domRow.append(domColumn);
                    for (var c in this.data.columns) {
                        var column = this.data.columns[c];
                        domColumn = $("<td></td>");
                        domColumn.text(item.v[column.name]);
                        domRow.append(domColumn);
                    }
                    // Add Edit link
                    var domEditColumn = $("<td class='hl'><a href='#'>EDIT</a></td>");
                    domEditColumn.click((function (url, iDomRow) {
                        return function () {
                            return tthis.configuration.editFunction(url, iDomRow);
                        };
                    })(item.uri, domRow));
                    domRow.append(domEditColumn);
                    var domDeleteColumn = $("<td class='hl'><a href='#'>DELETE</a></td>");
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
                    this.domTable.append(domRow);
                }
            };
            ItemListTable.prototype.updateItems = function (data) {
                this.data = data;
                $("tr", this.domTable).has("td")
                    .remove();
                this.createRowsForData();
            };
            return ItemListTable;
        })();
        GUI.ItemListTable = ItemListTable;
        var ItemContentConfiguration = (function () {
            function ItemContentConfiguration() {
                this.editFunction = function (url, property, domRow) { return false; };
                this.deleteFunction = function (url, property, domRow) { return false; };
                this.supportInlineEditing = true;
                this.supportNewProperties = true;
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
                this.domContainer = dom;
                dom.empty();
                var domTable = $("<table class='table'></table>");
                // First the headline
                var domRow = $("<tr><th>Title</th><th>Value</th><th>EDIT</th><th>DELETE</th></tr>");
                domTable.append(domRow);
                // Now, the items
                for (var property in this.item.v) {
                    domRow = $("<tr></tr>");
                    var value = this.item.v[property];
                    var domColumn = $("<td class='table_column_name'></td>");
                    domColumn.data("column", "name");
                    domColumn.text(property);
                    domRow.append(domColumn);
                    domColumn = $("<td class='table_column_value'></td>");
                    domColumn.data("column", "value");
                    domColumn.text(value);
                    domRow.append(domColumn);
                    // Add Edit link
                    var domEditColumn = $("<td class='hl table_column_edit'><a href='#'>EDIT</a></td>");
                    $("a", domEditColumn).click((function (url, property, idomRow, idomA) {
                        return function () {
                            if (tthis.configuration.supportInlineEditing) {
                                tthis.startInlineEditing(property, idomRow);
                                return false;
                            }
                            else {
                                return tthis.configuration.editFunction(url, property, idomRow);
                            }
                        };
                    })(this.item.uri, property, domRow, domA));
                    domRow.append(domEditColumn);
                    var domDeleteColumn = $("<td class='hl table_column_delete'><a href='#'>DELETE</a></td>");
                    var domA = $("a", domDeleteColumn);
                    $("a", domDeleteColumn).click((function (url, property, idomRow, idomA) {
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
                // Add new property
                if (this.configuration.supportNewProperties) {
                    this.offerNewProperty(domTable);
                }
                dom.append(domTable);
            };
            ItemContentTable.prototype.startInlineEditing = function (property, domRow) {
                var tthis = this;
                var domValue = $(".table_column_value", domRow);
                domValue.empty();
                var domTextBox = $("<input type='textbox' />");
                domTextBox.val(this.item.v[property]);
                domValue.append(domTextBox);
                var domEditColumn = $(".table_column_edit", domRow);
                domEditColumn.empty();
                var domEditOK = $("<a href='#'>OK</a>");
                domEditColumn.append(domEditOK);
                var domEditCancel = $("<a href='#'>Cancel</a>");
                domEditColumn.append(domEditCancel);
                //Sets the commands
                domEditOK.on('click', function () {
                    var newValue = domTextBox.val();
                    tthis.item.v[property] = newValue;
                    if (tthis.configuration.onEditProperty !== undefined) {
                        tthis.configuration.onEditProperty(tthis.item.uri, property, newValue);
                    }
                    tthis.show(tthis.domContainer);
                    return false;
                });
                domEditCancel.on('click', function () {
                    // Rebuilds the complete table
                    tthis.show(tthis.domContainer);
                    return false;
                });
            };
            ItemContentTable.prototype.offerNewProperty = function (domTable) {
                var tthis = this;
                var domNewProperty = $("<tr><td colspan='4'><a href='#'>NEW PROPERTY</a></td></tr>");
                $("a", domNewProperty).click(function () {
                    domNewProperty.empty();
                    var domNewPropertyName = $("<td class='table_column_name'><input type='textbox' /></td>");
                    var domNewPropertyValue = $("<td class='table_column_value'><input type='textbox' /></td>");
                    var domNewPropertyEdit = $("<td class='table_column_edit'><a href='#'>OK</a></td>");
                    var domNewPropertyCancel = $("<td class='table_column_edit'><a href='#'>CANCEL</a></td>");
                    domNewProperty.append(domNewPropertyName);
                    domNewProperty.append(domNewPropertyValue);
                    domNewProperty.append(domNewPropertyEdit);
                    domNewProperty.append(domNewPropertyCancel);
                    var inputProperty = $("input", domNewPropertyName);
                    var inputValue = $("input", domNewPropertyValue);
                    $("a", domNewPropertyEdit).click(function () {
                        var property = inputProperty.val();
                        var newValue = inputValue.val();
                        tthis.item.v[property] = newValue;
                        if (tthis.configuration.onNewProperty !== undefined) {
                            tthis.configuration.onNewProperty(tthis.item.uri, property, newValue);
                        }
                        tthis.show(tthis.domContainer);
                        return false;
                    });
                    $("a", domNewPropertyCancel).click(function () {
                        tthis.show(tthis.domContainer);
                        return false;
                    });
                    return false;
                });
                domTable.append(domNewProperty);
            };
            return ItemContentTable;
        })();
        GUI.ItemContentTable = ItemContentTable;
    })(GUI = DatenMeister.GUI || (DatenMeister.GUI = {}));
})(DatenMeister || (DatenMeister = {}));
;
