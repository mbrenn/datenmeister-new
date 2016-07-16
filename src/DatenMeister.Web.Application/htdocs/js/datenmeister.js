/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
define(["require", "exports", "./datenmeister-helper", "./datenmeister-client", "./datenmeister-ribbon", "./datenmeister-layout", "./datenmeister-logging"], function (require, exports, DMHelper, DMClient, DMRibbon, DMLayout, DMLog) {
    "use strict";
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
        tabFile.addIcon("Home", "img/icons/home", function () { layout.gotoHome(); });
        tabFile.addIcon("Refresh", "img/icons/refresh_update", function () { layout.refreshView(); });
        tabFile.addIcon("Workspaces", "img/icons/database", function () { layout.showWorkspaces(); });
        tabFile.addIcon("Add Workspace", "img/icons/database-add", function () { layout.showDialogNewWorkspace(); });
        if (changeEvent.workspace !== undefined) {
            // Ok, we have a workspace
            tabFile.addIcon("Delete Extent", "img/icons/database-delete", function () {
                DMClient.WorkspaceApi.deleteWorkspace(changeEvent.workspace)
                    .done(function () { return layout.navigateToWorkspaces(); });
            });
            tabFile.addIcon("Create Extent", "img/icons/folder_open-new", function () {
                layout.showNavigationForNewExtents(changeEvent.workspace);
            });
            tabFile.addIcon("Add Extent", "img/icons/folder_open-add", function () {
                layout.showDialogAddCsvExtent(changeEvent.workspace);
            });
            if (changeEvent.extent !== undefined) {
                tabFile.addIcon("Delete Extent", "img/icons/folder_open-delete", function () {
                    DMClient.ExtentApi.deleteExtent(changeEvent.workspace, changeEvent.extent)
                        .done(function () { return layout.navigateToExtents(changeEvent.workspace); });
                });
                tabFile.addIcon("Export Extent", "img/icons/folder_open-download", function () {
                    layout.exportExtent(changeEvent.workspace, changeEvent.extent);
                });
            }
            tabFile.addIcon("Add ZipCodes", "img/icons/folder_open-mail", function () {
                DMClient.ExampleApi.addZipCodes(changeEvent.workspace).done(function () { return layout.refreshView(); });
            });
        }
        tabFile.addIcon("Close", "img/icons/close_window", function () { window.close(); });
    }
});
//# sourceMappingURL=datenmeister.js.map