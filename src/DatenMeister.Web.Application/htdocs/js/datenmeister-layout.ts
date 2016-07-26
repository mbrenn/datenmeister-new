
import DMI = require("./datenmeister-interfaces");
import DMClient = require("./datenmeister-client");
import DMView = require("./datenmeister-view");
import DMTables = require("./datenmeister-tables");
import * as DMRibbon from "./datenmeister-ribbon";


export enum PageType {
    Workspaces,
    Extents,
    Items,
    ItemDetail,
    Dialog
}

export class Layout implements DMI.Api.ILayout {
    pluginResults: Array<DMI.Api.IPluginResult>;
    parent: JQuery;
    onRefresh: () => void;
    onLayoutChanged: (data: DMI.Api.ILayoutChangedEvent) => void;
    currentLayoutInformation: DMI.Api.ILayoutChangedEvent;
    ribbon: DMRibbon.Ribbon;

    constructor(parent: JQuery) {
        this.parent = parent;
        this.pluginResults = new Array<DMI.Api.IPluginResult>();
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

    navigateToItems(ws: string, extentUrl: string) {
        history.pushState({}, "", `#ws=${encodeURIComponent(ws)}&ext=${encodeURIComponent(extentUrl)}`);
        this.showItems(ws, extentUrl);
    }

    navigateToItem(ws: string, extentUrl: string, itemUrl: string, settings?: DMI.View.IItemViewSettings) {
        var url = `#ws=${encodeURIComponent(ws)}&ext=${encodeURIComponent(extentUrl)}&item=${encodeURIComponent(itemUrl)}`;

        if (settings !== undefined && settings !== null) {
            if (settings.isReadonly) {
                url += "&mode=readonly";
            }
        }

        history.pushState({}, "", url);
        this.showItem(ws, extentUrl, itemUrl, settings);
    }

    exportExtent(ws: string, extentUrl: string) {
        window.open(
            `/api/datenmeister/extent/extent_export?ws=${encodeURIComponent(ws)}&extent=${encodeURIComponent(extentUrl)}`);
    }

    navigateToDialog(configuration: DMI.Api.DialogConfiguration) {
        var oldPageType = this.currentLayoutInformation;

        var domTable = $(".data-dialog", this.parent);
        var value = new DMI.Table.DataTableItem();
        var tableConfiguration = new DMTables.ItemContentConfiguration();
        tableConfiguration.autoProperties = false;
        tableConfiguration.columns = configuration.columns;
        tableConfiguration.isReadOnly = false;
        tableConfiguration.supportNewProperties = false;
        tableConfiguration.onCancelForm = () => {
            this.switchLayout(oldPageType);

            if (configuration.onCancelForm !== undefined) {
                configuration.onCancelForm();
            }
        };

        tableConfiguration.onOkForm = () => {
            if (configuration.onOkForm !== undefined) {
                configuration.onOkForm(value);
            }
        }

        var itemTable = new DMTables.ItemContentTable(value, tableConfiguration);
        itemTable.show(domTable);

        this.switchLayout(
        {
            type: PageType.Dialog,
            workspace: configuration.ws,
            extent: configuration.extent
        });
    }

    showWorkspaces() {
        var tthis = this;
        tthis.switchLayout({ type: PageType.Workspaces });
        tthis.createTitle();

        var workbenchLogic = new DMView.WorkspaceView();
        workbenchLogic.onWorkspaceSelected = (id: string) => {
            // Loads the extent of the workspace, if the user has clicked on one of the workbenches
            tthis.navigateToExtents(id);
        };

        workbenchLogic.loadAndCreateHtmlForWorkbenchs($(".data-workspaces", this.parent));
    }

    showExtents(workspaceId: string) {
        var tthis = this;
        tthis.switchLayout({
            type: PageType.Extents,
            workspace: workspaceId
        });
        tthis.createTitle(workspaceId);
        var extentLogic = new DMView.ExtentView(this);
        extentLogic.onExtentSelected = (ws: string, extentUrl: string) => {
            tthis.navigateToItems(ws, extentUrl);
            return false;
        };

        extentLogic.loadAndCreateHtmlForWorkspace($(".data-extents", this.parent), workspaceId);
    }

    showItems(workspaceId: string, extentUrl: string) {
        var tthis = this;
        this.switchLayout(
        {
            type: PageType.Items,
            workspace: workspaceId,
            extent: extentUrl
        });

        this.createTitle(workspaceId, extentUrl);
        var extentLogic = new DMView.ExtentView(this);
        extentLogic.onItemEdit = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl, { isReadonly: true });
        };

