import {BaseField, IFormField} from "./Interfaces";
import {DmObject} from "../Mof";
import {IFormConfiguration} from "../forms/IFormConfiguration";
import {IForm, IFormNavigation} from "../forms/Interfaces";
import {injectNameByUri} from "../DomHelper";
import * as ClientItem from "../client/Items";
import * as SIC from "../controls/SelectItemControl";

export class Control {
    configuration: IFormConfiguration;
    isReadOnly: boolean;

    // Is connected to the item url of the element being connected to that element
    itemUrl: string;
    form: IFormNavigation;
    propertyName: string;

    _list: JQuery;

    constructor() {
        this._list = $("<span></span>");
    }

    createDomByValue(value: any): JQuery<HTMLElement> {
        this._list.empty();
        const tthis = this;

        if ((typeof value !== "object" && typeof value !== "function") || value === null || value === undefined) {
            const div = $("<div><em>undefined</em></null>");
            this._list.append(div);
        }

        const asDmObject = value as DmObject;
        if (this.configuration.isNewItem) {
            const div = $("<em>Element needs to be saved first</em>");
            this._list.append(div);
        } else if (this.isReadOnly) {
            const div = $("<div />");
            injectNameByUri(div, this.form.workspace, asDmObject.uri);
            this._list.append(div);
        } else {
            const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
            const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
            const containerChangeCell = $("<div></div>");

            unsetCell.on('click', () => {
                ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, tthis.propertyName).then(
                    () => {
                        tthis.reloadValuesFromServer();
                    }
                );
            });

            changeCell.on('click', () => {
                containerChangeCell.empty();
                const selectItem = new SIC.SelectItemControl();
                const settings = new SIC.Settings();
                settings.showWorkspaceInBreadcrumb = true;
                settings.showExtentInBreadcrumb = true;
                selectItem.itemSelected.addListener(
                    selectedItem => {
                        ClientItem.addReferenceToCollection(
                            tthis.form.workspace,
                            tthis.itemUrl,
                            {
                                property: tthis.propertyName,
                                referenceUri: selectedItem.uri,
                                workspaceId: selectItem.getUserSelectedWorkspace()
                            }
                        ).then(() => {
                            this.reloadValuesFromServer();
                        });
                    });

                selectItem.init(containerChangeCell, settings);

                return false;
            });

            this._list.append(changeCell);
            this._list.append(unsetCell);
        }

        return this._list;
    }

    reloadValuesFromServer() {
        alert('reloadValuesFromServer is not overridden.');
    }
}

export class Field extends Control implements IFormField {
    field: DmObject;

    createDom(dmElement: DmObject): JQuery<HTMLElement> {

        this._list.empty();

        const fieldName = this.field.get('name');
        let value = dmElement.get(fieldName);
        if (Array.isArray(value)) {
            if (value.length === 1) {
                value = value[0];
            } else {
                this._list.append($("<em>The value is an array and not supported by the referencefield</em>"));

                return this._list;
            }
        }

        if (this.isReadOnly === true) {
            if (value === undefined) {
                this._list.html("<em>undefined</em>");
            } else {
                this._list.text(value.get('name'));
            }
        } else {

            return this.createDomByValue(value);
        }

        return this._list;
    }

    evaluateDom(dmElement: DmObject) {

    }
}
