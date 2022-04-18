define(["require", "exports", "./Interfaces", "../DomHelper", "../client/Items", "../controls/SelectItemControl", "./SubElementField", "./ReferenceField"], function (require, exports, Interfaces_1, DomHelper_1, ClientItem, SIC, SubElementField, ReferenceField) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    var ModeValue;
    (function (ModeValue) {
        ModeValue[ModeValue["Value"] = 0] = "Value";
        ModeValue[ModeValue["Collection"] = 1] = "Collection";
        ModeValue[ModeValue["Reference"] = 2] = "Reference";
    })(ModeValue || (ModeValue = {}));
    class Field extends Interfaces_1.BaseField {
        createDom(dmElement) {
            const tthis = this;
            this._element = dmElement;
            const result = $("<div>");
            const headLine = $("<div class='dm-anydatafield-headline'><a class='dm-anydatafield-headline-value'>Value</a> " +
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
            }
            else if ((typeof this._fieldValue === "object" || typeof this._fieldValue === "function") && (this._fieldValue !== null)) {
                this.highlightReference();
            }
            else {
                this.highlightValue();
            }
            return result;
        }
        evaluateDom(dmElement) {
            if (this._mode === ModeValue.Value) {
                if (this._textBox !== undefined && this._textBox !== null) {
                    const fieldName = this.field.get('name').toString();
                    dmElement.set(fieldName, this._textBox.val());
                }
            }
        }
        highlightValue() {
            this._aValue.addClass('active');
            this._aCollection.removeClass('active');
            this._aReference.removeClass('active');
            this._mode = ModeValue.Value;
            this.updateDomContent();
        }
        highlightCollection() {
            this._aValue.removeClass('active');
            this._aCollection.addClass('active');
            this._aReference.removeClass('active');
            this._mode = ModeValue.Collection;
            this.updateDomContent();
        }
        highlightReference() {
            this._aValue.removeClass('active');
            this._aCollection.removeClass('active');
            this._aReference.addClass('active');
            this._mode = ModeValue.Reference;
            this.updateDomContent();
        }
        reloadAndUpdateDomContent() {
            const tthis = this;
            ClientItem.getProperty(this.form.workspace, this.itemUrl, this.field.get('name').toString()).then((item) => {
                tthis._fieldValue = item;
                tthis.updateDomContent();
            });
        }
        updateDomContent() {
            this._domElement.empty();
            /* Otherwise just create the correct field type. */
            if (this.isReadOnly) {
                this.updateDomContentReadOnly();
            }
            else {
                this.updateDomContentEditable();
            }
        }
        updateDomContentReadOnly() {
            var _a;
            const value = this._fieldValue;
            if (value === null || value === undefined
                || (this._mode === ModeValue.Reference && (typeof value !== "object" && typeof value !== "function"))) {
                const div = $("<div><em>Not set</em></null>");
                this._domElement.append(div);
            }
            else if (this._mode === ModeValue.Reference) {
                const field = this.createReferenceFieldInstance();
                const element = field.createDomByValue(value);
                this._domElement.append(element);
            }
            else if (this._mode === ModeValue.Value) {
                const div = $("<div />");
                div.text((_a = value === null || value === void 0 ? void 0 : value.toString()) !== null && _a !== void 0 ? _a : "");
                this._domElement.append(div);
            }
            else if (this._mode === ModeValue.Collection) {
                const field = this.createSubElementFieldInstance();
                const element = field.createDomByValue(value);
                this._domElement.append(element);
            }
        }
        updateDomContentEditable() {
            var _a;
            const value = this._fieldValue;
            const fieldName = this.field.get('name').toString();
            var tthis = this;
            if (this._mode === ModeValue.Reference) {
                if ((typeof value !== "object" && typeof value !== "function") || value === null || value === undefined) {
                    const div = $("<div><em>null</em></null>");
                    this._domElement.append(div);
                }
                else {
                    const asDmObject = value;
                    const div = $("<div />");
                    (0, DomHelper_1.injectNameByUri)(div, asDmObject.workspace, asDmObject.uri);
                    this._domElement.append(div);
                }
                if (this.configuration.isNewItem) {
                    const div = $("<em>Element needs to be saved first</em>");
                    this._domElement.append(div);
                }
                else {
                    const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
                    const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
                    const containerChangeCell = $("<div></div>");
                    unsetCell.on('click', () => {
                        ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, fieldName).then(() => {
                            tthis.updateDomContent();
                        });
                    });
                    changeCell.on('click', () => {
                        containerChangeCell.empty();
                        const selectItem = new SIC.SelectItemControl();
                        const settings = new SIC.Settings();
                        settings.showWorkspaceInBreadcrumb = true;
                        settings.showExtentInBreadcrumb = true;
                        selectItem.itemSelected.addListener(selectedItem => {
                            ClientItem.addReferenceToCollection(tthis.form.workspace, tthis.itemUrl, {
                                property: tthis.field.get('name'),
                                referenceUri: selectedItem.uri,
                                referenceWorkspaceId: selectItem.getUserSelectedWorkspace()
                            }).then(() => {
                                this.updateDomContent();
                            });
                        });
                        containerChangeCell.empty();
                        selectItem.init(containerChangeCell, settings);
                        return false;
                    });
                    this._domElement.append(changeCell);
                    this._domElement.append(unsetCell);
                    this._domElement.append(containerChangeCell);
                }
            }
            else if (this._mode === ModeValue.Value) {
                this._textBox = $("<input />");
                this._textBox.val((_a = value === null || value === void 0 ? void 0 : value.toString()) !== null && _a !== void 0 ? _a : "");
                this._domElement.append(this._textBox);
            }
            else if (this._mode === ModeValue.Collection) {
                if (this.configuration.isNewItem) {
                    const div = $("<em>Element needs to be saved first</em>");
                    this._domElement.append(div);
                }
                else {
                    const field = this.createSubElementFieldInstance();
                    const element = field.createDomByValue(value);
                    this._domElement.append(element);
                }
            }
        }
        createReferenceFieldInstance() {
            const element = new ReferenceField.Control();
            this.cloneField(element);
            return element;
        }
        cloneField(element) {
            element.isReadOnly = this.isReadOnly;
            element.configuration = this.configuration;
            element.itemUrl = this.itemUrl;
            element.propertyName = this.field.get('name').toString();
            element.form = this.form;
        }
        createSubElementFieldInstance() {
            const element = new SubElementField.Control();
            this.cloneField(element);
            return element;
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=AnyDataField.js.map