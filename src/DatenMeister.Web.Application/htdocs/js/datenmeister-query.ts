
import * as DMI from "datenmeister-interfaces"
import * as DMClient from "datenmeister-client"

export class ItemsFromExtentProvider implements DMI.IItemsProvider {

    ws: string;
    extent: string;

    constructor(ws: string, extent: string) {
        this.ws = ws;
        this.extent = extent;
    }


    performQuery(query: DMI.ItemInExtentQuery): JQueryDeferred<DMI.IItemsContent> {

        return DMClient.ExtentApi.getItems(this.ws, this.extent, query);
    }
}