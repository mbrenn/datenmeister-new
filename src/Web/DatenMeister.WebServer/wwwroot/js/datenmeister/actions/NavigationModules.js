import * as FormActions from "../FormActions.js";
import { ObjectType } from "../Mof.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import * as Navigation from "../Navigator.js";
export function loadModules() {
    FormActions.addModule(new ChangeForm());
    FormActions.addModule(new CreateNewItem());
    FormActions.addModule(new CreateAction());
    FormActions.addModule(new NavigateToExtent());
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
class NavigateToExtent extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("DatenMeister.Navigation.ToExtent", _DatenMeister._Actions._ClientActions.__NavigateToExtentClientAction_Uri);
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        Navigation.navigateToExtentProperties(element.get(_DatenMeister._Actions._ClientActions._NavigateToExtentClientAction.workspaceId), element.get(_DatenMeister._Actions._ClientActions._NavigateToExtentClientAction.extentUri));
    }
}
//# sourceMappingURL=NavigationModules.js.map