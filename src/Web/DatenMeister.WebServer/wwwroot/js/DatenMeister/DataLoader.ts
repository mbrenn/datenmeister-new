import * as Mof from "./Mof"
import * as Settings from "./Settings"
import * as ApiConnection from "./ApiConnection"
import * as ApiModels from "./ApiModels"
import {DmObject} from "./Mof";

export function loadObject(workspace: string, extent: string, id: string): JQuery.Deferred<Mof.DmObject, never, never> {
    const r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<object>(
        Settings.baseUrl +
        "api/items/get/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extent) +
        "/" +
        encodeURIComponent(id)
    ).done(x => {
        var dmObject =
            Mof.convertJsonObjectToDmObject(x);
        r.resolve(dmObject);
    });

    return r;
}

export function loadObjectByUri(workspace: string, url: string): JQuery.Deferred<Mof.DmObject, never, never> {
    var r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<object>(
        Settings.baseUrl +
        "api/items/get/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(url)
    ).done(x => {
        var dmObject =
            Mof.convertJsonObjectToDmObject(x);
        r.resolve(dmObject);
    });

    return r;
}

export function loadRootElementsFromExtent(workspace: string, extentUri: string): JQuery.Deferred<Array<Mof.DmObject>, never, never> {
    var r = jQuery.Deferred<Array<Mof.DmObject>, never, never>();

    ApiConnection.get<string>(
        Settings.baseUrl +
        "api/items/get_root_elements/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri)
    ).done(text => {
        const x = JSON.parse(text);
        let result = new Array<Mof.DmObject>();
        for (let n in x) {
            if (Object.prototype.hasOwnProperty.call(x, n)) {
                const v = x[n];
                result.push(Mof.convertJsonObjectToDmObject(v));
            }
        }

        r.resolve(result);
    });

    return r;
}

export function storeObjectByUri(workspace: string, url: string, element: DmObject) : JQuery.Deferred<void, never, never> {
    var r = jQuery.Deferred<void, never, never>();
    var result = Mof.createJsonFromObject(element);

    ApiConnection.put<string>(
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