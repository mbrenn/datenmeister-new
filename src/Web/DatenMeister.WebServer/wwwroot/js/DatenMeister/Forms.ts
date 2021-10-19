import * as Mof from "./Mof";
import * as DataLoader from "./DataLoader";
import DmObject = Mof.DmObject;
import * as ApiConnection from "./ApiConnection";
import * as ApiModels from "./ApiModels";
import * as Settings from "./Settings";
import {DetailFormActions} from "./FormActions";
import {injectNameByUri} from "./DomHelper";

export class Form {
    viewMode: string;
}

export interface IForm
{
    workspace: string;
    extentUri: string;
    itemId: string;
    element: DmObject;
    formElement: DmObject;
}

export class FormCreator implements IForm {

    element: DmObject;
    extentUri: string;
    formElement: DmObject;
    itemId: string;
    workspace: string;

    createFormByObject(parent: JQuery<HTMLElement>, isReadOnly: boolean) {
        let table;
        const tthis = this;
        parent.empty();

        const tabs = this.formElement.get("tab");
        for (let n in tabs) {
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }

            const tab = tabs[n] as DmObject;
            if (tab.metaClass.id == "DatenMeister.Models.Forms.DetailForm") {
                const detailForm = new DetailForm();
                detailForm.workspace = this.workspace;
                detailForm.extentUri = this.extentUri;
                detailForm.itemId = this.itemId;
                detailForm.formElement = tab;
                detailForm.element = this.element;
                
                detailForm.createFormByObject(parent, isReadOnly);
            }
        }
    }
}


export class DetailForm implements IForm {
    workspace: string;
    extentUri: string;
    itemId: string;
    element: DmObject;
    formElement: DmObject;
    
    fieldElements: Array<IFormField>;
    
    onCancel: () => void;
    onChange: (element: DmObject) => void;
    
    createFormByObject(parent: JQuery<HTMLElement>, isReadOnly: boolean) {
        let table;
        const tthis = this;
        parent.empty();
        this.fieldElements = new Array<IFormField>();

        const tabs = this.formElement.get("tab");
        for (let n in tabs) {
            if (!tabs.hasOwnProperty(n)) {
                continue;
            }

            const tab = tabs[n] as DmObject;
            if (tab.metaClass.id == "DatenMeister.Models.Forms.DetailForm") {
                const fields = tab.get("field");

                table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
                const tableBody = $("<tbody><tr><th>Name</th><th>Value</th></tr>");
                table.append(tableBody);

                for (let n in fields) {
                    if (!fields.hasOwnProperty(n)) continue;
                    const field = fields[n] as DmObject;
                    var tr = $("<tr><td class='key'></td><td class='value'></td></tr>");

                    const name =
                        field.get("title") as any as string ??
                        field.get("name") as any as string;

                    $(".key", tr).text(name);

                    const fieldMetaClassId = field.metaClass.id;
                    let fieldElement = null; // The instance if IFormField allowing to create the dom
                    let htmlElement; // The dom that had been created... 
                    switch (fieldMetaClassId) {
                        case "DatenMeister.Models.Forms.TextFieldData":
                            fieldElement = new TextField();
                            break;
                        case "DatenMeister.Models.Forms.MetaClassElementFieldData":
                            fieldElement = new MetaClassElementField();
                            break;
                        case "DatenMeister.Models.Forms.CheckboxFieldData":
                            fieldElement = new CheckboxField();
                            break;
                        case "DatenMeister.Models.Forms.DropDownFieldData":
                            fieldElement = new DropDownField();
                            break;
                        case "DatenMeister.Models.Forms.ActionFieldData":
                            fieldElement = new ActionField();
                            break;
                    }

                    if (fieldElement === null) {
                        // No field element was created.
                        htmlElement = $("<em></em>");
                        htmlElement.text(fieldMetaClassId ?? "unknown");
                        $(".value", tr).append(fieldElement);
                    } else {
                        fieldElement.field = field;
                        fieldElement.isReadOnly = isReadOnly;
                        fieldElement.form = this;

                        htmlElement = fieldElement.createDom(this.element);
                    }
                    
                    this.fieldElements.push(fieldElement);

                    $(".value", tr).append(htmlElement);
                    tableBody.append(tr);
                }

                if (!isReadOnly) {
                    // Add the Cancel and Submit buttons at the end of the creation to the table
                    // allowing the cancelling and setting of the properties
                    tr = $("<tr><td></td><td><button class='btn btn-secondary'>Cancel" +
                        "<button class='btn btn-primary'>Submit</button></td></tr>");
                    tableBody.append(tr);

                    $(".btn-secondary", tr).on('click', () => {
                        if (tthis.onCancel !== undefined && tthis.onCancel !== null) {
                            tthis.onCancel();
                        }
                    });

                    $(".btn-primary", tr).on('click', () => {
                        if (tthis.onChange !== undefined && tthis.onCancel !== null) {
                            for (let m in tthis.fieldElements) {
                                if (!tthis.fieldElements.hasOwnProperty(m)) continue;

                                const fieldElement = tthis.fieldElements[m];
                                fieldElement.evaluateDom(tthis.element);
                            }

                            tthis.onChange(tthis.element);
                        }
                    });
                }
            } // DetailForm
            else {
                table = $("<div>Unknown Formtype:<span class='id'></span></div> ");
                $(".id", table).text(tab.metaClass.id);
            }

            parent.append(table);
        }
    }
    
}

/* 
Describes the detailform including the connection to the webserver to download and upload forms.
It also includes the basic navigation to edit, view and submit the item changes
 */
