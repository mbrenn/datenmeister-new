
import DMI = require("./datenmeister-interfaces");
import DMClient = require("./datenmeister-client");
import DMViewPort = require("./datenmeister-viewport");
import DMView = require("./datenmeister-view");
import * as DMRibbon from "./datenmeister-ribbon";


export class Layout implements DMI.Api.ILayout {
    pluginResults: Array<DMI.Api.IPluginResult>;
    mainViewPort : DMViewPort.ViewPort;
    parent: JQuery;
    onRefresh: () => void;
    onViewPortChanged: (data: DMI.Api.ILayoutChangedEvent) => void;
    currentLayoutInformation: DMI.Api.ILayoutChangedEvent;
    ribbon: DMRibbon.Ribbon;

    constructor(parent: JQuery) {
        this.parent = parent;
        this.pluginResults = new Array<DMI.Api.IPluginResult>();
        this.mainViewPort = new DMViewPort.ViewPort($("#dm-viewport", this.parent), this);
    }

    refreshView() : void {
        if (this.onRefresh !== undefined && this.onRefresh !== null) {
            this.onRefresh();
        }
    }

    navigateToWorkspaces() {
        history.pushState({}, "", "#ws={all}");
        this.showWorkspaces();
    }

    navigateToExtents(workspaceId: string) {
        history.pushState({}, "", `#ws=${encodeURIComponent(workspaceId)}`);
        this.showExtents(workspaceId);
    }

    navigateToItems(ws: string, extentUrl: string, viewname?: string) {
        var url = `#ws=${encodeURIComponent(ws)}&ext=${encodeURIComponent(extentUrl)}`;
        if (viewname !== undefined && viewname !== null) {
            url += `&view=${encodeURIComponent(viewname)}`;
        }

        history.pushState({}, "", url);

        this.showItems(ws, extentUrl, viewname);
    }

    navigateToItem(ws: string, extentUrl: string, itemUrl: string, viewname?: string, settings?: DMI.View.IItemViewSettings) {
        var url = `#ws=${encodeURIComponent(ws)}&ext=${encodeURIComponent(extentUrl)}&item=${encodeURIComponent(itemUrl)}`;

        if (settings !== undefined && settings !== null) {
            if (settings.isReadonly) {
                url += "&mode=readonly";
            }
        }

        if (viewname !== undefined && viewname !== null) {
            url += `&view=${encodeURIComponent(viewname)}`;
        }

        history.pushState({}, "", url);
        this.showItem(ws, extentUrl, itemUrl, viewname, settings);
    }

    exportExtent(ws: string, extentUrl: string) {
        window.open(
            `/api/datenmeister/extent/extent_export_csv?ws=${encodeURIComponent(ws)}&extent=${encodeURIComponent(extentUrl)}`);
    }

    navigateToDialog(configuration: DMI.Api.DialogConfiguration) {
        var dialog = new DMView.DialogView(this);
        dialog.createDialog(configuration);
        this.mainViewPort.setView(dialog);
    }

    showWorkspaces() {
        var tthis = this;
        tthis.createTitle();

        var workbenchLogic = new DMView.WorkspaceView(this);
        workbenchLogic.onWorkspaceSelected = (id: string) => {
            // Loads the extent of the workspace, if the user has clicked on one of the workbenches
            tthis.navigateToExtents(id);
        };

        workbenchLogic.loadAndCreateHtmlForWorkbenchs();
        this.mainViewPort.setView(workbenchLogic);
    }

    showExtents(workspaceId: string) {
        var tthis = this;
        tthis.createTitle(workspaceId);
        var extentView = new DMView.ExtentView(this);
        extentView.onExtentSelected = (ws: string, extentUrl: string) => {
            tthis.navigateToItems(ws, extentUrl);
            return false;
        };

        extentView.loadAndCreateHtmlForWorkspace(workspaceId);
        this.mainViewPort.setView(extentView);
    }

