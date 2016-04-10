define(["require", "exports", "datenmeister-interfaces", "datenmeister-view", "datenmeister-tables"], function (require, exports, DMI, DMView, DMTables) {
    "use strict";
    (function (PageType) {
        PageType[PageType["Workspaces"] = 0] = "Workspaces";
        PageType[PageType["Extents"] = 1] = "Extents";
        PageType[PageType["Items"] = 2] = "Items";
        PageType[PageType["ItemDetail"] = 3] = "ItemDetail";
        PageType[PageType["Dialog"] = 4] = "Dialog";
    })(exports.PageType || (exports.PageType = {}));
    var PageType = exports.PageType;
    var Layout = (function () {
        function Layout(parent) {
            this.parent = parent;
        }
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
        Layout.prototype.navigateToItems = function (ws, extentUrl) {
            history.pushState({}, "", "#ws=" + encodeURIComponent(ws)
                + "&ext=" + encodeURIComponent(extentUrl));
            this.showItems(ws, extentUrl);
        };
        Layout.prototype.navigateToItem = function (ws, extentUrl, itemUrl, settings) {
            var url = "#ws=" + encodeURIComponent(ws)
                + "&ext=" + encodeURIComponent(extentUrl)
                + "&item=" + encodeURIComponent(itemUrl);
            if (settings !== undefined && settings !== null) {
                if (settings.isReadonly) {
                    url += "&mode=readonly";
                }
            }
            history.pushState({}, "", url);
            this.showItem(ws, extentUrl, itemUrl, settings);
        };
        Layout.prototype.navigateToDialog = function (configuration) {
            var _this = this;
            var oldPageType = this.currentPageType;
            var domTable = $(".data-dialog", this.parent);
            var value = new DMI.Table.DataTableItem();
            var tableConfiguration = new DMTables.ItemContentConfiguration();
            tableConfiguration.autoProperties = false;
            tableConfiguration.columns = configuration.columns;
            tableConfiguration.isReadOnly = false;
            tableConfiguration.supportNewProperties = false;
            tableConfiguration.onCancelForm = function () {
                _this.switchLayout(oldPageType);
                if (configuration.onCancelForm !== undefined) {
                    configuration.onCancelForm();
                }
            };
            tableConfiguration.onOkForm = function () {
                _this.switchLayout(oldPageType);
                if (configuration.onOkForm !== undefined) {
                    configuration.onOkForm(value);
                }
            };
            var itemTable = new DMTables.ItemContentTable(value, tableConfiguration);
            itemTable.show(domTable);
            this.switchLayout(PageType.Dialog, configuration.ws, configuration.extent);
        };
        Layout.prototype.showWorkspaces = function () {
            var tthis = this;
            tthis.switchLayout(PageType.Workspaces);
            tthis.createTitle();
            var workbenchLogic = new DMView.WorkspaceView();
            workbenchLogic.onWorkspaceSelected = function (id) {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                tthis.navigateToExtents(id);
            };
            workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".data-workspaces", this.parent));
        };
        Layout.prototype.showExtents = function (workspaceId) {
            var tthis = this;
            tthis.switchLayout(PageType.Extents, workspaceId);
            tthis.createTitle(workspaceId);
            var extentLogic = new DMView.ExtentView();
            extentLogic.onExtentSelected = function (ws, extentUrl) {
                tthis.navigateToItems(ws, extentUrl);
                return false;
            };
            extentLogic.loadAndCreateHtmlForWorkspace($(".data-extents", this.parent), workspaceId);
        };
        Layout.prototype.showItems = function (workspaceId, extentUrl) {
            var tthis = this;
            this.switchLayout(PageType.Items, workspaceId, extentUrl);
            this.createTitle(workspaceId, extentUrl);
            var extentLogic = new DMView.ExtentView(this);
            extentLogic.onItemEdit = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.onItemView = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl, { isReadonly: true });
            };
            extentLogic.onItemCreated = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl);
            };
            extentLogic.loadAndCreateHtmlForExtent($(".data-items", this.parent), workspaceId, extentUrl);
        };
        Layout.prototype.showItem = function (workspaceId, extentUrl, itemUrl, settings) {
            var tthis = this;
            this.switchLayout(PageType.ItemDetail, workspaceId, extentUrl, itemUrl);
            var extentLogic = new DMView.ItemView(this);
            extentLogic.onItemView = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl, { isReadonly: true });
            };
            this.createTitle(workspaceId, extentUrl, itemUrl);
            extentLogic.loadAndCreateHtmlForItem($(".data-itemdetail", this.parent), workspaceId, extentUrl, itemUrl, settings);
        };
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
        Layout.prototype.switchLayout = function (pageType, workspace, extent, item) {
            $(".only-workspaces").hide();
            $(".only-extents").hide();
            $(".only-items").hide();
            $(".only-itemdetail").hide();
            $(".only-dialog").hide();
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
            else if (pageType === PageType.Dialog) {
                $(".only-dialog").show();
            }
            this.currentPageType = pageType;
            this.throwLayoutChangedEvent({
                type: pageType,
                workspace: workspace,
                extent: extent,
                item: item
            });
        };
        Layout.prototype.setStatus = function (statusDom) {
            var dom = $(".dm-statusline");
            dom.empty();
            dom.append(statusDom);
        };
        Layout.prototype.throwLayoutChangedEvent = function (data) {
            if (this.onLayoutChanged !== undefined) {
                this.onLayoutChanged(data);
            }
        };
        return Layout;
    }());
    exports.Layout = Layout;
});
//# sourceMappingURL=datenmeister-layout.js.map