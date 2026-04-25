import * as Elements from "../client/Elements.js";
import * as Navigator from "../Navigator.js";
export class ElementsTreeViewConfig {
    workspace;
    extentUri;
}
export class ElementsTreeView {
    init(elementSelector, config) {
        $(elementSelector).fancytree({
            checkbox: false,
            source: Elements.getAllRootItems(config.workspace, config.extentUri).then(items => this.mapItems(items)),
            lazyLoad: (event, data) => {
                data.result = Elements.getAllChildItems(config.workspace, data.node.key).then(items => this.mapItems(items));
            },
            activate: (event, data) => {
                const itemUrl = data.node.key;
                if (itemUrl) {
                    Navigator.navigateToItemByUrl(config.workspace, itemUrl);
                }
            }
        });
    }
    mapItems(items) {
        return items.map(item => ({
            title: item.name,
            key: item.uri,
            lazy: true // Assume it might have children
        }));
    }
}
//# sourceMappingURL=ElementsTreeView.js.map