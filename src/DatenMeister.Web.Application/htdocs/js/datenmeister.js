/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
define(["require", "exports", "datenmeister-helper", "datenmeister-interfaces", "datenmeister-ribbon", "datenmeister-layout"], function (require, exports, DMHelper, DMI, DMRibbon, DMLayout) {
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
        buildRibbons(layout);
        $(".body-content").show();
    }
    exports.parseAndNavigateToWindowLocation = parseAndNavigateToWindowLocation;
    function buildRibbons(layout) {
        var domRibbon = $(".datenmeister-ribbon");
        var ribbon = new DMRibbon.Ribbon(domRibbon);
        var tab1 = ribbon.addTab("File");
        tab1.addIcon("Refresh", "img/icons/refresh_update", function () { layout.refreshView(); });
        tab1.addIcon("Workspaces", "img/icons/database", function () { layout.showWorkspaces(); });
        tab1.addIcon("Add Workspace", "img/icons/database-add", function () {
            var configuration = new DMI.Api.FormForItemConfiguration();
            configuration.onOkForm = function (data) { alert(data); };
            configuration.onCancelForm = function () { alert("CANCEL"); };
            var column = new DMI.Api.FieldConfiguration();
            column.propertyName = "title";
            column.title = "Title";
            configuration.addColumn(column);
            layout.navigateToDialog(configuration);
        });
        tab1.addIcon("Delete Workspace", "img/icons/database-delete", function () { alert("X"); });
        tab1.addIcon("Close", "img/icons/close_window", function () { window.close(); });
    }
});
//# sourceMappingURL=datenmeister.js.map