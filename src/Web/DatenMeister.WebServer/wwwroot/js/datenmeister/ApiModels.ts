
/*
// Defines the interfaces which are sent to the server
export namespace In {
    
    export interface IDeleteItemParams
    {
        workspace: string;
        extentUri: string;
        itemId: string;
    }

    export interface IDeleteExtentParams
    {
        workspace: string;
        extentUri: string;
    }

    export interface IDeleteWorkspaceParams
    {
        workspace: string;
    }
}
*/

/* The interface being the equivalent to DatenMeister.WebServer.Models.ItemWithNameAndId 
   To allow proxying of element 
 */

export interface ItemWithNameAndId
{
    uri?: string;
    name?: string;
    extentUri?: string;
    fullName?: string;
    id?: string;
    workspace?: string;
}


export interface ISuccessResult {
    success: boolean
}