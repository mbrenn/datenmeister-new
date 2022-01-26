define(["require", "exports", "./Forms.FieldFactory", "./Settings", "./Forms.SelectItemControl"], function (require, exports, Forms_FieldFactory_1, Settings, SIC) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {value: true});
    exports.openMetaClassSelectionFormForNewItem = exports.ListForm = void 0;
    class ListForm {
        refreshForm() {
            this.createFormByCollection(this.parentHtml, this.configuration);
        }
        createFormByCollection(parent, configuration) {
            var _a;
            this.parentHtml = parent;
            this.configuration = configuration;
            const tthis = this;
            if (configuration.isReadOnly === undefined) {
                configuration.isReadOnly = true;
            }
            let headline = $("<h2></h2>");
            headline.text(this.formElement.get('name'));
            parent.append(headline);
            // Evaluate the new buttons to create objects
            const defaultTypesForNewElements = this.formElement.getAsArray("defaultTypesForNewElements");
            if (defaultTypesForNewElements !== undefined) {
                for (let n in defaultTypesForNewElements) {
                    const inner = defaultTypesForNewElements[n];
                    const btn = $("<btn class='btn btn-secondary'></btn>");
                    btn.text("Create " + inner.get('name'));
                    btn.on('click', () => {
                        const uri = inner.uri;
                        document.location.href =
                            Settings.baseUrl +
                            "ItemAction/Extent.CreateItem?workspace=" +
                            encodeURIComponent(tthis.workspace) +
                            "&extent=" +
                            encodeURIComponent(tthis.extentUri) +
                            "&metaclass=" +
                            encodeURIComponent(uri);
                    });
                    parent.append(btn);
                }
            }
            // Evaluate the elements themselves
            if (!Array.isArray(this.elements)) {
                const div = $("<div></div>");
                div.text("Non-Array elements for ListForm: ");
                div.append($("<em></em>").text(this.elements));
                parent.append(div);
            } else {
                let table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
                const fields = this.formElement.getAsArray("field");
                const headerRow = $("<tbody><tr></tr></tbody>");
                const innerRow = $("tr", headerRow);
                for (let n in fields) {
                    if (!fields.hasOwnProperty(n))
                        continue;
                    const field = fields[n];
                    let cell = $("<th></th>");
                    cell.text((_a = field.get("title")) !== null && _a !== void 0 ? _a : field.get("name"));
                    innerRow.append(cell);
                }
                table.append(headerRow);
                let elements = this.elements;
                for (let n in elements) {
                    if (Object.prototype.hasOwnProperty.call(elements, n)) {
                        let element = this.elements[n];
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
                } else {
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