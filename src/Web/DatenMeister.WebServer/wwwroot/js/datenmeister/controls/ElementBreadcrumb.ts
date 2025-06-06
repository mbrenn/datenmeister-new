import * as ClientItems from '../client/Items.js'
import {EntentType, ItemWithNameAndId} from "../ApiModels.js";
import * as Navigator from '../Navigator.js'

export class ElementBreadcrumb {

    private _container: JQuery;

    constructor(container: JQuery) {
        this._container = container;
    }

    async createForExtent(workspace: string, extentUri: string): Promise<void> {
        const container = await ClientItems.getContainer(workspace, extentUri, true);

        this.build(container);
    }

    async createForItem(workspace: string, itemUri: string): Promise<void> {
        const container = await ClientItems.getContainer(workspace, itemUri, true);

        this.build(container);
    }

    private build(container: Array<ItemWithNameAndId>) {
        this._container.empty();

        let first = true;
        for (const n in container.reverse()) {
            // Creates the intersections
            if ( !first) {
                this._container.append($("<span> &lt; </span>"));
            }
            
            // Finds the item
            const item = container[n];
            const element = $("<a></a>");
            element.text(item.name);
            if (item.ententType === EntentType.Extent) {
                element.attr('href', Navigator.getLinkForNavigateToExtentItems(item.workspace, item.extentUri));
            } else {
                element.attr('href', Navigator.getLinkForNavigateToItemByUrl(item.workspace, item.uri));
            }

            this._container.append(element);

            // Adds the properties, in case the ententType is of type Extent
            if (item.ententType === EntentType.Extent) {
                const propertyElement = $("<a>(Properties)</a>");
                $(propertyElement).attr('href', Navigator.getLinkForNavigateToExtentProperties(item.workspace, item.extentUri));
                this._container.append($("<span> </span>"));
                this._container.append(propertyElement);
            }
            
            first = false;
        }
    }
}

export function createBreadcrumbForExtent(container: JQuery, workspace: string, extentUri: string) {
    let breadcrumb = new ElementBreadcrumb(container);
    const _ = breadcrumb.createForExtent(workspace, extentUri);
}

export async function createBreadcrumbForItem(container: JQuery, workspace: string, itemUri: string) {
    let breadcrumb = new ElementBreadcrumb(container);
    const _ = await breadcrumb.createForItem(workspace, itemUri);
}