

import * as DMClient from "./datenmeister-client";
import * as DMCI from "./datenmeister-clientinterface";

export class Toolbar {
    domToolbar: JQuery;
    items: Array<IToolbarItem>;
    constructor(container: JQuery) {
        this.domToolbar = $("<div class='dm-toolbar row'></div>");
        container.append(this.domToolbar);
        this.items = new Array<IToolbarItem>();
    }

    addItem(item: IToolbarItem) {
        this.items[item.getId()] = item;
        this.domToolbar.append(item.getDom());
    }
}

export interface IToolbarItem {
    getDom(): JQuery;

    /**
     * Gets a unique identification
     * @returns {The unique identification} 
     */
    getId(): string;
}

export class ToolbarItemBase implements IToolbarItem {

    protected domContent: JQuery;
    protected id: string;

    constructor(id: string) {
        this.id = id;
    }

    create(cols: number, text?: string): JQuery {
        if (text === undefined || text === null) {
            text = "";
        }

        this.domContent = $(`<div class='col-md-${cols}'>${text}</div>`);
        return this.domContent;
    }

    getDom(): JQuery {
        return this.domContent;
    }

    getId(): string {
        return this.id;
    }
}

export class ToolbarViewSelection extends ToolbarItemBase {
    
    private ws: string;
    private extentUrl: string;
    private itemUrl: string;

    onViewChanged: (typeUrl?: string) => void;

    constructor(ws: string, extentUrl: string, itemUrl?: string) {
        super("view");
        
        this.ws = ws;
        this.extentUrl = extentUrl;
        this.itemUrl = itemUrl;

        var tthis = this;
        super.create(2, "Loading views");

        DMClient.ExtentApi.getViews(this.ws, this.extentUrl)
            .done(
            (data) => {
                tthis.updateLayoutForViews(data.views);
            });
    }

    updateLayoutForViews(views: Array<DMCI.In.IItemModel>) {
        var tthis = this;
        if (this.domContent !== null && this.domContent !== undefined) {
            var data = views;
            this.domContent.empty();
            var domDropDown = $("<select class='form-control'><option value='{None}'>Switch to view...</option><option value='{All}'>All properties</option></select>");
            for (var n in data) {
                var type = data[n];
                var domOption = $(`<option value='${type.uri}'></option>`);
                domOption.text(type.name);
                domDropDown.append(domOption);
            }

            domDropDown.change(() => {
                // User clicked on a new view
                var value = domDropDown.val();
                if (value !== "{None}") {
                    tthis.onViewChanged(domDropDown.val());
                }
            });

            this.domContent.append(domDropDown);
        }
    }
}

export class ToolBarButtonItem extends ToolbarItemBase {
    onClicked: () => void;

    constructor(id: string, text: string) {
        super(id);
        var tthis = this;
        super.create(3);
        var domNewItem = $(`<a href='#' class='btn btn-default'>${text}</a>`);
        domNewItem.click(() => {
            if (tthis.onClicked !== undefined) {
                tthis.onClicked();
            }

            return false;
        });

        this.domContent.append(domNewItem);
    }
}

export class ToolbarSearchbox extends ToolbarItemBase {

    onSearch: (searchText: string) => void;

    constructor() {
        super("searchbox");

        var tthis = this;
        var result = super.create(3);
        var domSearchBox = $("<input type='textbox' class='form-control' placeholder='Search...' />");
        domSearchBox.keyup(() => {
            var searchText = domSearchBox.val();

            if (tthis.onSearch !== undefined) {
                tthis.onSearch(searchText);
            }
        });

        result.append(domSearchBox);
    }
}

export class ToolbarCreateableTypes extends ToolbarItemBase {
    onNewItemClicked: (type?: string) => void;
    createableTypes: Array<DMCI.In.IItemModel>;

    ws: string;
    extentUrl: string;

    constructor(ws: string, extentUrl: string) {
        super("createabletypes");
        var tthis = this;
        this.extentUrl = extentUrl;
        this.ws = ws;
        super.create(3);
        DMClient.ExtentApi.getCreatableTypes(ws, extentUrl).done((data) => {
            tthis.createableTypes = data.types;
            tthis.updateLayoutForCreatableTypes();
        });
    }

    updateLayoutForCreatableTypes() {

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

            domDropDown.change(() => {
                var value = domDropDown.val();
                if (value === "{None}" || value === undefined) {
                    return;
                }

                tthis.onNewItemClicked(domDropDown.val());
            });

            this.domContent.append(domDropDown);
        }
    }
}

export class ToolbarMetaClasses extends ToolbarItemBase {
    onItemClicked: (type?: string) => void;
    metaClasses: Array<DMCI.In.IItemModel>;

    ws: string;
    extentUrl: string;

    constructor(ws: string, extentUrl: string) {
        super("metaclasses");
        this.extentUrl = extentUrl;
        this.ws = ws;
        super.create(2);
    }

    updateLayout(metaClasses: Array<DMCI.In.IItemModel>) {
        var tthis = this;
        this.metaClasses = metaClasses;
        if (this.metaClasses !== null && this.metaClasses !== undefined) {
            var data = this.metaClasses;
            this.domContent.empty();
            var domDropDown = $("<select class='form-control'><option value='{All}'>All MetaClasses</option></select>");
            for (var n in data) {
                var type = data[n];
                var domOption = $("<option value='" + type.uri + "'></option>");
                domOption.text(type.name);

                domDropDown.append(domOption);
            }

            domDropDown.change(() => {
                var value = domDropDown.val();
                if (value === "{None}" || value === undefined) {
                    return;
                }

                tthis.onItemClicked(domDropDown.val());
            });

            this.domContent.append(domDropDown);
        }
    }
}

export class ToolbarPaging extends ToolbarItemBase {

    currentPage: number;
    totalPages: number;

    onPageChange: (newPage: number) => void;

    constructor() {
        super("paging");
        this.totalPages = 0;
        this.currentPage = 1;
        var result = super.create(3);
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

        result.append(domPaging);
    }

    throwOnPageChange(): void {
        if (this.onPageChange !== undefined) {
            this.onPageChange(this.currentPage);
        }
    }

    setTotalPages(pages: number): void {
        this.totalPages = pages;
        this.currentPage = Math.min(pages, this.currentPage);

        var domTotalPages = $(".dm_totalpages", this.domContent);
        domTotalPages.text(this.totalPages);
    }
}