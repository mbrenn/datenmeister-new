import {BaseField, IFormField} from "./Interfaces.js";
import {DmObject} from "../Mof.js";
import {injectNameByUri} from "../DomHelper.js";
import * as ClientItem from "../client/Items.js"
import * as SIC from "../controls/SelectItemControl.js";
import * as SubElementField from "./SubElementField.js";
import * as ReferenceField from "./ReferenceField.js";


enum ModeValue {
    Value,
    Collection,
    Reference
}

export class Field extends BaseField implements IFormField {

    _textBox: JQuery<HTMLElement>;
    
    // The texts on which the user can click to modify the type of the element
    private _aValue: JQuery;
    private _aCollection: JQuery;
    private _aReference: JQuery;

    private _mode: ModeValue;
    
    // This is the element containing the property
    private _element: DmObject;
    private _domElement: JQuery<HTMLElement>;
    
    // This is the element describing the property value; the element that shall be shown in this
    private _fieldValue: any;

    // Creates the overall DOM
    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
        const tthis = this;
        this._element = dmElement;

        const result = $("<div>");

        // Creates the headline in which the user can select which type of object he wants to add
        const headLine = $(
            "<div class='dm-anydatafield-headline'><a class='dm-anydatafield-headline-value'>Value</a> " +
            "| <a class='dm-anydatafield-headline-collection'>Collection</a> " +
            "| <a class='dm-anydatafield-headline-reference'>Reference</a></div>");

        this._aValue = $(".dm-anydatafield-headline-value", headLine);
        this._aCollection = $(".dm-anydatafield-headline-collection", headLine);
        this._aReference = $(".dm-anydatafield-headline-reference", headLine);

        this._domElement = $("<div></div>");

        this._aValue.on('click', () => {
            tthis.highlightValue();
        });

        this._aCollection.on('click', () => {
            tthis.highlightCollection();
        });

        this._aReference.on('click', () => {
            tthis.highlightReference();
        });

        result.append(headLine);
        result.append(this._domElement);

        const fieldName = this.field.get('name').toString();
        this._fieldValue = this._element.get(fieldName);

        // Selects depending on the found type. May be an array, a reference or a value
        if (this._fieldValue === null || this._fieldValue === undefined) {
            this.highlightReference();
        } else if ((typeof this._fieldValue === "object" || typeof this._fieldValue === "function")) {
            this.highlightReference();
        } else {
            this.highlightValue();
        }

