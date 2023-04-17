var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "datenmeister/../Mof", "datenmeister/../forms/FormFactory", "DatenMeister.StundenPlan"], function (require, exports, Mof, FormFactory, StundenPlanTypes) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.init = void 0;
    function init() {
        FormFactory.registerObjectForm(StundenPlanTypes._Forms.__SchedulerForm_Uri, () => {
            return new StundenPlanForm();
        });
    }
    exports.init = init;
    class StundenPlanForm {
        createFormByObject(parent, configuration) {
            return __awaiter(this, void 0, void 0, function* () {
                parent.append($("<span>Scheduler</span>"));
            });
        }
        refreshForm() {
            return;
        }
        storeFormValuesIntoDom(reuseExistingElement) {
            return __awaiter(this, void 0, void 0, function* () {
                return new Mof.DmObject();
            });
        }
    }
});
//# sourceMappingURL=stundenplan.js.map