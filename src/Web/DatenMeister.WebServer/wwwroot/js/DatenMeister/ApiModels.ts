
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
export namespace Out {
    export interface INamedElement {
        name: string;
        extentUri?: string;
        workspace?: string;
        itemId?: string;
    }
}