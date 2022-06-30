import * as Mof from "../Mof"
import * as Settings from "../Settings"
import * as ApiConnection from "../ApiConnection"


/*
    Gets the default form for an extent uri by the webserver
 */
export async function getDefaultFormForExtent(workspace: string, extentUri: string, viewMode: string) {
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
export async function getDefaultFormForMetaClass(metaClassUri: string) {

    const resultFromServer = await ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/default_for_metaclass/" +
        encodeURIComponent(metaClassUri)
    );

    return Mof.convertJsonObjectToDmObject(resultFromServer);
}


export async function getForm(formUri: string): Promise<Mof.DmObject> {
    const resultFromServer = await ApiConnection.get<object>(
        Settings.baseUrl +
        "api/forms/get/" +
        encodeURIComponent(formUri)
    );
    return Mof.convertJsonObjectToDmObject(resultFromServer);
}

/*
    Gets the default form for a certain item by the webserver
 */
export async function getDefaultFormForItem(workspace: string, item: string, viewMode: string) {
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

export interface GetViewModesResultServer{
    viewModes: Array<string>;
}

export interface GetViewModesResult{
    viewModes: Array<Mof.DmObject>;
}

export async function getViewModes() : Promise<GetViewModesResult> {
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