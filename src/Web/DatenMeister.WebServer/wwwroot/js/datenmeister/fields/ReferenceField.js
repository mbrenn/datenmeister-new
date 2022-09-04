var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Mof", "../DomHelper", "../client/Items", "../controls/SelectItemControl"], function (require, exports, Mof_1, DomHelper_1, ClientItem, SIC) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = exports.Control = void 0;
    class Control {
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
        createDomByValue(value) {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
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
                        (0, DomHelper_1.injectNameByUri)(div, asDmObject.workspace, asDmObject.uri);
                        this._list.append(div);
                    }
                    if (!this.isReadOnly) {
                        const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
                        const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
                        const containerChangeCell = $("<div></div>");
                        unsetCell.on('click', () => {
                            ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, tthis.propertyName).then(() => __awaiter(this, void 0, void 0, function* () {
                                yield tthis.reloadValuesFromServer();
                            }));
                        });
                        changeCell.on('click', () => __awaiter(this, void 0, void 0, function* () {
                            yield this.createSelectFields(containerChangeCell, value);
                            return false;
                        }));
                        this._list.append(changeCell);
                        this._list.append(unsetCell);
                        this._list.append(containerChangeCell);
                        // Checks, whether the Drop-Down Field shall be completely pre-created
                        if (this.inhibitInline !== true &&
                            ((_a = this.field) === null || _a === void 0 ? void 0 : _a.get('isSelectionInline', Mof_1.ObjectType.Boolean)) === true) {
                            yield this.createSelectFields(containerChangeCell, value);
                        }
                    }
                }
                return this._list;
            });
        }
        /** Creates the GUI elements in which the user is capable to select the items to be reference
         * @param containerChangeCell The cell which will contain the GUI elements. This cell will be emptied
         * @param value The value that is currently selected*/
        createSelectFields(containerChangeCell, value) {
            var _a, _b;
            return __awaiter(this, void 0, void 0, function* () {
                const tthis = this;
                containerChangeCell.empty();
                const selectItem = new SIC.SelectItemControl();
                const settings = new SIC.Settings();
                settings.showWorkspaceInBreadcrumb = true;
                settings.showExtentInBreadcrumb = true;
                selectItem.itemSelected.addListener((selectedItem) => __awaiter(this, void 0, void 0, function* () {
                    yield ClientItem.setPropertyReference(tthis.form.workspace, tthis.itemUrl, {
                        property: tthis.propertyName,
                        referenceUri: selectedItem.uri,
                        workspaceId: selectItem.getUserSelectedWorkspaceId()
                    });
                    containerChangeCell.empty();
                    tthis.inhibitInline = true;
                    yield this.reloadValuesFromServer();
                }));
                yield selectItem.initAsync(containerChangeCell, settings);
                if (value !== undefined && value !== null &&
                    (typeof value === "object" || typeof value === "function")) {
                    const valueAsDmObject = value;
                    yield selectItem.setItemByUri(valueAsDmObject.workspace, valueAsDmObject.uri);
                }
                else {
                    // No value is selected, so retrieve the default items
                    const workspaceId = (_a = this.field) === null || _a === void 0 ? void 0 : _a.get('defaultWorkspace', Mof_1.ObjectType.Single);
                    const itemUri = (_b = this.field) === null || _b === void 0 ? void 0 : _b.get('defaultItemUri', Mof_1.ObjectType.Single);
                    if (workspaceId !== undefined && workspaceId !== null) {
                        if (itemUri === null || itemUri === undefined) {
                            yield selectItem.setWorkspaceById(workspaceId);
                        }
                        else {
                            yield selectItem.setItemByUri(workspaceId, itemUri);
                        }
                    }
                }
            });
        }
        reloadValuesFromServer() {
            return __awaiter(this, void 0, void 0, function* () {
                alert('reloadValuesFromServer is not overridden.');
            });
        }
    }
    exports.Control = Control;
    class Field extends Control {
        createDom(dmElement) {
            return __awaiter(this, void 0, void 0, function* () {
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
                    return yield this.createDomByValue(value);
                }
                return this._list;
            });
        }
        evaluateDom(dmElement) {
        }
        reloadValuesFromServer() {
            return __awaiter(this, void 0, void 0, function* () {
                let value = yield ClientItem.getProperty(this.form.workspace, this.element.uri, this.fieldName);
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
                yield this.createDomByValue(value);
            });
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=ReferenceField.js.map