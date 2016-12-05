import * as DMI from "./datenmeister-interfaces";
import * as DMN from "./datenmeister-navigation";
import * as DMTables from "./datenmeister-tables";
import * as DMClient from "./datenmeister-client";
import * as DMQuery from "./datenmeister-query";
import * as DMVP from "./datenmeister-viewport";
import * as DMDialog from "./datenmeister-dialogs";
import * as DMToolbar from "./datenmeister-toolbar"


// Defines a base implementation of the IView interface
export class ViewBase implements DMVP.IView{
    public viewport: DMVP.ViewPort;
    protected navigation: DMN.INavigation;
    protected content: JQuery;
    protected layoutInformation: DMI.Api.ILayoutChangedEvent;

    protected toolbar: DMToolbar.Toolbar;

    constructor(navigation: DMN.INavigation) {
        this.navigation = navigation;
        this.content = $("<div></div>");
    }

    getContent(): JQuery {
        return this.content;
    }

    getLayoutInformation(): DMI.Api.ILayoutChangedEvent {
        if (this.layoutInformation == null || this.layoutInformation == undefined) {
            return null;
        }

        return this.layoutInformation;
    }

    setLayoutInformation(layoutInformation: DMI.Api.ILayoutChangedEvent): void {
        this.layoutInformation = layoutInformation;
    }

    addButtonLink(displayText: string, onClick: () => void): JQuery {
        var domItem =
            $(`<input type='button' class='btn'></input>`);
        
        domItem.val(displayText);
        domItem.click(onClick);
        this.content.append(domItem);

        return domItem;
    }

    addText(text:string): JQuery {
        var result = $("<div></div>");
        result.text(text);
        this.content.append(text);

        return result;
    }

    addEmptyDiv(): JQuery {
        var result = $("<div></div>");
        this.content.append(result);
        return result;
    }

    addToolbar(): DMToolbar.Toolbar {
        return new DMToolbar.Toolbar(this.content);
    }
}

export class ListView extends ViewBase {

    onItemEdit: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemView: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemCreated: (ws: string, extentUrl: string, itemUrl: string) => void;

    constructor(navigation: DMN.INavigation) {
        super(navigation);
    }
}

export class WorkspaceView extends ViewBase implements DMVP.IView {
    constructor(navigation: DMN.INavigation) {
        super(navigation);
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

        this.addButtonLink(
            "Add new Workspace",
            () => DMDialog.showDialogNewWorkspace(this.navigation));
    }
}

export class ExtentView extends ListView implements DMVP.IView {
    constructor(navigation: DMN.INavigation) {
        super(navigation);
    }

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
                                    if (tthis.onItemView !== undefined) {
                                        tthis.onItemView(ws, localEntry.uri, null);
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

        this.addButtonLink(
            "Add new Extent",
            () => DMDialog.showNavigationForNewExtents(tthis.navigation, ws));
    }
}

export class ItemsOfExtentView extends ListView implements DMVP.IView {
    onNewItemClicked: (typeUrl?: string) => void;
    onViewChanged: (typeUrl?: string) => void;
    onPageChange: (newPage: number) => void;

    supportSearchbox: boolean;

    /* true, if new properties shall be supported */
    supportNewItem: boolean;
    supportViews: boolean;
    supportPaging: boolean;

    constructor(navigation: DMN.INavigation) {
        super(navigation); this.supportSearchbox = true;
        this.supportNewItem = true;
        this.supportPaging = true;
        this.supportViews = true;
    }

