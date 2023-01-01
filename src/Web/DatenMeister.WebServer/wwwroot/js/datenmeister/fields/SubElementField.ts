import {IFormField} from "./Interfaces";
import {DmObject, ObjectType} from "../Mof";
import * as FieldFactory from "../forms/FieldFactory";
import * as SIC from "../controls/SelectItemControl";
import * as ClientItems from "../client/Items";
import * as ClientTypes from "../client/Types";
import {IFormConfiguration} from "../forms/IFormConfiguration";
import {IFormNavigation} from "../forms/Interfaces";
import * as Settings from "../Settings";
import {IInjectNameByUriParams, injectNameByUri} from "../DomHelper";
import {_DatenMeister} from "../models/DatenMeister.class";
import * as TypeSelectionControl from "../controls/TypeSelectionControl";
import {ItemWithNameAndId} from "../ApiModels";
import {moveItemInCollectionDown, moveItemInCollectionUp} from "../client/Actions.Items";
import * as FormActions from "../FormActions";
import * as Navigator from '../Navigator'

export class Control {
    configuration: IFormConfiguration;
    isReadOnly: boolean;

    // Is connected to the item url of the element being connected to that element
    itemUrl: string;
    form: IFormNavigation;
    propertyName: string;
    /**
     * The name of the action that shall be executed when the user clicks on the item
     */
    itemActionName: string;

    /**
     * Stores the property type. This information is used to pre-select the
     * SubElementField in which the user can define the metaclass for a element to be created
     */
    propertyType: ItemWithNameAndId;

    _list: JQuery;

    constructor() {
        this._list = $("<div></div>");
    }

