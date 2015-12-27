/// <reference path="typings/jquery/jquery.d.ts" />
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
        search: string;
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
        export class ExtentReferenceModel {
            ws: string;
            extent: string;
        }

        export class ItemReferenceModel extends ExtentReferenceModel {
            item: string;
        }

        export class ItemCreateModel extends ExtentReferenceModel
        {
            container: string
        }

        export class ItemUnsetPropertyModel extends ItemReferenceModel {
            property: string;
        }

        export class ItemDeleteModel extends ItemReferenceModel {
        }

        export class ItemSetPropertyModel extends ItemReferenceModel {
            property: string; 
            newValue: string;
        }
    }

    export class WorkspaceLogic {
        onWorkspaceSelected: (id: string) => void;

        loadAndCreateHtmlForWorkbenchs(container: JQuery): JQueryPromise<Array<IWorkspace>> {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/workspace/all",
                cache: false,
                success: function(data) {
                    tthis.createHtmlForWorkbenchs(container, data);
                    callback.resolve(null);
                },
                error: function(data) {
                    callback.reject(null);
                }
            });

            return callback;
        }
        
        createHtmlForWorkbenchs(container: JQuery, data: Array<IWorkspace>) {
            var tthis = this;
            container.empty();
            var compiledTable = $($("#template_workspace_table").html());
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                (function(localEntry) {
                    return function() {
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

        createItem(ws: string, extentUrl: string, container?: string) {
            var callback = $.Deferred();
            var postModel = new PostModels.ItemCreateModel();
            postModel.ws = ws;
            postModel.extent = extentUrl;
            postModel.container = container;

            $.ajax(
            {
                url: "/api/datenmeister/extent/item_create",
                data: postModel,
                method: "POST",
                success: (data: any) => { callback.resolve(true); },
                error: (data: any) => { callback.reject(false); }
            });
        }

        /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
        deleteItem(ws: string, extent: string, item: string): JQueryPromise<boolean> {
            var callback = $.Deferred();

            var postModel = new PostModels.ItemDeleteModel();
            postModel.ws = ws;
            postModel.extent = extent;
            postModel.item = item;

            $.ajax(
            {
                url: "/api/datenmeister/extent/item_delete",
                data: postModel,
                method: "POST",
                success: (data: any) => { callback.resolve(true); },
                error: (data: any) => { callback.reject(false); }
            });

            return callback;
        }

        deleteProperty(ws: string, extent: string, item: string, property: string): JQueryPromise<boolean> {
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
                success: (data: any) => { callback.resolve(true); },
                error: (data: any) => { callback.resolve(false); }
            });

            return callback;
        }

        setProperty(ws: string, extentUrl: string, itemUrl: string, property: string, newValue: string): JQueryPromise<boolean> {
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
                success: (data: any) => { callback.resolve(true); },
                error: (data: any) => { callback.resolve(false); }
            });

            return callback;
        }

        loadAndCreateHtmlForExtents(container: JQuery, ws: string): JQueryPromise<Object> {
            var tthis = this;

            var callback = $.Deferred();
            $.ajax(
            {
                url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
                cache: false,
                success: function(data) {
                    tthis.createHtmlForExtent(container, ws, data);
                    callback.resolve(null);
                },
                error: function(data) {
                    callback.reject(null);
                }
            });

            return callback;
        }

        createHtmlForExtent(container: JQuery, ws: string, data: Array<IExtent>) {
            var tthis = this;
            container.empty();

            if (data.length === 0) {

                container.html("<p>No extents were found</p>");

            } else {
                var compiledTable = $($("#template_extent_table").html());
                var compiled = _.template($("#template_extent").html());
                for (var n in data) {
                    var entry = data[n];
                    var line = compiled(entry);
                    var dom = $(line);
                    $(".data", dom).click(
                    (function(localEntry) {
                        return function() {
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
        }

        loadAndCreateHtmlForItems(container: JQuery, ws: string, extentUrl: string): JQueryPromise<Object>  {
            var tthis = this;

            var callback = $.Deferred();
            this.loadHtmlForItems(ws, extentUrl)
                .done(function(data: IExtentContent) {
                    tthis.createHtmlForItems(container, ws, extentUrl, data);
                    callback.resolve(null);
                })
                .fail(function(data) {
                    callback.reject(null);
                });

            return callback;
        }

        loadHtmlForItems(ws: string, extentUrl: string, searchString?: string): JQueryXHR {
            var url = "/api/datenmeister/extent/items?ws=" + encodeURIComponent(ws)
                + "&extent=" + encodeURIComponent(extentUrl);

            if (searchString !== undefined) {
                url += "&search=" + encodeURIComponent(searchString);
            }

            return $.ajax(
                {
                    url: url,
                    cache: false
                });
        }

        createHtmlForItems(container: JQuery, ws: string, extentUrl: string, data: IExtentContent) {
            var tthis = this;
            var configuration = new GUI.DataTableConfiguration();
            configuration.editFunction = function (url: string) {

                if (tthis.onItemSelected !== undefined) {
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

            var table = new GUI.ItemListTable(container, data.items, data.columns, configuration);

            configuration.onSearch = function (searchString) {
                tthis.loadHtmlForItems(ws, extentUrl, searchString)
                    .done((data: IExtentContent) => {
                        if (table.lastProcessedSearchString === data.search) {
                            table.updateItems(data.items);
                        }
                    });
            };

            table.show();
        }

        loadAndCreateHtmlForItem(container: JQuery, ws: string, extentUrl: string, itemUrl: string) {
            var tthis = this;

            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/item?ws=" + encodeURIComponent(ws)
                    + "&extent=" + encodeURIComponent(extentUrl)
                    + "&item=" + encodeURIComponent(itemUrl),
                cache: false,
                success: function(data) {
                    tthis.createHtmlForItem(container, ws, extentUrl, itemUrl, data);
                    callback.resolve(null);
                },
                error: function(data) {
                    callback.reject(null);
                }
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

            configuration.onEditProperty = (url: string, property: string, newValue: string) => {
                tthis.setProperty(ws, extentUrl, itemUrl, property, newValue);
            };

            configuration.onNewProperty = (url: string, property: string, newValue: string) => {
                tthis.setProperty(ws, extentUrl, itemUrl, property, newValue);
            };

            var table = new GUI.ItemContentTable(data, configuration);
            table.show(jQuery);
        }
    }

    export namespace Helper {
        // Helper function out of http://stackoverflow.com/questions/901115/how-can-i-get-query-string-values-in-javascript
        export function getParameterByNameFromHash(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&#]" + name + "=([^&#]*)"),
                results = regex.exec(location.hash);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
    }

    export namespace GUI {
        export function start() {
            $(document).ready(() => {
                window.onpopstate = ev => {
                    parseAndNavigateToWindowLocation();
                }; 

                parseAndNavigateToWindowLocation();
            });
        }

        export function parseAndNavigateToWindowLocation() {
            var ws = Helper.getParameterByNameFromHash("ws");
            var extentUrl = Helper.getParameterByNameFromHash("ext");
            var itemUrl = Helper.getParameterByNameFromHash("item");

            if (ws === "") {
                loadWorkspaces();
            } else if (extentUrl === "") {
                loadExtents(ws);
            } else if (itemUrl === "") {
                loadExtent(ws, extentUrl);
            } else {
                loadItem(ws, extentUrl, itemUrl);
            }
        }

        function createTitle(ws?: string, extentUrl?: string, itemUrl?: string) {
            var containerTitle = $(".container_title");
            var containerRefresh = $("<a href='#'>Refresh</a>");

            if (ws === undefined) {
                containerTitle.text("Workspaces - ");
                containerRefresh.click(() => { loadWorkspaces();
                    return false;
                });
            } else if (extentUrl === undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - Extents - ");
                containerRefresh.click(() => { loadExtents(ws);
                    return false;
                });
            } else if (itemUrl == undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - Items - ");
                containerRefresh.click(() => { loadExtent(ws, extentUrl);
                    return false;
                });
            } else {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - <a href='#' class='link_items'>Items</a> - ");
                containerRefresh.click(() => { loadItem(ws, extentUrl, itemUrl);
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

        export function loadWorkspaces() {
            var workbenchLogic = new DatenMeister.WorkspaceLogic();
            workbenchLogic.onWorkspaceSelected = (id: string) => {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                history.pushState({}, '', "#ws=" + encodeURIComponent(id));
                loadExtents(id);
            };

            workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".container_data"))
                .done(function (data) {
                    createTitle();
                })
                .fail(function() {
                });
        }

        export function loadExtents(workspaceId: string) {
            var extentLogic = new DatenMeister.ExtentLogic();
            extentLogic.onExtentSelected = function (ws: string, extentUrl: string) {
                history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                    + "&ext=" + encodeURIComponent(extentUrl));
                loadExtent(ws, extentUrl);
                return false;
            };

            extentLogic.loadAndCreateHtmlForExtents($(".container_data"), workspaceId)
                .done(function (data) {
                    createTitle(workspaceId);
                })
                .fail(function() {
                });
        }

        export function loadExtent(workspaceId: string, extentUrl: string) {
            var extentLogic = new DatenMeister.ExtentLogic();
            extentLogic.onItemSelected = function(ws: string, extentUrl: string, itemUrl: string) {
                history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                    + "&ext=" + encodeURIComponent(extentUrl)
                    + "&item=" + encodeURIComponent(itemUrl));
                loadItem(ws, extentUrl, itemUrl);
            };

            extentLogic.loadAndCreateHtmlForItems($(".container_data"), workspaceId, extentUrl).done(
                function(data) {
                    createTitle(workspaceId, extentUrl);
                });
        }

        export function loadItem(workspaceId: string, extentUrl: string, itemUrl: string) {
            var extentLogic = new DatenMeister.ExtentLogic();

            createTitle(workspaceId, extentUrl, itemUrl);
            extentLogic.loadAndCreateHtmlForItem($(".container_data"), workspaceId, extentUrl, itemUrl);
        }

        export class DataTableConfiguration {
            editFunction: (url: string, domRow: JQuery) => boolean;
            deleteFunction: (url: string, domRow: JQuery) => boolean;
            supportSearchbox: boolean;
            onSearch: (searchText: string) => void;

            constructor() {
                this.editFunction = function (url: string, domRow: JQuery) { return false; /*Ignoring*/ };
                this.deleteFunction = function (url: string, domRow: JQuery) { return false; /*Ignoring*/ };
                this.supportSearchbox = true;
            }
        }
        
        /*
         * Used to show a lot of items in a database. The table will use an array of MofObjects
         * as the datasource
         */
        export class ItemListTable {
            columns: Array<IDataTableColumn>;
            items: Array<IDataTableItem>;
            configuration: DataTableConfiguration;
            domContainer: JQuery;
            domTable: JQuery;
            lastProcessedSearchString: string;

            constructor(dom: JQuery, items: Array<IDataTableItem>, columns: Array<IDataTableColumn>, configuration: DataTableConfiguration) {
                this.domContainer = dom;
                this.items = items;
                this.columns = columns;
                this.configuration = configuration;
            }

            // Replaces the content at the dom with the created table
            show() {
                var tthis = this;
                this.domContainer.empty();

                if (this.configuration.supportSearchbox) {
                    var domSearchBox = $("<div><input type='textbox' /></div>");
                    var domInput = $("input", domSearchBox);
                    $("input", domSearchBox).keyup(() => {
                        var searchValue = domInput.val();
                        if (tthis.configuration.onSearch !== undefined) {
                            tthis.lastProcessedSearchString = searchValue;
                            tthis.configuration.onSearch(searchValue);
                        }
                    });

                    this.domContainer.append(domSearchBox);
                }

                this.domTable = $("<table class='table'></table>");

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

                this.domTable.append(domRow);

                // Now, the items
                tthis.createRowsForData();

                this.domContainer.append(this.domTable);
            }

            createRowsForData(): void {
                var tthis = this;
                // Now, the items
                for (var i in this.items) {
                    var item = this.items[i];

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

                    for (var c in this.columns) {
                        var column = this.columns[c];
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
                    domDeleteColumn.click((function (url: string, innerDomRow: JQuery, innerDomA: JQuery) {
                        return function () {
                            if (innerDomA.data("wasClicked") === true) {
                                return tthis.configuration.deleteFunction(url, innerDomRow);
                            } else {
                                innerDomA.data("wasClicked", true);
                                innerDomA.text("CONFIRM");
                                return false;
                            }
                        };
                    })(item.uri, domRow, domA));

                    domRow.append(domDeleteColumn);
                    this.domTable.append(domRow);
                }
            }

            updateItems(items) {
                this.items = items;
                $("tr", this.domTable).has("td")
                    .remove();
                this.createRowsForData();
            }
        }

        export class ItemContentConfiguration {
            autoProperties: boolean;

            // Gets or sets a flag, that the user can change the content of a property within the table. 
            // If the editing was performed, the onEditProperty-function will get called
            supportInlineEditing: boolean;
            supportNewProperties: boolean;

            editFunction: (url: string, property: string, domRow: JQuery) => boolean;
            deleteFunction: (url: string, property: string, domRow: JQuery) => boolean;

            onEditProperty: (url: string, property: string, newValue: string) => void;
            onNewProperty: (url: string, property: string, newValue: string) => void;

            constructor() {
                this.editFunction = (url: string, property: string, domRow: JQuery) => false;
                this.deleteFunction = (url: string, property: string, domRow: JQuery) => false;
                this.supportInlineEditing = true;
                this.supportNewProperties = true;
            }
        }

        export class ItemContentTable {
            item: IDataTableItem;
            configuration: ItemContentConfiguration;
            domContainer: JQuery;

            constructor(item: IDataTableItem, configuration: ItemContentConfiguration) {
                this.item = item;
                this.configuration = configuration;
            }

            show(dom: JQuery) {
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
                    let domEditColumn = $("<td class='hl table_column_edit'><a href='#'>EDIT</a></td>");
                    $("a", domEditColumn).click((function (url: string, property: string, idomRow: JQuery, idomA: JQuery) {
                        return function () {
                            if (tthis.configuration.supportInlineEditing) {
                                tthis.startInlineEditing(property, idomRow);
                                return false;
                            } else {
                                return tthis.configuration.editFunction(url, property, idomRow);
                            }
                        };
                    })(this.item.uri, property, domRow, domA));
                    domRow.append(domEditColumn);

                    let domDeleteColumn = $("<td class='hl table_column_delete'><a href='#'>DELETE</a></td>");
                    var domA = $("a", domDeleteColumn);
                    $("a", domDeleteColumn).click((function (url: string, property: string, idomRow: JQuery, idomA: JQuery) {
                        return function () {
                            if (idomA.data("wasClicked") === true) {
                                return tthis.configuration.deleteFunction(url, property, idomRow);
                            } else {
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
            }

            startInlineEditing(property: string, domRow: JQuery) {
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
                domEditOK.on('click', () => {
                    var newValue = domTextBox.val();
                    tthis.item.v[property] = newValue;

                    if (tthis.configuration.onEditProperty !== undefined) {
                        tthis.configuration.onEditProperty(tthis.item.uri, property, newValue);
                    }

                    tthis.show(tthis.domContainer);
                    return false;
                });

                domEditCancel.on('click', () => {
                    // Rebuilds the complete table
                    tthis.show(tthis.domContainer);

                    return false;
                });

            }

            offerNewProperty(domTable: JQuery) {
                var tthis = this;
                var domNewProperty = $("<tr><td colspan='4'><a href='#'>NEW PROPERTY</a></td></tr>");
                $("a", domNewProperty).click(() => {
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


                    $("a", domNewPropertyEdit).click(() => {
                        var property = inputProperty.val();
                        var newValue = inputValue.val();

                        tthis.item.v[property] = newValue;

                        if (tthis.configuration.onNewProperty !== undefined) {
                            tthis.configuration.onNewProperty(tthis.item.uri, property, newValue);
                        }

                        tthis.show(tthis.domContainer);
                        return false;
                    });

                    $("a", domNewPropertyCancel).click(() => {
                        tthis.show(tthis.domContainer);
                        return false;
                    });

                    return false;
                });

                domTable.append(domNewProperty);
            }
        }
    }
};
