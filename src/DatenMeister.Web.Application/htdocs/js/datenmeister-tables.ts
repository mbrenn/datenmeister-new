import * as DMI from "datenmeister-interfaces"

export class ItemTableConfiguration {
    onNewItemClicked: () => void;
    onItemEdit: (url: string, domRow: JQuery) => boolean;
    onItemDelete: (url: string, domRow: JQuery) => boolean;
    onPageChange: (newPage: number) => void;
    supportSearchbox: boolean;

    /* true, if new properties shall be supported */
    supportNewItem: boolean;

    showColumnForId: boolean;

    supportPaging: boolean;
    itemsPerPage: number;

    /* This method is called each time, the user has changed content in the search field */
    onSearch: (searchText: string) => void;

    constructor() {
        this.onItemEdit = (url: string, domRow: JQuery) => false;
        this.onItemDelete = (url: string, domRow: JQuery) => false;
        this.supportSearchbox = true;
        this.supportNewItem = true;
        this.showColumnForId = false;

        this.supportPaging = true;
        this.itemsPerPage = 20;
    }
}
        
/*
    * Used to show a lot of items in a database. The table will use an array of MofObjects
    * as the datasource
    */
export class ItemListTable {
    data: DMI.IExtentContent;
    configuration: ItemTableConfiguration;
    domContainer: JQuery;
    domTable: JQuery;
    lastProcessedSearchString: string;
    domTotalNumber: JQuery;
    domFilteredNumber: JQuery;
    currentPage: number;
    totalPages: number;

    constructor(dom: JQuery, data: DMI.IExtentContent, configuration: ItemTableConfiguration) {
        this.domContainer = dom;
        this.data = data;
        this.configuration = configuration;
        this.currentPage = 1;
        this.totalPages = 0;
    }

    throwOnPageChange(): void {
        if (this.configuration.onPageChange !== undefined) {
            this.configuration.onPageChange(this.currentPage);
        }
    }

    // Replaces the content at the dom with the created table
    show() {
        var tthis = this;
        this.domContainer.empty();

        if (this.configuration.supportSearchbox) {
            var domSearchBox = $("<div><input type='textbox' /></div>");
            var domInput = $("input", domSearchBox);
            $("input", domSearchBox).keyup(() => {
                var searchValue = domInput.val();
                if (tthis.configuration.onSearch !== undefined) {
                    tthis.lastProcessedSearchString = searchValue;
                    tthis.configuration.onSearch(searchValue);
                }
            });

            this.domContainer.append(domSearchBox);
        }

        if (this.configuration.supportNewItem) {
            var domNewItem = $("<div><a href='#'>Create new item</a></div>");
            domNewItem.click(() => {
                if (tthis.configuration.onNewItemClicked !== undefined) {
                    tthis.configuration.onNewItemClicked();
                }

                return false;
            });

            this.domContainer.append(domNewItem);
        }

        if (this.configuration.supportPaging) {
            var domPaging = $("<div><a href='#' class='dm_prevpage'>PREV</a> Page <input type='textbox' class='dm_page' value='1' " +
                "/> of <span class='dm_totalpages'> </span> <a href='#' class='dm_jumppage'>GO</a> <a href='#' class='dm_nextpage'>NEXT</a> ");
            this.domContainer.append(domPaging);

            var domPrev = $(".dm_prevpage", domPaging);
            var domNext = $(".dm_nextpage", domPaging);
            var domGo = $(".dm_jumppage", domPaging);
            var domCurrentPage = $(".dm_page", domPaging);

            domPrev.click(() => {
                tthis.currentPage--;
                tthis.currentPage = Math.max(1, tthis.currentPage);
                domCurrentPage.val(tthis.currentPage.toString());
                tthis.throwOnPageChange();
                return false;
            });

            domNext.click(() => {
                tthis.currentPage++;
                tthis.currentPage = Math.min(tthis.totalPages, tthis.currentPage);
                domCurrentPage.val(tthis.currentPage.toString());
                tthis.throwOnPageChange();
                return false;
            });

            domGo.click(() => {
                tthis.currentPage = domCurrentPage.val();
                tthis.currentPage = Math.max(1, tthis.currentPage);
                tthis.currentPage = Math.min(tthis.totalPages, tthis.currentPage);
                domCurrentPage.val(tthis.currentPage.toString());
                tthis.throwOnPageChange();
                return false;
            });
        }

        var domAmount = $("<div>Total: <span class='totalnumber'>##</span>, Filtered: <span class='filterednumber'>##</span>");
        this.domTotalNumber = $(".totalnumber", domAmount);
        this.domFilteredNumber = $(".filterednumber", domAmount);

        this.domContainer.append(domAmount);

        this.domTable = $("<table class='table'></table>");

        // First the headline

        var domRow = $("<tr></tr>");
        var domColumn;
        if (this.configuration.showColumnForId) {

            domColumn = $("<th>ID</th>");
            domRow.append(domColumn);
        }

        for (var c in this.data.columns) {
            var column = this.data.columns[c];
            domColumn = $("<th></th>");
            domColumn.text(column.title);
            domRow.append(domColumn);
        }

        // Creates the edit and delete column
        var domEditColumn = $("<th>EDIT</th>");
        domRow.append(domEditColumn);
        var domDeleteColumn = $("<th>DELETE</th>");
        domRow.append(domDeleteColumn);

        this.domTable.append(domRow);

        // Now, the items
        tthis.createRowsForData();

        this.domContainer.append(this.domTable);
    }

