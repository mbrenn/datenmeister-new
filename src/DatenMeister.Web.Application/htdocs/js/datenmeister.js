define(["require", "exports", "./datenmeister-helper", "./datenmeister-interfaces", "./datenmeister-client", "./datenmeister-layout", "./datenmeister-view", "./datenmeister-logging"], function (require, exports, DMHelper, DMI, DMClient, DMLayout, DMView, DMLog) {
    "use strict";
    exports.__esModule = true;
    function start() {
        var layout = new DMLayout.ApplicationWindow($("body"));
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
                parseAndNavigateToWindowLocation(layout);
            };
            parseAndNavigateToWindowLocation(layout);
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
        // Loads the clientplugins
        DMClient.ClientApi.getPlugins()
            .done(function (data) {
            var parameter = new DMI.Plugin.PluginParameter();
            parameter.version = "1.0";
            parameter.layout = layout;
            for (var n in data.scriptPaths) {
                var path = data.scriptPaths[n];
                // Now loading the plugin
                require([path], function (plugin) {
                    var result = plugin.load(parameter);
                    if (result !== undefined && result !== null) {
                        layout.pluginResults[layout.pluginResults.length] = result;
                        if (result.onViewPortChanged !== undefined) {
                            layout.mainViewPort.refresh();
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
        if (ws === "") {
            DMView.WorkspaceList.navigateToWorkspaces(layout.mainViewPort);
        }
        else if (ws === "{all}") {
            DMView.WorkspaceList.navigateToWorkspaces(layout.mainViewPort);
        }
        else if (extentUrl === "") {
            DMView.ExtentList.navigateToExtents(layout.mainViewPort, ws);
        }
        else if (itemUrl === "") {
            DMView.ItemList.navigateToItems(layout.mainViewPort, ws, extentUrl, view);
        }
        else {
            var settings = {};
            if (mode === "readonly") {
                settings.isReadonly = true;
            }
            DMView.ItemDetail.navigateToItem(layout.mainViewPort, ws, extentUrl, itemUrl, view, settings);
        }
        $(".body-content").show();
    }
    exports.parseAndNavigateToWindowLocation = parseAndNavigateToWindowLocation;
});
//# sourceMappingURL=datenmeister.js.map