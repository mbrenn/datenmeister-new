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
define(["require", "exports", "./Mof", "./DataLoader", "./ApiConnection", "./Settings", "./FormActions"], function (require, exports, Mof, DataLoader, ApiConnection, Settings, FormActions_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ActionField = exports.CheckboxField = exports.MetaClassElementField = exports.TextField = exports.BaseField = exports.getDefaultFormForItem = exports.DetailForm = exports.Form = void 0;
    Mof = __importStar(Mof);
    DataLoader = __importStar(DataLoader);
    ApiConnection = __importStar(ApiConnection);
    Settings = __importStar(Settings);
    class Form {
    }
    exports.Form = Form;
    class DetailForm {
        createViewForm(parent, workspace, uri) {
            var tthis = this;
            // Load the object
            var defer1 = DataLoader.loadObjectByUri(workspace, uri);
            // Load the form
            var defer2 = getDefaultFormForItem(workspace, uri, "");
            // Wait for both
            $.when(defer1, defer2).then(function (element, form) {
                tthis.element = element;
                tthis.formElement = form;
                tthis.workspace = workspace;
                tthis.uri = workspace;
                tthis.createFormByObject(parent, true);
            });
            parent.empty();
            parent.text("Loading content and form...");
        }
        createFormByObject(parent, isReadOnly) {
            var _a;
            parent.empty();
            const tabs = this.formElement.get("tab");
            for (let n in tabs) {
                if (!tabs.hasOwnProperty(n)) {
                    continue;
                }
                const tab = tabs[n];
                if (tab.metaClass.id == "DatenMeister.Models.Forms.DetailForm") {
                    var fields = tab.get("field");
                    var table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
                    var tableBody = $("<tbody><tr><th>Name</th><th>Value</th></tr>");
                    table.append(tableBody);
                    for (let m in fields) {
                        var tr = $("<tr><td class='key'></td><td class='value'></td></tr>");
                        if (!fields.hasOwnProperty(m)) {
                            continue;
                        }
                        var field = fields[m];
                        const name = (_a = field.get("title")) !== null && _a !== void 0 ? _a : field.get("name");
                        $(".key", tr).text(name);
                        var fieldMetaClassId = field.metaClass.id;
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
            return div;
        }
        evaluateDom(dmElement) {
        }
    }
    exports.MetaClassElementField = MetaClassElementField;
    class CheckboxField extends BaseField {
        createDom(dmElement) {
            var tthis = this;
            var result = $("<input type='checkbox'></input>");
            var fieldName = this.field.get('name').toString();
            if (dmElement.get(fieldName)) {
                result.prop('checked', true);
            }
            return result;
        }
        evaluateDom(dmElement) {
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
});
//# sourceMappingURL=Forms.js.map