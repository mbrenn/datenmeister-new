define(["require", "exports", "../DomHelper", "../Client.Items", "../Forms.SelectItemControl"], function (require, exports, DomHelper_1, ClientItem, SIC) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = exports.Control = void 0;
    class Control {
        constructor() {
            this._list = $("<span></span>");
        }
        createDomByValue(value) {
            this._list.empty();
            const tthis = this;
            if ((typeof value !== "object" && typeof value !== "function") || value === null || value === undefined) {
                const div = $("<div><em>null</em></null>");
                this._list.append(div);
            }
            const asDmObject = value;
            if (this.configuration.isNewItem) {
                const div = $("<em>Element needs to be saved first</em>");
                this._list.append(div);
            }
            else if (this.isReadOnly) {
                const div = $("<div />");
                (0, DomHelper_1.injectNameByUri)(div, this.form.workspace, asDmObject.uri);
                this._list.append(div);
            }
            else {
                const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
                const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
                const containerChangeCell = $("<div></div>");
                unsetCell.on('click', () => {
                    ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, tthis.propertyName).done(() => {
                        tthis.reloadValuesFromServer();
                    });
                });
                changeCell.on('click', () => {
                    containerChangeCell.empty();
                    const selectItem = new SIC.SelectItemControl();
                    const settings = new SIC.Settings();
                    settings.showWorkspaceInBreadcrumb = true;
                    settings.showExtentInBreadcrumb = true;
                    selectItem.onItemSelected = selectedItem => {
                        ClientItem.addReferenceToCollection(tthis.form.workspace, tthis.itemUrl, {
                            property: tthis.propertyName,
                            referenceUri: selectedItem.uri,
                            referenceWorkspaceId: selectItem.getUserSelectedWorkspace()
                        }).done(() => {
                            this.reloadValuesFromServer();
                        });
                    };
                    selectItem.init(containerChangeCell, settings);
                    return false;
                });
                this._list.append(changeCell);
                this._list.append(unsetCell);
            }
            return this._list;
        }
        reloadValuesFromServer() {
            alert('reloadValuesFromServer is not overridden.');
        }
    }
    exports.Control = Control;
    class Field extends Control {
        createDom(dmElement) {
            this._list.empty();
            const fieldName = this.field.get('name');
            let value = dmElement.get(fieldName);
            if (Array.isArray(value)) {
                if (value.length === 1) {
                    value = value[0];
                }
                else {
                    this._list.append($("<em>The value is an array and not supported by the referencefield</em>"));
                    return this._list;
                }
            }
            if (this.isReadOnly === true) {
                if (value === undefined) {
                    this._list.html("<em>undefined</em>");
                }
                else {
                    this._list.text(value.get('name'));
                }
            }
            else {
                this._list.text(value.get('name'));
            }
            return this._list;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=ReferenceField.js.map