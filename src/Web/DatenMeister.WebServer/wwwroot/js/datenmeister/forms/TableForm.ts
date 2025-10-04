import * as InterfacesForms from "./Interfaces.js";
import * as SIC from "../controls/SelectItemControl.js";
import * as Mof from "../Mof.js";
import * as FieldFactory from "./FieldFactory.js";
import {IFormConfiguration} from "./IFormConfiguration.js";
import * as Navigator from '../Navigator.js'
import * as _DatenMeister from "../models/DatenMeister.class.js";
import _FieldData = _DatenMeister._Forms._FieldData;
import * as burnJsPopup from "../../burnJsPopup.js"
import * as ClientItem from "../client/Items.js"
import * as ContextMenu from "./TableForm.ContextMenus.js"

interface PropertyMenuItem
{
    cellKeyTitle?: string;
    onCreateDom: (popupResult: burnJsPopup.PopupResult, jQuery: JQuery) => void;
    onSubmitForm?: () => void;

    /**
     * Allows a menuitem to change the button text
     * @returns The changed buttontext
     */
    callbackButtonText?: (query: JQuery) => boolean;
}

export class TableFormParameter {
    shortenFullText: boolean = true;
    allowSortingOfColumn: boolean = true;
    allowFilteringOnProperty: boolean = true;
    allowFreeTextFiltering: boolean = true;
    showFilterQuery: boolean = true;

    /**
     * MetaClass that is used to filter only upon the items having that specific metaclass
     */
    metaClass: string; 
}

class TableState {
    orderBy: string = undefined;
    orderByDescending: boolean = false;
    freeTextFilter: string = "";
    filterByProperty: Array<string> = new Array<string>();
}

class TableJQueryCaches {
    /* 
    Stores the overall div which hosts the full Table Form including all Buttons, Headline, Table, etc.
     */
    cacheContainer: JQuery;
    cacheHeadline: JQuery;
    
    /*
    We store the table itself in a container to allow a dedicated scrolling of the table
     */
    cacheTableContainer: JQuery;
    cacheTable: JQuery;
    cacheEmptyDiv: JQuery;
    cacheButtons: JQuery;
    /*
    Stores the empty div which gets filled to provide the type in case the user clicks on 'Create new Item'
     */
    cacheButtonsTypeSelection: JQuery;
    cacheFreeTextField: JQuery;
    /*
    Stores the option top provide some settings and also shows the query text
     */
    cacheSettings: JQuery;
    
    /*
    Stores the button which opens the form settings
     */
    cacheSettingsButton: JQuery;
    cacheQueryText: JQuery;
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

    /**
     * This callback allows to load the items in case the user has changed the QueryFilterParameters
     */
    callbackLoadItems: (query: InterfacesForms.QueryFilterParameter) => Promise<Array<Mof.DmObject>>;

    firstRun: boolean = true;

    /**
     * Refreshes the complete form including the parent item which might contain multiple tables
     */
    async refreshForm(): Promise<void> {

        if (this.configuration.refreshForm !== undefined) {
            await this.configuration.refreshForm();
        } else {
            await this.createFormByCollection(this.tableCache.parentHtml, this.configuration, true);
        }
    }

    /**
     * Just refreshes the form and performs a reloading of items from the server
     * The query parameters are taken into consideration
     */
    async reloadTable(): Promise<void> {
        this.updateFilterQueryText();
        await this.createFormByCollection(this.tableCache.parentHtml, this.configuration, true);
    }

    async refreshTable(): Promise<void> {
        this.updateFilterQueryText();
        await this.createTable();
    }

