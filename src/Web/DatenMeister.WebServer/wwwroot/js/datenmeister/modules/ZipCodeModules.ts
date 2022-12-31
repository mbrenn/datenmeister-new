import * as FormActions from "../FormActions"
import {DmObject} from "../Mof";
import {SubmitMethod} from "../forms/RowForm";
import {IFormNavigation} from "../forms/Interfaces";
import * as ApiConnection from "../ApiConnection";
import * as Settings from "../Settings";

export function loadModules() {
    FormActions.addModule(new ZipCodeTestAction());
    FormActions.addModule(new CreateZipExampleAction());
}


class ZipCodeTestAction extends FormActions.ItemFormActionModuleBase{
    constructor() {
        super("Zipcode.Test");
        this.actionVerb = "Test of Zipcode";
        this.skipSaving = true;
    }
    
    async loadObject(): Promise<DmObject> | undefined {
        const result = new DmObject();
        result.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
        return Promise.resolve(result);
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        alert(element.get('zip')?.toString() ?? "No Zip Code given");        
    }
}

class CreateZipExampleAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ZipExample.CreateExample");
        this.actionVerb = "Create Example";
        this.skipSaving = true;
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const id = element.get('id');
        await ApiConnection.post(
            Settings.baseUrl + "api/zip/create",
            {workspace: id})
            .then(
                data => {
                    document.location.reload();
                });
    }
}