import * as IForm from "./Interfaces";

interface EntryCollectionForm
{
    uri: string;
    factoryFunction: () => IForm.ICollectionFormElement;
}

interface EntryObjectForm
{
    uri: string;
    factoryFunction: () => IForm.IObjectFormElement;
}

const registerDataCollectionForm: Array<EntryCollectionForm> = 
    new Array<EntryCollectionForm>();

const registerDataObjectForm: Array<EntryObjectForm> 
    = new Array<EntryObjectForm>();

export function registerCollectionForm(uri: string, factoryFunction: () => IForm.ICollectionFormElement) {
    registerDataCollectionForm.push(
        {
            uri: uri,
            factoryFunction: factoryFunction
        }
    );
}

export function registerObjectForm(uri: string, factoryFunction: () => IForm.IObjectFormElement) {
    registerDataObjectForm.push(
        {
            uri: uri,
            factoryFunction: factoryFunction
        }
    );
}

export function getCollectionFormFactory(uri: string): () => IForm.ICollectionFormElement | undefined {
    for (let n in registerDataCollectionForm) {
        const item = registerDataCollectionForm[n];
        if (item.uri === uri) {
            return item.factoryFunction;
        }
    }

    return undefined;
}

export function getObjectFormFactory(uri: string): () => IForm.IObjectFormElement | undefined {
    for (let n in registerDataObjectForm) {
        const item = registerDataObjectForm[n];
        if (item.uri === uri) {
            return item.factoryFunction;
        }
    }

    return undefined;
}