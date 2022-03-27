import * as ApiModels from "./ApiModels";
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
    const r = new Promise<ItemWithNameAndId[]>((resolve, reject) => {
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

    return r;
}

export function loadNameOf(elementPosition: ApiModels.In.IElementPosition): Promise<ApiModels.Out.INamedElement> {
    return ApiConnection.get<ApiModels.Out.INamedElement>(
        Settings.baseUrl +
        "api/elements/get_name/" +
        encodeURIComponent(elementPosition.workspace) + "/" +
        encodeURIComponent(elementPosition.extentUri) + "/" +
        encodeURIComponent(elementPosition.item));
}

export function loadNameByUri(workspaceId: string, elementUri: string): Promise<ApiModels.Out.INamedElement> {
    if (workspaceId === undefined) {
        workspaceId = "_";
    }
    return ApiConnection.get<ApiModels.Out.INamedElement>(
        Settings.baseUrl +
        "api/elements/get_name/" +
        encodeURIComponent(workspaceId) + "/" +
        encodeURIComponent(elementUri));
}

export function findBySearchString(searchString): Promise<IOutFindBySearchString> {
    return ApiConnection.get<IOutFindBySearchString>(
        Settings.baseUrl +
        "api/elements/find_by_searchstring?search=" +
        encodeURIComponent(searchString));
}

export interface IOutFindBySearchString {
    resultType: string;
    reference: ItemWithNameAndId;
}