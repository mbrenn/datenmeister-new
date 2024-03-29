﻿import * as Mof from "./Mof.js"
import * as ClientItem from "./client/Items.js"
import * as ClientElements from "./client/Elements.js"


/**
 * Creates a temporary DmObjectWithSync which is mirrored on the server
 * @param metaClass Metaclass of the element to be created
 */
export async function createTemporaryDmObject (metaClass?: string) : Promise<Mof.DmObjectWithSync>
{
    const result = await ClientElements.createTemporaryElement(metaClass);
    return Mof.DmObjectWithSync.createFromReference(result.workspace, result.uri);
}

/**
 * Performs a sync of the element to the server. 
 * Only changes of the element are synced with the server
 * @param element Element ot be synced
 */
export async function sync(element : Mof.DmObjectWithSync) : Promise<void> {
    
    // Check first the set elements
    const paras = new Array<ClientItem.ISetPropertyParam>();
    for (const key in element.propertiesSet) {
        const value = element.get(key, Mof.ObjectType.Default);

        if (value === undefined || value === null) {
            // Element is not set, so unset it
            await ClientItem.unsetProperty(element.workspace, element.uri, key);            
            console.log('MofSync: Unsetting: ' + element.uri + " - " + key);
        } else if ((typeof value === "object" || value === "function") && (value !== null)) {
            // Element is a reference, so we need to set the reference directly
            const referenceValue = value as Mof.DmObject;
            await ClientItem.setPropertyReference(element.workspace, element.uri,
                {
                    property: key,
                    workspaceId: referenceValue.workspace,
                    referenceUri: referenceValue.uri
                });
            
            console.log('MofSync: Setting Reference for: ' + element.uri + " - " + key);
        } else {
            // Element is a pure property           
            paras.push(
                {
                    key: key,
                    value: value?.toString() ?? ""
                }
            );

            console.log('MofSync: Setting: ' + element.uri + " - " + key);
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
    
    element.clearSync();
}