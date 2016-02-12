define(["require", "exports", "datenmeister-interfaces"], function (require, exports, DMI) {
    var ItemTableConfiguration = (function () {
        function ItemTableConfiguration() {
            this.onItemEdit = function (url) { return false; };
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
            this.currentQuery = new DMI.PostModels.ItemInExtentQuery();
            this.currentQuery.amount = configuration.itemsPerPage;
        }
        ItemListTable.prototype.throwOnPageChange = function () {
            this.currentQuery.offset = (this.currentPage - 1) * this.configuration.itemsPerPage;
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
            var domToolbar = $("<div class='dm-toolbar row'></div>");
            if (this.configuration.supportNewItem) {
                var domNewItem = $("<div class='col-md-3'><a href='#' class='btn btn-default'>Create new item</a></div>");
                domNewItem.click(function () {
                    if (tthis.configuration.onNewItemClicked !== undefined) {
                        tthis.configuration.onNewItemClicked();
                    }
                    return false;
                });
                domToolbar.append(domNewItem);
            }
            if (this.configuration.supportPaging) {
                var domPaging = $("<div class='col-md-6 text-center form-inline'>"
                    + "<a href='#' class='dm-prevpage btn btn-default'>&lt;&lt;</a> Page "
                    + "<input class='form-control dm-page-selected' type='textbox' value='1'/> of "
                    + "<span class='dm_totalpages'> </span> "
                    + "<a href='#' class='dm-jumppage btn btn-default'>GO</a>&nbsp;"
                    + "<a href='#' class='dm-nextpage btn btn-default'>&gt;&gt;</a>");
                domToolbar.append(domPaging);
                var domPrev = $(".dm-prevpage", domPaging);
                var domNext = $(".dm-nextpage", domPaging);
                var domGo = $(".dm-jumppage", domPaging);
                var domCurrentPage = $(".dm-page-selected", domPaging);
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
            if (this.configuration.supportSearchbox) {
                var domSearchBox = $("<div class='form-inline col-md-3'><input type='textbox' class='form-control' placeholder='Search...' /></div>");
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
                domToolbar.append(domSearchBox);
            }
            this.domContainer.append(domToolbar);
            var domAmount = $("<div>Total: <span class='totalnumber'>##</span>, Filtered: <span class='filterednumber'>##</span>");
            this.domTotalNumber = $(".totalnumber", domAmount);
            this.domFilteredNumber = $(".filterednumber", domAmount);
            if (this.configuration.layout !== undefined) {
                this.configuration.layout.setStatus(domAmount);
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
            var domEditColumn = $("<th></th>");
            domRow.append(domEditColumn);
            var domDeleteColumn = $("<th></th>");
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
            var _this = this;
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
                            domColumn = $("<td></td>");
                            domColumn.append(createDomForContent(item, columns[c], false, this.configuration));
                            domRow.append(domColumn);
                        }
                    }
                    // Add Edit link
                    var domEditColumn = $("<td class='hl'><button href='#' class='btn btn-primary'>Edit</button></td>");
                    domEditColumn.click((function (url) {
                        return function () { return _this.configuration.onItemEdit(url); };
                    })(item.uri));
                    domRow.append(domEditColumn);
                    var domDeleteColumn = $("<td class='hl'><button href='#' class='btn btn-danger'>Delete</button></td>");
                    var domA = $("button", domDeleteColumn);
                    domDeleteColumn.click((function (url, innerDomRow, innerDomA) {
                        return function () {
                            if (innerDomA.data("wasClicked") === true) {
                                return _this.configuration.onItemDelete(url, innerDomRow);
                            }
                            else {
                                innerDomA.data("wasClicked", true);
                                innerDomA.text("Confirm");
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
    function createDomForContent(item, column, inEditMode, configuration) {
        if (inEditMode === undefined) {
            inEditMode = false;
        }
        var tthis = this;
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
                            if (configuration !== undefined && configuration.onItemEdit !== undefined) {
                                configuration.onItemEdit(innerListValue.u);
                            }
                            else {
                                alert('No Event handler within createDomForContent');
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
            if (inEditMode) {
                var domTextBox = $("<input type='textbox' class='form-control' />");
                domTextBox.val(contentValue);
                return domTextBox;
            }
            else {
                var domResult = $("<span></span>");
                domResult.text(contentValue);
                return domResult;
            }
        }
    }
    var ItemContentConfiguration = (function () {
        function ItemContentConfiguration() {
            this.startWithEditMode = true;
            this.autoProperties = false;
            this.supportNewProperties = true;
            this.columns = new Array();
        }
        ItemContentConfiguration.prototype.addColumn = function (column) {
            this.columns[this.columns.length] = column;
        };
        return ItemContentConfiguration;
    })();
    exports.ItemContentConfiguration = ItemContentConfiguration;
    var ItemContentTable = (function () {
        function ItemContentTable(item, configuration) {
            this.item = item;
            this.configuration = configuration;
        }
        ItemContentTable.prototype.show = function (dom) {
            var _this = this;
            var tthis = this;
            this.domContainer = dom;
            this.domContainer.empty();
            dom.empty();
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
                var domForEdit = createDomForContent(this.item, column, true);
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
            var domOkCancel = $("<tr><td colspan='3' class='text-right'><button class='btn btn-primary dm-button-ok'>OK</button><button class='btn btn-default dm-button-cancel'>Cancel</button></td></tr>");
            var domOk = $(".dm-button-ok", domOkCancel);
            var domCancel = $(".dm-button-cancel", domOkCancel);
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
            var domNewProperty = $("<tr><td colspan='4'><button class='btn btn-default'>NEW PROPERTY</button></td></tr>");
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
                    if (tthis.configuration.onNewProperty !== undefined) {
                        tthis.configuration.onNewProperty(tthis.item.uri, property, newValue);
                    }
                    // Adds the new property to the autogenerated rows                    
                    var column = {
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
    })();
    exports.ItemContentTable = ItemContentTable;
});
//# sourceMappingURL=datenmeister-tables.js.map