define(["require", "exports", "./datenmeister-navigation", "./datenmeister-client", "./datenmeister-interfaces", "./datenmeister-view"], function (require, exports, DMN, DMClient, DMI, DMView) {
    "use strict";
    function showDialogNewWorkspace(navigation) {
        var configuration = new DMN.DialogConfiguration();
        configuration.onOkForm = function (data) {
            DMClient.WorkspaceApi.createWorkspace({
                name: data.v["name"],
                annotation: data.v["annotation"]
            })
                .done(function () { return navigation.navigateToWorkspaces(); });
        };
        configuration.addColumn(new DMI.Table.DataField("Title", "name"));
        var annotationColumn = new DMI.Table.TextDataField("Annotation", "annotation");
        annotationColumn.lineHeight = 4;
        configuration.addColumn(annotationColumn);
        navigation.navigateToDialog(configuration);
    }
    exports.showDialogNewWorkspace = showDialogNewWorkspace;
    function showNavigationForNewExtents(navigation, workspace) {
        var view = new DMView.EmptyView(navigation);
        view.addLink("New CSV Extent", function () {
            showDialogNewCsvExtent(navigation, workspace);
        });
        view.addLink("New XmlExtent", function () {
            showDialogNewXmiExtent(navigation, workspace);
        });
        navigation.navigateToView(view);
    }
    exports.showNavigationForNewExtents = showNavigationForNewExtents;
    function showDialogNewCsvExtent(navigation, workspace) {
        var configuration = new DMN.DialogConfiguration();
        configuration.onOkForm = function (data) {
            DMClient.ExtentApi.createExtent({
                type: "csv",
                workspace: data.v["workspace"],
                contextUri: data.v["contextUri"],
                filename: data.v["filename"],
                columns: data.v["columns"]
            })
                .done(function () { return navigation.navigateToExtents(data.v["workspace"]); });
        };
        configuration.addColumn(new DMI.Table.DataField("Workspace", "workspace").withDefaultValue(workspace));
        configuration.addColumn(new DMI.Table.DataField("URI", "contextUri").withDefaultValue("dm:///"));
        configuration.addColumn(new DMI.Table.DataField("Filename", "filename"));
        configuration.addColumn(new DMI.Table.DataField("Columns", "columns").withDefaultValue("Column1,Column2"));
        configuration.ws = workspace;
        navigation.navigateToDialog(configuration);
    }
    exports.showDialogNewCsvExtent = showDialogNewCsvExtent;
    function showDialogAddCsvExtent(navigation, workspace) {
        var configuration = new DMN.DialogConfiguration();
        configuration.onOkForm = function (data) {
            DMClient.ExtentApi.addExtent({
                type: "csv",
                workspace: data.v["workspace"],
                contextUri: data.v["contextUri"],
                filename: data.v["filename"]
            })
                .done(function () { return navigation.navigateToExtents(data.v["workspace"]); });
        };
        configuration.addColumn(new DMI.Table.TextDataField("Workspace", "workspace").withDefaultValue(workspace));
        configuration.addColumn(new DMI.Table.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
        configuration.addColumn(new DMI.Table.TextDataField("Filename", "filename"));
        configuration.ws = workspace;
        navigation.navigateToDialog(configuration);
    }
    exports.showDialogAddCsvExtent = showDialogAddCsvExtent;
    function showDialogNewXmiExtent(navigation, workspace) {
        var configuration = new DMN.DialogConfiguration();
        configuration.onOkForm = function (data) {
            DMClient.ExtentApi.createExtent({
                type: "xmi",
                workspace: workspace,
                contextUri: data.v["contextUri"],
                name: data.v["name"]
            })
                .done(function () { return navigation.navigateToExtents(data.v["workspace"]); });
        };
        configuration.addColumn(new DMI.Table.TextDataField("Name", "name").withDefaultValue("name"));
        configuration.addColumn(new DMI.Table.TextDataField("Workspace", "workspace").withDefaultValue(workspace).asReadOnly());
        configuration.addColumn(new DMI.Table.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
        navigation.navigateToDialog(configuration);
    }
    exports.showDialogNewXmiExtent = showDialogNewXmiExtent;
});
//# sourceMappingURL=datenmeister-dialogs.js.map