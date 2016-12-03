var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "./datenmeister-client"], function (require, exports, DMClient) {
    "use strict";
    var Toolbar = (function () {
        function Toolbar(container) {
            this.domToolbar = $("<div class='dm-toolbar row'></div>");
            container.append(this.domToolbar);
            this.items = new Array();
        }
        Toolbar.prototype.addItem = function (item) {
            this.items[item.getId()] = item;
            this.domToolbar.append(item.getDom());
        };
        return Toolbar;
    }());
    exports.Toolbar = Toolbar;
    var ToolbarItemBase = (function () {
        function ToolbarItemBase(id) {
            this.id = id;
        }
        ToolbarItemBase.prototype.create = function (cols, text) {
            if (text === undefined || text === null) {
                text = "";
            }
            this.domContent = $("<div class='col-md-" + cols + "'>" + text + "</div>");
            return this.domContent;
        };
        ToolbarItemBase.prototype.getDom = function () {
            return this.domContent;
        };
        ToolbarItemBase.prototype.getId = function () {
            return this.id;
        };
        return ToolbarItemBase;
    }());
    exports.ToolbarItemBase = ToolbarItemBase;
    var ToolbarViewSelection = (function (_super) {
        __extends(ToolbarViewSelection, _super);
        function ToolbarViewSelection(ws, extentUrl) {
            _super.call(this, "view");
            this.extentUrl = extentUrl;
            this.ws = ws;
            var tthis = this;
            _super.prototype.create.call(this, 2, "Loading views");
            DMClient.ExtentApi.getViews(this.ws, this.extentUrl)
                .done(function (data) {
                tthis.updateLayoutForViews(data.views);
            });
        }
        ToolbarViewSelection.prototype.updateLayoutForViews = function (views) {
            var tthis = this;
            if (this.domContent !== null && this.domContent !== undefined) {
                var data = views;
                this.domContent.empty();
                var domDropDown = $("<select class='form-control'><option value='{None}'>Switch to view...</option><option value='{All}'>All properties</option></select>");
                for (var n in data) {
                    var type = data[n];
                    var domOption = $("<option value='" + type.uri + "'></option>");
                    domOption.text(type.name);
                    domDropDown.append(domOption);
                }
                domDropDown.change(function () {
                    // User clicked on a new view
                    var value = domDropDown.val();
                    if (value !== "{None}") {
                        tthis.onViewChanged(domDropDown.val());
                    }
                });
                this.domContent.append(domDropDown);
            }
        };
        return ToolbarViewSelection;
    }(ToolbarItemBase));
    exports.ToolbarViewSelection = ToolbarViewSelection;
    var ToolBarNewItem = (function (_super) {
        __extends(ToolBarNewItem, _super);
        function ToolBarNewItem() {
            _super.call(this, "newitem");
        }
        ToolBarNewItem.prototype.create = function () {
            var tthis = this;
            var result = _super.prototype.create.call(this, 2);
            var domNewItem = $("<div class='col-md-2'><a href='#' class='btn btn-default'>Create new item</a></div>");
            $("a", domNewItem).click(function () {
                if (tthis.onNewItemClicked !== undefined) {
                    tthis.onNewItemClicked();
                }
                return false;
            });
            return result;
        };
        return ToolBarNewItem;
    }(ToolbarItemBase));
    exports.ToolBarNewItem = ToolBarNewItem;
    var ToolbarSearchbox = (function (_super) {
        __extends(ToolbarSearchbox, _super);
        function ToolbarSearchbox() {
            _super.call(this, "searchbox");
            var tthis = this;
            var result = _super.prototype.create.call(this, 5);
            var domSearchBox = $("<input type='textbox' class='form-control' placeholder='Search...' />");
            domSearchBox.keyup(function () {
                var searchText = domSearchBox.val();
                if (tthis.onSearch !== undefined) {
                    tthis.onSearch(searchText);
                }
            });
            result.append(domSearchBox);
        }
        return ToolbarSearchbox;
    }(ToolbarItemBase));
    exports.ToolbarSearchbox = ToolbarSearchbox;
    var ToolbarCreateableTypes = (function (_super) {
        __extends(ToolbarCreateableTypes, _super);
        function ToolbarCreateableTypes(ws, extentUrl) {
            _super.call(this, "createabletypes");
            var tthis = this;
            this.extentUrl = extentUrl;
            this.ws = ws;
            var result = _super.prototype.create.call(this, 5);
            DMClient.ExtentApi.getCreatableTypes(ws, extentUrl).done(function (data) {
                tthis.createableTypes = data.types;
                tthis.updateLayoutForCreatableTypes();
            });
        }
        ToolbarCreateableTypes.prototype.updateLayoutForCreatableTypes = function () {
            var tthis = this;
            if (this.createableTypes !== null && this.createableTypes !== undefined) {
                var data = this.createableTypes;
                this.domContent.empty();
                var domDropDown = $("<select class='form-control'><option value='{None}'>Create Type...</option><option value=''>Unspecified</option></select>");
                for (var n in data) {
                    var type = data[n];
                    var domOption = $("<option value='" + type.uri + "'></option>");
                    domOption.text(type.name);
                    domDropDown.append(domOption);
                }
                domDropDown.change(function () {
                    var value = domDropDown.val();
                    if (value === "{None}" || value === undefined) {
                        return;
                    }
                    tthis.onNewItemClicked(domDropDown.val());
                });
                this.domContent.append(domDropDown);
            }
        };
        return ToolbarCreateableTypes;
    }(ToolbarItemBase));
    exports.ToolbarCreateableTypes = ToolbarCreateableTypes;
    var ToolbarPaging = (function (_super) {
        __extends(ToolbarPaging, _super);
        function ToolbarPaging() {
            _super.call(this, "paging");
            this.totalPages = 0;
            this.currentPage = 1;
            var result = _super.prototype.create.call(this, 5);
            var tthis = this;
            var domPaging = $("<div class='form-inline'>" +
                "<a href='#' class='dm-prevpage btn btn-default'>&lt;&lt;</a> Page " +
                "<input class='form-control dm-page-selected' type='textbox' value='1'/> of " +
                "<span class='dm_totalpages'> </span> " +
                "<a href='#' class='dm-jumppage btn btn-default'>GO</a>&nbsp;" +
                "<a href='#' class='dm-nextpage btn btn-default'>&gt;&gt;</a></div>");
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
            result.append(domPaging);
        }
        ToolbarPaging.prototype.throwOnPageChange = function () {
            if (this.onPageChange !== undefined) {
                this.onPageChange(this.currentPage);
            }
        };
        ToolbarPaging.prototype.setTotalPages = function (pages) {
            this.totalPages = pages;
            this.currentPage = Math.min(pages, this.currentPage);
            var domTotalPages = $(".dm_totalpages", this.domContent);
            domTotalPages.text(this.totalPages);
        };
        return ToolbarPaging;
    }(ToolbarItemBase));
    exports.ToolbarPaging = ToolbarPaging;
});
//# sourceMappingURL=datenmeister-toolbar.js.map