        extentLogic.onItemCreated = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl);
        };

        extentLogic.loadAndCreateHtmlForExtent($(".data-items", this.parent), workspaceId, extentUrl);
    }

    showItem(workspaceId: string, extentUrl: string, itemUrl: string, settings?: DMI.View.IItemViewSettings) {
        var tthis = this;
        this.switchLayout({
            type: PageType.ItemDetail,
            workspace: workspaceId,
            extent: extentUrl,
            item: itemUrl
        });

        var extentLogic = new DMView.ItemView(this);

        extentLogic.onItemView = (ws: string, extentUrl: string, itemUrl: string) => {
            tthis.navigateToItem(ws, extentUrl, itemUrl, { isReadonly: true });
        };

        this.createTitle(workspaceId, extentUrl, itemUrl);
        extentLogic.loadAndCreateHtmlForItem($(".data-itemdetail", this.parent),
            workspaceId,
            extentUrl,
            itemUrl,
            settings);
    }

    createTitle(ws?: string, extentUrl?: string, itemUrl?: string) {
        var tthis = this;
        var containerTitle = $(".container-title", this.parent);
        var ba = "&lt;&lt";
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

    showDialogNewWorkspace() {
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
        var view = new DMView.NavigationView();

        view.addLink("New CSV Extent",
        () => {
            tthis.showDialogNewCsvExtent(workspace);
        });

        view.addLink("New CSV Extent for UML class",
        () => {
            // showDialogNewExtent(layout, workspace);
        });

        view.addLink("New XmlExtent",
        () => {
            tthis.showDialogNewXmiExtent(workspace);
        });

        tthis.setView(view);
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

        configuration.addColumn(new DMI.Table.DataField("Workspace", "workspace").withDefaultValue(workspace));
        configuration.addColumn(new DMI.Table.DataField("URI", "contextUri").withDefaultValue("dm:///"));
        configuration.addColumn(new DMI.Table.DataField("Filename", "filename"));
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
                    workspace: data.v["workspace"],
                    contextUri: data.v["contextUri"],
                    filename: data.v["filename"]
                })
                .done(() => tthis.navigateToExtents(data.v["workspace"]));
        };

        configuration.addColumn(new DMI.Table.DataField("Workspace", "workspace").withDefaultValue(workspace));
        configuration.addColumn(new DMI.Table.DataField("URI", "contextUri").withDefaultValue("dm:///"));
        configuration.addColumn(new DMI.Table.DataField("Filename", "filename").withDefaultValue("d:\\file.xml"));

        tthis.navigateToDialog(configuration);
    }

    switchLayout(layoutInformation: DMI.Api.ILayoutChangedEvent) {
        $(".only-workspaces").hide();
        $(".only-extents").hide();
        $(".only-items").hide();
        $(".only-itemdetail").hide();
        $(".only-dialog").hide();

        if (layoutInformation.type === PageType.Workspaces) {
            $(".only-workspaces").show();
        } else if (layoutInformation.type === PageType.Extents) {
            $(".only-extents").show();
        } else if (layoutInformation.type === PageType.Items) {
            $(".only-items").show();
        } else if (layoutInformation.type === PageType.ItemDetail) {
            $(".only-itemdetail").show();
        } else if (layoutInformation.type === PageType.Dialog) {
            $(".only-dialog").show();
        }

        this.currentLayoutInformation = layoutInformation;

        this.throwLayoutChangedEvent(layoutInformation);
    }

    setStatus(statusDom: JQuery): void {
        var dom = $(".dm-statusline");
        dom.empty();
        dom.append(statusDom);
    }

    setView(view: DMView.IView) {
        this.switchLayout(
        {
            layout: this,
            type: PageType.Dialog
        });

        var container = $(".data-dialog", this.parent);
        container.empty();
        view.show(container);
    }

    lastLayoutConfiguration: DMI.Api.ILayoutChangedEvent;

    throwLayoutChangedEvent(data: DMI.Api.ILayoutChangedEvent): void {
        if (data !== undefined && data != null) {
            data.layout = this;
        }

        this.lastLayoutConfiguration = data;

        if (this.onLayoutChanged !== undefined) {
            this.onLayoutChanged(data);
        }

        if (this.pluginResults !== undefined) {
            for (var n in this.pluginResults) {
                var pluginResult = this.pluginResults[n];
                pluginResult.onLayoutChanged(data);
            }
        }
    }

    renavigate(): void {
        this.throwLayoutChangedEvent(this.lastLayoutConfiguration);
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