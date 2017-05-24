define(["require", "exports", "./datenmeister-client", "./datenmeister-view"], function (require, exports, DMClient, DMView) {
    "use strict";
    exports.__esModule = true;
    function load(plugin) {
        return {
            onRibbonUpdate: function (ev) {
                var tab = ev.layout.getRibbon().getOrAddTab("Tasks");
                if (ev.viewState.extent !== undefined && ev.viewState.extent !== null) {
                    tab.addIcon("Add Task", "...", function () {
                        DMClient.ExtentApi.createItem(ev.viewState.workspace, ev.viewState.extent, "datenmeister:///types#TaskMeisterLib.Model.IActivity")
                            .done(function (innerData) {
                            DMView.navigateToItem(ev.layout.mainViewPort, ev.viewState.workspace, ev.viewState.extent, innerData.newuri);
                        });
                    });
                    tab.addIcon("Show Tasks", "...,", function () {
                        DMView.navigateToItems(ev.layout.mainViewPort, ev.viewState.workspace, ev.viewState.extent, "dm:///management/views#Views.Activity.Detail");
                    });
                }
            },
            onViewPortChanged: function (ev) {
            }
        };
    }
    exports.load = load;
});
//# sourceMappingURL=taskmeister.js.map