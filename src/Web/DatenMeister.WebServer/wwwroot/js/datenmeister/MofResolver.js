import * as ClientItem from "./client/Items.js";
// Resolves the given value
// This means, that for primitive instances, the value will be directly returns via a defered instance
// while for pure references, the server will be queries.
// This is currently the most simple implementation
export function resolve(value) {
    return new Promise(resolve => {
        if (Array.isArray(value)) {
            resolve(value);
        }
        else if ((typeof value === "object" || typeof value === "function") && (value !== null)) {
            const asDmObject = value;
            if (asDmObject.isReference) {
                const workspace = asDmObject.workspace;
                if (workspace === undefined) {
                    alert('Workspace is undefined');
                    asDmObject.workspace = "_";
                }
                ClientItem.getObjectByUri(asDmObject.workspace, asDmObject.uri).then(loadedValue => resolve(loadedValue));
            }
            else {
                resolve(value);
            }
        }
        else {
            resolve(value);
        }
    });
}
//# sourceMappingURL=MofResolver.js.map