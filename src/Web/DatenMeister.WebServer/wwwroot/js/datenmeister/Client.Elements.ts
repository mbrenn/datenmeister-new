import {ItemWithNameAndId} from "./ApiModels";
import * as ApiConnection from "./ApiConnection"
import * as Settings from "./Settings"

export function getAllWorkspaces(): Promise<ItemWithNameAndId[]> {
    return load(undefined, undefined, undefined);
}

export function getAllExtents(workspaceId: string): Promise<ItemWithNameAndId[]> {
    return load(workspaceId, undefined, undefined);
}

export function getAllRootItems(workspaceId: string, extent: string): Promise<ItemWithNameAndId[]> {
    return load(workspaceId, extent, undefined);
}

export function getAllChildItems(workspaceId: string, extent: string, itemId: string): Promise<ItemWithNameAndId[]> {
    return load(workspaceId, extent, itemId);
}

function load(workspaceId: string, extent: string, itemId: string): Promise<ItemWithNameAndId[]> {
    return new Promise<ItemWithNameAndId[]>((resolve, reject) => {
        let url = '/api/elements/get_composites';
        if (workspaceId !== undefined && workspaceId !== null) {
            url += '/' + encodeURIComponent(workspaceId);

            if (extent !== undefined && extent !== null) {
                if (itemId !== undefined && itemId !== null) {
                    url += '/' + encodeURIComponent(extent + '#' + itemId);
                } else {
                    url += '/' + encodeURIComponent(extent);
                }
            }
        }

        ApiConnection.get<ItemWithNameAndId[]>(url).then(items => {
                resolve(items);
            }
        );
    });
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