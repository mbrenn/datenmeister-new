﻿export interface IWorkspace {
    id: string;
    annotation: string;
};

export interface IExtent {
    uri: string;
}

export interface IItemsContent {
    columns: Array<IDataTableColumn>;
    items: Array<IDataTableItem>;
    search: string;
    totalItemCount: number;
    filteredItemCount: number;    
}

export interface IExtentContent extends IItemsContent {
    url: string;
};

export interface IItemContentModel {
    uri: string;
    v: Array<string>;
}

export interface IDataTableColumn {
    title: string;
    name: string;
}

export interface IDataTableItem {
    // Stores the url of the object which can be used for reference
    uri: string;
    v: Array<string>;
}

export class DataTableItem {
    // Stores the url of the object which can be used for reference
    uri: string;
    v: Array<string>;

    constructor() {
        this.uri = "local:///";
        this.v = new Array<string>();
    }
}

export interface IItemTableQuery {
    searchString?: string;
    offset?: number;
    amount?: number;
}

export class ItemInExtentQuery implements IItemTableQuery {
    searchString: string;
    offset: number;
    amount: number;
}

export interface IItemsProvider {
    performQuery(query:  IItemTableQuery): JQueryDeferred<IItemsContent>;
}

/* Stores all the models that can be returned via one of the */
export module ReturnModule {
    export interface ICreateItemResult {
        success: boolean;
        newuri: string;
    }
}

export module PostModels {
    export class WorkspaceCreateModel {
        title: string;
        annotation: string;
        type: string;
    }

    /** This class is used to reference a single object within the database */
    export class ExtentReferenceModel {
        ws: string;
        extent: string;
    }

    export class ItemReferenceModel extends ExtentReferenceModel {
        item: string;
    }

    export class ItemCreateModel extends ExtentReferenceModel {
        container: string
    }

    export class ItemUnsetPropertyModel extends ItemReferenceModel {
        property: string;
    }

    export class ItemDeleteModel extends ItemReferenceModel {
    }

    export class ItemSetPropertyModel extends ItemReferenceModel {
        property: string;
        newValue: string;
    }

    export class ItemSetPropertiesModel extends ItemReferenceModel {
        v: Array<any>;
    }
}

export namespace Api {
    export interface ILayout {
        navigateToWorkspaces(): void;
        navigateToExtents(workspaceId: string): void;
        navigateToItems(ws: string, extentUrl: string): void;
        navigateToItem(ws: string, extentUrl: string, itemUrl: string): void;
        setStatus(statusDom: JQuery): void;
    }
    
    export class FieldConfiguration {
        propertyName: string;
        title: string;
    }

    export class FormForItemConfiguration {
        columns: Array<FieldConfiguration>;

        onOkForm: (data: any) => void;
        onCancelForm: () => void;

        constructor() {
            this.columns = new Array<FieldConfiguration>();
        }

        addColumn(column: FieldConfiguration): void {
            this.columns[this.columns.length] = column;
        }
    }

    
}