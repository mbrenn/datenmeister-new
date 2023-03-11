define(["require", "exports", "../models/DatenMeister.class", "../fields/TextField", "../fields/CheckboxField", "../fields/CheckboxListTaggingField", "../fields/DropDownField", "../fields/MetaClassElementField", "../fields/ActionField", "../fields/AnyDataField", "../fields/SubElementField", "../fields/SeparatorLineField", "../fields/ReferenceField", "../fields/ReferenceFieldFromCollection", "../fields/UnknownField"], function (require, exports, DatenMeister_class_1, TextField, CheckboxField, CheckboxListTaggingField, DropDownField, MetaClassElementField, ActionField, AnyDataField, SubElementField, SeparatorLineField, ReferenceField, ReferenceFieldFromCollection, UnknownField) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createField = void 0;
    function createField(fieldMetaClassUri, parameter) {
        let result;
        switch (fieldMetaClassUri) {
            case DatenMeister_class_1._DatenMeister._Forms.__TextFieldData_Uri:
                result = new TextField.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__MetaClassElementFieldData_Uri:
                result = new MetaClassElementField.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__ReferenceFieldData_Uri:
                result = new ReferenceField.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__ReferenceFieldFromCollectionData_Uri:
                result = new ReferenceFieldFromCollection.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__CheckboxFieldData_Uri:
                result = new CheckboxField.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__CheckboxListTaggingFieldData_Uri:
                result = new CheckboxListTaggingField.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__DropDownFieldData_Uri:
                result = new DropDownField.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__ActionFieldData_Uri:
                result = new ActionField.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__SubElementFieldData_Uri:
                result = new SubElementField.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__AnyDataFieldData_Uri:
                result = new AnyDataField.Field();
                break;
            case DatenMeister_class_1._DatenMeister._Forms.__SeparatorLineFieldData_Uri:
                result = new SeparatorLineField.Field();
                break;
            default:
                result = new UnknownField.Field(fieldMetaClassUri);
                break;
        }
        result.configuration = parameter.configuration;
        result.isReadOnly = parameter.isReadOnly;
        result.field = parameter.field;
        result.itemUrl = parameter.itemUrl;
        result.form = parameter.form;
        return result;
    }
    exports.createField = createField;
});
//# sourceMappingURL=FieldFactory.js.map