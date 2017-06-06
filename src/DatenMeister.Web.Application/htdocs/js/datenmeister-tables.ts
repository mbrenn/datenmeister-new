import * as DMH from "./datenmeister-helper"
import * as DMI from "./datenmeister-interfaces"
import * as DMCI from "./datenmeister-clientinterface"
import * as DMVM from "./datenmeister-viewmodels"
import * as DMClient from "./datenmeister-client"
import * as DMToolbar from "./datenmeister-toolbar"



export class ListTableConfiguration {
    fields: Array<Fields.IField>;

    constructor() {
        this.fields = new Array<Fields.IField>();
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
        var field: Fields.IField;
        var n;
        for (n in this.configuration.fields) {
            field = this.configuration.fields[n];
            var domColumn = $("<th></th>");
            domColumn.text(field.title);
            domHeadRow.append(domColumn);

            // Apply style to headline cell
            field.applyStandardStyles(domColumn);

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
                var domField = field.createDom(item);
                domCell.append(domField);
                domRow.append(domCell);

                // Apply style to headline cell
                field.applyStandardStyles(domCell);
            }

            this.domTable.append(domRow);
        }
        
    }
}


export class DetailTableConfiguration {
    fields: Array<Fields.IField>;

    constructor() {
        this.fields = new Array<Fields.IField>();
    }
}

export class DetailTableComposer {

    item: any;
    configuration: DetailTableConfiguration;
    domForEditArray: Array<JQuery>;
    container: JQuery;
    domTable: JQuery;

    onClickOk: (newItem: any) => void;
    onClickCancel: () => void;
    

    constructor(configuration: DetailTableConfiguration, container: JQuery) {
        this.configuration = configuration;
        this.container = container;
    }


    composeTable(item?: any): void {
        if (item === undefined || item === null) {
            item = {};
        }
        this.item = item;
        this.domTable = $("<table class='table table-condensed'></table>");
        this.composeContent();
        this.container.append(this.domTable);
    }

    refresh() {
        this.domTable.empty();
        this.composeContent();
    }

    composeContent() {
        var tthis = this;
        var domRow: JQuery;
        domRow = $("<tr><th>Title</th><th>Value</th><th></th></tr>");

        this.domTable.append(domRow);

        var field: Fields.IField;

        this.domForEditArray = new Array<JQuery>();

        // Now, the items
        for (var f in this.configuration.fields) {
            field = this.configuration.fields[f];
            domRow = $("<tr></tr>");
            var domColumn = $("<td class='table_column_name'></td>");
            domColumn.data("column", "name");
            domColumn.text(field.title);
            domRow.append(domColumn);

            domColumn = $("<td class='table_column_value'></td>");
            domColumn.data("column", "value");
            var domForEdit = field.createDom(this.item);
            domColumn.append(domForEdit);
            domRow.append(domColumn);

            domColumn = $("<td></td>");

            domRow.append(domColumn);

            this.domForEditArray[field.name] = domForEdit;

            this.domTable.append(domRow);
        }

        // Add new property
        /*if (this.configuration.supportNewProperties) {
            this.offerNewProperty(domTable);
        }*/

        // Adds the OK, Cancel button
        var domOkCancel = $("<tr><td colspan='3' class='text-right'></td></tr>");
        var domOkCancelColumn = $("td", domOkCancel);
/*        var domOk = $("<button class='btn btn-primary dm-button-ok'>OK</button>");
        domOkCancelColumn.append(domOk);*/

        if (this.onClickOk !== undefined || this.onClickOk !== undefined) {
            var domEdit = $("<button class='btn btn-primary dm-button-ok'>OK</button>");
            domEdit.click(() => {
                tthis.clickOnOk();
            });

            domOkCancelColumn.append(domEdit);
        }

        if (this.onClickCancel !== undefined || this.onClickCancel !== undefined) {
            var domCancel = $("<button class='btn btn-default dm-button-cancel'>Cancel</button>");
            domCancel.click(() => tthis.clickOnCancel());
            domOkCancelColumn.append(domCancel);
        }

        this.domTable.append(domOkCancel);
        
    }

    clickOnOk(): void {
        if (this.onClickOk !== undefined && this.onClickOk !== null) {
            var newItem = $.extend({}, this.item);
            this.submitForm(newItem);
            this.onClickOk(newItem);
        }
        
    }

    clickOnCancel(): void {
        if (this.onClickCancel !== undefined && this.onClickCancel !== null) {
            this.onClickCancel();
        }
    }

    submitForm(newItem: any) {
        var fields = this.configuration.fields;
        for (var f in fields) {
            if (fields.hasOwnProperty(f)) {
                var field = fields[f];
                var domEdit = this.domForEditArray[field.name];

                var value = domEdit.val();
                newItem[field.name] = value;
            }
        }
    }

