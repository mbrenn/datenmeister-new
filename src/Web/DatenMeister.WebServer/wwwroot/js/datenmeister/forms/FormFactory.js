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
    const indexUri = uri.indexOf('#');
    if (indexUri !== -1) {
        uri = uri.substring(indexUri + 1);
    }
    for (let n in registerDataCollectionForm) {
        const item = registerDataCollectionForm[n];
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
export function getObjectFormFactory(uri) {
    const indexUri = uri.indexOf('#');
    if (indexUri !== -1) {
        uri = uri.substring(indexUri + 1);
    }
    for (let n in registerDataObjectForm) {
        const item = registerDataObjectForm[n];
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
//# sourceMappingURL=FormFactory.js.map