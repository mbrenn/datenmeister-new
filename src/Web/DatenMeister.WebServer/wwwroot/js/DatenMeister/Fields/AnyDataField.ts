import {BaseField, IFormField} from "../Interfaces.Fields";
import {DmObject} from "../Mof";
import {injectNameByUri} from "../DomHelper";
import * as ClientItem from "../Client.Items"
import * as ClientItems from "../Client.Items"
import * as SIC from "../Forms.SelectItemControl";


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
            if (tthis.isReadOnly) {
                alert('Read-Only Mode');
            }

            tthis.highlightValue();
        });

        this._aCollection.on('click', () => {
            if (tthis.isReadOnly) {
                alert('Read-Only Mode');
            }

            tthis.highlightCollection();
        });

        this._aReference.on('click', () => {
            if (tthis.isReadOnly) {
                alert('Read-Only Mode');
            }

            tthis.highlightReference();
        });

        result.append(headLine);
        result.append(this._domElement);


        const fieldName = this.field.get('name').toString();
        const value = this._element.get(fieldName);

        if (value === null || value === undefined) {
            this.highlightReference();
        } else if ((typeof value === "object" || typeof value === "function") && (value !== null)) {
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

    private updateDomContent() {
        this._domElement.empty();
        const fieldName = this.field.get('name').toString();
        const value = this._element.get(fieldName);

        /* Otherwise just create the correct field type. */
        if (this.isReadOnly) {
            this.updateDomContentReadOnly(value);
        } else {
            this.updateDomContentEditable(value);
        }
    }

    private updateDomContentReadOnly(value) {
        if (value === null || value === undefined) {
            const div = $("<div><em>null</em></null>");
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
            const div = $("<div><em>Collections not supported</div>");
            this._domElement.append(div);
        }
    }

    private updateDomContentEditable(value) {

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
                        ClientItems.addReferenceToCollection(
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
            const div = $("<div><em>Collections not supported</div>");
            this._domElement.append(div);
        }
    }
    
}