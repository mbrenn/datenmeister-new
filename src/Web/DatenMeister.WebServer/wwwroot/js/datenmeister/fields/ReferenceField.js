import { ObjectType } from "../Mof.js";
import { injectNameByUri } from "../DomHelper.js";
import * as ClientItem from "../client/Items.js";
import * as SIC from "../controls/SelectItemControl.js";
export class Control {
    /** Initializes a new instance
     *
     * @param field This field contains the definition according ReferenceFieldData. It may be undefined,
     * then no support will be given for the selected item
     * */
    constructor(field) {
        this.field = field;
        this._list = $("<span></span>");
    }
    /** Creates the overall DOM-elements by getting the object */
    async createDomByValue(value) {
        this._list.empty();
        const tthis = this;
        const asDmObject = value;
        if (this.configuration.isNewItem) {
            // Unfortunately, for non-saved items, the user cannot select a reference since we 
            // will not find the reference again
            const div = $("<em>Element needs to be saved first</em>");
            this._list.append(div);
        }
        else {
            if ((typeof value !== "object" && typeof value !== "function") || value === null || value === undefined) {
                const div = $("<div><em class='dm-undefined'>undefined</em></div>");
                this._list.append(div);
            }
            else {
                const div = $("<div />");
                injectNameByUri(div, asDmObject.workspace, asDmObject.uri);
                this._list.append(div);
            }
            if (!this.isReadOnly) {
                const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
                const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
                const containerChangeCell = $("<div></div>");
                unsetCell.on('click', () => {
                    ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, tthis.propertyName).then(async () => {
                        await tthis.reloadValuesFromServer();
                    });
                });
                changeCell.on('click', async () => {
                    await this.createSelectFields(containerChangeCell, value);
                    return false;
                });
                this._list.append(changeCell);
                this._list.append(unsetCell);
                this._list.append(containerChangeCell);
                // Checks, whether the Drop-Down Field shall be completely pre-created
                if (this.inhibitInline !== true &&
                    this.field?.get('isSelectionInline', ObjectType.Boolean) === true) {
                    await this.createSelectFields(containerChangeCell, value);
                }
            }
        }
        return this._list;
    }
    /** Creates the GUI elements in which the user is capable to select the items to be reference
     * @param containerChangeCell The cell which will contain the GUI elements. This cell will be emptied
     * @param value The value that is currently selected*/
    async createSelectFields(containerChangeCell, value) {
        const tthis = this;
        containerChangeCell.empty();
        const selectItem = new SIC.SelectItemControl();
        const settings = new SIC.Settings();
        settings.showWorkspaceInBreadcrumb = true;
        settings.showExtentInBreadcrumb = true;
        selectItem.itemSelected.addListener(async (selectedItem) => {
            await ClientItem.setPropertyReference(tthis.form.workspace, tthis.itemUrl, {
                property: tthis.propertyName,
                referenceUri: selectedItem.uri,
                workspaceId: selectedItem.workspace
            });
            containerChangeCell.empty();
            tthis.inhibitInline = true;
            await this.reloadValuesFromServer();
        });
        await selectItem.initAsync(containerChangeCell, settings);
        if (value !== undefined && value !== null &&
            (typeof value === "object" || typeof value === "function")) {
            const valueAsDmObject = value;
            await selectItem.setItemByUri(valueAsDmObject.workspace, valueAsDmObject.uri);
        }
        else {
            // No value is selected, so retrieve the default items
            const workspaceId = this.field?.get('defaultWorkspace', ObjectType.Single);
            const itemUri = this.field?.get('defaultItemUri', ObjectType.Single);
            if (workspaceId !== undefined && workspaceId !== null) {
                if (itemUri === null || itemUri === undefined) {
                    await selectItem.setWorkspaceById(workspaceId);
                }
                else {
                    await selectItem.setItemByUri(workspaceId, itemUri);
                }
            }
        }
    }
    async reloadValuesFromServer() {
        alert('reloadValuesFromServer is not overridden.');
    }
}
export class Field extends Control {
    async createDom(dmElement) {
        this.element = dmElement;
        this._list.empty();
        this.fieldName = this.field.get('name');
        let value = dmElement.get(this.fieldName);
        if (Array.isArray(value)) {
            if (value.length === 1) {
                value = value[0];
            }
            else {
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
            }
            else {
                this._list.text(value.get('name'));
            }
        }
        else {
            return await this.createDomByValue(value);
        }
        return this._list;
    }
    async evaluateDom(dmElement) {
    }
    async reloadValuesFromServer() {
        let value = await ClientItem.getProperty(this.form.workspace, this.element.uri, this.fieldName);
        if (Array.isArray(value)) {
            if (value.length === 1) {
                value = value[0];
            }
            else {
                this._list.empty();
                this._list.append($("<em>The value is an array and not supported by the referencefield</em>"));
                return;
            }
        }
        await this.createDomByValue(value);
    }
}
//# sourceMappingURL=ReferenceField.js.map