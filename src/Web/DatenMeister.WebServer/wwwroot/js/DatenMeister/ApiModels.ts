
// Defines the interfaces which are sent to the server
export namespace In {
    export interface IElementPosition {
        workspace: string;
        extentUri: string;
        item: string;
    }

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
}

// Defines the interfaces which are returned by the server
export namespace Out {
    export interface INamedElement {
        name: string;
        extentUri?: string;
        workspace?: string;
        itemId?: string;
    }
}