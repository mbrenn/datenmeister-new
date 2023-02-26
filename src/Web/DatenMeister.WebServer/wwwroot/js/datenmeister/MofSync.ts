﻿import * as Mof from "./Mof"
import * as ClientItem from "./client/Items"
import {ISetPropertyParam} from "./client/Items";

/**
 * Performs a sync of the element to the server. 
 * Only changes of the element are synced with the server
 * @param element Element ot be synced
 */
export async function sync(element : Mof.DmObjectWithSync) : Promise<void> {
    
    // Check first the set elements
    const paras = new Array<ISetPropertyParam>();
    for (const key in element.propertiesSet) {
        const value = element.get(key, Mof.ObjectType.Default);

        if (value === undefined || value === null) {
            // Element is not set, so unset it
            await ClientItem.unsetProperty(element.workspace, element.uri, key);
        } else if ((typeof value === "object" || value === "function") && (value !== null)) {
            // Element is a reference, so we need to set the reference directly
            const referenceValue = value as Mof.DmObject;
            await ClientItem.setPropertyReference(element.workspace, element.uri,
                {
                    property: key,
                    workspaceId: referenceValue.workspace,
                    referenceUri: referenceValue.uri
                });
        } else {
            // Element is  a pure property
            paras.push(
                {
                    key: key,
                    value: value?.toString() ?? ""
                }
            );
        }
    }
    
    // Checks, if there is any property to be set
    if(paras.length > 0)
    {
        // Ok, we need to set the properties
        await ClientItem.setPropertiesByStringValues(
            element.workspace,
            element.uri,
            {
                properties:paras 
            }
        );
    }
    
    element.propertiesSet.length = 0;
}