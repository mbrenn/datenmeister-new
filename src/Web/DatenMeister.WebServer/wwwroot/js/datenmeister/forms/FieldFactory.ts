import {_DatenMeister} from "../models/DatenMeister.class.js"
import * as TextField from "../fields/TextField.js";
import * as CheckboxField from "../fields/CheckboxField.js";
import * as CheckboxListTaggingField from "../fields/CheckboxListTaggingField.js";
import * as DropDownField from "../fields/DropDownField.js";
import * as MetaClassElementField from "../fields/MetaClassElementField.js";
import * as ActionField from "../fields/ActionField.js";
import * as AnyDataField from "../fields/AnyDataField.js";
import * as SubElementField from "../fields/SubElementField.js";
import * as SeparatorLineField from "../fields/SeparatorLineField.js";
import * as ReferenceField from "../fields/ReferenceField.js";
import * as ReferenceFieldFromCollection from "../fields/ReferenceFieldFromCollection.js";
import * as UriReferenceFieldData from "../fields/UriReferenceFieldData.js";
import * as UnknownField from "../fields/UnknownField.js";
import {IFormField} from "../fields/Interfaces.js";
import {DmObject} from "../Mof.js";
import {IFormConfiguration} from "./IFormConfiguration.js";
import {IFormNavigation} from "./Interfaces.js";

interface ICreateFieldParameter {
    configuration: IFormConfiguration,
    isReadOnly: boolean;
    field: DmObject;
    itemUrl: string;
    form: IFormNavigation;
}

interface IFieldContainer {
    metaClassFieldData: string;
    factoryMethod: () => IFormField;
}

var registeredFieldContainers: IFieldContainer[] = new Array<IFieldContainer>();

export function registerField(metaClassFieldData: string, factoryMethod: () => IFormField): void {
    registeredFieldContainers.push(
        {
            metaClassFieldData: metaClassFieldData,
            factoryMethod: factoryMethod
        }
    );
}

export function canBeSorted(field: DmObject): boolean {
    const metaClassUri = field.metaClass.uri;

    if (metaClassUri === _DatenMeister._Forms.__TextFieldData_Uri) {
        return true;
    }

    return false;
}

export function canBeTextFiltered(field: DmObject): boolean {
    const metaClassUri = field.metaClass.uri;

    if (metaClassUri === _DatenMeister._Forms.__TextFieldData_Uri) {
        return true;
    }

    return false;
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
        case _DatenMeister._Forms.__ReferenceFieldFromCollectionData_Uri:
            result = new ReferenceFieldFromCollection.Field();
            break;
        case _DatenMeister._Forms.__CheckboxFieldData_Uri:
            result = new CheckboxField.Field();
            break;
        case _DatenMeister._Forms.__CheckboxListTaggingFieldData_Uri:
            result = new CheckboxListTaggingField.Field();
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
        case _DatenMeister._Forms.__UriReferenceFieldData_Uri:
            result = new UriReferenceFieldData.Field();
            break;
        default:
            for (var n in registeredFieldContainers) {
                var registeredField = registeredFieldContainers[n];
                if (registeredField.metaClassFieldData === fieldMetaClassUri) {
                    result = registeredField.factoryMethod();
                }
            }
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