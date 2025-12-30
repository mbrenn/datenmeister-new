import * as Mof from "../Mof.js"
import * as Settings from "../Settings.js"
import * as ApiConnection from "../ApiConnection.js"
import {ISuccessResult, ItemWithNameAndId} from "../ApiModels.js";

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
    itemUrl: string;
    workspace: string;
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
    itemUrl: string;
    workspace: string;
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

export async function getObjectByUri(workspace: string, url: string): Promise<Mof.DmObjectWithSync | undefined> {
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

export async function getItemWithNameAndId(workspace: string, url: string): Promise<ItemWithNameAndId | undefined> {
    try {
        const resultFromServer = await ApiConnection.get<ItemWithNameAndId>(
            Settings.baseUrl +
            "api/items/get_itemwithnameandid/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(url)
        );

        return resultFromServer;
    }
    catch(e)
    {
        return undefined;
    }
}

export interface IGetRootElementsParameter{
    /**
     * Url to the view node being used to retrieve the elements
     */
    viewNode?: string;    
    orderBy?: string; // Property to which the ordering shall be done
    orderByDescending?: boolean; // Flag, whether ordering shall be done by descending
    filterByProperties?: Array<string>; // Property filters. Key is Propertyname, Value is textfilter
    filterByFreetext?: string; // Additional freetext
}

export interface IGetRootElementsResult {
    success: boolean;
    rootElements: string;
    rootElementsAsObjects: Array<Mof.DmObject>
    message: string;   
}

export async function getRootElements(workspace: string, extentUri: string, parameter?: IGetRootElementsParameter): Promise<IGetRootElementsResult> {
    // Handle issue that empty urls cannot be resolved by ASP.Net, so we need to include a Workspace Name
    if(workspace === undefined || workspace === "" || workspace === null) {
        workspace = "Data";
    }
    
    let url = Settings.baseUrl +
        "api/items/get_root_elements/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri);

    const queryParams: string[] = [];

    // Checks if there is a view node being attached
    if (parameter?.viewNode !== undefined) {
        queryParams.push("viewNode=" + encodeURIComponent(parameter.viewNode));
    }

    if (parameter?.orderBy !== undefined && parameter?.orderBy !== '') {
        queryParams.push("orderBy=" + encodeURIComponent(parameter.orderBy));

        if (parameter?.orderByDescending !== undefined) {
            // Only order by descending in case orderBy is set
            queryParams.push("orderByDescending=" + encodeURIComponent(parameter.orderByDescending ? "true" : "false"));
        }
    }

    if (parameter?.filterByFreetext !== undefined && parameter?.filterByFreetext !== '') {
        queryParams.push("filterByFreetext=" + encodeURIComponent(parameter.filterByFreetext));
    }

    if (parameter?.filterByProperties !== undefined) {
        const serialized = serializeArrayToString(parameter.filterByProperties);
        if (serialized !== "" && serialized !== undefined) {
            queryParams.push("filterByProperties=" + serialized);
        }
    }

    // Join query parameters with '&' and append them to the URL
    if (queryParams.length > 0) {
        url += '?' + queryParams.join('&');
    }

    const resultFromServer = await ApiConnection.get<IGetRootElementsResult>(url);
    resultFromServer.rootElementsAsObjects = convertToMofObjects(resultFromServer.rootElements);
    return resultFromServer;
}

function serializeArrayToString(arrayValue) {
    let result = '';
    let ampersand = '';
    for (var key in arrayValue) {
        const value = arrayValue[key];
        result += `${ampersand}${encodeURIComponent(key)}=${ encodeURIComponent(value) }`;

        ampersand = '&';
    }

    return encodeURIComponent(result);
}

export function convertToMofObjects(resultFromServer: string) {
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

export async function getElements(queryUri: string): Promise<Array<Mof.DmObject>> {    
    let url = Settings.baseUrl +
        "api/items/get_elements/" +
        encodeURIComponent(queryUri);

    const resultFromServer = await ApiConnection.get<string>(url);
    return convertToMofObjects(resultFromServer);
}

export interface ISetIdParams {
    id: string;
}

export interface ISetIdResult {
    success: boolean;
    newUri: string;
}

export async function setId(workspaceId: string, itemUrl: string, newId: string) {
    let url = Settings.baseUrl +
        "api/items/set_id/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return await ApiConnection.post<ISetIdResult>(url, {id: newId} as ISetIdParams);
}

export async function getRootElementsAsItem(workspace: string, extentUri: string, parameter?: IGetRootElementsParameter): Promise<Array<ItemWithNameAndId>> {
    // Handle issue that empty urls cannot be resolved by ASP.Net, so we need to include a Workspace Name
    if (workspace === undefined || workspace === "" || workspace === null) {
        workspace = "Data";
    }

    let url = Settings.baseUrl +
        "api/items/get_root_elements_as_item/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri);

    // Checks, if there is a view node being attached
    if (parameter?.viewNode !== undefined) {
        url += "?viewNode=" + encodeURIComponent(parameter.viewNode);
    }

    return await ApiConnection.get<Array<ItemWithNameAndId>>(url);
}

export async function getElementsAsItem(queryUri: string): Promise<Array<ItemWithNameAndId>> {
    // Handle issue that empty urls cannot be resolved by ASP.Net, so we need to include a Workspace Name    
    let url = Settings.baseUrl +
        "api/items/get_elements_as_item/" +
        encodeURIComponent(queryUri);
    
    return await ApiConnection.get<Array<ItemWithNameAndId>>(url);
}

export async function getContainer(workspaceId: string, itemUri: string, self?: boolean): Promise<Array<ItemWithNameAndId>> {
    
    let uri = Settings.baseUrl + "api/items/get_container/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(itemUri);
    
    if (self === true) {
        uri += "?self=true";
    }
    
    return await ApiConnection.get<Array<ItemWithNameAndId>>(uri);
}

export async function setProperty(
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
        "api/items/set_properties/" +
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
    
    // Result.v is an array of two items. The first one whether it is set, 
    // the second one the value itself. We just return the value. 
    return Mof.convertJsonObjectToObjects(result.v[1]);
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


export class ExportXmiResult
{
    public xmi: string;
}
export async function exportXmi(workspace: string, itemUri: string) {
    let url = Settings.baseUrl +
        "api/items/export_xmi/"
        + encodeURIComponent(workspace) + "/"
        + encodeURIComponent(itemUri);
    return await ApiConnection.get<ExportXmiResult>(url);
}

export class ImportXmiResult
{
    public success: boolean;
}

export async function importXmi(workspace: string, itemUri: string, property: string, addToCollection: boolean, xmi: string) {
    let url = Settings.baseUrl +
        "api/items/import_xmi/"
        + encodeURIComponent(workspace) + "/"
        + encodeURIComponent(itemUri)
        + "?property=" + encodeURIComponent(property)
        + "&addToCollection=" + (addToCollection ? "true" : "false");

    return await ApiConnection.post<ImportXmiResult>(url, {xmi: xmi});
}