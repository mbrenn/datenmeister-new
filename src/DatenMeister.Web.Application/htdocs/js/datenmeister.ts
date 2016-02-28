﻿/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />

import * as DMHelper from "datenmeister-helper";
import * as DMI from "datenmeister-interfaces";
import * as DMTables from "datenmeister-tables";
import * as DMView from "datenmeister-view";
import * as DMClient from "datenmeister-client";
import * as DMRibbon from "datenmeister-ribbon";
import * as DMLayout from "datenmeister-layout";
import * as DMLog from "datenmeister-logging";


export function start() {

    // Information, when an ajax request failed
    $(document).ajaxError((ev, xhr, settings, error) => {
        if (xhr.responseJSON !== undefined &&
            xhr.responseJSON !== null &&
            xhr.responseJSON.ExceptionMessage !== undefined &&
            xhr.responseJSON.ExceptionMessage !== null) {
            DMLog.theLog.writeError(xhr.responseJSON.ExceptionMessage);
        } else {
            DMLog.theLog.writeError(xhr);
        }
    });

    $(document).ready(() => {
        window.onpopstate = (ev) => {
            parseAndNavigateToWindowLocation();
        };

        parseAndNavigateToWindowLocation();
    });

    // Ajax loading information
    var ajaxRequests = 0;
    $("#dm-ajaxloading").hide();
    $(document).ajaxStart(() => {
        $("#dm-ajaxloading").show();
        ajaxRequests++;
    });

    $(document).ajaxStop(() => {
        ajaxRequests--;
        if (ajaxRequests === 0) {
            $("#dm-ajaxloading").hide();
        }
    });
}

export function parseAndNavigateToWindowLocation() {
    var ws = DMHelper.getParameterByNameFromHash("ws");
    var extentUrl = DMHelper.getParameterByNameFromHash("ext");
    var itemUrl = DMHelper.getParameterByNameFromHash("item");
    var mode = DMHelper.getParameterByNameFromHash("mode");

    var layout = new DMLayout.Layout($("body"));
    layout.onLayoutChanged = (data) => buildRibbons(layout, data);

    if (ws === "") {
        layout.showWorkspaces();
    } else if (extentUrl === "") {
        layout.showExtents(ws);
    } else if (itemUrl === "") {
        layout.showItems(ws, extentUrl);
    } else {
        var settings: DMI.View.IItemViewSettings= {};
        if (mode === "readonly") {
            settings.isReadonly = true;
        }

        layout.showItem(ws, extentUrl, itemUrl, settings);
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

        tabFile.addIcon("Create Extent", "img/icons/folder_open-new", () => {
            showDialogNewExtent(layout, changeEvent.workspace);
        });

        tabFile.addIcon("Add Workspace", "img/icons/folder_open-add", () => {
            showDialogAddExtent(layout, changeEvent.workspace);
        });

        if (changeEvent.extent !== undefined) {
            tabFile.addIcon("Delete Extent", "img/icons/folder_open-delete", () => {
                DMClient.ExtentApi.deleteExtent(changeEvent.workspace, changeEvent.extent)
                    .done(() => layout.navigateToExtents(changeEvent.workspace));
            });
        }

        tabFile.addIcon("Add ZipCodes", "img/icons/folder_open-mail", () => {
            DMClient.ExampleApi.addZipCodes(changeEvent.workspace).done(
                () => layout.refreshView());
        });
    }

    tabFile.addIcon("Close", "img/icons/close_window", () => { window.close(); });
}

function showDialogNewWorkspace(layout: DMLayout.Layout) {
    var configuration = new DMI.Api.DialogConfiguration();

    configuration.onOkForm = data => {
        DMClient.WorkspaceApi.createWorkspace(
            {
                name: data.v["name"],
                annotation: data.v["annotation"]
            })
            .done(() => layout.navigateToWorkspaces());
    };

    configuration.addColumn(new DMI.Table.DataTableColumn("Title", "name"));
    configuration.addColumn(new DMI.Table.DataTableColumn("Annotation", "annotation"));

    layout.navigateToDialog(configuration);
}

function showDialogNewExtent(layout: DMLayout.Layout, workspace: string) {
    var configuration = new DMI.Api.DialogConfiguration();

    configuration.onOkForm = data => {
        DMClient.ExtentApi.createExtent(
            {
                workspace: data.v["workspace"],
                contextUri: data.v["contextUri"],
                filename: data.v["filename"], 
                columns: data.v["columns"]
            })
            .done(() => layout.navigateToExtents(data.v["workspace"]));
    };

    configuration.addColumn(new DMI.Table.DataTableColumn("Workspace", "workspace").withDefaultValue(workspace));
    configuration.addColumn(new DMI.Table.DataTableColumn("URI", "contextUri").withDefaultValue("dm:///"));
    configuration.addColumn(new DMI.Table.DataTableColumn("Filename", "filename"));
    configuration.addColumn(new DMI.Table.DataTableColumn("Columns", "columns").withDefaultValue("Column1,Column2"));
    configuration.ws = workspace;

    layout.navigateToDialog(configuration);
}

function showDialogAddExtent(layout: DMLayout.Layout, workspace: string) {
}