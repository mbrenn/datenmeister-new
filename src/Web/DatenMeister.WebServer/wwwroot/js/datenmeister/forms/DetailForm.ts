﻿import * as InterfacesForms from "../forms/Interfaces";
import * as InterfacesFields from "../fields/Interfaces";
import * as Mof from "../Mof";
import {createField} from "./FieldFactory";
import * as TextField from "../fields/TextField"
import {IFormConfiguration} from "./IFormConfiguration";

// Defines the possible submit methods, a user can chose to close the detail form
export enum SubmitMethod
{
    // The user clicked on the save button
    Save,
    // The user clicked on the save and close button
    SaveAndClose
}
    
export class DetailForm implements InterfacesForms.IForm {
    workspace: string;
    extentUri: string;
    itemId: string;
    element: Mof.DmObject;
    formElement: Mof.DmObject;

    fieldElements: Array<InterfacesFields.IFormField>;

    onCancel: () => void;
    onChange: (element: Mof.DmObject, method: SubmitMethod) => void;
    parentHtml: JQuery<HTMLElement>;
    configuration: IFormConfiguration;

    refreshForm(): void {
        this.createFormByObject(this.parentHtml, this.configuration);
    }

    createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration) {
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
        this.fieldElements = new Array<InterfacesFields.IFormField>();

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
            if (!fields.hasOwnProperty(n)) continue;
            const field = fields[n] as Mof.DmObject;
            tr = $("<tr><td class='key'></td><td class='value'></td></tr>");

            const name =
                (field.get("title") as any as string) ??
                (field.get("name") as any as string);

            $(".key", tr).text(name);

            const fieldMetaClassId = field.metaClass.id;
            let fieldElement = null; // The instance if IFormField allowing to create the dom
            let htmlElement; // The dom that had been created... 

            fieldElement = createField(
                fieldMetaClassId,
                {
                    configuration: configuration,
                    field: field,
                    itemUrl: itemUri,
                    isReadOnly: configuration.isReadOnly,
                    form: this
                });

            if (fieldElement === null) {
                // No field element was created.
                htmlElement = $("<em></em>");
                htmlElement.text(fieldMetaClassId ?? "unknown");
                $(".value", tr).append(fieldElement);
            } else {
                fieldElement.field = field;
                fieldElement.isReadOnly = configuration.isReadOnly;
                fieldElement.form = this;
                fieldElement.itemUrl = itemUri;

                htmlElement = fieldElement.createDom(this.element);
            }

            this.fieldElements.push(fieldElement);

            $(".value", tr).append(htmlElement);
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
                rowValue.append(textField.createDom(tthis.element));

                tthis.fieldElements.push(textField);
                newRow.insertBefore($('.dm-row-newproperty'));

                propertyTextField.trigger('focus');
            });
        }

        // Checks, if the form is a read-only form. If it is not a read-only, create the Accept and Reject buttons
        if (!configuration.isReadOnly) {
            // Add the Cancel and Submit buttons at the end of the creation to the table
            // allowing the cancelling and setting of the properties
            tr = $("<tr><td></td><td><button class='btn btn-secondary dm-detail-form-cancel'>Cancel</button>" +
                "<button class='btn btn-primary dm-detail-form-save'>Save</button>" +
                "<button class='btn btn-primary dm-detail-form-save-and-close'>Save &amp; Close</button></td></tr>");
            tableBody.append(tr);

            $(".dm-detail-form-cancel", tr).on('click', () => {
                if (tthis.onCancel !== undefined && tthis.onCancel !== null) {
                    tthis.onCancel();
                }
            });

            function saveHelper(method: SubmitMethod) {
                if (tthis.onChange !== undefined && tthis.onCancel !== null) {
                    for (let m in tthis.fieldElements) {
                        if (!tthis.fieldElements.hasOwnProperty(m)) continue;

                        const fieldElement = tthis.fieldElements[m];
                        fieldElement.evaluateDom(tthis.element);
                    }

                    tthis.onChange(tthis.element, method);
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

        const tableInfo =
            $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
        tableInfo.append(
            $("<tr><th>URL</th><td class='dm-detail-info-uri'>U</td></tr>"));
        tableInfo.append(
            $("<tr><th>Workspace</th><td class='dm-detail-info-workspace'>W</td></tr>"));
        tableInfo.append(
            $("<tr><th>Extent-Uri</th><td class='dm-detail-info-extenturi'>E</td></tr>"));
        tableInfo.append(
            $("<tr><th>Metaclass</th><td class='dm-detail-info-metaclass'>m</td></tr>"));

        $(".dm-detail-info-uri", tableInfo).text(this.element.uri ?? "none");
        $(".dm-detail-info-workspace", tableInfo).text(this.element.workspace ?? "none");
        $(".dm-detail-info-extenturi", tableInfo).text(this.element.extentUri ?? "none");
        $(".dm-detail-info-metaclass", tableInfo).text(this.element.metaClass?.fullName ?? "none");
        parent.append(tableInfo);
    }
}