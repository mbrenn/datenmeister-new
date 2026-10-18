import * as IForm from "./Interfaces.js";
import {IFormConfiguration} from "./IFormConfiguration.js";

type FormFactoryFunction<T> = (configuration: IFormConfiguration) => T;

const registerDataDecoupledForm = new Map<string, () => IForm.IDecoupledForm>();
const registerDataCollectionForm = new Map<string, FormFactoryFunction<IForm.ICollectionForm>>();
const registerDataObjectForm = new Map<string, FormFactoryFunction<IForm.IObjectForm>>();

function normalizeUri(uri: string): string {
    const hashIndex = uri.indexOf('#');
    return hashIndex !== -1 ? uri.substring(hashIndex + 1) : uri;
}

export function registerDecoupledForm(uri: string, factoryFunction: () => IForm.IDecoupledForm): void {
    registerDataDecoupledForm.set(uri, factoryFunction);
    registerDataDecoupledForm.set(normalizeUri(uri), factoryFunction);
}

export function registerCollectionForm(
    uri: string,
    factoryFunction: (configuration: IFormConfiguration) => IForm.ICollectionForm): void {
    registerDataCollectionForm.set(uri, factoryFunction);
    registerDataCollectionForm.set(normalizeUri(uri), factoryFunction);
}

export function registerObjectForm(
    uri: string,
    factoryFunction: (configuration: IFormConfiguration) => IForm.IObjectForm): void {
    registerDataObjectForm.set(uri, factoryFunction);
    registerDataObjectForm.set(normalizeUri(uri), factoryFunction);
}

export function getDecoupledFormFactory(uri: string): ((configuration: IFormConfiguration) => IForm.IDecoupledForm) | undefined {
    const factory = registerDataDecoupledForm.get(uri) ?? registerDataDecoupledForm.get(normalizeUri(uri));
    if (factory !== undefined) {
        return () => factory();
    }
    return undefined;
}

export function getCollectionFormFactory(uri: string): ((configuration: IFormConfiguration) => IForm.ICollectionForm) | undefined {
    return registerDataCollectionForm.get(uri) ?? registerDataCollectionForm.get(normalizeUri(uri));
}

export function getObjectFormFactory(uri: string): ((configuration: IFormConfiguration) => IForm.IObjectForm) | undefined {
    return registerDataObjectForm.get(uri) ?? registerDataObjectForm.get(normalizeUri(uri));
}
