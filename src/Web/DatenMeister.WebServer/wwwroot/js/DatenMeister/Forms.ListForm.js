define(["require", "exports", "./Forms.FieldFactory", "./Settings", "./Forms.SelectItemControl"], function (require, exports, Forms_FieldFactory_1, Settings, SIC) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.openMetaClassSelectionFormForNewItem = exports.ListForm = void 0;
    class ListForm {
        refreshForm() {
            this.createFormByCollection(this.parentHtml, this.configuration);
        }
        createFormByCollection(parent, configuration) {
            var _a, _b, _c, _d;
            this.parentHtml = parent;
            this.configuration = configuration;
            const tthis = this;
            if (configuration.isReadOnly === undefined) {
                configuration.isReadOnly = true;
            }
            let headline = $("<h2></h2>");
            headline.text((_a = this.formElement.get('title')) !== null && _a !== void 0 ? _a : this.formElement.get('name'));
            parent.append(headline);
            const property = this.formElement.get('property');
            // Evaluate the new buttons to create objects
            const defaultTypesForNewElements = this.formElement.getAsArray("defaultTypesForNewElements");
            if (defaultTypesForNewElements !== undefined) {
                for (let n in defaultTypesForNewElements) {
                    const inner = defaultTypesForNewElements[n];
                    (function (innerValue) {
                        const btn = $("<btn class='btn btn-secondary'></btn>");
                        btn.text("Create " + inner.get('name'));
                        btn.on('click', () => {
                            const uri = innerValue.get('metaClass').uri;
                            if (property === undefined) {
                                document.location.href =
                                    Settings.baseUrl +
                                    "ItemAction/Extent.CreateItem?workspace=" +
                                    encodeURIComponent(tthis.workspace) +
                                    "&extent=" +
                                    encodeURIComponent(tthis.extentUri) +
                                    "&metaclass=" +
                                    encodeURIComponent(uri);
                            } else {
                                document.location.href =
                                    Settings.baseUrl +
                                    "ItemAction/Extent.CreateItemInProperty?workspace=" +
                                    encodeURIComponent(tthis.workspace) +
                                    "&itemUrl=" +
                                    encodeURIComponent(tthis.itemId) +
                                    "&metaclass=" +
                                    encodeURIComponent(uri) +
                                    "&property=" +
                                    encodeURIComponent(property);
                            }
                        });
                        parent.append(btn);
                    })(inner);
                }
            }
            if (this.elements === undefined) {
                this.elements = [];
            }
            // Evaluate the elements themselves
            if (!Array.isArray(this.elements)) {
                const div = $("<div></div>");
                div.text("Non-Array elements for ListForm: ");
                div.append($("<em></em>").text(this.elements));
                parent.append(div);
            }
            else {
                let table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
                const fields = this.formElement.getAsArray("field");
                const headerRow = $("<tbody><tr></tr></tbody>");
                const innerRow = $("tr", headerRow);
                for (let n in fields) {
                    if (!fields.hasOwnProperty(n))
                        continue;
                    const field = fields[n];
                    let cell = $("<th></th>");
                    cell.text((_b = field.get("title")) !== null && _b !== void 0 ? _b : field.get("name"));
                    innerRow.append(cell);
                }
                table.append(headerRow);
                let metaClass = (_c = this.formElement.get('metaClass')) === null || _c === void 0 ? void 0 : _c.uri;
                let noItemsWithMetaClass = this.formElement.get('noItemsWithMetaClass');
                let elements = this.elements;
                for (let n in elements) {
                    if (Object.prototype.hasOwnProperty.call(elements, n)) {
                        let element = this.elements[n];
                        // Check, if the element may be shown
                        let elementsMetaClass = (_d = element.metaClass) === null || _d === void 0 ? void 0 : _d.uri;
                        if ((elementsMetaClass !== undefined && elementsMetaClass !== "") && noItemsWithMetaClass) {
                            // Only items with no metaclass may be shown, but the element is the metaclass
                            continue;
                        }
                        if ((metaClass !== undefined && metaClass !== "") && elementsMetaClass !== metaClass) {
                            // Only elements with given metaclass shall be shown, but given element is not of
                            // the metaclass type
                            continue;
                        }
                        const row = $("<tr></tr>");
                        for (let n in fields) {
                            if (!fields.hasOwnProperty(n))
                                continue;
                            const field = fields[n];
                            let cell = $("<td></td>");
                            const fieldMetaClassId = field.metaClass.id;
                            const fieldElement = (0, Forms_FieldFactory_1.createField)(fieldMetaClassId, {
                                configuration: configuration,
                                form: this,
                                field: field,
                                itemUrl: element.uri,
                                isReadOnly: configuration.isReadOnly
                            });
                            cell.append(fieldElement.createDom(element));
                            row.append(cell);
                        }
                        table.append(row);
                    }
                }
                parent.append(table);
            }
        }
    }
    exports.ListForm = ListForm;
    function openMetaClassSelectionFormForNewItem(buttonDiv, containerDiv, workspace, extentUri) {
        const tthis = this;
        buttonDiv.on('click', () => {
            containerDiv.empty();
            const selectItem = new SIC.SelectItemControl();
            const settings = new SIC.Settings();
            settings.showWorkspaceInBreadcrumb = true;
            settings.showExtentInBreadcrumb = true;
            selectItem.setWorkspaceById('Types');
            selectItem.setExtentByUri("dm:///_internal/types/internal");
            selectItem.onItemSelected = selectedItem => {
                if (selectedItem === undefined) {
                    document.location.href =
                        Settings.baseUrl +
                            "ItemAction/Extent.CreateItem?workspace=" +
                            encodeURIComponent(workspace) +
                            "&extent=" +
                            encodeURIComponent(extentUri);
                }
                else {
                    document.location.href =
                        Settings.baseUrl +
                            "ItemAction/Extent.CreateItem?workspace=" +
                            encodeURIComponent(workspace) +
                            "&extent=" +
                            encodeURIComponent(extentUri) +
                            "&metaclass=" +
                            encodeURIComponent(selectedItem.uri);
                }
            };
            selectItem.init(containerDiv, settings);
        });
    }
    exports.openMetaClassSelectionFormForNewItem = openMetaClassSelectionFormForNewItem;
});
//# sourceMappingURL=Forms.ListForm.js.map