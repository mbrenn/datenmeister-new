import * as DMI from "./datenmeister-interfaces";
import * as DMTables from "./datenmeister-tables";
import * as DMClient from "./datenmeister-client";
import * as DMQuery from "./datenmeister-query";

export class WorkspaceView {
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

    createHtmlForWorkbenchs(container: JQuery, data: Array<DMI.ClientResponse.IWorkspace>) {
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

export class ExtentView {
    layout: DMI.Api.ILayout;

    constructor(layout?: DMI.Api.ILayout) {
        this.layout = layout;
    }

    onExtentSelected: (ws: string, extent: string) => void;
    onItemEdit: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemView: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemCreated: (ws: string, extentUrl: string, itemUrl: string) => void;

    loadAndCreateHtmlForWorkspace(container: JQuery, ws: string): JQueryPromise<boolean> {
        var callback = $.Deferred();
        $.ajax(
        {
            url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
            cache: false,
            success: (data: Array<DMI.ClientResponse.IExtent>) => {
                this.createHtmlForWorkspace(container, ws, data);
                callback.resolve(true);
            },
            error: data => {
                callback.reject(false);
            }
        });

        return callback;
    }

    createHtmlForWorkspace(container: JQuery, ws: string, data: Array<DMI.ClientResponse.IExtent>) {
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
                        ((localEntry: DMI.ClientResponse.IExtent) => (
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

    loadAndCreateHtmlForExtent(container: JQuery, ws: string, extentUrl: string, query?: DMI.PostModels.IItemTableQuery): JQueryPromise<Object> {
        var tthis = this;

        // Creates the layout configuration and the handling on requests of the user
        var configuration = new DMTables.ItemTableConfiguration();
        configuration.onItemEdit = (url: string) => {
            if (tthis.onItemEdit !== undefined) {
                tthis.onItemEdit(ws, extentUrl, url);
            }

            return false;
        };
        configuration.onItemView = (url: string) => {
            if (tthis.onItemView !== undefined) {
                tthis.onItemView(ws, extentUrl, url);
            }

            return false;
        };

        configuration.onItemDelete = (url: string, domRow: JQuery) => {
            var callback = DMClient.ExtentApi.deleteItem(ws, extentUrl, url);
            callback
                .done(() => {
                    domRow.find("td").fadeOut(500, () => { domRow.remove(); });
                })
                .fail(() => { alert("FAILED"); });
            return false;
        };

        configuration.layout = this.layout;

        // Creates the layout
        var provider = new DMQuery.ItemsFromExtentProvider(ws, extentUrl);
        var table = new DMTables.ItemListTable(container, provider, configuration);
        
        if (query !== undefined && query !== null) {
            table.currentQuery = query;
        }

        configuration.onNewItemClicked = (metaclass) => {
            DMClient.ExtentApi.createItem(ws, extentUrl, undefined, metaclass)
                .done((innerData: DMI.ClientResponse.ICreateItemResult) => {
                    this.onItemCreated(ws, extentUrl, innerData.newuri);
                });
        };
        
        DMClient.ExtentApi.getCreatableTypes(ws, extentUrl).done(
            (data) => {
                table.setCreatableTypes(data.types);
            });


        return table.loadAndShow();
    }
}

export class ItemView
{
    layout: DMI.Api.ILayout;
    onItemView: (ws: string, extentUrl: string, itemUrl: string) => void;

    constructor(layout?: DMI.Api.ILayout) {
        this.layout = layout;
    }

    loadAndCreateHtmlForItem(container: JQuery, ws: string, extentUrl: string, itemUrl: string, settings?: DMI.View.IItemViewSettings): JQueryDeferred<Object> {
        var tthis = this;
        return DMClient.ItemApi.getItem(ws, extentUrl, itemUrl)
            .done((data) => {
                tthis.createHtmlForItem(container, ws, extentUrl, itemUrl, data, settings);
            });
    }

    createHtmlForItem(jQuery: JQuery, ws: string, extentUrl: string, itemUrl: string, data: DMI.ClientResponse.IItemContentModel, settings?: DMI.View.IItemViewSettings) {
        var tthis = this;
        jQuery.empty();
        var configuration = new DMTables.ItemContentConfiguration();
        configuration.columns = data.c;
        var isReadonly = false;

        if (settings !== undefined && settings !== null) {
            if (settings.isReadonly !== undefined && settings.isReadonly !== null) {
                isReadonly = settings.isReadonly;
            }
        }

        configuration.isReadOnly = isReadonly;
        configuration.supportNewProperties = !isReadonly;

        var table = new DMTables.ItemContentTable(data, configuration);
        if (isReadonly) {
            configuration.onOkForm = () => {
                tthis.layout.navigateToItems(ws, extentUrl);
            }

            configuration.onCancelForm = () => {
                tthis.layout.navigateToItems(ws, extentUrl);
            }
        } else {
            configuration.onOkForm = () => {
                DMClient.ItemApi.setProperties(ws, extentUrl, itemUrl, table.item)
                    .done(() => {
                        tthis.layout.navigateToItems(ws, extentUrl);
                    });
            };

            configuration.onCancelForm = () => {
                tthis.layout.navigateToItems(ws, extentUrl);
            }
        }

        configuration.onItemView = (url: string) => {
            if (tthis.onItemView !== undefined) {
                tthis.onItemView(ws, extentUrl, url);
            }

            return false;
        };

        var domTableOwner = $("<div class='data-items'></div>");
        table.show(domTableOwner);

        var domTableInfo = $("<table class='dm-metatable'>" +
            "<tr>" +
            "<th>Id: </th><td class='dm-tablecell-id'>None</td>" +
            "</tr>" +
            "<tr>" +
            "<th>Uri: </th><td class='dm-tablecell-uri'>None</td>" +
            "</tr>" +
            "<tr>" +
            "<th>Metaclass: </th><td class='dm-tablecell-metaclass'></td>" +
            "</tr>" +
            "<tr>" +
            "<th>Layer: </th><td class='dm-tablecell-layer'></td>" +
            "</tr>" +
            "</table>");
        if (data.metaclass !== undefined && data.metaclass !== null) {
            var domMetaClassLink = $("<a href='#'>3</a>").text(data.metaclass.name);
            domMetaClassLink.click(() => {
                tthis.layout.navigateToItem(
                    data.metaclass.ws,
                    data.metaclass.ext,
                    data.metaclass.uri
                );
            });

            $(".dm-tablecell-metaclass", domTableInfo).append(domMetaClassLink);
        }

        if (data.id !== undefined && data.id !== null) {
            $(".dm-tablecell-id", domTableInfo).text(data.id);
        }

        if (data.layer !== undefined && data.layer !== null) {
            $(".dm-tablecell-layer", domTableInfo).text(data.layer);
        }

        if (data.uri !== undefined && data.uri !== null) {

            $(".dm-tablecell-uri", domTableInfo).text(data.uri);
        }

        jQuery.append(domTableOwner);
        jQuery.append(domTableInfo);
    }
}

// This interface should be implemented by all views that can be added via 'setView' to a layout
export interface IView {
    show(container: JQuery): void;
}

// This class gives a navigation view with some links which can be clicked by the user and
// a user-defined action is being performed
export class NavigationView implements IView {
    private layout: DMI.Api.ILayout;
    private domList: JQuery;

    constructor(layout?: DMI.Api.ILayout) {
        this.layout = layout;
        this.domList = $("<ul class='dm-navigation-list'></ul>");
    }

    addLink(displayText: string, onClick: () => void): void {
        let domItem = $("<li></li>");
        domItem.text(displayText);
        domItem.click(onClick);
        this.domList.append(domItem);
    }

    show(container: JQuery) {
        container.append(this.domList);
    }
}