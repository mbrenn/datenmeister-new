import * as Mof from 'datenmeister/../Mof'
import * as FormFactory from 'datenmeister/../forms/FormFactory'
import * as StundenPlanTypes from 'DatenMeister.StundenPlan'
import { FormType, IObjectFormElement } from 'datenmeister/../forms/Interfaces';
import { IFormConfiguration } from 'datenmeister/../forms/IFormConfiguration';

export function init() {
    FormFactory.registerObjectForm(
        StundenPlanTypes._Forms.__SchedulerForm_Uri,
        () => {
            return new StundenPlanForm();
        }
    );
}


class StundenPlanForm implements IObjectFormElement {

    element: Mof.DmObject;

    workspace: string;
    extentUri: string;
    itemUrl: string;
    formElement: Mof.DmObject;
    formType: FormType;

    async createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration): Promise<void> {
        parent.append($("<span>Scheduler</span>"));
    }

    refreshForm(): void {
        return ;
    }

    async storeFormValuesIntoDom?(reuseExistingElement?: boolean): Promise<Mof.DmObject> {
        return new Mof.DmObject();
    }


}