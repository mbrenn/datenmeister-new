/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />

import * as DMHelper from "datenmeister-helper";
import * as DMI from "datenmeister-interfaces";
import * as DMTables from "datenmeister-tables";
import * as DMView from "datenmeister-view";
import * as DMClient from "datenmeister-client";
import * as DMRibbon from "datenmeister-ribbon";
import * as DMLayout from "datenmeister-layout"


export function start() {
    $(document).ready(() => {
        window.onpopstate = (ev) => {
            parseAndNavigateToWindowLocation();
        };

        parseAndNavigateToWindowLocation();
    });
}

export function parseAndNavigateToWindowLocation() {
    var ws = DMHelper.getParameterByNameFromHash("ws");
    var extentUrl = DMHelper.getParameterByNameFromHash("ext");
    var itemUrl = DMHelper.getParameterByNameFromHash("item");

    var layout = new DMLayout.Layout($("body"));

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

    $(".body-content").show();
}

function buildRibbons(layout: DMLayout.Layout) {
    var domRibbon = $(".datenmeister-ribbon");
    var ribbon = new DMRibbon.Ribbon(domRibbon);
    var tab1 = ribbon.addTab("File");

    tab1.addIcon("Refresh", "img/icons/refresh_update", () => { layout.refreshView(); });

    tab1.addIcon("Workspaces", "img/icons/database", () => { layout.showWorkspaces(); });
    tab1.addIcon("Add Workspace", "img/icons/database-add", () => { alert("X") });
    tab1.addIcon("Delete Workspace", "img/icons/database-delete", () => { alert("X") });

    tab1.addIcon("Close", "img/icons/close_window", () => { window.close(); });

}