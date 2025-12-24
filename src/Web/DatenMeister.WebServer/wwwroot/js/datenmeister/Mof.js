import { EntentType } from "./ApiModels.js";
export var ObjectType;
(function (ObjectType) {
    ObjectType[ObjectType["Default"] = 0] = "Default";
    ObjectType[ObjectType["Single"] = 1] = "Single";
    ObjectType[ObjectType["String"] = 2] = "String";
    ObjectType[ObjectType["Array"] = 3] = "Array";
    ObjectType[ObjectType["Boolean"] = 4] = "Boolean";
    ObjectType[ObjectType["Number"] = 5] = "Number";
    ObjectType[ObjectType["Object"] = 6] = "Object";
})(ObjectType || (ObjectType = {}));
// This id is a running id being used to define IDs of recently created objects
let runningId = 0;
export class ObjectValue {
}
export class DmObject {
    /**
     * Creates a new instance of the MofObject
      * @param metaClassUri A possible metaclass Uri
     * @param metaclassWorkspace the workspace in which the metaclass is residing
     */
    constructor(metaClassUri, metaclassWorkspace) {
        this.isReference = false;
        this.values = new Array();
        if (metaClassUri !== undefined) {
            this.setMetaClassByUri(metaClassUri, metaclassWorkspace);
        }
        runningId++;
        this.id = 'local_' + runningId.toString();
    }
    static createFromReference(workspaceId, itemUri) {
        const result = new DmObject();
        result.isReference = true;
        result.workspace = workspaceId;
        result.uri = itemUri;
        return result;
    }
    static createAsReferenceFromLocalId(id) {
        const result = new DmObject();
        result.isReference = true;
        if (id.id !== undefined) {
            id = id.id;
        }
        result.uri = '#' + id;
        result.id = id;
        return result;
    }
    /**
     * Modifies the key that it can be used for internal array storage.
     * Unfortunately, the array has some functions and these functions cannot be overwritten
     * @param key
     */
    static internalizeKey(key) {
        return "_" + key;
    }
    /**
     * Modifies the key that the internal key value can be used for external access.
     * This is the opposite of internalizeKey
     * @param key
     */
    static externalizeKey(key) {
        return key.substring(1);
    }
    /**
     * Sets a value and returns whether the value has been changed
     * @param key Key to be set
     * @param value Value to be set
     */
    set(key, value) {
        const internalizedKey = DmObject.internalizeKey(key);
        let objectValue = this.values[internalizedKey];
        if (objectValue === undefined) {
            objectValue = new ObjectValue();
            this.values[internalizedKey] = objectValue;
        }
        const oldValue = objectValue.value;
        const oldIsSet = objectValue.isSet;
        objectValue.value = value;
        objectValue.isSet = true;
        return !(oldValue === value && oldIsSet === true);
    }
    get(key, objectType) {
        const objectValue = this.values[DmObject.internalizeKey(key)];
        if (objectValue !== undefined && objectValue.isSet === false)
            return objectValue.defaultValue;
        let result = objectValue?.value;
        switch (objectType) {
            case ObjectType.Default:
                return result;
            case ObjectType.Object:
                return result;
            case ObjectType.Single:
                if (Array.isArray(result)) {
                    return result[0];
                }
                return result;
            case ObjectType.Array:
                if (result === undefined || result === null) {
                    return [];
                }
                if (Array.isArray(result)) {
                    return result;
                }
                return [result];
            case ObjectType.String:
                const resultString = this.get(key, ObjectType.Single);
                if (resultString === undefined) {
                    return undefined;
                }
                return (resultString?.toString() ?? "");
            case ObjectType.Boolean:
                if (Array.isArray(result)) {
                    result = result[0];
                }
                // Take the standard routine but also check that there is no '0' in the text
                return (Boolean(result) && result !== "0" && result !== "false");
            case ObjectType.Number:
                return result === undefined ? undefined : Number(result);
        }
        return result;
    }
    getAsArray(key) {
        const value = this.get(key);
        if (Array.isArray(value)) {
            return value;
        }
        if (value === undefined) {
            const newArray = [];
            this.set(key, newArray);
            return newArray;
        }
        else {
            const newArray = [value];
            this.set(key, newArray);
            return newArray;
        }
    }
    appendToArray(key, value) {
        const existingValue = this.get(key);
        if (Array.isArray(existingValue)) {
            existingValue.push(value);
            return;
        }
        if (existingValue === undefined || existingValue === null) {
            this.set(key, [value]);
            return;
        }
        else {
            this.set(key, [existingValue, value]);
        }
    }
    /**
     * Gets an enumeration of all property values.
     * This method is used to protect the internal transformation of key values according
     * internalize and externalize
     */
    getPropertyValues() {
        const result = new Array();
        for (let n in this.values) {
            if (!this.values.hasOwnProperty(n)) {
                continue;
            }
            const objectValue = this.values[n];
            result[DmObject.externalizeKey(n)] = objectValue.value;
        }
        return result;
    }
    isSet(key) {
        const objectValue = this.values[DmObject.internalizeKey(key)];
        return objectValue !== undefined && objectValue.isSet;
    }
    unset(key) {
        const objectValue = this.values[DmObject.internalizeKey(key)];
        if (objectValue !== undefined) {
            objectValue.isSet = false;
            objectValue.value = objectValue.defaultValue;
        }
    }
    toString() {
        let values = this.getPropertyValues();
        return DmObject.valueToString(values);
    }
    setMetaClassByUri(metaClassUri, workspace) {
        if (workspace === undefined) {
            workspace = "Types";
        }
        this.metaClass = { uri: metaClassUri, workspace: workspace };
    }
    static valueToString(item, indent = "") {
        let result = "";
        let komma = "";
        if (Array.isArray(item)) {
            result = `\r\n${indent}[`;
            for (let n in item) {
                if (Object.prototype.hasOwnProperty.call(item, n)) {
                    const value = item[n];
                    result += `${komma}${this.valueToString(value, indent + "  ")}`;
                    komma = ", ";
                }
            }
            result += "]";
        }
        else if ((typeof item === "object" || typeof item === "function") && (item !== null)) {
            for (let key in item) {
                if (Object.prototype.hasOwnProperty.call(item, key)) {
                    const value = item[key];
                    const externalKey = DmObject.externalizeKey(key);
                    result += `${komma}\r\n${indent}${externalKey}: ${DmObject.valueToString(value, indent + "  ")}`;
                    komma = ", ";
                }
            }
        }
        else if (typeof item === "string" || item instanceof String) {
            result = `"${item.toString()}"`;
        }
        else if (item === null) {
            return "null";
        }
        else if (item === undefined) {
            return "undefined";
        }
        else {
            result = item.toString();
        }
        return result;
    }
    /**
     * Creates a reference from item with name and id structure
     * @param item Item which contains the information about the reference
     */
    static createFromItemWithNameAndId(item) {
        return this.createFromReference(item.workspace, item.uri);
    }
}
export class DmObjectWithSync extends DmObject {
    constructor(metaClass) {
        super(metaClass);
        this.propertiesSet = new Array();
        this.isMetaClassSet = true;
    }
    clearSync() {
        this.propertiesSet = new Array();
    }
    unset(key) {
        super.unset(key);
        this.propertiesSet[key] = true;
    }
    set(key, value) {
        const result = super.set(key, value);
        if (result) {
            this.propertiesSet[key] = true;
        }
        return result;
    }
    setMetaClassByUri(metaClassUri, workspace) {
        super.setMetaClassByUri(metaClassUri, workspace);
        this.isMetaClassSet = true;
    }
    static createFromReference(workspaceId, itemUri) {
        const result = new DmObjectWithSync();
        result.isReference = true;
        result.workspace = workspaceId;
        result.uri = itemUri;
        return result;
    }
}
/**
 * Takes the given object and exports it as an ItemWithNameAndId
 * @param element
 */
