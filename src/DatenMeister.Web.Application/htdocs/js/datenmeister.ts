/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />

import * as DMHelper from "./datenmeister-helper";
import * as DMI from "./datenmeister-interfaces";
import * as DMTables from "./datenmeister-tables";
import * as DMView from "./datenmeister-view";
import * as DMRibbon from "./datenmeister-ribbon";
import * as DMClient from "./datenmeister-client";
import * as DMLayout from "./datenmeister-layout";
import * as DMLog from "./datenmeister-logging";
import * as DMN from "./datenmeister-navigation";

export function start() {

    var layout =
        new DMLayout.Layout($("body"));

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

            var parameter = new DMI.Api.PluginParameter();
            parameter.version = "1.0";
            parameter.layout = layout;

            for (var n in data.scriptPaths) {
                var path = data.scriptPaths[n];

                // Now loading the plugin
                require([path], plugin => {
                    var result : DMI.Api.IPluginResult = plugin.load(parameter);
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

export function parseAndNavigateToWindowLocation(layout: DMLayout.Layout) {
    var ws = DMHelper.getParameterByNameFromHash("ws");
    var extentUrl = DMHelper.getParameterByNameFromHash("ext");
    var itemUrl = DMHelper.getParameterByNameFromHash("item");
    var mode = DMHelper.getParameterByNameFromHash("mode");
    var view = DMHelper.getParameterByNameFromHash("view");
    layout.onViewPortChanged = (data) => {
         layout.buildRibbons(data);
    };

    if (ws === "") {
        // per default, show the data extent
        layout.showExtents("Data");
    } else if (ws === "{all}") {
        layout.showWorkspaces();
    } else if (extentUrl === "") {
        layout.showExtents(ws);
    } else if (itemUrl === "") {
        layout.showItems(ws, extentUrl, view);
    } else {
        var settings: DMN.Settings.IItemViewSettings = {};
        if (mode === "readonly") {
            settings.isReadonly = true;
        }

        layout.showItem(ws, extentUrl, itemUrl, view, settings);
    }

    $(".body-content").show();
}