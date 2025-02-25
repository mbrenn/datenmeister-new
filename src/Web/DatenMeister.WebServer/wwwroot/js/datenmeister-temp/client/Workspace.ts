import * as Settings from "../Settings.js"
import * as ApiConnection from "../ApiConnection.js"

export function createWorkspace(id: string, annotation: string, param?: CreateWorkspaceParams) {
    return new Promise<CreateWorkspaceResult>(resolve => {
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
            resolve(result);
        });
    });
}

export interface CreateWorkspaceParams {
    skipIfExisting?: boolean
}

export interface CreateWorkspaceResult {
    success: boolean;
    skipped: boolean;
}

export function deleteWorkspace(id: string, param?: DeleteWorkspaceParams) {
    return new Promise<DeleteWorkspaceResult>(resolve => {
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
            resolve(result);
        });
    });
}


export interface DeleteWorkspaceParams {
    skipIfExisting: boolean;
}

export interface DeleteWorkspaceResult {
    success: boolean;
    skipped: boolean;
}