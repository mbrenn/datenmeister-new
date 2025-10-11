import {ItemWithNameAndId} from "../ApiModels.js";
import * as ApiConnection from "../ApiConnection.js"
import * as Settings from "../Settings.js"
import * as Mof from "../Mof.js";
import { convertToMofObjects } from "./Items.js"
import {param} from "jquery";

export function getAllWorkspaces(): Promise<ItemWithNameAndId[]> {
    return load(undefined, undefined);
}

export function getAllExtents(workspaceId: string): Promise<ItemWithNameAndId[]> {
    return load(workspaceId, undefined);
}

export function getAllRootItems(workspaceId: string, extentUri: string): Promise<ItemWithNameAndId[]> {
    return load(workspaceId, extentUri);
}

export function getAllChildItems(workspaceId: string, itemUrl: string): Promise<ItemWithNameAndId[]> {
    return load(workspaceId, itemUrl);
}

/*
 * Loads the items from the server.
 * The ItemUri may be an extent (then the root items will be loaded)
 * or may be an item (then the composite children will be loaded)
 */
async function load(workspaceId: string, itemUri: string): Promise<ItemWithNameAndId[]> {
    let url = "/api/elements/get_composites";
    if (workspaceId !== undefined && workspaceId !== null) {
        url += '/' + encodeURIComponent(workspaceId);

        if (itemUri !== undefined && itemUri !== null) {
            url += '/' + encodeURIComponent(itemUri);
        }
    }

    return await ApiConnection.get<ItemWithNameAndId[]>(url);
}

export async function loadNameOf(workspaceId: string, extentUri: string, itemUri: string): Promise<ItemWithNameAndId> {
    return await ApiConnection.get<ItemWithNameAndId>(
        Settings.baseUrl +
        "api/elements/get_name/" +
        encodeURIComponent(workspaceId) + "/" +
        encodeURIComponent(extentUri) + "/" +
        encodeURIComponent(itemUri));
}


export async function loadNameByUri(workspaceId: string, elementUri: string): Promise<ItemWithNameAndId> {
    if (workspaceId === undefined) {
        workspaceId = "_";
    }

    const getUrl = Settings.baseUrl +
        "api/elements/get_name/" +
        encodeURIComponent(workspaceId) + "/" +
        encodeURIComponent(elementUri);

    return await ApiConnection.get<ItemWithNameAndId>(getUrl);
}

export interface ICreateTemporaryElementResult {
    success: boolean;
    
    workspace: string;
    
    uri: string;

    metaClassUri: string;

    metaClassWorkspace: string;
    
    id: string;
}

export async function createTemporaryElement(metaClassUri?: string) : Promise<ICreateTemporaryElementResult> {
    return await ApiConnection.put<ICreateTemporaryElementResult>(
        Settings.baseUrl +
        "api/elements/create_temporary_element", {
            metaClassUri: metaClassUri
        });
}

export function findBySearchString(searchString): Promise<IFindBySearchStringResult> {
    return ApiConnection.get<IFindBySearchStringResult>(
        Settings.baseUrl +
        "api/elements/find_by_searchstring?search=" +
        encodeURIComponent(searchString));
}

export interface IFindBySearchStringResult {
    resultType: string;
    reference: ItemWithNameAndId;
}

/**
 * Defines the interface to query objects from the server via a specific QueryStatement
 */
export interface IQueryObjectParameter {
    /**
     * The query itself to be used
     */
    query?: Mof.JsonFromMofObject;
    /**
     * The workspace of the dynamic source which will be added as 'input'
     */
    dynamicSourceWorkspaceId?: string;
    
    /**
     * The item Uri of the dynamic source which will be added as 'input'    
     */
    dynamicSourceItemUri?: string;

    /**
     * Defines the timeout in seconds. If the query is not finished in the given time, the query will be aborted.
     */
    timeout?: number;
}

/**
 * Defines the result of the query being sent to the server
 */
export interface IQueryObjectResult {
    /**
     * Enumeration of found elements
     */
    result: Array<Mof.DmObject>;    
}

/**
 * Calls the server to query the object according the provided parameters
 * @param query The query that is being used to query the object
 * @param parameters Additional parameters, the query object will be included into that element
 */
export async function queryObject(query: Mof.DmObject, parameters?: IQueryObjectParameter): Promise<IQueryObjectResult> {

    if (parameters === undefined) {
        parameters = {};
    }

    if (parameters.query === undefined || parameters.query === null) {
        parameters.query = Mof.createJsonFromObject(query);
    }

    const result = await ApiConnection.post<any>(
        Settings.baseUrl +
        "api/elements/query_object",
        parameters);

    return {
        result: convertToMofObjects(result.result)
    };
}