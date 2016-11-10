define(["require", "exports"], function (require, exports) {
    "use strict";
    var ViewPort = (function () {
        function ViewPort(container, layout) {
            this.container = container;
            this.layout = layout;
        }
        ViewPort.prototype.setView = function (view) {
            this.container.empty();
            this.container.append(view.getContent());
            this.layout.throwLayoutChangedEvent(view.getLayoutInformation());
            view.viewport = this;
        };
        return ViewPort;
    }());
    exports.ViewPort = ViewPort;
});
//# sourceMappingURL=datenmeister-viewport.js.map