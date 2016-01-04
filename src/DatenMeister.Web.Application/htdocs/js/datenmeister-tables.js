define(["require", "exports", "datenmeister-interfaces"], function (require, exports, DMI) {
    var ItemTableConfiguration = (function () {
        function ItemTableConfiguration() {
            this.onItemEdit = function (url, domRow) { return false; };
            this.onItemDelete = function (url, domRow) { return false; };
            this.supportSearchbox = true;
            this.supportNewItem = true;
            this.showColumnForId = false;
            this.supportPaging = true;
            this.itemsPerPage = 20;
        }
        return ItemTableConfiguration;
    })();
    exports.ItemTableConfiguration = ItemTableConfiguration;
    /*
        * Used to show a lot of items in a database. The table will use an array of MofObjects
        * as the datasource
        */
    var ItemListTable = (function () {
        function ItemListTable(dom, provider, configuration) {
            this.domContainer = dom;
            this.provider = provider;
            this.configuration = configuration;
            this.currentPage = 1;
            this.totalPages = 0;
            this.currentQuery = new DMI.ItemInExtentQuery();
            this.currentQuery.amount = configuration.itemsPerPage;
        }
        ItemListTable.prototype.throwOnPageChange = function () {
            this.currentQuery.offset = this.currentPage * (this.configuration.itemsPerPage - 1);
            this.reload();
            if (this.configuration.onPageChange !== undefined) {
                this.configuration.onPageChange(this.currentPage);
            }
        };
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
            if (this.configuration.supportSearchbox) {
                var domSearchBox = $("<div><input type='textbox' /></div>");
                var domInput = $("input", domSearchBox);
                $("input", domSearchBox).keyup(function () {
                    var searchValue = domInput.val();
                    tthis.lastProcessedSearchString = searchValue;
                    tthis.currentQuery.searchString = searchValue;
                    tthis.reload();
                    if (tthis.configuration.onSearch !== undefined) {
                        tthis.configuration.onSearch(searchValue);
                    }
                });
                this.domContainer.append(domSearchBox);
            }
            if (this.configuration.supportNewItem) {
                var domNewItem = $("<div><a href='#'>Create new item</a></div>");
                domNewItem.click(function () {
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
                domPrev.click(function () {
                    tthis.currentPage--;
                    tthis.currentPage = Math.max(1, tthis.currentPage);
                    domCurrentPage.val(tthis.currentPage.toString());
                    tthis.throwOnPageChange();
                    return false;
                });
                domNext.click(function () {
                    tthis.currentPage++;
                    tthis.currentPage = Math.min(tthis.totalPages, tthis.currentPage);
                    domCurrentPage.val(tthis.currentPage.toString());
                    tthis.throwOnPageChange();
                    return false;
                });
                domGo.click(function () {
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
            var columns = data.columns;
            for (var c in columns) {
                if (columns.hasOwnProperty(c)) {
                    var column = columns[c];
                    domColumn = $("<th></th>");
                    domColumn.text(column.title);
                    domRow.append(domColumn);
                }
            }
            // Creates the edit and delete column
            var domEditColumn = $("<th>EDIT</th>");
            domRow.append(domEditColumn);
            var domDeleteColumn = $("<th>DELETE</th>");
            domRow.append(domDeleteColumn);
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
            var tthis = this;
            this.domTotalNumber.text(data.totalItemCount);
            this.domFilteredNumber.text(data.filteredItemCount);
            if (this.configuration.supportPaging) {
                var domTotalPages = $(".dm_totalpages", this.domContainer);
                this.totalPages = Math.floor((data.filteredItemCount - 1) / this.configuration.itemsPerPage) + 1;
                domTotalPages.text(this.totalPages);
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
                    var columns = data.columns;
                    for (var c in columns) {
                        if (columns.hasOwnProperty(c)) {
                            var column = columns[c];
                            domColumn = $("<td></td>");
                            domColumn.text(item.v[column.name]);
                            domRow.append(domColumn);
                        }
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
                    domDeleteColumn.click((function (url, innerDomRow, innerDomA) {
                        return function () {
                            if (innerDomA.data("wasClicked") === true) {
                                return tthis.configuration.onItemDelete(url, innerDomRow);
                            }
                            else {
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
        };
        return ItemListTable;
    })();
    exports.ItemListTable = ItemListTable;
    var ItemContentConfiguration = (function () {
        function ItemContentConfiguration() {
            this.editFunction = function (url, property, domRow) { return false; };
            this.deleteFunction = function (url, property, domRow) { return false; };
            this.supportInlineEditing = true;
            this.supportNewProperties = true;
        }
        return ItemContentConfiguration;
    })();
    exports.ItemContentConfiguration = ItemContentConfiguration;
    var ItemContentTable = (function () {
        function ItemContentTable(item, configuration) {
            this.item = item;
            this.configuration = configuration;
        }
        ItemContentTable.prototype.show = function (dom) {
            var tthis = this;
            this.domContainer = dom;
            dom.empty();
            var domTable = $("<table class='table'></table>");
            // First the headline
            var domRow = $("<tr><th>Title</th><th>Value</th><th>EDIT</th><th>DELETE</th></tr>");
            domTable.append(domRow);
            // Now, the items
            var propertyValue = this.item.v;
            for (var property in propertyValue) {
                if (propertyValue.hasOwnProperty(property)) {
                    domRow = $("<tr></tr>");
                    var value = propertyValue[property];
                    var domColumn = $("<td class='table_column_name'></td>");
                    domColumn.data("column", "name");
                    domColumn.text(property);
                    domRow.append(domColumn);
                    domColumn = $("<td class='table_column_value'></td>");
                    domColumn.data("column", "value");
                    domColumn.text(value);
                    domRow.append(domColumn);
                    // Add Edit link
                    var domEditColumn = $("<td class='hl table_column_edit'><a href='#'>EDIT</a></td>");
                    $("a", domEditColumn).click((function (url, property, idomRow, idomA) {
                        return function () {
                            if (tthis.configuration.supportInlineEditing) {
                                tthis.startInlineEditing(property, idomRow);
                                return false;
                            }
                            else {
                                return tthis.configuration.editFunction(url, property, idomRow);
                            }
                        };
                    })(this.item.uri, property, domRow, domA));
                    domRow.append(domEditColumn);
                    var domDeleteColumn = $("<td class='hl table_column_delete'><a href='#'>DELETE</a></td>");
                    var domA = $("a", domDeleteColumn);
                    $("a", domDeleteColumn).click((function (url, property, idomRow, idomA) {
                        return function () {
                            if (idomA.data("wasClicked") === true) {
                                return tthis.configuration.deleteFunction(url, property, idomRow);
                            }
                            else {
                                idomA.data("wasClicked", true);
                                idomA.text("CONFIRM");
                                return false;
                            }
                        };
                    })(this.item.uri, property, domRow, domA));
                    domRow.append(domDeleteColumn);
                    domTable.append(domRow);
                }
            }
            // Add new property
            if (this.configuration.supportNewProperties) {
                this.offerNewProperty(domTable);
            }
            dom.append(domTable);
        };
        ItemContentTable.prototype.startInlineEditing = function (property, domRow) {
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
            domEditOK.on('click', function () {
                var newValue = domTextBox.val();
                tthis.item.v[property] = newValue;
                if (tthis.configuration.onEditProperty !== undefined) {
                    tthis.configuration.onEditProperty(tthis.item.uri, property, newValue);
                }
                tthis.show(tthis.domContainer);
                return false;
            });
            domEditCancel.on('click', function () {
                // Rebuilds the complete table
                tthis.show(tthis.domContainer);
                return false;
            });
        };
        ItemContentTable.prototype.offerNewProperty = function (domTable) {
            var tthis = this;
            var domNewProperty = $("<tr><td colspan='4'><a href='#'>NEW PROPERTY</a></td></tr>");
            $("a", domNewProperty).click(function () {
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
                $("a", domNewPropertyEdit).click(function () {
                    var property = inputProperty.val();
                    var newValue = inputValue.val();
                    tthis.item.v[property] = newValue;
                    if (tthis.configuration.onNewProperty !== undefined) {
                        tthis.configuration.onNewProperty(tthis.item.uri, property, newValue);
                    }
                    tthis.show(tthis.domContainer);
                    return false;
                });
                $("a", domNewPropertyCancel).click(function () {
                    tthis.show(tthis.domContainer);
                    return false;
                });
                return false;
            });
            domTable.append(domNewProperty);
        };
        return ItemContentTable;
    })();
    exports.ItemContentTable = ItemContentTable;
});
//# sourceMappingURL=datenmeister-tables.js.map