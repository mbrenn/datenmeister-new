/// <reference path="typings/jquery/jquery.d.ts" />

module DatenMeister {

    interface Workspace {
        id: string;
        annotation: string;
    };

    interface Extent {
        url: string;
    }

    interface ExtentContent {
        url: string;
        totalItemCount: number;
        filteredItemCount: number;
        columns: Array<DataTableColumn>;
        items: Array<DataTableItem>;
    };

    interface DataTableColumn {
        title: string;
        name: string;
    }

    interface DataTableItem {
        // Stores the url of the object which can be used for reference
        url: string;
        v: Array<string>;
    }

    export class WorkspaceLogic {
        loadAndCreateHtmlForWorkbenchs(container: JQuery): JQueryPromise<Array<Workspace>> {
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
        
        createHtmlForWorkbenchs(container: JQuery, data: Array<Workspace>) {
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (function (localEntry) {                        
                        return function () {
                            location.href = "/Home/workspace?ws=" + localEntry.id;
                        };
                    } (entry)));

                container.append(dom);
            }
        }
    };

    export class ExtentLogic {
        loadAndCreateHtmlForExtents(container: JQuery, ws: string): JQueryPromise<Object> {
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
        }

        createHtmlForExtent(container: JQuery, ws: string, data: Array<Workspace>) {
            container.empty();
            var compiled = _.template($("#template_extent").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (function (localEntry) {
                        return function () {
                            location.href = "/Home/extent?ws=" + ws + "&extent=" + localEntry.uri;
                        };
                    } (entry)));

                container.append(dom);
            }
        }

        loadAndCreateHtmlForItems(container: JQuery, ws: string, extentUrl: string) {
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
        }

        createHtmlForItems(container: JQuery, ws: string, extentUrl: string, data: ExtentContent) {
            var table = new GUI.DataTable(data.items, data.columns);
            table.show(container);
        }
    }

    export namespace GUI {
        export function loadWorkspaces() {
            $(document).ready(function () {
                var workbenchLogic = new DatenMeister.WorkspaceLogic();
                workbenchLogic.loadAndCreateHtmlForWorkbenchs($("#container_workspace")).done(function (data) {
                }).fail(function () {
                });
            });
        }

        export function loadExtents(workspaceId: string) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForExtents($("#container_extents"), workspaceId).done(function (data) {
                }).fail(function () {
                });
            });
        }

        export function loadExtent(workspaceId: string, extentUrl: string) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForItems($("#container_item"), workspaceId, extentUrl);
            });
        }

        /*
         * Used to show a lot of items in a database. The table will use an array of MofObjects
         * as the datasource
         */
        export class DataTable {
            columns: Array<DataTableColumn>;
            items: Array<DataTableItem>;

            constructor(items: Array<DataTableItem>, columns: Array<DataTableColumn>) {
                this.items = items;
                this.columns = columns;
            }

            // Replaces the content at the dom with the created table
            show(dom: JQuery) {
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

                    domTable.append(domRow);
                }

                dom.append(domTable);
            }
        }
    }
};
