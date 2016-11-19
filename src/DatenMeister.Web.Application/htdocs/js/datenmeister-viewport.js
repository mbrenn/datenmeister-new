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
            this.layout.throwViewPortChanged(view.getLayoutInformation());
            view.viewport = this;
        };
        /**
         * Throws the onViewPortChanged event
         * @param data Event information being changed
         */
        ViewPort.prototype.throwViewPortChanged = function (data) {
            if (this.onViewPortChanged !== undefined) {
                this.onViewPortChanged(data);
            }
        };
        return ViewPort;
    }());
    exports.ViewPort = ViewPort;
});
//# sourceMappingURL=datenmeister-viewport.js.map