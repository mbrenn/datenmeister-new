import * as Settings from "../Settings.js"
import * as ApiConnection from "../ApiConnection.js"
import {ISuccessResult, ItemWithNameAndId} from "../ApiModels.js";

export async function getAllTypes() {
    return await ApiConnection.get<Array<ItemWithNameAndId>>(
        Settings.baseUrl + "api/types/all");
}

/**
 * Gets the type of the property by referring to one metaClass and the propertyName
 * @param workspace Workspace in which the element was queried. 
 * @param metaClass Uri of the metaClass to be queried
 * @param propertyName Name of the metaclass' property to which the type of the property is queried
 */
export async function getPropertyType(workspace: string, metaClass: string, propertyName: string)
{
    try {
        return await ApiConnection.get<ItemWithNameAndId>(
            Settings.baseUrl + "api/types/propertytype/"
            + encodeURIComponent(workspace) + "/"
            + encodeURIComponent(metaClass) + "/"
            + encodeURIComponent(propertyName));
    }
    catch(error)
    {
        console.log(error);
        return undefined;
    }
}