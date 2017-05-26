var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
define(["require", "exports", "./datenmeister-interfaces", "./datenmeister-clientinterface", "./datenmeister-tables", "./datenmeister-client", "./datenmeister-query", "./datenmeister-dialogs", "./datenmeister-toolbar"], function (require, exports, DMI, DMCI, DMTables, DMClient, DMQuery, DMDialog, DMToolbar) {
    "use strict";
    exports.__esModule = true;
    // Defines a base implementation of the IView interface
    var ViewBase = (function () {
        function ViewBase(viewport) {
            this.viewport = viewport;
            this.content = $("<div></div>");
        }
        ViewBase.prototype.getContent = function () {
            return this.content;
        };
        ViewBase.prototype.getViewState = function () {
            if (this.viewState == null || this.viewState == undefined) {
                return null;
            }
            return this.viewState;
        };
        ViewBase.prototype.setViewState = function (layoutInformation) {
            this.viewState = layoutInformation;
        };
        ViewBase.prototype.addContent = function (domContent) {
            this.content.append(domContent);
        };
        ViewBase.prototype.addButtonLink = function (displayText, onClick) {
            var domItem = $("<input type='button' class='btn'></input>");
            domItem.val(displayText);
            domItem.click(onClick);
            this.content.append(domItem);
            return domItem;
        };
        ViewBase.prototype.addText = function (text) {
            var result = $("<div></div>");
            result.text(text);
            this.content.append(text);
            return result;
        };
        ViewBase.prototype.addEmptyDiv = function () {
            var result = $("<div></div>");
            this.content.append(result);
            return result;
        };
        ViewBase.prototype.addToolbar = function () {
            return new DMToolbar.Toolbar(this.content);
        };
        ViewBase.prototype.refresh = function () {
        };
        return ViewBase;
    }());
    exports.ViewBase = ViewBase;
    var ListView = (function (_super) {
        __extends(ListView, _super);
        function ListView(viewport) {
            return _super.call(this, viewport) || this;
        }
        return ListView;
    }(ViewBase));
    exports.ListView = ListView;
    var WorkspaceList;
    (function (WorkspaceList) {
        function navigateToWorkspaces(viewport) {
            var view = new WorkspaceView(viewport);
            view.load();
            viewport.setView(view);
            return view;
        }
        WorkspaceList.navigateToWorkspaces = navigateToWorkspaces;
        var WorkspaceView = (function (_super) {
            __extends(WorkspaceView, _super);
            function WorkspaceView(viewport) {
                return _super.call(this, viewport) || this;
            }
            WorkspaceView.prototype.load = function () {
                var result = $.Deferred();
                var tthis = this;
                DMClient.WorkspaceApi.getAllWorkspaces()
                    .done(function (data) {
                    tthis.createHtmlForWorkbenchs(data);
                    result.resolve(true);
                });
                this.setViewState({
                    type: DMI.Api.PageType.Workspaces
                });
                return result;
            };
            WorkspaceView.prototype.createHtmlForWorkbenchs = function (data) {
                var _this = this;
                var tthis = this;
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
                            ExtentList.navigateToExtents(tthis.viewport, workspaceId);
                            return false;
                        }); })(entry));
                        $(compiledTable).append(dom);
                    }
                }
                this.content.append(compiledTable);
                this.addButtonLink("Add new Workspace", function () { return DMDialog.showDialogNewWorkspace(_this.viewport); });
            };
            return WorkspaceView;
        }(ViewBase));
        WorkspaceList.WorkspaceView = WorkspaceView;
    })(WorkspaceList = exports.WorkspaceList || (exports.WorkspaceList = {}));
    var ExtentList;
    (function (ExtentList) {
        function navigateToExtents(viewport, workspaceId) {
            var view = new ExtentView(viewport, workspaceId);
            view.load();
            viewport.setView(view);
            return view;
        }
        ExtentList.navigateToExtents = navigateToExtents;
        var ExtentView = (function (_super) {
            __extends(ExtentView, _super);
            function ExtentView(viewport, ws) {
                var _this = _super.call(this, viewport) || this;
                _this.ws = ws;
                return _this;
            }
            ExtentView.prototype.load = function () {
                var _this = this;
                var tthis = this;
                // TODO tthis.createTitle(workspaceId);
                this.onItemView = function (ws, extentUrl, itemUrl) {
                    ItemList.navigateToItems(_this.viewport, ws, extentUrl);
                    return false;
                };
                var callback = $.Deferred();
                $.ajax({
                    url: "/api/datenmeister/extent/all?ws=" + encodeURIComponent(tthis.ws),
                    cache: false,
                    success: function (data) {
                        tthis.createHtmlForWorkspace(data);
                        callback.resolve(true);
                    },
                    error: function (data) {
                        callback.reject(false);
                    }
                });
                this.setViewState({
                    type: DMI.Api.PageType.Extents,
                    workspace: this.ws
                });
                return callback;
            };
            ExtentView.prototype.createHtmlForWorkspace = function (data) {
                var _this = this;
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
                            $(".data", dom)
                                .click((function (localEntry) { return (function () {
                                ItemList.navigateToItems(_this.viewport, tthis.ws, localEntry.uri);
                                return false;
                            }); })(entry));
                            compiledTable.append(dom);
                        }
                    }
                    this.content.append(compiledTable);
                }
                this.addButtonLink("Add new Extent", function () { return DMDialog.showNavigationForNewExtents(tthis.viewport, tthis.ws); });
            };
            return ExtentView;
        }(ListView));
        ExtentList.ExtentView = ExtentView;
    })(ExtentList = exports.ExtentList || (exports.ExtentList = {}));
    var ItemList;
    (function (ItemList) {
        function navigateToItems(viewport, ws, extentUrl, viewname) {
            var url = "#ws=" + encodeURIComponent(ws) + "&ext=" + encodeURIComponent(extentUrl);
            if (viewname !== undefined && viewname !== null) {
                url += "&view=" + encodeURIComponent(viewname);
            }
            history.pushState({}, "", url);
            this.showItems(ws, extentUrl, viewname);
        }
        ItemList.navigateToItems = navigateToItems;
        function exportExtent(viewport, ws, extentUrl) {
            window.open("/api/datenmeister/extent/extent_export_csv?ws=" + encodeURIComponent(ws) + "&extent=" + encodeURIComponent(extentUrl));
        }
        ItemList.exportExtent = exportExtent;
        var ItemsOfExtentView = (function (_super) {
            __extends(ItemsOfExtentView, _super);
            function ItemsOfExtentView(viewport) {
                var _this = _super.call(this, viewport) || this;
                _this.supportSearchbox = true;
                _this.supportNewItem = true;
                _this.supportPaging = true;
                _this.supportViews = true;
                _this.supportMetaClasses = true;
                return _this;
            }
            ItemsOfExtentView.prototype.load = function (ws, extentUrl, query) {
                var _this = this;
                var tthis = this;
                // Creates the layout configuration and the handling on requests of the user
                var configuration = new DMTables.ItemListTableConfiguration();
                configuration.onItemEdit = function (url) {
                    if (tthis.onItemEdit !== undefined) {
                        tthis.onItemEdit(ws, extentUrl, url);
                    }
                    return false;
                };
                configuration.onItemSelect = function (url) {
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
                // Creates the provider
                var provider = new DMQuery.ItemsFromExtentProvider(ws, extentUrl);
                // Creates the table
                this.toolbar = this.addToolbar();
                var container = this.addEmptyDiv();
                var table = new DMTables.ItemListTable(container, provider, configuration);
                if (query !== undefined && query !== null) {
                    table.currentQuery = query;
                }
                this.onViewChanged = function (viewUrl) {
                    query.view = viewUrl;
                    tthis.load(ws, extentUrl, query);
                };
                this.setViewState({
                    type: DMI.Api.PageType.Items,
                    workspace: ws,
                    extent: extentUrl
                });
                if (this.supportNewItem) {
                    var itemNew = new DMToolbar.ToolBarButtonItem("newitem", "Create Item");
                    itemNew.onClicked = function () {
                        var view = new CreatetableTypesView(_this.viewport, ws, extentUrl);
                        _this.viewport.setView(view);
                    };
                    this.toolbar.addItem(itemNew);
                }
                // Adds the searchbox and connects it to the tables
                if (this.supportSearchbox) {
                    var itemSearch = new DMToolbar.ToolbarSearchbox();
                    itemSearch.onSearch = function (searchText) {
                        table.currentQuery.searchString = searchText;
                        table.reload();
                    };
                    this.toolbar.addItem(itemSearch);
                }
                if (this.supportViews) {
                    var itemView = new DMToolbar.ToolbarViewSelection(ws, extentUrl);
                    itemView.onViewChanged = function (viewUrl) {
                        table.currentQuery.view = viewUrl;
                        table.reload();
                    };
                    this.toolbar.addItem(itemView);
                }
                if (this.supportMetaClasses) {
                    this.toolbarMetaClasses = new DMToolbar.ToolbarMetaClasses(ws, extentUrl);
                    this.toolbarMetaClasses.onItemClicked = function (viewUrl) {
                        alert('X');
                        table.reload();
                    };
                    this.toolbar.addItem(this.toolbarMetaClasses);
                    table.onDataReceived.addListener(function (data) {
                        tthis.toolbarMetaClasses.updateLayout(data.metaClasses);
                    });
                }
                if (this.supportPaging) {
                    var itemPaging = new DMToolbar.ToolbarPaging();
                    itemPaging.onPageChange = function (page) {
                        table.currentQuery.offset = (page - 1) * table.configuration.itemsPerPage;
                        table.reload();
                    };
                    table.configuration.paging = itemPaging;
                    this.toolbar.addItem(itemPaging);
                }
                table.loadAndShow();
            };
            ItemsOfExtentView.prototype.showItems = function (viewport, workspaceId, extentUrl, viewname) {
                var tthis = this;
                // TODO: this.viewport.createTitle(workspaceId, extentUrl);
                var extentView = new ItemsOfExtentView(this.viewport);
                extentView.onItemEdit = function (ws, extentUrl, itemUrl) {
                    ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, itemUrl);
                };
                extentView.onItemView = function (ws, extentUrl, itemUrl) {
                    ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, itemUrl, undefined, { isReadonly: true });
                };
                extentView.onItemCreated = function (ws, extentUrl, itemUrl) {
                    ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, itemUrl);
                };
                var query = new DMCI.Out.ItemTableQuery();
                query.view = viewname;
                query.amount = 20;
                extentView.load(workspaceId, extentUrl, query);
            };
            return ItemsOfExtentView;
        }(ListView));
        ItemList.ItemsOfExtentView = ItemsOfExtentView;
    })(ItemList = exports.ItemList || (exports.ItemList = {}));
    var ItemDetail;
    (function (ItemDetail) {
        function navigateToItem(viewport, ws, extentUrl, itemUrl, viewname, settings) {
            var url = "#ws=" + encodeURIComponent(ws) + "&ext=" + encodeURIComponent(extentUrl) + "&item=" + encodeURIComponent(itemUrl);
            if (settings !== undefined && settings !== null) {
                if (settings.isReadonly) {
                    url += "&mode=readonly";
                }
            }
            if (viewname !== undefined && viewname !== null) {
                url += "&view=" + encodeURIComponent(viewname);
            }
            history.pushState({}, "", url);
            this.showItem(ws, extentUrl, itemUrl, viewname, settings);
        }
        ItemDetail.navigateToItem = navigateToItem;
        var ItemView = (function (_super) {
            __extends(ItemView, _super);
            function ItemView(viewport) {
                var _this = _super.call(this, viewport) || this;
                _this.supportViews = true;
                return _this;
            }
            ItemView.prototype.load = function (ws, extentUrl, itemUrl, settings) {
                var tthis = this;
                this.setViewState({
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
                configuration.columns = data.c.fields;
                var isReadonly = false;
                this.toolbar = this.addToolbar();
                if (settings === undefined) {
                    settings = new DMI.Navigation.ItemViewSettings();
                }
                isReadonly = settings.isReadonly;
                configuration.isReadOnly = isReadonly;
                configuration.supportNewProperties = !isReadonly;
                var table = new DMTables.ItemContentTable(data, configuration);
                if (isReadonly) {
                    configuration.onOkForm = function () {
                        ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                    };
                    configuration.onCancelForm = function () {
                        ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                    };
                }
                else {
                    configuration.onOkForm = function () {
                        DMClient.ItemApi.setProperties(ws, extentUrl, itemUrl, table.item)
                            .done(function () {
                            ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                        });
                    };
                    configuration.onCancelForm = function () {
                        ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                    };
                }
                configuration.onItemSelect = function (url) {
                    if (tthis.onItemView !== undefined) {
                        tthis.onItemView(ws, extentUrl, url);
                    }
                    return false;
                };
                if (this.supportViews) {
                    var itemView = new DMToolbar.ToolbarViewSelection(ws, extentUrl, itemUrl);
                    itemView.onViewChanged = function (viewUrl) {
                        navigateToItem(tthis.viewport, ws, extentUrl, itemUrl, viewUrl, settings);
                    };
                    this.toolbar.addItem(itemView);
                }
                configuration.onEditButton = function () {
                    settings.isReadonly = false;
                    navigateToItem(tthis.viewport, ws, extentUrl, itemUrl, null, settings);
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
                    var domMetaClassLink = $("<span><a href='#'>3</a> (<span class='fullname'></span>)</span>");
                    $("a", domMetaClassLink).text(data.metaclass.name);
                    $(".fullname", domMetaClassLink).text(data.metaclass.fullname);
                    $("a", domMetaClassLink).click(function () {
                        navigateToItem(tthis.viewport, data.metaclass.ws, data.metaclass.ext, data.metaclass.uri);
                        return false;
                    });
                    $(".dm-tablecell-metaclass", domTableInfo).append(domMetaClassLink);
                    $(".dm-tablecell-metaclass", domTableInfo).attr("title", data.metaclass.uri);
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
            };
            return ItemView;
        }(ViewBase));
        ItemDetail.ItemView = ItemView;
    })(ItemDetail = exports.ItemDetail || (exports.ItemDetail = {}));
    function navigateToDialog(viewport, configuration) {
        var dialog = new DialogView(this);
        dialog.createDialog(configuration);
        this.setView(dialog);
    }
    exports.navigateToDialog = navigateToDialog;
    // This class gives a navigation view with some links which can be clicked by the user and
    // a user-defined action is being performed
    var EmptyView = (function (_super) {
        __extends(EmptyView, _super);
        function EmptyView(viewport) {
            return _super.call(this, viewport) || this;
        }
        /**
         * Adds a link to the view
         * @param displayText Text to be shown
         * @param onClick The function that is called when the user clicks
         */
        EmptyView.prototype.addLink = function (displayText, onClick) {
            return this.addButtonLink(displayText, onClick);
        };
        return EmptyView;
    }(ViewBase));
    exports.EmptyView = EmptyView;
    var DialogView = (function (_super) {
        __extends(DialogView, _super);
        function DialogView(viewport) {
            return _super.call(this, viewport) || this;
        }
        DialogView.prototype.createDialog = function (configuration) {
            var value = new DMCI.In.ItemContentModel();
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
            this.setViewState({
                type: DMI.Api.PageType.Dialog,
                workspace: configuration.ws,
                extent: configuration.ext
            });
        };
        return DialogView;
    }(ViewBase));
    exports.DialogView = DialogView;
    var CreatetableTypesView = (function (_super) {
        __extends(CreatetableTypesView, _super);
        function CreatetableTypesView(viewport, ws, extentUrl) {
            var _this = _super.call(this, viewport) || this;
            _this.extentUrl = extentUrl;
            _this.ws = ws;
            var tthis = _this;
            _this.addButtonLink("Unspecified", function () {
                DMClient.ExtentApi.createItem(ws, extentUrl, null)
                    .done(function (innerData) {
                    ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, innerData.newuri);
                });
            });
            _this.addButtonLink("Unspecified", function () {
                DMClient.ExtentApi.createItem(ws, extentUrl, null)
                    .done(function (innerData) {
                    ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, innerData.newuri);
                });
            });
            var domLoaded = _this.addText("Loading...");
            // Adds the default one, which creates just an empty, non-defined item.
            // Adds the ones, that can be created
            DMClient.ExtentApi.getCreatableTypes(_this.ws, _this.extentUrl).done(function (data) {
                for (var typeKey in data.types) {
                    var func = function (x) {
                        var type = data.types[typeKey];
                        tthis.addButtonLink(type.name, function () {
                            DMClient.ExtentApi.createItem(ws, extentUrl, type.uri)
                                .done(function (innerData) {
                                ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, innerData.newuri);
                            });
                        });
                    };
                    func(typeKey);
                }
            });
            return _this;
        }
        return CreatetableTypesView;
    }(ViewBase));
    exports.CreatetableTypesView = CreatetableTypesView;
});
//# sourceMappingURL=datenmeister-view.js.map