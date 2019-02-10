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
        ViewBase.prototype.emptyContent = function () {
            this.content.empty();
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
            this.content.empty();
        };
        ViewBase.prototype.load = function () {
            this.content = $("<div></div>");
            this.refresh();
            return this.content;
        };
        /**
         * This event will be called after the dom has been added to the window
         */
        ViewBase.prototype.onViewShown = function () {
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
            viewport.setView(view);
            return view;
        }
        WorkspaceList.navigateToWorkspaces = navigateToWorkspaces;
        var WorkspaceView = (function (_super) {
            __extends(WorkspaceView, _super);
            function WorkspaceView(viewport) {
                return _super.call(this, viewport) || this;
            }
            WorkspaceView.prototype.refresh = function () {
                var tthis = this;
                DMClient.WorkspaceApi.getAllWorkspaces()
                    .done(function (data) {
                    tthis.createHtmlForWorkbenchs(data);
                });
                this.setViewState({
                    type: DMI.Api.PageType.Workspaces
                });
            };
            WorkspaceView.prototype.createHtmlForWorkbenchs = function (data) {
                var _this = this;
                var tthis = this;
                var fields = [
                    new DMTables.Fields.TextboxField("id", "Name").readOnly(),
                    new DMTables.Fields.TextboxField("annotation", "Annotation").readOnly()
                ];
                var configuration = new DMTables.ListTableConfiguration();
                configuration.fields = fields;
                DMTables.Fields.addEditButton(configuration, function (item) {
                    var workspaceId = item.id;
                    ExtentList.navigateToExtents(tthis.viewport, workspaceId);
                    return false;
                });
                DMTables.Fields.addDeleteButton(configuration, function (item) {
                    var workspaceId = item.id;
                    DMClient.WorkspaceApi.deleteWorkspace(workspaceId).done(function () { return WorkspaceList.navigateToWorkspaces(tthis.viewport); });
                    return false;
                });
                var table = new DMTables.ListTableComposer(configuration, this.content);
                table.composeTable(data);
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
            ExtentView.prototype.refresh = function () {
                var _this = this;
                var tthis = this;
                // TODO tthis.createTitle(workspaceId);
                this.onItemView = function (ws, extentUrl, itemUrl) {
                    ItemList.navigateToItems(_this.viewport, ws, extentUrl);
                    return false;
                };
                DMClient.ExtentApi.getExtents(this.ws)
                    .done(function (data) {
                    tthis.createHtmlForWorkspace(data);
                });
                this.setViewState({
                    type: DMI.Api.PageType.Extents,
                    workspace: this.ws
                });
            };
            ExtentView.prototype.createHtmlForWorkspace = function (data) {
                var tthis = this;
                var fields = [
                    new DMTables.Fields.TextboxField("uri", "Uri").readOnly(),
                    new DMTables.Fields.TextboxField("count", "Items").readOnly(),
                    new DMTables.Fields.TextboxField("dataLayer", "Layer").readOnly(),
                    new DMTables.Fields.TextboxField("type", "Type").readOnly()
                ];
                var configuration = new DMTables.ListTableConfiguration();
                configuration.fields = fields;
                DMTables.Fields.addEditButton(configuration, function (item) {
                    ItemList.navigateToItems(tthis.viewport, tthis.ws, item.uri);
                });
                DMTables.Fields.addDeleteButton(configuration, function (item) {
                    DMClient.ExtentApi.deleteExtent(tthis.ws, item.uri)
                        .done(function () { return tthis.refresh(); });
                });
                var table = new DMTables.ListTableComposer(configuration, this.content);
                table.composeTable(data);
                this.addButtonLink("Add new Extent", function () { return DMDialog.showNavigationForNewExtents(tthis.viewport, tthis.ws); });
            };
            return ExtentView;
        }(ListView));
        ExtentList.ExtentView = ExtentView;
    })(ExtentList = exports.ExtentList || (exports.ExtentList = {}));
    var ItemList;
    (function (ItemList) {
        function navigateToItems(viewport, ws, extentUrl, viewname) {
            var view = new ItemsOfExtentView(viewport, ws, extentUrl);
            viewport.setView(view);
            return view;
        }
        ItemList.navigateToItems = navigateToItems;
        function exportExtent(viewport, ws, extentUrl) {
            window.open("/api/datenmeister/extent/extent_export_csv?ws=" + encodeURIComponent(ws) + "&extent=" + encodeURIComponent(extentUrl));
        }
        ItemList.exportExtent = exportExtent;
        var ItemsOfExtentView = (function (_super) {
            __extends(ItemsOfExtentView, _super);
            function ItemsOfExtentView(viewport, ws, extentUrl, query) {
                var _this = _super.call(this, viewport) || this;
                _this.supportSearchbox = true;
                _this.supportNewItem = true;
                _this.supportPaging = true;
                _this.supportViews = true;
                _this.supportMetaClasses = true;
                _this.ws = ws;
                _this.extentUrl = extentUrl;
                _this.query = query;
                if (_this.query === undefined || _this.query === null) {
                    _this.query = new DMCI.Out.ItemTableQuery();
                    _this.query.amount = 20;
                }
                return _this;
            }
            ItemsOfExtentView.prototype.refresh = function () {
                var _this = this;
                this.setViewState({
                    type: DMI.Api.PageType.Items,
                    workspace: this.ws,
                    extent: this.extentUrl
                });
                var tthis = this;
                // Creates the layout configuration and the handling on requests of the user
                /*
                configuration.onItemEdit = (url: string) => {
                    if (tthis.onItemEdit !== undefined) {
                        tthis.onItemEdit(tthis.ws, tthis.extentUrl, url);
                    }
    
                    return false;
                };
                configuration.onItemSelect = (url: string) => {
                    if (tthis.onItemView !== undefined) {
                        tthis.onItemView(tthis.ws, tthis.extentUrl, url);
                    }
    
                    return false;
                };
    
                configuration.onItemDelete = (url: string, domRow: JQuery) => {
                    var callback = DMClient.ExtentApi.deleteItem(tthis.ws, tthis.extentUrl, url);
                    callback
                        .done(() => {
                            domRow.find("td").fadeOut(500, () => { domRow.remove(); });
                        })
                        .fail(() => { alert("FAILED"); });
                    return false;
                };*/
                // Creates the provider
                var provider = new DMQuery.ItemsFromExtentProvider(this.ws, this.extentUrl);
                this.composeForProvider(provider);
                // Creates the table
                this.toolbar = this.addToolbar();
                var container = this.addEmptyDiv();
                this.table = new DMTables.ListTableComposer(new DMTables.ListTableConfiguration(), container);
                /*if (query !== undefined && query !== null) {
                    table.currentQuery = query;
                }*/
                /*this.onViewChanged = (viewUrl) => {
                    query.view = viewUrl;
                    tthis.load(ws, extentUrl, query);
                };*/
                if (this.supportNewItem) {
                    var itemNew = new DMToolbar.ToolBarButtonItem("newitem", "Create Item");
                    itemNew.onClicked = function () {
                        var view = new CreatetableTypesView(_this.viewport, tthis.ws, tthis.extentUrl);
                        _this.viewport.setView(view);
                    };
                    this.toolbar.addItem(itemNew);
                }
                /*
    
                // Adds the searchbox and connects it to the tables
                if (this.supportSearchbox) {
                    var itemSearch = new DMToolbar.ToolbarSearchbox();
                    itemSearch.onSearch = (searchText: string) => {
                        table.currentQuery.searchString = searchText;
                        table.reload();
                    };
    
                    this.toolbar.addItem(itemSearch);
                }
    
                if (this.supportViews) {
                    var itemView = new DMToolbar.ToolbarViewSelection(ws, extentUrl);
                    itemView.onViewChanged = viewUrl => {
                        table.currentQuery.view = viewUrl;
                        table.reload();
                    };
                    this.toolbar.addItem(itemView);
                }
    
                if (this.supportMetaClasses) {
                    this.toolbarMetaClasses = new DMToolbar.ToolbarMetaClasses(ws, extentUrl);
                    this.toolbarMetaClasses.onItemClicked = viewUrl => {
                        alert('X');
                        table.reload();
                    };
                    this.toolbar.addItem(this.toolbarMetaClasses);
    
                    table.onDataReceived.addListener((data) => {
                        tthis.toolbarMetaClasses.updateLayout(data.metaClasses);
                    });
                }
    
    
                if (this.supportPaging) {
                    var itemPaging = new DMToolbar.ToolbarPaging();
                    itemPaging.onPageChange = page => {
                        table.currentQuery.offset = (page - 1) * table.configuration.itemsPerPage;
                        table.reload();
                    };
    
                    table.configuration.paging = itemPaging;
                    this.toolbar.addItem(itemPaging);
                }
                */
            };
            ItemsOfExtentView.prototype.composeForProvider = function (provider) {
                var _this = this;
                provider.performQuery(this.query).done(function (items) {
                    var configuration = new DMTables.ListTableConfiguration();
                    for (var c in items.columns.fields) {
                        var column = items.columns.fields[c];
                        configuration.addField(new DMTables.Fields.TextboxField(column.name, column.title));
                    }
                    _this.table.configuration = configuration;
                    _this.table.items = items.items;
                    _this.table.composeContent();
                });
            };
            return ItemsOfExtentView;
        }(ListView));
        ItemList.ItemsOfExtentView = ItemsOfExtentView;
    })(ItemList = exports.ItemList || (exports.ItemList = {}));
    var ItemDetail;
    (function (ItemDetail) {
        function navigateToItem(viewport, ws, extentUrl, itemUrl, viewname, settings) {
            var itemView = new ItemView(this, ws, extentUrl, itemUrl, settings);
            itemView.onItemView = function (ws, extentUrl, itemUrl) {
                navigateToItem(viewport, ws, extentUrl, itemUrl, undefined, { isReadonly: true });
            };
            // TODO: this.createTitle(workspaceId, extentUrl, itemUrl);
            viewport.setView(itemView);
        }
        ItemDetail.navigateToItem = navigateToItem;
        var ItemView = (function (_super) {
            __extends(ItemView, _super);
            function ItemView(viewport, ws, extentUrl, itemUrl, settings) {
                var _this = _super.call(this, viewport) || this;
                _this.supportViews = true;
                _this.setViewState({
                    type: DMI.Api.PageType.ItemDetail,
                    workspace: ws,
                    extent: extentUrl,
                    item: itemUrl
                });
                return _this;
            }
            ItemView.prototype.load = function () {
                var tthis = this;
                /*
                DMClient.ItemApi.getItem(ws, extentUrl, itemUrl)
                    .done((data) => {
                        tthis.createHtmlForItem(ws, extentUrl, itemUrl, data, settings);
                    });*/
                throw "Not Implemented";
            };
            ItemView.prototype.createHtmlForItem = function (ws, extentUrl, itemUrl, data, settings) {
                throw ("Not Implemented");
                /*
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
                    configuration.onOkForm = () => {
                        ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                    };
                    configuration.onCancelForm = () => {
                        ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                    };
                } else {
                    configuration.onOkForm = () => {
                        DMClient.ItemApi.setProperties(ws, extentUrl, itemUrl, table.item)
                            .done(() => {
                                ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                            });
                    };
    
                    configuration.onCancelForm = () => {
                        ItemList.navigateToItems(tthis.viewport, ws, extentUrl);
                    };
                }
    
                configuration.onItemSelect = (url: string) => {
                    if (tthis.onItemView !== undefined) {
                        tthis.onItemView(ws, extentUrl, url);
                    }
    
                    return false;
                };
    
                if (this.supportViews) {
                    var itemView = new DMToolbar.ToolbarViewSelection(ws, extentUrl, itemUrl);
                    itemView.onViewChanged = viewUrl => {
                        navigateToItem(tthis.viewport, ws, extentUrl, itemUrl, viewUrl, settings);
                    };
    
                    this.toolbar.addItem(itemView);
                }
    
                configuration.onEditButton = () => {
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
    
                    $("a", domMetaClassLink).click(() => {
                        navigateToItem(
                            tthis.viewport,
                            data.metaclass.ws,
                            data.metaclass.ext,
                            data.metaclass.uri
                        );
    
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
                */
            };
            return ItemView;
        }(ViewBase));
        ItemDetail.ItemView = ItemView;
    })(ItemDetail = exports.ItemDetail || (exports.ItemDetail = {}));
    function navigateToDialog(viewport, configuration) {
        var dialog = new DialogView(viewport, configuration);
        viewport.setView(dialog);
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
        function DialogView(viewport, configuration) {
            var _this = _super.call(this, viewport) || this;
            _this.configuration = configuration;
            return _this;
        }
        DialogView.prototype.refresh = function () {
            var tthis = this;
            var tableConfiguration = new DMTables.DetailTableConfiguration();
            tableConfiguration.fields = DMTables.convertFieldDataToFields(this.configuration.columns);
            this.detailTable = new DMTables.DetailTableComposer(tableConfiguration, this.content);
            if (this.configuration.onOkForm !== undefined && this.configuration.onOkForm !== null) {
                this.detailTable.onClickOk = function (newItem) {
                    tthis.configuration.onOkForm(newItem);
                };
            }
            if (this.configuration.onCancelForm !== undefined && this.configuration.onCancelForm !== null) {
                this.detailTable.onClickCancel = function () {
                    tthis.configuration.onCancelForm();
                    return false;
                };
            }
            else {
                this.detailTable.onClickCancel = function () {
                    tthis.viewport.navigateBack();
                    return false;
                };
            }
            this.detailTable.composeTable();
        };
        DialogView.prototype.onViewShown = function () {
            this.detailTable.setFocusOnFirstRow();
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
            // Adds the default one, which creates just an empty, non-defined item.
            _this.addButtonLink("Unspecified", function () {
                DMClient.ExtentApi.createItem(ws, extentUrl, null)
                    .done(function (innerData) {
                    ItemDetail.navigateToItem(tthis.viewport, ws, extentUrl, innerData.newuri);
                });
            });
            _this.addText("Loading...");
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