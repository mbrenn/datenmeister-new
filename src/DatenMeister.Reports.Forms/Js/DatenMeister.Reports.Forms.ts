import * as Mof from '/js/datenmeister/Mof.js'
import * as FormActions from '/js/datenmeister/FormActions.js'
import * as IIForms from "/js/datenmeister/forms/Interfaces.js";
import * as IIFields from "/js/datenmeister/fields/Interfaces.js";
import { SubmitMethod } from "/js/datenmeister/forms/Forms.js";

export function init() {
    FormActions.addModule(new SwitchToReport());

}

export class SwitchToReport extends FormActions.ItemFormActionModuleBase
{
    constructor() {
        super();
        this.actionName = 'datenmeister.reports.forms.show';
        this.actionVerb = 'datenmeister.reports.forms.show';        
    }

    execute(form: IIForms.IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        alert('Click');
        return Promise.resolve(undefined);
    }
}

export class ReportField extends IIFields.BaseField implements IIFields.IFormField {
    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {
        return $("<div>We have a report</div>");
    }

    async evaluateDom(dmElement: Mof.DmObject): Promise<void> {
    }
}