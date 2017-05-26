define(["require", "exports", "./datenmeister-view"], function (require, exports, DMV) {
    "use strict";
    exports.__esModule = true;
    var ViewResolver = (function () {
        function ViewResolver() {
        }
        ViewResolver.prototype.resolveView = function (viewPort, viewState) {
            if (viewState.workspace === undefined || viewState.workspace === null) {
                DMV.WorkspaceList.navigateToWorkspaces(viewPort);
            }
        };
        return ViewResolver;
    }());
    exports.ViewResolver = ViewResolver;
});
//# sourceMappingURL=datenmeister-viewresolver.js.map