/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
"use strict";
var DMHelper = require("datenmeister-helper");
var DMI = require("datenmeister-interfaces");
var DMView = require("datenmeister-view");
var DMClient = require("datenmeister-client");
var DMRibbon = require("datenmeister-ribbon");
var DMLayout = require("datenmeister-layout");
var DMLog = require("datenmeister-logging");
function start() {
    // Information, when an ajax request failed
    $(document).ajaxError(function (ev, xhr, settings, error) {
        if (xhr.responseJSON !== undefined &&
            xhr.responseJSON !== null &&
            xhr.responseJSON.ExceptionMessage !== undefined &&
            xhr.responseJSON.ExceptionMessage !== null) {
            DMLog.theLog.writeError(xhr.responseJSON.ExceptionMessage);
        }
        else {
            DMLog.theLog.writeError(xhr);
        }
    });
    $(document).ready(function () {
        window.onpopstate = function (ev) {
            parseAndNavigateToWindowLocation();
        };
        parseAndNavigateToWindowLocation();
    });
    // Ajax loading information
    var ajaxRequests = 0;
    $("#dm-ajaxloading").hide();
    $(document).ajaxStart(function () {
        $("#dm-ajaxloading").show();
        ajaxRequests++;
    });
    $(document).ajaxStop(function () {
        ajaxRequests--;
        if (ajaxRequests === 0) {
            $("#dm-ajaxloading").hide();
        }
    });
}
exports.start = start;
function parseAndNavigateToWindowLocation() {
    var ws = DMHelper.getParameterByNameFromHash("ws");
    var extentUrl = DMHelper.getParameterByNameFromHash("ext");
    var itemUrl = DMHelper.getParameterByNameFromHash("item");
    var mode = DMHelper.getParameterByNameFromHash("mode");
    var layout = new DMLayout.Layout($("body"));
    layout.onLayoutChanged = function (data) { return buildRibbons(layout, data); };
    if (ws === "") {
        layout.showWorkspaces();
    }
    else if (extentUrl === "") {
        layout.showExtents(ws);
    }
    else if (itemUrl === "") {
        layout.showItems(ws, extentUrl);
    }
    else {
        var settings = {};
        if (mode === "readonly") {
            settings.isReadonly = true;
        }
        layout.showItem(ws, extentUrl, itemUrl, settings);
    }
    $(".body-content").show();
}
exports.parseAndNavigateToWindowLocation = parseAndNavigateToWindowLocation;
function buildRibbons(layout, changeEvent) {
    var domRibbon = $(".datenmeister-ribbon");
    var ribbon = new DMRibbon.Ribbon(domRibbon);
    var tabFile = ribbon.addTab("File");
    tabFile.addIcon("Refresh", "img/icons/refresh_update", function () { layout.refreshView(); });
    tabFile.addIcon("Workspaces", "img/icons/database", function () { layout.showWorkspaces(); });
    tabFile.addIcon("Add Workspace", "img/icons/database-add", function () { showDialogNewWorkspace(layout); });
    if (changeEvent.workspace !== undefined) {
        // Ok, we have a workspace
        tabFile.addIcon("Delete Extent", "img/icons/database-delete", function () {
            DMClient.WorkspaceApi.deleteWorkspace(changeEvent.workspace)
                .done(function () { return layout.navigateToWorkspaces(); });
        });
        tabFile.addIcon("Create Extent", "img/icons/folder_open-new", function () {
            showNavigationForNewExtents(layout, changeEvent.workspace);
        });
        tabFile.addIcon("Add Extent", "img/icons/folder_open-add", function () {
            showDialogAddCsvExtent(layout, changeEvent.workspace);
        });
        if (changeEvent.extent !== undefined) {
            tabFile.addIcon("Delete Extent", "img/icons/folder_open-delete", function () {
                DMClient.ExtentApi.deleteExtent(changeEvent.workspace, changeEvent.extent)
                    .done(function () { return layout.navigateToExtents(changeEvent.workspace); });
            });
        }
        tabFile.addIcon("Add ZipCodes", "img/icons/folder_open-mail", function () {
            DMClient.ExampleApi.addZipCodes(changeEvent.workspace).done(function () { return layout.refreshView(); });
        });
    }
    tabFile.addIcon("Close", "img/icons/close_window", function () { window.close(); });
}
function showDialogNewWorkspace(layout) {
    var configuration = new DMI.Api.DialogConfiguration();
    configuration.onOkForm = function (data) {
        DMClient.WorkspaceApi.createWorkspace({
            name: data.v["name"],
            annotation: data.v["annotation"]
        })
            .done(function () { return layout.navigateToWorkspaces(); });
    };
    configuration.addColumn(new DMI.Table.DataTableColumn("Title", "name"));
    configuration.addColumn(new DMI.Table.DataTableColumn("Annotation", "annotation"));
    layout.navigateToDialog(configuration);
}
function showNavigationForNewExtents(layout, workspace) {
    var view = new DMView.NavigationView(layout);
    view.addLink("New CSV Extent", function () {
        showDialogNewCsvExtent(layout, workspace);
    });
    view.addLink("New CSV Extent for UML class", function () {
        // showDialogNewExtent(layout, workspace);
    });
    view.addLink("New XmlExtent", function () {
        showDialogNewXmiExtent(layout, workspace);
    });
    layout.setView(view);
}
function showDialogNewCsvExtent(layout, workspace) {
    var configuration = new DMI.Api.DialogConfiguration();
    configuration.onOkForm = function (data) {
        DMClient.ExtentApi.createExtent({
            type: "csv",
            workspace: data.v["workspace"],
            contextUri: data.v["contextUri"],
            filename: data.v["filename"],
            columns: data.v["columns"]
        })
            .done(function () { return layout.navigateToExtents(data.v["workspace"]); });
    };
    configuration.addColumn(new DMI.Table.DataTableColumn("Workspace", "workspace").withDefaultValue(workspace));
    configuration.addColumn(new DMI.Table.DataTableColumn("URI", "contextUri").withDefaultValue("dm:///"));
    configuration.addColumn(new DMI.Table.DataTableColumn("Filename", "filename"));
    configuration.addColumn(new DMI.Table.DataTableColumn("Columns", "columns").withDefaultValue("Column1,Column2"));
    configuration.ws = workspace;
    layout.navigateToDialog(configuration);
}
function showDialogAddCsvExtent(layout, workspace) {
    var configuration = new DMI.Api.DialogConfiguration();
    configuration.onOkForm = function (data) {
        DMClient.ExtentApi.addExtent({
            type: "csv",
            workspace: data.v["workspace"],
            contextUri: data.v["contextUri"],
            filename: data.v["filename"]
        })
            .done(function () { return layout.navigateToExtents(data.v["workspace"]); });
    };
    configuration.addColumn(new DMI.Table.DataTableColumn("Workspace", "workspace").withDefaultValue(workspace));
    configuration.addColumn(new DMI.Table.DataTableColumn("URI", "contextUri").withDefaultValue("dm:///"));
    configuration.addColumn(new DMI.Table.DataTableColumn("Filename", "filename"));
    configuration.ws = workspace;
    layout.navigateToDialog(configuration);
}
function showDialogNewXmiExtent(layout, workspace) {
    var configuration = new DMI.Api.DialogConfiguration();
    configuration.onOkForm = function (data) {
        DMClient.ExtentApi.createExtent({
            type: "xmi",
            workspace: data.v["workspace"],
            contextUri: data.v["contextUri"],
            filename: data.v["filename"]
        })
            .done(function () { return layout.navigateToExtents(data.v["workspace"]); });
    };
    configuration.addColumn(new DMI.Table.DataTableColumn("Workspace", "workspace").withDefaultValue(workspace));
    configuration.addColumn(new DMI.Table.DataTableColumn("URI", "contextUri").withDefaultValue("dm:///"));
    configuration.addColumn(new DMI.Table.DataTableColumn("Filename", "filename").withDefaultValue("d:\\file.xml"));
    layout.navigateToDialog(configuration);
}
