/* Stores all the models that can be returned via one of the */
import * as DMRibbon from  "./datenmeister-ribbon";
import * as DMN from  "./datenmeister-navigation";

export module ClientResponse {

    export interface ICreateItemResult {
        success: boolean;
        newuri: string;
    }

    /**
     * Stores the information about the workspace
     */
    export interface IWorkspace {
        id: string;
        annotation: string;
    };

    /**
     * Stores the information about the selected extent
     */
    export interface IExtent {
        uri: string;
    }

    export interface IItemsContent {
        columns: IDataForm;
        items: Array<IItemContentModel>;
        metaClasses: Array<IItemModel>;
        search: string;
        totalItemCount: number;
        filteredItemCount: number;
    }

    export interface IExtentContent extends IItemsContent {
        url: string;
    };

    /**
     * Defines the information to reference the content model
     */
    export interface IItemModel {
        id: string;
        name: string;
        fullname: string;
        uri: string;
        ext: string;
        ws: string;
        layer: string;
    }

    /**
     * Defines the information of model and also includes the content
     */
    export interface IItemContentModel extends IItemModel {
        v: Array<string>;
        c: IDataForm;
        metaclass?: IItemModel;
    }

    /**
     * Implements the IItemModel interface by containing all the fields
     */
    export class ItemContentModel implements IItemContentModel {
        id: string;
        name: string;
        fullname: string;
        uri: string;
        ext: string;
        ws: string;
        layer: string;
        v: Array<string>;
        c: IDataForm;
        metaclass: IItemModel;

        constructor() {
            this.v = [];
        }
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
        metaClassUri: string;
    }

    export interface IDataTableItem {
        /**
         * Stores name of the workspace
         */ 
        ws: string;

        /**
         * Stores the url of the extent
         */
        ext: string;
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

    /** This class is used to reference a single object within the database */
    export class ExtentReferenceModel {
        ws: string;
        ext: string;
    }

    export class ItemReferenceModel extends ExtentReferenceModel {
        item: string;
    }

    export class ItemCreateModel extends ExtentReferenceModel {
//        container: string;
        metaclass: string;

        // Defines the url of the item to which the item shall be added
        parentItem: string;
        // Defines the property to which the item will be added
        parentProperty: string;
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

        metaClassUri: string;

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
}

export namespace Api {
    export interface ILayout {
        renavigate(): void;
        throwViewPortChanged(data: ILayoutChangedEvent): void;
        getRibbon(): DMRibbon.Ribbon;
    }

    /**
     * Defines the interface that will get called, when a table reads additional information from the 
     */
    export interface IItemsProvider {
        performQuery(query: IItemTableQuery): JQueryDeferred<ClientResponse.IItemsContent>;
    }

    /**
     * Defines the information interface being used to retrieve additional information from a database
     */
    export interface IItemTableQuery {
        view: string;
        searchString?: string;
        offset?: number;
        amount?: number;
    }

    export class ItemTableQuery implements IItemTableQuery {
        searchString: string;
        offset: number;
        amount: number;
        view: string;
    }

    export enum PageType {
        Workspaces,
        Extents,
        Items,
        ItemDetail,
        Dialog
    }

    export interface ILayoutChangedEvent {
        navigation?: DMN.INavigation;   // Will be set at the thrower
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

    /**
     * This interface has to be returned by all plugins
     */
    export interface IPluginResult {
    /**
     * Returns a function that will be called, if the viewport changes
     * @param ev Parameter containing the information about the changed view set
     * @returns {} The function
     */
        onViewPortChanged?: (ev: ILayoutChangedEvent) => void;
    }
}