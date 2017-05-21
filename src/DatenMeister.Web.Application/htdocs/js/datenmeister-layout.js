define(["require", "exports", "./datenmeister-viewport", "./datenmeister-dialogs", "./datenmeister-client", "./datenmeister-ribbon"], function (require, exports, DMViewPort, DMDialog, DMClient, DMRibbon) {
    "use strict";
    exports.__esModule = true;
    var ApplicationWindow = (function () {
        function ApplicationWindow(parent) {
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
        ApplicationWindow.prototype.throwViewPortChanged = function (ev) {
            if (this.pluginResults !== undefined) {
                for (var n in this.pluginResults) {
                    var pluginResult = this.pluginResults[n];
                    if (pluginResult.onViewPortChanged !== undefined &&
                        pluginResult.onViewPortChanged !== null) {
                        pluginResult.onViewPortChanged(ev);
                    }
                    if (pluginResult.onRibbonUpdate !== undefined &&
                        pluginResult.onRibbonUpdate !== null) {
                        var ribbonEv = {
                            layout: this,
                            navigation: ev.navigation,
                            viewState: ev.viewState
                        };
                        pluginResult.onRibbonUpdate(ribbonEv);
                    }
                }
            }
            this.buildRibbons(ev);
        };
        ApplicationWindow.prototype.getRibbon = function () {
            if (this.ribbon === null || this.ribbon === undefined) {
                var domRibbon = $(".datenmeister-ribbon");
                this.ribbon = new DMRibbon.Ribbon(domRibbon);
            }
            return this.ribbon;
        };
        /**
            * Builds the ribbons for the layout
            * @param layout ApplicationWindow to which the ribbons shall be built
            * @param changeEvent The event that the layout has been changed
            */
        ApplicationWindow.prototype.buildRibbons = function (changeEvent) {
            var tthis = this;
            var ribbon = tthis.getRibbon();
            ribbon.clear();
            var tabFile = ribbon.getOrAddTab("File");
            tabFile.addIcon("Home", "img/icons/home", function () { tthis.mainViewPort.gotoHome(); });
            tabFile.addIcon("Refresh", "img/icons/refresh_update", function () { tthis.mainViewPort.refresh(); });
            tabFile.addIcon("Workspaces", "img/icons/database", function () { tthis.mainViewPort.navigateToWorkspaces(); });
            tabFile.addIcon("Add Workspace", "img/icons/database-add", function () { DMDialog.showDialogNewWorkspace(tthis.mainViewPort); });
            if (changeEvent !== null && changeEvent !== undefined && changeEvent.viewState.workspace !== undefined) {
                // Ok, we have a workspace
                tabFile.addIcon("Delete Workspace", "img/icons/database-delete", function () {
                    DMClient.WorkspaceApi.deleteWorkspace(changeEvent.viewState.workspace)
                        .done(function () { return tthis.mainViewPort.navigateToWorkspaces(); });
                });
                tabFile.addIcon("Create Extent", "img/icons/folder_open-new", function () {
                    DMDialog.showNavigationForNewExtents(tthis.mainViewPort, changeEvent.viewState.workspace);
                });
                tabFile.addIcon("Add Extent", "img/icons/folder_open-add", function () {
                    DMDialog.showDialogAddCsvExtent(tthis.mainViewPort, changeEvent.viewState.workspace);
                });
                if (changeEvent.viewState.extent !== undefined) {
                    tabFile.addIcon("Delete Extent", "img/icons/folder_open-delete", function () {
                        DMClient.ExtentApi.deleteExtent(changeEvent.viewState.workspace, changeEvent.viewState.extent)
                            .done(function () { return tthis.mainViewPort.navigateToExtents(changeEvent.viewState.workspace); });
                    });
                    tabFile.addIcon("Export Extent", "img/icons/folder_open-download", function () {
                        tthis.mainViewPort.exportExtent(changeEvent.viewState.workspace, changeEvent.viewState.extent);
                    });
                }
                tabFile.addIcon("Add ZipCodes", "img/icons/folder_open-mail", function () {
                    DMClient.ExampleApi.addZipCodes(changeEvent.viewState.workspace).done(function () { return tthis.mainViewPort.refresh(); });
                });
            }
            tabFile.addIcon("Close", "img/icons/close_window", function () { window.close(); });
        };
        return ApplicationWindow;
    }());
    exports.ApplicationWindow = ApplicationWindow;
});
//# sourceMappingURL=datenmeister-layout.js.map