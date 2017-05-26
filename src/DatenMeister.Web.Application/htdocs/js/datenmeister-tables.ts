import * as DMH from "./datenmeister-helper"
import * as DMI from "./datenmeister-interfaces"
import * as DMCI from "./datenmeister-clientinterface"
import * as DMVM from "./datenmeister-viewmodels"
import * as DMClient from "./datenmeister-client"
import * as DMToolbar from "./datenmeister-toolbar"



export class ListTableConfiguration {
    fields: Array<IField>;

    constructor() {
        this.fields = new Array<IField>();
    }
}
/**
 * Composes the table as a list view
 */
export class ListTableComposer {
    // Called, when the user clicks on the item
    onClickItem: (item: DMCI.In.ITableItem) => void;
    items: Array<DMCI.In.ITableItem>;
    rows: Array<JQuery>;

    configuration: ListTableConfiguration;
    container: JQuery;
    domTable: JQuery;

    // This method can be used to add a filtering to the table
    filterItem: (item: any) => Boolean;

    constructor(configuration: ListTableConfiguration, container: JQuery) {
        this.configuration = configuration;
        this.container = container;
    }

    
    composeTable(items: Array<any>) {
        this.items = items;
        this.rows = new Array<JQuery>();
        
        this.domTable = $("<table class='table table-condensed'></table>");


        this.composeContent();
        this.container.append(this.domTable);
    }

    refresh() {
        this.domTable.empty();
        this.composeContent();
    }

    composeContent() {

        this.rows = new Array<JQuery>();
        // Creates the headrow
        var domHeadRow = $("<tr></tr>");
        var field: IField;
        var n;
        for (n in this.configuration.fields) {
            field = this.configuration.fields[n];
            var domColumn = $("<th></th>");
            domColumn.text(field.title);
            domHeadRow.append(domColumn);

        }

        this.domTable.append(domHeadRow);


        // Create content
        for (n in this.items) {
            var item = this.items[n];
            if (this.filterItem !== undefined && this.filterItem !== null && this.filterItem(item) === false) {
                // Item can be filtered out
                continue;
            }

            var domRow = $("<tr></tr>");
            this.rows[n] = domRow;

            for (var f in this.configuration.fields) {
                field = this.configuration.fields[f];
                var domCell = $("<td></td>");
                domCell.append(field.createDom(item));
                domRow.append(domCell);
            }

            this.domTable.append(domRow);
        }
        
    }
}

export namespace Fields {
    export function addEditButton(
        configuration: ListTableConfiguration,
        onClick: (item: any) => void) :IField
    {
        var buttonField = new ButtonField("EDIT", onClick);
        configuration.fields[configuration.fields.length] = buttonField;
        return buttonField;
    }
}


export interface IField {
    getFieldType(): string;
    title: string;
    name: string;
    defaultValue: any;
    isEnumeration: boolean;
    isReadOnly?: boolean;

    createDom(item: DMCI.In.ITableItem): JQuery;

}

export class FieldBase implements IField {
    title: string;
    name: string;
    defaultValue: any;
    isEnumeration: boolean;
    isReadOnly?: boolean;

    getFieldType(): string { throw new Error("Not implemented"); }

    createDom(item: DMCI.In.ITableItem): JQuery { throw new Error("Not implemented"); }

    readOnly(): FieldBase {
        this.isReadOnly = true;
        return this;
    }
}

export class ButtonField extends FieldBase {

    onClick: (item: any) => void;

    constructor(title: string,
        onClick: (item: any) => void) {
        super();
        this.title = title;
        this.onClick = onClick;
    }

    createDom(item: DMCI.In.ITableItem): JQuery {
        var button = $("<button href='#' class='btn btn-primary'></button>");
        button.click(() => {
            this.onClick(item);
            return false;
        });
        button.text(this.title);
        return button;
    };
}

export class TextboxField extends FieldBase {
    lineHeight: number;
    getFieldType(): string { return DMVM.ColumnTypes.textbox; }

    constructor(name: string, title: string) {
        super();
        this.title = title;
        this.name = name;
    }

    createDom(item: any): JQuery {
        var contentValue = item[this.name];
        var isReadonly = this.isReadOnly;

        // We have a textbox, so check if we have multiple line
        if (this.lineHeight !== undefined && this.lineHeight > 1) {
            let domTextBoxMultiple = $("<textarea class='form-control'></textarea>")
                .attr('rows', this.lineHeight);
            domTextBoxMultiple.val(contentValue);
            if (isReadonly) {
                domTextBoxMultiple.attr("readonly", "readonly");
            }

            return domTextBoxMultiple;
        } else {
            // Single line
            if (isReadonly) {
                let domResult = $("<span class='dm-itemtable-data'></span>");
                domResult.text(contentValue);
                return domResult;
            } else {
                let domTextBox = $("<input type='textbox' class='form-control' />");
                domTextBox.val(contentValue);

                if (this.isReadOnly) {
                    domTextBox.attr("readonly", "readonly");
                }

                return domTextBox;
            }
        }
    }
}

export class DropDownField extends FieldBase{

    values: Array<string>;

    getFieldType(): string {
        return DMVM.ColumnTypes.dropdown;
    }

    createDom(item: any): JQuery {
        var contentValue = item[this.name];
        if (contentValue === undefined) {
            contentValue = this.defaultValue;
        }

        let domDD = $("<select></select>");
        for (var name in this.values) {
            var displayText = this.values[name];
            let domOption = $("<option></option>").attr("value", name).text(displayText);
            domDD.append(domOption);
        }

        domDD.val(contentValue);
        return domDD;
    }
}

