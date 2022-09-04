import * as Mof from "../Mof"
import * as Settings from "../Settings"
import * as ApiConnection from "../ApiConnection"
import {ISuccessResult, ItemWithNameAndId} from "../ApiModels";

export async function getAllTypes() {
    return await ApiConnection.get<Array<ItemWithNameAndId>>(
        Settings.baseUrl + "api/types/all");
}

/**
 * Gets the type of the property by referring to one metaClass and the propertyName
 * @param metaClass Uri of the metaClass to be queried
 * @param propertyName
 */
export async function getPropertyType(workspace: string, metaClass: string, propertyName: string) {
    return await ApiConnection.get<ItemWithNameAndId>(
        Settings.baseUrl + "api/types/propertytype/"
        + encodeURIComponent(workspace) + "/"
        + encodeURIComponent(metaClass) + "/"
        + encodeURIComponent(propertyName));
}