    async createDomByFieldValue(fieldValue: any): Promise<JQuery<HTMLElement>> {

        const tthis = this;
        this._list.empty();       

        if (this.isReadOnly) {
            if (!Array.isArray(fieldValue)) {
                return $("<div><em>Element is not an Array</em></div>");
            }

            let ul = $("<ul class='list-unstyled'></ul>");

            let foundElements = 0;
            for (let m in fieldValue) {
                if (Object.prototype.hasOwnProperty.call(fieldValue, m)) {
                    let innerValue = fieldValue[m] as DmObject;
                    const item = $("<li></li>");
                    
                    const injectParams: IInjectNameByUriParams = {};
                    if (this.itemActionName !== undefined) {
                        injectParams.onClick = async x =>  {
                            const readObject = await ClientItems.getObjectByUri(x.workspace, x.uri);
                            await FormActions.execute(this.itemActionName, tthis.form, readObject);
                            return false;
                        }
                    }

                    let _ = injectNameByUri(item, innerValue.workspace, innerValue.uri, injectParams);
                    ul.append(item);
                    foundElements++;
                }
            }

            if (foundElements === 0) {
                ul = $("<em>No items</em>");
            }

            this._list.append(ul);
        } else {
            if (!Array.isArray(fieldValue)) {
                fieldValue = [];
            }

            const table = $("<table><tbody></tbody></table>");
            this._list.append(table);

            let fields = this.getFieldDefinitions();

            let fieldsData = new Array<DmObject>();
            if (fields === undefined) {
                const nameField = new DmObject();
                nameField.setMetaClassByUri(_DatenMeister._Forms.__TextFieldData_Uri);
                nameField.set("name", "name");
                nameField.set("title", "Name");
                nameField.set("isReadOnly", true);
                fieldsData.push(nameField);
            }

            /* Creates the table header */
            const tBody = $("tbody", table);
            const tr = $("<tr></tr>");
            for (let fieldDataKey in fieldsData) {
                let fieldData = fieldsData[fieldDataKey];

                let header = $("<th></th>");
                header.text(fieldData.get("title"));
                tr.append(header);
            }

            let deleteHeader = $("<th>Actions</th>");
            tr.append(deleteHeader);

            tBody.append(tr);

            /* Creates the rows */
            for (let m in fieldValue) {
                if (Object.prototype.hasOwnProperty.call(fieldValue, m)) {
                    const tr = $("<tr></tr>");
                    let innerValue = fieldValue[m] as DmObject;

                    for (let fieldDataKey in fieldsData) {
                        const td = $("<td></td>");
                        let fieldData = fieldsData[fieldDataKey];

                        const field = FieldFactory.createField(
                            fieldData.metaClass.uri,
                            {
                                field: fieldData,
                                isReadOnly: true,
                                itemUrl: innerValue.uri,
                                configuration: {},
                                form: tthis.form
                            });
                        const dom = await field.createDom(innerValue);
                        td.append(dom);
                        tr.append(td);
                    }

                    /* Creates the delete button */
                    const moveUp = $("<btn class='btn btn-secondary dm-item-moveup-button'>⬆️</btn>");
                    const moveDown = $("<btn class='btn btn-secondary dm-item-movedown-button'>⬇️</btn>");
                    
                    moveUp.on("click",
                        async () => {
                            await moveItemInCollectionUp(this.form.workspace, this.itemUrl, this.propertyName, innerValue.uri);
                            await this.reloadValuesFromServer();
                    });
                    moveDown.on("click",
                        async () => {
                            await moveItemInCollectionDown(this.form.workspace, this.itemUrl, this.propertyName, innerValue.uri);
                            await this.reloadValuesFromServer();
                        });

                    /* Creates the delete button */
                    let deleteCell = $("<td><btn class='btn btn-secondary'>Delete</btn></td>");
                    $("btn", deleteCell).on("click",
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
                    deleteCell.append(moveUp);
                    deleteCell.append(moveDown);

                    table.append(tr);
                }
            }

            const attachItem = $("<div>" +
                "<div>" +
                "<btn class='btn btn-secondary dm-subelements-attachitem-btn'>Attach Item</btn>" +
                "<btn class='btn btn-secondary dm-subelements-createitem-btn'>Create Item</btn>" +
                "</div>" +
                "<div class='dm-subelements-attachitem-box'></div>" +
                "<div class='dm-subelements-createitem-box'></div>" +
                "</div>");
            $(".dm-subelements-attachitem-btn", attachItem).on("click", () => {
                const containerDiv = $(".dm-subelements-attachitem-box", attachItem);
                containerDiv.empty();
                const selectItem = new SIC.SelectItemControl();
                const settings = new SIC.Settings();
                settings.showWorkspaceInBreadcrumb = true;
                settings.showExtentInBreadcrumb = true;
                selectItem.itemSelected.addListener(
                    selectedItem => {
                        ClientItems.addReferenceToCollection(
                            tthis.form.workspace,
                            tthis.itemUrl,
                            {
                                property: tthis.propertyName,
                                referenceUri: selectedItem.uri,
                                workspaceId: selectItem.getUserSelectedWorkspaceId()
                            }
                        ).then(() => {
                            this.reloadValuesFromServer();
                        });
                    });

                selectItem.init(containerDiv, settings);

                return false;
            });

            $(".dm-subelements-createitem-btn", attachItem).on("click", async () => {
                const container = $(".dm-subelements-createitem-box", attachItem);
                container.empty();
                
                // Create the Type Selection Control element in which the user can select the right 
                const control = new TypeSelectionControl.TypeSelectionControl(container);

                // Get the property type
                if(this.propertyType !== undefined) {
                    control.setCurrentTypeUrl(this.propertyType);
                }
                
                control.typeSelected.addListener(async x => {

                    if (x === undefined ||
                        x.selectedType === undefined ||
                        x.selectedType.uri === undefined) {
                        alert("Nothing is selected.");
                        return;
                    }

                    document.location.href = Navigator.getLinkForNavigateToCreateItemInProperty(
                        tthis.form.workspace, tthis.itemUrl, x.selectedType.uri, tthis.propertyName);
                });
                
                await control.createControl();
            });

            this._list.append(attachItem);
        }

        const refreshBtn = $("<div><btn class='dm-subelements-refresh-btn'><img src='/img/refresh-16.png' alt='Refresh' /></btn></div>");
        $(".dm-subelements-refresh-btn", refreshBtn).on("click", () => {
            tthis.reloadValuesFromServer();
        });

        this._list.append(refreshBtn);

        return this._list;
    }

    reloadValuesFromServer() {
        alert("reloadValuesFromServer is not overridden.");
    }

    /**
     * Returns the default definition of a name.
     * method can be overridden by the right field definitions
     */
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
            x => tthis.createDomByFieldValue(x)
        );
    }

    getFieldDefinitions(): Array<DmObject> {
        return this.field.get("form", ObjectType.Single)?.get("field", ObjectType.Array) as Array<DmObject>;
    }

    async createDom(dmElement: DmObject) {
        this.propertyName = this.field.get(_DatenMeister._Forms._ActionFieldData._name_);
        this.itemActionName = this.field.get(_DatenMeister._Forms._ActionFieldData.actionName);        

        if (this.configuration.isNewItem) {
            return $("<em>Element needs to be saved first</em>");
        } else {
            this._element = dmElement;
            const value = dmElement.get(this.propertyName);

            if (this._element.metaClass?.uri !== undefined
                && this.propertyName !== undefined
                && !this.isReadOnly) {
                this.propertyType =
                    await ClientTypes.getPropertyType(
                        this._element.metaClass.workspace,
                        this._element.metaClass.uri,
                        this.propertyName);
            }

            await this.createDomByFieldValue(value);

            return this._list
        }
    }

    evaluateDom(dmElement: DmObject) {

    }
}