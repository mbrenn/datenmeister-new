
import DMI = require("datenmeister-interfaces");
import DMView = require("datenmeister-view");
import DMTables = require("datenmeister-tables");


export enum PageType {
    Workspaces,
    Extents,
    Items,
    ItemDetail,
    Dialog
}

export class Layout implements DMI.Api.ILayout {
    parent: JQuery;
    onRefresh: () => void;
    currentPageType: PageType;

    constructor(parent: JQuery) {
        this.parent = parent;
    }

    refreshView() {
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
    
    navigateToItem(ws: string, extentUrl: string, itemUrl: string) {
        history.pushState({}, "", "#ws=" + encodeURIComponent(ws)
            + "&ext=" + encodeURIComponent(extentUrl)
            + "&item=" + encodeURIComponent(itemUrl));
        this.showItem(ws, extentUrl, itemUrl);
    }

    navigateToDialog(configuration: DMI.Api.FormForItemConfiguration) {
        var oldPageType = this.currentPageType;

        var domTable = $(".data-dialog", this.parent);
        var value = new DMI.DataTableItem();
        var tableConfiguration = new DMTables.ItemContentConfiguration();
        tableConfiguration.autoProperties = false;
        tableConfiguration.columns = configuration.columns;
        tableConfiguration.startWithEditMode = true;
        tableConfiguration.onCancelForm = () => {
            this.switchLayout(oldPageType);

            if (configuration.onCancelForm !== undefined) {
                configuration.onCancelForm();
            }
        };

        tableConfiguration.onOkForm = () => {  
            this.switchLayout(oldPageType);
            if (configuration.onOkForm !== undefined) {
                configuration.onOkForm(value);
            }
        }

        var itemTable = new DMTables.ItemContentTable(value, tableConfiguration);
        itemTable.show(domTable);
    }

    showWorkspaces() {
        var tthis = this;
        tthis.switchLayout(PageType.Workspaces);
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
        tthis.switchLayout(PageType.Extents);
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
        this.switchLayout(PageType.Items);
        this.createTitle(workspaceId, extentUrl);
        var extentLogic = new DMView.ExtentView(this);
        extentLogic.onItemSelected = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.onItemCreated = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.loadAndCreateHtmlForExtent($(".data-items", this.parent), workspaceId, extentUrl);
    }

    showItem(workspaceId: string, extentUrl: string, itemUrl: string) {
        this.switchLayout(PageType.ItemDetail);
        var extentLogic = new DMView.ItemView(this);

        this.createTitle(workspaceId, extentUrl, itemUrl);
        extentLogic.loadAndCreateHtmlForItem($(".data-itemdetail", this.parent), workspaceId, extentUrl, itemUrl);
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

    switchLayout(pageType: PageType) {
        $(".only-workspaces").hide();
        $(".only-extents").hide();
        $(".only-items").hide();
        $(".only-itemdetail").hide();

        if (pageType === PageType.Workspaces) {
            $(".only-workspaces").show();
        } else if (pageType === PageType.Extents) {
            $(".only-extents").show();
        } else if (pageType === PageType.Items) {
            $(".only-items").show();
        } else if (pageType === PageType.ItemDetail) {
            $(".only-itemdetail").show();
        } else if (pageType === PageType.Dialog) {
            $(".only-dialog").show();
        }

        this.currentPageType = pageType;
    }

    setStatus(statusDom: JQuery): void {
        var dom = $(".dm-statusline");
        dom.empty();
        dom.append(statusDom);
    }
}