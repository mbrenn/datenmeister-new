var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Mof", "../forms/FieldFactory", "../controls/SelectItemControl", "../client/Items", "../client/Types", "../DomHelper", "../models/DatenMeister.class", "../controls/TypeSelectionControl", "../client/Actions.Items", "../FormActions", "../Navigator"], function (require, exports, Mof_1, FieldFactory, SIC, ClientItems, ClientTypes, DomHelper_1, DatenMeister_class_1, TypeSelectionControl, Actions_Items_1, FormActions, Navigator) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = exports.Control = void 0;
    class Control {
        constructor() {
            this._list = $("<div></div>");
        }
        createDomByFieldValue(fieldValue) {
            return __awaiter(this, void 0, void 0, function* () {
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
                            let innerValue = fieldValue[m];
                            const item = $("<li></li>");
                            const injectParams = {};
                            if (this.itemActionName !== undefined) {
                                injectParams.onClick = (x) => __awaiter(this, void 0, void 0, function* () {
                                    const readObject = yield ClientItems.getObjectByUri(x.workspace, x.uri);
                                    yield FormActions.execute(this.itemActionName, tthis.form, readObject);
                                    return false;
                                });
                            }
                            let _ = (0, DomHelper_1.injectNameByUri)(item, innerValue.workspace, innerValue.uri, injectParams);
                            ul.append(item);
                            foundElements++;
                        }
                    }
                    if (foundElements === 0) {
                        ul = $("<em>No items</em>");
                    }
                    this._list.append(ul);
                }
                else {
                    if (!Array.isArray(fieldValue)) {
                        fieldValue = [];
                    }
                    const table = $("<table><tbody></tbody></table>");
                    this._list.append(table);
                    let fields = this.getFieldDefinitions();
                    let fieldsData = new Array();
                    if (fields === undefined) {
                        const nameField = new Mof_1.DmObject();
                        nameField.setMetaClassByUri(DatenMeister_class_1._DatenMeister._Forms.__TextFieldData_Uri);
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
                            let innerValue = fieldValue[m];
                            for (let fieldDataKey in fieldsData) {
                                const td = $("<td></td>");
                                let fieldData = fieldsData[fieldDataKey];
                                const field = FieldFactory.createField(fieldData.metaClass.uri, {
                                    field: fieldData,
                                    isReadOnly: true,
                                    itemUrl: innerValue.uri,
                                    configuration: {},
                                    form: tthis.form
                                });
                                const dom = yield field.createDom(innerValue);
                                td.append(dom);
                                tr.append(td);
                            }
                            /* Creates the delete button */
                            const moveUp = $("<btn class='btn btn-secondary dm-item-moveup-button'>⬆️</btn>");
                            const moveDown = $("<btn class='btn btn-secondary dm-item-movedown-button'>⬇️</btn>");
                            moveUp.on("click", () => __awaiter(this, void 0, void 0, function* () {
                                yield (0, Actions_Items_1.moveItemInCollectionUp)(this.form.workspace, this.itemUrl, this.propertyName, innerValue.uri);
                                yield this.reloadValuesFromServer();
                            }));
                            moveDown.on("click", () => __awaiter(this, void 0, void 0, function* () {
                                yield (0, Actions_Items_1.moveItemInCollectionDown)(this.form.workspace, this.itemUrl, this.propertyName, innerValue.uri);
                                yield this.reloadValuesFromServer();
                            }));
                            /* Creates the delete button */
                            let deleteCell = $("<td><btn class='btn btn-secondary'>Delete</btn></td>");
                            $("btn", deleteCell).on("click", () => {
                                ClientItems.removeReferenceFromCollection(tthis.form.workspace, tthis.itemUrl, {
                                    property: tthis.propertyName,
                                    referenceUri: innerValue.uri,
                                    referenceWorkspaceId: innerValue.workspace
                                })
                                    .then(() => {
                                    tthis.reloadValuesFromServer();
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
                        selectItem.itemSelected.addListener(selectedItem => {
                            ClientItems.addReferenceToCollection(tthis.form.workspace, tthis.itemUrl, {
                                property: tthis.propertyName,
                                referenceUri: selectedItem.uri,
                                workspaceId: selectItem.getUserSelectedWorkspaceId()
                            }).then(() => {
                                this.reloadValuesFromServer();
                            });
                        });
                        selectItem.init(containerDiv, settings);
                        return false;
                    });
                    $(".dm-subelements-createitem-btn", attachItem).on("click", () => __awaiter(this, void 0, void 0, function* () {
                        const container = $(".dm-subelements-createitem-box", attachItem);
                        container.empty();
                        // Create the Type Selection Control element in which the user can select the right 
                        const control = new TypeSelectionControl.TypeSelectionControl(container);
                        // Get the property type
                        if (this.propertyType !== undefined) {
                            control.setCurrentTypeUrl(this.propertyType);
                        }
                        control.typeSelected.addListener((x) => __awaiter(this, void 0, void 0, function* () {
                            if (x === undefined ||
                                x.selectedType === undefined ||
                                x.selectedType.uri === undefined) {
                                alert("Nothing is selected.");
                                return;
                            }
                            document.location.href = Navigator.getLinkForNavigateToCreateItemInProperty(tthis.form.workspace, tthis.itemUrl, x.selectedType.uri, tthis.propertyName);
                        }));
                        yield control.createControl();
                    }));
                    this._list.append(attachItem);
                }
                const refreshBtn = $("<div><btn class='dm-subelements-refresh-btn'><img src='/img/refresh-16.png' alt='Refresh' /></btn></div>");
                $(".dm-subelements-refresh-btn", refreshBtn).on("click", () => {
                    tthis.reloadValuesFromServer();
                });
                this._list.append(refreshBtn);
                return this._list;
            });
        }
        reloadValuesFromServer() {
            alert("reloadValuesFromServer is not overridden.");
        }
        /**
         * Returns the default definition of a name.
         * method can be overridden by the right field definitions
         */
        getFieldDefinitions() {
            return undefined;
        }
    }
    exports.Control = Control;
    class Field extends Control {
        reloadValuesFromServer() {
            const tthis = this;
            const url = this._element.uri;
            ClientItems.getProperty(this.form.workspace, url, this.propertyName).then(x => tthis.createDomByFieldValue(x));
        }
        getFieldDefinitions() {
            var _a;
            return (_a = this.field.get("form", Mof_1.ObjectType.Single)) === null || _a === void 0 ? void 0 : _a.get("field", Mof_1.ObjectType.Array);
        }
        createDom(dmElement) {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
                this.propertyName = this.field.get(DatenMeister_class_1._DatenMeister._Forms._ActionFieldData._name_);
                this.itemActionName = this.field.get(DatenMeister_class_1._DatenMeister._Forms._ActionFieldData.actionName);
                if (this.configuration.isNewItem) {
                    return $("<em>Element needs to be saved first</em>");
                }
                else {
                    this._element = dmElement;
                    const value = dmElement.get(this.propertyName);
                    if (((_a = this._element.metaClass) === null || _a === void 0 ? void 0 : _a.uri) !== undefined
                        && this.propertyName !== undefined
                        && !this.isReadOnly) {
                        this.propertyType =
                            yield ClientTypes.getPropertyType(this._element.metaClass.workspace, this._element.metaClass.uri, this.propertyName);
                    }
                    yield this.createDomByFieldValue(value);
                    return this._list;
                }
            });
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=SubElementField.js.map