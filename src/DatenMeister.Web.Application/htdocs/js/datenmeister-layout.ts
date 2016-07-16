
import DMI = require("./datenmeister-interfaces");
import DMView = require("./datenmeister-view");
import DMTables = require("./datenmeister-tables");


export enum PageType {
    Workspaces,
    Extents,
    Items,
    ItemDetail,
    Dialog
}

export interface ILayoutChangedEvent {
    type: PageType;
    workspace?: string;
    extent?: string;
    item?: string;
}

export class Layout implements DMI.Api.ILayout {
    parent: JQuery;
    onRefresh: () => void;
    onLayoutChanged: (data: ILayoutChangedEvent) => void;
    currentLayoutInformation: ILayoutChangedEvent;

    constructor(parent: JQuery) {
        this.parent = parent;
    }

    refreshView() : void {
        if (this.onRefresh !== undefined && this.onRefresh !== null) {
            this.onRefresh();
        }
    }

    navigateToWorkspaces() {
        history.pushState({}, "", "#");
        this.showWorkspaces();
    }

    navigateToExtents(workspaceId: string) {
        history.pushState({}, "", "#ws=" + encodeURIComponent(workspaceId));
        this.showExtents(workspaceId);
    }

    navigateToItems(ws: string, extentUrl: string) {
        history.pushState({}, "", "#ws=" + encodeURIComponent(ws)
            + "&ext=" + encodeURIComponent(extentUrl));
        this.showItems(ws, extentUrl);
    }

    navigateToItem(ws: string, extentUrl: string, itemUrl: string, settings?: DMI.View.IItemViewSettings) {
        var url = "#ws=" + encodeURIComponent(ws)
            + "&ext=" + encodeURIComponent(extentUrl)
            + "&item=" + encodeURIComponent(itemUrl);
        
        if (settings !== undefined && settings !== null) {
            if (settings.isReadonly) {
                url += "&mode=readonly";
            }
        }

        history.pushState({}, "", url);
        this.showItem(ws, extentUrl, itemUrl, settings);
    }

    exportExtent(ws: string, extentUrl: string) {
        window.open(
            "/api/datenmeister/extent/extent_export?ws="
            + encodeURIComponent(ws) + "&extent="
            + encodeURIComponent(extentUrl));
    }

