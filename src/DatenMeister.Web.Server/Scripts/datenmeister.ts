﻿/// <reference path="typings/jquery/jquery.d.ts" />

module DatenMeister {

    export interface IWorkspace {
        id: string;
        annotation: string;
    };

    export interface IExtent {
        url: string;
    }

    export interface IExtentContent {
        url: string;
        totalItemCount: number;
        filteredItemCount: number;
        columns: Array<IDataTableColumn>;
        items: Array<IDataTableItem>;
    };

    export interface IDataTableColumn {
        title: string;
        name: string;
    }

    export class DataTableConfiguration {
        editFunction: (url: string) => boolean;
        deleteFunction: (url: string) => boolean;

        DataTableConfiguration() {
            this.editFunction = function (url: string) { return false; /*Ignoring*/ };
            this.deleteFunction = function (url: string) { return false; /*Ignoring*/ };
        }
    }

    export interface IDataTableItem {
        // Stores the url of the object which can be used for reference
        uri: string;
        v: Array<string>;
    }

    export class WorkspaceLogic {
        loadAndCreateHtmlForWorkbenchs(container: JQuery): JQueryPromise<Array<IWorkspace>> {
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
        
        createHtmlForWorkbenchs(container: JQuery, data: Array<IWorkspace>) {
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (function (localEntry) {                        
                        return function () {
                            location.href = `/Home/workspace?ws=${encodeURIComponent(localEntry.id)}`;
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

        createHtmlForExtent(container: JQuery, ws: string, data: Array<IWorkspace>) {
            container.empty();
            var compiled = _.template($("#template_extent").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (function (localEntry) {
                        return function () {
                            location.href = "/Home/extent?ws=" + encodeURIComponent(ws)
                                + "&extent=" + encodeURIComponent(localEntry.uri);
                        };
                    } (entry)));

                container.append(dom);
            }
        }

        loadAndCreateHtmlForItems(container: JQuery, ws: string, extentUrl: string) {
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

        createHtmlForItems(container: JQuery, ws: string, extentUrl: string, data: IExtentContent) {
            var configuration = new DataTableConfiguration();
            configuration.editFunction = function (url: string) {

                location.href = "/Home/item?ws=" + encodeURIComponent(ws)
                    + "&extent=" + encodeURIComponent(extentUrl)
                    + "&item=" + encodeURIComponent(url);
                return false;
            };
            configuration.deleteFunction = function (url: string) {
                alert("DELETE: " + url);
                return false;
            };

            var table = new GUI.DataTable(data.items, data.columns, configuration);
            table.show(container);
        }

        loadAndCreateHtmlForItem(jQuery: JQuery, workspaceId: string, extentUrl: string, itemUrl: string) {
            // TODO: DO A LOT
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

        export function loadItem(workspaceId: string, extentUrl: string, itemUrl: string) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForItem($("#container_item"), workspaceId, extentUrl, itemUrl);
            });
        }

        /*
         * Used to show a lot of items in a database. The table will use an array of MofObjects
         * as the datasource
         */
        export class DataTable {
            columns: Array<IDataTableColumn>;
            items: Array<IDataTableItem>;
            configuration: DataTableConfiguration;

            constructor(items: Array<IDataTableItem>, columns: Array<IDataTableColumn>, configuration: DataTableConfiguration) {
                this.items = items;
                this.columns = columns;
                this.configuration = configuration;
            }

            // Replaces the content at the dom with the created table
            show(dom: JQuery) {
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
                            return tthis.configuration.editFunction (url);
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
    }
};
