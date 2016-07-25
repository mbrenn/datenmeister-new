define(["require", "exports", "./datenmeister-client"], function (require, exports, DMClient) {
    "use strict";
    function load(plugin) {
        return {
            onLayoutChanged: function (ev) {
                var tab = ev.layout.getRibbon().getOrAddTab("Tasks");
                if (ev.extent !== undefined && ev.extent !== null) {
                    tab.addIcon("Add Task", "...", function () {
                        DMClient.ExtentApi.createItem(ev.workspace, ev.extent, undefined, "datenmeister:///types#TaskMeisterLib.Model.IActivity")
                            .done(function (innerData) {
                            ev.layout.navigateToItem(ev.workspace, ev.extent, innerData.newuri);
                        });
                    });
                }
            }
        };
    }
    exports.load = load;
});
//# sourceMappingURL=taskmeister.js.map