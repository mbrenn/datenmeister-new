var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../FormActions", "../MofSync", "../ApiConnection", "../Settings"], function (require, exports, FormActions, MofSync, ApiConnection, Settings) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadModules = void 0;
    function loadModules() {
        FormActions.addModule(new ZipCodeTestAction());
        FormActions.addModule(new CreateZipExampleAction());
    }
    exports.loadModules = loadModules;
    class ZipCodeTestAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Zipcode.Test");
            this.actionVerb = "Test of Zipcode";
            this.skipSaving = true;
        }
        loadObject() {
            return __awaiter(this, void 0, void 0, function* () {
                const result = yield MofSync.createTemporaryDmObject("dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
                return Promise.resolve(result);
            });
        }
        execute(form, element, parameter, submitMethod) {
            var _a, _b;
            return __awaiter(this, void 0, void 0, function* () {
                alert((_b = (_a = element.get('zip')) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "No Zip Code given");
            });
        }
    }
    class CreateZipExampleAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("ZipExample.CreateExample");
            this.actionVerb = "Create Example";
            this.skipSaving = true;
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                const id = element.get('id');
                yield ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: id })
                    .then(data => {
                    document.location.reload();
                });
            });
        }
    }
});
//# sourceMappingURL=ZipCodeModules.js.map