    showItems(workspaceId: string, extentUrl: string, viewname?: string) {
        var tthis = this;

        this.createTitle(workspaceId, extentUrl);
        var extentView = new DMView.ExtentView(this);
        extentView.onItemEdit = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentView.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl, undefined, { isReadonly: true });
        };

        extentView.onItemCreated = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        var query = new DMI.PostModels.ItemInExtentQuery();
        query.view = viewname;

        extentView.loadAndCreateHtmlForExtent(workspaceId, extentUrl, query);
        this.mainViewPort.setView(extentView);
    }

    showItem(workspaceId: string,
        extentUrl: string,
        itemUrl: string,
        viewname?: string,
        settings?: DMI.View.IItemViewSettings) {
        var tthis = this;

        var itemView = new DMView.ItemView(this);

        itemView.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl, undefined, { isReadonly: true });
        };

        this.createTitle(workspaceId, extentUrl, itemUrl);
        itemView.loadAndCreateHtmlForItem(
            workspaceId,
            extentUrl,
            itemUrl,
            settings);
        this.mainViewPort.setView(itemView);
    }

    createTitle(ws?: string, extentUrl?: string, itemUrl?: string) {
        var tthis = this;
        var containerTitle = $(".container-title", this.parent);
        var ba = "&gt;&gt";
        if (ws === undefined) {
            containerTitle.text("Workspaces");
            this.onRefresh = () => {
                tthis.showWorkspaces();
                return false;
            };
        } else if (extentUrl === undefined) {
            containerTitle.html(
                `<a href='#' class='link_workspaces'>Workspace '<b>${ws}</b>'</a> ${ba} Extents`);
            this.onRefresh = () => {
                tthis.showExtents(ws);
                return false;
            };
        } else if (itemUrl == undefined) {
            containerTitle
                .html(
                    `<a href='#' class='link_workspaces'> Workspace '<b>${ws}</b>'</a> ${ba
                    } <a href='#' class='link_extents'>Extent '<b>${extentUrl}</b>'</a> ${ba
                    } Items`);
            this.onRefresh = () => {
                tthis.showItems(ws, extentUrl);
                return false;
            };
        } else {
            containerTitle
                .html(`<a href='#' class='link_workspaces'>Workspace '<b>${ws}</b>'</a> ${ba
                    } <a href='#' class='link_extents'>Extent '<b>${extentUrl}</b>'</a> ${ba
                    } <a href='#' class='link_items'>Items</a>`);
            this.onRefresh = () => {
                tthis.showItem(ws, extentUrl, itemUrl);
                return false;
            };
        }

        $(".link_workspaces", containerTitle)
            .click(() => {
                tthis.navigateToWorkspaces();
                return false;
            });
        $(".link_extents", containerTitle)
            .click(() => {
                tthis.navigateToExtents(ws);
                return false;
            });
        $(".link_items", containerTitle)
            .click(() => {
                tthis.showItems(ws, extentUrl);
                return false;
            });
    }

    showDialogNewWorkspace(): void {
        var tthis = this;
        var configuration = new DMI.Api.DialogConfiguration();

        configuration.onOkForm = data => {
            DMClient.WorkspaceApi.createWorkspace(
                {
                    name: data.v["name"],
                    annotation: data.v["annotation"]
                })
                .done(() => tthis.navigateToWorkspaces());
        };

        configuration.addColumn(new DMI.Table.DataField("Title", "name"));
        var annotationColumn = new DMI.Table.TextDataField("Annotation", "annotation");
        annotationColumn.lineHeight = 4;
        configuration.addColumn(annotationColumn);

        tthis.navigateToDialog(configuration);
    }
    
    showNavigationForNewExtents(workspace: string) {
        var tthis = this;
        var view = new DMView.NavigationView(this);

        view.addLink("New CSV Extent",
        () => {
            tthis.showDialogNewCsvExtent(workspace);
        });
        
        view.addLink("New XmlExtent",
        () => {
            tthis.showDialogNewXmiExtent(workspace);
        });

        this.mainViewPort.setView(view);
    }

    showDialogNewCsvExtent(workspace: string) {
        var tthis = this;
        var configuration = new DMI.Api.DialogConfiguration();

        configuration.onOkForm = data => {
            DMClient.ExtentApi.createExtent(
                {
                    type: "csv",
                    workspace: data.v["workspace"],
                    contextUri: data.v["contextUri"],
                    filename: data.v["filename"],
                    columns: data.v["columns"]
                })
                .done(() => tthis.navigateToExtents(data.v["workspace"]));
        };

        configuration.addColumn(new DMI.Table.DataField("Workspace", "workspace").withDefaultValue(workspace));
        configuration.addColumn(new DMI.Table.DataField("URI", "contextUri").withDefaultValue("dm:///"));
        configuration.addColumn(new DMI.Table.DataField("Filename", "filename"));
        configuration.addColumn(new DMI.Table.DataField("Columns", "columns").withDefaultValue("Column1,Column2"));
        configuration.ws = workspace;

        tthis.navigateToDialog(configuration);
    }

    showDialogAddCsvExtent(workspace: string) {
        var tthis = this;
        var configuration = new DMI.Api.DialogConfiguration();

        configuration.onOkForm = data => {
            DMClient.ExtentApi.addExtent(
                {
                    type: "csv",
                    workspace: data.v["workspace"],
                    contextUri: data.v["contextUri"],
                    filename: data.v["filename"]
                })
                .done(() => tthis.navigateToExtents(data.v["workspace"]));
        };

        configuration.addColumn(new DMI.Table.TextDataField("Workspace", "workspace").withDefaultValue(workspace));
        configuration.addColumn(new DMI.Table.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
        configuration.addColumn(new DMI.Table.TextDataField("Filename", "filename"));
        configuration.ws = workspace;

        tthis.navigateToDialog(configuration);
    }

    showDialogNewXmiExtent(workspace: string) {
        var tthis = this;
        var configuration = new DMI.Api.DialogConfiguration();
        configuration.onOkForm = data => {
            DMClient.ExtentApi.createExtent(
                {
                    type: "xmi",
                    workspace: workspace,
                    contextUri: data.v["contextUri"],
                    name: data.v["name"]
                })
                .done(() => tthis.navigateToExtents(data.v["workspace"]));
        };

        configuration.addColumn(new DMI.Table.TextDataField("Name", "name").withDefaultValue("name"));
        configuration.addColumn(new DMI.Table.TextDataField("Workspace", "workspace").withDefaultValue(workspace).asReadOnly());
        configuration.addColumn(new DMI.Table.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));

        tthis.navigateToDialog(configuration);
    }

    setStatus(statusDom: JQuery): void {
        var dom = $(".dm-statusline");
        dom.empty();
        dom.append(statusDom);
    }

    lastLayoutConfiguration: DMI.Api.ILayoutChangedEvent;

    throwViewPortChanged(data: DMI.Api.ILayoutChangedEvent): void {
        if (data !== undefined && data != null) {
            data.layout = this;
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
    }

    renavigate(): void {
        this.throwViewPortChanged(this.lastLayoutConfiguration);
    }

    gotoHome(): void {
        this.navigateToExtents("Data");
    }

    getRibbon(): DMRibbon.Ribbon {
        if (this.ribbon === null || this.ribbon === undefined) {
            var domRibbon = $(".datenmeister-ribbon");
            this.ribbon = new DMRibbon.Ribbon(domRibbon);
        }
        
        return this.ribbon;
    }
}