import * as FormActions from "../FormActions.js";
import * as MofSync from "../MofSync.js";
import * as ApiConnection from "../ApiConnection.js";
import * as Settings from "../Settings.js";
export function loadModules() {
    FormActions.addModule(new ZipCodeTestAction());
    FormActions.addModule(new CreateZipExampleAction());
}
class ZipCodeTestAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Zipcode.Test");
        this.actionVerb = "Test of Zipcode";
        this.skipSaving = true;
    }
    async loadObject() {
        const result = await MofSync.createTemporaryDmObject("dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
        return Promise.resolve(result);
    }
    async execute(form, element, parameter, submitMethod) {
        alert(element.get('zip')?.toString() ?? "No Zip Code given");
    }
}
class CreateZipExampleAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ZipExample.CreateExample");
        this.actionVerb = "Create Example";
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        const id = element.get('id');
        await ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: id })
            .then(data => {
            document.location.reload();
        });
    }
}
//# sourceMappingURL=ZipCodeModules.js.map