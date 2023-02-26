import {ItemWithNameAndId} from "../ApiModels";
import * as ApiConnection from "../ApiConnection"
import * as Settings from "../Settings"

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
    return await ApiConnection.get<ItemWithNameAndId>(
        Settings.baseUrl +
        "api/elements/get_name/" +
        encodeURIComponent(workspaceId) + "/" +
        encodeURIComponent(elementUri));
}

export interface ICreateTemporaryElementResult {
    success: boolean;
    
    workspace: string;
    
    uri: string;
}

export async function createTemporaryElement(metaClassUri?: string) : Promise<ICreateTemporaryElementResult> {
    return await ApiConnection.put<ICreateTemporaryElementResult>(
        Settings.baseUrl +
        "api/elements/create_temporary_element", {
            metaClassUri: metaClassUri
        });
}

export function findBySearchString(searchString): Promise<IFindBySearchString> {
    return ApiConnection.get<IFindBySearchString>(
        Settings.baseUrl +
        "api/elements/find_by_searchstring?search=" +
        encodeURIComponent(searchString));
}

export interface IFindBySearchString {
    resultType: string;
    reference: ItemWithNameAndId;
}