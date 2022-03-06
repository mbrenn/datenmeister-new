import * as TextField from "./fields/TextField";
import * as CheckboxField from "./fields/CheckboxField";
import * as DropDownField from "./fields/DropDownField";
import * as MetaClassElementField from "./fields/MetaClassElementField";
import * as ActionField from "./fields/ActionField";
import * as AnyDataField from "./fields/AnyDataField";
import * as SubElementField from "./fields/SubElementField";
import * as ReferenceField from "./fields/ReferenceField";
import * as UnknownField from "./fields/UnknownField";
import {IFormField} from "./Interfaces.Fields";
import {IForm} from "./Forms.Interfaces";
import {DmObject} from "./Mof";
import {IFormConfiguration} from "./IFormConfiguration";

interface ICreateFieldParameter {
    configuration: IFormConfiguration,
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