    loadAndCreateHtmlForExtent(
        ws: string,
        extentUrl: string,
        query?: DMI.Api.IItemTableQuery) : void{

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

        configuration.navigation = this.navigation;

        // Creates the provider
        var provider: DMQuery.ItemsFromExtentProvider = new DMQuery.ItemsFromExtentProvider(ws, extentUrl);

        // Creates the table
        var toolbar = this.addToolbar();
        var container = this.addEmptyDiv();

        var table = new DMTables.ItemListTable(container, provider, configuration);
        
        if (query !== undefined && query !== null) {
            table.currentQuery = query;
        }

        this.onViewChanged = (viewUrl) => {
            query.view = viewUrl;
            tthis.loadAndCreateHtmlForExtent(ws, extentUrl, query);
        };

        this.setLayoutInformation(
            {
                type: DMI.Api.PageType.Items,
                workspace: ws,
                extent: extentUrl
            });

        if (this.supportNewItem) {
            var itemNew = new DMToolbar.ToolbarButton("newitem", "Create Item");
            itemNew.onClicked = () => {

                var view = new CreatetableTypesView(this.navigation, ws, extentUrl);
                this.navigation.navigateToView(view);
            };

            toolbar.addItem(itemNew);
        }


        // Adds the searchbox and connects it to the tables
        if (this.supportSearchbox) {
            var itemSearch = new DMToolbar.ToolbarSearchbox();
            itemSearch.onSearch = (searchText: string) => {
                table.currentQuery.searchString = searchText;
                table.reload();
            };

            toolbar.addItem(itemSearch);
        }

        if (this.supportViews) {
            var itemView = new DMToolbar.ToolbarViewSelection(ws, extentUrl);
            itemView.onViewChanged = viewUrl => {
                table.currentQuery.view = viewUrl;
                table.reload();
            };
            toolbar.addItem(itemView);
        }

        if (this.supportPaging) {
            var itemPaging = new DMToolbar.ToolbarPaging();
            itemPaging.onPageChange = page => {
                table.currentQuery.offset = (page - 1) * table.configuration.itemsPerPage;
                table.reload();
            };

            table.configuration.paging = itemPaging;
            toolbar.addItem(itemPaging);
        }

        table.loadAndShow();
    }
}

export class ItemView extends ViewBase implements DMVP.IView
{
    onItemView: (ws: string, extentUrl: string, itemUrl: string) => void;

    constructor(navigation: DMN.INavigation) {
        super(navigation);
    }

    loadAndCreateHtmlForItem(ws: string, extentUrl: string, itemUrl: string, settings?: DMN.Settings.IItemViewSettings): JQueryDeferred<Object> {
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

    createHtmlForItem(ws: string, extentUrl: string, itemUrl: string, data: DMI.ClientResponse.IItemContentModel, settings?: DMN.Settings.IItemViewSettings) {
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
                tthis.navigation.navigateToItems(ws, extentUrl);
            }

            configuration.onCancelForm = () => {
                tthis.navigation.navigateToItems(ws, extentUrl);
            }
        } else {
            configuration.onOkForm = () => {
                DMClient.ItemApi.setProperties(ws, extentUrl, itemUrl, table.item)
                    .done(() => {
                        tthis.navigation.navigateToItems(ws, extentUrl);
                    });
            };

            configuration.onCancelForm = () => {
                tthis.navigation.navigateToItems(ws, extentUrl);
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
                tthis.navigation.navigateToItem(
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
export class EmptyView extends ViewBase implements DMVP.IView {

    constructor(navigation: DMN.INavigation) {
        super(navigation);
    }

    /**
     * Adds a link to the view 
     * @param displayText Text to be shown
     * @param onClick The function that is called when the user clicks
     */
    addLink(displayText: string, onClick: () => void): JQuery {
        return this.addButtonLink(displayText, onClick);
    }
}

export class DialogView extends ViewBase implements DMVP.IView {
    constructor(navigation: DMN.INavigation) {
        super(navigation);
    }

    createDialog(configuration: DMN.DialogConfiguration) {
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

export class CreatetableTypesView extends ViewBase implements DMVP.IView {
    private extentUrl: string;
    private ws: string;

    constructor(navigation: DMN.INavigation, ws: string, extentUrl: string) {
        super(navigation);
        this.extentUrl = extentUrl;
        this.ws = ws;
        var tthis = this;

        this.addButtonLink("Unspecified",
            () => {
                DMClient.ExtentApi.createItem(ws, extentUrl, null)
                    .done((innerData: DMI.ClientResponse.ICreateItemResult) => {
                        navigation.navigateToItem(ws, extentUrl, innerData.newuri);
                    });
            });

        
        DMClient.ExtentApi.getCreatableTypes(this.ws, this.extentUrl).done(
            (data) => {

                for (let typeKey in data.types) {
                    var func = (x: string) => {
                        var type = data.types[typeKey];
                        tthis.addButtonLink(
                            type.name,
                            () => {
                                DMClient.ExtentApi.createItem(ws, extentUrl, type.uri)
                                    .done((innerData: DMI.ClientResponse.ICreateItemResult) => {
                                        navigation.navigateToItem(ws, extentUrl, innerData.newuri);
                                    });
                            });
                    }

                    func(typeKey);
                }
            });
    }
}