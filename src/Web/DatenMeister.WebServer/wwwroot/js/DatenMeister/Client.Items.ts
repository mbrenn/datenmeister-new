import * as Mof from "./Mof"
import {DmObject} from "./Mof"
import * as Settings from "./Settings"
import * as ApiConnection from "./ApiConnection"

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
        const dmObject =
            Mof.convertJsonObjectToDmObject(x);
        r.resolve(dmObject);
    });

    return r;
}

export function loadObjectByUri(workspace: string, url: string): JQuery.Deferred<Mof.DmObject, never, never> {
    const r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<object>(
        Settings.baseUrl +
        "api/items/get/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(url)
    ).done(x => {
        const dmObject =
            Mof.convertJsonObjectToDmObject(x);
        r.resolve(dmObject);
    });

    return r;
}

export function loadRootElementsFromExtent(workspace: string, extentUri: string): JQuery.Deferred<Array<Mof.DmObject>, never, never> {
    const r = jQuery.Deferred<Array<Mof.DmObject>, never, never>();

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

export function storeObjectByUri(workspace: string, url: string, element: DmObject): JQuery.Deferred<void, never, never> {
    const r = jQuery.Deferred<void, never, never>();
    const result = Mof.createJsonFromObject(element);

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

export function setMetaclass(workspaceId: string, itemUrl: string, newMetaClass: string) {
    let url = Settings.baseUrl +
        "api/items/set_metaclass/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return ApiConnection.post(
        url,
        {metaclass: newMetaClass});
}
