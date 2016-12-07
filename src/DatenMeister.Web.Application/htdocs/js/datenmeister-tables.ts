import * as DMI from "./datenmeister-interfaces"
import * as DMN from "./datenmeister-navigation"
import * as DMClient from "./datenmeister-client"
import * as DMToolbar from "./datenmeister-toolbar"

export class ItemListTableConfiguration {
    onItemEdit: (url: string) => boolean;
    onItemDelete: (url: string, domRow: JQuery) => boolean;
    onItemSelect: (url: string) => boolean;

    isReadOnly: boolean;

    showColumnForId: boolean;
    itemsPerPage: number;
    navigation: DMN.INavigation;

    paging: DMToolbar.ToolbarPaging;
    
    constructor(navigation: DMN.INavigation) {
        this.onItemEdit = (url: string) => false;
        this.onItemDelete = (url: string, domRow: JQuery) => false;

        this.showColumnForId = false;
        this.itemsPerPage = 20;
        this.isReadOnly = true;
        this.navigation = navigation;
    }
}

/*
    * Used to show a lot of items in a database. The table will use an array of MofObjects
    * as the datasource
    */
export class ItemListTable {
    configuration: ItemListTableConfiguration;
    domContainer: JQuery;
    domTable: JQuery;
    lastProcessedSearchString: string;
    domTotalNumber: JQuery;
    domFilteredNumber: JQuery;
    totalPages: number;
    domNewItem: JQuery;
    createableTypes: Array<DMI.ClientResponse.IItemModel>;

    provider: DMI.Api.IItemsProvider;
    currentQuery: DMI.Api.IItemTableQuery;

    constructor(
        dom: JQuery,
        provider: DMI.Api.IItemsProvider,
        configuration: ItemListTableConfiguration) {
        this.domContainer = dom;
        this.provider = provider;
        this.configuration = configuration;
        this.totalPages = 0;
        this.currentQuery = new DMI.Api.ItemInExtentQuery();
        this.currentQuery.amount = configuration.itemsPerPage;
    }

    // Replaces the content at the dom with the created table
    loadAndShow(): JQueryDeferred<DMI.ClientResponse.IItemsContent> {
        return this.provider.performQuery(this.currentQuery).done((data) => {
            this.createDomForTable(data);
        });
    }

    reload(): JQueryDeferred<DMI.ClientResponse.IItemsContent> {
        return this.provider.performQuery(this.currentQuery).done((data) => {
            this.updateDomForItems(data);
        });
    }

    createDomForTable(data: DMI.ClientResponse.IItemsContent)
    {
        var tthis = this;
        this.domContainer.empty();

        var domAmount = $("<div>Total: <span class='totalnumber'>##</span>, Filtered: <span class='filterednumber'>##</span>");
        this.domTotalNumber = $(".totalnumber", domAmount);
        this.domFilteredNumber = $(".filterednumber", domAmount);

        if (this.configuration.navigation !== undefined) {
            this.configuration.navigation.setStatus(domAmount);
            this.configuration.isReadOnly = true;
        } else {
            this.domContainer.append(domAmount);
        }

        this.domTable = $("<table class='table table-condensed'></table>");

        // First the headline
        var domRow = $("<tr></tr>");
        var domColumn;
        if (this.configuration.showColumnForId) {

            domColumn = $("<th>ID</th>");
            domRow.append(domColumn);
        }

        var columns = data.columns.fields;
        for (var c in columns) {
            if (columns.hasOwnProperty(c)) {
                var column = columns[c];
                domColumn = $("<th></th>");
                domColumn.text(column.title);
                domRow.append(domColumn);
            }
        }

        // Creates the columns for commands
        var domCommand = $("<th></th>");
        domRow.append(domCommand);

        this.domTable.append(domRow);

        // Now, the items
        tthis.createRowsForItems(data);
        this.domContainer.append(this.domTable);
    }

    updateDomForItems(data: DMI.ClientResponse.IItemsContent): void {
        $("tr", this.domTable).has("td")
            .remove();
        this.createRowsForItems(data);
    }

