import * as Settings from "./Settings"
import * as ApiConnection from "./ApiConnection"
import * as Mof from "./Mof";
import {DmObject} from "./Mof";

export function createXmi(params: ICreateXmiParams) {
    return new Promise<ICreateXmiResult>((resolve, reject) => {

        let url = Settings.baseUrl +
            "api/extent/create_xmi";

        ApiConnection.post(
            url, params
        ).then((result: any) => {
            resolve({success: result.success});
        });
    });
}


export interface ICreateXmiParams {
    filePath: string;
    extentUri: string;
    workspace: string;
}

export interface ICreateXmiResult {
    success: boolean;
}

export function deleteExtent(params: IDeleteExtentsParams) {
    const r = new Promise<IDeleteExtentsResult>((resolve, reject) => {

        let url = Settings.baseUrl +
            "api/extent/delete";

        ApiConnection.deleteRequest(
            url, params
        ).then((result: any) => {
            resolve({success: result.success});
        });
    });

    return r;
}

export interface IDeleteExtentsParams {
    extentUri: string;
    workspace: string;
    skipIfNotExisting?: boolean;
}

export interface IDeleteExtentsResult {
    success: boolean;
}

export function setProperties(workspace: string, extentUri: string, properties: Mof.DmObject): Promise<void> {
    return ApiConnection.post(
        Settings.baseUrl + "api/extent/set_properties/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri),
        Mof.createJsonFromObject(properties)
    );
}

export function getProperties(workspace: string, extentUri: string) {
    return new Promise<DmObject>(resolve => {

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