import * as InterfacesForms from "./Interfaces.js";
import * as Mof from "../Mof.js";
import * as FieldFactory from "./FieldFactory.js";
import * as Settings from "../Settings.js";
import * as Navigator from '../Navigator.js';
import { _DatenMeister } from "../models/DatenMeister.class.js";
var _TableForm = _DatenMeister._Forms._TableForm;
import * as Actions from "../client/Actions.js";
var _FieldData = _DatenMeister._Forms._FieldData;
import * as burnJsPopup from "../../burnJsPopup.js";
export class TableFormParameter {
    constructor() {
        this.shortenFullText = true;
        this.allowSortingOfColumn = true;
        this.allowFreeTextFiltering = true;
    }
}
class TableState {
    constructor() {
        this.orderBy = undefined;
        this.orderByDescending = false;
        this.freeTextFilter = "";
    }
}
class TableJQueryCaches {
}
export class TableForm {
    constructor() {
        this.tableParameter = new TableFormParameter();
        this.tableState = new TableState();
        this.tableCache = new TableJQueryCaches();
    }
    /**
     * Refreshes the complete form including the parent item which might contain multiple tables
     */
    async refreshForm() {
        if (this.configuration.refreshForm !== undefined) {
            this.configuration.refreshForm();
        }
        else {
            await this.createFormByCollection(this.tableCache.parentHtml, this.configuration, true);
        }
    }
    /**
     * Just refreshes the form and performs a reloading of items from the server
     * The query parameters are taken into consideration
     */
    async reloadTable() {
        await this.createFormByCollection(this.tableCache.parentHtml, this.configuration, true);
    }
    async refreshTable() {
        this.createTable();
    }
    /**
     * This method just calls the createFormByCollection since a TableForm can
     * show the extent's elements directly or just the properties of an elemnet
     * @param parent The Html to which the table shall be added
     * @param configuration The Configuration for the table
     * @param refresh true, if we just would like to refresh the table and not create new elements
     */
    async createFormByObject(parent, configuration, refresh) {
        return await this.createFormByCollection(parent, configuration, refresh);
    }
    async createFormByCollection(parent, configuration, refresh) {
        this.tableCache.parentHtml = parent;
        this.configuration = configuration;
        this.tableParameter.metaClass = this.formElement.get('metaClass')?.uri;
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
        headLineLink.text(this.formElement.get('title')
            ?? this.formElement.get('name'));
        headLineLink.attr('href', Navigator.getLinkForNavigateToExtentItems(this.workspace, this.extentUri, { metaClass: this.tableParameter.metaClass }));
        if (refresh !== true) {
            parent.append(this.tableCache.cacheHeadline);
        }
        this.tableCache.cacheFreeTextField =
            refresh === true && this.tableCache.cacheFreeTextField !== undefined ?
                this.tableCache.cacheFreeTextField
                : $("<div class='dm-tableform-freetextform'></div>");
        this.tableCache.cacheFreeTextField.empty();
        if (refresh !== true) {
            parent.append(this.tableCache.cacheFreeTextField);
        }
        this.tableCache.cacheButtons =
            refresh === true && this.tableCache.cacheButtons !== undefined ?
                this.tableCache.cacheButtons
                : $("<div></div>");
        this.tableCache.cacheButtons.empty();
        if (refresh !== true) {
            parent.append(this.tableCache.cacheButtons);
        }
        // Evaluate the new buttons to create objects
        this.createButtonsForNewInstance();
        if (this.tableParameter.allowFreeTextFiltering) {
            // Create freetext
            this.createFreeTextField();
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
            this.tableCache.cacheEmptyDiv.append($("<em></em>").text(this.elements.toString()));
            if (refresh !== true) {
                parent.append(this.tableCache.cacheEmptyDiv);
            }
        }
        else {
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
    createButtonsForNewInstance() {
        const property = this.formElement.get('property');
        const tthis = this;
        const defaultTypesForNewElements = this.formElement.getAsArray("defaultTypesForNewElements");
        if (defaultTypesForNewElements !== undefined) {
            for (let n in defaultTypesForNewElements) {
                const inner = defaultTypesForNewElements[n];
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
                        }
                        else {
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
    }
    /**
     * Creates the free text field for filtering
     */
    createFreeTextField() {
        const tthis = this;
        const inputField = $('<input type="text" placeholder="Filter by Text"></input>');
        inputField.on('input', () => {
            tthis.tableState.freeTextFilter = inputField.val().toString();
            tthis.refreshTable();
        });
        this.tableCache.cacheFreeTextField.append(inputField);
        this.refreshTable();
    }
    /**
    * Creates the table itself that shall be shown
    */
    async createTable() {
        const tthis = this;
        if (this.tableCache?.cacheTable === undefined)
            return;
        this.tableCache.cacheTable.empty();
        const fields = this.formElement.getAsArray("field");
        const headerRow = $("<tbody><tr></tr></tbody>");
        const innerRow = $("tr", headerRow);
        // Create the column headlines
        for (let n in fields) {
            if (!fields.hasOwnProperty(n))
                continue;
            const field = fields[n];
            // Create the column
            let cell = $("<th></th>");
            const innerTable = $("<table class='dm-tablerow-columnheader'><tr><td class='dm-tablerow-columntitle'></td><td class='dm-tablerow-columnbuttons'></td></tr></table>");
            cell.append(innerTable);
            const titleCell = $(".dm-tablerow-columntitle", innerTable);
            const titleButtons = $(".dm-tablerow-columnbuttons", innerTable);
            // Create the text of the headline
            titleCell.text(field.get(_FieldData.title) ?? field.get(_FieldData._name_));
            if (this.tableParameter.allowSortingOfColumn) {
                this.createSortingButton(field, titleButtons);
            }
            // Create the column menu
            await this.appendColumnMenus(field, titleButtons);
            innerRow.append(cell);
        }
        this.tableCache.cacheTable.append(headerRow);
        let noItemsWithMetaClass = this.formElement.get('noItemsWithMetaClass');
        for (let n in this.elements) {
            if (Object.prototype.hasOwnProperty.call(this.elements, n)) {
                let element = this.elements[n];
                // Creates the row
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
                // If we have freetext, then we need to skip the row.
                if (this.tableParameter.allowFreeTextFiltering) {
                    let found = false;
                    for (let m in fields) {
                        if (!fields.hasOwnProperty(m))
                            continue;
                        const field = fields[m];
                        // Check if the field can be text filtered
                        const canBeTextFiltered = FieldFactory.canBeTextFiltered(field);
                        if (!canBeTextFiltered) {
                            continue;
                        }
                        const fieldName = field.get(_FieldData._name_);
                        const fieldValue = element.get(fieldName, Mof.ObjectType.String);
                        if (fieldValue !== undefined && fieldValue.toString().toLowerCase().indexOf(this.tableState.freeTextFilter.toLowerCase()) >= 0) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        continue;
                    }
                }
                const row = $("<tr></tr>");
                for (let n in fields) {
                    if (!fields.hasOwnProperty(n))
                        continue;
                    const field = fields[n];
                    let cell = $("<td></td>");
                    const fieldMetaClassUri = field.metaClass.uri;
                    const fieldElement = FieldFactory.createField(fieldMetaClassUri, {
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
                    }
                    else {
                        dom = await fieldElement.createDom(element);
                    }
                    cell.append(dom);
                    row.append(cell);
                }
                this.tableCache.cacheTable.append(row);
            }
        }
    }
    createSortingButton(field, titleButtons) {
        const tthis = this;
        const fieldCanBeSorted = FieldFactory.canBeSorted(field);
        if (fieldCanBeSorted) {
            const fieldName = field.get(_FieldData._name_);
            const isSorted = this.tableState.orderBy === fieldName;
            const isSortedDescending = this.tableState.orderByDescending;
            let sortingArrow;
            let onClick;
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
            titleButtons.append(sortingArrow);
        }
    }
    async appendColumnMenus(field, cell) {
        const propertyMenuItems = await this.createPropertyMenuItems(field);
        if (propertyMenuItems.length === 0) {
            return;
        }
        // Now create the button
        let contextItem = $("<div class='dm-tableform-sortbutton'>...</div>");
        cell.append(contextItem);
        contextItem.on('click', () => {
            const popup = burnJsPopup.createPopup();
            for (var n in propertyMenuItems) {
                const div = $("<div></div>");
                propertyMenuItems[n].onCreateDom(popup, div);
                $(popup.htmlContent).append(div);
            }
        });
    }
    async createPropertyMenuItems(field) {
        let result = [];
        const propertyName = field.get(_FieldData._name_, Mof.ObjectType.String);
        if (field.metaClass.uri !== _DatenMeister._Forms.__ActionFieldData_Uri
            && field.metaClass.uri !== _DatenMeister._Forms.__MetaClassElementFieldData_Uri) {
            const menuRemoveProperty = {
                onCreateDom: (popup, jquery) => {
                    const button = $("<button class='btn btn-secondary' type='button'>Clear Properties</button>");
                    button.on('click', async () => {
                        // Gets the data
                        const propertyName = field.get(_FieldData._name_, Mof.ObjectType.String);
                        const dataUrl = this.formElement.get(_TableForm.dataUrl, Mof.ObjectType.String);
                        // Creates the action
                        const action = new Mof.DmObject(_DatenMeister._Actions.__DeletePropertyFromCollectionAction_Uri);
                        action.set(_DatenMeister._Actions._DeletePropertyFromCollectionAction.collectionUrl, dataUrl);
                        action.set(_DatenMeister._Actions._DeletePropertyFromCollectionAction.propertyName, propertyName);
                        const parameter = {
                            parameter: action
                        };
                        await Actions.executeActionDirectly("Execute", parameter);
                        await this.refreshForm();
                    });
                    jquery.append(button);
                },
                requireConfirmation: true
            };
            result.push(menuRemoveProperty);
        }
        return result;
    }
}
//# sourceMappingURL=TableForm.js.map