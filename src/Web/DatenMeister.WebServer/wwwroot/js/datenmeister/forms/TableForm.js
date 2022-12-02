var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Interfaces", "./FieldFactory", "../Settings"], function (require, exports, Interfaces_1, FieldFactory_1, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.TableForm = void 0;
    class TableForm {
        constructor() {
            this.formType = Interfaces_1.FormType.Table;
        }
        refreshForm() {
            this.createFormByCollection(this.parentHtml, this.configuration);
        }
        createFormByCollection(parent, configuration) {
            var _a, _b, _c, _d;
            return __awaiter(this, void 0, void 0, function* () {
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
                                }
                                else {
                                    document.location.href =
                                        Settings.baseUrl +
                                            "ItemAction/Extent.CreateItemInProperty?workspace=" +
                                            encodeURIComponent(tthis.workspace) +
                                            "&itemUrl=" +
                                            encodeURIComponent(tthis.itemUrl) +
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
                    div.append($("<em></em>").text(this.elements.toString()));
                    parent.append(div);
                }
                else {
                    let table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top dm-tableform'></table>");
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
                                const fieldMetaClassUri = field.metaClass.uri;
                                const fieldElement = (0, FieldFactory_1.createField)(fieldMetaClassUri, {
                                    configuration: configuration,
                                    field: field,
                                    itemUrl: element.uri,
                                    isReadOnly: configuration.isReadOnly,
                                    form: this
                                });
                                const dom = yield fieldElement.createDom(element);
                                cell.append(dom);
                                row.append(cell);
                            }
                            table.append(row);
                        }
                    }
                    parent.append(table);
                }
            });
        }
    }
    exports.TableForm = TableForm;
});
//# sourceMappingURL=TableForm.js.map