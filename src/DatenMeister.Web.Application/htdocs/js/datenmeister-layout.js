define(["require", "exports", "datenmeister-tables", "datenmeister-client"], function (require, exports, DMTables, DMClient) {
    var WorkspaceLayout = (function () {
        function WorkspaceLayout() {
        }
        WorkspaceLayout.prototype.loadAndCreateHtmlForWorkbenchs = function (container) {
            var result = $.Deferred();
            var tthis = this;
            DMClient.WorkspaceApi.getAllWorkspaces()
                .done(function (data) {
                tthis.createHtmlForWorkbenchs(container, data);
                result.resolve(true);
            });
            return result;
        };
        WorkspaceLayout.prototype.createHtmlForWorkbenchs = function (container, data) {
            var _this = this;
            container.empty();
            var compiledTable = $($("#template_workspace_table").html());
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                if (data.hasOwnProperty(n)) {
                    var entry = data[n];
                    var line = compiled(entry);
                    var dom = $(line);
                    $(".data", dom).click((function (localEntry) { return (function () {
                        var workspaceId = localEntry.id;
                        if (_this.onWorkspaceSelected != undefined) {
                            _this.onWorkspaceSelected(workspaceId);
                        }
                        return false;
                    }); })(entry));
                    $(compiledTable).append(dom);
                }
            }
            container.append(compiledTable);
        };
        return WorkspaceLayout;
    })();
    exports.WorkspaceLayout = WorkspaceLayout;
    var ExtentLayout = (function () {
        function ExtentLayout() {
        }
        ExtentLayout.prototype.loadAndCreateHtmlForWorkspace = function (container, ws) {
            var _this = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
                cache: false,
                success: function (data) {
                    _this.createHtmlForWorkspace(container, ws, data);
                    callback.resolve(null);
                },
                error: function (data) {
                    callback.reject(null);
                }
            });
            return callback;
        };
        ExtentLayout.prototype.createHtmlForWorkspace = function (container, ws, data) {
            var tthis = this;
            container.empty();
            if (data.length === 0) {
                container.html("<p>No extents were found</p>");
            }
            else {
                var compiledTable = $($("#template_extent_table").html());
                var compiled = _.template($("#template_extent").html());
                for (var n in data) {
                    if (data.hasOwnProperty(n)) {
                        var entry = data[n];
                        var line = compiled(entry);
                        var dom = $(line);
                        $(".data", dom).click((function (localEntry) { return (function () {
                            if (tthis.onExtentSelected !== undefined) {
                                tthis.onExtentSelected(ws, localEntry.uri);
                            }
                            return false;
                        }); })(entry));
                        compiledTable.append(dom);
                    }
                }
                container.append(compiledTable);
            }
        };
        ExtentLayout.prototype.loadAndCreateHtmlForExtent = function (container, ws, extentUrl, query) {
            var _this = this;
            var callback = $.Deferred();
            this.loadHtmlForExtent(ws, extentUrl)
                .done(function (data) {
                _this.createHtmlForExtent(container, ws, extentUrl, data);
                callback.resolve(null);
            })
                .fail(function (data) {
                callback.reject(null);
            });
            return callback;
        };
        ExtentLayout.prototype.loadHtmlForExtent = function (ws, extentUrl, query) {
            var url = "/api/datenmeister/extent/items?ws=" + encodeURIComponent(ws)
                + "&extent=" + encodeURIComponent(extentUrl);
            if (query !== undefined && query !== null) {
                if (query.searchString !== undefined) {
                    url += "&search=" + encodeURIComponent(query.searchString);
                }
                if (query.offset !== undefined && query.offset !== null) {
                    url += "&o=" + encodeURIComponent(query.offset.toString());
                }
                if (query.amount !== undefined && query.amount !== null) {
                    url += "&a=" + encodeURIComponent(query.amount.toString());
                }
            }
            return $.ajax({
                url: url,
                cache: false
            });
        };
        ExtentLayout.prototype.createHtmlForExtent = function (container, ws, extentUrl, data) {
            var _this = this;
            var tthis = this;
            var configuration = new DMTables.ItemTableConfiguration();
            configuration.onItemEdit = function (url) {
                if (_this.onItemSelected !== undefined) {
                    _this.onItemSelected(ws, extentUrl, url);
                }
                return false;
            };
            configuration.onItemDelete = function (url, domRow) {
                var callback = DMClient.ExtentApi.deleteItem(ws, extentUrl, url);
                callback
                    .done(function () {
                    // tthis.loadAndCreateHtmlForItems(container, ws, extentUrl);
                    domRow.find("td").fadeOut(500, function () { domRow.remove(); });
                })
                    .fail(function () { alert("FAILED"); });
                return false;
            };
            var table = new DMTables.ItemListTable(container, data, configuration);
            configuration.onSearch = function (searchString) {
                _this.loadHtmlForExtent(ws, extentUrl, { searchString: searchString })
                    .done(function (innerData) {
                    if (table.lastProcessedSearchString === innerData.search) {
                        table.updateItems(innerData);
                    }
                });
            };
            configuration.onNewItemClicked = function () {
                DMClient.ExtentApi.createItem(ws, extentUrl)
                    .done(function (innerData) {
                    _this.onItemCreated(ws, extentUrl, innerData.newuri);
                });
            };
            var itemsPerPage = 10;
            configuration.onPageChange = function (newPage) {
                tthis.loadHtmlForExtent(ws, extentUrl, {
                    offset: itemsPerPage * (newPage - 1),
                    amount: itemsPerPage
                })
                    .done(function (innerData) {
                    table.updateItems(innerData);
                });
            };
            table.show();
        };
        ExtentLayout.prototype.loadAndCreateHtmlForItem = function (container, ws, extentUrl, itemUrl) {
            var _this = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/item?ws=" + encodeURIComponent(ws)
                    + "&extent=" + encodeURIComponent(extentUrl)
                    + "&item=" + encodeURIComponent(itemUrl),
                cache: false,
                success: function (data) {
                    _this.createHtmlForItem(container, ws, extentUrl, itemUrl, data);
                    callback.resolve(null);
                },
                error: function (data) {
                    callback.reject(null);
                }
            });
            return callback;
        };
        ExtentLayout.prototype.createHtmlForItem = function (jQuery, ws, extentUrl, itemUrl, data) {
            var configuration = new DMTables.ItemContentConfiguration();
            configuration.deleteFunction = function (url, property, domRow) {
                DMClient.ItemApi.deleteProperty(ws, extentUrl, itemUrl, property).done(function () { return domRow.find("td").fadeOut(500, function () { domRow.remove(); }); });
                return false;
            };
            configuration.onEditProperty = function (url, property, newValue) {
                DMClient.ItemApi.setProperty(ws, extentUrl, itemUrl, property, newValue);
            };
            configuration.onNewProperty = function (url, property, newValue) {
                DMClient.ItemApi.setProperty(ws, extentUrl, itemUrl, property, newValue);
            };
            var table = new DMTables.ItemContentTable(data, configuration);
            table.show(jQuery);
        };
        return ExtentLayout;
    })();
    exports.ExtentLayout = ExtentLayout;
});
//# sourceMappingURL=datenmeister-layout.js.map