define(["require", "exports", "./datenmeister-helper", "./datenmeister-interfaces", "./datenmeister-client", "./datenmeister-layout", "./datenmeister-logging"], function (require, exports, DMHelper, DMI, DMClient, DMLayout, DMLog) {
    "use strict";
    function start() {
        var layout = new DMLayout.Layout($("body"));
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
                parseAndNavigateToWindowLocation(layout);
            };
            parseAndNavigateToWindowLocation(layout);
        });
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
        DMClient.ClientApi.getPlugins()
            .done(function (data) {
            var parameter = new DMI.Api.PluginParameter();
            parameter.version = "1.0";
            parameter.layout = layout;
            for (var n in data.scriptPaths) {
                var path = data.scriptPaths[n];
                require([path], function (plugin) {
                    var result = plugin.load(parameter);
                    if (result !== undefined && result !== null) {
                        layout.pluginResults[layout.pluginResults.length] = result;
                        if (result.onViewPortChanged !== undefined) {
                            layout.renavigate();
                        }
                    }
                });
            }
        });
    }
    exports.start = start;
    function parseAndNavigateToWindowLocation(layout) {
        var ws = DMHelper.getParameterByNameFromHash("ws");
        var extentUrl = DMHelper.getParameterByNameFromHash("ext");
        var itemUrl = DMHelper.getParameterByNameFromHash("item");
        var mode = DMHelper.getParameterByNameFromHash("mode");
        var view = DMHelper.getParameterByNameFromHash("view");
        layout.onViewPortChanged = function (data) {
            buildRibbons(layout, data);
        };
        if (ws === "") {
            layout.showExtents("Data");
        }
        else if (ws === "{all}") {
            layout.showWorkspaces();
        }
        else if (extentUrl === "") {
            layout.showExtents(ws);
        }
        else if (itemUrl === "") {
            layout.showItems(ws, extentUrl, view);
        }
        else {
            var settings = {};
            if (mode === "readonly") {
                settings.isReadonly = true;
            }
            layout.showItem(ws, extentUrl, itemUrl, view, settings);
        }
        $(".body-content").show();
    }
    exports.parseAndNavigateToWindowLocation = parseAndNavigateToWindowLocation;
    function buildRibbons(layout, changeEvent) {
        var ribbon = layout.getRibbon();
        ribbon.clear();
        var tabFile = ribbon.getOrAddTab("File");
        tabFile.addIcon("Home", "img/icons/home", function () { layout.gotoHome(); });
        tabFile.addIcon("Refresh", "img/icons/refresh_update", function () { layout.refreshView(); });
        tabFile.addIcon("Workspaces", "img/icons/database", function () { layout.navigateToWorkspaces(); });
        tabFile.addIcon("Add Workspace", "img/icons/database-add", function () { layout.showDialogNewWorkspace(); });
        if (changeEvent !== null && changeEvent !== undefined && changeEvent.workspace !== undefined) {
            tabFile.addIcon("Delete Workspace", "img/icons/database-delete", function () {
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