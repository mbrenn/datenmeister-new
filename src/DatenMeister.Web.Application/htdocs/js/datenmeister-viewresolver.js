define(["require", "exports", "./datenmeister-view"], function (require, exports, DMV) {
    "use strict";
    exports.__esModule = true;
    var ViewResolver = (function () {
        function ViewResolver() {
        }
        ViewResolver.prototype.resolveView = function (viewPort, viewState) {
            if (viewState.workspace === undefined || viewState.workspace === null) {
                DMV.WorkspaceList.navigateToWorkspaces(viewPort);
                return;
            }
            else if (viewState.extent === undefined || viewState.extent === null) {
                DMV.ExtentList.navigateToExtents(viewPort, viewState.workspace);
            }
            else if (viewState.item === undefined || viewState.item === null) {
                DMV.ItemList.navigateToItems(viewPort, viewState.workspace, viewState.extent);
            }
            else {
                DMV.ItemDetail.navigateToItem(viewPort, viewState.workspace, viewState.extent, viewState.item);
            }
        };
        return ViewResolver;
    }());
    exports.ViewResolver = ViewResolver;
});
//# sourceMappingURL=datenmeister-viewresolver.js.map