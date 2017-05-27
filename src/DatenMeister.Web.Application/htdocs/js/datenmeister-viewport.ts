import DMI = require("./datenmeister-interfaces");
import DMView = require("./datenmeister-view");
import IViewState = DMI.Api.IViewState;
import IViewPort = DMI.Views.IViewPort;

export class ViewPort {
    private isMasterView: boolean;
    private container: JQuery;
    private layout: DMI.Api.ILayout;
    onViewPortChanged: (data: DMI.Api.ILayoutChangedEvent) => void;
    viewState: DMI.Api.IViewState;
    currentView: DMI.Views.IView;

    constructor(container: JQuery, layout: DMI.Api.ILayout) {
        this.container = container;
        this.layout = layout;
        this.isMasterView = true;
    }

    /**
     * Sets the view into the dom
     * @param view view to be set
     */
    setView(view: DMI.Views.IView): void {
        var viewContainer = $(".dm-view", this.container);
        viewContainer.empty();
        viewContainer.append(view.load());

        var viewState = view.getViewState();

        if (viewState !== undefined && viewState !== null) {
            var ev =
            {
                viewState: viewState
            };
            this.layout.throwViewPortChanged(ev);
        }

        view.viewport = this;
        this.addViewState(view.getViewState());
        this.currentView = view;
    }

    addViewState(viewState: IViewState) {
        if (viewState === undefined || viewState === null) {
            return;
        }

        var url = "#";
        if (viewState.workspace === undefined) {
            url += "ws={all}";
        } else if (viewState.extent === undefined) {
            url += `ws=${encodeURIComponent(viewState.workspace)}`;
        } else if (viewState.item === undefined) {
            url += `ws=${encodeURIComponent(viewState.workspace)}&ext=${encodeURIComponent(viewState.extent)}}`;
        } else {
            url += `ws=${encodeURIComponent(viewState.workspace)}&ext=${encodeURIComponent(viewState.extent)}&item=${
                encodeURIComponent(viewState.item)}`;

        }

        if (viewState.isReadonly) {
            url += "&mode=readonly";
        }

        if (viewState.viewname !== undefined && viewState.viewname !== null) {
            url += `&view=${encodeURIComponent(viewState.viewname)}`;
        }

        history.pushState({}, "", url);

    }


    navigateToView(view: DMI.Views.IView): void {
        this.setView(view);
    }

    gotoHome(): void {
        DMView.WorkspaceList.navigateToWorkspaces(this);
    }

    refresh(): void {
        if (this.currentView !== undefined && this.currentView !== null) {
            this.currentView.refresh();
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
            viewState: data
        };


        if (this.onViewPortChanged !== undefined) {
            this.onViewPortChanged(ev);
        }

        this.createTitle();

        this.layout.throwViewPortChanged(ev);
    }

    createTitle(ws?: string, extentUrl?: string, itemUrl?: string) {
        var tthis = this;
        var containerTitle = $(".container-title", this.container);
        var ba = "&gt;&gt";
        if (ws === undefined) {
            containerTitle.text("Workspaces");
        } else if (extentUrl === undefined) {
            containerTitle.html(
                `<a href='#' class='link_workspaces'>Workspace '<b>${ws}</b>'</a> ${ba} Extents`);
        } else if (itemUrl == undefined) {
            containerTitle
                .html(
                    `<a href='#' class='link_workspaces'> Workspace '<b>${ws}</b>'</a> ${ba
                    } <a href='#' class='link_extents'>Extent '<b>${extentUrl}</b>'</a> ${ba
                    } Items`);
        } else {
            containerTitle
                .html(`<a href='#' class='link_workspaces'>Workspace '<b>${ws}</b>'</a> ${ba
                    } <a href='#' class='link_extents'>Extent '<b>${extentUrl}</b>'</a> ${ba
                    } <a href='#' class='link_items'>Items</a>`);
        }

        $(".link_workspaces", containerTitle)
            .click(() => {
                DMView.WorkspaceList.navigateToWorkspaces(this);
                return false;
            });
        $(".link_extents", containerTitle)
            .click(() => {
                DMView.ExtentList.navigateToExtents(this, ws);
                return false;
            });
        $(".link_items", containerTitle)
            .click(() => {
                DMView.ItemList.navigateToItems(this, ws, extentUrl);
                return false;
            });
    }
}