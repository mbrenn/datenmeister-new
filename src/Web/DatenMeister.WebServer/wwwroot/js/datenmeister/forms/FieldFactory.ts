import {_DatenMeister} from "../models/DatenMeister.class"
import * as TextField from "../fields/TextField";
import * as CheckboxField from "../fields/CheckboxField";
import * as DropDownField from "../fields/DropDownField";
import * as MetaClassElementField from "../fields/MetaClassElementField";
import * as ActionField from "../fields/ActionField";
import * as AnyDataField from "../fields/AnyDataField";
import * as SubElementField from "../fields/SubElementField";
import * as SeparatorLineField from "../fields/SeparatorLineField";
import * as ReferenceField from "../fields/ReferenceField";
import * as UnknownField from "../fields/UnknownField";
import {IFormField} from "../fields/Interfaces";
import {DmObject} from "../Mof";
import {IFormConfiguration} from "./IFormConfiguration";
import {IFormNavigation} from "./Interfaces";

interface ICreateFieldParameter {
    configuration: IFormConfiguration,
    isReadOnly: boolean;
    field: DmObject;
    itemUrl: string;
    form: IFormNavigation;
}

export function createField(fieldMetaClassUri: string, parameter: ICreateFieldParameter): IFormField {

    let result: IFormField;
    switch (fieldMetaClassUri) {
        case _DatenMeister._Forms.__TextFieldData_Uri:
            result = new TextField.Field();
            break;
        case _DatenMeister._Forms.__MetaClassElementFieldData_Uri:
            result = new MetaClassElementField.Field();
            break;
        case _DatenMeister._Forms.__ReferenceFieldData_Uri:
            result = new ReferenceField.Field();
            break;
        case _DatenMeister._Forms.__CheckboxFieldData_Uri:
            result = new CheckboxField.Field();
            break;
        case _DatenMeister._Forms.__DropDownFieldData_Uri:
            result = new DropDownField.Field();
            break;
        case _DatenMeister._Forms.__ActionFieldData_Uri:
            result = new ActionField.Field();
            break;
        case _DatenMeister._Forms.__SubElementFieldData_Uri:
            result = new SubElementField.Field();
            break;
        case _DatenMeister._Forms.__AnyDataFieldData_Uri:
            result = new AnyDataField.Field();
            break;
        case _DatenMeister._Forms.__SeparatorLineFieldData_Uri:
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