    navigateToDialog(configuration: DMI.Api.DialogConfiguration) {
        var oldPageType = this.currentLayoutInformation;

        var domTable = $(".data-dialog", this.parent);
        var value = new DMI.Table.DataTableItem();
        var tableConfiguration = new DMTables.ItemContentConfiguration();
        tableConfiguration.autoProperties = false;
        tableConfiguration.columns = configuration.columns;
        tableConfiguration.isReadOnly = false;
        tableConfiguration.supportNewProperties = false;
        tableConfiguration.onCancelForm = () => {
            this.switchLayout(oldPageType);

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
        itemTable.show(domTable);

        this.switchLayout(
        {
            type: PageType.Dialog,
            workspace: configuration.ws,
            extent: configuration.extent
        });
    }

    showWorkspaces() {
        var tthis = this;
        tthis.switchLayout({ type: PageType.Workspaces });
        tthis.createTitle();

        var workbenchLogic = new DMView.WorkspaceView();
        workbenchLogic.onWorkspaceSelected = (id: string) => {
            // Loads the extent of the workspace, if the user has clicked on one of the workbenches
            tthis.navigateToExtents(id);
        };

        workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".data-workspaces", this.parent));
    }

    showExtents(workspaceId: string) {
        var tthis = this;
        tthis.switchLayout({
            type: PageType.Extents,
            workspace: workspaceId
        });
        tthis.createTitle(workspaceId);
        var extentLogic = new DMView.ExtentView();
        extentLogic.onExtentSelected = (ws: string, extentUrl: string) => {
            tthis.navigateToItems(ws, extentUrl);
            return false;
        };

        extentLogic.loadAndCreateHtmlForWorkspace($(".data-extents", this.parent), workspaceId);
    }

    showItems(workspaceId: string, extentUrl: string) {
        var tthis = this;
        this.switchLayout(
        {
            type: PageType.Items,
            workspace: workspaceId,
            extent: extentUrl
            });

        this.createTitle(workspaceId, extentUrl);
        var extentLogic = new DMView.ExtentView(this);
        extentLogic.onItemEdit = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl, { isReadonly: true });
        };

        extentLogic.onItemCreated = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.loadAndCreateHtmlForExtent($(".data-items", this.parent), workspaceId, extentUrl);
    }

    showItem(workspaceId: string, extentUrl: string, itemUrl: string, settings?: DMI.View.IItemViewSettings) {
        var tthis = this;
        this.switchLayout({
            type: PageType.ItemDetail,
            workspace: workspaceId,
            extent: extentUrl,
            item: itemUrl
        });

        var extentLogic = new DMView.ItemView(this);

        extentLogic.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl, { isReadonly: true });
        };

        this.createTitle(workspaceId, extentUrl, itemUrl);
        extentLogic.loadAndCreateHtmlForItem($(".data-itemdetail", this.parent), workspaceId, extentUrl, itemUrl, settings);
    }

    createTitle(ws?: string, extentUrl?: string, itemUrl?: string) {
        var tthis = this;
        var containerTitle = $(".container-title", this.parent);

        if (ws === undefined) {
            containerTitle.text("Workspaces");
            this.onRefresh = () => {
                tthis.showWorkspaces();
                return false;
            };
        } else if (extentUrl === undefined) {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - Extents");
            this.onRefresh = () => {
                tthis.showExtents(ws);
                return false;
            };
        } else if (itemUrl == undefined) {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - Items");
            this.onRefresh = () => {
                tthis.showItems(ws, extentUrl);
                return false;
            };
        } else {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - <a href='#' class='link_items'>Items</a>");
            this.onRefresh = () => {
                tthis.showItem(ws, extentUrl, itemUrl);
                return false;
            };
        }

        $(".link_workspaces", containerTitle).click(() => {
            tthis.navigateToWorkspaces();
            return false;
        });
        $(".link_extents", containerTitle).click(() => {
            tthis.navigateToExtents(ws);
            return false;
        });
        $(".link_items", containerTitle).click(() => {
            tthis.showItems(ws, extentUrl);
            return false;
        });
    }

    switchLayout(layoutInformation: ILayoutChangedEvent) {
        $(".only-workspaces").hide();
        $(".only-extents").hide();
        $(".only-items").hide();
        $(".only-itemdetail").hide();
        $(".only-dialog").hide();

        if (layoutInformation.type === PageType.Workspaces) {
            $(".only-workspaces").show();
        } else if (layoutInformation.type === PageType.Extents) {
            $(".only-extents").show();
        } else if (layoutInformation.type === PageType.Items) {
            $(".only-items").show();
        } else if (layoutInformation.type === PageType.ItemDetail) {
            $(".only-itemdetail").show();
        } else if (layoutInformation.type === PageType.Dialog) {
            $(".only-dialog").show();
        }

        this.currentLayoutInformation = layoutInformation;

        this.throwLayoutChangedEvent(layoutInformation);
    }

    setStatus(statusDom: JQuery): void {
        var dom = $(".dm-statusline");
        dom.empty();
        dom.append(statusDom);
    }

    setView(view: DMView.IView) {
        this.switchLayout(
        {
            type: PageType.Dialog
        });

        var container = $(".data-dialog", this.parent);
        container.empty();
        view.show(container);
    }

    throwLayoutChangedEvent(data: ILayoutChangedEvent) {
        if (this.onLayoutChanged !== undefined) {
            this.onLayoutChanged(data);
        }
    }

    gotoHome() {
        this.navigateToWorkspaces();
    }
}