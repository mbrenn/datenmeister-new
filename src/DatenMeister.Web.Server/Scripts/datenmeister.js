/// <reference path="typings/jquery/jquery.d.ts" />
var DatenMeister;
(function (DatenMeister) {
    ;
    ;
    class WorkspaceLogic {
        loadAndCreateHtmlForWorkbenchs(container) {
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
        }
        createHtmlForWorkbenchs(container, data) {
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click((function (localEntry) {
                    return function () {
                        location.href = `/Home/workspace?ws=${encodeURIComponent(localEntry.id)}`;
                    };
                }(entry)));
                container.append(dom);
            }
        }
    }
    DatenMeister.WorkspaceLogic = WorkspaceLogic;
    ;
    var Models;
    (function (Models) {
        class ItemReferenceModel {
        }
        Models.ItemReferenceModel = ItemReferenceModel;
        class ItemDeleteModel extends ItemReferenceModel {
        }
        Models.ItemDeleteModel = ItemDeleteModel;
    })(Models = DatenMeister.Models || (DatenMeister.Models = {}));
    class ExtentLogic {
        /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
        deleteItem(ws, extent, item) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/item_delete?ws=" + encodeURIComponent(ws)
                + "&extent=" + encodeURIComponent(extent)
                + "&item=" + encodeURIComponent(item))
                .done((data) => { callback.resolve(true); })
                .fail((data) => { callback.resolve(false); });
            return callback;
        }
        loadAndCreateHtmlForExtents(container, ws) {
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
        }
        createHtmlForExtent(container, ws, data) {
            container.empty();
            var compiled = _.template($("#template_extent").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click((function (localEntry) {
                    return function () {
                        location.href = "/Home/extent?ws=" + encodeURIComponent(ws)
                            + "&extent=" + encodeURIComponent(localEntry.uri);
                    };
                }(entry)));
                container.append(dom);
            }
        }
        loadAndCreateHtmlForItems(container, ws, extentUrl) {
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
        }
        createHtmlForItems(container, ws, extentUrl, data) {
            var tthis = this;
            var configuration = new GUI.DataTableConfiguration();
            configuration.editFunction = function (url) {
                location.href = "/Home/item?ws=" + encodeURIComponent(ws)
                    + "&extent=" + encodeURIComponent(extentUrl)
                    + "&item=" + encodeURIComponent(url);
                return false;
            };
            configuration.deleteFunction = function (url) {
                var callback = tthis.deleteItem(ws, extentUrl, url);
                callback
                    .done(() => {
                    alert("REMOVED");
                    tthis.loadAndCreateHtmlForItems(container, ws, extentUrl);
                })
                    .fail(() => { alert("FAILED"); });
                return false;
            };
            var table = new GUI.DataTable(data.items, data.columns, configuration);
            table.show(container);
        }
        loadAndCreateHtmlForItem(container, ws, extentUrl, itemUrl) {
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
        }
        createHtmlForItem(jQuery, ws, extentUrl, itemUrl, data) {
            var configuration = new GUI.ItemContentConfiguration();
            var table = new GUI.ItemContentTable(data, configuration);
            table.show(jQuery);
        }
    }
    DatenMeister.ExtentLogic = ExtentLogic;
    var GUI;
    (function (GUI) {
        function loadWorkspaces() {
            $(document).ready(function () {
                var workbenchLogic = new DatenMeister.WorkspaceLogic();
                workbenchLogic.loadAndCreateHtmlForWorkbenchs($("#container_workspace")).done(function (data) {
                }).fail(function () {
                });
            });
        }
        GUI.loadWorkspaces = loadWorkspaces;
        function loadExtents(workspaceId) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForExtents($("#container_extents"), workspaceId).done(function (data) {
                }).fail(function () {
                });
            });
        }
        GUI.loadExtents = loadExtents;
        function loadExtent(workspaceId, extentUrl) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForItems($("#container_item"), workspaceId, extentUrl);
            });
        }
        GUI.loadExtent = loadExtent;
        function loadItem(workspaceId, extentUrl, itemUrl) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForItem($("#container_item"), workspaceId, extentUrl, itemUrl);
            });
        }
        GUI.loadItem = loadItem;
        class DataTableConfiguration {
            constructor() {
                this.editFunction = function (url) { return false; /*Ignoring*/ };
                this.deleteFunction = function (url) { return false; /*Ignoring*/ };
            }
        }
        GUI.DataTableConfiguration = DataTableConfiguration;
        /*
         * Used to show a lot of items in a database. The table will use an array of MofObjects
         * as the datasource
         */
        class DataTable {
            constructor(items, columns, configuration) {
                this.items = items;
                this.columns = columns;
                this.configuration = configuration;
            }
            // Replaces the content at the dom with the created table
            show(dom) {
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
                    domEditColumn.click((function (url) {
                        return function () {
                            return tthis.configuration.editFunction(url);
                        };
                    })(item.uri));
                    domRow.append(domEditColumn);
                    domDeleteColumn = $("<td class='hl'><a href='#'>DELETE</a></td>");
                    domDeleteColumn.click((function (url) {
                        return function () {
                            return tthis.configuration.deleteFunction(url);
                        };
                    })(item.uri));
                    domRow.append(domDeleteColumn);
                    domTable.append(domRow);
                }
                dom.append(domTable);
            }
        }
        GUI.DataTable = DataTable;
        class ItemContentConfiguration {
            constructor() {
                this.editFunction = function (url) { return false; /*Ignoring*/ };
                this.deleteFunction = function (url) { return false; /*Ignoring*/ };
            }
        }
        GUI.ItemContentConfiguration = ItemContentConfiguration;
        class ItemContentTable {
            constructor(item, configuration) {
                this.item = item;
                this.configuration = configuration;
            }
            show(dom) {
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
                    let domEditColumn = $("<td class='hl'><a href='#'>EDIT</a></td>");
                    domEditColumn.click((function (url, property) {
                        return function () {
                            return tthis.configuration.editFunction(url, property);
                        };
                    })(this.item.uri, property));
                    domRow.append(domEditColumn);
                    let domDeleteColumn = $("<td class='hl'><a href='#'>DELETE</a></td>");
                    domDeleteColumn.click((function (url, property) {
                        return function () {
                            return tthis.configuration.deleteFunction(url, property);
                        };
                    })(this.item.uri, property));
                    domRow.append(domDeleteColumn);
                    domTable.append(domRow);
                }
                dom.append(domTable);
            }
        }
        GUI.ItemContentTable = ItemContentTable;
    })(GUI = DatenMeister.GUI || (DatenMeister.GUI = {}));
})(DatenMeister || (DatenMeister = {}));
;
//# sourceMappingURL=datenmeister.js.map