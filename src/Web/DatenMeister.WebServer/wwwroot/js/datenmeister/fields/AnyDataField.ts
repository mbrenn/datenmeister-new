import {BaseField, IFormField} from "../Interfaces.Fields";
import {DmObject} from "../Mof";
import {injectNameByUri} from "../DomHelper";
import * as ClientItem from "../Client.Items"
import * as SIC from "../Forms.SelectItemControl";
import * as SubElementField from "./SubElementField"


enum ModeValue {
    Value,
    Collection,
    Reference
}

export class Field extends BaseField implements IFormField {

    _textBox: JQuery<HTMLElement>;
    private _aValue: JQuery;
    private _aCollection: JQuery;
    private _aReference: JQuery;

    private _mode: ModeValue;
    private _element: DmObject;
    private _domElement: JQuery<HTMLElement>;
    private _fieldValue: any;

    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        const tthis = this;
        this._element = dmElement;

        const result = $("<div>");

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

        if (this._fieldValue === null || this._fieldValue === undefined) {
            this.highlightReference();
        } else if ((typeof this._fieldValue === "object" || typeof this._fieldValue === "function") && (this._fieldValue !== null)) {
            this.highlightReference();
        } else {
            this.highlightValue();
        }

        return result;
    }

    evaluateDom(dmElement: DmObject) {
        if (this._mode === ModeValue.Value) {
            if (this._textBox !== undefined && this._textBox !== null) {
                const fieldName = this.field.get('name').toString();
                dmElement.set(fieldName, this._textBox.val());
            }
        }
    }

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
    
    private reloadAndUpdateDomContent() {
        const tthis = this;
        ClientItem.getProperty(
            this.form.workspace,
            this.itemUrl,
            this.field.get('name').toString()
        ).done((item) => {
            tthis._fieldValue = item;
            tthis.updateDomContent();
        });
    }

    private updateDomContent() {
        this._domElement.empty();

        /* Otherwise just create the correct field type. */
        if (this.isReadOnly) {
            this.updateDomContentReadOnly();
        } else {
            this.updateDomContentEditable();
        }
    }

    private updateDomContentReadOnly() {
        const value = this._fieldValue;
        if (value === null || value === undefined) {
            const div = $("<div><em>Not set</em></null>");
            this._domElement.append(div);
        } else if (this._mode === ModeValue.Reference) {
            const asDmObject = value as DmObject;
            const div = $("<div />");
            injectNameByUri(div, asDmObject.workspace, asDmObject.uri);
            this._domElement.append(div);
        } else if (this._mode === ModeValue.Value) {
            const div = $("<div />");
            div.text(value?.toString() ?? "");
            this._domElement.append(div);
        } else if (this._mode === ModeValue.Collection) {
            const field = this.createSubElementFieldInstance();
            const element = field.createDomByValue(value);
            
            this._domElement.append(element);
        }
    }

    private updateDomContentEditable() {
        const value = this._fieldValue;
        const fieldName = this.field.get('name').toString();

        var tthis = this;
        if (this._mode === ModeValue.Reference) {
            if (value === null || value === undefined) {
                const div = $("<div><em>null</em></null>");
                this._domElement.append(div);
            } else {
                const asDmObject = value as DmObject;
                const div = $("<div />");
                injectNameByUri(div, asDmObject.workspace, asDmObject.uri);
                this._domElement.append(div);
            }

            if (this.configuration.isNewItem) {
                const div = $("<em>Element needs to be saved first</em>");
                this._domElement.append(div);
            } else {
                const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
                const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
                const containerChangeCell = $("<div></div>");

                unsetCell.on('click', () => {
                    ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, fieldName).done(
                        () => {
                            tthis.updateDomContent();
                        }
                    );
                });

                changeCell.on('click', () => {
                    containerChangeCell.empty();
                    const selectItem = new SIC.SelectItemControl();
                    const settings = new SIC.Settings();
                    settings.showWorkspaceInBreadcrumb = true;
                    settings.showExtentInBreadcrumb = true;
                    selectItem.onItemSelected = selectedItem => {
                        ClientItem.addReferenceToCollection(
                            tthis.form.workspace,
                            tthis.itemUrl,
                            {
                                property: tthis.field.get('name'),
                                referenceUri: selectedItem.uri,
                                referenceWorkspaceId: selectItem.getUserSelectedWorkspace()
                            }
                        ).done(() => {
                            this.updateDomContent();
                        });
                    };

                    selectItem.init(containerChangeCell, settings);

                    return false;
                });

                this._domElement.append(changeCell);
                this._domElement.append(unsetCell);
            }
        } else if (this._mode === ModeValue.Value) {
            this._textBox = $("<input />");
            this._textBox.val(value?.toString() ?? "");
            this._domElement.append(this._textBox)
        } else if (this._mode === ModeValue.Collection) {
            if (this.configuration.isNewItem) {
                const div = $("<em>Element needs to be saved first</em>");
                this._domElement.append(div);
            } else {
                const field = this.createSubElementFieldInstance();
                const element = field.createDomByValue(value);
                this._domElement.append(element);
            }
        }
    } 
    
    private createSubElementFieldInstance() {
        const element = new SubElementField.Control();
        element.isReadOnly = this.isReadOnly;
        element.configuration = this.configuration;
        element.itemUrl = this.itemUrl;
        element.propertyName = this.field.get('name').toString();
        
        return element;
    }
}