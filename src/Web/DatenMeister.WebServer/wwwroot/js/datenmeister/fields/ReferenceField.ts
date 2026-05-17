import {BaseField, IFormField} from "./Interfaces.js";
import * as Mof from "../Mof.js";
import * as DomHelper from "../DomHelper.js";
import {injectNameByUri} from "../DomHelper.js";
import * as ClientItem from "../client/Items.js";
import * as SIC from "../controls/SelectItemControl.js";
import * as ApiModels from "../ApiModels.js";
import {ItemWithNameAndId} from "../ApiModels.js";
import * as Settings from "../Settings.js";
import * as Uml from "../models/UML.js"

export class Control extends BaseField {
    propertyName: string;

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
        super();
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
                    const createCell = $("<btn class='btn btn-secondary'>Create</btn>");
                    const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
                    const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");

                    createCell.on('click', async () => {
                        // Now it is getting a bit difficult. We have to perform several actions here 
                        // 1) First, let the user decide on which package, the new item shall be created
                        // 2) Let the user decide which type shall be created
                        // 3) In case, the user has done this, we need to create the item
                        // 4) We need to set the reference of the newly created item
                        // 5) Reload the control, so the user can decide whether to continue editing the current item
                        //    or switch to the reference to edit the new item. 
                        
                        // 1) Let the user decide on which package the new item shall be created
                        const packageItem = await this.selectPackage(containerChangeCell);
                        
                        // 2) Define the type of the new item
                        const typeItem = await this.selectType(containerChangeCell);
                        
                        // 3) Let's create the child
                        const result = await ClientItem.addToContainer(packageItem.workspace, packageItem.uri,
                            {
                                metaClass: typeItem.uri
                            });
                        
                        // 4) Let's set the reference
                        if (tthis.referenceSetCall !== undefined) {
                            await this.referenceSetCall(
                                {
                                    workspace: result.workspace,
                                    uri: result.itemUri
                                }
                            );
                        }
                        await ClientItem.setPropertyReference(tthis.form.workspace, tthis.itemUrl, 
                            {
                                property: tthis.propertyName, 
                                referenceUri: result.itemUri, 
                                workspaceId: result.workspace
                            });
                        
                        await tthis.reloadValuesFromServer();                        
                        
                        return false;
                    });

                    changeCell.on('click', async () => {
                        await this.createSelectFields(containerChangeCell, value);

                        return false;
                    });

                    unsetCell.on('click', () => {
                        ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, tthis.propertyName).then(
                            async () => {
                                await tthis.reloadValuesFromServer();
                            }
                        );
                    });

                    this._list.append(createCell);
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

    /**
     * Allows the selection of a package.
     * Resolves with the selected item's link details or rejects if no valid selection is made.
     *
     * @param {JQuery} changeContainerCell - The container element where the selection control will be initialized.
     * @return {Promise<ApiModels.ItemLink>} A promise that resolves with the selected item link containing its workspace and URI, or rejects if no item is selected.
     */
    private selectPackage(changeContainerCell: JQuery) : Promise<ApiModels.ItemLink> {

        return this.selectItem(changeContainerCell, { workspaceId: Settings.WorkspaceData, title: "Select Package in which the item shall be created:" });
    }

    /**
     * Allows the selection of a type
     * Resolves with the selected item's link details or rejects if no valid selection is made.
     *
     * @param {JQuery} changeContainerCell - The container element where the selection control will be initialized.
     * @return {Promise<ApiModels.ItemLink>} A promise that resolves with the selected item link containing its workspace and URI, or rejects if no item is selected.
     */
    private selectType(changeContainerCell: JQuery) : Promise<ApiModels.ItemLink> {

        return this.selectItem(changeContainerCell, { workspaceId: Settings.WorkspaceTypes, title: "Select type of new item:" });
    }

    /**
     * Selects an item using a custom selection control and returns the selected item's information.
     *
     * @param changeContainerCell A JQuery object representing the HTML element where the selection control will be initialized.
     * @param parameter The parameters to create the selection control.
     * @return A Promise resolving to an object containing the selected item's workspace and URI, or rejecting if no item is selected.
     */
    private selectItem(changeContainerCell: JQuery<HTMLElement>, parameter: ISelectItemParameter) {
        return new Promise<ApiModels.ItemLink>(async (resolve, reject) => {
            
            const workspaceId = parameter.workspaceId;
            const title = parameter.title;

            changeContainerCell.empty();
            const selectItem = new SIC.SelectItemControl();
            const settings = new SIC.Settings();
            settings.showWorkspaceInBreadcrumb = true;
            settings.showExtentInBreadcrumb = true;
            if(title !== undefined) settings.headline = title;
            await selectItem.setWorkspaceById(workspaceId);
            selectItem.itemSelected.addListener(
                selectedItem => {

                    if (selectedItem === undefined ||
                        selectedItem.uri === undefined) {
                        alert("Nothing is selected.");
                        reject("Nothing is selected");
                        return;
                    }

                    resolve({
                        workspace: selectedItem.workspace,
                        uri: selectedItem.uri
                    });
                });

            selectItem.init(changeContainerCell, settings);
        });
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

                if (this.callbackUpdateField !== undefined) {
                    this.callbackUpdateField();
                }

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
        this.propertyName = this.fieldName;
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

        if (this.callbackUpdateField !== undefined) {
            this.callbackUpdateField();
        }
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

/**
 * Defines the parameters to select items
 */
interface ISelectItemParameter{
    /**
     * Preselected workspace
     */
    workspaceId?: string,
    /**
     * Title of the form
     */
    title?: string
}