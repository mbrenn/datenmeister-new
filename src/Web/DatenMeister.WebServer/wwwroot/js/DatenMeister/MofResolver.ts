﻿import {DmObject} from "./Mof";
import * as ClientItem from "./Client.Items"

/// Resolves the given value
/// This means, that for primitive instances, the value will be directly returns via a defered instance
/// while for pure references, the server will be queries.
/// This is currently the most simple implementation
export function resolve(value: any): JQuery.Deferred<any> {
    const r = jQuery.Deferred<any, never, never>();

    if (Array.isArray(value)) {
        r.resolve(value);
    } else if ((typeof value === "object" || typeof value === "function") && (value !== null)) {
        const asDmObject = value as DmObject;
        if (asDmObject.isReference) {
            const workspace = asDmObject.workspace;
            if (workspace === undefined) {
                alert('Workspace is undefined');
                asDmObject.workspace = "_";
            }

            ClientItem.loadObjectByUri(
                asDmObject.workspace,
                asDmObject.uri).done(
                loadedValue => r.resolve(loadedValue));
        } else {
            r.resolve(value);
        }
    } else {
        r.resolve(value);
    }

    return r;
}