        return result;
    }
    async evaluateDom(dmElement: DmObject) : Promise<void> {
        if (this._mode === ModeValue.Value) {
            if (this._textBox !== undefined && this._textBox !== null) {
                const fieldName = this.field.get('name').toString();
                dmElement.set(fieldName, this._textBox.val());
            }
        }
    }

    /*
     * These helper methods are to be called when one of the three types were selected and 
     * shall be switched to
     */
    private highlightValue() {
        this._aValue.addClass('active');
        this._aCollection.removeClass('active');
        this._aReference.removeClass('active');

        this._mode = ModeValue.Value;
        this.updateDomContent();
    }

    private highlightCollection() {
        this._aValue.removeClass('active');
        this._aCollection.addClass('active');
        this._aReference.removeClass('active');

        this._mode = ModeValue.Collection;
        this.updateDomContent();
    }

    private highlightReference() {
        this._aValue.removeClass('active');
        this._aCollection.removeClass('active');
        this._aReference.addClass('active');

        this._mode = ModeValue.Reference;
        this.updateDomContent();
    }

    private async reloadAndUpdateDomContent() {
        const tthis = this;
        tthis._fieldValue = await ClientItem.getProperty(
            this.form.workspace,
            this.itemUrl,
            this.field.get('name').toString()
        );
        
        await tthis.updateDomContent();
    }
    
    // Performs a 'reload' of the complete DOM
    private async updateDomContent() {
        this._domElement.empty();

        /* Otherwise just create the correct field type. */
        if (this.isReadOnly) {
            await this.updateDomContentReadOnly();
        } else {
            await this.updateDomContentEditable();
        }
    }

    // Rebuilds the BOM in the read-only mode
    private async updateDomContentReadOnly() {
        let value = this._fieldValue;
        if (value === null || value === undefined
            || (this._mode === ModeValue.Reference && (typeof value !== "object" && typeof value !== "function"))) {
            const div = $("<div><em class='dm-undefined'>Undefined</em></div>");
            this._domElement.append(div);
        } else if (this._mode === ModeValue.Reference) {
            if(Array.isArray(value)) {
                value = value[0];
            }
            
            const field = this.createReferenceFieldInstance();            
            const element = await field.createDomByValue(value);
            this._domElement.append(element);
        } else if (this._mode === ModeValue.Value) {
            const div = $("<div />");
            div.text(value?.toString() ?? "");
            this._domElement.append(div);
        } else if (this._mode === ModeValue.Collection) {
            const field = this.createSubElementFieldInstance();
            const element = await field.createDomByFieldValue(value);
            this._domElement.append(element);
        }
    }

    // Rebuilds the DOM in the edit mode
    private async updateDomContentEditable() {
        if (this._mode === ModeValue.Reference) {
            const tthis = this;

            let value = this._fieldValue as DmObject;
            if(Array.isArray(value)) {
                value = value[0] as DmObject;
            }
            
            const fieldName = this.field.get('name').toString();
            if ((typeof value !== "object" && typeof value !== "function") || value === null || value === undefined) {
                // Nothing is selected... ==> Null value
                const div = $("<div><em class='dm-undefined'>undefined</em></null>");
                this._domElement.append(div);
            } else {
                // An element is selected. The name can be shown
                const asDmObject = value as DmObject;
                const div = $("<div />");
                injectNameByUri(div, asDmObject.workspace, asDmObject.uri);
                this._domElement.append(div);
            }

            if (this.configuration.isNewItem) {
                // If we are having a new element, the element needs to be saved, so we 
                // have an element id which can be used to reference this element
                const div = $("<em>Element needs to be saved first</em>");
                this._domElement.append(div);
            } else {
                // If we are having a reference to the containing element
                // create the buttons to change and unset the property
                const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
                const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
                const containerChangeCell = $("<div></div>");

                unsetCell.on('click', async () => {
                    // Unsets the property and close
                    await ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, fieldName);
                    tthis.updateDomContent();
                });

                changeCell.on('click', async () => {
                    // The user wants to select a new one
                    containerChangeCell.empty();
                    const selectItem = new SIC.SelectItemControl();
                    const settings = new SIC.Settings();
                    settings.showWorkspaceInBreadcrumb = true;
                    settings.showExtentInBreadcrumb = true;
                    settings.hideAtStartup = true;
                    selectItem.itemSelected.addListener(
                        async selectedItem => {
                            await ClientItem.setPropertyReference(
                                tthis.form.workspace,
                                tthis.itemUrl,
                                {
                                    property: tthis.field.get('name'),
                                    referenceUri: selectedItem.uri,
                                    workspaceId: selectItem.getUserSelectedWorkspaceId()
                                }
                            );

                            await tthis.reloadAndUpdateDomContent();
                        });

                    containerChangeCell.empty();
                    await selectItem.initAsync(containerChangeCell, settings);

                    // Sets the item, if defined
                    if (value?.workspace !== undefined && value.uri !== undefined) {
                        await selectItem.setItemByUri(value.workspace, value.uri);

                    } else {
                        // Sets the workspace, if defined
                        if (value?.workspace !== undefined) {
                            await selectItem.setWorkspaceById(value.workspace);
                        } else if (this._element?.workspace !== undefined) {
                            await selectItem.setWorkspaceById(tthis._element.workspace);
                        }

                        // Sets the extent, if defined
                        if (value?.extentUri !== undefined) {
                            await selectItem.setWorkspaceById(value.workspace);
                        } else if (this._element?.extentUri !== undefined && this._element.workspace !== undefined) {
                            await selectItem.setExtentByUri(this._element.workspace, tthis._element.extentUri);
                        }
                    }
                    
                    selectItem.showControl();

                    return false;
                });

                this._domElement.append(changeCell);
                this._domElement.append(unsetCell);
                this._domElement.append(containerChangeCell);
            }
        } else if (this._mode === ModeValue.Value) {
            const value = this._fieldValue;
            this._textBox = $("<input />");
            this._textBox.val(value?.toString() ?? "");
            this._domElement.append(this._textBox);
        } else if (this._mode === ModeValue.Collection) {

            const value = this._fieldValue;            
            if (this.configuration.isNewItem) {
                const div = $("<em>Element needs to be saved first</em>");
                this._domElement.append(div);
            } else {
                const field = this.createSubElementFieldInstance();
                const element = await field.createDomByFieldValue(value);
                this._domElement.append(element);
            }
        }
    }

    private createReferenceFieldInstance() {
        const element = new ReferenceField.Control();
        this.cloneField(element);

        return element;
    }

    private createSubElementFieldInstance() {
        const element = new SubElementField.Control();
        this.cloneField(element);

        return element;
    }

    private cloneField(element: ReferenceField.Control | SubElementField.Control) {
        element.isReadOnly = this.isReadOnly;
        element.configuration = this.configuration;
        element.itemUrl = this.itemUrl;
        element.propertyName = this.field.get('name').toString();
        element.form = this.form;
    }
}