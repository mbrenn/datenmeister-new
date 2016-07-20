define(["require", "exports", "./datenmeister-interfaces", "./datenmeister-client", "./datenmeister-view", "./datenmeister-tables", "./datenmeister-ribbon"], function (require, exports, DMI, DMClient, DMView, DMTables, DMRibbon) {
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
            this.pluginResults = new Array();
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
            history.pushState({}, "", "#ws=" + encodeURIComponent(ws) + "&ext=" + encodeURIComponent(extentUrl));
            this.showItems(ws, extentUrl);
        };
        Layout.prototype.navigateToItem = function (ws, extentUrl, itemUrl, settings) {
            var url = "#ws=" +
                encodeURIComponent(ws) +
                "&ext=" +
                encodeURIComponent(extentUrl) +
                "&item=" +
                encodeURIComponent(itemUrl);
            if (settings !== undefined && settings !== null) {
                if (settings.isReadonly) {
                    url += "&mode=readonly";
                }
            }
            history.pushState({}, "", url);
            this.showItem(ws, extentUrl, itemUrl, settings);
        };
        Layout.prototype.exportExtent = function (ws, extentUrl) {
            window.open("/api/datenmeister/extent/extent_export?ws="
                + encodeURIComponent(ws) + "&extent="
                + encodeURIComponent(extentUrl));
        };
        Layout.prototype.navigateToDialog = function (configuration) {
            var _this = this;
            var oldPageType = this.currentLayoutInformation;
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
                if (configuration.onOkForm !== undefined) {
                    configuration.onOkForm(value);
                }
            };
            var itemTable = new DMTables.ItemContentTable(value, tableConfiguration);
            itemTable.show(domTable);
            this.switchLayout({
                type: PageType.Dialog,
                workspace: configuration.ws,
                extent: configuration.extent
            });
        };
        Layout.prototype.showWorkspaces = function () {
            var tthis = this;
            tthis.switchLayout({ type: PageType.Workspaces });
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
            tthis.switchLayout({
                type: PageType.Extents,
                workspace: workspaceId
            });
            tthis.createTitle(workspaceId);
            var extentLogic = new DMView.ExtentView(this);
            extentLogic.onExtentSelected = function (ws, extentUrl) {
                tthis.navigateToItems(ws, extentUrl);
                return false;
            };
            extentLogic.loadAndCreateHtmlForWorkspace($(".data-extents", this.parent), workspaceId);
        };
        Layout.prototype.showItems = function (workspaceId, extentUrl) {
            var tthis = this;
            this.switchLayout({
                type: PageType.Items,
                workspace: workspaceId,
                extent: extentUrl
            });
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
            this.switchLayout({
                type: PageType.ItemDetail,
                workspace: workspaceId,
                extent: extentUrl,
                item: itemUrl
            });
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
                containerTitle
                    .html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - Items");
                this.onRefresh = function () {
                    tthis.showItems(ws, extentUrl);
                    return false;
                };
            }
            else {
                containerTitle
                    .html("<a href='#' class='link_workspaces'>Workspaces</a> - <a href='#' class='link_extents'>Extents</a> - <a href='#' class='link_items'>Items</a>");
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
        Layout.prototype.showDialogNewWorkspace = function () {
            var tthis = this;
            var configuration = new DMI.Api.DialogConfiguration();
            configuration.onOkForm = function (data) {
                DMClient.WorkspaceApi.createWorkspace({
                    name: data.v["name"],
                    annotation: data.v["annotation"]
                })
                    .done(function () { return tthis.navigateToWorkspaces(); });
            };
            configuration.addColumn(new DMI.Table.DataField("Title", "name"));
            var annotationColumn = new DMI.Table.TextDataField("Annotation", "annotation");
            annotationColumn.lineHeight = 4;
            configuration.addColumn(annotationColumn);
            tthis.navigateToDialog(configuration);
        };
        Layout.prototype.showNavigationForNewExtents = function (workspace) {
            var tthis = this;
            var view = new DMView.NavigationView();
            view.addLink("New CSV Extent", function () {
                tthis.showDialogNewCsvExtent(workspace);
            });
            view.addLink("New CSV Extent for UML class", function () {
                // showDialogNewExtent(layout, workspace);
            });
            view.addLink("New XmlExtent", function () {
                tthis.showDialogNewXmiExtent(workspace);
            });
            tthis.setView(view);
        };
        Layout.prototype.showDialogNewCsvExtent = function (workspace) {
            var tthis = this;
            var configuration = new DMI.Api.DialogConfiguration();
            configuration.onOkForm = function (data) {
                DMClient.ExtentApi.createExtent({
                    type: "csv",
                    workspace: data.v["workspace"],
                    contextUri: data.v["contextUri"],
                    filename: data.v["filename"],
                    columns: data.v["columns"]
                })
                    .done(function () { return tthis.navigateToExtents(data.v["workspace"]); });
            };
            configuration.addColumn(new DMI.Table.DataField("Workspace", "workspace").withDefaultValue(workspace));
            configuration.addColumn(new DMI.Table.DataField("URI", "contextUri").withDefaultValue("dm:///"));
            configuration.addColumn(new DMI.Table.DataField("Filename", "filename"));
            configuration.addColumn(new DMI.Table.DataField("Columns", "columns").withDefaultValue("Column1,Column2"));
            configuration.ws = workspace;
            tthis.navigateToDialog(configuration);
        };
        Layout.prototype.showDialogAddCsvExtent = function (workspace) {
            var tthis = this;
            var configuration = new DMI.Api.DialogConfiguration();
            configuration.onOkForm = function (data) {
                DMClient.ExtentApi.addExtent({
                    type: "csv",
                    workspace: data.v["workspace"],
                    contextUri: data.v["contextUri"],
                    filename: data.v["filename"]
                })
                    .done(function () { return tthis.navigateToExtents(data.v["workspace"]); });
            };
            configuration.addColumn(new DMI.Table.DataField("Workspace", "workspace").withDefaultValue(workspace));
            configuration.addColumn(new DMI.Table.DataField("URI", "contextUri").withDefaultValue("dm:///"));
            configuration.addColumn(new DMI.Table.DataField("Filename", "filename"));
            configuration.ws = workspace;
            tthis.navigateToDialog(configuration);
        };
        Layout.prototype.showDialogNewXmiExtent = function (workspace) {
            var tthis = this;
            var configuration = new DMI.Api.DialogConfiguration();
            configuration.onOkForm = function (data) {
                DMClient.ExtentApi.createExtent({
                    type: "xmi",
                    workspace: data.v["workspace"],
                    contextUri: data.v["contextUri"],
                    filename: data.v["filename"]
                })
                    .done(function () { return tthis.navigateToExtents(data.v["workspace"]); });
            };
            configuration.addColumn(new DMI.Table.DataField("Workspace", "workspace").withDefaultValue(workspace));
            configuration.addColumn(new DMI.Table.DataField("URI", "contextUri").withDefaultValue("dm:///"));
            configuration.addColumn(new DMI.Table.DataField("Filename", "filename").withDefaultValue("d:\\file.xml"));
            tthis.navigateToDialog(configuration);
        };
        Layout.prototype.switchLayout = function (layoutInformation) {
            $(".only-workspaces").hide();
            $(".only-extents").hide();
            $(".only-items").hide();
            $(".only-itemdetail").hide();
            $(".only-dialog").hide();
            if (layoutInformation.type === PageType.Workspaces) {
                $(".only-workspaces").show();
            }
            else if (layoutInformation.type === PageType.Extents) {
                $(".only-extents").show();
            }
            else if (layoutInformation.type === PageType.Items) {
                $(".only-items").show();
            }
            else if (layoutInformation.type === PageType.ItemDetail) {
                $(".only-itemdetail").show();
            }
            else if (layoutInformation.type === PageType.Dialog) {
                $(".only-dialog").show();
            }
            this.currentLayoutInformation = layoutInformation;
            this.throwLayoutChangedEvent(layoutInformation);
        };
        Layout.prototype.setStatus = function (statusDom) {
            var dom = $(".dm-statusline");
            dom.empty();
            dom.append(statusDom);
        };
        Layout.prototype.setView = function (view) {
            this.switchLayout({
                layout: this,
                type: PageType.Dialog
            });
            var container = $(".data-dialog", this.parent);
            container.empty();
            view.show(container);
        };
        Layout.prototype.throwLayoutChangedEvent = function (data) {
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
        };
        Layout.prototype.renavigate = function () {
            this.throwLayoutChangedEvent(this.lastLayoutConfiguration);
        };
        Layout.prototype.gotoHome = function () {
            this.navigateToWorkspaces();
        };
        Layout.prototype.getRibbon = function () {
            if (this.ribbon === null || this.ribbon === undefined) {
                var domRibbon = $(".datenmeister-ribbon");
                this.ribbon = new DMRibbon.Ribbon(domRibbon);
            }
            return this.ribbon;
        };
        return Layout;
    }());
    exports.Layout = Layout;
});
//# sourceMappingURL=datenmeister-layout.js.map