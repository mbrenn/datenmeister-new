export namespace In {
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
 * Defines the information of model and also includes the view including the fields
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


export module Out {

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
}