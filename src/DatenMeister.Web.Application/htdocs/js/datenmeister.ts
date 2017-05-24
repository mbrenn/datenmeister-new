
import * as DMHelper from "./datenmeister-helper";
import * as DMI from "./datenmeister-interfaces";
import * as DMClient from "./datenmeister-client";
import * as DMLayout from "./datenmeister-layout";
import * as DMView from "./datenmeister-view";
import * as DMLog from "./datenmeister-logging";

export function start() {

    var layout =
        new DMLayout.ApplicationWindow($("body"));

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
            parseAndNavigateToWindowLocation(layout);
        };

        parseAndNavigateToWindowLocation(layout);
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

    // Loads the clientplugins
    DMClient.ClientApi.getPlugins()
        .done((data: DMClient.ClientApi.IGetPluginsResponse) => {

            var parameter = new DMI.Plugin.PluginParameter();
            parameter.version = "1.0";
            parameter.layout = layout;

            for (var n in data.scriptPaths) {
                var path = data.scriptPaths[n];

                // Now loading the plugin
                require([path], plugin => {
                    var result: DMI.Plugin.IPluginResult = plugin.load(parameter);
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

export function parseAndNavigateToWindowLocation(layout: DMLayout.ApplicationWindow) {
    var ws = DMHelper.getParameterByNameFromHash("ws");
    var extentUrl = DMHelper.getParameterByNameFromHash("ext");
    var itemUrl = DMHelper.getParameterByNameFromHash("item");
    var mode = DMHelper.getParameterByNameFromHash("mode");
    var view = DMHelper.getParameterByNameFromHash("view");

    if (ws === "") {
        DMView.navigateToWorkspaces(layout.mainViewPort); 
    } else if (ws === "{all}") {
        DMView.navigateToWorkspaces(layout.mainViewPort); 
    } else if (extentUrl === "") {
        DMView.navigateToExtents(layout.mainViewPort, ws);
    } else if (itemUrl === "") {
        DMView.navigateToItems(layout.mainViewPort, ws, extentUrl, view);
    } else {
        var settings: DMI.Navigation.IItemViewSettings = {};
        if (mode === "readonly") {
            settings.isReadonly = true;
        }

        layout.mainViewPort.showItem(layout.mainViewPort, ws, extentUrl, itemUrl, view, settings);
    }

    $(".body-content").show();
}