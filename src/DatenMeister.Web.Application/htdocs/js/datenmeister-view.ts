///<reference path="../../node_modules/@types/underscore/index.d.ts"/>
import * as DMI from "./datenmeister-interfaces";
import * as DMCI from "./datenmeister-clientinterface";
import * as DMVM from "./datenmeister-viewmodels"
import * as DMTables from "./datenmeister-tables";
import * as DMClient from "./datenmeister-client";
import * as DMQuery from "./datenmeister-query";
import * as DMDialog from "./datenmeister-dialogs";
import * as DMToolbar from "./datenmeister-toolbar";
import IView = DMI.Views.IView;
import IViewPort = DMI.Views.IViewPort;
declare var _: _.UnderscoreStatic;

// Defines a base implementation of the IView interface
export class ViewBase implements DMI.Views.IView{
    public viewport: DMI.Views.IViewPort;
    protected content: JQuery;
    protected viewState: DMI.Api.IViewState;

    protected toolbar: DMToolbar.Toolbar;

    constructor(viewport: DMI.Views.IViewPort) {
        this.viewport = viewport;
        this.content = $("<div></div>");
    }

    emptyContent(): void {
        this.content.empty();
    }

    getViewState(): DMI.Api.IViewState {
        if (this.viewState == null || this.viewState == undefined) {
            return null;
        }

        return this.viewState;
    }

    setViewState(layoutInformation: DMI.Api.IViewState): void {
        this.viewState = layoutInformation;
    }

    addContent(domContent: JQuery): void {
        this.content.append(domContent);

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

    refresh(): void {
        this.content.empty();
    }

    load(): JQuery {
        this.content = $("<div></div>");
        this.refresh();
        return this.content;
    }

    /**
     * This event will be called after the dom has been added to the window
     */
    onViewShown(): void {
    }
}

export class ListView extends ViewBase {

    onItemEdit: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemView: (ws: string, extentUrl: string, itemUrl: string) => void;
    onItemCreated: (ws: string, extentUrl: string, itemUrl: string) => void;

    constructor(viewport: DMI.Views.IViewPort) {
        super(viewport);
    }
}

export namespace WorkspaceList {
    export function navigateToWorkspaces(viewport: IViewPort): IView {
        var view = new WorkspaceView(viewport);
        viewport.setView(view);
        return view;
    }

    export class WorkspaceView extends ViewBase implements DMI.Views.IView {
        constructor(viewport: DMI.Views.IViewPort) {
            super(viewport);
        }

        refresh(): void {
            var tthis = this;
            DMClient.WorkspaceApi.getAllWorkspaces()
                .done((data) => {
                    tthis.createHtmlForWorkbenchs(data);
                });

            this.setViewState(
                {
                    type: DMI.Api.PageType.Workspaces
                });
        }

        createHtmlForWorkbenchs(data: Array<DMCI.In.IWorkspace>) {
            var tthis = this;
            var fields = [
                new DMTables.Fields.TextboxField("id", "Name").readOnly(),
                new DMTables.Fields.TextboxField("annotation", "Annotation").readOnly()
            ];

            var configuration = new DMTables.ListTableConfiguration();
            configuration.fields = fields;

            DMTables.Fields.addEditButton(
                configuration,
                (item: DMCI.In.IWorkspace) => {
                    var workspaceId = item.id;
                    ExtentList.navigateToExtents(tthis.viewport, workspaceId);
                    return false;
                });

            DMTables.Fields.addDeleteButton(
                configuration,
                (item: DMCI.In.IWorkspace) => {
                    var workspaceId = item.id;
                    DMClient.WorkspaceApi.deleteWorkspace(workspaceId).done(
                        () => WorkspaceList.navigateToWorkspaces(tthis.viewport));
                    return false;
                });

            var table = new DMTables.ListTableComposer(configuration, this.content);
            table.composeTable(data);


            this.addButtonLink(
                "Add new Workspace",
                () => DMDialog.showDialogNewWorkspace(this.viewport));
        }
    }

}

export namespace ExtentList {

