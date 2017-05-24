import DMI = require("./datenmeister-interfaces");
import DMV = require("./datenmeister-view");
import IViewState = DMI.Api.IViewState;
import IViewPort = DMI.Views.IViewPort;

export class ViewResolver{
    resolveView(viewPort: IViewPort, viewState: IViewState) {
        if (viewState.workspace === undefined || viewState.workspace === null) {
            DMV.navigateToWorkspaces(viewPort);
        }
    }
}
