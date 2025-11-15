import {IFormField} from "./Interfaces.js";
import * as Mof from "../Mof.js";
import {IFormConfiguration} from "../forms/IFormConfiguration.js";
import {IFormNavigation} from "../forms/Interfaces.js";
import {injectNameByObject, injectNameByUri} from "../DomHelper.js";
import * as ClientItem from "../client/Items.js";
import * as SIC from "../controls/SelectItemControl.js";
import * as DomHelper from "../DomHelper.js";
import {ItemWithNameAndId} from "../ApiModels";

export class Control {
    configuration: IFormConfiguration;
    isReadOnly: boolean;

    /** Is connected to the item url of the element being connected to that element */
    itemUrl: string;
    form: IFormNavigation;
    propertyName: string;
    
    /** Defines the field properties */
    field: Mof.DmObject;

    /** Defines whether the field flag to create the selection fields directly 
     * at form creation shall be skipped, even if isSelectionInline is being set.
     * This flag will be set when user has clicked on 'Set'.  */
    inhibitInline: boolean;
    
    referenceSetCall: (selectedItem: ItemWithNameAndId) => Promise<void>;
    
    _list: JQuery;

    /** Initializes a new instance 
     * 
     * @param field This field contains the definition according ReferenceFieldData. It may be undefined, 
     * then no support will be given for the selected item
     * */
    
    constructor(field?: Mof.DmObject) {
        this.field = field;
        this._list = $("<span></span>");
    }

    /** Creates the overall DOM-elements by getting the object */    
    async createDomByValue(value: any): Promise<JQuery<HTMLElement>> {
        this._list.empty();
        const tthis = this;

        const asMofDmObject = value as Mof.DmObject;
        if (this.configuration.isNewItem) {
            // Unfortunately, for non-saved items, the user cannot select a reference since we 
            // will not find the reference again
            const div = $("<em>Element needs to be saved first</em>");
            this._list.append(div);
        } else {
            const isSelectionInline = this.field?.get('isSelectionInline', Mof.ObjectType.Boolean);
            
            if(!isSelectionInline) {
                if ((typeof value !== "object" && typeof value !== "function") || value === null || value === undefined) {
                    const div = $("<div><em class='dm-undefined'>undefined</em></div>");
                    this._list.append(div);
                } else {
                    const div = $("<div />");
                    let _ = injectNameByUri(div, asMofDmObject.workspace, asMofDmObject.uri);
                    this._list.append(div);
                }
            }

            if (!this.isReadOnly) {
                const containerChangeCell = $("<div></div>");
                
                if(!(this.inhibitInline !== true && isSelectionInline)) {
                    const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
                    const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");

                    unsetCell.on('click', () => {
                        ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, tthis.propertyName).then(
                            async () => {
                                await tthis.reloadValuesFromServer();
                            }
                        );
                    });

                    changeCell.on('click', async () => {
                        await this.createSelectFields(containerChangeCell, value);

                        return false;
                    });

                    this._list.append(changeCell);
                    this._list.append(unsetCell);
                }
                else {

                    await this.createSelectFields(containerChangeCell, value);
                }
                
                this._list.append(containerChangeCell);
            }
        }       