export function convertToItemWithNameAndId(element) {
    return {
        id: element.id,
        uri: element.uri,
        extentUri: element.extentUri,
        workspace: element.workspace,
        ententType: EntentType.Item
    };
}
/*
    Converts the given element to a json element that it can be used to send to the webserver
    The receiving function is MofJsonDeconverter.Convert in which the retrieved
    value is returned to MofObject
 */
export function createJsonFromObject(element) {
    const result = {};
    const values = {};
    function convertValue(elementValue) {
        if (Array.isArray(elementValue)) {
            // Do not send out arrays or objects
            const value = {};
            for (let n in elementValue) {
                const childItem = elementValue[n];
                value[n] = convertValue(childItem);
            }
            return value;
        }
        else if (((typeof elementValue === "object" || typeof elementValue === "function") && (elementValue !== null))) {
            if (elementValue instanceof DmObject) {
                // This is an object, so perform the transformation
                return createJsonFromObject(elementValue);
            }
            else {
                return elementValue;
            }
        }
        else {
            return elementValue;
        }
    }
    if (!element.isReference) {
        const internalValues = element["values"];
        for (const internalizedKey in internalValues) {
            if (!internalValues.hasOwnProperty(internalizedKey))
                continue;
            const objectValue = internalValues[internalizedKey];
            const key = DmObject.externalizeKey(internalizedKey);
            values[key] = convertValue(objectValue.value);
        }
    }
    else {
        // Object is reference
        result.r = element.uri;
        result.w = element.workspace;
    }
    if (element.metaClass !== undefined && element.metaClass !== null) {
        result.m = element.metaClass;
    }
    result.id = element.id;
    if (Object.keys(values).length > 0) {
        result.v = values;
    }
    return result;
}
/**
 * Converts the Object as given by the server to the JS-World.
 * In case of native objects, the native object will be returned.
 * In case of arrays, the arrays.
 * In case of elements, the corresponding DmObjects
 */
