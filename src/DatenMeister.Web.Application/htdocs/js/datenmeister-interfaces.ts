export interface IWorkspace {
    id: string;
    annotation: string;
};

export interface IExtent {
    uri: string;
}

export interface IExtentContent {
    url: string;
    totalItemCount: number;
    filteredItemCount: number;
    columns: Array<IDataTableColumn>;
    items: Array<IDataTableItem>;
    search: string;
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

export interface IItemTableQuery {
    searchString?: string;
    offset?: number;
    amount?: number;
}

/* Stores all the models that can be returned via one of the */
export module ReturnModule {
    export interface ICreateItemResult {
        success: boolean;
        newuri: string;
    }
}

export module PostModels {

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
}