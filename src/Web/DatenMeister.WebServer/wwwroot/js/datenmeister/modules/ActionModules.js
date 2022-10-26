var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../FormActions", "../Mof", "../client/Actions"], function (require, exports, FormActions, Mof_1, ActionClient) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadModules = void 0;
    function loadModules() {
        FormActions.addModule(new JsonItemAlertAction());
        FormActions.addModule(new ActionExecuteAction());
    }
    exports.loadModules = loadModules;
    class JsonItemAlertAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("JSON.Item.Alert");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                alert(JSON.stringify((0, Mof_1.createJsonFromObject)(element)));
            });
        }
    }
    class ActionExecuteAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Action.Execute");
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                // Executes the action directly
                const result = yield ActionClient.executeAction(element.workspace, element.uri);
                if (result.success) {
                    alert('Success');
                }
                else {
                    alert('Failure');
                }
            });
        }
    }
});
//# sourceMappingURL=ActionModules.js.map