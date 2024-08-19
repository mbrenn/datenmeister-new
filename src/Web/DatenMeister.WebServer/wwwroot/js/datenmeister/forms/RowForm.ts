import * as InterfacesForms from "../forms/Interfaces.js";
import * as InterfacesFields from "../fields/Interfaces.js";
import * as Mof from "../Mof.js";
import { createField } from "./FieldFactory.js";
import * as Navigation from "../Navigator.js"
import * as TextField from "../fields/TextField.js"
import {IFormConfiguration} from "./IFormConfiguration.js";
import {_DatenMeister} from "../models/DatenMeister.class.js";
import {DmObject, DmObjectWithSync} from "../Mof.js";
import { SubmitMethod } from "./Forms.js";
    
export class RowForm implements InterfacesForms.IObjectFormElement {
    pageNavigation: InterfacesForms.IPageNavigation;
    workspace: string;
    extentUri: string;
    itemUrl: string;
    element: Mof.DmObjectWithSync;
    formElement: Mof.DmObject;

    fieldElements: Array<InterfacesFields.IFormField>;

    onCancel: () => void;
    onChange: (element: Mof.DmObject, method: SubmitMethod) => void;
    parentHtml: JQuery<HTMLElement>;
    configuration: IFormConfiguration;

    async refreshForm(): Promise<void> {
        this.createFormByObject(this.parentHtml, this.configuration);
    }

    async createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration) {
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
        this.fieldElements = new Array<InterfacesFields.IFormField>();

        const fields = this.formElement.getAsArray("field");

        table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top dm-rowform'></table>");
        const tableBody = $("<tbody><tr><th>Name</th><th>Value</th></tr></tbody>");
        table.append(tableBody);

        const itemUri = this.itemUrl === undefined
            ? ""
            : tthis.itemUrl.indexOf('#') === -1
                ? tthis.extentUri + "#" + tthis.itemUrl
                : tthis.itemUrl;

        // Creates the fields for the item
        for (let n in fields) {
            if (!fields.hasOwnProperty(n)) continue;
            const field = fields[n] as Mof.DmObject;

            const fieldMetaClassId = field.metaClass?.id;
            const fieldMetaClassUri = field.metaClass?.uri ?? "Undefined metaclass";
            let fieldElement = null; // The instance if IFormField allowing to create the dom
            let htmlElement; // The dom that had been created...
            const isFieldReadOnly = field.get(_DatenMeister._Forms._FieldData.isReadOnly, Mof.ObjectType.Boolean);
            
            // Creates the field to be shown 
            fieldElement = createField(
                fieldMetaClassUri,
                {
                    configuration: configuration,
                    field: field,
                    itemUrl: itemUri,
                    isReadOnly: configuration.isReadOnly,
                    form: this
                });

            const singleColumn = 
                fieldElement?.showNameField === undefined ? false : fieldElement.showNameField();

            // Creates the row
            if (singleColumn) {
                tr = $("<tr><td class='value' colspan='2'></td></tr>");
            } else {
                tr = $("<tr><td class='key'></td><td class='value'></td></tr>");
            }

            // Creates the key column content
            if (!singleColumn) {
                let name =
                    (field.get(_DatenMeister._Forms._FieldData.title) as any as string);

                if (name === undefined || name === null || name === "") {
                    name = (field.get(_DatenMeister._Forms._FieldData.name) as any as string);
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
            } else {
                fieldElement.field = field;
                fieldElement.isReadOnly = configuration.isReadOnly || isFieldReadOnly;
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

                textField.createDom(tthis.element).then(
                    x => {
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

        const tableInfo =
            $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
        tableInfo.append(
            $("<tr><th>URL</th><td class='dm-detail-info-uri'>U</td></tr>"));
        tableInfo.append(
            $("<tr><th>Workspace</th><td><a class='dm-detail-info-workspace'>W</a></td></tr>"));
        tableInfo.append(
            $("<tr><th>Extent-Uri</th><td><a class='dm-detail-info-extenturi'>E</a></td></tr>"));
        tableInfo.append(
            $("<tr><th>Metaclass</th><td><a class='dm-detail-info-metaclass'>m</a></td></tr>"));

        $(".dm-detail-info-uri", tableInfo).text((this.element.uri + " ")?? "none");
        if (this.element.uri !== undefined && this.element.uri !== "") {

            const copy = $("<a href='#' class='nounderline'>📋</a>");
            copy.on('click', () => {
                navigator.clipboard.writeText(this.element.uri);
                alert('Url copied to Clipboard');
                return false;
            });

            $(".dm-detail-info-uri", tableInfo).append(copy);
        }

        $(".dm-detail-info-workspace", tableInfo).text(this.element.workspace ?? "none");
        if (this.element.workspace !== undefined) {
            $(".dm-detail-info-workspace", tableInfo).attr('href', Navigation.getLinkForNavigateToWorkspace(this.element.workspace));
        }

        $(".dm-detail-info-extenturi", tableInfo).text(this.element.extentUri ?? "none");
        if (this.element.extentUri !== undefined) {
            $(".dm-detail-info-extenturi", tableInfo).attr('href', Navigation.getLinkForNavigateToExtent(this.element.workspace, this.element.extentUri));
        }

        $(".dm-detail-info-metaclass", tableInfo).text(this.element.metaClass?.fullName ?? "none");
        if (this.element.metaClass !== undefined) {
            $(".dm-detail-info-metaclass", tableInfo).attr('href', Navigation.getLinkForNavigateToItem(
                this.element.metaClass.workspace,
                this.element.metaClass.extentUri,
                this.element.metaClass.id));
        }
        parent.append(tableInfo);
    }

    async storeFormValuesIntoDom() : Promise<DmObjectWithSync> {
        if (this.onChange !== undefined && this.onCancel !== null) {

            for (let m in this.fieldElements) {
                if (!this.fieldElements.hasOwnProperty(m)) continue;

                const fieldElement = this.fieldElements[m];

                // Just take the fields which are not readonly
                if (fieldElement.field.get(_DatenMeister._Forms._FieldData.isReadOnly, Mof.ObjectType.Boolean) !== true) {

                    // Comment out to store the values only in the saveElement
                    await fieldElement.evaluateDom(this.element);
                }
            }

            return this.element;
        }
    }
}