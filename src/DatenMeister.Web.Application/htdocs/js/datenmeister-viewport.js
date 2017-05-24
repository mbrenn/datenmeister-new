define(["require", "exports", "./datenmeister-view"], function (require, exports, DMView) {
    "use strict";
    exports.__esModule = true;
    var ViewPort = (function () {
        function ViewPort(container, layout) {
            this.container = container;
            this.layout = layout;
            this.isMasterView = true;
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
                    viewState: viewState
                };
                this.layout.throwViewPortChanged(ev);
            }
            view.viewport = this;
            this.addViewState(view.getViewState());
            this.currentView = view;
        };
        ViewPort.prototype.addViewState = function (viewState) {
            if (viewState.workspace === undefined) {
                history.pushState({}, "", "#ws={all}");
            }
        };
        ViewPort.prototype.navigateToView = function (view) {
            this.setView(view);
        };
        ViewPort.prototype.showExtents = function (viewport, workspaceId) {
            var tthis = this;
            tthis.createTitle(workspaceId);
            var extentView = new DMView.ExtentView(this);
            extentView.onItemView = function (ws, extentUrl, itemUrl) {
                DMView.navigateToItems(viewport, ws, extentUrl);
                return false;
            };
            extentView.load(workspaceId);
            this.setView(extentView);
        };
        ViewPort.prototype.showItem = function (viewport, workspaceId, extentUrl, itemUrl, viewname, settings) {
            var tthis = this;
            var itemView = new DMView.ItemView(this);
            itemView.onItemView = function (ws, extentUrl, itemUrl) {
                DMView.navigateToItem(viewport, ws, extentUrl, itemUrl, undefined, { isReadonly: true });
            };
            this.createTitle(workspaceId, extentUrl, itemUrl);
            itemView.load(workspaceId, extentUrl, itemUrl, settings);
            this.setView(itemView);
        };
        ViewPort.prototype.gotoHome = function () {
            DMView.navigateToWorkspaces(this);
        };
        ViewPort.prototype.refresh = function () {
            if (this.currentView !== undefined && this.currentView !== null) {
                this.currentView.refresh();
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
                viewState: data
            };
            if (this.onViewPortChanged !== undefined) {
                this.onViewPortChanged(ev);
            }
            this.createTitle();
            this.layout.throwViewPortChanged(ev);
        };
        ViewPort.prototype.createTitle = function (ws, extentUrl, itemUrl) {
            var _this = this;
            var tthis = this;
            var containerTitle = $(".container-title", this.container);
            var ba = "&gt;&gt";
            if (ws === undefined) {
                containerTitle.text("Workspaces");
            }
            else if (extentUrl === undefined) {
                containerTitle.html("<a href='#' class='link_workspaces'>Workspace '<b>" + ws + "</b>'</a> " + ba + " Extents");
            }
            else if (itemUrl == undefined) {
                containerTitle
                    .html("<a href='#' class='link_workspaces'> Workspace '<b>" + ws + "</b>'</a> " + ba + " <a href='#' class='link_extents'>Extent '<b>" + extentUrl + "</b>'</a> " + ba + " Items");
            }
            else {
                containerTitle
                    .html("<a href='#' class='link_workspaces'>Workspace '<b>" + ws + "</b>'</a> " + ba + " <a href='#' class='link_extents'>Extent '<b>" + extentUrl + "</b>'</a> " + ba + " <a href='#' class='link_items'>Items</a>");
            }
            $(".link_workspaces", containerTitle)
                .click(function () {
                DMView.navigateToWorkspaces(_this);
                return false;
            });
            $(".link_extents", containerTitle)
                .click(function () {
                DMView.navigateToExtents(_this, ws);
                return false;
            });
            $(".link_items", containerTitle)
                .click(function () {
                DMView.navigateToItems(_this, ws, extentUrl);
                return false;
            });
        };
        return ViewPort;
    }());
    exports.ViewPort = ViewPort;
});
//# sourceMappingURL=datenmeister-viewport.js.map