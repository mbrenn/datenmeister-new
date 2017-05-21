
import * as DMClient from "./datenmeister-client"
import * as DMCI from "./datenmeister-clientinterface"
import * as DMI from "./datenmeister-interfaces"

export class ItemsFromExtentProvider implements DMI.Api.IItemsProvider {

    ws: string;
    ext: string;

    constructor(ws: string, extent: string) {
        this.ws = ws;
        this.ext = extent;
    }


    performQuery(query: DMCI.Out.IItemTableQuery): JQueryDeferred<DMCI.In.IItemsContent> {
        return DMClient.ExtentApi.getItems(this.ws, this.ext, query);
    }
}