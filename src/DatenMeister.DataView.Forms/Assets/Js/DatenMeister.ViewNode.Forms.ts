
import * as FormActions from '/js/datenmeister/FormActions.js';
import {IFormNavigation} from "/js/datenmeister/forms/Interfaces";
import {DmObject} from "/js/datenmeister/Mof.js";
import {SubmitMethod} from "/js/datenmeister/forms/Forms.js";
import * as DmModel from '/js/datenmeister/models/DatenMeister.class.js';

export function init()
{
    FormActions.addModule(new ViewNodeRenderAction());
}

class ViewNodeRenderAction
    extends FormActions.ItemFormActionModuleBase
    implements FormActions.IItemFormActionModule
{
    constructor() {
        super();
        this.actionName = "DatenMeister.ViewNodes.RenderItemsInPopup";
    }

    async execute(form?: IFormNavigation, element?: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<DmObject | void> {
        if(element === undefined) throw new Error("element is null");
        
        // We have to create the temporary action on the server to navigate to it. 
        
        // Now open the popup window
        const createWindow = new DmObject(DmModel._Actions._ClientActions.__NavigateOpenActionInWindow_Uri);
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenActionInWindow.context, "render");
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenActionInWindow.actionUrl, element.uri);
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenActionInWindow.workspaceId, element.workspace);

        await FormActions.executeClientAction(createWindow);
        return Promise.resolve(undefined);
    }
}