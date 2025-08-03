import * as FormActions from "../FormActions.js";
import * as Mof from "../Mof.js";
import * as FormClient from "../client/Forms.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as ActionClient from "../client/Actions.js";
import * as Navigation from "../Navigator.js";
import {IFormNavigation} from "../forms/Interfaces";
import {DmObject} from "../Mof.js";
import {SubmitMethod} from "../forms/Forms";

export function loadModules() {
    FormActions.addModule(new FormsCreateByMetaClassAction());
    FormActions.addModule(new NavigateToItemClientAction());
}

class FormsCreateByMetaClassAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Forms.Create.ByMetaClass");
        this.actionVerb = "Create by MetaClass";
        this.skipSaving = true;
        this.defaultMetaClassUri = _DatenMeister._Actions.__CreateFormByMetaClass_Uri;
    }
    async loadForm() {
        return await FormClient.getForm("dm:///_internal/forms/internal#Forms.Create.ByMetaClass");
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const result = await ActionClient.executeAction(element.workspace, element.uri);
        if (result.success !== true) {
            alert('Form was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
        }
        else {
            alert('Form was created successfully');
        }
    }
}

class NavigateToItemClientAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Forms.NavigateToItem", _DatenMeister._Actions._ClientActions.__NavigateToItemClientAction_Uri);
        this.actionVerb = "Navigate to Item";
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const workspaceId = element.get(_DatenMeister._Actions._ClientActions._NavigateToItemClientAction.workspaceId, Mof.ObjectType.String);
        const itemUri = element.get(_DatenMeister._Actions._ClientActions._NavigateToItemClientAction.itemUrl, Mof.ObjectType.String);
        const formUri = element.get(_DatenMeister._Actions._ClientActions._NavigateToItemClientAction.formUri, Mof.ObjectType.String);
        Navigation.navigateToItemByUrl(workspaceId, itemUri, {
            formUri: formUri
        });
    }
}
//# sourceMappingURL=FormModules.js.map