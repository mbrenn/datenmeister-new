import * as Settings from './Settings'
import * as ApiModels from "./ApiModels";

export function loadNameOf(elementPosition: ApiModels.In.IElementPosition): JQuery.jqXHR<ApiModels.Out.INamedElement> {
    return $.ajax(
        Settings.baseUrl +
        "api/elements/get_name/" +
        encodeURIComponent(elementPosition.workspace) + "/" +
        encodeURIComponent(elementPosition.extentUri) + "/" +
        encodeURIComponent(elementPosition.item));
}

export function loadNameByUri(elementUri:string): JQuery.jqXHR<ApiModels.Out.INamedElement> {
    return $.ajax(
        Settings.baseUrl +
        "api/elements/get_name/" +
        encodeURIComponent(elementUri));
}