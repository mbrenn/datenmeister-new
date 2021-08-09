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
define(["require", "exports", "./Mof", "./DataLoader", "./ApiConnection", "./Settings"], function (require, exports, Mof, DataLoader, ApiConnection, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.MetaClassElementFieldData = exports.TextField = exports.BaseField = exports.getDefaultFormForItem = exports.DetailForm = exports.Form = void 0;
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
                tthis.createFormByObject(parent, element, form, true);
            });
            parent.empty();
            parent.text("createViewForm");
        }
        createFormByObject(parent, element, form, isReadOnly) {
            parent.text("createViewFormByObject");
            var table = $("<table class='table table-striped table-bordered dm-table-nofullwidth align-top'></table>");
            const tabs = form.get("tab");
            for (let n in tabs) {
                if (!tabs.hasOwnProperty(n)) {
                    continue;
                }
                const tab = tabs[n];
                var fields = tab.get("field");
                for (let m in fields) {
                    var tr = $("<tr><td class='key'></td><td class='value'></td></tr>");
                    if (!fields.hasOwnProperty(m)) {
                        continue;
                    }
                    var field = fields[m];
                    const name = field.get("name");
                    $(".key", tr).text(name);
                    var fieldMetaClassId = field.metaClass.id;
                    let fieldElement;
                    if (fieldMetaClassId === "DatenMeister.Models.Forms.TextFieldData") {
                        fieldElement = new TextField();
                    }
                    else if (fieldMetaClassId === "DatenMeister.Models.Forms.MetaClassElementFieldData") {
                        fieldElement = new MetaClassElementFieldData();
                    }
                    else {
                        fieldElement = $("<em></em>");
                        fieldElement.text(fieldMetaClassId !== null && fieldMetaClassId !== void 0 ? fieldMetaClassId : "unknown");
                        $(".value", tr).append(fieldElement);
                        table.append(tr);
                        continue;
                    }
                    fieldElement.field = field;
                    fieldElement.isReadOnly = isReadOnly;
                    let htmlElement = fieldElement.createDom(element);
                    $(".value", tr).append(htmlElement);
                    table.append(tr);
                }
            }
            parent.append(table);
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
        }
    }
    exports.TextField = TextField;
    class MetaClassElementFieldData extends BaseField {
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
    exports.MetaClassElementFieldData = MetaClassElementFieldData;
});
//# sourceMappingURL=Forms.js.map