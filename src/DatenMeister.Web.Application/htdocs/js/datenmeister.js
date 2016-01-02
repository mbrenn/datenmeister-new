/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
define(["require", "exports", "datenmeister-helper", "datenmeister-layout"], function (require, exports, DMHelper, DMLayout) {
    var Navigation;
    (function (Navigation) {
        function start() {
            $(document).ready(function () {
                window.onpopstate = function (ev) {
                    parseAndNavigateToWindowLocation();
                };
                parseAndNavigateToWindowLocation();
            });
        }
        Navigation.start = start;
        function parseAndNavigateToWindowLocation() {
            var ws = DMHelper.getParameterByNameFromHash("ws");
            var extentUrl = DMHelper.getParameterByNameFromHash("ext");
            var itemUrl = DMHelper.getParameterByNameFromHash("item");
            if (ws === "") {
                loadWorkspaces();
            }
            else if (extentUrl === "") {
                loadExtents(ws);
            }
            else if (itemUrl === "") {
                loadExtent(ws, extentUrl);
            }
            else {
                loadItem(ws, extentUrl, itemUrl);
            }
        }
        Navigation.parseAndNavigateToWindowLocation = parseAndNavigateToWindowLocation;
        function createTitle(ws, extentUrl, itemUrl) {
            var containerTitle = $(".container_title");
            var containerRefresh = $("<a href='#'>Refresh</a>");
            if (ws === undefined) {
                containerTitle.text("Workspaces - ");
                containerRefresh.click(function () {
                    loadWorkspaces();
                    return false;
                });
            }
            else if (extentUrl === undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - Extents - ");
                containerRefresh.click(function () {
                    loadExtents(ws);
                    return false;
                });
            }
            else if (itemUrl == undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - Items - ");
                containerRefresh.click(function () {
                    loadExtent(ws, extentUrl);
                    return false;
                });
            }
            else {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - <a href='#' class='link_items'>Items</a> - ");
                containerRefresh.click(function () {
                    loadItem(ws, extentUrl, itemUrl);
                    return false;
                });
            }
            containerTitle.append(containerRefresh);
            $(".link_workspaces", containerTitle).click(function () {
                loadWorkspaces();
                return false;
            });
            $(".link_extents", containerTitle).click(function () {
                loadExtents(ws);
                return false;
            });
            $(".link_items", containerTitle).click(function () {
                loadExtent(ws, extentUrl);
                return false;
            });
        }
        function loadWorkspaces() {
            var workbenchLogic = new DMLayout.WorkspaceLayout();
            workbenchLogic.onWorkspaceSelected = function (id) {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                history.pushState({}, "", "#ws=" + encodeURIComponent(id));
                loadExtents(id);
            };
            workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".container_data"))
                .done(function (data) {
                createTitle();
            })
                .fail(function () {
            });
        }
        Navigation.loadWorkspaces = loadWorkspaces;
        function loadExtents(workspaceId) {
            var extentLogic = new DMLayout.ExtentLayout();
            extentLogic.onExtentSelected = function (ws, extentUrl) {
                history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                    + "&ext=" + encodeURIComponent(extentUrl));
                loadExtent(ws, extentUrl);
                return false;
            };
            extentLogic.loadAndCreateHtmlForWorkspace($(".container_data"), workspaceId)
                .done(function (data) {
                createTitle(workspaceId);
            })
                .fail(function () {
            });
        }
        Navigation.loadExtents = loadExtents;
        function loadExtent(workspaceId, extentUrl) {
            var extentLogic = new DMLayout.ExtentLayout();
            extentLogic.onItemSelected = function (ws, extentUrl, itemUrl) {
                navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.onItemCreated = function (ws, extentUrl, itemUrl) {
                navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.loadAndCreateHtmlForExtent($(".container_data"), workspaceId, extentUrl).done(function (data) {
                createTitle(workspaceId, extentUrl);
            });
        }
        Navigation.loadExtent = loadExtent;
        function navigateToItem(ws, extentUrl, itemUrl) {
            history.pushState({}, '', "#ws=" + encodeURIComponent(ws)
                + "&ext=" + encodeURIComponent(extentUrl)
                + "&item=" + encodeURIComponent(itemUrl));
            loadItem(ws, extentUrl, itemUrl);
        }
        Navigation.navigateToItem = navigateToItem;
        function loadItem(workspaceId, extentUrl, itemUrl) {
            var extentLogic = new DMLayout.ExtentLayout();
            createTitle(workspaceId, extentUrl, itemUrl);
            extentLogic.loadAndCreateHtmlForItem($(".container_data"), workspaceId, extentUrl, itemUrl);
        }
        Navigation.loadItem = loadItem;
    })(Navigation = exports.Navigation || (exports.Navigation = {}));
});
//# sourceMappingURL=datenmeister.js.map