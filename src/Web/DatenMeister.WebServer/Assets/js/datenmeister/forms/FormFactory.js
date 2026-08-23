const registerDataDecoupledForm = new Array();
const registerDataCollectionForm = new Array();
const registerDataObjectForm = new Array();
export function registerDecoupledForm(uri, factoryFunction) {
    if (getDecoupledFormFactory(uri) !== undefined)
        return;
    registerDataDecoupledForm.push({
        uri: uri,
        factoryFunction: factoryFunction
    });
}
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
function getFormByUri(uri, register) {
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
export function getDecoupledFormFactory(uri) {
    return getFormByUri(uri, registerDataDecoupledForm);
}
export function getCollectionFormFactory(uri) {
    return getFormByUri(uri, registerDataCollectionForm);
}
export function getObjectFormFactory(uri) {
    return getFormByUri(uri, registerDataObjectForm);
}
//# sourceMappingURL=FormFactory.js.map