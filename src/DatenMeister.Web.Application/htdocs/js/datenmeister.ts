/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />

import * as DMHelper from "datenmeister-helper";
import * as DMI from "datenmeister-interfaces";
import * as DMTables from "datenmeister-tables";
import * as DMLayout from "datenmeister-view";
import * as DMClient from "datenmeister-client";
import * as DMRibbon from "datenmeister-ribbon";

export function start() {
    $(document).ready(() => {
        window.onpopstate = (ev) => {
            parseAndNavigateToWindowLocation();
        };

        parseAndNavigateToWindowLocation();
    });
}

function buildRibbons(layout: Layout) {
    var domRibbon = $(".datenmeister-ribbon");
    var ribbon = new DMRibbon.Ribbon(domRibbon);
    var tab1 = ribbon.addTab("File");

    tab1.addIcon("Refresh", "img/icons/refresh_update", () => { layout.refreshView(); });

    tab1.addIcon("Workspaces", "img/icons/database", () => { layout.showWorkspaces(); });
    tab1.addIcon("Add Workspace", "img/icons/database-add", () => { alert("X") });
    tab1.addIcon("Delete Workspace", "img/icons/database-delete", () => { alert("X") });

    tab1.addIcon("Close", "img/icons/close_window", () => { window.close(); });

}

export function parseAndNavigateToWindowLocation() {
    var ws = DMHelper.getParameterByNameFromHash("ws");
    var extentUrl = DMHelper.getParameterByNameFromHash("ext");
    var itemUrl = DMHelper.getParameterByNameFromHash("item");

    var layout = new Layout($("body"));

    if (ws === "") {
        layout.showWorkspaces();
    } else if (extentUrl === "") {
        layout.showExtents(ws);
    } else if (itemUrl === "") {
        layout.showItems(ws, extentUrl);
    } else {
        layout.showItem(ws, extentUrl, itemUrl);
    }

    buildRibbons(layout);
}

export enum PageType {
    Workspaces,
    Extents,
    Items,
    ItemDetail
}

export class Layout implements DMI.Api.ILayout
{
    parent: JQuery;
    onRefresh: () => void;

    constructor(parent: JQuery) {
        this.parent = parent;
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

    navigateToItem(ws: string, extentUrl: string, itemUrl: string) {
        history.pushState({}, "", "#ws=" + encodeURIComponent(ws)
            + "&ext=" + encodeURIComponent(extentUrl)
            + "&item=" + encodeURIComponent(itemUrl));
        this.showItem(ws, extentUrl, itemUrl);
    }

    showWorkspaces() {
        var tthis = this;
        tthis.switchLayout(PageType.Workspaces);
        tthis.createTitle();

        var workbenchLogic = new DMLayout.WorkspaceView();
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
        var extentLogic = new DMLayout.ExtentView();
        extentLogic.onExtentSelected = (ws: string, extentUrl: string) => {
            history.pushState({}, "", "#ws=" + encodeURIComponent(ws)
                + "&ext=" + encodeURIComponent(extentUrl));
            tthis.showItems(ws, extentUrl);
            return false;
        };

        extentLogic.loadAndCreateHtmlForWorkspace($(".data-extents", this.parent), workspaceId);
    }

    showItems(workspaceId: string, extentUrl: string) {
        var tthis = this;
        this.switchLayout(PageType.Items);
        this.createTitle(workspaceId, extentUrl);
        var extentLogic = new DMLayout.ExtentView(this);
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
        var extentLogic = new DMLayout.ItemView(this);

        this.createTitle(workspaceId, extentUrl, itemUrl);
        extentLogic.loadAndCreateHtmlForItem($(".data-itemdetail", this.parent), workspaceId, extentUrl, itemUrl);
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
        }
    }

    setStatus(statusDom: JQuery): void {
        var dom = $(".dm-statusline");
        dom.empty();
        dom.append(statusDom);
    }
}