import {ItemWithNameAndId} from "../ApiModels.js";
import * as ApiConnection from "../ApiConnection.js"
import * as Settings from "../Settings.js"
import * as Mof from "../Mof.js";
import { convertToMofObjects } from "./Items.js"

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

export interface IQueryObjectParameter {
    query: Mof.JsonFromMofObject;
    timeout?: number; // Timeout in seconds
}

export interface IQueryObjectResult {
    result: Array<Mof.DmObject>;
}

export async function queryObject(query: Mof.DmObject, timeout?: number): Promise<IQueryObjectResult> {
    const json = Mof.createJsonFromObject(query);

    const parameters: IQueryObjectParameter = {
        query: json
    };

    if (timeout !== undefined && timeout !== null && timeout > 0) {
        parameters.timeout = timeout;
    }

    const result = await ApiConnection.post<any>(
        Settings.baseUrl +
        "api/elements/query_object",
        parameters);

    return {
        result: convertToMofObjects(result.result)
    };
}