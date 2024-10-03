﻿import * as InterfacesForms from "./Interfaces.js";
import * as Mof from "../Mof.js";
import {createField} from "./FieldFactory.js";
import * as Settings from "../Settings.js";
import {IFormConfiguration} from "./IFormConfiguration.js";
import * as Navigator from '../Navigator.js'
import {_DatenMeister} from "../models/DatenMeister.class.js";
import _TableForm = _DatenMeister._Forms._TableForm;
import * as Actions from "../client/Actions.js";
import _FieldData = _DatenMeister._Forms._FieldData;

interface PropertyMenuItem
{
    title: string;
    onClick?: () => (void);
    requireConfirmation?: boolean;
}

export class TableFormParameter {
    shortenFullText: boolean = true;
    allowSortingOfColumn: boolean = true;

    /**
     * MetaClass that is used to filter only upon the items having that specific metaclass
     */
    metaClass: string; 
}

class TableState {
    orderBy: string = undefined;
    orderByDescending: boolean = false;
}

class TableJQueryCaches {

    cacheHeadline: JQuery;

    cacheTable: JQuery;

    cacheEmptyDiv: JQuery;

    cacheButtons: JQuery;

    parentHtml: JQuery<HTMLElement>;

}
export class TableForm implements InterfacesForms.ICollectionFormElement, InterfacesForms.IObjectFormElement {

    /**
     * Caches the elements to allow direct manipulation without additional server round-trip
     */
    elements: Array<Mof.DmObject>;

    /**
     * To be set, if only the element's of a property shall be shown
     * the type of the element is retrieved by the tab form
     */
    element: Mof.DmObject;
    extentUri: string;
    formElement: Mof.DmObject;
    itemUrl: string;
    workspace: string;    

    configuration: IFormConfiguration;
    tableParameter: TableFormParameter = new TableFormParameter();
    tableState: TableState = new TableState();
    tableCache: TableJQueryCaches = new TableJQueryCaches();

    pageNavigation: InterfacesForms.IPageNavigation;

    callbackLoadItems: (query: InterfacesForms.QueryFilterParameter) => Promise<Array<Mof.DmObject>>;

    /**
     * Refreshes the complete form including the parent item which might contain multiple tables
     */
    async refreshForm(): Promise<void> {

        if (this.configuration.refreshForm !== undefined) {
            this.configuration.refreshForm();
        } else {
            await this.createFormByCollection(this.tableCache.parentHtml, this.configuration, true);
        }
    }

    /**
     * Just refreshes the form and performs a reloading of items from the server
     * The query parameters are taken into consideration
     */
    async reloadTable(): Promise<void> {

        await this.createFormByCollection(this.tableCache.parentHtml, this.configuration, true);
    }