export class MofDetailForm extends DetailForm implements IForm {

    createViewForm(parent: JQuery<HTMLElement>, workspace: string, extentUri: string, uri: string) {
        this.createForm(parent, workspace, extentUri, uri, true);
    }

    createEditForm(parent: JQuery<HTMLElement>, workspace: string, extentUri: string, uri: string) {
        this.createForm(parent, workspace, extentUri, uri, false);
    }

    createForm(parent: JQuery<HTMLElement>, workspace: string, extentUri: string, itemId: string, isReadOnly: boolean) {
        const tthis = this;

        // Load the object
        const defer1 = DataLoader.loadObjectByUri(workspace, itemId);

        // Load the form
        const defer2 = getDefaultFormForItem(workspace, itemId, "");

        // Wait for both
        $.when(defer1, defer2).then(function (element, form) {
            tthis.element = element;
            tthis.formElement = form;
            tthis.workspace = workspace;
            tthis.itemId = itemId;
            tthis.createFormByObject(parent, isReadOnly);
        });
        
        this.onCancel = () =>{
            tthis.createViewForm(parent, workspace, extentUri, itemId);
        }
        
        this.onChange = (element)=> {
            DataLoader.storeObjectByUri(workspace, itemId, tthis.element).done(
                () => {
                    tthis.createViewForm(parent, workspace, extentUri, itemId);
                }
            );
        }

        parent.empty();
        parent.text("Loading content and form...");
    }
}

export function getDefaultFormForItem(workspace: string, item: string,  viewMode: string): JQuery.Deferred<Mof.DmObject, never, never> {
    var r = jQuery.Deferred<Mof.DmObject, never, never>();

    ApiConnection.get<ApiModels.Out.IItem>(
        Settings.baseUrl +
        "api/forms/default_for_item/" +
        encodeURIComponent(workspace) +
        "/" +
        encodeURIComponent(item) +
        "/" +
        encodeURIComponent(viewMode)
    ).done(x => {
        const dmObject =
            Mof.createObjectFromJson(x.item, x.metaClass);
        r.resolve(dmObject);
    });

    return r;
}

export interface IFormField
{
    // Defines the field to be used to create the dom for the field
    field: DmObject;
    
    isReadOnly: boolean;
    
    form: IForm;

    // Creates the dom depending on the given field and the internal object
    createDom(dmElement: DmObject) : JQuery<HTMLElement>;

    // Evaluates the result of the user and injects it into the given element
    evaluateDom(dmElement:DmObject);
}

export class BaseField
{
    field: DmObject;
    isReadOnly: boolean;
    form: IForm;
}

export class TextField extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    createDom(dmElement: Mof.DmObject) {
        var fieldName = this.field.get('name').toString();
        if (this.isReadOnly) {
            const div = $("<div />");
            div.text(dmElement.get(fieldName)?.toString() ?? "unknown");
            return div;
        } else {
            this._textBox = $("<input />");
            this._textBox.val(dmElement.get(fieldName)?.toString() ?? "unknown");
            
            return this._textBox;
        }
    }

    evaluateDom(dmElement: Mof.DmObject) {
        if (this._textBox !== undefined && this._textBox !== null)
        {
            var fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._textBox.val());
        }
    }
}

export class MetaClassElementField extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    createDom(dmElement: Mof.DmObject) {

        const div = $("<div />");
        if (dmElement.metaClass !== undefined && dmElement.metaClass !== null) {
            div.text(dmElement.metaClass.id);
        } else {
            div.text("unknown");
        }

        injectNameByUri(div, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
        return div;
    }

    evaluateDom(dmElement: Mof.DmObject) {
    }
}

export class CheckboxField extends BaseField implements IFormField {
    _checkbox: JQuery<HTMLElement>;

    createDom(dmElement: DmObject): JQuery<HTMLElement> {

        this._checkbox = $("<input type='checkbox'></input>");

        var fieldName = this.field.get('name').toString();
        if (dmElement.get(fieldName)) {
            this._checkbox.prop('checked', true);
        }

        if (this.isReadOnly) {
            this._checkbox.prop('disabled', 'disabled');
        }

        return this._checkbox;
    }

    evaluateDom(dmElement: DmObject) {
        if (this._checkbox !== undefined && this._checkbox !== null) {
            var fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._checkbox.prop('checked'));
        }
    }
}

export class ActionField extends BaseField implements IFormField {
    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        var tthis = this;
        var title = this.field.get('title');
        var action = this.field.get('actionName');

        var result = $("<button class='btn btn-secondary' type='button'></button>");
        result.text(title);

        result.on('click', () => {
            DetailFormActions.execute(action, tthis.form, dmElement);
        });

        return result;
    }

    evaluateDom(dmElement: DmObject) {

    }
}

export class DropDownField extends BaseField implements IFormField {
    _selectBox: JQuery<HTMLElement>;

    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        var fieldName = this.field.get('name').toString();
        var selectedValue = dmElement.get(fieldName);
        var values = this.field.get('values') as Array<DmObject>;

        this._selectBox = $("<select></select>");
        for (const value of values) {
            var option = $("<option></option>");
            option.val(value.get('value').toString());
            option.text(value.get('name').toString());
            this._selectBox.append(option);
        }

        this._selectBox.val(selectedValue);
        if (this.isReadOnly) {
            this._selectBox.prop('disabled', 'disabled');
        }

        return this._selectBox;
    }

    evaluateDom(dmElement: DmObject) {

        var fieldName = this.field.get('name').toString();
        dmElement.set(fieldName, this._selectBox.val());
    }
}