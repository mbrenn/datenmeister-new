import * as Mof from '/js/datenmeister/Mof.js'
import * as FormActions from '/js/datenmeister/FormActions.js'
import * as IIForms from "/js/datenmeister/forms/Interfaces.js";
import { SubmitMethod } from "/js/datenmeister/forms/Forms.js";
import { IFormConfiguration } from '../../Web/DatenMeister.WebServer/wwwroot/js/datenmeister/forms/IFormConfiguration';
import * as FormFactory from "/js/datenmeister/forms/FormFactory.js"
import * as Model from "./DatenMeister.Reports.Types.js"
import * as ActionClient from '/js/datenmeister/client/Actions.js'

export function init() {
    FormActions.addModule(new SwitchToReport());

    FormFactory.registerObjectForm(
        Model._Root.__ReportForm_Uri,
        () => new ReportForm());

}

// Adds the action on which the user can switch to the report view
export class SwitchToReport extends FormActions.ItemFormActionModuleBase implements FormActions.IItemFormActionModule {
    constructor() {
        super();
        this.actionName = 'datenmeister.reports.forms.show';
        this.actionVerb = 'datenmeister.reports.forms.show';        
    }

    execute(form: IIForms.IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {

        var asFormPage = form as IIForms.IPageForm;

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
export class ReportForm implements IIForms.IObjectFormElement {

    pageNavigation: IIForms.IPageNavigation;
    workspace: string;
    extentUri: string;
    itemUrl: string;
    formElement: Mof.DmObject;

    element: Mof.DmObject;
    async createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration): Promise<void> {

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

    async refreshForm(): Promise<void> {
        
    }

    async storeFormValuesIntoDom?(reuseExistingElement?: boolean): Promise<Mof.DmObject> {
        return Promise.resolve(undefined);
    }
    
}

// The action request which allows to retrieve the report itself
export interface IRequestReportParams {
    workspaceId: string;
    itemUri: string;
}
export interface IRequestReportResult {
    reportHtml: string;
}

async function loadReport(workspace: string, itemUri: string) : Promise<string> {
    const action = new Mof.DmObject(Model._Root.__RequestReportAction_Uri);
    action.set(Model._Root._RequestReportAction.workspace, workspace);
    action.set(Model._Root._RequestReportAction.itemUri, itemUri);

    const parameter: ActionClient.ExecuteActionParams =
    {
        parameter: action
    };

    const result = await ActionClient.executeActionDirectly("Execute", parameter);
    return result.resultAsDmObject.get(Model._Root._RequestReportResult.report);
}