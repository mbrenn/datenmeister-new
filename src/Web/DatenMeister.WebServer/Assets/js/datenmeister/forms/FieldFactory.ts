import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as TextField from "../fields/TextField.js";
import * as CheckboxField from "../fields/CheckboxField.js";
import * as CheckboxListTaggingField from "../fields/CheckboxListTaggingField.js";
import * as CompositeField from "../fields/CompositeField.js";
import * as DateTimeField from "../fields/DateTimeField.js";
import * as DropDownField from "../fields/DropDownField.js";
import * as MetaClassElementField from "../fields/MetaClassElementField.js";
import * as ActionField from "../fields/ActionField.js";
import * as MergedFieldsInCell from "../fields/MergedFieldsInCell.js";
import * as AnyDataField from "../fields/AnyDataField.js";
import * as SubElementField from "../fields/SubElementField.js";
import * as SeparatorLineField from "../fields/SeparatorLineField.js";
import * as ReferenceField from "../fields/ReferenceField.js";
import * as DropDownByCollection from "../fields/DropDownByCollection.js";
import * as DropDownByQuery from "../fields/DropDownByQuery.js";
import * as UriReferenceFieldData from "../fields/UriReferenceFieldData.js";
import * as UnknownField from "../fields/UnknownField.js";
import {IFieldRenderContext, IFormField} from "../fields/Interfaces.js";
import * as Mof from "../Mof.js";
import {IFormConfiguration} from "./IFormConfiguration.js";
import {IFormNavigation, FormType} from "./Interfaces.js";

export type FieldFactoryMethod = (context: IFieldRenderContext) => IFormField;

const registeredFieldFactories = new Map<string, FieldFactoryMethod>();

export function registerField(metaClassFieldData: string, factoryMethod: FieldFactoryMethod): void {
    registeredFieldFactories.set(metaClassFieldData, factoryMethod);
}

export function registerDefaultFields(): void {
    registerField(_DatenMeister._Forms._FieldTypes.__TextFieldData_Uri, ctx => new TextField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__MetaClassElementFieldData_Uri, ctx => new MetaClassElementField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__ReferenceFieldData_Uri, ctx => new ReferenceField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__DropDownByCollection_Uri, ctx => new DropDownByCollection.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__DropDownByQueryData_Uri, ctx => new DropDownByQuery.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__CheckboxFieldData_Uri, ctx => new CheckboxField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__CompositeFieldData_Uri, ctx => new CompositeField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__DateTimeFieldData_Uri, ctx => new DateTimeField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__CheckboxListTaggingFieldData_Uri, ctx => new CheckboxListTaggingField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__DropDownFieldData_Uri, ctx => new DropDownField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__ActionFieldData_Uri, ctx => new ActionField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__MergedFieldsInCellData_Uri, ctx => new MergedFieldsInCell.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__SubElementFieldData_Uri, ctx => new SubElementField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__AnyDataFieldData_Uri, ctx => new AnyDataField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__SeparatorLineFieldData_Uri, ctx => new SeparatorLineField.Field(ctx));
    registerField(_DatenMeister._Forms._FieldTypes.__UriReferenceFieldData_Uri, ctx => new UriReferenceFieldData.Field(ctx));
}

// Initialize default field registrations
registerDefaultFields();

export function canBeSorted(field: Mof.DmObject): boolean {
    const metaClassUri = field.metaClass?.uri;

    if (metaClassUri === _DatenMeister._Forms._FieldTypes.__TextFieldData_Uri ||
        metaClassUri === _DatenMeister._Forms._FieldTypes.__DateTimeFieldData_Uri) {
        return true;
    }

    return false;
}

export function canBeTextFiltered(field: Mof.DmObject): boolean {
    const metaClassUri = field.metaClass?.uri;

    if (metaClassUri === _DatenMeister._Forms._FieldTypes.__TextFieldData_Uri ||
        metaClassUri === _DatenMeister._Forms._FieldTypes.__DateTimeFieldData_Uri) {
        return true;
    }

    return false;
}

export function createField(
    fieldMetaClassUriOrContext: string | IFieldRenderContext,
    parameter?: IFieldRenderContext): IFormField {

    let context: IFieldRenderContext;
    let fieldMetaClassUri: string;

    if (typeof fieldMetaClassUriOrContext === "string") {
        fieldMetaClassUri = fieldMetaClassUriOrContext;
        context = parameter ?? {
            field: new Mof.DmObject(),
            isReadOnly: false
        };
    } else {
        context = fieldMetaClassUriOrContext;
        fieldMetaClassUri = context.field?.metaClass?.uri ?? "";
    }

    const factory = registeredFieldFactories.get(fieldMetaClassUri);
    let result: IFormField;

    if (factory !== undefined) {
        result = factory(context);
    } else {
        result = new UnknownField.Field(fieldMetaClassUri, context);
    }

    // Ensure context properties are properly attached
    if (context.configuration !== undefined) result.configuration = context.configuration;
    if (context.isReadOnly !== undefined) result.isReadOnly = context.isReadOnly;
    if (context.field !== undefined) result.field = context.field;
    if (context.itemUrl !== undefined) result.itemUrl = context.itemUrl;
    if (context.form !== undefined) result.form = context.form;

    return result;
}
