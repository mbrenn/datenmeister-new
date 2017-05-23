import * as DMI from "./datenmeister-interfaces";
import * as DMViewPort from "./datenmeister-viewport";
import * as DMDialog from "./datenmeister-dialogs";
import * as DMClient from "./datenmeister-client";
import * as DMRibbon from "./datenmeister-ribbon";

export class ApplicationWindow implements DMI.Api.ILayout {
    pluginResults: Array<DMI.Plugin.IPluginResult>;
    mainViewPort: DMViewPort.ViewPort;
    parent: JQuery;
    ribbon: DMRibbon.Ribbon;

    constructor(parent: JQuery) {
        var tthis = this;
        this.parent = parent;
        this.pluginResults = new Array<DMI.Plugin.IPluginResult>();
        this.mainViewPort = new DMViewPort.ViewPort($("#dm-viewport", this.parent), this);
        this.mainViewPort.onViewPortChanged = data => {
            this.throwViewPortChanged(data);
            tthis.buildRibbons(data);
        };
    }

    throwViewPortChanged(ev: DMI.Api.ILayoutChangedEvent) {

        if (this.pluginResults !== undefined) {
            for (var n in this.pluginResults) {
                var pluginResult = this.pluginResults[n];
                if (pluginResult.onViewPortChanged !== undefined &&
                    pluginResult.onViewPortChanged !== null) {
                    pluginResult.onViewPortChanged(ev);
                }

                if (pluginResult.onRibbonUpdate !== undefined &&
                    pluginResult.onRibbonUpdate !== null) {

                    var ribbonEv =
                    {
                        layout: this,
                        navigation: ev.navigation,
                        viewState: ev.viewState
                    };
                    pluginResult.onRibbonUpdate(ribbonEv);
                }
            }
        }

        this.buildRibbons(ev);
    }

    lastLayoutConfiguration: DMI.Api.ILayoutChangedEvent;

    getRibbon(): DMRibbon.Ribbon {
        if (this.ribbon === null || this.ribbon === undefined) {
            var domRibbon = $(".datenmeister-ribbon");
            this.ribbon = new DMRibbon.Ribbon(domRibbon);
        }
        
        return this.ribbon;
    }
    
    /**
        * Builds the ribbons for the layout
        * @param layout ApplicationWindow to which the ribbons shall be built
        * @param changeEvent The event that the layout has been changed
        */
     buildRibbons(changeEvent: DMI.Api.ILayoutChangedEvent): void {
        var tthis = this;
        var ribbon = tthis.getRibbon();
        ribbon.clear();
        var tabFile = ribbon.getOrAddTab("File");

        tabFile.addIcon("Home", "img/icons/home", () => { tthis.mainViewPort.gotoHome(); });
        tabFile.addIcon("Refresh", "img/icons/refresh_update", () => { tthis.mainViewPort.refresh(); });

        tabFile.addIcon("Workspaces", "img/icons/database", () => { tthis.mainViewPort.navigateToWorkspaces(); });
        tabFile.addIcon("Add Workspace", "img/icons/database-add", () => { DMDialog.showDialogNewWorkspace(tthis.mainViewPort); });

        if (changeEvent !== null && changeEvent !== undefined && changeEvent.viewState.workspace !== undefined) {
            // Ok, we have a workspace
            tabFile.addIcon("Delete Workspace", "img/icons/database-delete", () => {
                DMClient.WorkspaceApi.deleteWorkspace(changeEvent.viewState.workspace)
                    .done(() => tthis.mainViewPort.navigateToWorkspaces());
            });

            tabFile.addIcon("Create Extent", "img/icons/folder_open-new", () => {
                DMDialog.showNavigationForNewExtents(tthis.mainViewPort, changeEvent.viewState.workspace);
            });

            tabFile.addIcon("Add Extent", "img/icons/folder_open-add", () => {
                DMDialog.showDialogAddCsvExtent(tthis.mainViewPort, changeEvent.viewState.workspace);
            });

            if (changeEvent.viewState.extent !== undefined) {
                tabFile.addIcon("Delete Extent", "img/icons/folder_open-delete", () => {
                    DMClient.ExtentApi.deleteExtent(changeEvent.viewState.workspace, changeEvent.viewState.extent)
                        .done(() => tthis.mainViewPort.navigateToExtents(changeEvent.viewState.workspace));
                });

                tabFile.addIcon("Export Extent", "img/icons/folder_open-download", () => {
                    tthis.mainViewPort.exportExtent(changeEvent.viewState.workspace, changeEvent.viewState.extent);
                });
            }

            tabFile.addIcon("Add ZipCodes", "img/icons/folder_open-mail", () => {
                DMClient.ExampleApi.addZipCodes(changeEvent.viewState.workspace).done(
                    () => tthis.mainViewPort.refresh());
            });
        }

        tabFile.addIcon("Close", "img/icons/close_window", () => { window.close(); });
    }

}