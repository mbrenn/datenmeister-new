import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
export var FormModel;
(function (FormModel) {
    function createEmptyFormObject() {
        const form = new Mof.DmObject();
        form.metaClass =
            {
                id: _DatenMeister._Forms.__ObjectForm_Uri,
                uri: _DatenMeister._Forms.__ObjectForm_Uri
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
    FormModel.createEmptyFormObject = createEmptyFormObject;
})(FormModel || (FormModel = {}));
// Defines the possible viewmode of a form
export var FormMode;
(function (FormMode) {
    // The user can not edit the fields and just views the information
    FormMode[FormMode["ViewMode"] = 0] = "ViewMode";
    // The user can edit the fields and submit these changes
    FormMode[FormMode["EditMode"] = 1] = "EditMode";
})(FormMode || (FormMode = {}));
// Defines the possible submit methods, a user can chose to close the detail form
export var SubmitMethod;
(function (SubmitMethod) {
    // The user clicked on the save button
    SubmitMethod[SubmitMethod["Save"] = 0] = "Save";
    // The user clicked on the save and close button
    SubmitMethod[SubmitMethod["SaveAndClose"] = 1] = "SaveAndClose";
    // Some user defined actions are supported here
    SubmitMethod[SubmitMethod["UserDefined1"] = 2] = "UserDefined1";
    SubmitMethod[SubmitMethod["UserDefined2"] = 3] = "UserDefined2";
    SubmitMethod[SubmitMethod["UserDefined3"] = 4] = "UserDefined3";
})(SubmitMethod || (SubmitMethod = {}));
//# sourceMappingURL=Forms.js.map