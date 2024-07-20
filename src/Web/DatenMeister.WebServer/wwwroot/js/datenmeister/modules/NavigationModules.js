import * as FormActions from "../FormActions.js";
import { ObjectType } from "../Mof.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import * as Navigation from "../Navigator.js";
export function loadModules() {
    FormActions.addModule(new ChangeForm());
    FormActions.addModule(new CreateNewItem());
    FormActions.addModule(new CreateAction());
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
class CreateNewItem extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("DatenMeister.Navigation.CreateNewItem");
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
    }
}
class CreateAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("DatenMeister.Navigation.CreateAction");
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        const actionType = parameter.get(_DatenMeister._Actions._ParameterTypes._NavigationDefineActionParameter.actionName, ObjectType.String);
        const formUrl = parameter.get(_DatenMeister._Actions._ParameterTypes._NavigationDefineActionParameter.formUrl, ObjectType.String);
        const metaClassUrl = parameter.get(_DatenMeister._Actions._ParameterTypes._NavigationDefineActionParameter.metaClassUrl, ObjectType.String);
        Navigation.navigateToAction(actionType, formUrl, { workspace: element.workspace, itemUri: element.uri, metaClass: metaClassUrl });
    }
}
//# sourceMappingURL=NavigationModules.js.map