    /**
     * This method just calls the createFormByCollection since a TableForm can 
     * show the extent's elements directly or just the properties of an elemnet
     * @param parent The Html to which the table shall be added
     * @param configuration The Configuration for the table
     * @param refresh true, if we just would like to refresh the table and not create new elements
     */
    async createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration, refresh?: boolean) {

        const tthis = this;

        // We need to get a loading mechanism in case the user wants to filter or sort. Unfortunately, the queries are not support
        this.callbackLoadItems = async (query: InterfacesForms.QueryFilterParameter) => {
            // As mentioned, the queries are not supported, so we don't show them
            this.tableParameter.allowSortingOfColumn = false;
            this.tableParameter.allowFilteringOnProperty = false;

            // Loads the properties as a whole
            const property = this.formElement.get(_DatenMeister._Forms._TableForm.property, Mof.ObjectType.String);

            const result = await ClientItem.getProperty(tthis.workspace, tthis.itemUrl, property);
            if (Array.isArray(result)) {
                return result;
            }
            else {
                return [];
            }
        };

        return await this.createFormByCollection(parent, configuration, refresh);
    }

    /**
     * Creates the form by the given collection
     * @param parent The parent html element
     * @param configuration The configuration for the form
     * @param refresh true, if we just would like to refresh the table and not create new elements
     */
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
        query.filterByProperties = this.tableState.filterByProperty;
        query.filterByFreetext = this.tableState.freeTextFilter;

        this.elements = await this.callbackLoadItems(query);

        if (this.firstRun) {
            this.firstRun = false;
            
            this.tableCache.cacheContainer = $("<div class='dm-tableform-container'></div>");
            parent.append(this.tableCache.cacheContainer);
            
            this.tableCache.cacheHeadline = $("<h2><a></a></h2>");
            this.tableCache.cacheContainer.append(this.tableCache.cacheHeadline);

            this.tableCache.cacheFreeTextField = $("<div class='dm-tableform-freetextform'></div>");
            this.tableCache.cacheContainer.append(this.tableCache.cacheFreeTextField);

            this.tableCache.cacheButtons = $("<div class='dm-tableform-buttons'></div>");
            this.tableCache.cacheContainer.append(this.tableCache.cacheButtons);
            this.tableCache.cacheButtonsTypeSelection = $("<div class='dm-tableform-button-typeselection'></div>");
            this.tableCache.cacheContainer.append(this.tableCache.cacheButtonsTypeSelection);

            this.tableCache.cacheSettings = $("<div class='dm-tableform-settings'><a class='btn btn-secondary'>Tableform Settings</a></div>");
            this.tableCache.cacheContainer.append(this.tableCache.cacheSettings);
            
            this.tableCache.cacheSettingsButton = $("<div class='dm-tableform-settings-button'></div>");
            this.tableCache.cacheSettings.append(this.tableCache.cacheSettingsButton);
            await this.initializeTableSettingsButton();
            
            this.tableCache.cacheQueryText = $('<div class="dm-tableform-querytext"></div>');
            this.tableCache.cacheSettings.append(this.tableCache.cacheQueryText);

            this.tableCache.cacheEmptyDiv = $("<div></div>");
            this.tableCache.cacheContainer.append(this.tableCache.cacheEmptyDiv);
            
            this.tableCache.cacheTableContainer = $("<div class='dm-tableform-tablecontainer'></div>");
            this.tableCache.cacheContainer.append(this.tableCache.cacheTableContainer);

            this.tableCache.cacheTable = $("<table class='table table-striped table-bordered align-top dm-tableform'></table>");
            this.tableCache.cacheTableContainer.append(this.tableCache.cacheTable);

            // Create filter for freetext
            if (this.tableParameter.allowFreeTextFiltering) {
                // Create freetext
                this.createFreeTextField();
            }
        }

        const headLineLink = $("a", this.tableCache.cacheHeadline);
        headLineLink.text(
            this.formElement.get('title')
            ?? this.formElement.get('name'));

        const link = Navigator.getLinkForNavigateToExtentItems(
            this.workspace, this.extentUri, {metaClass: this.tableParameter.metaClass});
        if (link !== null) {
            headLineLink.attr(
                'href', link);
        }

        this.tableCache.cacheButtons.empty();

        // Evaluate the new buttons to create objects
        this.createButtonsForNewInstance();

        // Create Query Text
        this.updateFilterQueryText();
        
        // Creates the table 
        if (this.elements === undefined) {
            this.elements = [];
        }

        if (!Array.isArray(this.elements)) {
            // Creates an empty table in case a non-array was given
            this.tableCache.cacheEmptyDiv.empty();
            this.tableCache.cacheEmptyDiv.text("Non-Array elements for ListForm: ");
            this.tableCache.cacheEmptyDiv.append($("<em></em>").text((this.elements as any).toString()));

        } else {
            // Creates the table            
            await this.createTable();
        }
    }

    /*
    * Creates the buttons for the new instance
     */
    private createButtonsForNewInstance() {
        const property = this.formElement.get('property');
        const tthis = this;

        if (this.formElement.get(_DatenMeister._Forms._TableForm.inhibitNewUnclassifiedItems, Mof.ObjectType.Boolean) !== true) {
            // Creates default unclassified button
            createUnclassifiedButton();
        }

        // Goes through all the elements
        const defaultTypesForNewElements = this.formElement.getAsArray("defaultTypesForNewElements");
        if (defaultTypesForNewElements !== undefined) {
            for (let n in defaultTypesForNewElements) {
                const inner = defaultTypesForNewElements[n] as Mof.DmObject;
                createButton(
                    inner.get('name', Mof.ObjectType.String),
                    inner.get('metaClass', Mof.ObjectType.Object));
            }
        }

        /**
         * Creates the button to create a new item with the given metaclass and adds it to the button row
         */
        function createUnclassifiedButton() {
            // If user clicks on the button, the user has the opportunity to select the field to be created. 
            const btn = $("<btn class='btn btn-secondary'></btn>");
            const typeSelection = $("<div></div>");
            btn.text("Create new Item");
            btn.on('click', async () => {
                typeSelection.empty();
                const selectItem = new SIC.SelectItemControl();
                const settings = new SIC.Settings();
                settings.showWorkspaceInBreadcrumb = true;
                settings.showExtentInBreadcrumb = true;
                settings.setButtonText = 'Create new Item';
                selectItem.itemSelected.addListener(
                    selectedItem => {
                        if (tthis.itemUrl === undefined) {
                            document.location.href = Navigator.getLinkForNavigateToCreateNewItemInExtent(
                                tthis.workspace,
                                tthis.extentUri,
                                selectedItem === undefined ? undefined : selectedItem.uri,
                                selectedItem === undefined ? undefined : selectedItem.workspace);
                        } else {
                            document.location.href = Navigator.getLinkForNavigateToCreateItemInProperty(
                                tthis.workspace,
                                tthis.itemUrl,
                                selectedItem === undefined ? undefined : selectedItem.uri,
                                selectedItem === undefined ? undefined : selectedItem.workspace,
                                property);
                        }
                    });

                await selectItem.setWorkspaceById('Types');
                await selectItem.setExtentByUri("Types", "dm:///_internal/types/internal");

                selectItem.init(typeSelection, settings);
            });


            tthis.tableCache.cacheButtons.append(btn);
            tthis.tableCache.cacheButtonsTypeSelection.append(typeSelection);
        }

        /**
         * Creates the button to create a new item with the given metaclass and adds it to the button row
         * @param name - The name of the button
         * @param metaClass - The metaclass of the item to be created
         */
        function createButton (name: string, metaClass?: Mof.DmObject) {
            const metaClassUri = metaClass?.uri;
            const metaClassWorkspace = metaClass?.workspace;
            
            const btn = $("<btn class='btn btn-secondary'></btn>");
            btn.text("Create " + name);
            btn.on('click', () => {
                                
                // Creates the location to the buttons
                if (property === undefined || property === null) {
                    document.location.href =
                        Navigator.getLinkForNavigateToCreateNewItemInExtent(
                            tthis.workspace,
                            tthis.extentUri,
                            metaClassUri, 
                            metaClassWorkspace);
                } else {
                    document.location.href = Navigator.getLinkForNavigateToCreateItemInProperty(
                        tthis.workspace,
                        tthis.itemUrl,
                        metaClassUri,
                        "Types",
                        property);
                }
            });

            tthis.tableCache.cacheButtons.append(btn);
        }
    }

    /**
     * Creates the free text field for filtering
     */
    private createFreeTextField() {
        const tthis = this;

        const inputField = $('<input type="text" placeholder="Filter by Text"></input>');
        inputField.on('input', async () => {
            tthis.tableState.freeTextFilter = inputField.val().toString();
            await tthis.reloadTable();
        });

        this.tableCache.cacheFreeTextField.append(inputField);
    }
    
    private async initializeTableSettingsButton()
    {
        $(".btn", this.tableCache.cacheSettings).on('click', async () => {
            alert('X');
            const popup = burnJsPopup.createPopup();

            const table = $("<table class='table table-bordered dm-table-nofullwidth align-top dm-tableform'><th>Action</th><th>Parameter</th></table>");
            $(popup.htmlContent).append(table);

            // Add submit line
            const submitButton = $("<button class='btn btn-primary' type='button'>Submit</button>");
            submitButton.on('click', () => {
                /*propertyMenuItems.forEach(menuItem => {
                    if (menuItem.onSubmitForm) {
                        menuItem.onSubmitForm();
                    }
                });*/

                popup.closePopup();
            });
            const submitRow = $("<tr><td></td><td class='dm-value'></td></tr>").append(submitButton);


            $("td.dm-value", submitRow).append(submitButton);
            table.append(submitRow);
        });
    }

    /**
     * Updates the filter query text which describes the current filter settings
     */
    private updateFilterQueryText() {
        if (this.tableParameter.showFilterQuery) {
            const queryText = this.getSummaryOfQuery();
            this.tableCache.cacheQueryText.text(queryText);
        }
    }

    /**
    * Creates the table itself that shall be shown
    */
    private async createTable() {
        const tthis = this;
        if (this.tableCache?.cacheTable === undefined) return;
        this.tableCache.cacheTable.empty();

        const fields = this.formElement.getAsArray("field") as Mof.DmObject[];

        const headerRow = $("<tbody><tr></tr></tbody>");
        const innerRow = $("tr", headerRow);

        // Create the column headlines
        for (const field of fields) {

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
        const metaClassFilter = this.tableParameter.metaClass;

        for (const element of this.elements) {

            const elementsMetaClass = element.metaClass?.uri;

            // Check if the element may be shown
            if ((elementsMetaClass && noItemsWithMetaClass) ||
                (metaClassFilter && elementsMetaClass !== metaClassFilter)) {
                continue;
            }

            /*            
            // If we have freetext, then we need to skip the row.
            if (this.tableParameter.allowFreeTextFiltering && !this.isElementMatchingFreeTextFilter(element, fields)) {
                continue;
            }            
             */

            const row = $("<tr></tr>");

            for (const field of fields) {
                let cell = $("<td></td>");

                const fieldMetaClassUri = field.metaClass.uri;
                const fieldElement = FieldFactory.createField(
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

    /**
     * Checks if the specified element matches the free text filter based on the provided fields.
     * @param element - The element to check against the free text filter.
     * @param fields - The fields to consider for the free text filter.
     * @returns True if the element matches the free text filter, false otherwise.
     */
    private isElementMatchingFreeTextFilter(element: Mof.DmObject, fields: Mof.DmObject[]): boolean {
        const filterText = this.tableState.freeTextFilter.toLowerCase();
        
        if (filterText === undefined || filterText === null || filterText === "") {
            // Item is matching in case the filterText is empty
            return true;
        }
        
        return fields.some(field => {
            if (!FieldFactory.canBeTextFiltered(field)) return false;
            const fieldName = field.get(_FieldData._name_);
            const fieldValue = element.get(fieldName, Mof.ObjectType.String);
            return fieldValue?.toString().toLowerCase().includes(filterText);
        });
    }

    /**
     * Creates a sorting button for the specified field and appends it to the title buttons.
     * The button allows toggling between ascending, descending, and no sorting states.
     *
     * @param field - The field for which the sorting button is created.
     * @param titleButtons - The jQuery element to which the sorting button will be appended.
     */
    private createSortingButton(field: Mof.DmObject, titleButtons: JQuery<HTMLElement>) {
        const fieldCanBeSorted = FieldFactory.canBeSorted(field);

        if (!fieldCanBeSorted) return;

        const fieldName = field.get(_FieldData._name_);
        const isSorted = this.tableState.orderBy === fieldName;
        const isSortedDescending = this.tableState.orderByDescending;

        let sortingArrow: JQuery;
        let onClick: () => Promise<void>;

        if (!isSorted) {
            // Starting point, not sorted --> Transition to Sorted
            sortingArrow = $('<span class="dm-tableform-sortbutton">⇅</span>');
            onClick = async () => {
                this.tableState.orderBy = fieldName;
                this.tableState.orderByDescending = false;
                await this.reloadTable();
            };
        } else if (isSorted && !isSortedDescending) {
            // Now we are sorted, on next click, we are sorted by descnding
            sortingArrow = $('<span class="dm-tableform-sortbutton">↓</span>');
            onClick = async () => {
                this.tableState.orderByDescending = true;
                await this.reloadTable();
            };
        } else if (isSorted && isSortedDescending) {
            // Now we are sorted descending, on next click, we switch back to unsorted
            sortingArrow = $('<span class="dm-tableform-sortbutton">↑</span>');
            
            onClick = async () => {
                this.tableState.orderByDescending = false;
                this.tableState.orderBy = undefined;
                await this.reloadTable();
            };
            
        }

        sortingArrow.on('click', onClick);
        titleButtons.append(sortingArrow);
    }


    private async appendColumnMenus(field: Mof.DmObject, cell: JQuery<HTMLElement>) {
        const propertyMenuItems = await this.createPropertyMenuItems(field);
        if (propertyMenuItems.length === 0) {
            return;
        }

        // Now create the button
        let manualButtonContent = false;
        let contextItem = $("<div class='dm-tableform-sortbutton'></div>");
        cell.append(contextItem);

        propertyMenuItems.forEach(menuItem => {
            if (menuItem.callbackButtonText) {
                if (menuItem.callbackButtonText(contextItem)) {
                    manualButtonContent = true;
                }
            }
        });

        // If, no content-driven content is defined, add the three dots
        if (!manualButtonContent) {
            contextItem.text("...");
        }                

        // Defines the callback! 
        contextItem.on('click', () => {

            const popup = burnJsPopup.createPopup();

            const table = $("<table class='table table-bordered dm-table-nofullwidth align-top dm-tableform'><th>Action</th><th>Parameter</th></table>");
            $(popup.htmlContent).append(table);

            propertyMenuItems.forEach(menuItem => {
                const tableRow = $("<tr><td class='dm-key'></td><td class='dm-value'></td></tr>");
                const cellKey = $(".dm-key", tableRow);
                const cellValue = $(".dm-value", tableRow);

                menuItem.onCreateDom(popup, cellValue);
                if (menuItem.cellKeyTitle !== undefined) {
                    cellKey.text(menuItem.cellKeyTitle);
                }

                table.append(tableRow);
            });

            // Add submit line
            const submitButton = $("<button class='btn btn-primary' type='button'>Submit</button>");
            submitButton.on('click', () => {
                propertyMenuItems.forEach(menuItem => {
                    if (menuItem.onSubmitForm) {
                        menuItem.onSubmitForm();
                    }
                });

                popup.closePopup();
            });
            const submitRow = $("<tr><td></td><td class='dm-value'></td></tr>").append(submitButton);


            $("td.dm-value", submitRow).append(submitButton);
            table.append(submitRow);
        });
    }

    async createPropertyMenuItems(field: Mof.DmObject): Promise<PropertyMenuItem[]> {
        let result = [];

        if (field.metaClass.uri !== _DatenMeister._Forms.__ActionFieldData_Uri
            && field.metaClass.uri !== _DatenMeister._Forms.__MetaClassElementFieldData_Uri) {

            result.push(ContextMenu.createFunctionToRemoveAllProperties(field));
            
            if (this.tableParameter.allowFilteringOnProperty) {
                result.push(ContextMenu.createFunctionToFilterInProperty(this, field));
            }
        }

        return result;
    }

    /**
     * Gets the summary text which is read by the user to understand the effective filtering
     * @returns The summary text
     */
    getSummaryOfQuery() {
        let result = "";
        let andText = '';

        if (this.tableState.orderBy !== undefined) {
            result += `Order By: ${this.tableState.orderBy}`;
            if (this.tableState.orderByDescending) {
                result += ' (Descending)';
            }

            andText = ' AND ';
        }

        if (this.tableState.freeTextFilter !== undefined && this.tableState.freeTextFilter !== "") {
            result += `${andText}Free-Textfilter: ${this.tableState.freeTextFilter}`;
            andText = ' AND ';
        }

        if (this.tableState.filterByProperty !== undefined) {
            for (let key in this.tableState.filterByProperty) {
                const value = this.tableState.filterByProperty[key];

                result += `${andText + key} is '${value}'`;
                andText = ' AND ' 
            }
        }        

        if (result === "") {
            return "No Filter";
        }

        return result;
    }
}