import * as IForm from "./Interfaces.js";
import {IFormConfiguration} from "./IFormConfiguration.js";

interface IHasUri<T> {
    uri: string;
    factoryFunction: (configuration: IFormConfiguration) => T;
}

interface EntryCollectionForm extends IHasUri<IForm.ICollectionForm>
{
    uri: string;
    factoryFunction: (configuration: IFormConfiguration) => IForm.ICollectionForm;
}

interface EntryObjectForm extends IHasUri<IForm.IObjectForm> {
    uri: string;
    factoryFunction: (configuration: IFormConfiguration) => IForm.IObjectForm;
}

interface EntryDecoupledForm extends IHasUri<IForm.IDecoupledForm> {
    uri: string;
    factoryFunction: () => IForm.IDecoupledForm;
}

const registerDataDecoupledForm: Array<EntryDecoupledForm> = new Array<EntryDecoupledForm>();

const registerDataCollectionForm: Array<EntryCollectionForm> = new Array<EntryCollectionForm>();

const registerDataObjectForm: Array<EntryObjectForm> = new Array<EntryObjectForm>();

export function registerDecoupledForm(uri: string, factoryFunction: () => IForm.IDecoupledForm) {
    if(getDecoupledFormFactory(uri) !== undefined) return;

    registerDataDecoupledForm.push(
        {
            uri: uri,
            factoryFunction: factoryFunction
        }
    );
}

export function registerCollectionForm(
    uri: string,
    factoryFunction: (configuration: IFormConfiguration) => IForm.ICollectionForm) {
    if(getCollectionFormFactory(uri) !== undefined) return;

    registerDataCollectionForm.push(
        {
            uri: uri,
            factoryFunction: factoryFunction
        }
    );
}

export function registerObjectForm(uri: string, factoryFunction: (configuration: IFormConfiguration) => IForm.IObjectForm) {
    if(getObjectFormFactory(uri) !== undefined) return;
    
    registerDataObjectForm.push(
        {
            uri: uri,
            factoryFunction: factoryFunction
        }
    );
}

function getFormByUri<T>(uri: string, register: Array<IHasUri<T>>): ((configuration: IFormConfiguration) => T) | undefined {
    const indexUri = uri.indexOf('#');
    if (indexUri !== -1) {
        uri = uri.substring(indexUri + 1);
    }

    for (let n in register) {
        const item = register[n];

        const indexItemUri = item.uri.indexOf('#');
        if (indexItemUri !== -1) {
            item.uri = item.uri.substring(indexItemUri + 1);
        }

        if (item.uri === uri) {
            return item.factoryFunction;
        }
    }

    return undefined;
}

export function getDecoupledFormFactory(uri: string): ((configuration: IFormConfiguration) => IForm.IDecoupledForm) | undefined {
    return getFormByUri<IForm.IDecoupledForm>(uri, registerDataDecoupledForm);
}

export function getCollectionFormFactory(uri: string): ((configuration: IFormConfiguration) => IForm.ICollectionForm) | undefined {
    return getFormByUri<IForm.ICollectionForm>(uri, registerDataCollectionForm);
}

export function getObjectFormFactory(uri: string): ((configuration: IFormConfiguration) => IForm.IObjectForm) | undefined {
    return getFormByUri<IForm.IObjectForm>(uri, registerDataObjectForm);
}