
import {ItemWithNameAndId} from "../ApiModels.js";
import * as Elements from "../client/Elements.js";
import * as Navigator from "../Navigator.js";

export class ElementsTreeViewConfig
{
    workspace: string;
    extentUri: string;
}

export class ElementsTreeView {
    init(elementSelector: string, config: ElementsTreeViewConfig)
    {
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

    private mapItems(items: ItemWithNameAndId[]): any[] {
        return items.map(item => ({
            title: item.name,
            key: item.uri,
            lazy: true // Assume it might have children
        }));
    }
}