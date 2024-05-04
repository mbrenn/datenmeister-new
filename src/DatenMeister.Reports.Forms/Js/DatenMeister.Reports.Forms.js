import * as FormActions from '/js/datenmeister/FormActions.js';
import * as FormFactory from "/js/datenmeister/forms/FormFactory.js";
import * as Model from "./DatenMeister.Reports.Types.js";
export function init() {
    FormActions.addModule(new SwitchToReport());
    FormFactory.registerObjectForm(Model._Root.__ReportForm_Uri, () => new ReportForm());
}
// Adds the action on which the user can switch to the report view
export class SwitchToReport extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super();
        this.actionName = 'datenmeister.reports.forms.show';
        this.actionVerb = 'datenmeister.reports.forms.show';
    }
    execute(form, element, parameter, submitMethod) {
        var asFormPage = form;
        if (asFormPage?.pageNavigation?.switchFormUrl === undefined) {
            alert('switchFormUrl is not implemented');
        }
        else {
            // switches to the formurl
            asFormPage.pageNavigation.switchFormUrl("dm:///_internal/forms/internal#12541f2c-4e9a-4860-8bf3-e1b52330ec1f");
        }
        return Promise.resolve(undefined);
    }
}
// Shows the report within the form
export class ReportForm {
    async createFormByObject(parent, configuration) {
        parent.append($("<div>We are having a report... At least, I hope so</div>"));
    }
    async refreshForm() {
    }
    async storeFormValuesIntoDom(reuseExistingElement) {
        return Promise.resolve(undefined);
    }
}
//# sourceMappingURL=DatenMeister.Reports.Forms.js.map