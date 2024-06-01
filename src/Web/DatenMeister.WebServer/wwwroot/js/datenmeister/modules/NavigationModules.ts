import * as FormActions from "../FormActions.js"
import { DmObject, ObjectType } from "../Mof.js";
import { IFormNavigation, IPageForm } from "../forms/Interfaces.js";
import { SubmitMethod } from "../forms/Forms.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";

export function loadModules() {

    FormActions.addModule(new ChangeForm());
}

class ChangeForm extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("DatenMeister.Navigation.ChangeForm");
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const formUrl = parameter.get("formUrl", ObjectType.String);

        var asFormPage = form as IPageForm;
        asFormPage.pageNavigation.switchFormUrl(formUrl);
    }
}