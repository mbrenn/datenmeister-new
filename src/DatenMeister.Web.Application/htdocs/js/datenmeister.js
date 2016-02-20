/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
define(["require", "exports", "datenmeister-helper", "datenmeister-interfaces", "datenmeister-client", "datenmeister-ribbon", "datenmeister-layout", "datenmeister-logging"], function (require, exports, DMHelper, DMI, DMClient, DMRibbon, DMLayout, DMLog) {
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
            tabFile.addIcon("Delete Workspace", "img/icons/database-delete", function () {
                DMClient.WorkspaceApi.deleteWorkspace(changeEvent.workspace)
                    .done(function () { return layout.navigateToWorkspaces(); });
            });
            tabFile.addIcon("Create Extent", "img/icons/folder_open-add", function () {
                showDialogNewExtent(layout, changeEvent.workspace);
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
        var configuration = new DMI.Api.FormForItemConfiguration();
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
    function showDialogNewExtent(layout, workspace) {
        var configuration = new DMI.Api.FormForItemConfiguration();
        configuration.onOkForm = function (data) {
            DMClient.ExtentApi.createExtent({
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
        layout.navigateToDialog(configuration);
    }
});
//# sourceMappingURL=datenmeister.js.map