define(["require", "exports", "./datenmeister-clientinterface", "./datenmeister-view"], function (require, exports, DMCI, DMView) {
    "use strict";
    exports.__esModule = true;
    var ViewPort = (function () {
        function ViewPort(container, layout) {
            this.container = container;
            this.layout = layout;
        }
        /**
         * Sets the view into the dom
         * @param view view to be set
         */
        ViewPort.prototype.setView = function (view) {
            var viewContainer = $(".dm-view", this.container);
            viewContainer.empty();
            viewContainer.append(view.getContent());
            var viewState = view.getViewState();
            if (viewState !== undefined && viewState !== null) {
                var ev = {
                    navigation: this,
                    viewState: viewState
                };
                this.layout.throwViewPortChanged(ev);
            }
            view.viewport = this;
        };
        ViewPort.prototype.navigateToWorkspaces = function () {
            history.pushState({}, "", "#ws={all}");
            this.showWorkspaces();
        };
        ViewPort.prototype.navigateToView = function (view) {
            this.setView(view);
        };
        ViewPort.prototype.navigateToExtents = function (workspaceId) {
            history.pushState({}, "", "#ws=" + encodeURIComponent(workspaceId));
            this.showExtents(workspaceId);
        };
        ViewPort.prototype.navigateToItems = function (ws, extentUrl, viewname) {
            var url = "#ws=" + encodeURIComponent(ws) + "&ext=" + encodeURIComponent(extentUrl);
            if (viewname !== undefined && viewname !== null) {
                url += "&view=" + encodeURIComponent(viewname);
            }
            history.pushState({}, "", url);
            this.showItems(ws, extentUrl, viewname);
        };
        ViewPort.prototype.navigateToItem = function (ws, extentUrl, itemUrl, viewname, settings) {
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
        ViewPort.prototype.exportExtent = function (ws, extentUrl) {
            window.open("/api/datenmeister/extent/extent_export_csv?ws=" + encodeURIComponent(ws) + "&extent=" + encodeURIComponent(extentUrl));
        };
        ViewPort.prototype.navigateToDialog = function (configuration) {
            var dialog = new DMView.DialogView(this);
            dialog.createDialog(configuration);
            this.setView(dialog);
        };
        ViewPort.prototype.showWorkspaces = function () {
            var tthis = this;
            tthis.createTitle();
            var workbenchLogic = new DMView.WorkspaceView(this);
            workbenchLogic.onWorkspaceSelected = function (id) {
                // Loads the extent of the workspace, if the user has clicked on one of the workbenches
                tthis.navigateToExtents(id);
            };
            workbenchLogic.loadAndCreateHtmlForWorkbenchs();
            this.setView(workbenchLogic);
        };
        ViewPort.prototype.showExtents = function (workspaceId) {
            var tthis = this;
            tthis.createTitle(workspaceId);
            var extentView = new DMView.ExtentView(this);
            extentView.onItemView = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItems(ws, extentUrl);
                return false;
            };
            extentView.loadAndCreateHtmlForWorkspace(workspaceId);
            this.setView(extentView);
        };
        ViewPort.prototype.showItems = function (workspaceId, extentUrl, viewname) {
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
            var query = new DMCI.Out.ItemTableQuery();
            query.view = viewname;
            query.amount = 20;
            extentView.loadAndCreateHtmlForExtent(workspaceId, extentUrl, query);
            this.setView(extentView);
        };
        ViewPort.prototype.showItem = function (workspaceId, extentUrl, itemUrl, viewname, settings) {
            var tthis = this;
            var itemView = new DMView.ItemView(this);
            itemView.onItemView = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl, undefined, { isReadonly: true });
            };
            this.createTitle(workspaceId, extentUrl, itemUrl);
            itemView.loadAndCreateHtmlForItem(workspaceId, extentUrl, itemUrl, settings);
            this.setView(itemView);
        };
        ViewPort.prototype.gotoHome = function () {
            this.navigateToExtents("Data");
        };
        ViewPort.prototype.renavigate = function () {
            this.throwViewPortChanged(this.viewState);
        };
        ViewPort.prototype.refresh = function () {
            if (this.onRefresh !== undefined && this.onRefresh !== null) {
                this.onRefresh();
            }
        };
        ViewPort.prototype.setStatus = function (statusDom) {
            var dom = $(".dm-statusline");
            dom.empty();
            dom.append(statusDom);
        };
        ViewPort.prototype.throwViewPortChanged = function (data) {
            this.viewState = data;
            var ev = {
                navigation: this,
                viewState: data
            };
            if (this.onViewPortChanged !== undefined) {
                this.onViewPortChanged(ev);
            }
            this.layout.throwViewPortChanged(ev);
        };
        ViewPort.prototype.createTitle = function (ws, extentUrl, itemUrl) {
            var tthis = this;
            var containerTitle = $(".container-title", this.container);
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
        return ViewPort;
    }());
    exports.ViewPort = ViewPort;
});
//# sourceMappingURL=datenmeister-viewport.js.map