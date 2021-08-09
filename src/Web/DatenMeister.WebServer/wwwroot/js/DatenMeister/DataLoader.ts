import * as Mof from "./Mof"
import * as Settings from "./Settings"
import * as ApiConnection from "./ApiConnection"
import * as ApiModels from "./ApiModels"

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

export function loadObjectByUri(workspace: string, item: string): JQuery.Deferred<Mof.DmObject, never, never> {
    var r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<ApiModels.Out.IItem>(
        Settings.baseUrl +
        "api/items/get/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(item)
    ).done(x => {
        var dmObject =
            Mof.createObjectFromJson(x.item, x.metaClass);
        r.resolve(dmObject);
    });

    return r;
}