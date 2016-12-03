
import DMI = require("./datenmeister-interfaces");
import DMN = require("./datenmeister-navigation");
import DMViewPort = require("./datenmeister-viewport");
import DMView = require("./datenmeister-view");
import * as DMDialog from "./datenmeister-dialogs";
import * as DMClient from "./datenmeister-client";
import * as DMRibbon from "./datenmeister-ribbon";

export class Layout implements DMI.Api.ILayout, DMN.INavigation {
    pluginResults: Array<DMI.Api.IPluginResult>;
    mainViewPort: DMViewPort.ViewPort;
    parent: JQuery;
    onRefresh: () => void;
    onViewPortChanged: (data: DMI.Api.ILayoutChangedEvent) => void;
    currentLayoutInformation: DMI.Api.ILayoutChangedEvent;
    ribbon: DMRibbon.Ribbon;

    constructor(parent: JQuery) {
        var tthis = this;
        this.parent = parent;
        this.pluginResults = new Array<DMI.Api.IPluginResult>();
        this.mainViewPort = new DMViewPort.ViewPort($("#dm-viewport", this.parent), this);
        this.mainViewPort.onViewPortChanged = data => {
            this.throwViewPortChanged(data);
            tthis.buildRibbons(data);
        };
    }

    refreshView(): void {
        if (this.onRefresh !== undefined && this.onRefresh !== null) {
            this.onRefresh();
        }
    }

    navigateToWorkspaces(): void {
        history.pushState({}, "", "#ws={all}");
        this.showWorkspaces();
    }

    navigateToView(view: DMViewPort.IView): void {
        this.mainViewPort.setView(view);
    }

    navigateToExtents(workspaceId: string): void {
        history.pushState({}, "", `#ws=${encodeURIComponent(workspaceId)}`);
        this.showExtents(workspaceId);
    }

    navigateToItems(ws: string, extentUrl: string, viewname?: string): void {
        var url = `#ws=${encodeURIComponent(ws)}&ext=${encodeURIComponent(extentUrl)}`;
        if (viewname !== undefined && viewname !== null) {
            url += `&view=${encodeURIComponent(viewname)}`;
        }

        history.pushState({}, "", url);

        this.showItems(ws, extentUrl, viewname);
    }

    navigateToItem(ws: string, extentUrl: string, itemUrl: string, viewname?: string, settings?: DMN.Settings.IItemViewSettings): void {
        var url = `#ws=${encodeURIComponent(ws)}&ext=${encodeURIComponent(extentUrl)}&item=${encodeURIComponent(itemUrl)}`;

        if (settings !== undefined && settings !== null) {
            if (settings.isReadonly) {
                url += "&mode=readonly";
            }
        }

        if (viewname !== undefined && viewname !== null) {
            url += `&view=${encodeURIComponent(viewname)}`;
        }

        history.pushState({}, "", url);
        this.showItem(ws, extentUrl, itemUrl, viewname, settings);
    }

    exportExtent(ws: string, extentUrl: string) {
        window.open(
            `/api/datenmeister/extent/extent_export_csv?ws=${encodeURIComponent(ws)}&extent=${encodeURIComponent(extentUrl)}`);
    }

    navigateToDialog(configuration: DMN.DialogConfiguration): void {
        var dialog = new DMView.DialogView(this);
        dialog.createDialog(configuration);
        this.mainViewPort.setView(dialog);
    }

    showWorkspaces() {
        var tthis = this;
        tthis.createTitle();

        var workbenchLogic = new DMView.WorkspaceView(this);
        workbenchLogic.onWorkspaceSelected = (id: string) => {
            // Loads the extent of the workspace, if the user has clicked on one of the workbenches
            tthis.navigateToExtents(id);
        };

        workbenchLogic.loadAndCreateHtmlForWorkbenchs();
        this.mainViewPort.setView(workbenchLogic);
    }

    showExtents(workspaceId: string) {
        var tthis = this;
        tthis.createTitle(workspaceId);
        var extentView = new DMView.ExtentView(this);
        extentView.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItems(ws, extentUrl);
            return false;
        };

