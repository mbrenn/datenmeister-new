define(["require", "exports", "datenmeister/../forms/FormFactory", "DatenMeister.StundenPlan"], function (require, exports, FormFactory, StundenPlanTypes) {
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
            throw new Error('Method not implemented.');
        }
        refreshForm() {
            throw new Error('Method not implemented.');
        }
        storeFormValuesIntoDom(reuseExistingElement) {
            throw new Error('Method not implemented.');
        }
    }
});
//# sourceMappingURL=stundenplan.js.map