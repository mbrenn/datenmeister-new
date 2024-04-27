import * as FormActions from '/js/datenmeister/FormActions.js';
import * as IIFields from "/js/datenmeister/fields/Interfaces.js";
export function init() {
    FormActions.addModule(new SwitchToReport());
}
export class SwitchToReport extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super();
        this.actionName = 'datenmeister.reports.forms.show';
        this.actionVerb = 'datenmeister.reports.forms.show';
    }
    execute(form, element, parameter, submitMethod) {
        alert('Click');
        return Promise.resolve(undefined);
    }
}
export class ReportField extends IIFields.BaseField {
    async createDom(dmElement) {
        return $("<div>We have a report</div>");
    }
    async evaluateDom(dmElement) {
    }
}
//# sourceMappingURL=DatenMeister.Reports.Forms.js.map