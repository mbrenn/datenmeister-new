define(["require", "exports", "./datenmeister-view", "./datenmeister-viewresolver"], function (require, exports, DMView, DMViewResolver) {
    "use strict";
    exports.__esModule = true;
    var ViewPort = (function () {
        function ViewPort(container, layout) {
            this.container = container;
            this.layout = layout;
            this.isMasterView = true;
            this.viewStateHistory = new Array();
        }
        /**
         * Sets the view into the dom
         * @param view view to be set
         */
        ViewPort.prototype.setView = function (view) {
            var viewContainer = $(".dm-view", this.container);
            viewContainer.empty();
            viewContainer.append(view.load());
            var viewState = view.getViewState();
            if (viewState !== undefined && viewState !== null) {
                var ev = {
                    viewState: viewState
                };
                this.layout.throwViewPortChanged(ev);
            }
            view.viewport = this;
            this.addViewState(viewState);
            this.currentView = view;
        };
        ViewPort.prototype.gotoViewState = function (viewState) {
            if (viewState === undefined || viewState === null) {
                return;
            }
            var url = "#";
            if (viewState.workspace === undefined) {
                url += "ws={all}";
            }
            else if (viewState.extent === undefined) {
                url += "ws=" + encodeURIComponent(viewState.workspace);
            }
            else if (viewState.item === undefined) {
                url += "ws=" + encodeURIComponent(viewState.workspace) + "&ext=" + encodeURIComponent(viewState.extent) + "}";
            }
            else {
                url += "ws=" + encodeURIComponent(viewState.workspace) + "&ext=" + encodeURIComponent(viewState.extent) + "&item=" + encodeURIComponent(viewState.item);
            }
            if (viewState.isReadonly) {
                url += "&mode=readonly";
            }
            if (viewState.viewname !== undefined && viewState.viewname !== null) {
                url += "&view=" + encodeURIComponent(viewState.viewname);
            }
            history.pushState({}, "", url);
        };
        ViewPort.prototype.addViewState = function (viewState) {
            this.gotoViewState(viewState);
            this.viewStateHistory.push(viewState);
        };
        ViewPort.prototype.navigateBack = function () {
            this.viewStateHistory.pop();
            var viewState = this.viewStateHistory[this.viewStateHistory.length - 1];
            this.gotoViewState(viewState);
            DMViewResolver.resolveView(this, viewState);
        };
        ViewPort.prototype.gotoHome = function () {
            DMView.WorkspaceList.navigateToWorkspaces(this);
        };
        ViewPort.prototype.refresh = function () {
            //$(".dm-view", this.container).empty();
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
                DMView.WorkspaceList.navigateToWorkspaces(tthis);
                return false;
            });
            $(".link_extents", containerTitle)
                .click(function () {
                DMView.ExtentList.navigateToExtents(tthis, ws);
                return false;
            });
            $(".link_items", containerTitle)
                .click(function () {
                DMView.ItemList.navigateToItems(tthis, ws, extentUrl);
                return false;
            });
        };
        return ViewPort;
    }());
    exports.ViewPort = ViewPort;
});
//# sourceMappingURL=datenmeister-viewport.js.map