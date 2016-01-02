import * as DMI from "datenmeister-interfaces";
import * as DMTables from "datenmeister-tables";
import * as DMClient from "datenmeister-client";

export class WorkspaceLayout {
    onWorkspaceSelected: (id: string) => void;

    loadAndCreateHtmlForWorkbenchs(container: JQuery): JQueryPromise<boolean> {
        var result = $.Deferred();
        var tthis = this;
        DMClient.WorkspaceApi.getAllWorkspaces()
            .done((data) => {
                tthis.createHtmlForWorkbenchs(container, data);
                result.resolve(true);
            });

        return result;
    }

    createHtmlForWorkbenchs(container: JQuery, data: Array<DMI.IWorkspace>) {
        container.empty();
        var compiledTable = $($("#template_workspace_table").html());
        var compiled = _.template($("#template_workspace").html());
        for (var n in data) {
            if (data.hasOwnProperty(n)) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (localEntry => (
                        () => {
                            var workspaceId = localEntry.id;
                            if (this.onWorkspaceSelected != undefined) {
                                this.onWorkspaceSelected(workspaceId);
                            }

                            return false;
                        })
                    )(entry)
                );

                $(compiledTable).append(dom);
            }
        }

        container.append(compiledTable);
    }
}

export class ExtentLayout {
    onExtentSelected: (ws: string, extent: string) => void;
    onItemSelected: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemCreated: (ws: string, extentUrl: string, itemUrl: string) => void;


    loadAndCreateHtmlForWorkspace(container: JQuery, ws: string): JQueryPromise<Object> {
        var callback = $.Deferred();
        $.ajax(
            {
                url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
                cache: false,
                success: data => {
                    this.createHtmlForWorkspace(container, ws, data);
                    callback.resolve(null);
                },
                error: data => {
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
                if (data.hasOwnProperty(n)) {
                    var entry = data[n];
                    var line = compiled(entry);
                    var dom = $(line);
                    $(".data", dom).click(
                        ((localEntry: DMI.IExtent) => (
                            () => {
                                if (tthis.onExtentSelected !== undefined) {
                                    tthis.onExtentSelected(ws, localEntry.uri);
                                }

                                return false;
                            })
                        )(entry)
                    );

                    compiledTable.append(dom);
                }
            }

            container.append(compiledTable);
        }
    }

    loadAndCreateHtmlForExtent(container: JQuery, ws: string, extentUrl: string, query?: DMI.IItemTableQuery): JQueryPromise<Object> {
        var callback = $.Deferred();
        this.loadHtmlForExtent(ws, extentUrl)
            .done((data: DMI.IExtentContent) => {
                this.createHtmlForExtent(container, ws, extentUrl, data);
                callback.resolve(null);
            })
            .fail(data => {
                callback.reject(null);
            });

        return callback;
    }

    loadHtmlForExtent(ws: string, extentUrl: string, query?: DMI.IItemTableQuery): JQueryXHR {
        var url = "/api/datenmeister/extent/items?ws=" + encodeURIComponent(ws)
            + "&extent=" + encodeURIComponent(extentUrl);

        if (query !== undefined && query !== null) {
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
            var callback = DMClient.ExtentApi.deleteItem(ws, extentUrl, url);
            callback
                .done(() => {
                    // tthis.loadAndCreateHtmlForItems(container, ws, extentUrl);
                    domRow.find("td").fadeOut(500, () => { domRow.remove(); });
                })
                .fail(() => { alert("FAILED"); });
            return false;
        };

        var table = new DMTables.ItemListTable(container, data, configuration);

        configuration.onSearch = searchString => {
            this.loadHtmlForExtent(ws, extentUrl, { searchString: searchString })
                .done((innerData: DMI.IExtentContent) => {
                    if (table.lastProcessedSearchString === innerData.search) {
                        table.updateItems(innerData);
                    }
                });
        };

        configuration.onNewItemClicked = () => {
            DMClient.ExtentApi.createItem(ws, extentUrl)
                .done((innerData: DMI.ReturnModule.ICreateItemResult) => {
                    this.onItemCreated(ws, extentUrl, innerData.newuri);
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
        var callback = $.Deferred();
        $.ajax({
            url: "/api/datenmeister/extent/item?ws=" + encodeURIComponent(ws)
            + "&extent=" + encodeURIComponent(extentUrl)
            + "&item=" + encodeURIComponent(itemUrl),
            cache: false,
            success: data => {
                this.createHtmlForItem(container, ws, extentUrl, itemUrl, data);
                callback.resolve(null);
            },
            error: data => {
                callback.reject(null);
            }
        });

        return callback;
    }

    createHtmlForItem(jQuery: JQuery, ws: string, extentUrl: string, itemUrl: string, data: DMI.IItemContentModel) {
        var configuration = new DMTables.ItemContentConfiguration();
        configuration.deleteFunction = (url: string, property: string, domRow: JQuery) => {
            DMClient.ItemApi.deleteProperty(ws, extentUrl, itemUrl, property).done(() => domRow.find("td").fadeOut(500, () => { domRow.remove(); }));
            return false;
        };

        configuration.onEditProperty = (url: string, property: string, newValue: string) => {
            DMClient.ItemApi.setProperty(ws, extentUrl, itemUrl, property, newValue);
        };

        configuration.onNewProperty = (url: string, property: string, newValue: string) => {
            DMClient.ItemApi.setProperty(ws, extentUrl, itemUrl, property, newValue);
        };

        var table = new DMTables.ItemContentTable(data, configuration);
        table.show(jQuery);
    }
}
