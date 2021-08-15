import * as Mof from "./Mof"
import * as Settings from "./Settings"
import * as ApiConnection from "./ApiConnection"
import * as ApiModels from "./ApiModels"
import {DmObject} from "./Mof";

export function loadObject(workspace: string, extent: string, id: string): JQuery.Deferred<Mof.DmObject, never, never> {
    var r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<ApiModels.Out.IItem>(
        Settings.baseUrl +
        "api/items/get/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extent) +
        "/" +
        encodeURIComponent(id)
    ).done(x => {
        var dmObject =
            Mof.createObjectFromJson(x.item);
        r.resolve(dmObject);
    });

    return r;
}

export function loadObjectByUri(workspace: string, url: string): JQuery.Deferred<Mof.DmObject, never, never> {
    var r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<ApiModels.Out.IItem>(
        Settings.baseUrl +
        "api/items/get/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(url)
    ).done(x => {
        var dmObject =
            Mof.createObjectFromJson(x.item, x.metaClass);
        r.resolve(dmObject);
    });

    return r;
}

export function storeObjectByUri(workspace: string, url: string, element: DmObject) : JQuery.Deferred<void, never, never> {
    var r = jQuery.Deferred<void, never, never>();
    var result = Mof.createJsonFromObject(element);

    ApiConnection.put<ApiModels.Out.IItem>(
        Settings.baseUrl +
        "api/items/set/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(url),
        result
    ).done(x => {
        r.resolve();
    });

    return r;
}