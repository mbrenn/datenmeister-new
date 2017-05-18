"use strict";
exports.__esModule = true;
var ViewPort = (function () {
    function ViewPort(container, layout) {
        this.container = container;
        this.layout = layout;
    }
    /**
     * Sets the view into the dom
     * @param view view to be set
     */
    ViewPort.prototype.setView = function (view) {
        this.container.empty();
        this.container.append(view.getContent());
        var layoutInformation = view.getLayoutInformation();
        if (layoutInformation !== undefined && layoutInformation !== null) {
            this.layout.throwViewPortChanged(layoutInformation);
        }
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
//# sourceMappingURL=datenmeister-viewport.js.map