    export function navigateToExtents(viewport: IViewPort, workspaceId: string): DMI.Views.IView {
        var view = new ExtentView(viewport, workspaceId);
        viewport.setView(view);
        return view;
    }

    export class ExtentView extends ListView implements DMI.Views.IView {
        ws: string; 

        constructor(viewport: DMI.Views.IViewPort, ws: string) {
            super(viewport);
            this.ws = ws;
        }

        refresh(): void {
            var tthis = this;
            // TODO tthis.createTitle(workspaceId);
            this.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
                ItemList.navigateToItems(this.viewport, ws, extentUrl);
                return false;
            };

            DMClient.ExtentApi.getExtents(this.ws)
                .done((data: Array<DMCI.In.IExtent>) => {
                    tthis.createHtmlForWorkspace(data);
                });

            this.setViewState({
                type: DMI.Api.PageType.Extents,
                workspace: this.ws
            });
        }

        createHtmlForWorkspace(data: Array<DMCI.In.IExtent>) {
            var tthis = this;
            var fields = [
                new DMTables.Fields.TextboxField("uri", "Uri").readOnly(),
                new DMTables.Fields.TextboxField("count", "Items").readOnly(),
                new DMTables.Fields.TextboxField("dataLayer", "Layer").readOnly(),
                new DMTables.Fields.TextboxField("type", "Type").readOnly()
            ];

            var configuration = new DMTables.ListTableConfiguration();
            configuration.fields = fields;

            DMTables.Fields.addEditButton(
                configuration,
                (item: DMCI.In.IExtent) => {
                    ItemList.navigateToItems(tthis.viewport, tthis.ws, item.uri);
                });

            DMTables.Fields.addDeleteButton(
                configuration,
                (item: DMCI.In.IExtent) => {
                    DMClient.ExtentApi.deleteExtent(
                            tthis.ws,
                            item.uri)
                        .done(() => tthis.refresh());
                });

            var table = new DMTables.ListTableComposer(configuration, this.content);
            table.composeTable(data);

            this.addButtonLink(
                "Add new Extent",
                () => DMDialog.showNavigationForNewExtents(tthis.viewport, tthis.ws));
        }
    }
}

export namespace ItemList {
    export function navigateToItems(viewport: IViewPort, ws: string, extentUrl: string, viewname ?: string): ItemsOfExtentView {
        var view = new ItemsOfExtentView(viewport, ws, extentUrl);
        viewport.setView(view);
        return view;
    }

    export function exportExtent(viewport: IViewPort, ws: string, extentUrl: string) {
        window.open(
            `/api/datenmeister/extent/extent_export_csv?ws=${encodeURIComponent(ws)}&extent=${encodeURIComponent(extentUrl)}`);
    }

    export class ItemsOfExtentView extends ListView implements DMI.Views.IView {
        ws: string;
        extentUrl: string;
        query: DMCI.Out.IItemTableQuery;

        onNewItemClicked: (typeUrl?: string) => void;
        onViewChanged: (typeUrl?: string) => void;
        onPageChange: (newPage: number) => void;

        supportSearchbox: boolean;

        /* true, if new properties shall be supported */
        supportNewItem: boolean;
        supportViews: boolean;
        supportPaging: boolean;
        supportMetaClasses: boolean;
        toolbarMetaClasses: DMToolbar.ToolbarMetaClasses;

        table: DMTables.ListTableComposer;

        constructor(viewport: DMI.Views.IViewPort,
            ws: string,
            extentUrl: string,
            query?: DMCI.Out.IItemTableQuery) {
            super(viewport);
            this.supportSearchbox = true;
            this.supportNewItem = true;
            this.supportPaging = true;
            this.supportViews = true;
            this.supportMetaClasses = true;
            this.ws = ws;
            this.extentUrl = extentUrl;
            this.query = query;
            if (this.query === undefined || this.query === null) {
                this.query = new DMCI.Out.ItemTableQuery();
                this.query.amount = 20;
            }
        }

