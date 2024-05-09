import * as Mof from '/js/datenmeister/Mof.js';
import * as FormActions from '/js/datenmeister/FormActions.js';
import * as FormFactory from "/js/datenmeister/forms/FormFactory.js";
import * as Model from "./DatenMeister.Reports.Types.js";
import * as ActionClient from '/js/datenmeister/client/Actions.js';
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
        // Add the loading information
        const loadingDiv = $("<div class='loading'>Loading...</div>");
        parent.append(loadingDiv);
        // Loading the report
        const htmlReult = await loadReport(this.workspace, this.itemUrl);
        const container = $("<div class='dm-report'></div>");
        container.html(htmlReult);
        // Remove loading...
        parent.append(container);
        loadingDiv.remove();
    }
    async refreshForm() {
    }
    async storeFormValuesIntoDom(reuseExistingElement) {
        return Promise.resolve(undefined);
    }
}
async function loadReport(workspace, itemUri) {
    const action = new Mof.DmObject(Model._Root.__RequestReportAction_Uri);
    action.set(Model._Root._RequestReportAction.workspace, workspace);
    action.set(Model._Root._RequestReportAction.itemUri, itemUri);
    const parameter = {
        parameter: action
    };
    const result = await ActionClient.executeActionDirectly("Execute", parameter);
    return result.resultAsDmObject.get(Model._Root._RequestReportResult.report);
}
//# sourceMappingURL=DatenMeister.Reports.Forms.js.map