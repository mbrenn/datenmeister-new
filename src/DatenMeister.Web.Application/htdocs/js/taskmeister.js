define(["require", "exports", "./datenmeister-client"], function (require, exports, DMClient) {
    "use strict";
    function load(plugin) {
        return {
            onViewPortChanged: function (ev) {
                var tab = ev.layout.getRibbon().getOrAddTab("Tasks");
                if (ev.extent !== undefined && ev.extent !== null) {
                    tab.addIcon("Add Task", "...", function () {
                        DMClient.ExtentApi.createItem(ev.workspace, ev.extent, "datenmeister:///types#TaskMeisterLib.Model.IActivity")
                            .done(function (innerData) {
                            ev.layout.navigateToItem(ev.workspace, ev.extent, innerData.newuri);
                        });
                    });
                    tab.addIcon("Show Tasks", "...,", function () {
                        ev.layout.navigateToItems(ev.workspace, ev.extent, "dm:///management/views#Views.Activity.Detail");
                    });
                }
            }
        };
    }
    exports.load = load;
});
//# sourceMappingURL=taskmeister.js.map