import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js"

export namespace FormModel {
    export function createEmptyFormObject() {
        const form = new Mof.DmObject();
        form.metaClass =
            {
                id: _DatenMeister._Forms.__ObjectForm_Uri,
                uri:  _DatenMeister._Forms.__ObjectForm_Uri
            };
        
        const detailForm = new Mof.DmObject();
        detailForm.metaClass =
            {
                id: _DatenMeister._Forms.__RowForm_Uri,
                uri: _DatenMeister._Forms.__RowForm_Uri
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

// Defines the possible submit methods, a user can chose to close the detail form
export enum SubmitMethod {
    // The user clicked on the save button
    Save,
    // The user clicked on the save and close button
    SaveAndClose,

    // Some user defined actions are supported here
    UserDefined1,
    UserDefined2,
    UserDefined3
}