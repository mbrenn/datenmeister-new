
import * as Settings from "./Settings"
import * as ApiConnection from "./ApiConnection"

export function createXmi(params: ICreateXmiParams)
{
    const r = jQuery.Deferred<any, never, never>();

    let url = Settings.baseUrl +
        "api/extent/create_xmi";

    ApiConnection.post(
        url, params
    ).done((result: any) => {
        r.resolve({success: result.success});
    });
    
    return r;
}


export interface ICreateXmiParams
{
    filePath: string;
    extentUri: string;
    workspace: string;
}

export function deleteExtent(params: IDeleteExtentsParams)
{
    const r = jQuery.Deferred<any, never, never>();

    let url = Settings.baseUrl +
        "api/extent/delete";

    ApiConnection.deleteRequest(
        url, params
    ).done((result: any) => {
        r.resolve({success: result.success});
    });

    return r;
}

export interface IDeleteExtentsParams
{
    extentUri: string;
    workspace: string;
    skipIfNotExisting?: boolean;
}