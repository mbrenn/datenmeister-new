import * as FormActions from "../FormActions.js"
import {DmObject} from "../Mof.js";
import { SubmitMethod } from "../forms/Forms.js";
import {IFormNavigation} from "../forms/Interfaces.js";
import * as FormClient from "../client/Forms.js";
import * as DatenMeisterModel from "../models/DatenMeister.class.js";
import * as ActionClient from "../client/Actions.js";

export function loadModules() {
    FormActions.addModule(new FormsCreateByMetaClassAction());
}


class FormsCreateByMetaClassAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Forms.Create.ByMetaClass");
        this.actionVerb = "Create by MetaClass";
        this.skipSaving = true;
        this.defaultMetaClassUri = DatenMeisterModel._DatenMeister._Actions.__CreateFormByMetaClass_Uri;
    }

    async loadForm(): Promise<DmObject> | undefined {
        return await FormClient.getForm("dm:///_internal/forms/internal#Forms.Create.ByMetaClass");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        const result = await ActionClient.executeAction(
            element.workspace,
            element.uri
        );

        if (result.success !== true) {
            alert('Form was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
        } else {
            alert('Form was created successfully');
        }
    }
}