export function convertJsonObjectToObjects(element) {
    if (Array.isArray(element)) {
        const arrayResult = [];
        for (let m in element) {
            const inner = element[m];
            arrayResult.push(convertJsonObjectToObjects(inner));
        }
        return arrayResult;
    }
    else if ((typeof element === "object" || typeof element === "function") && (element !== null)) {
        return convertJsonObjectToDmObject(element);
    }
    return element;
}
/*
// Creates the given object from the included json
// The corresponding C# class is DatenMeister.Modules.Json.MofJsonConverter.Convert
*/
export function convertJsonObjectToDmObject(element) {
    if (element === undefined || element === null || element === "") {
        return undefined;
    }
    if (typeof element === 'string' || element instanceof String) {
        element = JSON.parse(element);
    }
    const result = new DmObjectWithSync();
    const elementValues = element["v"];
    if (elementValues !== undefined && elementValues !== null) {
        for (let key in elementValues) {
            if (Object.prototype.hasOwnProperty.call(elementValues, key)) {
                let valueArray = elementValues[key];
                const isPropertySet = valueArray[0];
                let value = valueArray[1];
                if (Array.isArray(value)) {
                    // Converts array
                    const finalValue = [];
                    for (const m in value) {
                        if (!(value.hasOwnProperty(m))) {
                            continue;
                        }
                        let arrayValue = value[m];
                        if ((typeof arrayValue === "object" || typeof arrayValue === "function") && (arrayValue !== null)) {
                            arrayValue = convertJsonObjectToDmObject(arrayValue);
                        }
                        finalValue.push(arrayValue);
                    }
                    value = finalValue;
                }
                else if ((typeof value === "object" || typeof value === "function") && (value !== null)) {
                    // Converts to DmObject, if item is an object
                    value = convertJsonObjectToDmObject(value);
                }
                const internalizedKey = DmObject.internalizeKey(key);
                const objectValue = new ObjectValue();
                objectValue.isSet = isPropertySet;
                objectValue.value = value;
                result["values"][internalizedKey] = objectValue;
            }
        }
        // Removes the set properties since they are already synced with the server
        result.clearSync();
    }
    const elementMetaClass = element["m"];
    if (elementMetaClass !== undefined && elementMetaClass !== null) {
        result.metaClass = elementMetaClass;
    }
    const elementUri = element["u"];
    if (elementUri !== undefined && elementUri !== null) {
        result.uri = elementUri;
    }
    const elementId = element["id"];
    if (elementId !== undefined && elementId !== null) {
        result.id = elementId;
    }
    const elementReferenceUri = element["r"];
    if (elementReferenceUri !== undefined && elementReferenceUri !== null) {
        result.uri = elementReferenceUri;
        result.isReference = true;
    }
    const extentUri = element["e"];
    if (extentUri !== undefined && extentUri !== null) {
        result.extentUri = extentUri;
    }
    const workspace = element["w"];
    if (workspace !== undefined && workspace !== null) {
        result.workspace = workspace;
    }
    return result;
}
export function getName(value) {
    if (value === null || value === undefined) {
        return "Null";
    }
    if (Array.isArray(value)) {
        let spacer = "";
        let result = "";
        for (let n in value) {
            const inner = value[n];
            result += spacer + getName(inner);
            spacer = ", ";
        }
        return result;
    }
    if ((typeof value === "object" || typeof value === "function")) {
        return value.get('name');
    }
    if (value.toString !== undefined) {
        return value.toString();
    }
    return value;
}
//# sourceMappingURL=Mof.js.map