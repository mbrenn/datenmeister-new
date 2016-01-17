/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
define(["require", "exports", "datenmeister-helper", "datenmeister-view", "datenmeister-ribbon"], function (require, exports, DMHelper, DMLayout, DMRibbon) {
    function start() {
        $(document).ready(function () {
            window.onpopstate = function (ev) {
                parseAndNavigateToWindowLocation();
            };
            parseAndNavigateToWindowLocation();
        });
    }
    exports.start = start;
    function buildRibbons(layout) {
        var domRibbon = $(".datenmeister-ribbon");
        var ribbon = new DMRibbon.Ribbon(domRibbon);
        var tab1 = ribbon.addTab("File");
        tab1.addIcon("Refresh", "img/icons/refresh_update", function () { layout.refreshView(); });
        tab1.addIcon("Workspaces", "img/icons/database", function () { layout.showWorkspaces(); });
        tab1.addIcon("Add Workspace", "img/icons/database-add", function () { alert("X"); });
        tab1.addIcon("Delete Workspace", "img/icons/database-delete", function () { alert("X"); });
        tab1.addIcon("Close", "img/icons/close_window", function () { window.close(); });
    }
    function parseAndNavigateToWindowLocation() {
        var ws = DMHelper.getParameterByNameFromHash("ws");
        var extentUrl = DMHelper.getParameterByNameFromHash("ext");
        var itemUrl = DMHelper.getParameterByNameFromHash("item");
        var layout = new Layout($("body"));
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
            layout.showItem(ws, extentUrl, itemUrl);
        }
        buildRibbons(layout);
    }
    exports.parseAndNavigateToWindowLocation = parseAndNavigateToWindowLocation;
    (function (PageType) {
        PageType[PageType["Workspaces"] = 0] = "Workspaces";
        PageType[PageType["Extents"] = 1] = "Extents";
        PageType[PageType["Items"] = 2] = "Items";
        PageType[PageType["ItemDetail"] = 3] = "ItemDetail";
    })(exports.PageType || (exports.PageType = {}));
    var PageType = exports.PageType;
    var Layout = (function () {
        function Layout(parent) {
            this.parent = parent;
        }
        Layout.prototype.createTitle = function (ws, extentUrl, itemUrl) {
            var tthis = this;
            var containerTitle = $(".container-title", this.parent);
            if (ws === undefined) {
                containerTitle.text("Workspaces");
                this.onRefresh = function () {
                    tthis.showWorkspaces();
                    return false;
                };
            }
            else if (extentUrl === undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - Extents");
                this.onRefresh = function () {
                    tthis.showExtents(ws);
                    return false;
                };
            }
            else if (itemUrl == undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - Items");
                this.onRefresh = function () {
                    tthis.showItems(ws, extentUrl);
                    return false;
                };
            }
            else {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - <a href='#' class='link_items'>Items</a>");
                this.onRefresh = function () {
                    tthis.showItem(ws, extentUrl, itemUrl);
                    return false;
                };
            }
            $(".link_workspaces", containerTitle).click(function () {
                tthis.navigateToWorkspaces();
                return false;
            });
            $(".link_extents", containerTitle).click(function () {
                tthis.navigateToExtents(ws);
                return false;
            });
            $(".link_items", containerTitle).click(function () {
                tthis.showItems(ws, extentUrl);
                return false;
            });
        };
        Layout.prototype.refreshView = function () {
            if (this.onRefresh !== undefined && this.onRefresh !== null) {
                this.onRefresh();
            }
        };
        Layout.prototype.navigateToWorkspaces = function () {
            history.pushState({}, "", "#");
            this.showWorkspaces();
        };
        Layout.prototype.navigateToExtents = function (workspaceId) {
            history.pushState({}, "", "#ws=" + encodeURIComponent(workspaceId));
            this.showExtents(workspaceId);
        };
        Layout.prototype.navigateToItem = function (ws, extentUrl, itemUrl) {
            history.pushState({}, "", "#ws=" + encodeURIComponent(ws)
                + "&ext=" + encodeURIComponent(extentUrl)
                + "&item=" + encodeURIComponent(itemUrl));
            this.showItem(ws, extentUrl, itemUrl);
        };
        Layout.prototype.showWorkspaces = function () {
            var tthis = this;
            tthis.switchLayout(PageType.Workspaces);
            tthis.createTitle();
            var workbenchLogic = new DMLayout.WorkspaceView();
            workbenchLogic.onWorkspaceSelected = function (id) {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                tthis.navigateToExtents(id);
            };
            workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".data-workspaces", this.parent));
        };
        Layout.prototype.showExtents = function (workspaceId) {
            var tthis = this;
            tthis.switchLayout(PageType.Extents);
            tthis.createTitle(workspaceId);
            var extentLogic = new DMLayout.ExtentView();
            extentLogic.onExtentSelected = function (ws, extentUrl) {
                history.pushState({}, "", "#ws=" + encodeURIComponent(ws)
                    + "&ext=" + encodeURIComponent(extentUrl));
                tthis.showItems(ws, extentUrl);
                return false;
            };
            extentLogic.loadAndCreateHtmlForWorkspace($(".data-extents", this.parent), workspaceId);
        };
        Layout.prototype.showItems = function (workspaceId, extentUrl) {
            var tthis = this;
            this.switchLayout(PageType.Items);
            this.createTitle(workspaceId, extentUrl);
            var extentLogic = new DMLayout.ExtentView(this);
            extentLogic.onItemSelected = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.onItemCreated = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.loadAndCreateHtmlForExtent($(".data-items", this.parent), workspaceId, extentUrl);
        };
        Layout.prototype.showItem = function (workspaceId, extentUrl, itemUrl) {
            this.switchLayout(PageType.ItemDetail);
            var extentLogic = new DMLayout.ItemView(this);
            this.createTitle(workspaceId, extentUrl, itemUrl);
            extentLogic.loadAndCreateHtmlForItem($(".data-itemdetail", this.parent), workspaceId, extentUrl, itemUrl);
        };
        Layout.prototype.switchLayout = function (pageType) {
            $(".only-workspaces").hide();
            $(".only-extents").hide();
            $(".only-items").hide();
            $(".only-itemdetail").hide();
            if (pageType === PageType.Workspaces) {
                $(".only-workspaces").show();
            }
            else if (pageType === PageType.Extents) {
                $(".only-extents").show();
            }
            else if (pageType === PageType.Items) {
                $(".only-items").show();
            }
            else if (pageType === PageType.ItemDetail) {
                $(".only-itemdetail").show();
            }
        };
        Layout.prototype.setStatus = function (statusDom) {
            var dom = $(".dm-statusline");
            dom.empty();
            dom.append(statusDom);
        };
        return Layout;
    })();
    exports.Layout = Layout;
});
//# sourceMappingURL=datenmeister.js.map