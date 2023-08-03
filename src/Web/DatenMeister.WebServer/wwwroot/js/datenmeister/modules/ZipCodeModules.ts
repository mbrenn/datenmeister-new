import * as FormActions from "../FormActions.js"
import {DmObject, DmObjectWithSync} from "../Mof.js";
import * as MofSync from "../MofSync.js";
import {SubmitMethod} from "../forms/RowForm.js";
import {IFormNavigation} from "../forms/Interfaces.js";
import * as ApiConnection from "../ApiConnection.js";
import * as Settings from "../Settings.js";
import * as Navigator from "../Navigator.js"

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
    
    async loadObject(): Promise<DmObjectWithSync> | undefined {
        const result = await MofSync.createTemporaryDmObject(
            "dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
        return Promise.resolve(result);
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        alert(element.get('zip')?.toString() ?? "No Zip Code given");        
    }
}

export interface ICreateZipExampleActionResult
{
    success: boolean;
    extentUri: string;
    workspaceId: string;
}

class CreateZipExampleAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ZipExample.CreateExample");
        this.actionVerb = "Create Example";
        this.skipSaving = true;
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const id = element.get('id');
        await ApiConnection.post<ICreateZipExampleActionResult>(
            Settings.baseUrl + "api/zip/create",
            {workspace: id})
            .then(
                data => {
                    Navigator.navigateToExtentItems(
                        data.workspaceId,
                        data.extentUri
                    );                    
                });
    }
}