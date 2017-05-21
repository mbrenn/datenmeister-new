
import * as DMCI from "./datenmeister-clientinterface";

export class DataField implements DMCI.In.IFieldData {
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

export class TextDataField extends DataField implements DMCI.In.ITextFieldData {

    lineHeight: number;

    constructor(title?: string, name?: string) {
        super(ColumnTypes.textbox, title, name);
        this.lineHeight = 1;
    }
}

export class DateTimeDataField extends DataField implements DMCI.In.IDateTimeFieldData {

    showDate: boolean;
    showTime: boolean;

    constructor(title?: string, name?: string) {
        super(ColumnTypes.dateTime, title, name);
        this.showDate = true;
        this.showTime = true;
    }
}

export class DropDownDataField extends DataField implements DMCI.In.IDropDownFieldData {
    values: Array<string>;

    constructor(title?: string, name?: string) {
        super(ColumnTypes.dropdown, title, name);
    }
}

export class SubElementsDataField extends DataField implements DMCI.In.ISubElementsFieldData {

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