        extentView.loadAndCreateHtmlForWorkspace(workspaceId);
        this.mainViewPort.setView(extentView);
    }

    showItems(workspaceId: string, extentUrl: string, viewname?: string) {
        var tthis = this;

        this.createTitle(workspaceId, extentUrl);
        var extentView = new DMView.ItemsOfExtentView(this);
        extentView.onItemEdit = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentView.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl, undefined, { isReadonly: true });
        };

        extentView.onItemCreated = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        var query = new DMI.Api.ItemInExtentQuery();
        query.view = viewname;
        query.amount = 20;

        extentView.loadAndCreateHtmlForExtent(workspaceId, extentUrl, query);
        this.mainViewPort.setView(extentView);
    }

    showItem(workspaceId: string,
        extentUrl: string,
        itemUrl: string,
        viewname?: string,
        settings?: DMN.Settings.IItemViewSettings) {
        var tthis = this;

        var itemView = new DMView.ItemView(this);

        itemView.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl, undefined, { isReadonly: true });
        };

        this.createTitle(workspaceId, extentUrl, itemUrl);
        itemView.loadAndCreateHtmlForItem(
            workspaceId,
            extentUrl,
            itemUrl,
            settings);
        this.mainViewPort.setView(itemView);
    }

    createTitle(ws?: string, extentUrl?: string, itemUrl?: string) {
        var tthis = this;
        var containerTitle = $(".container-title", this.parent);
        var ba = "&gt;&gt";
        if (ws === undefined) {
            containerTitle.text("Workspaces");
            this.onRefresh = () => {
                tthis.showWorkspaces();
                return false;
            };
        } else if (extentUrl === undefined) {
            containerTitle.html(
                `<a href='#' class='link_workspaces'>Workspace '<b>${ws}</b>'</a> ${ba} Extents`);
            this.onRefresh = () => {
                tthis.showExtents(ws);
                return false;
            };
        } else if (itemUrl == undefined) {
            containerTitle
                .html(
                    `<a href='#' class='link_workspaces'> Workspace '<b>${ws}</b>'</a> ${ba
                    } <a href='#' class='link_extents'>Extent '<b>${extentUrl}</b>'</a> ${ba
                    } Items`);
            this.onRefresh = () => {
                tthis.showItems(ws, extentUrl);
                return false;
            };
        } else {
            containerTitle
                .html(`<a href='#' class='link_workspaces'>Workspace '<b>${ws}</b>'</a> ${ba
                    } <a href='#' class='link_extents'>Extent '<b>${extentUrl}</b>'</a> ${ba
                    } <a href='#' class='link_items'>Items</a>`);
            this.onRefresh = () => {
                tthis.showItem(ws, extentUrl, itemUrl);
                return false;
            };
        }

        $(".link_workspaces", containerTitle)
            .click(() => {
                tthis.navigateToWorkspaces();
                return false;
            });
        $(".link_extents", containerTitle)
            .click(() => {
                tthis.navigateToExtents(ws);
                return false;
            });
        $(".link_items", containerTitle)
            .click(() => {
                tthis.showItems(ws, extentUrl);
                return false;
            });
    }

    setStatus(statusDom: JQuery): void {
        var dom = $(".dm-statusline");
        dom.empty();
        dom.append(statusDom);
    }

    lastLayoutConfiguration: DMI.Api.ILayoutChangedEvent;

    throwViewPortChanged(data: DMI.Api.ILayoutChangedEvent): void {
        if (data !== undefined && data != null) {
            data.layout = this;
            data.navigation = this;
        }

        this.lastLayoutConfiguration = data;

        if (this.onViewPortChanged !== undefined) {
            this.onViewPortChanged(data);
        }

        if (this.pluginResults !== undefined) {
            for (var n in this.pluginResults) {
                var pluginResult = this.pluginResults[n];
                pluginResult.onViewPortChanged(data);
            }
        }
    }

    renavigate(): void {
        this.throwViewPortChanged(this.lastLayoutConfiguration);
    }

    gotoHome(): void {
        this.navigateToExtents("Data");
    }

    getRibbon(): DMRibbon.Ribbon {
        if (this.ribbon === null || this.ribbon === undefined) {
            var domRibbon = $(".datenmeister-ribbon");
            this.ribbon = new DMRibbon.Ribbon(domRibbon);
        }
        
        return this.ribbon;
    }
    
    /**
        * Builds the ribbons for the layout
        * @param layout Layout to which the ribbons shall be built
        * @param changeEvent The event that the layout has been changed
        */
     buildRibbons(changeEvent: DMI.Api.ILayoutChangedEvent): void {
        var tthis = this;
        var ribbon = tthis.getRibbon();
        ribbon.clear();
        var tabFile = ribbon.getOrAddTab("File");

        tabFile.addIcon("Home", "img/icons/home", () => { tthis.gotoHome(); });
        tabFile.addIcon("Refresh", "img/icons/refresh_update", () => { tthis.refreshView(); });

        tabFile.addIcon("Workspaces", "img/icons/database", () => { tthis.navigateToWorkspaces(); });
        tabFile.addIcon("Add Workspace", "img/icons/database-add", () => { DMDialog.showDialogNewWorkspace(tthis); });

        if (changeEvent !== null && changeEvent !== undefined && changeEvent.workspace !== undefined) {
            // Ok, we have a workspace
            tabFile.addIcon("Delete Workspace", "img/icons/database-delete", () => {
                DMClient.WorkspaceApi.deleteWorkspace(changeEvent.workspace)
                    .done(() => tthis.navigateToWorkspaces());
            });

            tabFile.addIcon("Create Extent", "img/icons/folder_open-new", () => {
                DMDialog.showNavigationForNewExtents(tthis, changeEvent.workspace);
            });

            tabFile.addIcon("Add Extent", "img/icons/folder_open-add", () => {
                DMDialog.showDialogAddCsvExtent(tthis, changeEvent.workspace);
            });

            if (changeEvent.extent !== undefined) {
                tabFile.addIcon("Delete Extent", "img/icons/folder_open-delete", () => {
                    DMClient.ExtentApi.deleteExtent(changeEvent.workspace, changeEvent.extent)
                        .done(() => tthis.navigateToExtents(changeEvent.workspace));
                });

                tabFile.addIcon("Export Extent", "img/icons/folder_open-download", () => {
                    tthis.exportExtent(changeEvent.workspace, changeEvent.extent);
                });
            }

            tabFile.addIcon("Add ZipCodes", "img/icons/folder_open-mail", () => {
                DMClient.ExampleApi.addZipCodes(changeEvent.workspace).done(
                    () => tthis.refreshView());
            });
        }

        tabFile.addIcon("Close", "img/icons/close_window", () => { window.close(); });
    }

}