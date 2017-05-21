
import DMI = require("./datenmeister-interfaces");
import DMCI = require("./datenmeister-clientinterface");
import DMView = require("./datenmeister-view");

export class ViewPort implements DMI.Navigation.INavigation{
    private container: JQuery;
    private layout: DMI.Api.ILayout;
    onViewPortChanged: (data: DMI.Api.ILayoutChangedEvent) => void;
    viewState: DMI.Api.IViewState;
    onRefresh: () => void;

    constructor(container: JQuery, layout: DMI.Api.ILayout) {
        this.container = container;
        this.layout = layout;
    }

    /**
     * Sets the view into the dom
     * @param view view to be set
     */
    setView(view: DMI.Views.IView): void {
        var viewContainer = $(".dm-view", this.container);
        viewContainer.empty();
        viewContainer.append(view.getContent());

        var viewState = view.getViewState();

        if (viewState !== undefined && viewState !== null) {
            var ev =
            {
                navigation: this,
                viewState: viewState
            };
            this.layout.throwViewPortChanged(ev);
        }

        view.viewport = this;
    }

    navigateToWorkspaces(): void {
        history.pushState({}, "", "#ws={all}");
        this.showWorkspaces();
    }

    navigateToView(view: DMI.Views.IView): void {
        this.setView(view);
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

    navigateToItem(ws: string, extentUrl: string, itemUrl: string, viewname?: string, settings?: DMI.Navigation.IItemViewSettings): void {
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

    navigateToDialog(configuration: DMI.Navigation.DialogConfiguration): void {
        var dialog = new DMView.DialogView(this);
        dialog.createDialog(configuration);
        this.setView(dialog);
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
        this.setView(workbenchLogic);
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
        this.setView(extentView);
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

        var query = new DMCI.Out.ItemTableQuery();
        query.view = viewname;
        query.amount = 20;

        extentView.loadAndCreateHtmlForExtent(workspaceId, extentUrl, query);
        this.setView(extentView);
    }

    showItem(workspaceId: string,
        extentUrl: string,
        itemUrl: string,
        viewname?: string,
        settings?: DMI.Navigation.IItemViewSettings) {
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
        this.setView(itemView);
    }

    gotoHome(): void {
        this.navigateToExtents("Data");
    }
    
    renavigate(): void {
        this.throwViewPortChanged(this.viewState);
    }

    refresh(): void {
        if (this.onRefresh !== undefined && this.onRefresh !== null) {
            this.onRefresh();
        }
    }

    setStatus(statusDom: JQuery): void {
        var dom = $(".dm-statusline");
        dom.empty();
        dom.append(statusDom);
    }

    throwViewPortChanged(data: DMI.Api.IViewState): void {
        this.viewState = data;

        var ev =
        {
            navigation: this,
            viewState: data
        };


        if (this.onViewPortChanged !== undefined) {
            this.onViewPortChanged(ev);
        }

        this.layout.throwViewPortChanged(ev);
    }

    createTitle(ws?: string, extentUrl?: string, itemUrl?: string) {
        var tthis = this;
        var containerTitle = $(".container-title", this.container);
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
}