    /**
     * This method just calls the createFormByCollection since a TableForm can 
     * show the extent's elements directly or just the properties of an elemnet
     * @param parent The Html to which the table shall be added
     * @param configuration The Configuration for the table
     * @param refresh true, if we just would like to refresh the table and not create new elements
     */
    async createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration, refresh?: boolean)
    {        
        return await this.createFormByCollection(parent, configuration, refresh);
    }

    async createFormByCollection(parent: JQuery<HTMLElement>, configuration: IFormConfiguration, refresh?: boolean) {
        this.tableCache.parentHtml = parent;
        this.configuration = configuration;

        this.tableParameter.metaClass = (this.formElement.get('metaClass') as Mof.DmObject)?.uri;
        const tthis = this;

        if (this.configuration.isReadOnly === undefined) {
            this.configuration.isReadOnly = true;
        }

        if (this.callbackLoadItems === undefined) {
            throw "No callbackLoadItems is set";
        }

        // Loads the data
        const query = new InterfacesForms.QueryFilterParameter();
        query.orderBy = this.tableState.orderBy;
        query.orderByDescending = this.tableState.orderByDescending;

        this.elements = await this.callbackLoadItems(query);

        // Creates the headlines
        this.tableCache.cacheHeadline =
            refresh === true && this.tableCache.cacheHeadline !== undefined
                ? this.tableCache.cacheHeadline
                : $("<h2><a></a></h2>");

        const headLineLink = $("a", this.tableCache.cacheHeadline);
        headLineLink.text(
            this.formElement.get('title')
            ?? this.formElement.get('name'));

        headLineLink.attr(
            'href',
            Navigator.getLinkForNavigateToExtentItems(this.workspace, this.extentUri, { metaClass: this.tableParameter.metaClass}));

        if (refresh !== true) {
            parent.append(this.tableCache.cacheHeadline);
        }

        const property = this.formElement.get('property');

        this.tableCache.cacheButtons =
            refresh === true && this.tableCache.cacheHeadline !== undefined
            ? this.tableCache.cacheButtons
                : $("<div></div>");
        this.tableCache.cacheButtons.empty();
        if (refresh !== true) {
            parent.append(this.tableCache.cacheButtons);
        }

        // Evaluate the new buttons to create objects
        const defaultTypesForNewElements = this.formElement.getAsArray("defaultTypesForNewElements");
        if (defaultTypesForNewElements !== undefined) {
            for (let n in defaultTypesForNewElements) {
                const inner = defaultTypesForNewElements[n] as Mof.DmObject;
                (function (innerValue) {

                    const btn = $("<btn class='btn btn-secondary'></btn>");
                    btn.text("Create " + inner.get('name'));
                    btn.on('click', () => {
                        const uri = innerValue.get('metaClass').uri;
                        if (property === undefined) {
                            document.location.href =
                                Settings.baseUrl +
                                "ItemAction/Extent.CreateItem?workspace=" +
                                encodeURIComponent(tthis.workspace) +
                                "&extent=" +
                                encodeURIComponent(tthis.extentUri) +
                                "&metaclass=" +
                                encodeURIComponent(uri);
                        } else {
                            document.location.href =
                                Settings.baseUrl +
                                "ItemAction/Extent.CreateItemInProperty?workspace=" +
                                encodeURIComponent(tthis.workspace) +
                                "&itemUrl=" +
                                encodeURIComponent(tthis.itemUrl) +
                                "&metaclass=" +
                                encodeURIComponent(uri) +
                                "&property=" +
                                encodeURIComponent(property);
                        }
                    });

                    tthis.tableCache.cacheButtons.append(btn);
                })(inner);

            }
        }

        // Creates the table 
        if (this.elements === undefined) {
            this.elements = [];
        }
                
        if (!Array.isArray(this.elements)) {
            // Creates an empty table in case a non-array was given
            this.tableCache.cacheEmptyDiv =
                refresh === true && this.tableCache.cacheTable !== undefined
                ? this.tableCache.cacheTable
                    : $("<div></div>");
            this.tableCache.cacheEmptyDiv.empty();
            this.tableCache.cacheEmptyDiv.text("Non-Array elements for ListForm: ");
            this.tableCache.cacheEmptyDiv.append($("<em></em>").text((this.elements as any).toString()));

            if (refresh !== true) {
                parent.append(this.tableCache.cacheEmptyDiv);
            }
        } else {
            // Creates the the table
            this.tableCache.cacheTable =
                refresh === true && this.tableCache.cacheTable !== undefined
                ? this.tableCache.cacheTable
                    : $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top dm-tableform'></table>");

            await this.createTable();

            if (refresh !== true) {
                parent.append(this.tableCache.cacheTable);
            }
        }
    }

    /**
    * Creates the table itself that shall be shown
    */
    private async createTable() {
        const tthis = this;
        this.tableCache.cacheTable.empty();

        const fields = this.formElement.getAsArray("field");

        const headerRow = $("<tbody><tr></tr></tbody>");
        const innerRow = $("tr", headerRow);

        // Create the column headlines
        for (let n in fields) {
            if (!fields.hasOwnProperty(n)) continue;
            const field = fields[n] as Mof.DmObject;
            const fieldName = field.get(_FieldData._name_);

            // Create the column
            let cell = $("<th></th>");

            // Create the text of the headline
            cell.text(field.get(_FieldData.title) ?? field.get(_FieldData._name_));

            if (this.tableParameter.allowSortingOfColumn) {
                const isSorted = this.tableState.orderBy === fieldName;
                const isSortedDescending = this.tableState.orderByDescending;

                let sortingArrow: JQuery;
                let onClick: () => void;
                if (isSorted) {
                    if (!isSortedDescending) {
                        sortingArrow = $('<span class="dm-tableform-sortbutton">↓</span>');
                        onClick = async () => {
                            tthis.tableState.orderBy = fieldName;
                            tthis.tableState.orderByDescending = true;
                            await tthis.reloadTable();
                        };
                    }
                    else {
                        sortingArrow = $('<span class="dm-tableform-sortbutton">↑</span>');
                        onClick = async () => {
                            tthis.tableState.orderBy = fieldName;
                            tthis.tableState.orderByDescending = false;
                            await tthis.reloadTable();

                        };
                    }
                }
                else {
                    sortingArrow = $('<span class="dm-tableform-sortbutton">⇅</span>');

                    onClick = async () => {
                        tthis.tableState.orderBy = fieldName;
                        tthis.tableState.orderByDescending = false;
                        await tthis.reloadTable();
                    };
                }

                sortingArrow.on('click', onClick);
                cell.append(sortingArrow);
            }

            // Create the column menu
            await this.appendColumnMenus(field, cell);

            innerRow.append(cell);
        }

        this.tableCache.cacheTable.append(headerRow);

        let noItemsWithMetaClass = this.formElement.get('noItemsWithMetaClass');

        for (let n in this.elements) {
            if (Object.prototype.hasOwnProperty.call(this.elements, n)) {
                let element = this.elements[n];

                // Check, if the element may be shown
                let elementsMetaClass = element.metaClass?.uri;
                if ((elementsMetaClass !== undefined && elementsMetaClass !== "") && noItemsWithMetaClass) {
                    // The Element has a metaclass, but we don't want to see them
                    continue;
                }

                if ((this.tableParameter.metaClass !== undefined && this.tableParameter.metaClass !== "") && elementsMetaClass !== this.tableParameter.metaClass) {
                    // The element has no metaclass or wrong metaclass but we have a specific metaclass to be queried
                    continue;
                }

                const row = $("<tr></tr>");

                for (let n in fields) {
                    if (!fields.hasOwnProperty(n)) continue;
                    const field = fields[n] as Mof.DmObject;
                    let cell = $("<td></td>");

                    const fieldMetaClassUri = field.metaClass.uri;
                    const fieldElement = createField(
                        fieldMetaClassUri,
                        {
                            configuration: this.configuration,
                            field: field,
                            itemUrl: element.uri,
                            isReadOnly: this.configuration.isReadOnly,
                            form: this
                        });

                    let dom;
                    if (fieldElement === undefined) {
                        dom = $("<span></span>");
                        dom.text("Field for " + field.get("name", Mof.ObjectType.String) + " not found");
                    } else {
                        dom = await fieldElement.createDom(element);
                    }

                    cell.append(dom);
                    row.append(cell);
                }

                this.tableCache.cacheTable.append(row);
            }
        }
    }

    private async appendColumnMenus(field: Mof.DmObject, cell: JQuery<HTMLElement>) {
        const column = await this.createPropertyMenuItems(field);
        if (column.length > 0) {
            let contextItem = $("<div class='dm-contextmenu'><div class='dm-contextmenu-dots'>...</div><div class='dm-contextmenu-item-container'></div></div>");
            const htmlContainer = $(".dm-contextmenu-item-container", contextItem);
            for (const m in column) {
                const menuProperty = column[m];
                const htmlItem = $("<div class='dm-contextmenu-item'></div>");
                htmlItem.text(menuProperty.title);
                if (menuProperty.onClick !== undefined) {
                    htmlItem.on('click',
                        () => {
                            if (menuProperty.requireConfirmation !== true
                                || htmlItem.attr('data-require') === '1') {
                                menuProperty.onClick();
                            }
                            else {
                                htmlItem.attr('data-require', '1');
                                htmlItem.text('Confirm?');
                            }
                        });
                }

                htmlContainer.append(htmlItem);
            }
            $(".dm-contextmenu-dots", contextItem).on('click', () => {

                if (htmlContainer.attr('data-display') === 'visible') {
                    htmlContainer.hide();
                    htmlContainer.attr('data-display', '');
                }
                else {
                    htmlContainer.show();
                    htmlContainer.attr('data-display', 'visible');
                }
            });


            cell.append(contextItem);
        }
    }

    async createPropertyMenuItems(field: Mof.DmObject): Promise<PropertyMenuItem[]> {

        let result = [];
        const propertyName = field.get(_FieldData._name_, Mof.ObjectType.String);

        if (field.metaClass.uri !== _DatenMeister._Forms.__ActionFieldData_Uri
            && field.metaClass.uri !== _DatenMeister._Forms.__MetaClassElementFieldData_Uri) {
            const menuRemoveProperty =
                {
                    title: "Clear Property",
                    requireConfirmation: true,
                    onClick: async () => {

                        // Gets the data
                        const propertyName = field.get(_FieldData._name_, Mof.ObjectType.String);
                        const dataUrl = this.formElement.get(_TableForm.dataUrl, Mof.ObjectType.String);

                        // Creates the action
                        const action =
                            new Mof.DmObject(_DatenMeister._Actions.__DeletePropertyFromCollectionAction_Uri);
                        action.set(_DatenMeister._Actions._DeletePropertyFromCollectionAction.collectionUrl, dataUrl);
                        action.set(_DatenMeister._Actions._DeletePropertyFromCollectionAction.propertyName, propertyName);

                        const parameter: Actions.ExecuteActionParams =
                            {
                                parameter: action
                            };

                        await Actions.executeActionDirectly("Execute", parameter);
                        await this.refreshForm();
                    }
                };

            result.push(menuRemoveProperty);
        }
        
        return result;
    }
}