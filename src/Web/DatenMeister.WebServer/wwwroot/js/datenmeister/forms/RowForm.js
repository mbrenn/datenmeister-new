var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Mof", "./FieldFactory", "../fields/TextField", "../models/DatenMeister.class"], function (require, exports, Mof, FieldFactory_1, TextField, DatenMeister_class_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.RowForm = exports.SubmitMethod = void 0;
    // Defines the possible submit methods, a user can chose to close the detail form
    var SubmitMethod;
    (function (SubmitMethod) {
        // The user clicked on the save button
        SubmitMethod[SubmitMethod["Save"] = 0] = "Save";
        // The user clicked on the save and close button
        SubmitMethod[SubmitMethod["SaveAndClose"] = 1] = "SaveAndClose";
    })(SubmitMethod = exports.SubmitMethod || (exports.SubmitMethod = {}));
    class RowForm {
        refreshForm() {
            this.createFormByObject(this.parentHtml, this.configuration);
        }
        createFormByObject(parent, configuration) {
            var _a, _b, _c, _d, _e, _f, _g;
            return __awaiter(this, void 0, void 0, function* () {
                this.parentHtml = parent;
                this.configuration = configuration;
                if (configuration.isReadOnly === undefined) {
                    configuration.isReadOnly = true;
                }
                if (configuration.allowAddingNewProperties === undefined) {
                    configuration.allowAddingNewProperties = false;
                }
                let tr;
                let table;
                const tthis = this;
                parent.empty();
                this.fieldElements = new Array();
                const fields = this.formElement.getAsArray("field");
                table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
                const tableBody = $("<tbody><tr><th>Name</th><th>Value</th></tr></tbody>");
                table.append(tableBody);
                const itemUri = this.itemId === undefined
                    ? ""
                    : tthis.itemId.indexOf('#') === -1
                        ? tthis.extentUri + "#" + tthis.itemId
                        : tthis.itemId;
                // Creates the fields for the item
                for (let n in fields) {
                    if (!fields.hasOwnProperty(n))
                        continue;
                    const field = fields[n];
                    const fieldMetaClassId = field.metaClass.id;
                    const fieldMetaClassUri = field.metaClass.uri;
                    let fieldElement = null; // The instance if IFormField allowing to create the dom
                    let htmlElement; // The dom that had been created... 
                    // Creates the field to be shown 
                    fieldElement = (0, FieldFactory_1.createField)(fieldMetaClassUri, {
                        configuration: configuration,
                        field: field,
                        itemUrl: itemUri,
                        isReadOnly: configuration.isReadOnly,
                        form: this
                    });
                    const singleColumn = (fieldElement === null || fieldElement === void 0 ? void 0 : fieldElement.showNameField) === undefined ? false : fieldElement.showNameField();
                    // Creates the row
                    if (singleColumn) {
                        tr = $("<tr><td class='value' colspan='2'></td></tr>");
                    }
                    else {
                        tr = $("<tr><td class='key'></td><td class='value'></td></tr>");
                    }
                    // Creates the key column content
                    if (!singleColumn) {
                        let name = (_a = field.get(DatenMeister_class_1._DatenMeister._Forms._FieldData.title)) !== null && _a !== void 0 ? _a : field.get(DatenMeister_class_1._DatenMeister._Forms._FieldData.name);
                        const isReadOnly = field.get(DatenMeister_class_1._DatenMeister._Forms._FieldData.isReadOnly);
                        if (isReadOnly) {
                            name += " [R]";
                        }
                        $(".key", tr).text(name);
                    }
                    // Creates the value column content
                    if (fieldElement === null) {
                        // No field element was created.
                        htmlElement = $("<em></em>");
                        htmlElement.text(fieldMetaClassId !== null && fieldMetaClassId !== void 0 ? fieldMetaClassId : "unknown");
                        $(".value", tr).append(fieldElement);
                    }
                    else {
                        fieldElement.field = field;
                        fieldElement.isReadOnly = configuration.isReadOnly;
                        fieldElement.form = this;
                        fieldElement.itemUrl = itemUri;
                        htmlElement = fieldElement.createDom(this.element);
                        // Pushes the field to the internal field list, so the data can be retrieved afterwards
                        this.fieldElements.push(fieldElement);
                        // We have to create a function which is then executed within the closure. 
                        ((trInner, htmlElementInner) => {
                            htmlElementInner.then(x => {
                                // And finally adds it            
                                $(".value", trInner).append(x);
                            });
                        })(tr, htmlElement);
                    }
                    tableBody.append(tr);
                }
                // Checks, if user may add additional properties, if yes, include a button and create the corresponding
                // logic
                if (!configuration.isReadOnly && configuration.allowAddingNewProperties) {
                    tr = $("<tr class='dm-row-newproperty'><td></td><td><button class='btn btn-secondary'>Add new property</button></td></tr>");
                    tableBody.append(tr);
                    const button = $("button", tr);
                    button.on('click', () => {
                        const newRow = $("<tr><td><input class='dm-textfield-key' type='text' /></td><td class='dm-row-value'></td></tr>");
                        const rowValue = $(".dm-row-value", newRow);
                        const propertyTextField = $(".dm-textfield-key", newRow);
                        const textField = new TextField.Field();
                        textField.field = new Mof.DmObject();
                        textField.form = tthis;
                        textField.isReadOnly = configuration.isReadOnly;
                        textField.OverridePropertyValue =
                            () => {
                                return propertyTextField.val().toString();
                            };
                        textField.createDom(tthis.element).then(x => {
                            rowValue.append(x);
                        });
                        tthis.fieldElements.push(textField);
                        newRow.insertBefore($('.dm-row-newproperty'));
                        propertyTextField.trigger('focus');
                    });
                }
                // Checks, if the form is a read-only form. If it is not a read-only, create the Accept and Reject buttons
                if (!configuration.isReadOnly) {
                    // Add the Cancel and Submit buttons at the end of the creation to the table
                    // allowing the cancelling and setting of the properties
                    const submitName = (_b = configuration.submitName) !== null && _b !== void 0 ? _b : "Save";
                    const cancelButton = $("<button class='btn btn-secondary dm-detail-form-cancel'>Cancel</button>");
                    const saveButton = $("<button class='btn btn-primary dm-detail-form-save'></button>").text(submitName);
                    const saveAndCloseButton = $("<button class='btn btn-primary dm-detail-form-save-and-close'>Save &amp; Close</button>");
                    tr = $("<tr><td></td><td class='dm-form-submitbutton-cell'></td></tr>");
                    const formSubmitButtonsCell = $('.dm-form-submitbutton-cell', tr);
                    if (configuration.showCancelButton !== false) {
                        formSubmitButtonsCell.append(cancelButton);
                    }
                    formSubmitButtonsCell.append(saveButton);
                    if (configuration.submitName === undefined) {
                        formSubmitButtonsCell.append(saveAndCloseButton);
                    }
                    tableBody.append(tr);
                    $(".dm-detail-form-cancel", tr).on('click', () => {
                        if (tthis.onCancel !== undefined && tthis.onCancel !== null) {
                            tthis.onCancel();
                        }
                    });
                    function saveHelper(method) {
                        if (tthis.onChange !== undefined && tthis.onCancel !== null) {
                            const saveElement = new Mof.DmObject();
                            for (let m in tthis.fieldElements) {
                                if (!tthis.fieldElements.hasOwnProperty(m))
                                    continue;
                                const fieldElement = tthis.fieldElements[m];
                                if (fieldElement.field.get(DatenMeister_class_1._DatenMeister._Forms._FieldData.isReadOnly, Mof.ObjectType.Boolean) !== true) {
                                    // Just take the fields which are not readonly
                                    fieldElement.evaluateDom(tthis.element);
                                    // Now evaluates the field and put only the properties being shown
                                    // into the DmObject to avoid overwriting of protected and non-shown properties
                                    fieldElement.evaluateDom(saveElement);
                                }
                            }
                            tthis.onChange(saveElement, method);
                        }
                    }
                    $(".dm-detail-form-save", tr).on('click', () => {
                        saveHelper(SubmitMethod.Save);
                    });
                    $(".dm-detail-form-save-and-close", tr).on('click', () => {
                        saveHelper(SubmitMethod.SaveAndClose);
                    });
                }
                parent.append(table);
                const tableInfo = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
                tableInfo.append($("<tr><th>URL</th><td class='dm-detail-info-uri'>U</td></tr>"));
                tableInfo.append($("<tr><th>Workspace</th><td class='dm-detail-info-workspace'>W</td></tr>"));
                tableInfo.append($("<tr><th>Extent-Uri</th><td class='dm-detail-info-extenturi'>E</td></tr>"));
                tableInfo.append($("<tr><th>Metaclass</th><td class='dm-detail-info-metaclass'>m</td></tr>"));
                $(".dm-detail-info-uri", tableInfo).text((_c = this.element.uri) !== null && _c !== void 0 ? _c : "none");
                $(".dm-detail-info-workspace", tableInfo).text((_d = this.element.workspace) !== null && _d !== void 0 ? _d : "none");
                $(".dm-detail-info-extenturi", tableInfo).text((_e = this.element.extentUri) !== null && _e !== void 0 ? _e : "none");
                $(".dm-detail-info-metaclass", tableInfo).text((_g = (_f = this.element.metaClass) === null || _f === void 0 ? void 0 : _f.fullName) !== null && _g !== void 0 ? _g : "none");
                parent.append(tableInfo);
            });
        }
    }
    exports.RowForm = RowForm;
});
//# sourceMappingURL=RowForm.js.map