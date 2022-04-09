import {IFormField} from "./Interfaces";
import {DmObject} from "../Mof";
import * as FieldFactory from "../forms/FieldFactory";
import * as SIC from "../controls/SelectItemControl";
import * as ClientItems from "../client/Items";
import {resolve} from "../MofResolver"
import {navigateToItemByUrl} from "../Navigator";
import {IFormConfiguration} from "../forms/IFormConfiguration";
import {IForm, IFormNavigation} from "../forms/Interfaces";
import * as Settings from "../Settings";

export class Control {
    configuration: IFormConfiguration;
    isReadOnly: boolean;

    // Is connected to the item url of the element being connected to that element
    itemUrl: string;
    form: IFormNavigation;
    propertyName: string;

    _list: JQuery;

    constructor() {
        this._list = $("<div></div>");
    }

    createDomByValue(value: any): JQuery<HTMLElement> {

        const tthis = this;
        this._list.empty();

        if (this.isReadOnly) {
            if (!Array.isArray(value)) {
                return $("<div><em>Element is not an Array</em></div>")
            }

            let ul = $("<ul class='list-unstyled'></ul>");

            let foundElements = 0;
            for (let m in value) {
                if (Object.prototype.hasOwnProperty.call(value, m)) {
                    let innerValue = value[m] as DmObject;
                    const item = $("<li><a></a></li>");

                    // Resolve the elements
                    ((a: DmObject, b: JQuery) => {
                        resolve(a).then(resolvedRaw => {
                            const resolved = resolvedRaw as DmObject;
                            const link = $("a", b);
                            const name = resolved.get('name');
                            if (name !== undefined && name !== "") {
                                link.text(resolved.get('name'));
                            } else {
                                link.append($("<em>Unnamed</em>"));
                            }

                            link.attr('href', '#');
                            link.on('click', () => {
                                navigateToItemByUrl(resolved.workspace, resolved.uri);
                                return false;
                            });
                        });
                    })(innerValue, item);

                    ul.append(item);
                    foundElements++;
                }
            }

            if (foundElements === 0) {
                ul = $("<em>No items</em>");
            }

            this._list.append(ul);
        } else {
            if (!Array.isArray(value)) {
                value = [];
            }

            const table = $("<table><tbody></tbody></table>");
            this._list.append(table);

            let fields = this.getFieldDefinitions();

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
                                isReadOnly: true,
                                itemUrl: innerValue.uri,
                                configuration: {},
                                form: tthis.form
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
                                    property: tthis.propertyName,
                                    referenceUri: innerValue.uri,
                                    referenceWorkspaceId: innerValue.workspace
                                })
                                .then(() => {
                                    tthis.reloadValuesFromServer()
                                });
                        });

                    tr.append(deleteCell);

                    table.append(tr);
                }
            }

            const attachItem = $("<div><btn class='btn btn-secondary dm-subelements-appenditem-btn'>Attach Item</btn><div class='dm-subelements-appenditem-box'></div></div>");
            $(".dm-subelements-appenditem-btn", attachItem).on('click', () => {
                const containerDiv = $(".dm-subelements-appenditem-box", attachItem);
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
                            property: tthis.propertyName,
                            referenceUri: selectedItem.uri,
                            referenceWorkspaceId: selectItem.getUserSelectedWorkspace()
                        }
                    ).then(() => {
                        this.reloadValuesFromServer();
                    });
                };

                selectItem.init(containerDiv, settings);

                return false;
            });

            this._list.append(attachItem);

            const newItem = $("<div><btn class='btn btn-secondary dm-subelements-appenditem-btn'>Create Item</btn></div>");
            newItem.on('click', () => {
                document.location.href =
                    Settings.baseUrl +
                    "ItemAction/Extent.CreateItemInProperty?workspace=" +
                    encodeURIComponent(tthis.form.workspace) +
                    "&itemUrl=" +
                    encodeURIComponent(tthis.itemUrl) +
                    /*"&metaclass=" +
                    encodeURIComponent(uri) +*/
                    "&property=" +
                    encodeURIComponent(tthis.propertyName);
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

    reloadValuesFromServer() {
        alert('reloadValuesFromServer is not overridden.');
    }

    // Returns the default definition of a name.
    // This method can be overridden by the right field definitions
    getFieldDefinitions(): Array<DmObject> | undefined {
        return undefined;
    }
}

export class Field extends Control implements IFormField {

    _element: DmObject;
    field: DmObject;

    reloadValuesFromServer(): void {
        const tthis = this;
        const url = this._element.uri;

        ClientItems.getProperty(this.form.workspace, url, this.propertyName).then(
            x => tthis.createDomByValue(x)
        );
    }

    getFieldDefinitions(): Array<DmObject> {
        return this.field.get('form')?.get('field') as Array<DmObject>;
    }

    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        this.propertyName = this.field.get('name');

        if (this.configuration.isNewItem) {
            return $("<em>Element needs to be saved first</em>");
        } else {
            this._element = dmElement;
            const value = dmElement.get(this.propertyName);

            this.createDomByValue(value);

            return this._list
        }
    }

    evaluateDom(dmElement: DmObject) {

    }
}