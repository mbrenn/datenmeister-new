import {ItemWithNameAndId} from "./ApiModels";
import * as ApiConnection from "./ApiConnection"
import * as Settings from "./Settings"

export function getAllWorkspaces() : JQueryPromise<ItemWithNameAndId[]> {
    return load(undefined, undefined, undefined);
}

export function getAllExtents(workspaceId: string): JQueryPromise<ItemWithNameAndId[]> {
    return load(workspaceId, undefined, undefined);
}

export function getAllRootItems(workspaceId: string, extent: string): JQueryPromise<ItemWithNameAndId[]> {
    return load(workspaceId, extent, undefined);
}

export function getAllChildItems(workspaceId: string, extent: string, itemId: string) : JQueryPromise<ItemWithNameAndId[]> {
    return load(workspaceId, extent, itemId);
}

export function setMetaclass(workspaceId: string, itemUrl: string, newMetaClass: string) {
    let url = Settings.baseUrl +
        "api/items/set_metaclass/" +
        encodeURIComponent(workspaceId) +
        "/" +
        encodeURIComponent(itemUrl);
    return ApiConnection.post(
        url,
        { metaclass: newMetaClass });
}

function load(workspaceId: string, extent: string, itemId: string): JQueryPromise<ItemWithNameAndId[]> {
    const r = jQuery.Deferred<ItemWithNameAndId[], never, never>();

    let url = '/api/elements/get_composites';
    if (workspaceId !== undefined && workspaceId !== null) {
        url += '/' + encodeURIComponent(workspaceId);

        if (extent !== undefined && extent !== null) {
            if (itemId !== undefined && itemId !== null) {
                url += '/' + encodeURIComponent(extent+ '#' + itemId);
            } else {
                url += '/' + encodeURIComponent(extent);
            }
        }
    }

    ApiConnection.get<ItemWithNameAndId[]>(url).done(items => {
            r.resolve(items);
        }
    );

    return r;
}