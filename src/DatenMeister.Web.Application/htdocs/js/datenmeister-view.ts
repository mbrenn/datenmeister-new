import * as DMI from "./datenmeister-interfaces";
import * as DMTables from "./datenmeister-tables";
import * as DMClient from "./datenmeister-client";
import * as DMQuery from "./datenmeister-query";
import * as DMVP from "./datenmeister-viewport";

// This interface should be implemented by all views that can be added via 'setView' to a layout
export interface IView {
    viewport: DMVP.ViewPort;
    getContent(): JQuery;
    getLayoutInformation(): DMI.Api.ILayoutChangedEvent;
}

// Defines a base implementation of the IView interface
export class ViewBase implements IView{
    public viewport: DMVP.ViewPort;
    protected layout: DMI.Api.ILayout;
    protected content: JQuery;
    protected layoutInformation: DMI.Api.ILayoutChangedEvent;

    constructor(layout: DMI.Api.ILayout) {
        this.layout = layout;
        this.content = $("<div></div>");
    }

    getContent(): JQuery {
        return this.content;
    }

    getLayoutInformation(): DMI.Api.ILayoutChangedEvent {
        if (this.layoutInformation == null || this.layoutInformation == undefined) {
            throw "Layoutinformation is not set";
        }

        return this.layoutInformation;
    }

    setLayoutInformation(layoutInformation: DMI.Api.ILayoutChangedEvent): void {
        this.layoutInformation = layoutInformation;
    }

    insertLink(container: JQuery, displayText: string, onClick: () => void): JQuery {
        var domItem =
            $(`<input type='button' class='btn'></input>`);
        
        domItem.val(displayText);
        domItem.click(onClick);
        container.append(domItem);

        return domItem;
    }
}

export class WorkspaceView extends ViewBase implements IView {
    constructor(layout: DMI.Api.ILayout) {
        super(layout);
    }

    onWorkspaceSelected: (id: string) => void;

    loadAndCreateHtmlForWorkbenchs(): JQueryPromise<boolean> {
        var result = $.Deferred();
        var tthis = this;
        DMClient.WorkspaceApi.getAllWorkspaces()
            .done((data) => {
                tthis.createHtmlForWorkbenchs(data);
                result.resolve(true);
            });

        this.setLayoutInformation(
        {
            type: DMI.Api.PageType.Workspaces
        });

        return result;
    }

    createHtmlForWorkbenchs(data: Array<DMI.ClientResponse.IWorkspace>) {
        this.content.empty();
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

        this.content.append(compiledTable);

        this.insertLink(this.content,
            "Add new Workspace",
            () => this.layout.showDialogNewWorkspace());
    }
}

export class ExtentView extends ViewBase implements IView{
    constructor(layout: DMI.Api.ILayout) {
        super(layout);
    }

    onExtentSelected: (ws: string, extent: string) => void;
    onItemEdit: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemView: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemCreated: (ws: string, extentUrl: string, itemUrl: string) => void;

    loadAndCreateHtmlForWorkspace(ws: string): JQueryPromise<boolean> {
        var callback = $.Deferred();
        $.ajax(
        {
            url: `/api/datenmeister/extent/all?ws=${encodeURIComponent(ws)}`,
            cache: false,
            success: (data: Array<DMI.ClientResponse.IExtent>) => {
                this.createHtmlForWorkspace(ws, data);
                callback.resolve(true);
            },
            error: data => {
                callback.reject(false);
            }
        });

        this.setLayoutInformation({
            type: DMI.Api.PageType.Extents,
            workspace: ws
        });

        return callback;
    }

