const registerDataDecoupledForm = new Map();
const registerDataCollectionForm = new Map();
const registerDataObjectForm = new Map();
function normalizeUri(uri) {
    const hashIndex = uri.indexOf('#');
    return hashIndex !== -1 ? uri.substring(hashIndex + 1) : uri;
}
export function registerDecoupledForm(uri, factoryFunction) {
    registerDataDecoupledForm.set(uri, factoryFunction);
    registerDataDecoupledForm.set(normalizeUri(uri), factoryFunction);
}
export function registerCollectionForm(uri, factoryFunction) {
    registerDataCollectionForm.set(uri, factoryFunction);
    registerDataCollectionForm.set(normalizeUri(uri), factoryFunction);
}
export function registerObjectForm(uri, factoryFunction) {
    registerDataObjectForm.set(uri, factoryFunction);
    registerDataObjectForm.set(normalizeUri(uri), factoryFunction);
}
export function getDecoupledFormFactory(uri) {
    const factory = registerDataDecoupledForm.get(uri) ?? registerDataDecoupledForm.get(normalizeUri(uri));
    if (factory !== undefined) {
        return () => factory();
    }
    return undefined;
}
export function getCollectionFormFactory(uri) {
    return registerDataCollectionForm.get(uri) ?? registerDataCollectionForm.get(normalizeUri(uri));
}
export function getObjectFormFactory(uri) {
    return registerDataObjectForm.get(uri) ?? registerDataObjectForm.get(normalizeUri(uri));
}
//# sourceMappingURL=FormFactory.js.map