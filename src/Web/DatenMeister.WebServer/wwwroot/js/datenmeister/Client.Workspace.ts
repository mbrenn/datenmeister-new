import * as Settings from "./Settings"
import * as ApiConnection from "./ApiConnection"

export function createWorkspace(id: string, annotation: string, param?: CreateWorkspaceParams) {
    const r = new Promise<CreateWorkspaceResult>(resolve => {
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
        ).then((result: any) => {
            resolve({success: result.success});
        });
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
    const r = new Promise<DeleteWorkspaceResult>(resolve => {
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
        ).then((result: any) => {
            resolve({success: result.success});
        });
    });

    return r;
}


export interface DeleteWorkspaceParams {
    skipIfExisting: boolean;
}

export interface DeleteWorkspaceResult {
    success: boolean;
}