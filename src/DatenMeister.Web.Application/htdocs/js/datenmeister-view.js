define(["require", "exports", "datenmeister-tables", "datenmeister-client", "datenmeister-query"], function (require, exports, DMTables, DMClient, DMQuery) {
    var WorkspaceView = (function () {
        function WorkspaceView() {
        }
        WorkspaceView.prototype.loadAndCreateHtmlForWorkbenchs = function (container) {
            var result = $.Deferred();
            var tthis = this;
            DMClient.WorkspaceApi.getAllWorkspaces()
                .done(function (data) {
                tthis.createHtmlForWorkbenchs(container, data);
                result.resolve(true);
            });
            return result;
        };
        WorkspaceView.prototype.createHtmlForWorkbenchs = function (container, data) {
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
        return WorkspaceView;
    })();
    exports.WorkspaceView = WorkspaceView;
    var ExtentView = (function () {
        function ExtentView(layout) {
            this.layout = layout;
        }
        ExtentView.prototype.loadAndCreateHtmlForWorkspace = function (container, ws) {
            var _this = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
                cache: false,
                success: function (data) {
                    _this.createHtmlForWorkspace(container, ws, data);
                    callback.resolve(true);
                },
                error: function (data) {
                    callback.reject(false);
                }
            });
            return callback;
        };
        ExtentView.prototype.createHtmlForWorkspace = function (container, ws, data) {
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
        ExtentView.prototype.loadAndCreateHtmlForExtent = function (container, ws, extentUrl, query) {
            var _this = this;
            var tthis = this;
            var configuration = new DMTables.ItemTableConfiguration();
            configuration.onItemEdit = function (url) {
                if (tthis.onItemEdit !== undefined) {
                    tthis.onItemEdit(ws, extentUrl, url);
                }
                return false;
            };
            configuration.onItemView = function (url) {
                if (tthis.onItemView !== undefined) {
                    tthis.onItemView(ws, extentUrl, url);
                }
                return false;
            };
            configuration.onItemDelete = function (url, domRow) {
                var callback = DMClient.ExtentApi.deleteItem(ws, extentUrl, url);
                callback
                    .done(function () {
                    domRow.find("td").fadeOut(500, function () { domRow.remove(); });
                })
                    .fail(function () { alert("FAILED"); });
                return false;
            };
            configuration.layout = this.layout;
            var provider = new DMQuery.ItemsFromExtentProvider(ws, extentUrl);
            var table = new DMTables.ItemListTable(container, provider, configuration);
            if (query !== undefined && query !== null) {
                table.currentQuery = query;
            }
            configuration.onNewItemClicked = function () {
                DMClient.ExtentApi.createItem(ws, extentUrl)
                    .done(function (innerData) {
                    _this.onItemCreated(ws, extentUrl, innerData.newuri);
                });
            };
            return table.loadAndShow();
        };
        return ExtentView;
    })();
    exports.ExtentView = ExtentView;
    var ItemView = (function () {
        function ItemView(layout) {
            this.layout = layout;
        }
        ItemView.prototype.loadAndCreateHtmlForItem = function (container, ws, extentUrl, itemUrl, settings) {
            var tthis = this;
            return DMClient.ItemApi.getItem(ws, extentUrl, itemUrl)
                .done(function (data) {
                tthis.createHtmlForItem(container, ws, extentUrl, itemUrl, data, settings);
            });
        };
        ItemView.prototype.createHtmlForItem = function (jQuery, ws, extentUrl, itemUrl, data, settings) {
            var tthis = this;
            jQuery.empty();
            var configuration = new DMTables.ItemContentConfiguration();
            configuration.columns = data.c;
            var isReadonly = false;
            if (settings !== undefined && settings !== null) {
                if (settings.isReadonly !== undefined && settings.isReadonly !== null) {
                    isReadonly = settings.isReadonly;
                }
            }
            configuration.isReadOnly = isReadonly;
            configuration.supportNewProperties = !isReadonly;
            /*
            configuration.deleteFunction = (url: string, property: string, domRow: JQuery) => {
                DMClient.ItemApi.deleteProperty(ws, extentUrl, itemUrl, property).done(() => domRow.find("td").fadeOut(500, () => { domRow.remove(); }));
                return false;
            };
    
            configuration.onEditProperty = (url: string, property: string, newValue: string) => {
                DMClient.ItemApi.setProperty(ws, extentUrl, itemUrl, property, newValue);
            };
            
            configuration.onNewProperty = (url: string, property: string, newValue: string) => {
                DMClient.ItemApi.setProperty(ws, extentUrl, itemUrl, property, newValue);
            };*/
            var table = new DMTables.ItemContentTable(data, configuration);
            if (isReadonly) {
                configuration.onOkForm = function () {
                    tthis.layout.navigateToItems(ws, extentUrl);
                };
                configuration.onCancelForm = function () {
                    tthis.layout.navigateToItems(ws, extentUrl);
                };
            }
            else {
                configuration.onOkForm = function () {
                    DMClient.ItemApi.setProperties(ws, extentUrl, itemUrl, table.item)
                        .done(function () {
                        tthis.layout.navigateToItems(ws, extentUrl);
                    });
                };
                configuration.onCancelForm = function () {
                    tthis.layout.navigateToItems(ws, extentUrl);
                };
            }
            var domTableOwner = $("<div></div>");
            table.show(domTableOwner);
            if (data.metaclass !== undefined && data.metaclass !== null) {
                var domMetaClass = $("<div><span>Metaclass: </span></div>");
                domMetaClass.append($("<span></span>").text(data.metaclass.name));
                jQuery.append(domMetaClass);
            }
            jQuery.append(domTableOwner);
        };
        return ItemView;
    })();
    exports.ItemView = ItemView;
});
//# sourceMappingURL=datenmeister-view.js.map