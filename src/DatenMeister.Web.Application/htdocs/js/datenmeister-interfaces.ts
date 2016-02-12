

/* Stores all the models that can be returned via one of the */
export module ClientResponse {
    export interface ICreateItemResult {
        success: boolean;
        newuri: string;
    }
    export interface IWorkspace {
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
        c: Array<IDataTableColumn>;
    }

    export interface IDataTableColumn {
        title?: string;
        name?: string;
        defaultValue?: any;
        isEnumeration?: boolean;
    }

    export interface IDataTableItem {
        // Stores the url of the object which can be used for reference
        uri: string;
        v: Array<string>;
    }
}

export module PostModels {

    export interface IItemTableQuery {
        searchString?: string;
        offset?: number;
        amount?: number;
    }

    export interface IWorkspaceCreateModel {
        name: string;
        annotation: string;
        type?: string;
    }

    export interface IExtentCreateModel {
        workspace: string;
        contextUri: string;
        filename: string;
        columns: string;
    }

    export class ItemInExtentQuery implements IItemTableQuery {
        searchString: string;
        offset: number;
        amount: number;
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

export namespace View {
    export interface IItemViewSettings {
        readonly: boolean;
    }

    export class ItemViewSettings implements IItemViewSettings {
        readonly: boolean;
    }
}

export namespace Table {

    export class DataTableColumn implements ClientResponse.IDataTableColumn {
        title: string;
        name: string;
        defaultValue: any;
        isEnumeration: boolean;

        constructor(title?: string, name?: string) {
            this.title = title;
            this.name = name;
        }

        withDefaultValue(value: any): DataTableColumn {
            this.defaultValue = value;
            return this;
        }
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
}

export namespace Api {
    export interface ILayout {
        navigateToWorkspaces(): void;
        navigateToExtents(workspaceId: string): void;
        navigateToItems(ws: string, extentUrl: string): void;
        navigateToItem(ws: string, extentUrl: string, itemUrl: string): void;
        setStatus(statusDom: JQuery): void;
    }

    export interface IItemsProvider {
        performQuery(query: PostModels.IItemTableQuery): JQueryDeferred<ClientResponse.IItemsContent>;
    }

    export class FormForItemConfiguration {
        columns: Array<ClientResponse.IDataTableColumn>;

        onOkForm: (data: any) => void;
        onCancelForm: () => void;

        constructor() {
            this.columns = new Array<ClientResponse.IDataTableColumn>();
        }

        addColumn(column: ClientResponse.IDataTableColumn): void {
            this.columns[this.columns.length] = column;
        }
    }
}