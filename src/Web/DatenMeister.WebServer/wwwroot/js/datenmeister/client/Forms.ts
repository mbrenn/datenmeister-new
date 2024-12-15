import * as Mof from "../Mof.js"
import * as Settings from "../Settings.js"
import * as ApiConnection from "../ApiConnection.js"
import { ItemWithNameAndId } from "../ApiModels.js"
import { FormType } from "../forms/Interfaces.js";

/*
    Gets the default form for an extent uri by the webserver
 */
export async function getCollectionFormForExtent(workspace: string, extentUri: string, viewMode: string) {
    const resultFromServer = await ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/default_for_extent/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri) +
        "/" +
        encodeURIComponent(viewMode)
    );

    return Mof.convertJsonObjectToDmObject(resultFromServer);
}

/*
    Gets the default form for an extent uri by the webserver
 */
export async function getObjectFormForMetaClass(metaClassUri: string, viewMode?: string) {

    const viewModeUri = 
        viewMode === undefined || viewMode === "" ? 
            "" : 
            "/" + encodeURI(viewMode);

    if (metaClassUri === undefined || metaClassUri === null || metaClassUri === '') {
        // Replaces empty metaclassUri by '_' to match URI-pattern
        metaClassUri = '_';
    }
        
    const resultFromServer = await ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/default_object_for_metaclass/" +
        encodeURIComponent(metaClassUri) +
        viewModeUri
    );

    return Mof.convertJsonObjectToDmObject(resultFromServer);
}
    
export async function getForm(formUri: string, formType?: FormType): Promise<Mof.DmObjectWithSync> {
    const resultFromServer = await ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/get/" +
        encodeURIComponent(formUri) +
        (formType === undefined ? "" : "?formtype=" + encodeURIComponent(formType))
    );
    
    return Mof.convertJsonObjectToDmObject(resultFromServer);
}

/*
    Gets the default form for a certain item by the webserver
 */
export async function getObjectFormForItem(workspace: string, item: string, viewMode: string) {
    const resultFromServer = await ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/default_for_item/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(item) +
        "/" +
        encodeURIComponent(viewMode)
    );
    return Mof.convertJsonObjectToDmObject(resultFromServer);
}

export interface ICreateCollectionFormForExtentResult {
    createdForm: ItemWithNameAndId;
}

export async function createCollectionFormForExtent(workspace: string, extentUri: string, viewMode?: string):
    Promise<ICreateCollectionFormForExtentResult> {
    return await ApiConnection.post<ICreateCollectionFormForExtentResult>(
        Settings.baseUrl +
        "api/forms/create_collection_form_for_extent/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri) +
        "/" +
        encodeURIComponent(viewMode ?? ""),
        {}
    );
}

export interface ICreateObjectFormForItemResult {
    createdForm: ItemWithNameAndId;
}

export async function createObjectFormForItem(workspace: string, extentUri: string, viewMode?: string):
    Promise<ICreateObjectFormForItemResult> {
    return await ApiConnection.post<ICreateObjectFormForItemResult>(
        Settings.baseUrl +
        "api/forms/create_object_form_for_item/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri) +
        "/" +
        encodeURIComponent(viewMode ?? ""),
        {}
    );
}

export interface GetViewModesResultServer{
    viewModes: Array<string>;
}

export interface IGetViewModesResult {
    viewModes: Array<Mof.DmObject>;
}

export async function getViewModes() : Promise<IGetViewModesResult> {
    const resultFromServer = await ApiConnection.get<GetViewModesResultServer>(
        Settings.baseUrl +
        "api/forms/get_viewmodes");

    const result =
        {
            viewModes: new Array<Mof.DmObject>()
        };
    
    for (let n in resultFromServer.viewModes) {
        const value = resultFromServer.viewModes[n];

        result.viewModes.push(Mof.convertJsonObjectToDmObject(value));
    }

    return result;
}


export interface IGetDefaultViewModesResult {
    viewMode: Mof.DmObject;
}

export async function getDefaultViewMode(workspace: string, extentUri: string){
    const apiResult = await ApiConnection.get<IGetDefaultViewModesResult>(
        Settings.baseUrl +
        "api/forms/get_default_viewmode/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(extentUri));
        
    return {
        viewMode: Mof.convertJsonObjectToDmObject(apiResult.viewMode)
    }
}