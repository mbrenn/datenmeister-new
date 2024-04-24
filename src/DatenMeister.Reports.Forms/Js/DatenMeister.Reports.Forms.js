import * as FormActions from '/js/datenmeister/FormActions.js';
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
//# sourceMappingURL=DatenMeister.Reports.Forms.js.map