import {BaseField, IFormField} from "../Interfaces.Fields";
import {DmObject} from "../Mof";
import * as FieldFactory from "../Forms.FieldFactory";

import {getItemDetailUri} from "../Website";
import * as SIC from "../Forms.SelectItemControl";
import * as ClientItems from "../Client.Items";

export class Field extends BaseField implements IFormField {

    _list: JQuery;
    _element: DmObject;

    reloadValuesFromServer(): void {
        const tthis = this;
        const url = this._element.uri;
        const fieldName = this.field.get('name');

        ClientItems.getProperty(this.form.workspace, url, fieldName).done(
            x => tthis.createDomByValue(x)
        );
    }

    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        this._element = dmElement;
        const fieldName = this.field.get('name');
        const value = dmElement.get(fieldName);

        this._list = $("<div></div>");
        this.createDomByValue(value);

        return this._list
    }

    createDomByValue(value: any): JQuery<HTMLElement> {
        const tthis = this;
        this._list.empty();

        if (this.isReadOnly) {

            let ul = $("<ul class='list-unstyled'></ul>");

            let foundElements = 0;
            for (let m in value) {
                if (Object.prototype.hasOwnProperty.call(value, m)) {
                    let innerValue = value[m] as DmObject;

                    const item = $("<li><a></a></li>");
                    const link = $("a", item);
                    const name = innerValue.get('name');
                    if (name !== undefined && name !== "") {
                        link.text(innerValue.get('name'));
                    } else {
                        link.append($("<em>Unnamed</em>"));
                    }

                    link.attr('href', getItemDetailUri(innerValue));
                    ul.append(item);

                    foundElements++;
                }
            }

            if (foundElements === 0) {
                ul = $("<em>No items</em>");
            }

            this._list.append(ul);
        } else {
            const table = $("<table><tbody></tbody></table>");
            this._list.append(table);

            let fields = this.field.get('form')?.get('field');

            let fieldsData = new Array<DmObject>();
            if (fields === undefined) {
                const nameField = new DmObject();
                nameField.setMetaClassById("DatenMeister.Models.Forms.TextFieldData");
                nameField.set('name', 'name');
                nameField.set('title', 'Name');
                nameField.set('isReadOnly', true);
                fieldsData.push(nameField);
            }

            /* Creates the table header */
            const tBody = $("tbody", table);
            const tr = $("<tr></tr>");
            for (let fieldDataKey in fieldsData) {
                let fieldData = fieldsData[fieldDataKey];

                let header = $("<th></th>");
                header.text(fieldData.get('title'));
                tr.append(header);
            }

            let deleteHeader = $("<th>Delete</th>");
            tr.append(deleteHeader);

            tBody.append(tr);

            /* Creates the rows */
            for (let m in value) {
                if (Object.prototype.hasOwnProperty.call(value, m)) {
                    const tr = $("<tr></tr>");
                    let innerValue = value[m] as DmObject;

                    for (let fieldDataKey in fieldsData) {
                        const td = $("<td></td>");
                        let fieldData = fieldsData[fieldDataKey];

                        const field = FieldFactory.createField(
                            fieldData.metaClass.id,
                            {
                                field: fieldData,
                                form: this.form,
                                isReadOnly: true,
                                itemUrl: innerValue.uri,
                                configuration: {}
                            });
                        const dom = field.createDom(innerValue);
                        td.append(dom);
                        tr.append(td);
                    }

                    /* Creates the delete button */
                    let deleteCell = $("<td><btn class='btn btn-secondary'>Delete</btn></td>");
                    $("btn", deleteCell).on('click',
                        () => {
                            ClientItems.removeReferenceFromCollection(
                                tthis.form.workspace,
                                tthis.itemUrl,
                                {
                                    property: tthis.field.get('name'),
                                    referenceUri: innerValue.uri,
                                    referenceWorkspaceId: innerValue.workspace
                                })
                                .done(() => {
                                    tthis.reloadValuesFromServer()
                                });
                        });

                    tr.append(deleteCell);

                    table.append(tr);
                }
            }

            const newItem = $("<div><btn class='btn btn-secondary dm-subelements-appenditem-btn'>Attach new Item</btn><div class='dm-subelements-appenditem-box'></div></div>");
            $(".dm-subelements-appenditem-btn", newItem).on('click', () => {
                const containerDiv = $(".dm-subelements-appenditem-box");
                containerDiv.empty();
                const selectItem = new SIC.SelectItemControl();
                const settings = new SIC.Settings();
                settings.showWorkspaceInBreadcrumb = true;
                settings.showExtentInBreadcrumb = true;
                selectItem.onItemSelected = selectedItem => {
                    ClientItems.addReferenceToCollection(
                        tthis.form.workspace,
                        tthis.itemUrl,
                        {
                            property: tthis.field.get('name'),
                            referenceUri: selectedItem.uri,
                            referenceWorkspaceId: selectItem.getUserSelectedWorkspace()
                        }
                    ).done(() => {
                        this.reloadValuesFromServer();
                    });
                };

                selectItem.init(containerDiv, settings);

                return false;
            });


            this._list.append(newItem);
        }

        const refreshBtn = $("<div><btn class='dm-subelements-refresh-btn'><img src='/img/refresh-16.png' alt='Refresh' /></btn></div>");
        $(".dm-subelements-refresh-btn", refreshBtn).on('click', () => {
            tthis.reloadValuesFromServer();
        });
        this._list.append(refreshBtn);

        return this._list;
    }

    evaluateDom(dmElement: DmObject) {

    }
}