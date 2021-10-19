var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
define(["require", "exports", "./Mof", "./DataLoader", "./ApiConnection", "./Settings", "./FormActions", "./DomHelper"], function (require, exports, Mof, DataLoader, ApiConnection, Settings, FormActions_1, DomHelper_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.DropDownField = exports.ActionField = exports.CheckboxField = exports.MetaClassElementField = exports.TextField = exports.BaseField = exports.getDefaultFormForItem = exports.MofDetailForm = exports.DetailForm = exports.Form = void 0;
    Mof = __importStar(Mof);
    DataLoader = __importStar(DataLoader);
    ApiConnection = __importStar(ApiConnection);
    Settings = __importStar(Settings);
    class Form {
    }
    exports.Form = Form;
    class DetailForm {
        createFormByObject(parent, isReadOnly) {
            var _a;
            let table;
            const tthis = this;
            parent.empty();
            this.fieldElements = new Array();
            const tabs = this.formElement.get("tab");
            for (let n in tabs) {
                if (!tabs.hasOwnProperty(n)) {
                    continue;
                }
                const tab = tabs[n];
                if (tab.metaClass.id == "DatenMeister.Models.Forms.DetailForm") {
                    const fields = tab.get("field");
                    table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
                    const tableBody = $("<tbody><tr><th>Name</th><th>Value</th></tr>");
                    table.append(tableBody);
                    for (let n in fields) {
                        if (!fields.hasOwnProperty(n))
                            continue;
                        const field = fields[n];
                        var tr = $("<tr><td class='key'></td><td class='value'></td></tr>");
                        const name = (_a = field.get("title")) !== null && _a !== void 0 ? _a : field.get("name");
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
                            htmlElement.text(fieldMetaClassId !== null && fieldMetaClassId !== void 0 ? fieldMetaClassId : "unknown");
                            $(".value", tr).append(fieldElement);
                        }
                        else {
                            fieldElement.field = field;
                            fieldElement.isReadOnly = isReadOnly;
                            fieldElement.form = this;
                            htmlElement = fieldElement.createDom(this.element);
                        }
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
                                    if (!tthis.fieldElements.hasOwnProperty(m))
                                        continue;
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
    exports.DetailForm = DetailForm;
    /*
    Describes the detailform including the connection to the webserver to download and upload forms.
    It also includes the basic navigation to edit, view and submit the item changes
     */
    class MofDetailForm extends DetailForm {
        createViewForm(parent, workspace, extentUri, uri) {
            this.createForm(parent, workspace, extentUri, uri, true);
        }
        createEditForm(parent, workspace, extentUri, uri) {
            this.createForm(parent, workspace, extentUri, uri, false);
        }
        createForm(parent, workspace, extentUri, itemId, isReadOnly) {
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
            this.onCancel = () => {
                tthis.createViewForm(parent, workspace, extentUri, itemId);
            };
            this.onChange = (element) => {
                DataLoader.storeObjectByUri(workspace, itemId, tthis.element).done(() => {
                    tthis.createViewForm(parent, workspace, extentUri, itemId);
                });
            };
            parent.empty();
            parent.text("Loading content and form...");
        }
    }
    exports.MofDetailForm = MofDetailForm;
    function getDefaultFormForItem(workspace, item, viewMode) {
        var r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl +
            "api/forms/default_for_item/" +
            encodeURIComponent(workspace) +
            "/" +
            encodeURIComponent(item) +
            "/" +
            encodeURIComponent(viewMode)).done(x => {
            const dmObject = Mof.createObjectFromJson(x.item, x.metaClass);
            r.resolve(dmObject);
        });
        return r;
    }
    exports.getDefaultFormForItem = getDefaultFormForItem;
    class BaseField {
    }
    exports.BaseField = BaseField;
    class TextField extends BaseField {
        createDom(dmElement) {
            var _a, _b, _c, _d;
            var fieldName = this.field.get('name').toString();
            if (this.isReadOnly) {
                const div = $("<div />");
                div.text((_b = (_a = dmElement.get(fieldName)) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "unknown");
                return div;
            }
            else {
                this._textBox = $("<input />");
                this._textBox.val((_d = (_c = dmElement.get(fieldName)) === null || _c === void 0 ? void 0 : _c.toString()) !== null && _d !== void 0 ? _d : "unknown");
                return this._textBox;
            }
        }
        evaluateDom(dmElement) {
            if (this._textBox !== undefined && this._textBox !== null) {
                var fieldName = this.field.get('name').toString();
                dmElement.set(fieldName, this._textBox.val());
            }
        }
    }
    exports.TextField = TextField;
    class MetaClassElementField extends BaseField {
        createDom(dmElement) {
            const div = $("<div />");
            if (dmElement.metaClass !== undefined && dmElement.metaClass !== null) {
                div.text(dmElement.metaClass.id);
            }
            else {
                div.text("unknown");
            }
            DomHelper_1.injectNameByUri(div, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
            return div;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.MetaClassElementField = MetaClassElementField;
    class CheckboxField extends BaseField {
        createDom(dmElement) {
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
        evaluateDom(dmElement) {
            if (this._checkbox !== undefined && this._checkbox !== null) {
                var fieldName = this.field.get('name').toString();
                dmElement.set(fieldName, this._checkbox.prop('checked'));
            }
        }
    }
    exports.CheckboxField = CheckboxField;
    class ActionField extends BaseField {
        createDom(dmElement) {
            var tthis = this;
            var title = this.field.get('title');
            var action = this.field.get('actionName');
            var result = $("<button class='btn btn-secondary' type='button'></button>");
            result.text(title);
            result.on('click', () => {
                FormActions_1.DetailFormActions.execute(action, tthis.form, dmElement);
            });
            return result;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.ActionField = ActionField;
    class DropDownField extends BaseField {
        createDom(dmElement) {
            var fieldName = this.field.get('name').toString();
            var selectedValue = dmElement.get(fieldName);
            var values = this.field.get('values');
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
        evaluateDom(dmElement) {
            var fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._selectBox.val());
        }
    }
    exports.DropDownField = DropDownField;
});
//# sourceMappingURL=Forms.js.map