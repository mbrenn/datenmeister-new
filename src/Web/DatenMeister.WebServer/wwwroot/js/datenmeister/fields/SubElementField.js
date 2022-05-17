define(["require", "exports", "../Mof", "../forms/FieldFactory", "../controls/SelectItemControl", "../client/Items", "../MofResolver", "../Navigator", "../Settings"], function (require, exports, Mof_1, FieldFactory, SIC, ClientItems, MofResolver_1, Navigator_1, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = exports.Control = void 0;
    class Control {
        constructor() {
            this._list = $("<div></div>");
        }
        createDomByValue(value) {
            const tthis = this;
            this._list.empty();
            if (this.isReadOnly) {
                if (!Array.isArray(value)) {
                    return $("<div><em>Element is not an Array</em></div>");
                }
                let ul = $("<ul class='list-unstyled'></ul>");
                let foundElements = 0;
                for (let m in value) {
                    if (Object.prototype.hasOwnProperty.call(value, m)) {
                        let innerValue = value[m];
                        const item = $("<li><a></a></li>");
                        // Resolve the elements
                        ((a, b) => {
                            (0, MofResolver_1.resolve)(a).then(resolvedRaw => {
                                const resolved = resolvedRaw;
                                const link = $("a", b);
                                const name = resolved.get('name');
                                if (name !== undefined && name !== "") {
                                    link.text(resolved.get('name'));
                                }
                                else {
                                    link.append($("<em>Unnamed</em>"));
                                }
                                link.attr('href', '#');
                                link.on('click', () => {
                                    (0, Navigator_1.navigateToItemByUrl)(resolved.workspace, resolved.uri);
                                    return false;
                                });
                            });
                        })(innerValue, item);
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
                if (!Array.isArray(value)) {
                    value = [];
                }
                const table = $("<table><tbody></tbody></table>");
                this._list.append(table);
                let fields = this.getFieldDefinitions();
                let fieldsData = new Array();
                if (fields === undefined) {
                    const nameField = new Mof_1.DmObject();
                    nameField.setMetaClassById("DatenMeister.Models.Forms.TextFieldData");
                    nameField.set('name', 'name');
                    nameField.set('title', 'Name');
                    nameField.set('isReadOnly', true);
                    fieldsData.push(nameField);
                }
                /* Creates the table header */
                const tBody = $("tbody", table);
                const tr = $("<tr></tr>");
                for (let fieldDataKey in fieldsData) {
                    let fieldData = fieldsData[fieldDataKey];
                    let header = $("<th></th>");
                    header.text(fieldData.get('title'));
                    tr.append(header);
                }
                let deleteHeader = $("<th>Delete</th>");
                tr.append(deleteHeader);
                tBody.append(tr);
                /* Creates the rows */
                for (let m in value) {
                    if (Object.prototype.hasOwnProperty.call(value, m)) {
                        const tr = $("<tr></tr>");
                        let innerValue = value[m];
                        for (let fieldDataKey in fieldsData) {
                            const td = $("<td></td>");
                            let fieldData = fieldsData[fieldDataKey];
                            const field = FieldFactory.createField(fieldData.metaClass.id, {
                                field: fieldData,
                                isReadOnly: true,
                                itemUrl: innerValue.uri,
                                configuration: {},
                                form: tthis.form
                            });
                            const dom = field.createDom(innerValue);
                            td.append(dom);
                            tr.append(td);
                        }
                        /* Creates the delete button */
                        let deleteCell = $("<td><btn class='btn btn-secondary'>Delete</btn></td>");
                        $("btn", deleteCell).on('click', () => {
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
                        table.append(tr);
                    }
                }
                const attachItem = $("<div><btn class='btn btn-secondary dm-subelements-appenditem-btn'>Attach Item</btn><div class='dm-subelements-appenditem-box'></div></div>");
                $(".dm-subelements-appenditem-btn", attachItem).on('click', () => {
                    const containerDiv = $(".dm-subelements-appenditem-box", attachItem);
                    containerDiv.empty();
                    const selectItem = new SIC.SelectItemControl();
                    const settings = new SIC.Settings();
                    settings.showWorkspaceInBreadcrumb = true;
                    settings.showExtentInBreadcrumb = true;
                    selectItem.itemSelected.addListener(selectedItem => {
                        ClientItems.addReferenceToCollection(tthis.form.workspace, tthis.itemUrl, {
                            property: tthis.propertyName,
                            referenceUri: selectedItem.uri,
                            workspaceId: selectItem.getUserSelectedWorkspace()
                        }).then(() => {
                            this.reloadValuesFromServer();
                        });
                    });
                    selectItem.init(containerDiv, settings);
                    return false;
                });
                this._list.append(attachItem);
                const newItem = $("<div><btn class='btn btn-secondary dm-subelements-appenditem-btn'>Create Item</btn></div>");
                newItem.on('click', () => {
                    document.location.href =
                        Settings.baseUrl +
                            "ItemAction/Extent.CreateItemInProperty?workspace=" +
                            encodeURIComponent(tthis.form.workspace) +
                            "&itemUrl=" +
                            encodeURIComponent(tthis.itemUrl) +
                            /*"&metaclass=" +
                            encodeURIComponent(uri) +*/
                            "&property=" +
                            encodeURIComponent(tthis.propertyName);
                });
                this._list.append(newItem);
            }
            const refreshBtn = $("<div><btn class='dm-subelements-refresh-btn'><img src='/img/refresh-16.png' alt='Refresh' /></btn></div>");
            $(".dm-subelements-refresh-btn", refreshBtn).on('click', () => {
                tthis.reloadValuesFromServer();
            });
            this._list.append(refreshBtn);
            return this._list;
        }
        reloadValuesFromServer() {
            alert('reloadValuesFromServer is not overridden.');
        }
        // Returns the default definition of a name.
        // This method can be overridden by the right field definitions
        getFieldDefinitions() {
            return undefined;
        }
    }
    exports.Control = Control;
    class Field extends Control {
        reloadValuesFromServer() {
            const tthis = this;
            const url = this._element.uri;
            ClientItems.getProperty(this.form.workspace, url, this.propertyName).then(x => tthis.createDomByValue(x));
        }
        getFieldDefinitions() {
            var _a;
            return (_a = this.field.get('form', Mof_1.ObjectType.Single)) === null || _a === void 0 ? void 0 : _a.get('field', Mof_1.ObjectType.Array);
        }
        createDom(dmElement) {
            this.propertyName = this.field.get('name');
            if (this.configuration.isNewItem) {
                return $("<em>Element needs to be saved first</em>");
            }
            else {
                this._element = dmElement;
                const value = dmElement.get(this.propertyName);
                this.createDomByValue(value);
                return this._list;
            }
        }
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=SubElementField.js.map