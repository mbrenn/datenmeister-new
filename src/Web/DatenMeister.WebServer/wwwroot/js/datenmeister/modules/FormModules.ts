import * as FormActions from "../FormActions"
import {DmObject} from "../Mof";
import {SubmitMethod} from "../forms/RowForm";
import {IFormNavigation} from "../forms/Interfaces";
import * as FormClient from "../client/Forms";
import * as DatenMeisterModel from "../models/DatenMeister.class";
import * as ActionClient from "../client/Actions";

export function loadModules() {
    FormActions.addModule(new FormsCreateByMetaClassAction());
}


class FormsCreateByMetaClassAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Forms.Create.ByMetaClass");
    }

    async loadForm(): Promise<DmObject> | undefined {
        return await FormClient.getForm("dm:///_internal/forms/internal#Forms.Create.ByMetaClass");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        const extentCreationParameter = new DmObject();
        extentCreationParameter.set('configuration', element);
        extentCreationParameter.setMetaClassByUri(
            DatenMeisterModel._DatenMeister._Actions.__CreateFormByMetaClass_Uri
        );

        const result = await ActionClient.executeActionDirectly(
            "Execute",
            {
                parameter: extentCreationParameter
            }
        );

        if (result.success !== true) {
            alert('Form was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
        } else {
            alert('Form was created successfully');
        }

    }
}