    createRowsForData(): void {
        var tthis = this;

        this.domTotalNumber.text(this.data.totalItemCount);
        this.domFilteredNumber.text(this.data.filteredItemCount);

        if (this.configuration.supportPaging) {
            var domTotalPages = $(".dm_totalpages", this.domContainer);
            this.totalPages = Math.floor((this.data.filteredItemCount - 1) / this.configuration.itemsPerPage) + 1;
            domTotalPages.text(this.totalPages);
        }

        // Now, the items
        for (var i in this.data.items) {
            var item = this.data.items[i];

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

            for (var c in this.data.columns) {
                var column = this.data.columns[c];
                domColumn = $("<td></td>");
                domColumn.text(item.v[column.name]);
                domRow.append(domColumn);
            }

            // Add Edit link
            var domEditColumn = $("<td class='hl'><a href='#'>EDIT</a></td>");
            domEditColumn.click((function (url, iDomRow) {
                return function () {
                    return tthis.configuration.onItemEdit(url, iDomRow);
                };
            })(item.uri, domRow));
            domRow.append(domEditColumn);

            var domDeleteColumn = $("<td class='hl'><a href='#'>DELETE</a></td>");
            var domA = $("a", domDeleteColumn);
            domDeleteColumn.click((function (url: string, innerDomRow: JQuery, innerDomA: JQuery) {
                return function () {
                    if (innerDomA.data("wasClicked") === true) {
                        return tthis.configuration.onItemDelete(url, innerDomRow);
                    } else {
                        innerDomA.data("wasClicked", true);
                        innerDomA.text("CONFIRM");
                        return false;
                    }
                };
            })(item.uri, domRow, domA));

            domRow.append(domDeleteColumn);
            this.domTable.append(domRow);
        }
    }

    updateItems(data: DMI.IExtentContent) {
        this.data = data;
        $("tr", this.domTable).has("td")
            .remove();
        this.createRowsForData();
    }
}

export class ItemContentConfiguration {
    autoProperties: boolean;

    // Gets or sets a flag, that the user can change the content of a property within the table. 
    // If the editing was performed, the onEditProperty-function will get called
    supportInlineEditing: boolean;
    supportNewProperties: boolean;

    editFunction: (url: string, property: string, domRow: JQuery) => boolean;
    deleteFunction: (url: string, property: string, domRow: JQuery) => boolean;

    onEditProperty: (url: string, property: string, newValue: string) => void;
    onNewProperty: (url: string, property: string, newValue: string) => void;

    constructor() {
        this.editFunction = (url: string, property: string, domRow: JQuery) => false;
        this.deleteFunction = (url: string, property: string, domRow: JQuery) => false;
        this.supportInlineEditing = true;
        this.supportNewProperties = true;
    }
}

export class ItemContentTable {
    item: DMI.IDataTableItem;
    configuration: ItemContentConfiguration;
    domContainer: JQuery;

    constructor(item: DMI.IDataTableItem, configuration: ItemContentConfiguration) {
        this.item = item;
        this.configuration = configuration;
    }

