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
            DataLoader.loadObjectByUri(workspace, uri).done(element => this.createViewFormByObject(parent, element, null));
            parent.empty();
            parent.text("createViewForm");
        }
        createViewFormByObject(parent, element, form) {
            parent.text("createViewFormByObject");
        }
    }
    exports.DetailForm = DetailForm;
    function getDefaultFormForItem(workspace, item, viewMode) {
        var r = jQuery.Deferred();
        ApiConnection.get(Settings.baseUrl +
            "api/default_for_item/" +
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