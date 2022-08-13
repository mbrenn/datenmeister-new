var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
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
        // Creates the overall DOM
        createDom(dmElement) {
            return __awaiter(this, void 0, void 0, function* () {
                const tthis = this;
                this._element = dmElement;
                const result = $("<div>");
                // Creates the headline in which the user can select which type of object he wants to add
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
                // Selects depending on the found type. May be an array, a reference or a value
                if (this._fieldValue === null || this._fieldValue === undefined) {
                    this.highlightReference();
                }
                else if ((typeof this._fieldValue === "object" || typeof this._fieldValue === "function")) {
                    this.highlightReference();
                }
                else {
                    this.highlightValue();
                }
                return result;
            });
        }
        evaluateDom(dmElement) {
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
            return __awaiter(this, void 0, void 0, function* () {
                const tthis = this;
                tthis._fieldValue = yield ClientItem.getProperty(this.form.workspace, this.itemUrl, this.field.get('name').toString());
                yield tthis.updateDomContent();
            });
        }
        // Performs a 'reload' of the complete DOM
        updateDomContent() {
            return __awaiter(this, void 0, void 0, function* () {
                this._domElement.empty();
                /* Otherwise just create the correct field type. */
                if (this.isReadOnly) {
                    yield this.updateDomContentReadOnly();
                }
                else {
                    yield this.updateDomContentEditable();
                }
            });
        }
        // Rebuilds the BOM in the read-only mode
        updateDomContentReadOnly() {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
                let value = this._fieldValue;
                if (value === null || value === undefined
                    || (this._mode === ModeValue.Reference && (typeof value !== "object" && typeof value !== "function"))) {
                    const div = $("<div><em class='dm-undefined'>Undefined</em></div>");
                    this._domElement.append(div);
                }
                else if (this._mode === ModeValue.Reference) {
                    if (Array.isArray(value)) {
                        value = value[0];
                    }
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
                    const element = yield field.createDomByFieldValue(value);
                    this._domElement.append(element);
                }
            });
        }
        // Rebuilds the DOM in the edit mode
        updateDomContentEditable() {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
                if (this._mode === ModeValue.Reference) {
                    const tthis = this;
                    let value = this._fieldValue;
                    if (Array.isArray(value)) {
                        value = value[0];
                    }
                    const fieldName = this.field.get('name').toString();
                    if ((typeof value !== "object" && typeof value !== "function") || value === null || value === undefined) {
                        // Nothing is selected... ==> Null value
                        const div = $("<div><em class='dm-undefined'>undefined</em></null>");
                        this._domElement.append(div);
                    }
                    else {
                        // An element is selected. The name can be shown
                        const asDmObject = value;
                        const div = $("<div />");
                        (0, DomHelper_1.injectNameByUri)(div, asDmObject.workspace, asDmObject.uri);
                        this._domElement.append(div);
                    }
                    if (this.configuration.isNewItem) {
                        // If we are having a new element, the element needs to be saved, so we 
                        // have an element id which can be used to reference this element
                        const div = $("<em>Element needs to be saved first</em>");
                        this._domElement.append(div);
                    }
                    else {
                        // If we are having a reference to the containing element
                        // create the buttons to change and unset the property
                        const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
                        const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
                        const containerChangeCell = $("<div></div>");
                        unsetCell.on('click', () => __awaiter(this, void 0, void 0, function* () {
                            // Unsets the property and close
                            yield ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, fieldName);
                            tthis.updateDomContent();
                        }));
                        changeCell.on('click', () => __awaiter(this, void 0, void 0, function* () {
                            var _b, _c;
                            // The user wants to select a new one
                            containerChangeCell.empty();
                            const selectItem = new SIC.SelectItemControl();
                            const settings = new SIC.Settings();
                            settings.showWorkspaceInBreadcrumb = true;
                            settings.showExtentInBreadcrumb = true;
                            settings.hideAtStartup = true;
                            selectItem.itemSelected.addListener((selectedItem) => __awaiter(this, void 0, void 0, function* () {
                                yield ClientItem.setPropertyReference(tthis.form.workspace, tthis.itemUrl, {
                                    property: tthis.field.get('name'),
                                    referenceUri: selectedItem.uri,
                                    workspaceId: selectItem.getUserSelectedWorkspaceId()
                                });
                                yield tthis.reloadAndUpdateDomContent();
                            }));
                            containerChangeCell.empty();
                            yield selectItem.initAsync(containerChangeCell, settings);
                            // Sets the item, if defined
                            if (value.workspace !== undefined && value.uri !== undefined) {
                                yield selectItem.setItemByUri(value.workspace, value.uri);
                            }
                            else {
                                // Sets the workspace, if defined
                                if ((value === null || value === void 0 ? void 0 : value.workspace) !== undefined) {
                                    yield selectItem.setWorkspaceById(value.workspace);
                                }
                                else if (((_b = this._element) === null || _b === void 0 ? void 0 : _b.workspace) !== undefined) {
                                    yield selectItem.setWorkspaceById(tthis._element.workspace);
                                }
                                // Sets the extent, if defined
                                if ((value === null || value === void 0 ? void 0 : value.extentUri) !== undefined) {
                                    yield selectItem.setWorkspaceById(value.workspace);
                                }
                                else if (((_c = this._element) === null || _c === void 0 ? void 0 : _c.extentUri) !== undefined) {
                                    yield selectItem.setExtentByUri(value.workspace, tthis._element.extentUri);
                                }
                            }
                            selectItem.showControl();
                            return false;
                        }));
                        this._domElement.append(changeCell);
                        this._domElement.append(unsetCell);
                        this._domElement.append(containerChangeCell);
                    }
                }
                else if (this._mode === ModeValue.Value) {
                    const value = this._fieldValue;
                    this._textBox = $("<input />");
                    this._textBox.val((_a = value === null || value === void 0 ? void 0 : value.toString()) !== null && _a !== void 0 ? _a : "");
                    this._domElement.append(this._textBox);
                }
                else if (this._mode === ModeValue.Collection) {
                    const value = this._fieldValue;
                    if (this.configuration.isNewItem) {
                        const div = $("<em>Element needs to be saved first</em>");
                        this._domElement.append(div);
                    }
                    else {
                        const field = this.createSubElementFieldInstance();
                        const element = yield field.createDomByFieldValue(value);
                        this._domElement.append(element);
                    }
                }
            });
        }
        createReferenceFieldInstance() {
            const element = new ReferenceField.Control();
            this.cloneField(element);
            return element;
        }
        createSubElementFieldInstance() {
            const element = new SubElementField.Control();
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
    }
    exports.Field = Field;
});
//# sourceMappingURL=AnyDataField.js.map