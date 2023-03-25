import * as InterfacesForms from "./Interfaces";
import {FormType} from "./Interfaces";
import * as Mof from "../Mof";
import {createField} from "./FieldFactory";
import * as Settings from "../Settings";
import {IFormConfiguration} from "./IFormConfiguration";
import * as Navigator from '../Navigator'

export class TableForm implements InterfacesForms.IForm {
    elements: Array<Mof.DmObject>;
    extentUri: string;
    formElement: Mof.DmObject;
    itemUrl: string;
    workspace: string;
    parentHtml: JQuery<HTMLElement>;
    configuration: IFormConfiguration;
    formType: FormType = FormType.Table;

    cacheHeadline: JQuery;

    cacheTable: JQuery;

    cacheEmptyDiv: JQuery;

    cacheButtons: JQuery;

    async refreshForm(): Promise<void> {
        await this.createFormByCollection(this.parentHtml, this.configuration, true);
    }

    async createFormByCollection(parent: JQuery<HTMLElement>, configuration: IFormConfiguration, refresh?: boolean) {
        this.parentHtml = parent;
        this.configuration = configuration;

        let metaClass = (this.formElement.get('metaClass') as Mof.DmObject)?.uri;
        const tthis = this;

        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }

        this.cacheHeadline =
            refresh === true && this.cacheHeadline !== undefined
                ? this.cacheHeadline
                : $("<h2><a></a></h2>");
        this.cacheHeadline.empty();

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
            this.cacheEmptyDiv.append($("<em></em>").text((this.elements as any).toString()));

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

            for (let n in fields) {
                if (!fields.hasOwnProperty(n)) continue;
                const field = fields[n] as Mof.DmObject;

                let cell = $("<th></th>");
                cell.text(field.get("title") ?? field.get("name"));

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

                        const dom = await fieldElement.createDom(element);

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
}