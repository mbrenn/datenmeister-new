define(["require", "exports", "./datenmeister-interfaces", "./datenmeister-viewport", "./datenmeister-view", "./datenmeister-dialogs", "./datenmeister-client", "./datenmeister-ribbon"], function (require, exports, DMI, DMViewPort, DMView, DMDialog, DMClient, DMRibbon) {
    "use strict";
    var Layout = (function () {
        function Layout(parent) {
            var _this = this;
            var tthis = this;
            this.parent = parent;
            this.pluginResults = new Array();
            this.mainViewPort = new DMViewPort.ViewPort($("#dm-viewport", this.parent), this);
            this.mainViewPort.onViewPortChanged = function (data) {
                _this.throwViewPortChanged(data);
                tthis.buildRibbons(data);
            };
        }
        Layout.prototype.refreshView = function () {
            if (this.onRefresh !== undefined && this.onRefresh !== null) {
                this.onRefresh();
            }
        };
        Layout.prototype.navigateToWorkspaces = function () {
            history.pushState({}, "", "#ws={all}");
            this.showWorkspaces();
        };
        Layout.prototype.navigateToView = function (view) {
            this.mainViewPort.setView(view);
        };
        Layout.prototype.navigateToExtents = function (workspaceId) {
            history.pushState({}, "", "#ws=" + encodeURIComponent(workspaceId));
            this.showExtents(workspaceId);
        };
        Layout.prototype.navigateToItems = function (ws, extentUrl, viewname) {
            var url = "#ws=" + encodeURIComponent(ws) + "&ext=" + encodeURIComponent(extentUrl);
            if (viewname !== undefined && viewname !== null) {
                url += "&view=" + encodeURIComponent(viewname);
            }
            history.pushState({}, "", url);
            this.showItems(ws, extentUrl, viewname);
        };
        Layout.prototype.navigateToItem = function (ws, extentUrl, itemUrl, viewname, settings) {
            var url = "#ws=" + encodeURIComponent(ws) + "&ext=" + encodeURIComponent(extentUrl) + "&item=" + encodeURIComponent(itemUrl);
            if (settings !== undefined && settings !== null) {
                if (settings.isReadonly) {
                    url += "&mode=readonly";
                }
            }
            if (viewname !== undefined && viewname !== null) {
                url += "&view=" + encodeURIComponent(viewname);
            }
            history.pushState({}, "", url);
            this.showItem(ws, extentUrl, itemUrl, viewname, settings);
        };
        Layout.prototype.exportExtent = function (ws, extentUrl) {
            window.open("/api/datenmeister/extent/extent_export_csv?ws=" + encodeURIComponent(ws) + "&extent=" + encodeURIComponent(extentUrl));
        };
        Layout.prototype.navigateToDialog = function (configuration) {
            var dialog = new DMView.DialogView(this);
            dialog.createDialog(configuration);
            this.mainViewPort.setView(dialog);
        };
        Layout.prototype.showWorkspaces = function () {
            var tthis = this;
            tthis.createTitle();
            var workbenchLogic = new DMView.WorkspaceView(this);
            workbenchLogic.onWorkspaceSelected = function (id) {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                tthis.navigateToExtents(id);
            };
            workbenchLogic.loadAndCreateHtmlForWorkbenchs();
            this.mainViewPort.setView(workbenchLogic);
        };
        Layout.prototype.showExtents = function (workspaceId) {
            var tthis = this;
            tthis.createTitle(workspaceId);
            var extentView = new DMView.ExtentView(this);
            extentView.onItemView = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItems(ws, extentUrl);
                return false;
            };
            extentView.loadAndCreateHtmlForWorkspace(workspaceId);
            this.mainViewPort.setView(extentView);
        };
        Layout.prototype.showItems = function (workspaceId, extentUrl, viewname) {
            var tthis = this;
            this.createTitle(workspaceId, extentUrl);
            var extentView = new DMView.ItemsOfExtentView(this);
            extentView.onItemEdit = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl);
            };
            extentView.onItemView = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl, undefined, { isReadonly: true });
            };
            extentView.onItemCreated = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl);
            };
            var query = new DMI.Api.ItemTableQuery();
            query.view = viewname;
            query.amount = 20;
            extentView.loadAndCreateHtmlForExtent(workspaceId, extentUrl, query);
            this.mainViewPort.setView(extentView);
        };
        Layout.prototype.showItem = function (workspaceId, extentUrl, itemUrl, viewname, settings) {
            var tthis = this;
            var itemView = new DMView.ItemView(this);
            itemView.onItemView = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl, undefined, { isReadonly: true });
            };
            this.createTitle(workspaceId, extentUrl, itemUrl);
            itemView.loadAndCreateHtmlForItem(workspaceId, extentUrl, itemUrl, settings);
            this.mainViewPort.setView(itemView);
        };
        Layout.prototype.createTitle = function (ws, extentUrl, itemUrl) {
            var tthis = this;
            var containerTitle = $(".container-title", this.parent);
            var ba = "&gt;&gt";
            if (ws === undefined) {
                containerTitle.text("Workspaces");
                this.onRefresh = function () {
                    tthis.showWorkspaces();
                    return false;
                };
            }
            else if (extentUrl === undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspace '<b>" + ws + "</b>'</a> " + ba + " Extents");
                this.onRefresh = function () {
                    tthis.showExtents(ws);
                    return false;
                };
            }
            else if (itemUrl == undefined) {
                containerTitle
                    .html("<a href='#' class='link_workspaces'> Workspace '<b>" + ws + "</b>'</a> " + ba + " <a href='#' class='link_extents'>Extent '<b>" + extentUrl + "</b>'</a> " + ba + " Items");
                this.onRefresh = function () {
                    tthis.showItems(ws, extentUrl);
                    return false;
                };
            }
            else {
                containerTitle
                    .html("<a href='#' class='link_workspaces'>Workspace '<b>" + ws + "</b>'</a> " + ba + " <a href='#' class='link_extents'>Extent '<b>" + extentUrl + "</b>'</a> " + ba + " <a href='#' class='link_items'>Items</a>");
                this.onRefresh = function () {
                    tthis.showItem(ws, extentUrl, itemUrl);
                    return false;
                };
            }
            $(".link_workspaces", containerTitle)
                .click(function () {
                tthis.navigateToWorkspaces();
                return false;
            });
            $(".link_extents", containerTitle)
                .click(function () {
                tthis.navigateToExtents(ws);
                return false;
            });
            $(".link_items", containerTitle)
                .click(function () {
                tthis.showItems(ws, extentUrl);
                return false;
            });
        };
        Layout.prototype.setStatus = function (statusDom) {
            var dom = $(".dm-statusline");
            dom.empty();
            dom.append(statusDom);
        };
        Layout.prototype.throwViewPortChanged = function (data) {
            if (data !== undefined && data != null) {
                data.layout = this;
                data.navigation = this;
            }
            this.lastLayoutConfiguration = data;
            if (this.onViewPortChanged !== undefined) {
                this.onViewPortChanged(data);
            }
            if (this.pluginResults !== undefined) {
                for (var n in this.pluginResults) {
                    var pluginResult = this.pluginResults[n];
                    pluginResult.onViewPortChanged(data);
                }
            }
        };
        Layout.prototype.renavigate = function () {
            this.throwViewPortChanged(this.lastLayoutConfiguration);
        };
        Layout.prototype.gotoHome = function () {
            this.navigateToExtents("Data");
        };
        Layout.prototype.getRibbon = function () {
            if (this.ribbon === null || this.ribbon === undefined) {
                var domRibbon = $(".datenmeister-ribbon");
                this.ribbon = new DMRibbon.Ribbon(domRibbon);
            }
            return this.ribbon;
        };
        /**
            * Builds the ribbons for the layout
            * @param layout Layout to which the ribbons shall be built
            * @param changeEvent The event that the layout has been changed
            */
        Layout.prototype.buildRibbons = function (changeEvent) {
            var tthis = this;
            var ribbon = tthis.getRibbon();
            ribbon.clear();
            var tabFile = ribbon.getOrAddTab("File");
            tabFile.addIcon("Home", "img/icons/home", function () { tthis.gotoHome(); });
            tabFile.addIcon("Refresh", "img/icons/refresh_update", function () { tthis.refreshView(); });
            tabFile.addIcon("Workspaces", "img/icons/database", function () { tthis.navigateToWorkspaces(); });
            tabFile.addIcon("Add Workspace", "img/icons/database-add", function () { DMDialog.showDialogNewWorkspace(tthis); });
            if (changeEvent !== null && changeEvent !== undefined && changeEvent.workspace !== undefined) {
                // Ok, we have a workspace
                tabFile.addIcon("Delete Workspace", "img/icons/database-delete", function () {
                    DMClient.WorkspaceApi.deleteWorkspace(changeEvent.workspace)
                        .done(function () { return tthis.navigateToWorkspaces(); });
                });
                tabFile.addIcon("Create Extent", "img/icons/folder_open-new", function () {
                    DMDialog.showNavigationForNewExtents(tthis, changeEvent.workspace);
                });
                tabFile.addIcon("Add Extent", "img/icons/folder_open-add", function () {
                    DMDialog.showDialogAddCsvExtent(tthis, changeEvent.workspace);
                });
                if (changeEvent.extent !== undefined) {
                    tabFile.addIcon("Delete Extent", "img/icons/folder_open-delete", function () {
                        DMClient.ExtentApi.deleteExtent(changeEvent.workspace, changeEvent.extent)
                            .done(function () { return tthis.navigateToExtents(changeEvent.workspace); });
                    });
                    tabFile.addIcon("Export Extent", "img/icons/folder_open-download", function () {
                        tthis.exportExtent(changeEvent.workspace, changeEvent.extent);
                    });
                }
                tabFile.addIcon("Add ZipCodes", "img/icons/folder_open-mail", function () {
                    DMClient.ExampleApi.addZipCodes(changeEvent.workspace).done(function () { return tthis.refreshView(); });
                });
            }
            tabFile.addIcon("Close", "img/icons/close_window", function () { window.close(); });
        };
        return Layout;
    }());
    exports.Layout = Layout;
});
//# sourceMappingURL=datenmeister-layout.js.map