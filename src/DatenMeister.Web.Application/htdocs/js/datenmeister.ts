/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />

import * as DMHelper from "datenmeister-helper";
import * as DMI from "datenmeister-interfaces";
import * as DMTables from "datenmeister-tables"

export class WorkspaceLogic {
    onWorkspaceSelected: (id: string) => void;

    loadAndCreateHtmlForWorkbenchs(container: JQuery): JQueryPromise<Array<DMI.IWorkspace>> {
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

    createHtmlForWorkbenchs(container: JQuery, data: Array<DMI.IWorkspace>) {
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
}

export class ExtentLogic {
    onExtentSelected: (ws: string, extent: string) => void;
    onItemSelected: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemCreated: (ws: string, extentUrl: string, itemUrl: string) => void;

    createItem(ws: string, extentUrl: string, container?: string): JQueryDeferred<DMI.ReturnModule.ICreateItemResult> {
        var callback = $.Deferred();
        var postModel = new DMI.PostModels.ItemCreateModel();
        postModel.ws = ws;
        postModel.extent = extentUrl;
        postModel.container = container;

        $.ajax(
        {
            url: "/api/datenmeister/extent/item_create",
            data: postModel,
            method: "POST",
            success: (data: any) => { callback.resolve(data); },
            error: (data: any) => { callback.reject(false); }
        });

        return callback;
    }

    /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
    deleteItem(ws: string, extent: string, item: string): JQueryPromise<boolean> {
        var callback = $.Deferred();

        var postModel = new DMI.PostModels.ItemDeleteModel();
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

        var postModel = new DMI.PostModels.ItemUnsetPropertyModel();
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

        var postModel = new DMI.PostModels.ItemSetPropertyModel();
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

    loadAndCreateHtmlForWorkspace(container: JQuery, ws: string): JQueryPromise<Object> {
        var tthis = this;

        var callback = $.Deferred();
        $.ajax(
        {
            url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
            cache: false,
            success: function(data) {
                tthis.createHtmlForWorkspace(container, ws, data);
                callback.resolve(null);
            },
            error: function(data) {
                callback.reject(null);
            }
        });

        return callback;
    }

    createHtmlForWorkspace(container: JQuery, ws: string, data: Array<DMI.IExtent>) {
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

    loadAndCreateHtmlForExtent(container: JQuery, ws: string, extentUrl: string, query?: DMI.IItemTableQuery): JQueryPromise<Object> {
        var tthis = this;

        var callback = $.Deferred();
        this.loadHtmlForExtent(ws, extentUrl)
            .done(function(data: DMI.IExtentContent) {
                tthis.createHtmlForExtent(container, ws, extentUrl, data);
                callback.resolve(null);
            })
            .fail(function(data) {
                callback.reject(null);
            });

        return callback;
    }

    loadHtmlForExtent(ws: string, extentUrl: string, query?: DMI.IItemTableQuery): JQueryXHR {
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

        return $.ajax(
        {
            url: url,
            cache: false
        });
    }

    createHtmlForExtent(container: JQuery, ws: string, extentUrl: string, data: DMI.IExtentContent) {
        var tthis = this;
        var configuration = new DMTables.ItemTableConfiguration();
        configuration.onItemEdit = (url: string) => {

            if (this.onItemSelected !== undefined) {
                this.onItemSelected(ws, extentUrl, url);
            }

            return false;
        };

        configuration.onItemDelete = (url: string, domRow: JQuery) => {
            var callback = this.deleteItem(ws, extentUrl, url);
            callback
                .done(() => {
                    // tthis.loadAndCreateHtmlForItems(container, ws, extentUrl);
                    domRow.find("td").fadeOut(500, () => { domRow.remove(); });
                })
                .fail(() => { alert("FAILED"); });
            return false;
        };

        var table = new DMTables.ItemListTable(container, data, configuration);

        configuration.onSearch = function(searchString) {
            tthis.loadHtmlForExtent(ws, extentUrl, { searchString: searchString })
                .done((innerData: DMI.IExtentContent) => {
                    if (table.lastProcessedSearchString === innerData.search) {
                        table.updateItems(innerData);
                    }
                });
        };

        configuration.onNewItemClicked = function() {
            tthis.createItem(ws, extentUrl)
                .done((innerData: DMI.ReturnModule.ICreateItemResult) => {
                    tthis.onItemCreated(ws, extentUrl, innerData.newuri);
                });
        };

        var itemsPerPage = 10;
        configuration.onPageChange = (newPage: number) => {
            tthis.loadHtmlForExtent(ws, extentUrl, {
                    offset: itemsPerPage * (newPage - 1),
                    amount: itemsPerPage
                })
                .done((innerData: DMI.IExtentContent) => {
                    table.updateItems(innerData);
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

    createHtmlForItem(jQuery: JQuery, ws: string, extentUrl: string, itemUrl: string, data: DMI.IItemContentModel) {
        var tthis = this;
        var configuration = new DMTables.ItemContentConfiguration();
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

        var table = new DMTables.ItemContentTable(data, configuration);
        table.show(jQuery);
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
        var ws = DMHelper.getParameterByNameFromHash("ws");
        var extentUrl = DMHelper.getParameterByNameFromHash("ext");
        var itemUrl = DMHelper.getParameterByNameFromHash("item");

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
            containerRefresh.click(() => {
                loadWorkspaces();
                return false;
            });
        } else if (extentUrl === undefined) {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - Extents - ");
            containerRefresh.click(() => {
                loadExtents(ws);
                return false;
            });
        } else if (itemUrl == undefined) {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - Items - ");
            containerRefresh.click(() => {
                loadExtent(ws, extentUrl);
                return false;
            });
        } else {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - <a href='#' class='link_items'>Items</a> - ");
            containerRefresh.click(() => {
                loadItem(ws, extentUrl, itemUrl);
                return false;
            });
        }

        containerTitle.append(containerRefresh);

        $(".link_workspaces", containerTitle).click(() => {
            loadWorkspaces();
            return false;
        });
        $(".link_extents", containerTitle).click(() => {
            loadExtents(ws);
            return false;
        });
        $(".link_items", containerTitle).click(() => {
            loadExtent(ws, extentUrl);
            return false;
        });
    }

    export function loadWorkspaces() {
        var workbenchLogic = new WorkspaceLogic();
        workbenchLogic.onWorkspaceSelected = (id: string) => {
            // Loads the extent of the workspace, if the user has clicked on one of the workbenches
            history.pushState({}, "", "#ws=" + encodeURIComponent(id));
            loadExtents(id);
        };

        workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".container_data"))
            .done(function(data) {
                createTitle();
            })
            .fail(function() {
            });
    }

    export function loadExtents(workspaceId: string) {
        var extentLogic = new ExtentLogic();
        extentLogic.onExtentSelected = function(ws: string, extentUrl: string) {
            history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                + "&ext=" + encodeURIComponent(extentUrl));
            loadExtent(ws, extentUrl);
            return false;
        };

        extentLogic.loadAndCreateHtmlForWorkspace($(".container_data"), workspaceId)
            .done(function(data) {
                createTitle(workspaceId);
            })
            .fail(function() {
            });
    }

    export function loadExtent(workspaceId: string, extentUrl: string) {
        var extentLogic = new ExtentLogic();
        extentLogic.onItemSelected = function(ws: string, extentUrl: string, itemUrl: string) {
            navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.onItemCreated = function(ws: string, extentUrl: string, itemUrl: string) {
            navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.loadAndCreateHtmlForExtent($(".container_data"), workspaceId, extentUrl).done(
            data => {
                createTitle(workspaceId, extentUrl);
            });
    }

    export function navigateToItem(ws: string, extentUrl: string, itemUrl: string) {
        history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
            + "&ext=" + encodeURIComponent(extentUrl)
            + "&item=" + encodeURIComponent(itemUrl));
        loadItem(ws, extentUrl, itemUrl);
    }

    export function loadItem(workspaceId: string, extentUrl: string, itemUrl: string) {
        var extentLogic = new ExtentLogic();

        createTitle(workspaceId, extentUrl, itemUrl);
        extentLogic.loadAndCreateHtmlForItem($(".container_data"), workspaceId, extentUrl, itemUrl);
    }
}