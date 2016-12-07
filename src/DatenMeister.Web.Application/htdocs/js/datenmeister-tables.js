define(["require", "exports", "./datenmeister-interfaces", "./datenmeister-client"], function (require, exports, DMI, DMClient) {
    "use strict";
    var ItemListTableConfiguration = (function () {
        function ItemListTableConfiguration(navigation) {
            this.onItemEdit = function (url) { return false; };
            this.onItemDelete = function (url, domRow) { return false; };
            this.showColumnForId = false;
            this.itemsPerPage = 20;
            this.isReadOnly = true;
            this.navigation = navigation;
        }
        return ItemListTableConfiguration;
    }());
    exports.ItemListTableConfiguration = ItemListTableConfiguration;
    /*
        * Used to show a lot of items in a database. The table will use an array of MofObjects
        * as the datasource
        */
    var ItemListTable = (function () {
        function ItemListTable(dom, provider, configuration) {
            this.domContainer = dom;
            this.provider = provider;
            this.configuration = configuration;
            this.totalPages = 0;
            this.currentQuery = new DMI.Api.ItemInExtentQuery();
            this.currentQuery.amount = configuration.itemsPerPage;
        }
        // Replaces the content at the dom with the created table
        ItemListTable.prototype.loadAndShow = function () {
            var _this = this;
            return this.provider.performQuery(this.currentQuery).done(function (data) {
                _this.createDomForTable(data);
            });
        };
        ItemListTable.prototype.reload = function () {
            var _this = this;
            return this.provider.performQuery(this.currentQuery).done(function (data) {
                _this.updateDomForItems(data);
            });
        };
        ItemListTable.prototype.createDomForTable = function (data) {
            var tthis = this;
            this.domContainer.empty();
            var domAmount = $("<div>Total: <span class='totalnumber'>##</span>, Filtered: <span class='filterednumber'>##</span>");
            this.domTotalNumber = $(".totalnumber", domAmount);
            this.domFilteredNumber = $(".filterednumber", domAmount);
            if (this.configuration.navigation !== undefined) {
                this.configuration.navigation.setStatus(domAmount);
                this.configuration.isReadOnly = true;
            }
            else {
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
        };
        ItemListTable.prototype.updateDomForItems = function (data) {
            $("tr", this.domTable).has("td")
                .remove();
            this.createRowsForItems(data);
        };
        ItemListTable.prototype.createRowsForItems = function (data) {
            var _this = this;
            this.domTotalNumber.text(data.totalItemCount);
            this.domFilteredNumber.text(data.filteredItemCount);
            if (this.configuration.paging !== undefined) {
                this.configuration.paging.setTotalPages(Math.floor((data.filteredItemCount - 1) / this.configuration.itemsPerPage) + 1);
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
                            domColumn.append(createDomForContent(item, columns[c], this.configuration));
                            domRow.append(domColumn);
                        }
                    }
                    // Add Edit link
                    var buttons = $("<td class='dm-itemtable-commands'></td>");
                    var domViewColumn = $("<button href='#' class='btn btn-primary'>View</button>");
                    domViewColumn.click((function (url) {
                        return function () { return _this.configuration.onItemSelect(url); };
                    })(item.uri));
                    buttons.append(domViewColumn);
                    var domEditColumn = $("<button href='#' class='btn btn-primary'>Edit</button>");
                    domEditColumn.click((function (url) {
                        return function () { return _this.configuration.onItemEdit(url); };
                    })(item.uri));
                    buttons.append(domEditColumn);
                    var domDeleteColumn = $("<button href='#' class='btn btn-danger'>Delete</button>");
                    domDeleteColumn.click((function (url, innerDomRow, innerDomDelete) {
                        return function () {
                            if (innerDomDelete.data("wasClicked") === true) {
                                return _this.configuration.onItemDelete(url, innerDomRow);
                            }
                            else {
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
        };
        return ItemListTable;
    }());
    exports.ItemListTable = ItemListTable;
    var ItemContentConfiguration = (function () {
        function ItemContentConfiguration(navigation) {
            this.isReadOnly = false;
            this.autoProperties = false;
            this.supportNewProperties = true;
            this.columns = new Array();
            this.navigation = navigation;
        }
        ItemContentConfiguration.prototype.addColumn = function (column) {
            this.columns[this.columns.length] = column;
        };
        return ItemContentConfiguration;
    }());
    exports.ItemContentConfiguration = ItemContentConfiguration;
    /**
     * Defines the table for one item and shows all properties
     */
    var ItemContentTable = (function () {
        function ItemContentTable(item, configuration) {
            this.item = item;
            this.configuration = configuration;
        }
        ItemContentTable.prototype.show = function (dom) {
            var _this = this;
            dom.empty();
            this.domContainer = dom;
            var domTable = $("<table class='table table-condensed'></table>");
            // First the headline
            var domRow;
            domRow = $("<tr><th>Title</th><th>Value</th><th></th></tr>");
            domTable.append(domRow);
            var propertyValue = this.item.v;
            var column;
            if (this.configuration.autoProperties) {
                this.configuration.columns.length = 0;
                for (var property in propertyValue) {
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
            this.domForEditArray = new Array();
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
                var domForEdit = createDomForContent(this.item, column, this.configuration);
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
                domEdit.click(function () {
                    _this.configuration.onEditButton();
                });
                domOkCancelColumn.append(domEdit);
            }
            var domCancel = $("<button class='btn btn-default dm-button-cancel'>Cancel</button>");
            domOkCancelColumn.append(domCancel);
            domOk.click(function () {
                _this.submitForm();
                if (_this.configuration.onOkForm !== undefined) {
                    _this.configuration.onOkForm();
                }
            });
            domCancel.click(function () {
                if (_this.configuration.onCancelForm !== undefined) {
                    _this.configuration.onCancelForm();
                }
            });
            domTable.append(domOkCancel);
            dom.append(domTable);
        };
        ItemContentTable.prototype.submitForm = function () {
            var columns = this.configuration.columns;
            for (var columnNr in columns) {
                if (columns.hasOwnProperty(columnNr)) {
                    var column = columns[columnNr];
                    var domEdit = this.domForEditArray[column.name];
                    var value = domEdit.val();
                    this.item.v[column.name] = value;
                }
            }
        };
        ItemContentTable.prototype.offerNewProperty = function (domTable) {
            var _this = this;
            var tthis = this;
            var domNewProperty = $("<tr><td colspan='3'><button class='btn btn-default'>Add Property</button></td></tr>");
            $("button", domNewProperty).click(function () {
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
                domNewPropertyEdit.click(function () {
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
                    tthis.configuration.columns[_this.configuration.columns.length] = column;
                    tthis.show(tthis.domContainer);
                    return false;
                });
                domNewPropertyCancel.click(function () {
                    tthis.show(tthis.domContainer);
                    return false;
                });
                return false;
            });
            domTable.append(domNewProperty);
        };
        return ItemContentTable;
    }());
    exports.ItemContentTable = ItemContentTable;
    /**
     * Creates the DOM for the content as defined by the columns
     * @param item Item, whose content shall be shown
     * @param column Column defining the content
     * @param inEditMode true, if the field is in edit mode
     * @param configuration Configuration of the complete table
     */
    function createDomForContent(item, column, configuration) {
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
    var DropDownField = (function () {
        function DropDownField() {
        }
        DropDownField.prototype.createDom = function (item, column, configuration) {
            var contentValue = item.v[column.name];
            if (contentValue === undefined) {
                contentValue = column.defaultValue;
            }
            var domDD = $("<select></select>");
            var asDD = column;
            for (var name in asDD.values) {
                var displayText = asDD.values[name];
                var domOption = $("<option></option>").attr("value", name).text(displayText);
                domDD.append(domOption);
            }
            domDD.val(contentValue);
            return domDD;
        };
        return DropDownField;
    }());
    exports.DropDownField = DropDownField;
    var SubElementField = (function () {
        function SubElementField() {
        }
        SubElementField.prototype.createDom = function (item, column, configuration) {
            var domSE = $("<ul></ul>");
            var asSE = column;
            // The content value
            var contentValue = item.v[column.name];
            for (var n in contentValue) {
                var subItem = contentValue[n];
                var func = function (innerItem) {
                    var domLine = $("<li><a href='#'></a></li>");
                    var domA = $("a", domLine);
                    domA.text(innerItem.v);
                    domA.click(function () {
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
                btn.click(function () {
                    DMClient.ExtentApi.createItemAsSubElement(item.ws, item.ext, item.uri, column.name, null).done(function (data) {
                        var uri = data.newuri;
                        configuration.navigation.navigateToItem(item.ws, item.ext, uri);
                        return false;
                    });
                });
                domSE.append(btn);
            }
            return domSE;
        };
        return SubElementField;
    }());
    exports.SubElementField = SubElementField;
    /**
     * Creates the DOM for the content as defined by column, when the column.fieldType is not set
     * @param item Item, whose content shall be shown
     * @param column Column defining the content
     * @param inEditMode true, if the field is in edit mode
     * @param configuration Configuration of the complete table
     */
    function createDefaultDomForContent(item, column, configuration) {
        var contentValue = item.v[column.name];
        if (contentValue === undefined) {
            contentValue = column.defaultValue;
        }
        // Enumerates all values
        if (column.isEnumeration) {
            var domResult = $("<ul></ul>");
            if (contentValue !== undefined) {
                for (var n in contentValue) {
                    var listValue = contentValue[n];
                    var domEntry = $("<li><a href='#'></a></li>");
                    var domInner = $("a", domEntry);
                    domInner.click((function (innerListValue) {
                        return function () {
                            if (configuration !== undefined && configuration.onItemSelect !== undefined) {
                                configuration.onItemSelect(innerListValue.u);
                            }
                            else {
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
        }
        else {
            var asTextBox = column;
            var isReadonly = configuration.isReadOnly || asTextBox.isReadOnly;
            // We have a textbox, so check if we have multiple line
            if (asTextBox.lineHeight !== undefined && asTextBox.lineHeight > 1) {
                var domTextBoxMultiple = $("<textarea class='form-control'></textarea>")
                    .attr('rows', asTextBox.lineHeight);
                domTextBoxMultiple.val(contentValue);
                if (isReadonly) {
                    domTextBoxMultiple.attr("readonly", "readonly");
                }
                return domTextBoxMultiple;
            }
            else {
                if (isReadonly) {
                    var domResult = $("<span class='dm-itemtable-data'></span>");
                    domResult.text(contentValue);
                    return domResult;
                }
                else {
                    var domTextBox = $("<input type='textbox' class='form-control' />");
                    domTextBox.val(contentValue);
                    if (asTextBox.isReadOnly) {
                        domTextBox.attr("readonly", "readonly");
                    }
                    return domTextBox;
                }
            }
        }
    }
});
//# sourceMappingURL=datenmeister-tables.js.map