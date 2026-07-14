
import * as FormActions from '/js/datenmeister/FormActions.js';
import {IFormNavigation} from "/js/datenmeister/forms/Interfaces";
import {DmObject} from "/js/datenmeister/Mof";
import {SubmitMethod} from "/js/datenmeister/forms/Forms";

export function init()
{
    FormActions.addModule(new SwimlaneShowReportAction());    
}

class SwimlaneShowReportAction
    extends FormActions.ItemFormActionModuleBase
    implements FormActions.IItemFormActionModule 
{
    constructor() {
        super();
        this.actionName = "Reports.Swimlane.Show";
    }
    
    async execute(form?: IFormNavigation, element?: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<DmObject | void> {
        alert('Execute Form'); 
        return Promise.resolve(undefined);
    }
}

