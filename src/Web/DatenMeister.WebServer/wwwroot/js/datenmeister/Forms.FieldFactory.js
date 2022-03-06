define(["require", "exports", "./fields/TextField", "./fields/CheckboxField", "./fields/DropDownField", "./fields/MetaClassElementField", "./fields/ActionField", "./fields/AnyDataField", "./fields/SubElementField", "./fields/ReferenceField", "./fields/UnknownField"], function (require, exports, TextField, CheckboxField, DropDownField, MetaClassElementField, ActionField, AnyDataField, SubElementField, ReferenceField, UnknownField) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {value: true});
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
            case "DatenMeister.Models.Forms.ReferenceFieldData":
                result = new ReferenceField.Field();
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