    createHtmlForWorkspace(ws: string, data: Array<DMI.ClientResponse.IExtent>) {
        var tthis = this;
        this.content.empty();

        if (data.length === 0) {
            this.content.html("<p>No extents were found</p>");
        } else {
            var compiledTable = $($("#template_extent_table").html());
            var compiled = _.template($("#template_extent").html());
            for (var n in data) {
                if (data.hasOwnProperty(n)) {
                    var entry: DMI.ClientResponse.IExtent = data[n];
                    var line: string = compiled(entry);
                    var dom: JQuery = $(line);
                    $(".data", dom)
                        .click(
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

            this.content.append(compiledTable);
        }
        
        this.insertLink(this.content,
            "Add new Extent",
            () => tthis.layout.showNavigationForNewExtents(ws));
    }

    loadAndCreateHtmlForExtent(
        ws: string,
        extentUrl: string,
        query?: DMI.PostModels.IItemTableQuery): JQueryPromise<Object> {
        var tthis = this;

        // Creates the layout configuration and the handling on requests of the user
        var configuration: DMTables.ItemListTableConfiguration = new DMTables.ItemListTableConfiguration();
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
        var provider: DMQuery.ItemsFromExtentProvider = new DMQuery.ItemsFromExtentProvider(ws, extentUrl);
        var table = new DMTables.ItemListTable(this.content, provider, configuration);
        
        if (query !== undefined && query !== null) {
            table.currentQuery = query;
        }

        configuration.onViewChanged = (viewUrl) => {
            query.view = viewUrl;
            tthis.loadAndCreateHtmlForExtent(ws, extentUrl, query);
        };
        
        DMClient.ExtentApi.getCreatableTypes(ws, extentUrl).done(
            (data) => {

                configuration.onNewItemClicked = (metaclass) => {
                    var view = new NavigationView(tthis.layout);
                    for (let typeKey in data.types) {
                        var type = data.types[typeKey];
                        view.addLink(type.name, () => alert(type.name));
                    }

                    tthis.viewport.setView(view);
                    /*DMClient.ExtentApi.createItem(ws, extentUrl, undefined, metaclass)
                        .done((innerData: DMI.ClientResponse.ICreateItemResult) => {
                            this.onItemCreated(ws, extentUrl, innerData.newuri);
                        });*/
                };
                table.setCreatableTypes(data.types);
            });

        DMClient.ExtentApi.getViews(ws, extentUrl)
            .done(
                (data) => {
                    table.setViews(data.views);
                });

        this.setLayoutInformation(
            {
                type: DMI.Api.PageType.Items,
                workspace: ws,
                extent: extentUrl
            });

        return table.loadAndShow();
    }
}

export class ItemView extends ViewBase implements IView
{
    onItemView: (ws: string, extentUrl: string, itemUrl: string) => void;

    constructor(layout: DMI.Api.ILayout) {
        super(layout);
    }

    loadAndCreateHtmlForItem(ws: string, extentUrl: string, itemUrl: string, settings?: DMI.View.IItemViewSettings): JQueryDeferred<Object> {
        var tthis = this;

        this.setLayoutInformation({
            type: DMI.Api.PageType.ItemDetail,
            workspace: ws,
            extent: extentUrl,
            item: itemUrl
        });

        return DMClient.ItemApi.getItem(ws, extentUrl, itemUrl)
            .done((data) => {
                tthis.createHtmlForItem(ws, extentUrl, itemUrl, data, settings);
            });
    }

    createHtmlForItem(ws: string, extentUrl: string, itemUrl: string, data: DMI.ClientResponse.IItemContentModel, settings?: DMI.View.IItemViewSettings) {
        var tthis = this;
        this.content.empty();
        var configuration = new DMTables.ItemContentConfiguration();
        configuration.columns = data.c.fields;
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
            var domMetaClassLink = $("<span><a href='#'>3</a> (<span class='fullname'></span>)</span>");
            $("a", domMetaClassLink).text(data.metaclass.name);
            $(".fullname", domMetaClassLink).text(data.metaclass.fullname);

            $("a", domMetaClassLink).click(() => {
                tthis.layout.navigateToItem(
                    data.metaclass.ws,
                    data.metaclass.ext,
                    data.metaclass.uri
                );

                return false;
            });

            $(".dm-tablecell-metaclass", domTableInfo).append(domMetaClassLink);
            $(".dm-tablecell-metaclass", domTableInfo).attr("title", data.metaclass.uri);
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

        this.content.append(domTableOwner);
        this.content.append(domTableInfo);
    }
}

// This class gives a navigation view with some links which can be clicked by the user and
// a user-defined action is being performed
export class NavigationView extends ViewBase implements IView {

    private domList : JQuery;
    constructor(layout: DMI.Api.ILayout) {
        super(layout);
        var domList = $("<ul class='dm-navigation-list'></ul>");
        this.domList = domList;
        this.content.append(this.domList);
    }

    /**
     * Adds a link to the view 
     * @param displayText Text to be shown
     * @param onClick The function that is called when the user clicks
     */
    addLink(displayText: string, onClick: () => void): JQuery {
        return this.insertLink(this.domList, displayText, onClick);
    }
}

export class DialogView extends ViewBase implements IView {
    constructor(layout: DMI.Api.ILayout) {
        super(layout);
    }

    createDialog(configuration: DMI.Api.DialogConfiguration) {
        var value = new DMI.ClientResponse.ItemContentModel();
        var tableConfiguration = new DMTables.ItemContentConfiguration();
        tableConfiguration.autoProperties = false;
        tableConfiguration.columns = configuration.columns;
        tableConfiguration.isReadOnly = false;
        tableConfiguration.supportNewProperties = false;
        tableConfiguration.onCancelForm = () => {
            if (configuration.onCancelForm !== undefined) {
                configuration.onCancelForm();
            }
        };

        tableConfiguration.onOkForm = () => {
            if (configuration.onOkForm !== undefined) {
                configuration.onOkForm(value);
            }
        }

        var itemTable = new DMTables.ItemContentTable(value, tableConfiguration);
        itemTable.show(this.content);

        this.setLayoutInformation({
                type: DMI.Api.PageType.Dialog,
                workspace: configuration.ws,
                extent: configuration.ext
            });
    }
}