
import * as FormActions from '/js/datenmeister/FormActions.js';
import {IFormNavigation} from "/js/datenmeister/forms/Interfaces";
import {DmObject} from "/js/datenmeister/Mof.js";
import {SubmitMethod} from "/js/datenmeister/forms/Forms.js";
import * as DmModel from '/js/datenmeister/models/DatenMeister.class.js';

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
        const createWindow = new DmObject(DmModel._Actions._ClientActions.__NavigateOpenWindow_Uri);
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenWindow.url, "http://www.spiegel.de");
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenWindow.isAbsoluteUrl, true);

        await FormActions.executeClientAction(createWindow);
        return Promise.resolve(undefined);
    }
}

