/* Stores all the models that can be returned via one of the */
import Datenmeisterlayout = require("datenmeister-layout");
import * as DMRibbon from  "./datenmeister-ribbon";


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
        columns: Array<IDataField>;
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
        c: Array<IDataField>;
        metaclass?: IItemModel;
        layer: string;
    }

    export interface IDataField {
        type: string;
        title?: string;
        name?: string;
        defaultValue?: any;
        isEnumeration?: boolean;
    }

    export interface IDropDownDataField extends IDataField {
        values: Array<string>;
    }

    export interface ITextDataField extends IDataField {
        lineHeight: number;
    }

    export interface IDateTimeDataField extends IDataField {
        showDate: boolean;
        showTime: boolean;
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

    export class DataField implements ClientResponse.IDataField {
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

        withDefaultValue(value: any): DataField {
            this.defaultValue = value;
            return this;
        }
    }

    export class TextDataField extends DataField implements ClientResponse.ITextDataField {

        lineHeight: number;

        constructor(title?: string, name?: string) {
            super(title, name);
            this.type = ColumnTypes.textbox;
            this.lineHeight = 1;
        }
    }

    export class DateTimeDataField extends DataField implements ClientResponse.IDateTimeDataField {

        showDate: boolean;
        showTime: boolean;

        constructor(title?: string, name?: string) {
            super(title, name);
            this.type = ColumnTypes.dropdown;
            this.showDate = true;
            this.showTime = true;
        }
    }

    export class DropDownDataField extends DataField implements ClientResponse.IDropDownDataField {
        values: Array<string>;

        constructor(title?: string, name?: string) {
            super(title, name);
            this.type = ColumnTypes.dropdown;
        }
    }

    export class ColumnTypes {
        static textbox = "textbox";
        static dropdown = "dropdown";
        static dateTime = "datetime";
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
        renavigate(): void;
        navigateToWorkspaces(): void;
        navigateToExtents(workspaceId: string): void;
        navigateToItems(ws: string, extentUrl: string): void;
        navigateToItem(ws: string, extentUrl: string, itemUrl: string): void;
        setStatus(statusDom: JQuery): void;
        throwLayoutChangedEvent(data: ILayoutChangedEvent): void;

        showDialogNewWorkspace(): void;
        showNavigationForNewExtents(workspace: string): void;

        getRibbon(): DMRibbon.Ribbon;
    }

    export interface IItemsProvider {
        performQuery(query: PostModels.IItemTableQuery): JQueryDeferred<ClientResponse.IItemsContent>;
    }

    export class FormForItemConfiguration {
        columns: Array<ClientResponse.IDataField>;

        onOkForm: (data: any) => void;
        onCancelForm: () => void;

        constructor() {
            this.columns = new Array<ClientResponse.IDataField>();
        }

        addColumn(column: ClientResponse.IDataField): void {
            this.columns[this.columns.length] = column;
        }
    }

    export class DialogConfiguration extends FormForItemConfiguration {
        ws: string;
        extent: string;
    }

    export enum PageType {
        Workspaces,
        Extents,
        Items,
        ItemDetail,
        Dialog
    }

    export interface ILayoutChangedEvent {
        layout?: ILayout;   // Will be set at the thrower
        type: PageType;
        workspace?: string;
        extent?: string;
        item?: string;
    }

    export class PluginParameter {
        version: string;
        layout: ILayout;
    }

    export interface IPluginResult {
        onViewPortChanged?: (ev: ILayoutChangedEvent) => void;
    }
}