﻿/// <reference path="typings/jquery/jquery.d.ts" />
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
    layout.onLayoutChanged = (data) => buildRibbons(layout, data);

    if (ws === "") {
        layout.showWorkspaces();
    } else if (extentUrl === "") {
        layout.showExtents(ws);
    } else if (itemUrl === "") {
        layout.showItems(ws, extentUrl);
    } else {
        layout.showItem(ws, extentUrl, itemUrl);
    }
    

    $(".body-content").show();
}

function buildRibbons(layout: DMLayout.Layout, changeEvent: DMLayout.ILayoutChangedEvent) {
    var domRibbon = $(".datenmeister-ribbon");
    var ribbon = new DMRibbon.Ribbon(domRibbon);
    var tabFile = ribbon.addTab("File");

    tabFile.addIcon("Refresh", "img/icons/refresh_update", () => { layout.refreshView(); });

    tabFile.addIcon("Workspaces", "img/icons/database", () => { layout.showWorkspaces(); });
    tabFile.addIcon("Add Workspace", "img/icons/database-add", () => { showDialogNewWorkspace(layout); });

    if (changeEvent.workspace !== undefined) {
        // Ok, we have a workspace
        tabFile.addIcon("Delete Workspace", "img/icons/database-delete", () => {
            DMClient.WorkspaceApi.deleteWorkspace(changeEvent.workspace)
                .done(() => layout.navigateToWorkspaces());
        });

        tabFile.addIcon("Create Extent", "img/icons/folder_open-add", () => {
            showDialogNewExtent(layout, changeEvent.workspace);
        });

        if (changeEvent.extent !== undefined) {
            tabFile.addIcon("Delete Extent", "img/icons/folder_open-delete", () => {
                DMClient.ExtentApi.deleteExtent(changeEvent.workspace, changeEvent.extent)
                    .done(() => layout.navigateToExtents(changeEvent.workspace));
            });
        }

        tabFile.addIcon("Add ZipCodes", "img/icons/folder_open-mail", () => {
            DMClient.ExampleApi.addZipCodes(changeEvent.workspace);
        });
    }

    tabFile.addIcon("Close", "img/icons/close_window", () => { window.close(); });
}

function showDialogNewWorkspace(layout: DMLayout.Layout) {
    var configuration = new DMI.Api.FormForItemConfiguration();
    configuration.onOkForm = data => {
        DMClient.WorkspaceApi.createWorkspace(
            {
                name: data.v["name"],
                annotation: data.v["annotation"]
            })
            .done(() => layout.navigateToWorkspaces());
    };

    var column =
        {
            title: "Title",
            name: "name"
        };

    configuration.addColumn(column);
    column =
        {
            title: "Annotation",
            name: "annotation"
        };

    configuration.addColumn(column);

    layout.navigateToDialog(configuration);
}

function showDialogNewExtent(layout: DMLayout.Layout, workspace: string) {
    alert('X');
}