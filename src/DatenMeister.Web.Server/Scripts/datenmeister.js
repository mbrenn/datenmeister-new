/// <reference path="typings/jquery/jquery.d.ts" />
var DatenMeister;
(function (DatenMeister) {
    ;
    ;
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
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click((function (localEntry) {
                    return function () {
                        location.href = "/Home/workspace?ws=" + localEntry.id;
                    };
                }(entry)));
                container.append(dom);
            }
        };
        return WorkspaceLogic;
    })();
    DatenMeister.WorkspaceLogic = WorkspaceLogic;
    ;
    var ExtentLogic = (function () {
        function ExtentLogic() {
        }
        ExtentLogic.prototype.loadAndCreateHtmlForExtents = function (container, ws) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/all?ws=" + ws).
                done(function (data) {
                tthis.createHtmlForExtent(container, ws, data);
                callback.resolve(null);
            })
                .fail(function (data) {
                callback.reject(null);
            });
            return callback;
        };
        ExtentLogic.prototype.createHtmlForExtent = function (container, ws, data) {
            container.empty();
            var compiled = _.template($("#template_extent").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click((function (localEntry) {
                    return function () {
                        location.href = "/Home/extent?ws=" + ws + "&extent=" + localEntry.uri;
                    };
                }(entry)));
                container.append(dom);
            }
        };
        ExtentLogic.prototype.loadAndCreateHtmlForItems = function (container, ws, extentUrl) {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/items?ws=" + ws + "&url=" + extentUrl).
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
            var table = new GUI.DataTable(data.items, data.columns);
            table.show(container);
        };
        return ExtentLogic;
    })();
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
        /*
         * Used to show a lot of items in a database. The table will use an array of MofObjects
         * as the datasource
         */
        var DataTable = (function () {
            function DataTable(items, columns) {
                this.items = items;
                this.columns = columns;
            }
            // Replaces the content at the dom with the created table
            DataTable.prototype.show = function (dom) {
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
                    domTable.append(domRow);
                }
                dom.append(domTable);
            };
            return DataTable;
        })();
        GUI.DataTable = DataTable;
    })(GUI = DatenMeister.GUI || (DatenMeister.GUI = {}));
})(DatenMeister || (DatenMeister = {}));
;
//# sourceMappingURL=datenmeister.js.map