    /*item: any;
    domForEditArray: Array<JQuery>;

    constructor(
        item: DMCI.In.IItemContentModel,
        configuration: ItemContentConfiguration) {
        this.item = item;
        this.configuration = configuration;
    }

    show(dom: JQuery) {
        dom.empty();
        this.domContainer = dom;

        var domTable = $("<table class='table table-condensed'></table>");

        // First the headline
        var domRow: JQuery;
        domRow = $("<tr><th>Title</th><th>Value</th><th></th></tr>");

        domTable.append(domRow);

        var propertyValue = this.item.v;
        var column: DMCI.In.IFieldData;

        if (this.configuration.autoProperties) {
            this.configuration.columns.length = 0;
            for (let property in propertyValue) {
                if (propertyValue.hasOwnProperty(property)) {
                    column = {
                        fieldType: "textbox",
                        title: property,
                        name: property
                    };

                    this.configuration.columns[this.configuration.columns.length] = column;
                }
            }
        }

        this.domForEditArray = new Array<JQuery>();

        // Now, the items
        for (var columnNr in this.configuration.columns) {
            column = this.configuration.columns[columnNr];
            domRow = $("<tr></tr>");
            var domColumn = $("<td class='table_column_name'></td>");
            domColumn.data("column", "name");
            domColumn.text(column.title);
            domRow.append(domColumn);

            domColumn = $("<td class='table_column_value'></td>");
            domColumn.data("column", "value");
            var domForEdit = createDomForContent(
                this.item,
                column,
                this.configuration);
            domColumn.append(domForEdit);
            domRow.append(domColumn);

            domColumn = $("<td></td>");
            domRow.append(domColumn);

            this.domForEditArray[column.name] = domForEdit;

            domTable.append(domRow);
        }

        // Add new property
        if (this.configuration.supportNewProperties) {
            this.offerNewProperty(domTable);
        }

        // Adds the OK, Cancel button
        var domOkCancel = $("<tr><td colspan='3' class='text-right'></td></tr>");
        var domOkCancelColumn = $("td", domOkCancel);
        var domOk = $("<button class='btn btn-primary dm-button-ok'>OK</button>");
        domOkCancelColumn.append(domOk);

        if (this.configuration.onEditButton !== undefined) {
            var domEdit = $("<button class='btn btn-primary dm-button-ok'>Edit</button>");
            domEdit.click(() => {
                this.configuration.onEditButton();
            });

            domOkCancelColumn.append(domEdit);
        }

        var domCancel = $("<button class='btn btn-default dm-button-cancel'>Cancel</button>");

        domOkCancelColumn.append(domCancel);

        domOk.click(() => {
            this.submitForm();
            if (this.configuration.onOkForm !== undefined) {
                this.configuration.onOkForm();
            }
        });

        domCancel.click(() => {
                if (this.configuration.onCancelForm !== undefined) {
                    this.configuration.onCancelForm();
                }
            }
        );

        domTable.append(domOkCancel);

        dom.append(domTable);
    }

    offerNewProperty(domTable: JQuery) {
        var tthis = this;
        var domNewProperty = $("<tr><td colspan='3'><button class='btn btn-default'>Add Property</button></td></tr>");
        $("button", domNewProperty).click(() => {
            domNewProperty.empty();
            var domNewPropertyName = $("<td class='table_column_name'><input type='textbox' class='form-control' /></td>");
            var domNewPropertyValue = $("<td class='table_column_value'><input type='textbox' class='form-control' /></td>");
            domNewProperty.append(domNewPropertyName);
            domNewProperty.append(domNewPropertyValue);
            var inputProperty = $("input", domNewPropertyName);
            var inputValue = $("input", domNewPropertyValue);

            var tdButtons = $("<td class='table_column_edit'></td>");
            var domNewPropertyEdit = $("<button href='#' class='btn btn-default dm-button-ok'>OK</button>");
            var domNewPropertyCancel = $("<button href='#' class='btn btn-default dm-button-cancel'>Cancel</button>");
            tdButtons.append(domNewPropertyEdit);
            tdButtons.append(domNewPropertyCancel);
            domNewProperty.append(tdButtons);

            domNewPropertyEdit.click(() => {
                var property = inputProperty.val();
                var newValue = inputValue.val();

                tthis.submitForm();
                tthis.item.v[property] = newValue;

                // Adds the new property to the autogenerated rows                    
                var column = {
                    fieldType: "textbox",
                    title: property,
                    name: property
                };

                tthis.configuration.columns[this.configuration.columns.length] = column;

                tthis.show(tthis.domContainer);
                return false;
            });

            domNewPropertyCancel.click(() => {
                tthis.show(tthis.domContainer);
                return false;
            });

            return false;
        });

        domTable.append(domNewProperty);
    }*/
}


