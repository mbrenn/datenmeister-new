import * as IForm from "./Interfaces.js";

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
    if(getCollectionFormFactory(uri) !== undefined) return;

    registerDataCollectionForm.push(
        {
            uri: uri,
            factoryFunction: factoryFunction
        }
    );
}

export function registerObjectForm(uri: string, factoryFunction: () => IForm.IObjectFormElement) {
    if(getObjectFormFactory(uri) !== undefined) return;
    
    registerDataObjectForm.push(
        {
            uri: uri,
            factoryFunction: factoryFunction
        }
    );
}

export function getCollectionFormFactory(uri: string): () => IForm.ICollectionFormElement | undefined {

    var indexUri = uri.indexOf('#');
    if (indexUri !== -1) {
        uri = uri.substring(indexUri + 1);
    }

    for (let n in registerDataCollectionForm) {
        const item = registerDataCollectionForm[n];

        var indexItemUri = item.uri.indexOf('#');
        if (indexItemUri !== -1) {
            item.uri = item.uri.substring(indexItemUri + 1);
        }

        if (item.uri === uri) {
            return item.factoryFunction;
        }
    }

    return undefined;
}

export function getObjectFormFactory(uri: string): () => IForm.IObjectFormElement | undefined {
    var indexUri = uri.indexOf('#');
    if (indexUri !== -1) {
        uri = uri.substring(indexUri + 1);
    }

    for (let n in registerDataObjectForm) {
        
        const item = registerDataObjectForm[n];
        
        var indexItemUri = item.uri.indexOf('#');
        if (indexItemUri !== -1) {
            item.uri = item.uri.substring(indexItemUri + 1);
        }

        if (item.uri === uri) {
            return item.factoryFunction;
        }
    }

    return undefined;
}