        refresh(): void {
            this.setViewState({
                type: DMI.Api.PageType.Items,
                workspace: this.ws,
                extent: this.extentUrl
            });

            var tthis = this;

            // Creates the layout configuration and the handling on requests of the user

            /*
            configuration.onItemEdit = (url: string) => {
                if (tthis.onItemEdit !== undefined) {
                    tthis.onItemEdit(tthis.ws, tthis.extentUrl, url);
                }

                return false;
            };
            configuration.onItemSelect = (url: string) => {
                if (tthis.onItemView !== undefined) {
                    tthis.onItemView(tthis.ws, tthis.extentUrl, url);
                }

                return false;
            };

            configuration.onItemDelete = (url: string, domRow: JQuery) => {
                var callback = DMClient.ExtentApi.deleteItem(tthis.ws, tthis.extentUrl, url);
                callback
                    .done(() => {
                        domRow.find("td").fadeOut(500, () => { domRow.remove(); });
                    })
                    .fail(() => { alert("FAILED"); });
                return false;
            };*/

            // Creates the provider
            var provider: DMQuery.ItemsFromExtentProvider = new DMQuery.ItemsFromExtentProvider(this.ws, this.extentUrl);
            this.composeForProvider(provider);

            // Creates the table
            this.toolbar = this.addToolbar();
            var container = this.addEmptyDiv();

            this.table = new DMTables.ListTableComposer(new DMTables.ListTableConfiguration(), container);

            /*if (query !== undefined && query !== null) {
                table.currentQuery = query;
            }*/

            /*this.onViewChanged = (viewUrl) => {
                query.view = viewUrl;
                tthis.load(ws, extentUrl, query);
            };*/

            
            if (this.supportNewItem) {
                var itemNew = new DMToolbar.ToolBarButtonItem("newitem", "Create Item");
                itemNew.onClicked = () => {

                    var view = new CreatetableTypesView(this.viewport, tthis.ws, tthis.extentUrl);
                    this.viewport.setView(view);
                };

                this.toolbar.addItem(itemNew);
            }

            /*

            // Adds the searchbox and connects it to the tables
            if (this.supportSearchbox) {
                var itemSearch = new DMToolbar.ToolbarSearchbox();
                itemSearch.onSearch = (searchText: string) => {
                    table.currentQuery.searchString = searchText;
                    table.reload();
                };

                this.toolbar.addItem(itemSearch);
            }

            if (this.supportViews) {
                var itemView = new DMToolbar.ToolbarViewSelection(ws, extentUrl);
                itemView.onViewChanged = viewUrl => {
                    table.currentQuery.view = viewUrl;
                    table.reload();
                };
                this.toolbar.addItem(itemView);
            }

            if (this.supportMetaClasses) {
                this.toolbarMetaClasses = new DMToolbar.ToolbarMetaClasses(ws, extentUrl);
                this.toolbarMetaClasses.onItemClicked = viewUrl => {
                    alert('X');
                    table.reload();
                };
                this.toolbar.addItem(this.toolbarMetaClasses);

                table.onDataReceived.addListener((data) => {
                    tthis.toolbarMetaClasses.updateLayout(data.metaClasses);
                });
            }


            if (this.supportPaging) {
                var itemPaging = new DMToolbar.ToolbarPaging();
                itemPaging.onPageChange = page => {
                    table.currentQuery.offset = (page - 1) * table.configuration.itemsPerPage;
                    table.reload();
                };

                table.configuration.paging = itemPaging;
                this.toolbar.addItem(itemPaging);
            }
            */
        }