    createRowsForItems(data: DMI.ClientResponse.IItemsContent): void {
        this.domTotalNumber.text(data.totalItemCount);
        this.domFilteredNumber.text(data.filteredItemCount);

        if (this.configuration.paging !== undefined) {
            this.configuration.paging.setTotalPages(
                Math.floor((data.filteredItemCount - 1) / this.configuration.itemsPerPage) + 1);
        }

        // Now, the items
        var items = data.items;
        for (var i in items) {
            if (items.hasOwnProperty(i)) {
                var item = items[i];

                // Gets the id of the item
                var id = item.uri;
                var hashIndex = item.uri.indexOf("#");
                if (hashIndex !== -1) {
                    id = item.uri.substring(hashIndex + 1);
                }

                var domRow = $("<tr></tr>");
                var domColumn;
                if (this.configuration.showColumnForId) {
                    domColumn = $("<td></td>");
                    domColumn.text(id);
                    domRow.append(domColumn);
                }

                var columns = data.columns.fields;
                for (var c in columns) {
                    if (columns.hasOwnProperty(c)) {
                        domColumn = $("<td></td>");
                        domColumn.append(createDomForContent(
                            item,
                            columns[c],
                            this.configuration));

                        domRow.append(domColumn);
                    }
                }

                // Add Edit link
                var buttons = $("<td class='dm-itemtable-commands'></td>");
                var domViewColumn = $("<button href='#' class='btn btn-primary'>View</button>");
                domViewColumn.click((url => {
                    return () => this.configuration.onItemSelect(url);
                })(item.uri));
                buttons.append(domViewColumn);

                var domEditColumn = $("<button href='#' class='btn btn-primary'>Edit</button>");
                domEditColumn.click((url => {
                    return () => this.configuration.onItemEdit(url);
                })(item.uri));
                buttons.append(domEditColumn);

                var domDeleteColumn = $("<button href='#' class='btn btn-danger'>Delete</button>");
                domDeleteColumn.click(((url: string, innerDomRow: JQuery, innerDomDelete: JQuery)=> {
                    return () => {
                        if (innerDomDelete.data("wasClicked") === true) {
                            return this.configuration.onItemDelete(url, innerDomRow);
                        } else {
                            innerDomDelete.data("wasClicked", true);
                            innerDomDelete.text("Confirm");
                            return false;
                        }
                    };
                })(item.uri, domRow, domDeleteColumn));

                buttons.append(domDeleteColumn);
                domRow.append(buttons);
                this.domTable.append(domRow);
            }
        }
    }
}

export class ItemContentConfiguration {
    autoProperties: boolean;
    columns: Array<DMI.ClientResponse.IFieldData>;

    // Gets or sets a flag, whether we should start with full edit mode
    // if we start with edit mode, all property values will be shown as an edit field
    // and an 'OK', 'Cancel'-button is added at the buttom of the table
    isReadOnly: boolean;

    /**
     * Includes the information whether the button to create new properties shall be included
     */
    supportNewProperties: boolean;

    navigation: DMN.INavigation;
    
    onItemSelect: (url: string) => boolean;

    onOkForm: () => void;
    onCancelForm: () => void;

    /**
     * Called, if the user clicks on the edit button. The edit button is not shown, when there is no item attached to it
     * @returns {} Function being called
     */
    onEditButton: () => void;

    constructor(navigation: DMN.INavigation) {
        this.isReadOnly = false;
        this.autoProperties = false;
        this.supportNewProperties = true;
        this.columns = new Array<DMI.ClientResponse.IFieldData>();
        this.navigation = navigation;
    }

    addColumn(column: DMI.ClientResponse.IFieldData) {
        this.columns[this.columns.length] = column;
    }
}

/**
 * Defines the table for one item and shows all properties
 */
export class ItemContentTable {
    item: DMI.ClientResponse.IItemContentModel;
    configuration: ItemContentConfiguration;
    domContainer: JQuery;
    domForEditArray: Array<JQuery>;

    constructor(
        item: DMI.ClientResponse.IItemContentModel,
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
        var column: DMI.ClientResponse.IFieldData;

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

    submitForm() {
        var columns = this.configuration.columns;
        for (var columnNr in columns) {
            if (columns.hasOwnProperty(columnNr)) {
                var column = columns[columnNr];
                var domEdit = this.domForEditArray[column.name];

                var value = domEdit.val();
                this.item.v[column.name] = value;
            }
        }
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
    }
}

/**
 * Creates the DOM for the content as defined by the columns
 * @param item Item, whose content shall be shown
 * @param column Column defining the content
 * @param inEditMode true, if the field is in edit mode
 * @param configuration Configuration of the complete table
 */
function createDomForContent(
    item: DMI.ClientResponse.IItemContentModel,
    column: DMI.ClientResponse.IFieldData,
    configuration: ItemListTableConfiguration | ItemContentConfiguration): JQuery {

    if (column.fieldType === DMI.Table.ColumnTypes.dropdown) {
        var dropdownField = new DropDownField();
        return dropdownField.createDom(item, column, configuration);
    }

    if (column.fieldType === DMI.Table.ColumnTypes.subElements) {
        var field = new SubElementField();
        return field.createDom(item, column, configuration);
    }

    return createDefaultDomForContent(item, column, configuration);
}

export class DropDownField {

