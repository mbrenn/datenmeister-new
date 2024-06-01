import * as FormActions from "../FormActions.js";
import { ObjectType } from "../Mof.js";
export function loadModules() {
    FormActions.addModule(new ChangeForm());
}
class ChangeForm extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("DatenMeister.Navigation.ChangeForm");
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        const formUrl = parameter.get("formUrl", ObjectType.String);
        var asFormPage = form;
        asFormPage.pageNavigation.switchFormUrl(formUrl);
    }
}
//# sourceMappingURL=NavigationModules.js.map