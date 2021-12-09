define(["require", "exports", "./Fields/TextField", "./Fields/CheckboxField", "./Fields/DropDownField", "./Fields/MetaClassElementField", "./Fields/ActionField", "./Fields/AnyDataField", "./Fields/SubElementField", "./Fields/UnknownField"], function (require, exports, TextField, CheckboxField, DropDownField, MetaClassElementField, ActionField, AnyDataField, SubElementField, UnknownField) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createField = void 0;
    function createField(fieldMetaClassId, parameter) {
        let result;
        switch (fieldMetaClassId) {
            case "DatenMeister.Models.Forms.TextFieldData":
                result = new TextField.Field();
                break;
            case "DatenMeister.Models.Forms.MetaClassElementFieldData":
                result = new MetaClassElementField.Field();
                break;
            case "DatenMeister.Models.Forms.CheckboxFieldData":
                result = new CheckboxField.Field();
                break;
            case "DatenMeister.Models.Forms.DropDownFieldData":
                result = new DropDownField.Field();
                break;
            case "DatenMeister.Models.Forms.ActionFieldData":
                result = new ActionField.Field();
                break;
            case "DatenMeister.Models.Forms.SubElementFieldData":
                result = new SubElementField.Field();
                break;
            case "DatenMeister.Models.Forms.AnyDataFieldData":
                result = new AnyDataField.Field();
                break;
            default:
                result = new UnknownField.Field(fieldMetaClassId);
                break;
        }
        result.configuration = parameter.configuration;
        result.form = parameter.form;
        result.isReadOnly = parameter.isReadOnly;
        result.field = parameter.field;
        result.itemUrl = parameter.itemUrl;
        return result;
    }
    exports.createField = createField;
});
//# sourceMappingURL=Forms.FieldFactory.js.map