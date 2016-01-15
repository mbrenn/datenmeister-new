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

        buildRibbons();
    });


}

function buildRibbons() {
    var domRibbon = $(".datenmeister-ribbon");
    var ribbon = new DMRibbon.Ribbon(domRibbon);
    var tab1 = ribbon.addTab("File");
    tab1.addIcon("Close", "img/icons/close_window", () => { window.close(); });

    var tab2 = ribbon.addTab("Data");
    tab2.addIcon("Refresh", "img/icons/refresh_update", () => { alert('data') });
}

export function parseAndNavigateToWindowLocation() {
    var ws = DMHelper.getParameterByNameFromHash("ws");
    var extentUrl = DMHelper.getParameterByNameFromHash("ext");
    var itemUrl = DMHelper.getParameterByNameFromHash("item");

    var layout = new Layout($(".body-content"));

    if (ws === "") {
        layout.showWorkspaces();
    } else if (extentUrl === "") {
        layout.showExtents(ws);
    } else if (itemUrl === "") {
        layout.showItems(ws, extentUrl);
    } else {
        layout.showItem(ws, extentUrl, itemUrl);
    }
}

export enum PageType {
    Workspaces,
    Extents,
    Items,
    ItemDetail
}

export class Layout
{
    parent: JQuery;

    constructor(parent: JQuery) {
        this.parent = parent;
    }

    createTitle(ws?: string, extentUrl?: string, itemUrl?: string) {
        var tthis = this;
        var containerTitle = $(".container_title", this.parent);
        var containerRefresh = $("<a href='#'>Refresh</a>");

        if (ws === undefined) {
            containerTitle.text("Workspaces - ");
            containerRefresh.click(() => {
                tthis.showWorkspaces();
                return false;
            });
        } else if (extentUrl === undefined) {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - Extents - ");
            containerRefresh.click(() => {
                tthis.showExtents(ws);
                return false;
            });
        } else if (itemUrl == undefined) {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - Items - ");
            containerRefresh.click(() => {
                tthis.showItems(ws, extentUrl);
                return false;
            });
        } else {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - <a href='#' class='link_items'>Items</a> - ");
            containerRefresh.click(() => {
                tthis.showItem(ws, extentUrl, itemUrl);
                return false;
            });
        }

        containerTitle.append(containerRefresh);

        $(".link_workspaces", containerTitle).click(() => {
            tthis.showWorkspaces();
            return false;
        });
        $(".link_extents", containerTitle).click(() => {
            tthis.showExtents(ws);
            return false;
        });
        $(".link_items", containerTitle).click(() => {
            tthis.showItems(ws, extentUrl);
            return false;
        });
    }

    showWorkspaces() {
        var tthis = this;
        tthis.switchLayout(PageType.Workspaces);

        var workbenchLogic = new DMLayout.WorkspaceLayout();
        workbenchLogic.onWorkspaceSelected = (id: string) => {
            // Loads the extent of the workspace, if the user has clicked on one of the workbenches
            history.pushState({}, "", "#ws=" + encodeURIComponent(id));
            tthis.showExtents(id);
        };

        workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".data-workspaces", this.parent))
            .done(data => {
                tthis.createTitle();
            })
            .fail(() => {
            });
    }

    showExtents(workspaceId: string) {
        var tthis = this;
        tthis.switchLayout(PageType.Extents);
        var extentLogic = new DMLayout.ExtentLayout();
        extentLogic.onExtentSelected = (ws: string, extentUrl: string) => {
            history.pushState({}, "", "#ws=" + encodeURIComponent(ws)
                + "&ext=" + encodeURIComponent(extentUrl));
            tthis.showItems(ws, extentUrl);
            return false;
        };

        extentLogic.loadAndCreateHtmlForWorkspace($(".data-extents", this.parent), workspaceId)
            .done(data => {
                tthis.createTitle(workspaceId);
            })
            .fail(() => {
            });
    }

    showItems(workspaceId: string, extentUrl: string) {
        var tthis = this;
        tthis.switchLayout(PageType.Items);
        var extentLogic = new DMLayout.ExtentLayout();
        extentLogic.onItemSelected = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.onItemCreated = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.loadAndCreateHtmlForExtent($(".data-items", this.parent), workspaceId, extentUrl).done(
            data => {
                tthis.createTitle(workspaceId, extentUrl);
            });
    }

    navigateToItem(ws: string, extentUrl: string, itemUrl: string) {
        history.pushState({}, "", "#ws=" + encodeURIComponent(ws)
            + "&ext=" + encodeURIComponent(extentUrl)
            + "&item=" + encodeURIComponent(itemUrl));
        this.showItem(ws, extentUrl, itemUrl);
    }

    showItem(workspaceId: string, extentUrl: string, itemUrl: string) {
        this.switchLayout(PageType.ItemDetail);
        var extentLogic = new DMLayout.ItemView();

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
}