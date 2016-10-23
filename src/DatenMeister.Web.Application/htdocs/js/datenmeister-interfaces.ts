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
        columns: IDataForm;
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
        c: IDataForm;
        metaclass?: IItemModel;
        layer: string;
    }

    export interface IDataForm {
        fields: Array<IFieldData>;
        name: string;

    }

    export interface IFieldData {
        fieldType: string;
        title?: string;
        name?: string;
        defaultValue?: any;
        isEnumeration?: boolean;
        isReadOnly?: boolean;
    }

    export interface IDropDownFieldData extends IFieldData {
        values: Array<string>;
    }

    export interface ITextFieldData extends IFieldData {
        lineHeight: number;
    }

    export interface IDateTimeFieldData extends IFieldData {
        showDate: boolean;
        showTime: boolean;
    }

    export interface ISubElementsFieldData extends IFieldData {
    }

    export interface IDataTableItem {
        // Stores the url of the object which can be used for reference
        uri: string;
        v: Array<string>;
    }

    export interface IExtentCreateableTypeResult {
        types: Array<IItemModel>;
    }

    export interface IExtentViews {
        views: Array<IItemModel>;
    }
}

export module PostModels {

    export interface IItemTableQuery {
        view: string;
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
        filename?: string;
        name?: string;
        columns?: string;
    }

    export class ItemInExtentQuery implements IItemTableQuery {
        searchString: string;
        offset: number;
        amount: number;
        view: string;
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

    export class DataField implements ClientResponse.IFieldData {
        fieldType: string;
        title: string;
        name: string;
        defaultValue: any;
        isEnumeration: boolean;
        isReadOnly: boolean;

        constructor(fieldType: string, title?: string, name?: string) {
            this.fieldType = fieldType;
            this.title = title;
            this.name = name;
        }

        withDefaultValue(value: any): DataField {
            this.defaultValue = value;
            return this;
        }

        asReadOnly(): DataField {
            this.isReadOnly = true;
            return this;
        }
    }

    export class TextDataField extends DataField implements ClientResponse.ITextFieldData {

        lineHeight: number;

        constructor(title?: string, name?: string) {
            super(ColumnTypes.textbox, title, name);
            this.lineHeight = 1;
        }
    }

    export class DateTimeDataField extends DataField implements ClientResponse.IDateTimeFieldData {

        showDate: boolean;
        showTime: boolean;

        constructor(title?: string, name?: string) {
            super(ColumnTypes.dateTime, title, name);
            this.showDate = true;
            this.showTime = true;
        }
    }

    export class DropDownDataField extends DataField implements ClientResponse.IDropDownFieldData {
        values: Array<string>;

        constructor(title?: string, name?: string) {
            super(ColumnTypes.dropdown, title, name);
        }
    }

    export class SubElementsDataField extends DataField implements ClientResponse.ISubElementsFieldData {
        
        constructor(title?: string, name?: string) {
            super(ColumnTypes.subElements, title, name);
        }
    }

    export class ColumnTypes {
        static textbox = "text";
        static dropdown = "dropdown";
        static dateTime = "datetime";
        static subElements = "subelements";
    }

    export class DataTableItem {
        // stores the url of the object which can be used for reference
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
        navigateToItems(ws: string, extentUrl: string, viewUrl?: string): void;
        navigateToItem(ws: string,
            extentUrl: string,
            itemUrl: string,
            viewUrl?: string,
            settings?: View.IItemViewSettings): void;

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
        columns: Array<ClientResponse.IFieldData>;

        onOkForm: (data: any) => void;
        onCancelForm: () => void;

        constructor() {
            this.columns = new Array<ClientResponse.IFieldData>();
        }

        addColumn(column: ClientResponse.IFieldData): void {
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