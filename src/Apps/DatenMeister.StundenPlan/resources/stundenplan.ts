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

    createFormByObject(parent: JQuery<HTMLElement>, configuration: IFormConfiguration): Promise<void> {
        throw new Error('Method not implemented.');
    }

    refreshForm(): void {
        throw new Error('Method not implemented.');
    }

    storeFormValuesIntoDom?(reuseExistingElement?: boolean): Promise<Mof.DmObject> {
        throw new Error('Method not implemented.');
    }


}