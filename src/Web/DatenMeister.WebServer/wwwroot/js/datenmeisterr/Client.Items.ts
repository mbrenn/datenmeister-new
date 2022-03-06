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

export function addReferenceToCollection(
    workspaceId: string, itemUrl: string, parameter: IAddReferenceToCollectionParameter) {
    let url = Settings.baseUrl +
        "api/items/add_ref_to_collection/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);

    return ApiConnection.post(
        url,
        {
            property: parameter.property,
            workspaceId: parameter.referenceWorkspaceId,
            referenceUri: parameter.referenceUri
        }
    );
}

export function removeReferenceFromCollection(
    workspaceId: string, itemUrl: string, parameter: IRemoveReferenceToCollectionParameter) {
    let url = Settings.baseUrl +
        "api/items/remove_ref_to_collection/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);

    return ApiConnection.post(
        url,
        {
            property: parameter.property,
            workspaceId: parameter.referenceWorkspaceId,
            referenceUri: parameter.referenceUri
        }
    );
}

export function getProperty(
    workspaceId: string, itemUrl: string, property: string): JQuery.Deferred<any, never, never> {
    const r = jQuery.Deferred<any, never, never>();
    let url = Settings.baseUrl +
        "api/items/get_property/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl) +
        "?property=" +
        encodeURIComponent(property);
    const result = ApiConnection.get<IGetPropertyResult>(url);
    result.done(x => {
        const dmObject =
            Mof.convertJsonObjectToObjects(x.v);
        r.resolve(dmObject);
    });

    return r;
}

export function unsetProperty(
    workspaceId: string, itemUrl: string, property: string): JQuery.Deferred<any, never, never> {
    const r = jQuery.Deferred<any, never, never>();
    let url = Settings.baseUrl +
        "api/items/unset_property/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    const result = ApiConnection.put<any>(url, {property: property});
    result.done(x => {
        r.resolve(true);
    });

    return r;
}

export interface IGetPropertyResult {
    v: any;
}


export interface IAddReferenceToCollectionParameter {
    property: string;
    referenceUri: string;
    referenceWorkspaceId?: string;
}

export interface IRemoveReferenceToCollectionParameter {
    property: string;
    referenceUri: string;
    referenceWorkspaceId?: string;
}

export interface IUnsetPropertyResult {
    property: string;
}