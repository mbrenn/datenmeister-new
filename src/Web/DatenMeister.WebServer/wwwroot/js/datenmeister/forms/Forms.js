define(["require", "exports", "../Mof", "../models/DatenMeister.class"], function (require, exports, Mof, _DatenMeister) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.FormMode = exports.FormModel = void 0;
    var FormModel;
    (function (FormModel) {
        function createEmptyFormObject() {
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
        FormModel.createEmptyFormObject = createEmptyFormObject;
    })(FormModel = exports.FormModel || (exports.FormModel = {}));
    // Defines the possible viewmode of a form
    var FormMode;
    (function (FormMode) {
        // The user can not edit the fields and just views the information
        FormMode[FormMode["ViewMode"] = 0] = "ViewMode";
        // The user can edit the fields and submit these changes
        FormMode[FormMode["EditMode"] = 1] = "EditMode";
    })(FormMode = exports.FormMode || (exports.FormMode = {}));
});
//# sourceMappingURL=Forms.js.map