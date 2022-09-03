
export enum EntentType {
    Item= "Item",
    Extent = "Extent",
    Workspace = "Workspace"
}

/**
 * Correspondent to DatenMeister.Json.ItemLink.cs
 */
export interface ItemLink {
    workspace?: string;
    uri: string;
}

/**
 * Correspondent to DatenMeister.Json.ItemWithNameAndId
 */
export interface ItemWithNameAndId {
    uri: string;
    name?: string;
    extentUri?: string;
    fullName?: string;
    id?: string;
    workspace?: string;
    typeName?: string;
    typeUri?: string;
    ententType?: EntentType;
}   


export interface ISuccessResult {
    success: boolean
}