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
    exports.TextField = exports.getDefaultFormForItem = exports.DetailForm = exports.Form = void 0;
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
                tthis.createViewFormByObject(parent, element, form);
            });
            parent.empty();
            parent.text("createViewForm");
        }
        createViewFormByObject(parent, element, form) {
            var _a, _b;
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
                    $(".value", tr).text((_b = (_a = element.get(name)) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "unknown");
                    parent.append(tr);
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
    class TextField {
        createDom(parent, dmElement) {
            var fieldName = this.Field['name'];
            this._textBox = $("<input />");
            this._textBox.val(dmElement.get(fieldName).toString());
        }
        evaluateDom(dmElement) {
        }
    }
    exports.TextField = TextField;
});
//# sourceMappingURL=Forms.js.map