        composeForProvider(provider: DMI.Api.IItemsProvider) {

            provider.performQuery(this.query).done((items: DMCI.In.IItemsContent) => {

                var configuration: DMTables.ListTableConfiguration =
                    new DMTables.ListTableConfiguration();
                for (var c in items.columns.fields) {

                    var column = items.columns.fields[c];
                    configuration.addField(
                        new DMTables.Fields.TextboxField(column.name, column.title));

                }

                this.table.configuration = configuration;
                this.table.items = items.items;
                this.table.composeContent();
            });
        }

        /*
        showItems(viewport: IViewPort, workspaceId: string, extentUrl: string, viewname?: string) {
            var tthis = this;
            // TODO: this.viewport.createTitle(workspaceId, extentUrl);

            
            var extentView = new ItemsOfExtentView(this.viewport, workspaceId, extentUrl, query);
            extentView.onItemEdit = (ws: string, extentUrl: string, itemUrl: string) => {
                ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, itemUrl);
            };

            extentView.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
                ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, itemUrl, undefined, { isReadonly: true });
            };

            extentView.onItemCreated = (ws: string, extentUrl: string, itemUrl: string) => {
                ItemDetail. navigateToItem(tthis.viewport, ws, extentUrl, itemUrl);
            };
        }*/
    }
}

export namespace ItemDetail {
    export function navigateToItem(
        viewport: IViewPort,
        ws: string,
        extentUrl: string,
        itemUrl: string,
        viewname ?: string,
        settings?: DMI.Navigation.IItemViewSettings): void {

        var itemView = new ItemView(this, ws, extentUrl, itemUrl, settings);
        itemView.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            navigateToItem(viewport, ws, extentUrl, itemUrl, undefined, { isReadonly: true });
        };

        // TODO: this.createTitle(workspaceId, extentUrl, itemUrl);

        viewport.setView(itemView);
    }


    export class ItemView extends ViewBase implements DMI.Views.IView {
        onItemView: (ws: string, extentUrl: string, itemUrl: string) => void;
        supportViews: boolean;

        constructor(viewport: DMI.Views.IViewPort, ws: string, extentUrl: string, itemUrl: string, settings?: DMI.Navigation.IItemViewSettings) {
            super(viewport);
            this.supportViews = true;

            this.setViewState({
                type: DMI.Api.PageType.ItemDetail,
                workspace: ws,
                extent: extentUrl,
                item: itemUrl
            });
        }

        load(): JQuery {
            var tthis = this;
            /*
            DMClient.ItemApi.getItem(ws, extentUrl, itemUrl)
                .done((data) => {
                    tthis.createHtmlForItem(ws, extentUrl, itemUrl, data, settings);
                });*/

            throw "Not Implemented";

        }

        createHtmlForItem(ws: string, extentUrl: string, itemUrl: string, data: DMCI.In.IItemContentModel, settings?: DMI.Navigation.IItemViewSettings) {
            throw ("Not Implemented");
            /*
            var tthis = this;
            this.content.empty();
            var configuration = new DMTables.ItemContentConfiguration();
            configuration.columns = data.c.fields;
            var isReadonly = false;
            this.toolbar = this.addToolbar();

            if (settings === undefined) {
                settings = new DMI.Navigation.ItemViewSettings();
            }

            isReadonly = settings.isReadonly;

            configuration.isReadOnly = isReadonly;
            configuration.supportNewProperties = !isReadonly;

            var table = new DMTables.ItemContentTable(data, configuration);
            if (isReadonly) {
                configuration.onOkForm = () => {
                    ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                };
                configuration.onCancelForm = () => {
                    ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                };
            } else {
                configuration.onOkForm = () => {
                    DMClient.ItemApi.setProperties(ws, extentUrl, itemUrl, table.item)
                        .done(() => {
                            ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                        });
                };

                configuration.onCancelForm = () => {
                    ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                };
            }

            configuration.onItemSelect = (url: string) => {
                if (tthis.onItemView !== undefined) {
                    tthis.onItemView(ws, extentUrl, url);
                }

                return false;
            };

            if (this.supportViews) {
                var itemView = new DMToolbar.ToolbarViewSelection(ws, extentUrl, itemUrl);
                itemView.onViewChanged = viewUrl => {
                    navigateToItem(tthis.viewport, ws, extentUrl, itemUrl, viewUrl, settings);
                };

                this.toolbar.addItem(itemView);
            }

            configuration.onEditButton = () => {
                settings.isReadonly = false;
                navigateToItem(tthis.viewport, ws, extentUrl, itemUrl, null, settings);
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
                    navigateToItem(
                        tthis.viewport,
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
            */
        }
    }

}

