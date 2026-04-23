export class ElementsTreeViewConfig {
    workspace;
    extentUri;
}
export class ElementsTreeView {
    init() {
        $("#test_tree_view").fancytree({
            checkbox: true,
            source: [
                { title: "Node 1" },
                { title: "Node 2", key: "id2" },
                { title: "Folder 3", folder: true, children: [
                        { title: "Node 3.1" },
                        { title: "Node 3.2" }
                    ] },
                { title: "Folder 2", folder: true }
            ],
            activate: function (event, data) {
                $("#status").text("Activate: " + data.node);
            }
        });
    }
}
//# sourceMappingURL=ElementsTreeView.js.map