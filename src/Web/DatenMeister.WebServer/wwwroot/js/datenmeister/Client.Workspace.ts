
import * as Settings from "./Settings"
import * as ApiConnection from "./ApiConnection"

export function createWorkspace(id: string, annotation: string, param?: CreateWorkspaceParams) {
    const r = jQuery.Deferred<any, never, never>();

    let url = Settings.baseUrl +
        "api/workspace/create";

    const data: any = {
        id: id,
        annotation: annotation
    };

    if (param?.skipIfExisting !== undefined) {
        data.skipIfExisting = param.skipIfExisting;
    }

    ApiConnection.put(
        url,
        data
    ).done((result: any) => {
        r.resolve({success: result.success});
    });

    return r;
}

export interface CreateWorkspaceParams {
    skipIfExisting?: boolean
}

export interface CreateWorkspaceResult {
    success: boolean;
}

export function deleteWorkspace(id: string, param?: DeleteWorkspaceParams) {
    const r = jQuery.Deferred<DeleteWorkspaceResult, never, never>();

    let url = Settings.baseUrl +
        "api/workspace/delete";

    const data: any = {
        id: id
    };

    if (param?.skipIfExisting !== undefined) {
        data.skipIfExisting = param.skipIfExisting;
    }

    ApiConnection.deleteRequest(
        url,
        data
    ).done((result: any) => {
        r.resolve({success: result.success});
    });

    return r;
}


export interface DeleteWorkspaceParams {
    skipIfExisting: boolean;
}

export interface DeleteWorkspaceResult {
    success: boolean;
}