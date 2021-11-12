import * as TextField from "./Fields/TextField";
import * as CheckboxField from "./Fields/CheckboxField";
import * as DropDownField from "./Fields/DropDownField";
import * as MetaClassElementField from "./Fields/MetaClassElementField";
import * as ActionField from "./Fields/ActionField";
import * as AnyDataField from "./Fields/AnyDataField";
import * as SubElementField from "./Fields/SubElementField";
import * as UnknownField from "./Fields/UnknownField";
import {IFormField} from "./Interfaces.Fields";
import {IForm} from "./Interfaces.Forms";
import {DmObject} from "./Mof";

interface ICreateFieldParameter
{
    form: IForm;
    isReadOnly: boolean;
    field: DmObject;
    itemUrl: string;
}

export function createField(fieldMetaClassId: string, parameter: ICreateFieldParameter): IFormField {

    let result: IFormField;
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

    result.form = parameter.form;
    result.isReadOnly = parameter.isReadOnly;
    result.field = parameter.field;
    result.itemUrl = parameter.itemUrl;

    return result;
}
