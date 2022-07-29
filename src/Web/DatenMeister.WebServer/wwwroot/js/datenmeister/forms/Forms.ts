import * as Mof from "../Mof";
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