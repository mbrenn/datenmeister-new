import * as Mof from "../Mof.js";
import { createField } from "./FieldFactory.js";
import * as Navigation from "../Navigator.js";
import * as TextField from "../fields/TextField.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import { SubmitMethod } from "./Forms.js";
import * as ClientItem from "../client/Items.js";
class FieldInForm {
    setCheckboxState(isSet) {
        this.checkbox.prop("checked", isSet);
        if (isSet) {
            this.checkbox.attr("title", "The checkbox is set because the value itself is set");
        }
        else {
            this.checkbox.attr("title", "The checkbox is not set because the value itself is unset");
        }
    }
    getCheckboxState() {
        if (this.checkbox !== null) {
            return this.checkbox.prop("checked") !== false;
        }
        return false;
    }
    addChangeCallbackToFieldElement(callback) {
        if (this.fieldElement.callbackUpdateField === undefined) {
            this.fieldElement.callbackUpdateField = callback;
        }
        else {
            this.fieldElement.callbackUpdateField = () => {
                callback();
                this.fieldElement.callbackUpdateField();
            };
        }
    }
}
export class RowForm {
    async refreshForm() {
        this.createFormByObject(this.parentHtml, this.configuration);
    }
    async createFormByObject(parent, configuration) {
        this.parentHtml = parent;
        this.configuration = configuration;
        // Connect the events
        if (configuration.onCancel !== undefined) {
            this.onCancel = configuration.onCancel;
        }
        if (configuration.onSubmit !== undefined) {
            this.onChange = configuration.onSubmit;
        }
        // Sets pre-defined configuration
        if (configuration.isReadOnly === undefined) {
            configuration.isReadOnly = true;
        }
        if (configuration.allowAddingNewProperties === undefined) {
            configuration.allowAddingNewProperties = false;
        }
        // Creates the table itself
        let tr;
        let table;
        const tthis = this;
        parent.empty();
        this.fieldElements = new Array();
        const fields = this.formElement.getAsArray("field");
        table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top dm-rowform'></table>");
        const tableBody = $("<tbody><tr>" +
            "<th>Name</th>" +
            "<th>Value</th>" +
            "<th title='Indicates whether the value is set in the data object'>Set?</th>" +
            "</tr></tbody>");
        table.append(tableBody);
        const itemUri = this.itemUrl === undefined
            ? ""
            : tthis.itemUrl.indexOf('#') === -1
                ? tthis.extentUri + "#" + tthis.itemUrl
                : tthis.itemUrl;
        // Creates the fields for the item
        for (let n in fields) {
            if (!fields.hasOwnProperty(n))
                continue;
            const field = fields[n];
            const fieldMetaClassId = field.metaClass?.id;
            const fieldMetaClassUri = field.metaClass?.uri ?? "Undefined metaclass";
            let fieldElement = null; // The instance if IFormField allowing to create the dom
            let htmlElement; // The dom that had been created...
            const isFieldReadOnly = field.get(_DatenMeister._Forms._FieldData.isReadOnly, Mof.ObjectType.Boolean)
                || configuration.isReadOnly;
            // Creates the field to be shown 
            fieldElement = createField(fieldMetaClassUri, {
                configuration: configuration,
                field: field,
                itemUrl: itemUri,
                isReadOnly: isFieldReadOnly,
                form: this
            });
            const singleColumn = fieldElement?.showNameField === undefined ? false : fieldElement.showNameField();
            // Creates the row
            if (singleColumn) {
                tr = $("<tr><td class='value' colspan='3'></td></tr>");
            }
            else {
                tr = $("<tr>" +
                    "<td class='key'></td>" +
                    "<td class='value'></td>" +
                    "<td class='isset'><input type='checkbox' class='checkbox_isset' /></td>" +
                    "</tr>");
            }
            const checkbox = $(".checkbox_isset", tr);
            // Creates the key column content
            if (!singleColumn) {
                let name = field.get(_DatenMeister._Forms._FieldData.title);
                if (name === undefined || name === null || name === "") {
                    name = field.get(_DatenMeister._Forms._FieldData.name);
                }
                if (isFieldReadOnly) {
                    // name += " [R]";
                }
                $(".key", tr).text(name);
            }
            // Creates the value column content
            if (fieldElement === null) {
                // No field element was created.
                htmlElement = $("<em></em>");
                htmlElement.text(fieldMetaClassId ?? "unknown");
                $(".value", tr).append(fieldElement);
                checkbox.prop("disabled", true);
            }
            else {
                fieldElement.field = field;
                fieldElement.isReadOnly = isFieldReadOnly;
                fieldElement.form = this;
                fieldElement.itemUrl = itemUri;
                htmlElement = fieldElement.createDom(this.element);
                // Pushes the field to the internal field list, so the data can be retrieved afterwards
                const fieldInForm = new FieldInForm();
                fieldInForm.fieldElement = fieldElement;
                fieldInForm.field = field;
                fieldInForm.checkbox = checkbox;
                this.fieldElements.push(fieldInForm);
                // We have to create a function which is then executed within the closure. 
                ((trInner, htmlElementInner, innerFieldInForm) => {
                    htmlElementInner.then(x => {
                        // And finally adds it            
                        $(".value", trInner).append(x);
                        innerFieldInForm.checkbox.prop("disabled", isFieldReadOnly);
                        const propertyName = innerFieldInForm.field.get(_DatenMeister._Forms._FieldData._name_, Mof.ObjectType.String);
                        const showValue = innerFieldInForm.fieldElement.showValue === undefined
                            || innerFieldInForm.fieldElement.showValue();
                        if (propertyName === undefined || propertyName === null || propertyName === "" || !showValue) {
                            innerFieldInForm.checkbox.hide();
                            innerFieldInForm.checkbox = null;
                        }
                        else {
                            const isSet = this.element.isSet(propertyName);
                            innerFieldInForm.setCheckboxState(isSet);
                            innerFieldInForm.addChangeCallbackToFieldElement(() => {
                                innerFieldInForm.setCheckboxState(true);
                            });
                        }
                    });
                })(tr, htmlElement, fieldInForm);
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
                const fieldInformation = new FieldInForm();
                fieldInformation.fieldElement = textField;
                fieldInformation.field = textField.field;
                tthis.fieldElements.push(fieldInformation);
                newRow.insertBefore($('.dm-row-newproperty'));
                propertyTextField.trigger('focus');
            });
        }
        // Checks, if the form is a read-only form. If it is not a read-only, create the Accept and Reject buttons
        if (!configuration.isReadOnly) {
            // Add the Cancel and Submit buttons at the end of the creation to the table
            // allowing the cancelling and setting of the properties
            const submitName = configuration.submitName ?? "Save";
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
            $(".dm-detail-form-save", tr).on('click', async () => {
                const result = await this.storeFormValuesIntoDom();
                tthis.onChange(result, SubmitMethod.Save);
            });
            $(".dm-detail-form-save-and-close", tr).on('click', async () => {
                const result = await this.storeFormValuesIntoDom();
                tthis.onChange(result, SubmitMethod.SaveAndClose);
            });
        }
        parent.append(table);
        const tableInfo = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
        tableInfo.append($("<tr><th>ID</th><td class='dm-detail-info-id'><span class='dm-detail-info-id-value'>I</span>" +
            "<button class='dm-detail-info-id-edit btn btn-secondary' type='button'>Edit</button></td></tr>"));
        tableInfo.append($("<tr><th>URL</th><td><span class='dm-detail-info-uri'>U</span></td></tr>"));
        tableInfo.append($("<tr><th>Workspace</th><td><a class='dm-detail-info-workspace'>W</a></td></tr>"));
        tableInfo.append($("<tr><th>Extent-Uri</th><td><a class='dm-detail-info-extenturi'>E</a></td></tr>"));
        tableInfo.append($("<tr><th>Metaclass</th><td><a class='dm-detail-info-metaclass'>m</a></td></tr>"));
        $(".dm-detail-info-id-value", tableInfo).text((this.element.id) ?? "none");
        let isEditing = false;
        let editField = null;
        $(".dm-detail-info-id-edit", tableInfo).on('click', async () => {
            if (!isEditing) {
                editField = $("<input type='text' class='dm-detail-info-id-value-field' />");
                editField.val(this.element.id);
                $(".dm-detail-info-id-value", tableInfo).empty().append(editField);
                $(".dm-detail-info-id-edit", tableInfo).text('Change ID');
            }
            else {
                const oldId = this.element.id;
                const newId = editField.val().toString();
                if (newId === '') {
                    $(".dm-detail-info-id", tableInfo).append("<span class='text-warning'>An empty id cannot be set.</span>");
                    return;
                }
                this.element.id = editField.val().toString();
                if (oldId !== this.element.id) {
                    try {
                        // We only need to update the text in case the id changes
                        await ClientItem.setId(this.element.workspace, this.element.uri, this.element.id);
                        // Since the id got changed, we navigate to the new id to avoid any mishap
                        Navigation.navigateToItem(this.element.workspace, this.element.extentUri, this.element.id);
                    }
                    catch (e) {
                        if (e.toString().includes("IdIsAlreadySetException")) {
                            $(".dm-detail-info-id", tableInfo).append("<span class='text-warning'>The ID is already set within that extent.</span>");
                        }
                        else {
                            $(".dm-detail-info-id", tableInfo).append(e.toString());
                        }
                    }
                }
                $(".dm-detail-info-id-value", tableInfo).text(this.element.id);
            }
            isEditing = !isEditing;
        });
        if (this.element.uri !== undefined && this.element.uri !== "") {
            $(".dm-detail-info-uri", tableInfo).text(this.element.uri);
            const copy = $("<a href='#' class='nounderline'>📋</a>");
            copy.on('click', () => {
                navigator.clipboard.writeText(this.element.uri);
                alert('Url copied to Clipboard');
                return false;
            });
            $(".dm-detail-info-uri", tableInfo).append(copy);
        }
        if (this.element.workspace === undefined) {
            $(".dm-detail-info-workspace", tableInfo).text("none");
            $(".dm-detail-info-workspace", tableInfo).addClass("nolink");
        }
        else {
            $(".dm-detail-info-workspace", tableInfo).text(this.element.workspace);
            $(".dm-detail-info-workspace", tableInfo).attr('href', Navigation.getLinkForNavigateToWorkspace(this.element.workspace));
        }
        if (this.element.workspace === undefined || this.element.extentUri === undefined) {
            $(".dm-detail-info-extenturi", tableInfo).text("none");
            $(".dm-detail-info-extenturi", tableInfo).addClass("nolink");
        }
        else {
            $(".dm-detail-info-extenturi", tableInfo).text(this.element.extentUri);
            $(".dm-detail-info-extenturi", tableInfo).attr('href', Navigation.getLinkForNavigateToExtentProperties(this.element.workspace, this.element.extentUri));
        }
        if (this.element.metaClass?.workspace === undefined ||
            this.element.metaClass.extentUri === undefined ||
            this.element.metaClass.id === undefined) {
            $(".dm-detail-info-metaclass", tableInfo).text("none");
            $(".dm-detail-info-metaclass", tableInfo).addClass("nolink");
        }
        else {
            $(".dm-detail-info-metaclass", tableInfo).text(this.element.metaClass?.fullName);
            $(".dm-detail-info-metaclass", tableInfo).attr('href', Navigation.getLinkForNavigateToItem(this.element.metaClass.workspace, this.element.metaClass.extentUri, this.element.metaClass.id));
        }
        parent.append(tableInfo);
    }
    async storeFormValuesIntoDom() {
        if (this.onChange !== undefined && this.onCancel !== null) {
            for (let m in this.fieldElements) {
                if (!this.fieldElements.hasOwnProperty(m))
                    continue;
                const fieldInForm = this.fieldElements[m];
                let managed = false;
                // Just take the fields which are not readonly
                if (fieldInForm.field.get(_DatenMeister._Forms._FieldData.isReadOnly, Mof.ObjectType.Boolean) !== true) {
                    // Unsets the field in case the checkbox is not set
                    if (fieldInForm.getCheckboxState() === false) {
                        this.element.unset(fieldInForm.field.get(_DatenMeister._Forms._FieldData._name_, Mof.ObjectType.String));
                        managed = true;
                    }
                    if (!managed) {
                        // Evaluate the field element to allow the element storing the properties.
                        await fieldInForm.fieldElement.evaluateDom(this.element);
                    }
                }
            }
            return this.element;
        }
    }
}
//# sourceMappingURL=RowForm.js.map