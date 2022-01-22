import * as InterfacesForms from "./Forms.Interfaces";
import * as Mof from "./Mof";
import {DmObject} from "./Mof";
import {createField} from "./Forms.FieldFactory";
import * as Settings from "./Settings";
import {IFormConfiguration} from "./IFormConfiguration";
import * as SIC from "./Forms.SelectItemControl";

export class ListForm implements InterfacesForms.IForm {
    elements: Array<DmObject>;
    extentUri: string;
    formElement: DmObject;
    itemId: string;
    workspace: string;
    parentHtml: JQuery<HTMLElement>;
    configuration: IFormConfiguration;

    refreshForm(): void {
        this.createFormByCollection(this.parentHtml, this.configuration);
    }

    createFormByCollection(parent: JQuery<HTMLElement>, configuration: IFormConfiguration) {
        this.parentHtml = parent;
        this.configuration = configuration;

        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }
        
        let headline = $("<h2></h2>");
        headline.text(this.formElement.get('name'));
        parent.append(headline);

        if (!Array.isArray(this.elements)) {
            const div = $("<div></div>");
            div.text("Non-Array elements for ListForm: ");
            div.append($("<em></em>").text(this.elements));
            parent.append(div);
        }
        else {
            let table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
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

            table.append(headerRow);

            let elements = this.elements;
            for (let n in elements) {
                if (Object.prototype.hasOwnProperty.call(elements, n)) {
                    let element = this.elements[n];

                    const row = $("<tr></tr>");

                    for (let n in fields) {
                        if (!fields.hasOwnProperty(n)) continue;
                        const field = fields[n] as DmObject;
                        let cell = $("<td></td>");

                        const fieldMetaClassId = field.metaClass.id;
                        const fieldElement = createField(
                            fieldMetaClassId,
                            {
                                configuration: configuration,
                                form: this,
                                field: field,
                                itemUrl: element.uri,
                                isReadOnly: configuration.isReadOnly
                            });

                        cell.append(fieldElement.createDom(element));
                        row.append(cell);
                    }

                    table.append(row);
                }
            }

            parent.append(table);
        }
    }
}

export function openMetaClassSelectionFormForNewItem(buttonDiv: JQuery, containerDiv: JQuery, workspace: string, extentUri: string) {
    const tthis = this;

    buttonDiv.on('click', () => {
        containerDiv.empty();
        const selectItem = new SIC.SelectItemControl();
        const settings = new SIC.Settings();
        settings.showWorkspaceInBreadcrumb = true;
        settings.showExtentInBreadcrumb = true;
        selectItem.onItemSelected = selectedItem => {
            if (selectedItem === undefined) {
                document.location.href =
                    Settings.baseUrl +
                    "ItemAction/Extent.CreateItem?workspace=" +
                    encodeURIComponent(workspace) +
                    "&extent=" +
                    encodeURIComponent(extentUri);
            } else {
                document.location.href =
                    Settings.baseUrl +
                    "ItemAction/Extent.CreateItem?workspace=" +
                    encodeURIComponent(workspace) +
                    "&extent=" +
                    encodeURIComponent(extentUri) +
                    "&metaclass=" +
                    encodeURIComponent(selectedItem.uri);
            }
        };

        selectItem.init(containerDiv, settings);
    });
}