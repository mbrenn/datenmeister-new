import DMClient = require("./datenmeister-client");
import DMI = require("./datenmeister-interfaces");
import * as DMVM from "./datenmeister-viewmodels";
import * as DMView from "./datenmeister-view";

export function showDialogNewWorkspace(viewport: DMI.Views.IViewPort): void {
    var configuration = new DMI.Navigation.DialogConfiguration();

    configuration.onOkForm = data => {
        alert(data.name);
        alert(data.annotation);

        return;
        /*DMClient.WorkspaceApi.createWorkspace(
            {
                name: data["name"],
                annotation: data["annotation"]
            })
            .done(() => DMView.WorkspaceList.navigateToWorkspaces(viewport));*/
    };

    configuration.addColumn(new DMVM.TextDataField("Name", "name"));
    var annotationColumn = new DMVM.TextDataField("Annotation", "annotation");
    annotationColumn.lineHeight = 4;
    configuration.addColumn(annotationColumn);
    
    DMView.navigateToDialog(viewport, configuration);
}

export function showNavigationForNewExtents(viewport: DMI.Views.IViewPort, workspace: string) {
    var view = new DMView.EmptyView(viewport);

    view.addLink("New CSV Extent",
        () => {
            showDialogNewCsvExtent(viewport, workspace);
        });

    view.addLink("New XmlExtent",
        () => {
            showDialogNewXmiExtent(viewport, workspace);
        });

    viewport.setView(view);
}

export function showDialogNewCsvExtent(viewport: DMI.Views.IViewPort, workspace: string) {
    var configuration = new DMI.Navigation.DialogConfiguration();

    configuration.onOkForm = data => {
        DMClient.ExtentApi.createExtent(
            {
                type: "csv",
                workspace: data.v["workspace"],
                contextUri: data.v["contextUri"],
                filename: data.v["filename"],
                columns: data.v["columns"]
            })
            .done(() => DMView.ExtentList.navigateToExtents(viewport, data.v["workspace"]));
    };

    configuration.addColumn(new DMVM.TextDataField("Workspace", "workspace").withDefaultValue(workspace));
    configuration.addColumn(new DMVM.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
    configuration.addColumn(new DMVM.TextDataField("Filename", "filename"));
    configuration.addColumn(new DMVM.TextDataField("Columns", "columns").withDefaultValue("Column1,Column2"));
    // TODO configuration.ws = workspace;

    DMView.navigateToDialog(viewport, configuration);
}

export function showDialogAddCsvExtent(viewport: DMI.Views.IViewPort, workspace: string) {
    var configuration = new DMI.Navigation.DialogConfiguration();

    configuration.onOkForm = data => {
        DMClient.ExtentApi.addExtent(
            {
                type: "csv",
                workspace: data.v["workspace"],
                contextUri: data.v["contextUri"],
                filename: data.v["filename"]
            })
            .done(() => DMView.ExtentList.navigateToExtents(viewport, data.v["workspace"]));
    };

    configuration.addColumn(new DMVM.TextDataField("Workspace", "workspace").withDefaultValue(workspace));
    configuration.addColumn(new DMVM.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
    configuration.addColumn(new DMVM.TextDataField("Filename", "filename"));
    // TODO configuration.ws = workspace;

    DMView.navigateToDialog(viewport, configuration);
}

export function showDialogNewXmiExtent(
    viewport: DMI.Views.IViewPort,
    workspace: string) {
    var configuration = new DMI.Navigation.DialogConfiguration();
    configuration.onOkForm = data => {
        DMClient.ExtentApi.createExtent(
            {
                type: "xmi",
                workspace: workspace,
                contextUri: data.v["contextUri"],
                name: data.v["name"]
            })
            .done(() => DMView.ExtentList.navigateToExtents(viewport, data.v["workspace"]));
    };

    configuration.addColumn(new DMVM.TextDataField("Name", "name").withDefaultValue("name"));
    configuration.addColumn(new DMVM.TextDataField("Workspace", "workspace").withDefaultValue(workspace).asReadOnly());
    configuration.addColumn(new DMVM.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));

    DMView.navigateToDialog(viewport, configuration);
}
