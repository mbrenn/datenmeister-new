var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Interfaces", "./FieldFactory", "../Settings", "../Navigator"], function (require, exports, Interfaces_1, FieldFactory_1, Settings, Navigator) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.TableForm = void 0;
    class TableForm {
        constructor() {
            this.formType = Interfaces_1.FormType.Table;
        }
        refreshForm() {
            return __awaiter(this, void 0, void 0, function* () {
                yield this.createFormByCollection(this.parentHtml, this.configuration, true);
            });
        }
        /**
         * This method just calls the createFormByCollection since a TableForm can
         * show the extent's elements directly or just the properties of an elemnet
         * @param parent The Html to which the table shall be added
         * @param configuration The Configuration for the table
         * @param refresh true, if we just would like to refresh the table and not create new elements
         */
        createFormByObject(parent, configuration, refresh) {
            return __awaiter(this, void 0, void 0, function* () {
                if (this.elements === undefined && this.element !== undefined) {
                    this.elements = this.element.get(this.formElement.get("property"));
                }
                return this.createFormByCollection(parent, configuration, refresh);
            });
        }
        createFormByCollection(parent, configuration, refresh) {
            var _a, _b, _c, _d;
            return __awaiter(this, void 0, void 0, function* () {
                this.parentHtml = parent;
                this.configuration = configuration;
                let metaClass = (_a = this.formElement.get('metaClass')) === null || _a === void 0 ? void 0 : _a.uri;
                const tthis = this;
                if (configuration.isReadOnly === undefined) {
                    configuration.isReadOnly = true;
                }
                this.cacheHeadline =
                    refresh === true && this.cacheHeadline !== undefined
                        ? this.cacheHeadline
                        : $("<h2><a></a></h2>");
                this.cacheHeadline.empty();
                const headLineLink = $("a", this.cacheHeadline);
                headLineLink.text((_b = this.formElement.get('title')) !== null && _b !== void 0 ? _b : this.formElement.get('name'));
                headLineLink.attr('href', Navigator.getLinkForNavigateToExtentItems(this.workspace, this.extentUri, { metaClass: metaClass }));
                if (refresh !== true) {
                    parent.append(this.cacheHeadline);
                }
                const property = this.formElement.get('property');
                this.cacheButtons =
                    refresh === true && this.cacheHeadline !== undefined
                        ? this.cacheButtons
                        : $("<div></div>");
                this.cacheButtons.empty();
                if (refresh !== true) {
                    parent.append(this.cacheButtons);
                }
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
                            tthis.cacheButtons.append(btn);
                        })(inner);
                    }
                }
                if (this.elements === undefined) {
                    this.elements = [];
                }
                // Evaluate the elements themselves
                if (!Array.isArray(this.elements)) {
                    this.cacheEmptyDiv =
                        refresh === true && this.cacheTable !== undefined
                            ? this.cacheTable
                            : $("<div></div>");
                    this.cacheEmptyDiv.empty();
                    this.cacheEmptyDiv.text("Non-Array elements for ListForm: ");
                    this.cacheEmptyDiv.append($("<em></em>").text(this.elements.toString()));
                    if (refresh !== true) {
                        parent.append(this.cacheEmptyDiv);
                    }
                }
                else {
                    this.cacheTable =
                        refresh === true && this.cacheTable !== undefined
                            ? this.cacheTable
                            : $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top dm-tableform'></table>");
                    this.cacheTable.empty();
                    const fields = this.formElement.getAsArray("field");
                    const headerRow = $("<tbody><tr></tr></tbody>");
                    const innerRow = $("tr", headerRow);
                    for (let n in fields) {
                        if (!fields.hasOwnProperty(n))
                            continue;
                        const field = fields[n];
                        let cell = $("<th></th>");
                        cell.text((_c = field.get("title")) !== null && _c !== void 0 ? _c : field.get("name"));
                        innerRow.append(cell);
                    }
                    this.cacheTable.append(headerRow);
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
                            this.cacheTable.append(row);
                        }
                    }
                    if (refresh !== true) {
                        parent.append(this.cacheTable);
                    }
                }
            });
        }
    }
    exports.TableForm = TableForm;
});
//# sourceMappingURL=TableForm.js.map