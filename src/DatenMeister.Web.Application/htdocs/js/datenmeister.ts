﻿/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />

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

    export interface IItemContentModel {
        uri: string;
        v: Array<string>;
    }

    export interface IDataTableColumn {
        title: string;
        name: string;
    }

    export interface IDataTableItem {
        // Stores the url of the object which can be used for reference
        uri: string;
        v: Array<string>;
    }

    export module PostModels {

        /** This class is used to reference a single object within the database */
        export class ItemReferenceModel {
            ws: string;
            extent: string;
            item: string;
        }

        export class ItemUnsetPropertyModel extends ItemReferenceModel {
            property: string;
        }

        export class ItemDeleteModel extends ItemReferenceModel {
        }
    }

    export class WorkspaceLogic {
        onWorkspaceSelected: (id: string) => void;

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
            var tthis = this;
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (function (localEntry) {          
                        return function () {
                            var workspaceId = localEntry.id;
                            if (tthis.onWorkspaceSelected != null) {
                                tthis.onWorkspaceSelected(localEntry.id);
                            }

                        };
                    } (entry)));

                container.append(dom);
            }
        }
    };

    export namespace Models {
        export class ItemReferenceModel {
            ws: string;
            extent: string;
            item: string;
        }

        export class ItemDeleteModel extends ItemReferenceModel {

        }
    }

    export class ExtentLogic {
        onExtentSelected: (ws: string, extent: string) => void;
        onItemSelected: (ws: string, extentUrl: string, itemUrl: string) => void;

        /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
        deleteItem(ws: string, extent: string, item: string): JQueryPromise<boolean> {
            var callback = $.Deferred();

            var postModel = new PostModels.ItemDeleteModel();
            postModel.ws = ws;
            postModel.extent = extent;
            postModel.item = item;

            $.ajax("/api/datenmeister/extent/item_delete",
                {
                    data: postModel,
                    method: "POST"
                })
                .done((data: any) => { callback.resolve(true); })
                .fail((data: any) => { callback.resolve(false); });
            return callback;
        }

        deleteProperty(ws: string, extent: string, item: string, property: string): JQueryPromise<boolean> {
            var callback = $.Deferred();


            var postModel = new PostModels.ItemUnsetPropertyModel();
            postModel.ws = ws;
            postModel.extent = extent;
            postModel.item = item;
            postModel.property = property;

            $.ajax("/api/datenmeister/extent/item_unset_property", 
                {
                    data: postModel,
                    method: "POST"
                })
                .done((data: any) => { callback.resolve(true); })
                .fail((data: any) => { callback.resolve(false); });
            return callback;
        }

        loadAndCreateHtmlForExtents(container: JQuery, ws: string): JQueryPromise<Object> {
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

        createHtmlForExtent(container: JQuery, ws: string, data: Array<IExtent>) {
            var tthis = this;
            container.empty();

            if (data.length === 0) {

                container.html("<tr><td>No extents were found</td></tr>");

            } else {
                var compiled = _.template($("#template_extent").html());
                for (var n in data) {
                    var entry = data[n];
                    var line = compiled(entry);
                    var dom = $(line);
                    $(".data", dom).click(
                    (function(localEntry) {
                        return function() {
                            if (tthis.onExtentSelected !== null) {
                                tthis.onExtentSelected(ws, localEntry.uri);
                            }
                        };
                    }(entry)));

                    container.append(dom);
                }
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
            var tthis = this;
            var configuration = new GUI.DataTableConfiguration();
            configuration.editFunction = function (url: string) {

                if (tthis.onItemSelected !== null) {
                    tthis.onItemSelected(ws, extentUrl, url);
                }
                return false;
            };

            configuration.deleteFunction = function (url: string, domRow: JQuery) {
                var callback = tthis.deleteItem(ws, extentUrl, url);
                callback
                    .done(() => {
                        // tthis.loadAndCreateHtmlForItems(container, ws, extentUrl);
                        domRow.find("td").fadeOut(500, () => { domRow.remove(); });
                    })
                    .fail(() => { alert("FAILED"); });
                return false;
            };

            var table = new GUI.DataTable(data.items, data.columns, configuration);
            table.show(container);
        }

        loadAndCreateHtmlForItem(container: JQuery, ws: string, extentUrl: string, itemUrl: string) {
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

        createHtmlForItem(jQuery: JQuery, ws: string, extentUrl: string, itemUrl: string, data: IItemContentModel) {
            var tthis = this;
            var configuration = new GUI.ItemContentConfiguration();
            configuration.deleteFunction = (url: string, property: string, domRow: JQuery) => {
                tthis.deleteProperty(ws, extentUrl, itemUrl, property).done(() => domRow.find("td").fadeOut(500, () => { domRow.remove(); }));
                return false;
            };

            var table = new GUI.ItemContentTable(data, configuration);

            table.show(jQuery);
        }

    }

    export namespace GUI {
        export function start() {
            $(document).ready(() => { loadWorkspaces(); });
        }

        export function loadWorkspaces() {
            var workbenchLogic = new DatenMeister.WorkspaceLogic();
            workbenchLogic.onWorkspaceSelected = (id: string) => {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                loadExtents(id);
            };
            $(".container_title").text("Workspaces");
            workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".container_data"))
                .done(function(data) {
                })
                .fail(function() {
                });
        }

        export function loadExtents(workspaceId: string) {
            var extentLogic = new DatenMeister.ExtentLogic();
            var containerTitle = $(".container_title");
            containerTitle.html("<a href='#' class='link_workspace'>Workspaces</a> - Extents");
            $(".link_workspace", containerTitle).click(function () {
                loadWorkspaces();
                return false;
            });
            extentLogic.loadAndCreateHtmlForExtents($(".container_data"), workspaceId)
                .done(function(data) {
                })
                .fail(function() {
                });
        }

        export function loadExtent(workspaceId: string, extentUrl: string) {
            var extentLogic = new DatenMeister.ExtentLogic();
            $(".container_title").text("Items");
            extentLogic.loadAndCreateHtmlForItems($(".container_data"), workspaceId, extentUrl);
        }

        export function loadItem(workspaceId: string, extentUrl: string, itemUrl: string) {
            var extentLogic = new DatenMeister.ExtentLogic();
            extentLogic.loadAndCreateHtmlForItem($(".container_data"), workspaceId, extentUrl, itemUrl);
        }

        export class DataTableConfiguration {
            editFunction: (url: string, domRow: JQuery) => boolean;
            deleteFunction: (url: string, domRow: JQuery) => boolean;

            constructor() {
                this.editFunction = function (url: string, domRow: JQuery) { return false; /*Ignoring*/ };
                this.deleteFunction = function (url: string, domRow: JQuery) { return false; /*Ignoring*/ };
            }
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
                    domEditColumn.click((function (url, iDomRow) {
                        return function () {
                            return tthis.configuration.editFunction(url, iDomRow);
                        };
                    })(item.uri, domRow));                   
                    domRow.append(domEditColumn);

                    domDeleteColumn = $("<td class='hl'><a href='#'>DELETE</a></td>");
                    var domA = $("a", domDeleteColumn);
                    domDeleteColumn.click((function (url: string, idomRow: JQuery, idomA: JQuery) {
                        return function () {
                            if (idomA.data("wasClicked") === true) {
                                
                                return tthis.configuration.deleteFunction(url, idomRow);
                            } else {
                                idomA.data("wasClicked", true);
                                idomA.text("CONFIRM");
                            }
                        };
                    })(item.uri, domRow, domA));
                    domRow.append(domDeleteColumn);  

                    domTable.append(domRow);
                }

                dom.append(domTable);
            }
        }
        
        export class ItemContentConfiguration {
            autoProperties: boolean;

            editFunction: (url: string, property: string, domRow: JQuery) => boolean;
            deleteFunction: (url: string, property: string, domRow: JQuery) => boolean;

            constructor() {
                this.editFunction = (url: string, property: string, domRow: JQuery) => false;
                this.deleteFunction = (url: string, property: string, domRow: JQuery) => false;
            }
        }

        export class ItemContentTable {
            item: IDataTableItem;
            configuration: ItemContentConfiguration;

            constructor(item: IDataTableItem, configuration: ItemContentConfiguration) {
                this.item = item;
                this.configuration = configuration;
            }

            show(dom: JQuery) {
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
                    domEditColumn.click((function (url: string, property: string, idomRow: JQuery, idomA: JQuery) {
                        return function () {
                            return tthis.configuration.editFunction(url, property, idomRow);
                        };
                    })(this.item.uri, property, domRow, domA));
                    domRow.append(domEditColumn);

                    let domDeleteColumn = $("<td class='hl'><a href='#'>DELETE</a></td>");
                    var domA = $("a", domDeleteColumn);
                    domDeleteColumn.click((function (url: string, property: string, idomRow: JQuery, idomA: JQuery) {
                        return function () {
                            if (idomA.data("wasClicked") === true) {

                                return tthis.configuration.deleteFunction(url, property, idomRow);
                            } else {
                                idomA.data("wasClicked", true);
                                idomA.text("CONFIRM");
                            }
                            
                        };
                    })(this.item.uri, property, domRow, domA));
                    domRow.append(domDeleteColumn);

                    domTable.append(domRow);
                }

                dom.append(domTable);

            }
        }
    }
};
