/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
define(["require", "exports", "datenmeister-helper", "datenmeister-interfaces", "datenmeister-client", "datenmeister-ribbon", "datenmeister-layout"], function (require, exports, DMHelper, DMI, DMClient, DMRibbon, DMLayout) {
    function start() {
        $(document).ready(function () {
            window.onpopstate = function (ev) {
                parseAndNavigateToWindowLocation();
            };
            parseAndNavigateToWindowLocation();
        });
    }
    exports.start = start;
    function parseAndNavigateToWindowLocation() {
        var ws = DMHelper.getParameterByNameFromHash("ws");
        var extentUrl = DMHelper.getParameterByNameFromHash("ext");
        var itemUrl = DMHelper.getParameterByNameFromHash("item");
        var layout = new DMLayout.Layout($("body"));
        layout.onLayoutChanged = function (data) { return buildRibbons(layout, data); };
        if (ws === "") {
            layout.showWorkspaces();
        }
        else if (extentUrl === "") {
            layout.showExtents(ws);
        }
        else if (itemUrl === "") {
            layout.showItems(ws, extentUrl);
        }
        else {
            layout.showItem(ws, extentUrl, itemUrl);
        }
        $(".body-content").show();
    }
    exports.parseAndNavigateToWindowLocation = parseAndNavigateToWindowLocation;
    function buildRibbons(layout, changeEvent) {
        var domRibbon = $(".datenmeister-ribbon");
        var ribbon = new DMRibbon.Ribbon(domRibbon);
        var tabFile = ribbon.addTab("File");
        tabFile.addIcon("Refresh", "img/icons/refresh_update", function () { layout.refreshView(); });
        tabFile.addIcon("Workspaces", "img/icons/database", function () { layout.showWorkspaces(); });
        tabFile.addIcon("Add Workspace", "img/icons/database-add", function () { showDialogNewWorkspace(layout); });
        if (changeEvent.workspace !== undefined) {
            // Ok, we have a workspace
            tabFile.addIcon("Delete Workspace", "img/icons/database-delete", function () {
                DMClient.WorkspaceApi.deleteWorkspace(changeEvent.workspace)
                    .done(function () { return layout.navigateToWorkspaces(); });
            });
            tabFile.addIcon("Create Extent", "img/icons/folder_open-add", function () {
                showDialogNewExtent(layout, changeEvent.workspace);
            });
            if (changeEvent.extent !== undefined) {
                tabFile.addIcon("Delete Extent", "img/icons/folder_open-delete", function () {
                    DMClient.ExtentApi.deleteExtent(changeEvent.workspace, changeEvent.extent)
                        .done(function () { return layout.navigateToExtents(changeEvent.workspace); });
                });
            }
            tabFile.addIcon("Add ZipCodes", "img/icons/folder_open-mail", function () {
                DMClient.ExampleApi.addZipCodes(changeEvent.workspace);
            });
        }
        tabFile.addIcon("Close", "img/icons/close_window", function () { window.close(); });
    }
    function showDialogNewWorkspace(layout) {
        var configuration = new DMI.Api.FormForItemConfiguration();
        configuration.onOkForm = function (data) {
            DMClient.WorkspaceApi.createWorkspace({
                name: data.v["name"],
                annotation: data.v["annotation"]
            })
                .done(function () { return layout.navigateToWorkspaces(); });
        };
        var column = {
            title: "Title",
            name: "name"
        };
        configuration.addColumn(column);
        column =
            {
                title: "Annotation",
                name: "annotation"
            };
        configuration.addColumn(column);
        layout.navigateToDialog(configuration);
    }
    function showDialogNewExtent(layout, workspace) {
        alert('X');
    }
});
//# sourceMappingURL=datenmeister.js.map