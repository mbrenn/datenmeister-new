/// <reference path="typings/jquery/jquery.d.ts" />
var DatenMeister;
(function (DatenMeister) {
    ;
    ;
    class DataTableConfiguration {
        DataTableConfiguration() {
            this.editFunction = function (url) { return false; /*Ignoring*/ };
            this.deleteFunction = function (url) { return false; /*Ignoring*/ };
        }
    }
    DatenMeister.DataTableConfiguration = DataTableConfiguration;
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
    class ExtentLogic {
        loadAndCreateHtmlForExtents(container, ws) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws)).
                done(function (data) {
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
            var configuration = new DataTableConfiguration();
            configuration.editFunction = function (url) {
                location.href = "/Home/item?ws=" + encodeURIComponent(ws)
                    + "&extent=" + encodeURIComponent(extentUrl)
                    + "&item=" + encodeURIComponent(url);
                return false;
            };
            configuration.deleteFunction = function (url) {
                alert("DELETE: " + url);
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
            throw new Error("Not implemented");
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
    })(GUI = DatenMeister.GUI || (DatenMeister.GUI = {}));
})(DatenMeister || (DatenMeister = {}));
;
//# sourceMappingURL=datenmeister.js.map