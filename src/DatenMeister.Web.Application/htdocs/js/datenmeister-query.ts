
import * as DMI from "./datenmeister-interfaces"
import * as DMClient from "./datenmeister-client"

export class ItemsFromExtentProvider implements DMI.Api.IItemsProvider {

    ws: string;
    ext: string;

    constructor(ws: string, extent: string) {
        this.ws = ws;
        this.ext = extent;
    }


    performQuery(query: DMI.PostModels.IItemTableQuery): JQueryDeferred<DMI.ClientResponse.IItemsContent> {
        return DMClient.ExtentApi.getItems(this.ws, this.ext, query);
    }
}