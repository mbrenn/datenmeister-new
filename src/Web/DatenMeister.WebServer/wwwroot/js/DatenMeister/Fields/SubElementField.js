define(["require", "exports", "../Interfaces.Fields", "../Mof", "../Forms.FieldFactory", "../Website", "../Forms.SelectItemControl", "../Client.Items"], function (require, exports, Interfaces_Fields_1, Mof_1, FieldFactory, Website_1, SIC, ClientItems) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {value: true});
    exports.Field = void 0;
    class Field extends Interfaces_Fields_1.BaseField {
        reloadValuesFromServer() {
            const url = this._element.uri;
            const fieldName = this.field.get('name');
            ClientItems.getProperty(this.form.workspace, url, fieldName).done(x => this.createDomByValue(x));
        }
        createDom(dmElement) {
            this._element = dmElement;
            const fieldName = this.field.get('name');
            const value = dmElement.get(fieldName);
            return this.createDomByValue(value);
        }
        createDomByValue(value) {
            var _a;
            var tthis = this;
            if (this.isReadOnly) {
                this._list = $("<div></div>");
                let ul = $("<ul class='list-unstyled'></ul>");
                let foundElements = 0;
                for (let m in value) {
                    if (Object.prototype.hasOwnProperty.call(value, m)) {
                        let innerValue = value[m];
                        const item = $("<li><a></a></li>");
                        const link = $("a", item);
                        const name = innerValue.get('name');
                        if (name !== undefined && name !== "") {
                            link.text(innerValue.get('name'));
                        } else {
                            link.append($("<em>Unnamed</em>"));
                        }
                        link.attr('href', (0, Website_1.getItemDetailUri)(innerValue));
                        ul.append(item);
                        foundElements++;
                    }
                }
                if (foundElements === 0) {
                    ul = $("<em>No items</em>");
                }
                this._list.append(ul);
            } else {
                this._list = $("<div></div>");
                const table = $("<table><tbody></tbody></table>");
                this._list.append(table);
                let fields = (_a = this.field.get('form')) === null || _a === void 0 ? void 0 : _a.get('field');
                let fieldsData = [];
                if (fields === undefined) {
                    const nameField = new Mof_1.DmObject();
                    nameField.setMetaClassById("DatenMeister.Models.Forms.TextFieldData");
                    nameField.set('name', 'name');
                    nameField.set('title', 'Name');
                    nameField.set('isReadOnly', true);
                    fieldsData.push(nameField);
                }
                const tBody = $("tbody", table);
                const tr = $("<tr></tr>");
                for (let fieldDataKey in fieldsData) {
                    let fieldData = fieldsData[fieldDataKey];
                    let header = $("<th></th>");
                    header.text(fieldData.get('title'));
                    tr.append(header);
                }
                tBody.append(tr);
                for (let m in value) {
                    if (Object.prototype.hasOwnProperty.call(value, m)) {
                        const tr = $("<tr></tr>");
                        for (let fieldDataKey in fieldsData) {
                            const td = $("<td></td>");
                            let innerValue = value[m];
                            let fieldData = fieldsData[fieldDataKey];
                            const field = FieldFactory.createField(fieldData.metaClass.id, {
                                field: fieldData,
                                form: this.form,
                                isReadOnly: true,
                                itemUrl: innerValue.uri,
                                configuration: {}
                            });
                            const dom = field.createDom(innerValue);
                            td.append(dom);
                            tr.append(td);
                        }
                        table.append(tr);
                    }
                }
                const newItem = $("<div><btn class='btn btn-secondary dm-subelements-appenditem-btn'>Attach new Item</btn><div class='dm-subelements-appenditem-box'></div></div>");
                $(".dm-subelements-appenditem-btn", newItem).on('click', () => {
                    const containerDiv = $(".dm-subelements-appenditem-box");
                    containerDiv.empty();
                    const selectItem = new SIC.SelectItemControl();
                    const settings = new SIC.Settings();
                    settings.showWorkspaceInBreadcrumb = true;
                    settings.showExtentInBreadcrumb = true;
                    selectItem.onItemSelected = selectedItem => {
                        ClientItems.addReferenceToCollection(tthis.form.workspace, tthis.itemUrl, {
                            property: tthis.field.get('name'),
                            referenceUri: selectedItem.uri,
                            referenceWorkspaceId: selectItem.getSelectedWorkspace()
                        }).done(x => {
                            this.reloadValuesFromServer();
                        });
                    };
                    selectItem.init(containerDiv, settings);
                    return false;
                });
                this._list.append(newItem);
            }
            const refreshBtn = $("<div><btn class='btn btn-secondary dm-subelements-refresh-btn'>Refresh</btn></div>");
            $(".dm-subelements-refresh-btn", refreshBtn).on('click', () => {
                tthis.reloadValuesFromServer();
            });
            this._list.append(refreshBtn);
            return this._list;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=SubElementField.js.map