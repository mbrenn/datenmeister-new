import DMClient = require("./datenmeister-client");
import DMI = require("./datenmeister-interfaces");
import DMView = require("./datenmeister-view");
import * as DMVM from "./datenmeister-viewmodels";

export function showDialogNewWorkspace(navigation: DMI.Navigation.INavigation): void {
    var configuration = new DMI.Navigation.DialogConfiguration();

    configuration.onOkForm = data => {
        DMClient.WorkspaceApi.createWorkspace(
            {
                name: data.v["name"],
                annotation: data.v["annotation"]
            })
            .done(() => navigation.navigateToWorkspaces());
    };

    configuration.addColumn(new DMVM.TextDataField("Name", "name"));
    var annotationColumn = new DMVM.TextDataField("Annotation", "annotation");
    annotationColumn.lineHeight = 4;
    configuration.addColumn(annotationColumn);

    navigation.navigateToDialog(configuration);
}

export function showNavigationForNewExtents(navigation: DMI.Navigation.INavigation, workspace: string) {
    var view = new DMView.EmptyView(navigation);

    view.addLink("New CSV Extent",
        () => {
            showDialogNewCsvExtent(navigation, workspace);
        });

    view.addLink("New XmlExtent",
        () => {
            showDialogNewXmiExtent(navigation, workspace);
        });

    navigation.navigateToView(view);
}

export function showDialogNewCsvExtent(navigation: DMI.Navigation.INavigation, workspace: string) {
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
            .done(() => navigation.navigateToExtents(data.v["workspace"]));
    };

    configuration.addColumn(new DMVM.TextDataField("Workspace", "workspace").withDefaultValue(workspace));
    configuration.addColumn(new DMVM.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
    configuration.addColumn(new DMVM.TextDataField("Filename", "filename"));
    configuration.addColumn(new DMVM.TextDataField("Columns", "columns").withDefaultValue("Column1,Column2"));
    configuration.ws = workspace;

    navigation.navigateToDialog(configuration);
}

export function showDialogAddCsvExtent(navigation: DMI.Navigation.INavigation, workspace: string) {
    var configuration = new DMI.Navigation.DialogConfiguration();

    configuration.onOkForm = data => {
        DMClient.ExtentApi.addExtent(
            {
                type: "csv",
                workspace: data.v["workspace"],
                contextUri: data.v["contextUri"],
                filename: data.v["filename"]
            })
            .done(() => navigation.navigateToExtents(data.v["workspace"]));
    };

    configuration.addColumn(new DMVM.TextDataField("Workspace", "workspace").withDefaultValue(workspace));
    configuration.addColumn(new DMVM.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
    configuration.addColumn(new DMVM.TextDataField("Filename", "filename"));
    configuration.ws = workspace;

    navigation.navigateToDialog(configuration);
}

export function showDialogNewXmiExtent(navigation: DMI.Navigation.INavigation, workspace: string) {
    var configuration = new DMI.Navigation.DialogConfiguration();
    configuration.onOkForm = data => {
        DMClient.ExtentApi.createExtent(
            {
                type: "xmi",
                workspace: workspace,
                contextUri: data.v["contextUri"],
                name: data.v["name"]
            })
            .done(() => navigation.navigateToExtents(data.v["workspace"]));
    };

    configuration.addColumn(new DMVM.TextDataField("Name", "name").withDefaultValue("name"));
    configuration.addColumn(new DMVM.TextDataField("Workspace", "workspace").withDefaultValue(workspace).asReadOnly());
    configuration.addColumn(new DMVM.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));

    navigation.navigateToDialog(configuration);
}