export function navigateToDialog(viewport: IViewPort, configuration: DMI.Navigation.DialogConfiguration): void {
    var dialog = new DialogView(viewport, configuration);
    viewport.setView(dialog);
}



// This class gives a navigation view with some links which can be clicked by the user and
// a user-defined action is being performed
export class EmptyView extends ViewBase implements DMI.Views.IView {

    constructor(viewport: DMI.Views.IViewPort) {
        super(viewport);
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

export class DialogView extends ViewBase implements DMI.Views.IView {
    /**
     * Configuration fo the dialog
     */
    configuration: DMI.Navigation.DialogConfiguration;
    detailTable: DMTables.DetailTableComposer;

    constructor(viewport: DMI.Views.IViewPort, configuration: DMI.Navigation.DialogConfiguration) {
        super(viewport);
        this.configuration = configuration;
    }

    refresh(): void {
        var tthis = this;
        var tableConfiguration = new DMTables.DetailTableConfiguration();
        tableConfiguration.fields = DMTables.convertFieldDataToFields(this.configuration.columns);

        this.detailTable = new DMTables.DetailTableComposer(tableConfiguration, this.content);

        if (this.configuration.onOkForm !== undefined && this.configuration.onOkForm !== null) {
            this.detailTable.onClickOk = (newItem: any) => {
                tthis.configuration.onOkForm(newItem);
            }
        }

        if (this.configuration.onCancelForm !== undefined && this.configuration.onCancelForm !== null) {
            this.detailTable.onClickCancel = () => {
                tthis.configuration.onCancelForm();
                return false;
            }
        } else {
            this.detailTable.onClickCancel = () => {
                tthis.viewport.navigateBack();
                return false;
            }
        }

        this.detailTable.composeTable();
    }

    onViewShown(): void {
        this.detailTable.setFocusOnFirstRow();
    }
}

export class CreatetableTypesView extends ViewBase implements DMI.Views.IView {
    private extentUrl: string;
    private ws: string;
    
    constructor(viewport: DMI.Views.IViewPort, ws: string, extentUrl: string) {
        super(viewport);
        this.extentUrl = extentUrl;
        this.ws = ws;
        var tthis = this;

        // Adds the default one, which creates just an empty, non-defined item.
        this.addButtonLink("Unspecified",
            () => {
                DMClient.ExtentApi.createItem(ws, extentUrl, null)
                    .done((innerData: DMCI.In.ICreateItemResult) => {
                        ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, innerData.newuri);
                    });
            });

        this.addText("Loading...");
        
        // Adds the ones, that can be created
        DMClient.ExtentApi.getCreatableTypes(this.ws, this.extentUrl).done(
            (data) => {

                for (let typeKey in data.types) {
                    var func = (x: string) => {
                        var type = data.types[typeKey];
                        tthis.addButtonLink(
                            type.name,
                            () => {
                                DMClient.ExtentApi.createItem(ws, extentUrl, type.uri)
                                    .done((innerData: DMCI.In.ICreateItemResult) => {
                                        ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, innerData.newuri);
                                    });
                            });
                    }

                    func(typeKey);
                }
            });
    }
}