        return this._list;
    }

    /** Creates the GUI elements in which the user is capable to select the items to be reference
     * @param containerChangeCell The cell which will contain the GUI elements. This cell will be emptied
     * @param value The value that is currently selected*/
    private async createSelectFields(containerChangeCell: JQuery<HTMLElement>,  value: any) {
        
        const tthis = this;
        containerChangeCell.empty();
        const isSelectionInline = this.field?.get('isSelectionInline', Mof.ObjectType.Boolean);
        
        const selectItem = new SIC.SelectItemControl();
        const settings = new SIC.Settings();
        settings.showWorkspaceInBreadcrumb = true;
        settings.showExtentInBreadcrumb = true;      
        settings.hideButtonRow = isSelectionInline;
        
        // Depending on whether we are having a inline item, we react upon an explicit click via 'set' button
        // or directly while the user is navigating
        const eventType = isSelectionInline ? selectItem.itemClicked : selectItem.itemSelected;
        eventType.addListener(
            async selectedItem => {

                if (tthis.referenceSetCall !== undefined) {
                    await this.referenceSetCall(selectedItem);
                }
                
                await ClientItem.setPropertyReference(
                    tthis.form.workspace,
                    tthis.itemUrl,
                    {
                        property: tthis.propertyName,
                        referenceUri: selectedItem.uri,
                        workspaceId: selectedItem.workspace
                    }
                );

                if (!isSelectionInline) {
                    containerChangeCell.empty();
                    tthis.inhibitInline = true;

                    await this.reloadValuesFromServer();
                }
            });

        await selectItem.initAsync(containerChangeCell, settings);

        if (value !== undefined && value !== null &&
            (typeof value === "object" || typeof value === "function")) {
            const valueAsMofDmObject = value as Mof.DmObject;
            await selectItem.setItemByUri(valueAsMofDmObject.workspace, valueAsMofDmObject.uri);
        } else {
            // No value is selected, so retrieve the default items
            const workspaceId = this.field?.get('defaultWorkspace', Mof.ObjectType.Single);
            const itemUri = this.field?.get('defaultItemUri', Mof.ObjectType.Single);
            if (workspaceId !== undefined && workspaceId !== null) {

                if (itemUri === null || itemUri === undefined) {
                    await selectItem.setWorkspaceById(workspaceId);
                } else {
                    await selectItem.setItemByUri(workspaceId, itemUri);
                }
            } else {
                // If there is no default selection and item has not been pre-selected by the field
                // configuration itself, choose the extent in which the containing element is residing
                await selectItem.setExtentByUri(this.form.workspace, this.form.extentUri);
            }
        }
    }

    async reloadValuesFromServer() {
        alert('reloadValuesFromServer is not overridden.');
    }
}

export class Field extends Control implements IFormField {
    // The information about the field configuration
    field: Mof.DmObject;

    // The element being shown
    element: Mof.DmObject;

    // The name of the field being derived from the field
    fieldName: string;

    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {

        this.element = dmElement;
        this.referenceSetCall = this.callbackSetReference;

        this._list.empty();

        this.fieldName = this.field.get('name');
        let value = dmElement.get(this.fieldName);
        if (Array.isArray(value)) {
            if (value.length === 1) {
                value = value[0];
            } else {
                this._list.append($("<em>The value is an array and not supported by the referencefield</em>"));

                return this._list;
            }
        }

        // Sets the properties being required by the parent class
        this.propertyName = this.fieldName
        this.itemUrl = dmElement.uri;

        if (this.isReadOnly === true) {
            if (value === undefined || value === null) {
                this._list.html("<em class='dm-undefined'>undefined</em>");
            } else if (value.get === undefined) {
                this._list.text(value.toString());
            } else {                
                await DomHelper.injectNameByObject(this._list, value as Mof.DmObject);
            }
        } else {
            return await this.createDomByValue(value);
        }

        return this._list;
    }
    
    async callbackSetReference(selectedElement: ItemWithNameAndId) {
        this.element.set(this.fieldName, Mof.DmObject.createFromItemWithNameAndId(selectedElement));
    }

    async evaluateDom(dmElement: Mof.DmObject) : Promise<void> {        
    }

    async reloadValuesFromServer() {
        let value = await ClientItem.getProperty(this.form.workspace, this.element.uri, this.fieldName);

        if (Array.isArray(value)) {
            if (value.length === 1) {
                value = value[0];
            } else {
                this._list.empty();
                this._list.append($("<em>The value is an array and not supported by the referencefield</em>"));
                return;
            }
        }

        // After the reload, set the element with the value
        this.element.set(this.fieldName, value);

        await this.createDomByValue(value);
    }
}