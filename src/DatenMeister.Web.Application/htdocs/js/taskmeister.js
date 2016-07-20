define(["require", "exports"], function (require, exports) {
    "use strict";
    function load(plugin) {
        return {
            onLayoutChanged: function (ev) {
                var tab = ev.layout.getRibbon().getOrAddTab("Tasks");
                if (ev.extent !== undefined && ev.extent !== null) {
                    tab.addIcon("Test", "...", function () {
                        alert('JO');
                    });
                }
            }
        };
    }
    exports.load = load;
});
//# sourceMappingURL=taskmeister.js.map