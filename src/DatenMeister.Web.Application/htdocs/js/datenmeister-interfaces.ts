﻿

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

    export interface IItemModel {
        name: string;
        uri: string;
        ext: string;
        ws: string;
        layer: string;
    }

    export interface IItemContentModel {
        id: string;
        uri: string;
        v: Array<string>;
        c: Array<IDataTableColumn>;
        metaclass?: IItemModel;
        layer: string;
    }

    export interface IDataTableColumn {
        type: string;
        title?: string;
        name?: string;
        defaultValue?: any;
        isEnumeration?: boolean;
    }

    export interface IDataTableDropDown extends IDataTableColumn {
        values: Array<string>;
    }

    export interface IDataTableItem {
        // Stores the url of the object which can be used for reference
        uri: string;
        v: Array<string>;
    }

    export interface IExtentCreateableTypeResult {
        types: Array<IItemModel>
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

    export interface IExtentAddModel {
        type: string;
        workspace: string;
        contextUri: string;
        filename: string;
    }

    export interface IExtentCreateModel {
        type: string;
        workspace: string;
        contextUri: string;
        filename: string;
        columns?: string;
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
        container: string;
        metaclass: string;
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
        isReadonly?: boolean;
    }

    export class ItemViewSettings implements IItemViewSettings {
        isReadonly: boolean;
    }
}

export namespace Table {

    export class DataTableColumn implements ClientResponse.IDataTableColumn {
        type: string;
        title: string;
        name: string;
        defaultValue: any;
        isEnumeration: boolean;

        constructor(title?: string, name?: string) {
            this.type = ColumnTypes.textbox;
            this.title = title;
            this.name = name;
        }

        withDefaultValue(value: any): DataTableColumn {
            this.defaultValue = value;
            return this;
        }
    }

    export class DataTableDropDown extends DataTableColumn  implements  ClientResponse.IDataTableDropDown{
        values: Array<string>;

        constructor(title?: string, name?: string) {
            super(title, name);
            this.type = ColumnTypes.dropdown;
        }
    }

    export class ColumnTypes {
        static textbox = "textbox";
        static dropdown = "dropdown";
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

    export class DialogConfiguration extends FormForItemConfiguration {
        ws: string;
        extent: string;
    }
}