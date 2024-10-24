import { FormType } from "./Interfaces.js";
import * as Mof from "../Mof.js";
import { createField } from "./FieldFactory.js";
import * as Settings from "../Settings.js";
import * as Navigator from '../Navigator.js';
import { _DatenMeister } from "../models/DatenMeister.class.js";
var _TableForm = _DatenMeister._Forms._TableForm;
import * as Actions from "../client/Actions.js";
var _FieldData = _DatenMeister._Forms._FieldData;
export class TableForm {
    constructor() {
        this.formType = FormType.Table;
    }
    async refreshForm() {
        if (this.configuration.refreshForm !== undefined) {
            this.configuration.refreshForm();
        }
        else {
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
    async createFormByObject(parent, configuration, refresh) {
        if (this.elements === undefined && this.element !== undefined) {
            this.elements = this.element.get(this.formElement.get("property"));
        }
        return await this.createFormByCollection(parent, configuration, refresh);
    }
    async createFormByCollection(parent, configuration, refresh) {
        this.parentHtml = parent;
        this.configuration = configuration;
        let metaClass = this.formElement.get('metaClass')?.uri;
        const tthis = this;
        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }
        this.cacheHeadline =
            refresh === true && this.cacheHeadline !== undefined
                ? this.cacheHeadline
                : $("<h2><a></a></h2>");
        const headLineLink = $("a", this.cacheHeadline);
        headLineLink.text(this.formElement.get('title')
            ?? this.formElement.get('name'));
        headLineLink.attr('href', Navigator.getLinkForNavigateToExtentItems(this.workspace, this.extentUri, { metaClass: metaClass }));
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
                    tthis.cacheButtons.append(btn);
                })(inner);
            }
        }
        if (this.elements === undefined) {
            this.elements = [];
        }
        // Evaluate the elements themselves
        if (!Array.isArray(this.elements)) {
            this.cacheEmptyDiv =
                refresh === true && this.cacheTable !== undefined
                    ? this.cacheTable
                    : $("<div></div>");
            this.cacheEmptyDiv.empty();
            this.cacheEmptyDiv.text("Non-Array elements for ListForm: ");
            this.cacheEmptyDiv.append($("<em></em>").text(this.elements.toString()));
            if (refresh !== true) {
                parent.append(this.cacheEmptyDiv);
            }
        }
        else {
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
                if (!fields.hasOwnProperty(n))
                    continue;
                const field = fields[n];
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
            let elements = this.elements;
            for (let n in elements) {
                if (Object.prototype.hasOwnProperty.call(elements, n)) {
                    let element = this.elements[n];
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
                        if (!fields.hasOwnProperty(n))
                            continue;
                        const field = fields[n];
                        let cell = $("<td></td>");
                        const fieldMetaClassUri = field.metaClass.uri;
                        const fieldElement = createField(fieldMetaClassUri, {
                            configuration: configuration,
                            field: field,
                            itemUrl: element.uri,
                            isReadOnly: configuration.isReadOnly,
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
                    this.cacheTable.append(row);
                }
            }
            if (refresh !== true) {
                parent.append(this.cacheTable);
            }
        }
    }
    async appendColumnMenus(field, cell) {
        const column = await this.createPropertyMenuItems(field);
        let contextItem = $("<div class='dm-contextmenu'><div class='dm-contextmenu-dots'>...</div><div class='dm-contextmenu-item-container'></div></div>");
        const htmlContainer = $(".dm-contextmenu-item-container", contextItem);
        for (const m in column) {
            const menuProperty = column[m];
            const htmlItem = $("<div class='dm-contextmenu-item'></div>");
            htmlItem.text(menuProperty.title);
            if (menuProperty.onClick !== undefined) {
                htmlItem.on('click', () => {
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
    async createPropertyMenuItems(field) {
        let result = [];
        const propertyName = field.get(_FieldData._name_, Mof.ObjectType.String);
        if (field.metaClass.uri !== _DatenMeister._Forms.__ActionFieldData_Uri
            && field.metaClass.uri !== _DatenMeister._Forms.__MetaClassElementFieldData_Uri) {
            const menuRemoveProperty = {
                title: "Clear Property",
                requireConfirmation: true,
                onClick: async () => {
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
                }
            };
            result.push(menuRemoveProperty);
        }
        // If no entry is added, add at least some comment
        if (result.length === 0) {
            result.push({ title: "No Action" });
        }
        return result;
    }
}
//# sourceMappingURL=TableForm.js.map