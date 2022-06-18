import * as Mof from "../Mof"
import * as Settings from "../Settings"
import * as ApiConnection from "../ApiConnection"
import {ISuccessResult, ItemWithNameAndId} from "../ApiModels";

export async function createItemInExtent(workspaceId: string, extentUri: string, param: ICreateItemInExtentParams) {
    const evaluatedParameter =
        {
            metaClass: param.metaClass,
            properties: undefined
        };

    if (param.properties !== undefined && param.properties !== null) {
        evaluatedParameter.properties = Mof.createJsonFromObject(param.properties);
    }

    return await ApiConnection.post<ICreateItemInExtentResult>(
        Settings.baseUrl + "api/items/create_in_extent/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(extentUri),
        evaluatedParameter
    );
}

export interface ICreateItemInExtentParams {
    metaClass?: string;
    properties?: Mof.DmObject;
}

export interface ICreateItemInExtentResult{
    success: boolean;
    itemId: string;
}

export async function createItemAsChild(workspaceId: string, itemUri: string, param: ICreateItemAsChildParams)
{
    const evaluatedParameter =
        {
            metaClass: param.metaClass,
            property: param.property,
            asList: param.asList ?? false,            
            properties: undefined
        };

    if (param.properties !== undefined && param.properties !== null) {
        evaluatedParameter.properties = Mof.createJsonFromObject(param.properties);
    }

    return await ApiConnection.post<ICreateItemAsChildResult>(
        Settings.baseUrl + "api/items/create_child/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(itemUri),
        evaluatedParameter
    );
}

export interface ICreateItemAsChildParams {
    metaClass?: string;
    property: string;
    asList?: boolean;
    properties?: Mof.DmObject;    
}

export interface ICreateItemAsChildResult
{
    success: boolean;
    itemId: string;
}

export async function deleteRootElements(workspaceId: string, extentUri: string)
{
    return await ApiConnection.deleteRequest<ISuccessResult>(
        Settings.baseUrl + "api/items/delete_root_elements/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(extentUri), {}
    );
}

export async function deleteItem(workspaceId: string, itemUri: string) {
    return await ApiConnection.deleteRequest<ISuccessResult>(
        Settings.baseUrl + "api/items/delete/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(itemUri),
        {}
    );
}

export async function deleteItemFromExtent(workspaceId: string, itemUrl: string) {
    return await ApiConnection.deleteRequest<ISuccessResult>(
        Settings.baseUrl + "api/items/delete_from_extent/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(itemUrl),
        {}
    );
}

export async function getObject(workspace: string, extent: string, id: string) {
    const resultFromServer = ApiConnection.get<object>(
        Settings.baseUrl +
        "api/items/get/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extent) +
        "/" +
        encodeURIComponent(id)
    );

    return Mof.convertJsonObjectToDmObject(resultFromServer);
}

export async function getObjectByUri(workspace: string, url: string): Promise<Mof.DmObject | undefined> {
    try {
        const resultFromServer = await ApiConnection.get<object>(
            Settings.baseUrl +
            "api/items/get/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(url)
        );

        return Mof.convertJsonObjectToDmObject(resultFromServer);
    }
    catch(e)
    {
        return undefined;
    }
}

export async function getRootElements(workspace: string, extentUri: string): Promise<Array<Mof.DmObject>> {
    const resultFromServer = await ApiConnection.get<string>(
        Settings.baseUrl +
        "api/items/get_root_elements/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri)
    );

    const x = JSON.parse(resultFromServer);
    let result = new Array<Mof.DmObject>();
    for (let n in x) {
        if (Object.prototype.hasOwnProperty.call(x, n)) {
            const v = x[n];
            result.push(Mof.convertJsonObjectToDmObject(v));
        }
    }

    return result;
}

export async function getContainer(workspaceId: string, itemUri: string, self?: boolean): Promise<Array<ItemWithNameAndId>> {
    
    let uri = Settings.baseUrl + "api/items/get_container/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(itemUri);
    
    if ( self === true )
    {
        uri += "?self=true";
    }
    
    return await ApiConnection.get<Array<ItemWithNameAndId>>(uri);
}

export async  function setProperty(
    workspaceId: string, itemUrl: string, property: string, value: any): Promise<ISuccessResult> {
    let url = Settings.baseUrl +
        "api/items/set_property/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return await ApiConnection.put<any>(url, {key: property, value: value});
}
export async function unsetProperty(
    workspaceId: string, itemUrl: string, property: string): Promise<ISuccessResult> {
    let url = Settings.baseUrl +
        "api/items/unset_property/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);

    return await ApiConnection.post<any>(url, {property: property});
}

export async function setPropertiesByStringValues(workspaceId: string, itemUrl: string, params: ISetPropertiesParams): Promise<ISuccessResult> {
    let url = Settings.baseUrl +
        "api/items/unset_property/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    
    return await ApiConnection.post(url, params);    
}

export interface ISetPropertyParam
{
    key: string;
    value: string;
}

export interface ISetPropertiesParams {
    properties: Array<ISetPropertyParam>;
}

export async function setProperties(workspaceId: string, itemUrl: string, properties: Mof.DmObject): Promise<ISuccessResult> {    
    let url = Settings.baseUrl +
        "api/items/set/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);

    return await ApiConnection.put(
        url,
        Mof.createJsonFromObject(properties));
}

export async function getProperty(
    workspaceId: string, itemUrl: string, property: string): Promise<any> {

    let url = Settings.baseUrl +
        "api/items/get_property/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl) +
        "?property=" +
        encodeURIComponent(property);
    const result = await ApiConnection.get<IGetPropertyResult>(url);
    return Mof.convertJsonObjectToObjects(result.v);
}

export async function setMetaclass(workspaceId: string, itemUrl: string, newMetaClass: string) {
    let url = Settings.baseUrl +
        "api/items/set_metaclass/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return await ApiConnection.post(
        url,
        {metaclass: newMetaClass});
}

export interface IGetPropertyResult {
    v: any;
}

export async function addReferenceToCollection(
    workspaceId: string, itemUrl: string, parameter: IAddReferenceToCollectionParameter) {
    let url = Settings.baseUrl +
        "api/items/add_ref_to_collection/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);

    await ApiConnection.post(
        url,
        {
            property: parameter.property,
            workspaceId: parameter.workspaceId,
            referenceUri: parameter.referenceUri
        }
    );
}


export interface IAddReferenceToCollectionParameter {
    property: string;
    referenceUri: string;
    workspaceId?: string;
}

export async function setPropertyReference(
    workspaceId: string, itemUrl: string, parameter: ISetPropertyReferenceParams): Promise<ISuccessResult> {
    let url = Settings.baseUrl +
        "api/items/set_property_reference/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);

    return await ApiConnection.post(
        url,
        {
            property: parameter.property,
            workspaceId: parameter.workspaceId,
            referenceUri: parameter.referenceUri
        }
    );
}

export interface ISetPropertyReferenceParams {
    property: string;
    referenceUri: string;
    workspaceId?: string;
}

export async function removeReferenceFromCollection(
    workspaceId: string, itemUrl: string, parameter: IRemoveReferenceFromCollectionParameter) {
    let url = Settings.baseUrl +
        "api/items/remove_ref_to_collection/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);

    await ApiConnection.post(
        url,
        {
            property: parameter.property,
            workspaceId: parameter.referenceWorkspaceId,
            referenceUri: parameter.referenceUri
        }
    );
}

export interface IRemoveReferenceFromCollectionParameter {
    property: string;
    referenceUri: string;
    referenceWorkspaceId?: string;
}