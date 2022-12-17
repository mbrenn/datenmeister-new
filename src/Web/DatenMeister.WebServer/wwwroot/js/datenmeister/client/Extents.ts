import * as Settings from "../Settings"
import * as ApiConnection from "../ApiConnection"
import * as Mof from "../Mof";

export interface IExistsResult
{
    exists: boolean;
}

export async function exists(workspaceId: string, extent: string) {
    let url = Settings.baseUrl +
        "api/extent/exists/"
        + encodeURIComponent(workspaceId) + "/"
        + encodeURIComponent(extent);

    return await ApiConnection.get<IExistsResult>(url);
}


export class ExportXmiResult
{
    public xmi: string;
}
export async function exportXmi(workspace: string, extentUri: string) {
    let url = Settings.baseUrl +
        "api/extent/export_xmi/"
        + encodeURIComponent(workspace) + "/"
        + encodeURIComponent(extentUri);
    return await ApiConnection.get<ExportXmiResult>(url);
}

export function createXmi(params: ICreateXmiParams) {
    return new Promise<ICreateXmiResult>((resolve, reject) => {

        let url = Settings.baseUrl +
            "api/extent/create_xmi";

        ApiConnection.post(
            url, params
        ).then((result: any) => {
            resolve(result);
        });
    });
}

export interface ICreateXmiParams {
    filePath: string;
    extentUri: string;
    workspace: string;
    skipIfExisting?: boolean;
}

export interface ICreateXmiResult {
    success: boolean;
    skipped: boolean;
}

export function deleteExtent(params: IDeleteExtentsParams) {
    return new Promise<IDeleteExtentsResult>((resolve, reject) => {

        let url = Settings.baseUrl +
            "api/extent/delete";

        ApiConnection.deleteRequest(
            url, params
        ).then((result: any) => {
            resolve(result);
        });
    });
}

export interface IDeleteExtentsParams {
    extentUri: string;
    workspace: string;
    skipIfNotExisting?: boolean;
}

export interface IDeleteExtentsResult {
    success: boolean;
    skipped: boolean;
}

export function setProperties(workspace: string, extentUri: string, properties: Mof.DmObject): Promise<void> {
    return ApiConnection.post(
        Settings.baseUrl + "api/extent/set_properties/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri),
        Mof.createJsonFromObject(properties)
    );
}

export function getProperties(workspace: string, extentUri: string) {
    return new Promise<Mof.DmObject>(resolve => {

        ApiConnection.get<object>(
            Settings.baseUrl + "api/extent/get_properties/"
            + encodeURIComponent(workspace)
            + "/" + encodeURIComponent(extentUri))
            .then(x => {
                const dmObject =
                    Mof.convertJsonObjectToDmObject(x);
                resolve(dmObject);
            });
    });
}