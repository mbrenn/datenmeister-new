var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "./datenmeister-interfaces", "./datenmeister-tables", "./datenmeister-client", "./datenmeister-query"], function (require, exports, DMI, DMTables, DMClient, DMQuery) {
    "use strict";
    var ViewBase = (function () {
        function ViewBase(layout) {
            this.layout = layout;
            this.content = $("<div></div>");
        }
        ViewBase.prototype.getContent = function () {
            return this.content;
        };
        ViewBase.prototype.getLayoutInformation = function () {
            if (this.layoutInformation == null || this.layoutInformation == undefined) {
                throw "Layoutinformation is not set";
            }
            return this.layoutInformation;
        };
        ViewBase.prototype.setLayoutInformation = function (layoutInformation) {
            this.layoutInformation = layoutInformation;
        };
        return ViewBase;
    }());
    exports.ViewBase = ViewBase;
    var WorkspaceView = (function (_super) {
        __extends(WorkspaceView, _super);
        function WorkspaceView(layout) {
            _super.call(this, layout);
        }
        WorkspaceView.prototype.loadAndCreateHtmlForWorkbenchs = function () {
            var result = $.Deferred();
            var tthis = this;
            DMClient.WorkspaceApi.getAllWorkspaces()
                .done(function (data) {
                tthis.createHtmlForWorkbenchs(data);
                result.resolve(true);
            });
            this.setLayoutInformation({
                type: DMI.Api.PageType.Workspaces
            });
            return result;
        };
        WorkspaceView.prototype.createHtmlForWorkbenchs = function (data) {
            var _this = this;
            this.content.empty();
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
            this.content.append(compiledTable);
        };
        return WorkspaceView;
    }(ViewBase));
    exports.WorkspaceView = WorkspaceView;
    var ExtentView = (function (_super) {
        __extends(ExtentView, _super);
        function ExtentView(layout) {
            _super.call(this, layout);
        }
        ExtentView.prototype.loadAndCreateHtmlForWorkspace = function (ws) {
            var _this = this;
            var callback = $.Deferred();
            $.ajax({
                url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(ws),
                cache: false,
                success: function (data) {
                    _this.createHtmlForWorkspace(ws, data);
                    callback.resolve(true);
                },
                error: function (data) {
                    callback.reject(false);
                }
            });
            this.setLayoutInformation({
                type: DMI.Api.PageType.Extents,
                workspace: ws
            });
            return callback;
        };
        ExtentView.prototype.createHtmlForWorkspace = function (ws, data) {
            var tthis = this;
            this.content.empty();
            if (data.length === 0) {
                this.content.html("<p>No extents were found</p>");
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
                this.content.append(compiledTable);
            }
            // TODO: Replace with add link
            var newExtentButton = $("<input type= 'button' value='Add new Extent' class='btn'></input>");
            newExtentButton.click(function () { return tthis.layout.showNavigationForNewExtents(ws); });
            this.content.append(newExtentButton);
        };
        ExtentView.prototype.loadAndCreateHtmlForExtent = function (ws, extentUrl, query) {
            var _this = this;
            var tthis = this;
            // Creates the layout configuration and the handling on requests of the user
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
            // Creates the layout
            var provider = new DMQuery.ItemsFromExtentProvider(ws, extentUrl);
            var table = new DMTables.ItemListTable(this.content, provider, configuration);
            if (query !== undefined && query !== null) {
                table.currentQuery = query;
            }
            configuration.onNewItemClicked = function (metaclass) {
                DMClient.ExtentApi.createItem(ws, extentUrl, undefined, metaclass)
                    .done(function (innerData) {
                    _this.onItemCreated(ws, extentUrl, innerData.newuri);
                });
            };
            DMClient.ExtentApi.getCreatableTypes(ws, extentUrl).done(function (data) {
                table.setCreatableTypes(data.types);
            });
            this.setLayoutInformation({
                type: DMI.Api.PageType.Items,
                workspace: ws,
                extent: extentUrl
            });
            return table.loadAndShow();
        };
        return ExtentView;
    }(ViewBase));
    exports.ExtentView = ExtentView;
    var ItemView = (function (_super) {
        __extends(ItemView, _super);
        function ItemView(layout) {
            _super.call(this, layout);
        }
        ItemView.prototype.loadAndCreateHtmlForItem = function (ws, extentUrl, itemUrl, settings) {
            var tthis = this;
            this.setLayoutInformation({
                type: DMI.Api.PageType.ItemDetail,
                workspace: ws,
                extent: extentUrl,
                item: itemUrl
            });
            return DMClient.ItemApi.getItem(ws, extentUrl, itemUrl)
                .done(function (data) {
                tthis.createHtmlForItem(ws, extentUrl, itemUrl, data, settings);
            });
        };
        ItemView.prototype.createHtmlForItem = function (ws, extentUrl, itemUrl, data, settings) {
            var tthis = this;
            this.content.empty();
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
            configuration.onItemView = function (url) {
                if (tthis.onItemView !== undefined) {
                    tthis.onItemView(ws, extentUrl, url);
                }
                return false;
            };
            var domTableOwner = $("<div class='data-items'></div>");
            table.show(domTableOwner);
            var domTableInfo = $("<table class='dm-metatable'>" +
                "<tr>" +
                "<th>Id: </th><td class='dm-tablecell-id'>None</td>" +
                "</tr>" +
                "<tr>" +
                "<th>Uri: </th><td class='dm-tablecell-uri'>None</td>" +
                "</tr>" +
                "<tr>" +
                "<th>Metaclass: </th><td class='dm-tablecell-metaclass'></td>" +
                "</tr>" +
                "<tr>" +
                "<th>Layer: </th><td class='dm-tablecell-layer'></td>" +
                "</tr>" +
                "</table>");
            if (data.metaclass !== undefined && data.metaclass !== null) {
                var domMetaClassLink = $("<a href='#'>3</a>").text(data.metaclass.name);
                domMetaClassLink.click(function () {
                    tthis.layout.navigateToItem(data.metaclass.ws, data.metaclass.ext, data.metaclass.uri);
                });
                $(".dm-tablecell-metaclass", domTableInfo).append(domMetaClassLink);
            }
            if (data.id !== undefined && data.id !== null) {
                $(".dm-tablecell-id", domTableInfo).text(data.id);
            }
            if (data.layer !== undefined && data.layer !== null) {
                $(".dm-tablecell-layer", domTableInfo).text(data.layer);
            }
            if (data.uri !== undefined && data.uri !== null) {
                $(".dm-tablecell-uri", domTableInfo).text(data.uri);
            }
            this.content.append(domTableOwner);
            this.content.append(domTableInfo);
            this.content.append(domTableInfo);
        };
        return ItemView;
    }(ViewBase));
    exports.ItemView = ItemView;
    // This class gives a navigation view with some links which can be clicked by the user and
    // a user-defined action is being performed
    var NavigationView = (function (_super) {
        __extends(NavigationView, _super);
        function NavigationView(layout) {
            _super.call(this, layout);
            var domList = $("<ul class='dm-navigation-list'></ul>");
            this.domList = domList;
            this.content.append(this.domList);
        }
        NavigationView.prototype.addLink = function (displayText, onClick) {
            this.insertLink(this.domList, displayText, onClick);
        };
        NavigationView.prototype.insertLink = function (container, displayText, onClick) {
            var domItem = $("<li></li>");
            domItem.text(displayText);
            domItem.click(onClick);
            container.append(domItem);
        };
        return NavigationView;
    }(ViewBase));
    exports.NavigationView = NavigationView;
    var DialogView = (function (_super) {
        __extends(DialogView, _super);
        function DialogView(layout) {
            _super.call(this, layout);
        }
        DialogView.prototype.createDialog = function (configuration) {
            var value = new DMI.Table.DataTableItem();
            var tableConfiguration = new DMTables.ItemContentConfiguration();
            tableConfiguration.autoProperties = false;
            tableConfiguration.columns = configuration.columns;
            tableConfiguration.isReadOnly = false;
            tableConfiguration.supportNewProperties = false;
            tableConfiguration.onCancelForm = function () {
                if (configuration.onCancelForm !== undefined) {
                    configuration.onCancelForm();
                }
            };
            tableConfiguration.onOkForm = function () {
                if (configuration.onOkForm !== undefined) {
                    configuration.onOkForm(value);
                }
            };
            var itemTable = new DMTables.ItemContentTable(value, tableConfiguration);
            itemTable.show(this.content);
            this.setLayoutInformation({
                type: DMI.Api.PageType.Dialog,
                workspace: configuration.ws,
                extent: configuration.extent
            });
        };
        return DialogView;
    }(ViewBase));
    exports.DialogView = DialogView;
});
//# sourceMappingURL=datenmeister-view.js.map