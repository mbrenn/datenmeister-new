
export enum EntentType{
    Item, 
    Extent, 
    Workspace
}

export interface ItemWithNameAndId
{
    uri?: string;
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