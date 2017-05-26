import DMI = require("./datenmeister-interfaces");
import DMV = require("./datenmeister-view");
import IViewState = DMI.Api.IViewState;
import IViewPort = DMI.Views.IViewPort;

export class ViewResolver{
    resolveView(viewPort: IViewPort, viewState: IViewState) {
        if (viewState.workspace === undefined || viewState.workspace === null) {
            DMV.WorkspaceList.navigateToWorkspaces(viewPort);
            return;
        } else if (viewState.extent === undefined || viewState.extent === null) {
            DMV.ExtentList.navigateToExtents(viewPort, viewState.workspace);
        } else if (viewState.item === undefined || viewState.item === null) {
            DMV.ItemList.navigateToItems(viewPort, viewState.workspace, viewState.extent);
        } else {
            DMV.ItemDetail.navigateToItem(viewPort, viewState.workspace, viewState.extent, viewState.item);
        }
    }
}
