/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />

import * as DMHelper from "datenmeister-helper";
import * as DMI from "datenmeister-interfaces";
import * as DMTables from "datenmeister-tables";
import * as DMLayout from "datenmeister-layout";
import * as DMClient from "datenmeister-client";


export namespace Navigation {
    export function start() {
        $(document).ready(() => {
            window.onpopstate = ev => {
                parseAndNavigateToWindowLocation();
            };

            parseAndNavigateToWindowLocation();
        });
    }

    export function parseAndNavigateToWindowLocation() {
        var ws = DMHelper.getParameterByNameFromHash("ws");
        var extentUrl = DMHelper.getParameterByNameFromHash("ext");
        var itemUrl = DMHelper.getParameterByNameFromHash("item");

        if (ws === "") {
            loadWorkspaces();
        } else if (extentUrl === "") {
            loadExtents(ws);
        } else if (itemUrl === "") {
            loadExtent(ws, extentUrl);
        } else {
            loadItem(ws, extentUrl, itemUrl);
        }
    }

    function createTitle(ws?: string, extentUrl?: string, itemUrl?: string) {
        var containerTitle = $(".container_title");
        var containerRefresh = $("<a href='#'>Refresh</a>");

        if (ws === undefined) {
            containerTitle.text("Workspaces - ");
            containerRefresh.click(() => {
                loadWorkspaces();
                return false;
            });
        } else if (extentUrl === undefined) {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - Extents - ");
            containerRefresh.click(() => {
                loadExtents(ws);
                return false;
            });
        } else if (itemUrl == undefined) {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - Items - ");
            containerRefresh.click(() => {
                loadExtent(ws, extentUrl);
                return false;
            });
        } else {
            containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - <a href='#' class='link_items'>Items</a> - ");
            containerRefresh.click(() => {
                loadItem(ws, extentUrl, itemUrl);
                return false;
            });
        }

        containerTitle.append(containerRefresh);

        $(".link_workspaces", containerTitle).click(() => {
            loadWorkspaces();
            return false;
        });
        $(".link_extents", containerTitle).click(() => {
            loadExtents(ws);
            return false;
        });
        $(".link_items", containerTitle).click(() => {
            loadExtent(ws, extentUrl);
            return false;
        });
    }

    export function loadWorkspaces() {
        var workbenchLogic = new DMLayout.WorkspaceLayout();
        workbenchLogic.onWorkspaceSelected = (id: string) => {
            // Loads the extent of the workspace, if the user has clicked on one of the workbenches
            history.pushState({}, "", "#ws=" + encodeURIComponent(id));
            loadExtents(id);
        };

        workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".container_data"))
            .done(function(data) {
                createTitle();
            })
            .fail(function() {
            });
    }

    export function loadExtents(workspaceId: string) {
        var extentLogic = new DMLayout.ExtentLayout();
        extentLogic.onExtentSelected = function(ws: string, extentUrl: string) {
            history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                + "&ext=" + encodeURIComponent(extentUrl));
            loadExtent(ws, extentUrl);
            return false;
        };

        extentLogic.loadAndCreateHtmlForWorkspace($(".container_data"), workspaceId)
            .done(function(data) {
                createTitle(workspaceId);
            })
            .fail(function() {
            });
    }

    export function loadExtent(workspaceId: string, extentUrl: string) {
        var extentLogic = new DMLayout.ExtentLayout();
        extentLogic.onItemSelected = function(ws: string, extentUrl: string, itemUrl: string) {
            navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.onItemCreated = function(ws: string, extentUrl: string, itemUrl: string) {
            navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.loadAndCreateHtmlForExtent($(".container_data"), workspaceId, extentUrl).done(
            data => {
                createTitle(workspaceId, extentUrl);
            });
    }

    export function navigateToItem(ws: string, extentUrl: string, itemUrl: string) {
        history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
            + "&ext=" + encodeURIComponent(extentUrl)
            + "&item=" + encodeURIComponent(itemUrl));
        loadItem(ws, extentUrl, itemUrl);
    }

    export function loadItem(workspaceId: string, extentUrl: string, itemUrl: string) {
        var extentLogic = new DMLayout.ExtentLayout();

        createTitle(workspaceId, extentUrl, itemUrl);
        extentLogic.loadAndCreateHtmlForItem($(".container_data"), workspaceId, extentUrl, itemUrl);
    }
}