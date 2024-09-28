import * as InterfacesForms from "./Interfaces.js";
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
    parentHtml: JQuery<HTMLElement>;
    configuration: IFormConfiguration;
    shortenFullText: boolean = true; 
    pageNavigation: InterfacesForms.IPageNavigation;

    cacheHeadline: JQuery;

    cacheTable: JQuery;

    cacheEmptyDiv: JQuery;

    cacheButtons: JQuery;

    callbackLoadItems: (query: InterfacesForms.QueryFilterParameter) => Promise<Array<Mof.DmObject>>;

    async refreshForm(): Promise<void> {

        if (this.configuration.refreshForm !== undefined) {
            this.configuration.refreshForm();
        } else {
            await this.createFormByCollection(this.parentHtml, this.configuration, true);
        }
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
        this.parentHtml = parent;
        this.configuration = configuration;

        let metaClass = (this.formElement.get('metaClass') as Mof.DmObject)?.uri;
        const tthis = this;

        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }

        if (this.callbackLoadItems === undefined) {
            throw "No callbackLoadItems is set";
        }

        this.cacheHeadline =
            refresh === true && this.cacheHeadline !== undefined
                ? this.cacheHeadline
                : $("<h2><a></a></h2>");

        const headLineLink = $("a", this.cacheHeadline);
        headLineLink.text(
            this.formElement.get('title')
            ?? this.formElement.get('name'));

        headLineLink.attr(
            'href',
            Navigator.getLinkForNavigateToExtentItems(this.workspace, this.extentUri, {metaClass: metaClass}));

        if (refresh !== true) {
            parent.append(this.cacheHeadline);
        }

        const property = this.formElement.get('property');

        this.cacheButtons =
            refresh === true && this.cacheHeadline !== undefined
                ? this.cacheButtons
                : $("<div></div>");
        this.cacheButtons.empty();
        if (refresh !== true) {
            parent.append(this.cacheButtons);
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

                    tthis.cacheButtons.append(btn);
                })(inner);

            }
        }

        const query = new InterfacesForms.QueryFilterParameter();

        let elements = this.elements = await this.callbackLoadItems(query);

        if (elements === undefined) {
            elements = [];
        }

        // Evaluate the elements themselves
        if (!Array.isArray(elements)) {
            this.cacheEmptyDiv =
                refresh === true && this.cacheTable !== undefined
                    ? this.cacheTable
                    : $("<div></div>");
            this.cacheEmptyDiv.empty();
            this.cacheEmptyDiv.text("Non-Array elements for ListForm: ");
            this.cacheEmptyDiv.append($("<em></em>").text((elements as any).toString()));

            if (refresh !== true) {
                parent.append(this.cacheEmptyDiv);
            }
        } else {
            this.cacheTable =
                refresh === true && this.cacheTable !== undefined
                    ? this.cacheTable
                    : $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top dm-tableform'></table>");
            this.cacheTable.empty();

            const fields = this.formElement.getAsArray("field");

            const headerRow = $("<tbody><tr></tr></tbody>");
            const innerRow = $("tr", headerRow);

            // Create the column headlines
            for (let n in fields) {
                if (!fields.hasOwnProperty(n)) continue;
                const field = fields[n] as Mof.DmObject;

                // Create the column
                let cell = $("<th></th>");

                // Create the text of the headline
                cell.text(field.get(_FieldData.title) ?? field.get(_FieldData._name_));

                // Create the column menu
                await this.appendColumnMenus(field, cell);

                innerRow.append(cell);
            }

            this.cacheTable.append(headerRow);

            let noItemsWithMetaClass = this.formElement.get('noItemsWithMetaClass');

            for (let n in elements) {
                if (Object.prototype.hasOwnProperty.call(elements, n)) {
                    let element = elements[n];

                    // Check, if the element may be shown
                    let elementsMetaClass = element.metaClass?.uri;
                    if ((elementsMetaClass !== undefined && elementsMetaClass !== "") && noItemsWithMetaClass) {
                        // Only items with no metaclass may be shown, but the element is the metaclass
                        continue;
                    }

                    if ((metaClass !== undefined && metaClass !== "") && elementsMetaClass !== metaClass) {
                        // Only elements with given metaclass shall be shown, but given element is not of
                        // the metaclass type
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
                                configuration: configuration,
                                field: field,
                                itemUrl: element.uri,
                                isReadOnly: configuration.isReadOnly,
                                form: this
                            });

                        let dom;
                        if (fieldElement === undefined) {
                            dom = $("<span></span>");
                            dom.text ("Field for " + field.get("name", Mof.ObjectType.String) + " not found");
                        } else {
                            dom = await fieldElement.createDom(element);
                        }

                        cell.append(dom);
                        row.append(cell);
                    }

                    this.cacheTable.append(row);
                }
            }

            if (refresh !== true) {
                parent.append(this.cacheTable);
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