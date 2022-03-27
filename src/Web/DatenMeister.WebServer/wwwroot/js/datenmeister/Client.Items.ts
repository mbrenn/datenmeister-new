import * as Mof from "./Mof"
import {DmObject} from "./Mof"
import * as Settings from "./Settings"
import * as ApiConnection from "./ApiConnection"

export function loadObject(workspace: string, extent: string, id: string): Promise<Mof.DmObject> {
    return new Promise<Mof.DmObject>((resolve, reject) => {

        ApiConnection.get<object>(
            Settings.baseUrl +
            "api/items/get/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(extent) +
            "/" +
            encodeURIComponent(id)
        ).then(x => {
            const dmObject =
                Mof.convertJsonObjectToDmObject(x);
            resolve(dmObject);
        });
    });
}

export function loadObjectByUri(workspace: string, url: string): Promise<Mof.DmObject> {
    return new Promise<Mof.DmObject>((resolve, reject) => {

        ApiConnection.get<object>(
            Settings.baseUrl +
            "api/items/get/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(url)
        ).then(x => {
            const dmObject =
                Mof.convertJsonObjectToDmObject(x);
            resolve(dmObject);
        });
    });
}

export function loadRootElementsFromExtent(workspace: string, extentUri: string): Promise<Array<Mof.DmObject>> {
    return new Promise<Array<Mof.DmObject>>((resolve, reject) => {
        ApiConnection.get<string>(
            Settings.baseUrl +
            "api/items/get_root_elements/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(extentUri)
        ).then(text => {
            const x = JSON.parse(text);
            let result = new Array<Mof.DmObject>();
            for (let n in x) {
                if (Object.prototype.hasOwnProperty.call(x, n)) {
                    const v = x[n];
                    result.push(Mof.convertJsonObjectToDmObject(v));
                }
            }

            resolve(result);
        });
    });
}

export function storeObjectByUri(workspace: string, url: string, element: DmObject): Promise<void> {
    return new Promise<void>((resolve, reject) => {
        const result = Mof.createJsonFromObject(element);

        ApiConnection.put<string>(
            Settings.baseUrl +
            "api/items/set/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(url),
            result
        ).then(x => {
            resolve();
        });
    });
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
    workspaceId: string, itemUrl: string, property: string): Promise<any> {
    const r = new Promise<any>((resolve, reject) => {
        let url = Settings.baseUrl +
            "api/items/get_property/" +
            encodeURIComponent(workspaceId) +
            "/" +
            encodeURIComponent(itemUrl) +
            "?property=" +
            encodeURIComponent(property);
        const result = ApiConnection.get<IGetPropertyResult>(url);
        result.then(x => {
            const dmObject =
                Mof.convertJsonObjectToObjects(x.v);
            resolve(dmObject);
        });
    });

    return r;
}

export function setProperty(
    workspaceId: string, itemUrl: string, property: string, value: any): Promise<any> {
    return new Promise<boolean>(resolve => {
        let url = Settings.baseUrl +
            "api/items/set_property/" +
            encodeURIComponent(workspaceId) +
            "/" +
            encodeURIComponent(itemUrl);
        const result = ApiConnection.put<any>(url, {key: property, value: value});
        result.then(x => {
            resolve(true);
        });
    });
}

export function unsetProperty(
    workspaceId: string, itemUrl: string, property: string): Promise<any> {
    return new Promise<boolean>(resolve => {
        let url = Settings.baseUrl +
            "api/items/unset_property/" +
            encodeURIComponent(workspaceId) +
            "/" +
            encodeURIComponent(itemUrl);
        const result = ApiConnection.put<any>(url, {property: property});
        result.then(x => {
            resolve(true);
        });
    });
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