export namespace Fields {
    export function addEditButton(
        configuration: ListTableConfiguration,
        onClick: (item: any) => void): IField {
        var buttonField = new ButtonField("EDIT", onClick);
        buttonField.horizontalAlignment = Alignments.Right;
        buttonField.width = -1;
        configuration.fields[configuration.fields.length] = buttonField;
        return buttonField;
    }

    export function addDeleteButton(
        configuration: ListTableConfiguration,
        onClick: (item: any) => void): IField {
        var buttonField = new ButtonField("DELETE");
        buttonField.horizontalAlignment = Alignments.Right;
        buttonField.width = -1;
        buttonField.click((item: any, button: ButtonFieldInstance) => {
            if (button !== undefined && button.state === true) {
                onClick(item);
            } else {
                button.state = true;
                button.setText("CONFIRM");
            }

        });

        configuration.fields[configuration.fields.length] = buttonField;
        return buttonField;
    }


    export interface IField {
        getFieldType(): string;
        title: string;
        name: string;
        defaultValue: any;
        isEnumeration: boolean;
        isReadOnly?: boolean;
        /**
         * Stores the with of the field.
         * If the width is -1, the corresponding column will be set to a minimum width
         */
        width: number;

        createDom(item: any): JQuery;
        applyStandardStyles(domCell: JQuery);

    }

    /**
     * Enumerates the alignments
     */
    export enum Alignments
    {
        Left,
        Center,
        Right,
        Top, 
        Middle,
        Bottom
        
    }

    export class FieldBase implements IField {
        title: string;
        name: string;
        defaultValue: any;
        isEnumeration: boolean;
        isReadOnly?: boolean;
        horizontalAlignment: Alignments;
        width: number;

        getFieldType(): string { throw new Error("Not implemented"); }

        createDom(item: any): JQuery { throw new Error("Not implemented"); }

        readOnly(): FieldBase {
            this.isReadOnly = true;
            return this;
        }

        applyStandardStyles(dom: JQuery) {
            if (this.horizontalAlignment === Alignments.Right) {
                dom.css("text-align", "right");
            }

            // Defines the width of the cell
            if (this.width === -1) {
                dom.css("width", "1%");
            } else if (this.width !== undefined && this.width !== null) {
                dom.css("width", this.width + "%");
            }
        }
    }

    export class ButtonField extends FieldBase {
        onClick: (item: any, button: ButtonFieldInstance) => void;

        constructor(title: string,
            onClick?: (item: any, button: ButtonFieldInstance) => void) {
            super();
            this.title = title;
            this.onClick = onClick;
        }

        click(onClick: (item: any, button: ButtonFieldInstance) => void) {
            this.onClick = onClick;
        }

        createDom(item: any): JQuery {
            var domButton = $("<button href='#' class='btn btn-primary'></button>");

            var instance = new ButtonFieldInstance();
            instance.domContainer = domButton;

            domButton.click(() => {
                this.onClick(item, instance);
                return false;
            });
            domButton.text(this.title);
            return domButton;
        };
    }

    export class ButtonFieldInstance {
        /**
         * Just a status for delete button
         */
        state: Boolean;

        domContainer: JQuery;

        constructor() {
            this.state = false;
        }

        setText(text: string): void {
            this.domContainer.text(text);
        }

        setState(state: Boolean) {
            this.state = state;
        }

        getState(): Boolean {
            return this.state;
        }
    }

    export class TextboxField extends FieldBase {
        lineHeight: number;
        getFieldType(): string { return DMVM.ColumnTypes.textbox; }

        constructor(name?: string, title?: string) {
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

    export class DropDownField extends FieldBase {
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
}

/**
 * Converts field data structure to real fields
 * @param fieldDatas The data to be converted as an array
 */
export function convertFieldDataToFields(fieldDatas: Array<DMCI.In.IFieldData>): Array<Fields.IField> {
    var result = new Array<Fields.IField>();
    for (var n in fieldDatas) {
        result[n] = convertFieldDataToField(fieldDatas[n]);
    }

    return result;
}

/**
 * Converts one instance of field data to a real field
 * @param data Data to be converted
 */
export function convertFieldDataToField(data: DMCI.In.IFieldData): Fields.IField {
    var field: Fields.FieldBase;

    switch (data.fieldType) {
        case DMVM.ColumnTypes.textbox:
            field = new Fields.TextboxField();
            break;
        default:
            throw "Unknown fieldtype: " + data.fieldType;
    }

    field.title = data.title;
    field.isReadOnly = data.isReadOnly;
    field.isEnumeration = data.isEnumeration;
    field.defaultValue = data.defaultValue;
    field.name = data.name;
    return field;


}