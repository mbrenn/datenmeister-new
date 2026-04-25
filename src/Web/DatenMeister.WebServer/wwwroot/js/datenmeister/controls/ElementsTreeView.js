import * as Elements from "../client/Elements.js";
import * as Navigator from "../Navigator.js";
import { UserEvent } from "../../burnsystems/Events.js";
export class ElementsTreeViewConfig {
    workspace;
    extentUri;
}
export class ElementsTreeView {
    /**
     * This event is called when an item is activated in the tree
     */
    itemActivated = new UserEvent();
    configuration;
    init(elementSelector, config) {
        this.configuration = config;
        $(elementSelector).fancytree({
            checkbox: false,
            source: Elements.getAllRootItems(config.workspace, config.extentUri).then(items => this.mapItems(items)),
            lazyLoad: (event, data) => {
                data.result = Elements.getAllChildItems(config.workspace, data.node.key).then(items => this.mapItems(items));
            },
            activate: (event, data) => {
                const itemUrl = data.node.key;
                if (itemUrl) {
                    this.itemActivated.invoke(itemUrl);
                }
            }
        });
    }
    /**
     * Adds the event to navigate to the item when it is activated
     * @param workspace The workspace to navigate in
     */
    addEventToNavigateToItem() {
        this.itemActivated.addListener(itemUrl => {
            Navigator.navigateToItemByUrl(this.configuration.workspace, itemUrl);
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