    createDom(item: DMI.ClientResponse.IItemContentModel,
        column: DMI.ClientResponse.IFieldData,
        configuration: ItemListTableConfiguration | ItemContentConfiguration): JQuery {
        var contentValue = item.v[column.name];
        if (contentValue === undefined) {
            contentValue = column.defaultValue;
        }

        let domDD = $("<select></select>");
        let asDD = column as DMI.ClientResponse.IDropDownFieldData;
        for (var name in asDD.values) {
            var displayText = asDD.values[name];
            let domOption = $("<option></option>").attr("value", name).text(displayText);
            domDD.append(domOption);
        }

        domDD.val(contentValue);
        return domDD;
    }
}

export class SubElementField {

    createDom(item: DMI.ClientResponse.IItemContentModel,
        column: DMI.ClientResponse.IFieldData,
        configuration: ItemListTableConfiguration | ItemContentConfiguration): JQuery {
        let domSE = $("<ul></ul>");
        let asSE = column as DMI.ClientResponse.ISubElementsFieldData;

        // The content value
        var contentValue = item.v[column.name];
        for (var n in contentValue) {
            var subItem = contentValue[n];

            var func = innerItem => {
                var domLine = $("<li><a href='#'></a></li>");
                var domA = $("a", domLine);
                domA.text(innerItem.v);
                domA.click(() => {
                    configuration.navigation.navigateToItem(item.ws, item.ext, innerItem.u);
                    return false;
                });
                domSE.append(domLine);
            };

            func(subItem);
        }

        // For read-only things, don't show the button for new properties
        if (!configuration.isReadOnly) {
            var btn = $("<button>New Element</button>");
            btn.click(() => {
                DMClient.ExtentApi.createItemAsSubElement(
                    item.ws,
                    item.ext,
                    item.uri,
                    column.name,
                    null
                ).done((data) => {
                    var uri = data.newuri;
                    configuration.navigation.navigateToItem(item.ws, item.ext, uri);
                    return false;
                });
            });

            domSE.append(btn);
        }

        return domSE;
    }
}


/**
 * Creates the DOM for the content as defined by column, when the column.fieldType is not set
 * @param item Item, whose content shall be shown
 * @param column Column defining the content
 * @param inEditMode true, if the field is in edit mode
 * @param configuration Configuration of the complete table
 */
function createDefaultDomForContent(
    item: DMI.ClientResponse.IItemContentModel,
    column: DMI.ClientResponse.IFieldData,
    configuration: ItemListTableConfiguration | ItemContentConfiguration) : JQuery {

    var contentValue = item.v[column.name];
    if (contentValue === undefined) {
        contentValue = column.defaultValue;
    }

    // Enumerates all values
    if (column.isEnumeration) {
        let domResult = $("<ul></ul>");
        if (contentValue !== undefined) {
            for (var n in contentValue) {
                var listValue = contentValue[n];
                var domEntry = $("<li><a href='#'></a></li>");
                var domInner = $("a", domEntry);
                domInner.click(((innerListValue) => {
                    return () => {
                        if (configuration !== undefined && configuration.onItemSelect !== undefined) {
                            configuration.onItemSelect(innerListValue.u);
                        } else {
                            alert("No Event handler for 'onItemView' within createDomForContent");
                        }

                        return false;
                    };
                })(listValue));
                domInner.text(listValue.v);
                domResult.append(domEntry);
            }
        }

        return domResult;
    } else {
        let asTextBox = column as DMI.ClientResponse.ITextFieldData;
        var isReadonly = configuration.isReadOnly || asTextBox.isReadOnly;

        // We have a textbox, so check if we have multiple line
        if (asTextBox.lineHeight !== undefined && asTextBox.lineHeight > 1) {
            let domTextBoxMultiple = $("<textarea class='form-control'></textarea>")
                .attr('rows', asTextBox.lineHeight);
            domTextBoxMultiple.val(contentValue);
            if (isReadonly) {
                domTextBoxMultiple.attr("readonly", "readonly");
            }

            return domTextBoxMultiple;
        } else {
            if (isReadonly) {
                let domResult = $("<span class='dm-itemtable-data'></span>");
                domResult.text(contentValue);
                return domResult;
            } else {
                let domTextBox = $("<input type='textbox' class='form-control' />");
                domTextBox.val(contentValue);

                if (asTextBox.isReadOnly) {
                    domTextBox.attr("readonly", "readonly");
                }

                return domTextBox;
            }
        }
    }
}