    show(dom: JQuery) {
        var tthis = this;
        this.domContainer = dom;
        dom.empty();
        var domTable = $("<table class='table'></table>");

        // First the headline
        var domRow = $("<tr><th>Title</th><th>Value</th><th>EDIT</th><th>DELETE</th></tr>");
        domTable.append(domRow);

        // Now, the items
        for (var property in this.item.v) {
            domRow = $("<tr></tr>");
            var value = this.item.v[property];
            var domColumn = $("<td class='table_column_name'></td>");
            domColumn.data("column", "name");
            domColumn.text(property);
            domRow.append(domColumn);

            domColumn = $("<td class='table_column_value'></td>");
            domColumn.data("column", "value");
            domColumn.text(value);
            domRow.append(domColumn);

            // Add Edit link
            let domEditColumn = $("<td class='hl table_column_edit'><a href='#'>EDIT</a></td>");
            $("a", domEditColumn).click((function(url: string, property: string, idomRow: JQuery, idomA: JQuery) {
                return function() {
                    if (tthis.configuration.supportInlineEditing) {
                        tthis.startInlineEditing(property, idomRow);
                        return false;
                    } else {
                        return tthis.configuration.editFunction(url, property, idomRow);
                    }
                };
            })(this.item.uri, property, domRow, domA));
            domRow.append(domEditColumn);

            let domDeleteColumn = $("<td class='hl table_column_delete'><a href='#'>DELETE</a></td>");
            var domA = $("a", domDeleteColumn);
            $("a", domDeleteColumn).click((function(url: string, property: string, idomRow: JQuery, idomA: JQuery) {
                return function() {
                    if (idomA.data("wasClicked") === true) {
                        return tthis.configuration.deleteFunction(url, property, idomRow);
                    } else {
                        idomA.data("wasClicked", true);
                        idomA.text("CONFIRM");
                        return false;
                    }

                };
            })(this.item.uri, property, domRow, domA));
            domRow.append(domDeleteColumn);

            domTable.append(domRow);
        }

        // Add new property
        if (this.configuration.supportNewProperties) {
            this.offerNewProperty(domTable);
        }

        dom.append(domTable);
    }

    startInlineEditing(property: string, domRow: JQuery) {
        var tthis = this;
        var domValue = $(".table_column_value", domRow);
        domValue.empty();

        var domTextBox = $("<input type='textbox' />");
        domTextBox.val(this.item.v[property]);
        domValue.append(domTextBox);

        var domEditColumn = $(".table_column_edit", domRow);
        domEditColumn.empty();

        var domEditOK = $("<a href='#'>OK</a>");
        domEditColumn.append(domEditOK);
        var domEditCancel = $("<a href='#'>Cancel</a>");
        domEditColumn.append(domEditCancel);

        //Sets the commands
        domEditOK.on('click', () => {
            var newValue = domTextBox.val();
            tthis.item.v[property] = newValue;

            if (tthis.configuration.onEditProperty !== undefined) {
                tthis.configuration.onEditProperty(tthis.item.uri, property, newValue);
            }

            tthis.show(tthis.domContainer);
            return false;
        });

        domEditCancel.on('click', () => {
            // Rebuilds the complete table
            tthis.show(tthis.domContainer);

            return false;
        });

    }

    offerNewProperty(domTable: JQuery) {
        var tthis = this;
        var domNewProperty = $("<tr><td colspan='4'><a href='#'>NEW PROPERTY</a></td></tr>");
        $("a", domNewProperty).click(() => {
            domNewProperty.empty();
            var domNewPropertyName = $("<td class='table_column_name'><input type='textbox' /></td>");
            var domNewPropertyValue = $("<td class='table_column_value'><input type='textbox' /></td>");
            var domNewPropertyEdit = $("<td class='table_column_edit'><a href='#'>OK</a></td>");
            var domNewPropertyCancel = $("<td class='table_column_edit'><a href='#'>CANCEL</a></td>");
            domNewProperty.append(domNewPropertyName);
            domNewProperty.append(domNewPropertyValue);
            domNewProperty.append(domNewPropertyEdit);
            domNewProperty.append(domNewPropertyCancel);

            var inputProperty = $("input", domNewPropertyName);
            var inputValue = $("input", domNewPropertyValue);


            $("a", domNewPropertyEdit).click(() => {
                var property = inputProperty.val();
                var newValue = inputValue.val();

                tthis.item.v[property] = newValue;

                if (tthis.configuration.onNewProperty !== undefined) {
                    tthis.configuration.onNewProperty(tthis.item.uri, property, newValue);
                }

                tthis.show(tthis.domContainer);
                return false;
            });

            $("a", domNewPropertyCancel).click(() => {
                tthis.show(tthis.domContainer);
                return false;
            });

            return false;
        });

        domTable.append(domNewProperty);
    }
}