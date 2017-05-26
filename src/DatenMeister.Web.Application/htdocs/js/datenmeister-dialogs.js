define(["require", "exports", "./datenmeister-client", "./datenmeister-interfaces", "./datenmeister-viewmodels", "./datenmeister-view"], function (require, exports, DMClient, DMI, DMVM, DMView) {
    "use strict";
    exports.__esModule = true;
    function showDialogNewWorkspace(viewport) {
        var configuration = new DMI.Navigation.DialogConfiguration();
        configuration.onOkForm = function (data) {
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
    exports.showDialogNewWorkspace = showDialogNewWorkspace;
    function showNavigationForNewExtents(viewport, workspace) {
        var view = new DMView.EmptyView(viewport);
        view.addLink("New CSV Extent", function () {
            showDialogNewCsvExtent(viewport, workspace);
        });
        view.addLink("New XmlExtent", function () {
            showDialogNewXmiExtent(viewport, workspace);
        });
        viewport.setView(view);
    }
    exports.showNavigationForNewExtents = showNavigationForNewExtents;
    function showDialogNewCsvExtent(viewport, workspace) {
        var configuration = new DMI.Navigation.DialogConfiguration();
        configuration.onOkForm = function (data) {
            DMClient.ExtentApi.createExtent({
                type: "csv",
                workspace: data.v["workspace"],
                contextUri: data.v["contextUri"],
                filename: data.v["filename"],
                columns: data.v["columns"]
            })
                .done(function () { return DMView.ExtentList.navigateToExtents(viewport, data.v["workspace"]); });
        };
        configuration.addColumn(new DMVM.TextDataField("Workspace", "workspace").withDefaultValue(workspace));
        configuration.addColumn(new DMVM.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
        configuration.addColumn(new DMVM.TextDataField("Filename", "filename"));
        configuration.addColumn(new DMVM.TextDataField("Columns", "columns").withDefaultValue("Column1,Column2"));
        configuration.ws = workspace;
        DMView.navigateToDialog(viewport, configuration);
    }
    exports.showDialogNewCsvExtent = showDialogNewCsvExtent;
    function showDialogAddCsvExtent(viewport, workspace) {
        var configuration = new DMI.Navigation.DialogConfiguration();
        configuration.onOkForm = function (data) {
            DMClient.ExtentApi.addExtent({
                type: "csv",
                workspace: data.v["workspace"],
                contextUri: data.v["contextUri"],
                filename: data.v["filename"]
            })
                .done(function () { return DMView.ExtentList.navigateToExtents(viewport, data.v["workspace"]); });
        };
        configuration.addColumn(new DMVM.TextDataField("Workspace", "workspace").withDefaultValue(workspace));
        configuration.addColumn(new DMVM.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
        configuration.addColumn(new DMVM.TextDataField("Filename", "filename"));
        configuration.ws = workspace;
        DMView.navigateToDialog(viewport, configuration);
    }
    exports.showDialogAddCsvExtent = showDialogAddCsvExtent;
    function showDialogNewXmiExtent(viewport, workspace) {
        var configuration = new DMI.Navigation.DialogConfiguration();
        configuration.onOkForm = function (data) {
            DMClient.ExtentApi.createExtent({
                type: "xmi",
                workspace: workspace,
                contextUri: data.v["contextUri"],
                name: data.v["name"]
            })
                .done(function () { return DMView.ExtentList.navigateToExtents(viewport, data.v["workspace"]); });
        };
        configuration.addColumn(new DMVM.TextDataField("Name", "name").withDefaultValue("name"));
        configuration.addColumn(new DMVM.TextDataField("Workspace", "workspace").withDefaultValue(workspace).asReadOnly());
        configuration.addColumn(new DMVM.TextDataField("URI", "contextUri").withDefaultValue("dm:///"));
        DMView.navigateToDialog(viewport, configuration);
    }
    exports.showDialogNewXmiExtent = showDialogNewXmiExtent;
});
//# sourceMappingURL=datenmeister-dialogs.js.map