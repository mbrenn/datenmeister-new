var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../FormActions", "../Mof", "../client/Forms", "../models/DatenMeister.class", "../client/Actions"], function (require, exports, FormActions, Mof_1, FormClient, DatenMeisterModel, ActionClient) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadModules = void 0;
    function loadModules() {
        FormActions.addModule(new FormsCreateByMetaClassAction());
    }
    exports.loadModules = loadModules;
    class FormsCreateByMetaClassAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Forms.Create.ByMetaClass");
        }
        loadForm() {
            return __awaiter(this, void 0, void 0, function* () {
                return yield FormClient.getForm("dm:///_internal/forms/internal#Forms.Create.ByMetaClass");
            });
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                const extentCreationParameter = new Mof_1.DmObject();
                extentCreationParameter.set('configuration', element);
                extentCreationParameter.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__CreateFormByMetaClass_Uri);
                const result = yield ActionClient.executeActionDirectly("Execute", {
                    parameter: extentCreationParameter
                });
                if (result.success !== true) {
                    alert('Form was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
                }
                else {
                    alert('Form was created successfully');
                }
            });
        }
    }
});
//# sourceMappingURL=FormModules.js.map