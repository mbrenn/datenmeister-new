import * as Mof from "../Mof";
import * as DataLoader from "../client/Items";
import * as ClientForms from "../client/Forms";
import * as DetailForm from "./RowForm";
import {SubmitMethod} from "./RowForm";
import * as IForm from "./Interfaces";
import {TableForm} from "./TableForm";
import {IFormConfiguration} from "./IFormConfiguration";
import {navigateToExtent, navigateToItemByUrl} from "../Navigator";
import DmObject = Mof.DmObject;
import {ViewModeSelectionForm} from "./ViewModeSelectionForm";
import * as VML from "./ViewModeLogic"
import * as _DatenMeister from "../models/DatenMeister.class"

export namespace FormModel {
    export function createEmptyFormObject() {
        const form = new Mof.DmObject();
        form.metaClass =
            {
                id: _DatenMeister._DatenMeister._Forms.__ObjectForm_Uri
            };
        
        const detailForm = new Mof.DmObject();
        detailForm.metaClass =
            {
                id: _DatenMeister._DatenMeister._Forms.__RowForm_Uri
            };

        form.set('tab', [detailForm]);

        return form;
    }
}



// Defines the possible viewmode of a form
export enum FormMode
{
    // The user can not edit the fields and just views the information
    ViewMode,
    // The user can edit the fields and submit these changes
    EditMode
}