const registerDataCollectionForm = new Array();
const registerDataObjectForm = new Array();
export function registerCollectionForm(uri, factoryFunction) {
    if (getCollectionFormFactory(uri) !== undefined)
        return;
    registerDataCollectionForm.push({
        uri: uri,
        factoryFunction: factoryFunction
    });
}
export function registerObjectForm(uri, factoryFunction) {
    if (getObjectFormFactory(uri) !== undefined)
        return;
    registerDataObjectForm.push({
        uri: uri,
        factoryFunction: factoryFunction
    });
}
export function getCollectionFormFactory(uri) {
    for (let n in registerDataCollectionForm) {
        const item = registerDataCollectionForm[n];
        if (item.uri === uri) {
            return item.factoryFunction;
        }
    }
    return undefined;
}
export function getObjectFormFactory(uri) {
    for (let n in registerDataObjectForm) {
        const item = registerDataObjectForm[n];
        if (item.uri === uri) {
            return item.factoryFunction;
        }
    }
    return undefined;
}
//# sourceMappingURL=FormFactory.js.map