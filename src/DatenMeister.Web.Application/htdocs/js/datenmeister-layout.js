define(["require", "exports", "./datenmeister-interfaces", "./datenmeister-client", "./datenmeister-viewport", "./datenmeister-view", "./datenmeister-ribbon"], function (require, exports, DMI, DMClient, DMViewPort, DMView, DMRibbon) {
    "use strict";
    var Layout = (function () {
        function Layout(parent) {
            this.parent = parent;
            this.pluginResults = new Array();
            this.mainViewPort = new DMViewPort.ViewPort($("#dm-viewport", this.parent), this);
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
            extentView.onExtentSelected = function (ws, extentUrl) {
                tthis.navigateToItems(ws, extentUrl);
                return false;
            };
            extentView.loadAndCreateHtmlForWorkspace(workspaceId);
            this.mainViewPort.setView(extentView);
        };
        Layout.prototype.showItems = function (workspaceId, extentUrl, viewname) {
            var tthis = this;
            this.createTitle(workspaceId, extentUrl);
            var extentView = new DMView.ExtentView(this);
            extentView.onItemEdit = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl);
            };
            extentView.onItemView = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl, undefined, { isReadonly: true });
            };
            extentView.onItemCreated = function (ws, extentUrl, itemUrl) {
                tthis.navigateToItem(ws, extentUrl, itemUrl);
            };
            var query = new DMI.PostModels.ItemInExtentQuery();
            query.view = viewname;
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
            var view = new DMView.NavigationView(this);
            view.addLink("New CSV Extent", function () {
                tthis.showDialogNewCsvExtent(workspace);
            });
            view.addLink("New XmlExtent", function () {
                tthis.showDialogNewXmiExtent(workspace);
            });
            this.mainViewPort.setView(view);
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
            configuration.addColumn(new DMI.Table.TextDataField("Workspace", "workspace").withDefaultValue(workspace));
            configuration.addColumn(new DMI.Table.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
            configuration.addColumn(new DMI.Table.TextDataField("Filename", "filename"));
            configuration.ws = workspace;
            tthis.navigateToDialog(configuration);
        };
        Layout.prototype.showDialogNewXmiExtent = function (workspace) {
            var tthis = this;
            var configuration = new DMI.Api.DialogConfiguration();
            configuration.onOkForm = function (data) {
                DMClient.ExtentApi.createExtent({
                    type: "xmi",
                    workspace: workspace,
                    contextUri: data.v["contextUri"],
                    name: data.v["name"]
                })
                    .done(function () { return tthis.navigateToExtents(data.v["workspace"]); });
            };
            configuration.addColumn(new DMI.Table.TextDataField("Name", "name").withDefaultValue("name"));
            configuration.addColumn(new DMI.Table.TextDataField("Workspace", "workspace").withDefaultValue(workspace).asReadOnly());
            configuration.addColumn(new DMI.Table.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
            tthis.navigateToDialog(configuration);
        };
        Layout.prototype.setStatus = function (statusDom) {
            var dom = $(".dm-statusline");
            dom.empty();
            dom.append(statusDom);
        };
        Layout.prototype.throwViewPortChanged = function (data) {
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
        return Layout;
    }());
    exports.Layout = Layout;
});
//# sourceMappingURL=datenmeister-layout.js.map