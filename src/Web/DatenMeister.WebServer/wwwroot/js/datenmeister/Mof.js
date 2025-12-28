/**
 * Mof.ts - Meta Object Facility TypeScript Implementation
 *
 * This module provides the core infrastructure for working with MOF (Meta Object Facility) objects
 * in TypeScript. It includes:
 * - DmObject: The main class representing MOF objects with properties and metaclasses
 * - Type-safe property access with ObjectType enum
 * - JSON serialization/deserialization for server communication
 * - Reference handling for object relationships
 * - Synchronization tracking for change detection
 */
import { EntentType } from "./ApiModels.js";
/**
 * Defines the type of value expected when retrieving a property from a DmObject.
 * Used to ensure type-safe access and automatic conversion of property values.
 */
export var ObjectType;
(function (ObjectType) {
    /** Returns the raw value without any conversion */
    ObjectType[ObjectType["Default"] = 0] = "Default";
    /** Returns the first element if the value is an array, otherwise returns the value itself */
    ObjectType[ObjectType["Single"] = 1] = "Single";
    /** Converts the value to a string */
    ObjectType[ObjectType["String"] = 2] = "String";
    /** Ensures the value is returned as an array (wraps single values, returns empty array for null/undefined) */
    ObjectType[ObjectType["Array"] = 3] = "Array";
    /** Converts the value to a boolean (handles strings like "0" and "false") */
    ObjectType[ObjectType["Boolean"] = 4] = "Boolean";
    /** Converts the value to a number */
    ObjectType[ObjectType["Number"] = 5] = "Number";
    /** Returns the value as a DmObject */
    ObjectType[ObjectType["Object"] = 6] = "Object";
})(ObjectType || (ObjectType = {}));
/**
 * Global counter for generating unique IDs for newly created DmObjects.
 * Each new object gets a unique ID in the format 'local_N'.
 */
let runningId = 0;
/**
 * Represents a property value in a DmObject, tracking whether it has been explicitly set
 * or contains a default value. This distinction is important for change tracking and synchronization.
 */
export class ObjectValue {
}
/**
 * DmObject - Core class representing a MOF (Meta Object Facility) object.
 *
 * This class provides:
 * - Dynamic property storage with type-safe access
 * - Metaclass support for type information
 * - Reference handling for object relationships
 * - Workspace and URI management for persistence
 * - Serialization support for server communication
 */
export class DmObject {
    /**
     * Creates a new instance of the DmObject.
     * Automatically generates a unique ID for the object.
     *
     * @param metaClassUri Optional URI of the metaclass defining this object's type
     * @param metaclassWorkspace The workspace containing the metaclass (defaults to "Types" if not specified)
     */
    constructor(metaClassUri, metaclassWorkspace) {
        /** Indicates whether this object is a reference to another object (true) or a full object (false) */
        this.isReference = false;
        this.values = new Array();
        if (metaClassUri !== undefined) {
            this.setMetaClassByUri(metaClassUri, metaclassWorkspace);
        }
        // Generate unique ID for this object instance
        runningId++;
        this.id = 'local_' + runningId.toString();
    }
    /**
     * Creates a reference object pointing to an existing object in a workspace.
     * Reference objects store only workspace and URI information, not the actual property values.
     *
     * @param workspaceId The workspace containing the referenced object
     * @param itemUri The URI of the referenced object
     * @returns A DmObject configured as a reference
     */
    static createFromReference(workspaceId, itemUri) {
        const result = new DmObject();
        result.isReference = true;
        result.workspace = workspaceId;
        result.uri = itemUri;
        return result;
    }
    /**
     * Creates a reference object using a local ID (for objects not yet persisted to the server).
     * The URI is formatted as '#localId' to indicate it's a local reference.
     *
     * @param id Either a string ID or a DmObject (from which the ID will be extracted)
     * @returns A DmObject configured as a local reference
     */
    static createAsReferenceFromLocalId(id) {
        const result = new DmObject();
        result.isReference = true;
        // Extract ID from DmObject if needed
        if (id.id !== undefined) {
            id = id.id;
        }
        result.uri = '#' + id;
        result.id = id;
        return result;
    }
    /**
     * Converts an external property key to its internal storage format.
     * Adds an underscore prefix to avoid conflicts with built-in array methods.
     *
     * @param key The external property key (e.g., "name")
     * @returns The internalized key (e.g., "_name")
     */
    static internalizeKey(key) {
        return "_" + key;
    }
    /**
     * Converts an internal storage key back to its external format.
     * Removes the underscore prefix added by internalizeKey.
     *
     * @param key The internal property key (e.g., "_name")
     * @returns The external key (e.g., "name")
     */
    static externalizeKey(key) {
        return key.substring(1);
    }
    /**
     * Sets a property value on this object.
     *
     * This method:
     * 1. Internalizes the key to avoid conflicts with array methods
     * 2. Creates an ObjectValue if the property doesn't exist
     * 3. Updates the value and marks it as explicitly set
     * 4. Returns true if the value actually changed, false if it remained the same
     *
     * @param key The property name to set
     * @param value The value to assign to the property
     * @returns true if the value changed, false if it was already set to the same value
     */
    set(key, value) {
        const internalizedKey = DmObject.internalizeKey(key);
        let objectValue = this.values[internalizedKey];
        if (objectValue === undefined) {
            objectValue = new ObjectValue();
            this.values[internalizedKey] = objectValue;
        }
        // Track old state to detect changes
        const oldValue = objectValue.value;
        const oldIsSet = objectValue.isSet;
        // Update value and mark as explicitly set
        objectValue.value = value;
        objectValue.isSet = true;
        // Return true if value changed
        return !(oldValue === value && oldIsSet === true);
    }
    /**
     * Retrieves a property value with optional type conversion.
     *
     * This method handles multiple scenarios:
     * 1. Returns default value if property is not explicitly set
     * 2. Converts value based on the specified ObjectType:
     *    - Default/Object: Returns raw value
     *    - Single: Returns first element of array, or value itself
     *    - Array: Wraps single values in array, returns empty array for null/undefined
     *    - String: Converts to string representation
     *    - Boolean: Converts to boolean (handles "0" and "false" strings)
     *    - Number: Converts to number
     *
     * @param key The property name to retrieve
     * @param objectType Optional type specification for conversion
     * @returns The property value, converted according to objectType
     */
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
    /**
     * Retrieves a property value as an array, converting if necessary.
     *
     * This method ensures the property is always an array:
     * - If already an array, returns it as-is
     * - If undefined, creates and sets an empty array
     * - If a single value, wraps it in an array
     *
     * @param key The property name to retrieve as an array
     * @returns The property value as an array
     */
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
    /**
     * Appends a value to a property, treating it as an array.
     *
     * This method handles three scenarios:
     * 1. Property is already an array: pushes the value to it
     * 2. Property is undefined/null: creates a new array with the value
     * 3. Property is a single value: converts to array containing both old and new values
     *
     * @param key The property name to append to
     * @param value The value to append
     */
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
     * Retrieves all property values with externalized keys.
     *
     * This method converts internal storage keys (with underscore prefix) back to
     * their external form, creating an array-like object suitable for iteration.
     *
     * @returns An array-like object with externalized keys and their values
     */
    getPropertyValues() {
        const result = new Array();
        for (let n in this.values) {
            if (!this.values.hasOwnProperty(n)) {
                continue;
            }
            const objectValue = this.values[n];
            // Convert internal key back to external form
            result[DmObject.externalizeKey(n)] = objectValue.value;
        }
        return result;
    }
    /**
     * Checks whether a property has been explicitly set on this object.
     *
     * @param key The property name to check
     * @returns true if the property has been explicitly set, false if it only has a default value or is unset
     */
    isSet(key) {
        const objectValue = this.values[DmObject.internalizeKey(key)];
        return objectValue !== undefined && objectValue.isSet;
    }
    /**
     * Unsets a property, reverting it to its default value.
     * The property will be marked as not explicitly set.
     *
     * @param key The property name to unset
     */
    unset(key) {
        const objectValue = this.values[DmObject.internalizeKey(key)];
        if (objectValue !== undefined) {
            objectValue.isSet = false;
            objectValue.value = objectValue.defaultValue;
        }
    }
    /**
     * Generates a human-readable string representation of this object.
     * Shows all property names and values in a formatted structure.
     *
     * @returns A formatted string representation of the object
     */
    toString() {
        let values = this.getPropertyValues();
        return DmObject.valueToString(values);
    }
    /**
     * Sets the metaclass of this object by URI.
     * The metaclass defines the type and structure of the object.
     *
     * @param metaClassUri The URI of the metaclass
     * @param workspace The workspace containing the metaclass (defaults to "Types")
     */
    setMetaClassByUri(metaClassUri, workspace) {
        if (workspace === undefined) {
            workspace = "Types";
        }
        this.metaClass = { uri: metaClassUri, workspace: workspace };
    }
    /**
     * Recursively converts a value to a formatted string representation.
     *
     * This static helper method handles:
     * - Arrays: Formats as bracketed list with indentation
     * - Objects: Formats as key-value pairs with indentation
     * - Strings: Wraps in quotes
     * - Primitives: Converts to string
     * - null/undefined: Returns literal strings "null"/"undefined"
     *
     * @param item The value to convert to string
     * @param indent Current indentation level for formatting
     * @returns A formatted string representation
     */
    static valueToString(item, indent = "") {
        let result = "";
        let komma = "";
        if (Array.isArray(item)) {
            // Format arrays with brackets and indentation
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
            // Format objects with key-value pairs and indentation
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
            // Wrap strings in quotes
            result = `"${item.toString()}"`;
        }
        else if (item === null) {
            return "null";
        }
        else if (item === undefined) {
            return "undefined";
        }
        else {
            // Convert primitives to string
            result = item.toString();
        }
        return result;
    }
    /**
     * Creates a reference object from an ItemWithNameAndId structure.
     *
     * @param item Object containing workspace and URI information
     * @returns A DmObject configured as a reference
     */
    static createFromItemWithNameAndId(item) {
        return this.createFromReference(item.workspace, item.uri);
    }
}
/**
 * Extended DmObject with synchronization tracking capabilities.
 *
 * This class extends DmObject to track which properties have been modified since
 * the last synchronization with the server. This enables efficient delta updates
 * by only sending changed properties rather than the entire object.
 *
 * Key features:
 * - Tracks property modifications in propertiesSet array
 * - Tracks metaclass changes with isMetaClassSet flag
 * - Overrides set/unset to record changes
 * - Provides clearSync() to reset change tracking after successful synchronization
 */
export class DmObjectWithSync extends DmObject {
    /**
     * Creates a new DmObjectWithSync with change tracking initialized.
     *
     * @param metaClass Optional metaclass URI
     */
    constructor(metaClass) {
        super(metaClass);
        this.propertiesSet = new Array();
        this.isMetaClassSet = true;
    }
    /**
     * Clears the synchronization state, resetting all change tracking.
     * Called after successfully synchronizing with the server.
     */
    clearSync() {
        this.propertiesSet = new Array();
    }
    /**
     * Unsets a property and marks it as modified for synchronization.
     *
     * @param key The property name to unset
     */
    unset(key) {
        super.unset(key);
        this.propertiesSet[key] = true;
    }
    /**
     * Sets a property value and tracks the modification for synchronization.
     *
     * @param key The property name to set
     * @param value The value to assign
     * @returns true if the value changed, false otherwise
     */
    set(key, value) {
        const result = super.set(key, value);
        if (result) {
            this.propertiesSet[key] = true;
        }
        return result;
    }
    /**
     * Sets the metaclass and marks it as modified for synchronization.
     *
     * @param metaClassUri The metaclass URI
     * @param workspace The workspace containing the metaclass
     */
    setMetaClassByUri(metaClassUri, workspace) {
        super.setMetaClassByUri(metaClassUri, workspace);
        this.isMetaClassSet = true;
    }
    /**
     * Creates a reference object with synchronization tracking enabled.
     *
     * @param workspaceId The workspace containing the referenced object
     * @param itemUri The URI of the referenced object
     * @returns A DmObjectWithSync configured as a reference
     */
    static createFromReference(workspaceId, itemUri) {
        const result = new DmObjectWithSync();
        result.isReference = true;
        result.workspace = workspaceId;
        result.uri = itemUri;
        return result;
    }
}
/**
 * Converts a DmObject to an ItemWithNameAndId structure.
 * This simplified representation is used for identifying objects in API calls.
 *
 * @param element The DmObject to convert
 * @returns An ItemWithNameAndId object with identification information
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
/**
 * Serializes a DmObject to JSON format for server communication.
 *
 * This function converts a DmObject into the JSON structure expected by the server.
 * The corresponding C# deserialization is handled by MofJsonDeconverter.Convert.
 *
 * Key behaviors:
 * - For reference objects: Only includes workspace (w) and URI (r)
 * - For full objects: Includes all property values (v) with externalized keys
 * - Nested DmObjects are recursively converted
 * - Arrays are preserved and their contents converted
 * - Always includes metaclass (m) if present
 *
 * @param element The DmObject to serialize
 * @returns A JSON-compatible object ready for server transmission
 */
export function createJsonFromObject(element) {
    const result = {};
    const values = {};
    /**
     * Inner helper function that recursively converts values for JSON serialization.
     * Handles arrays, DmObjects, and primitive values.
     */
    function convertValue(elementValue) {
        if (Array.isArray(elementValue)) {
            // Convert array elements recursively
            const value = {};
            for (let n in elementValue) {
                const childItem = elementValue[n];
                value[n] = convertValue(childItem);
            }
            return value;
        }
        else if (((typeof elementValue === "object" || typeof elementValue === "function") && (elementValue !== null))) {
            if (elementValue instanceof DmObject) {
                // Recursively serialize nested DmObjects
                return createJsonFromObject(elementValue);
            }
            else {
                return elementValue;
            }
        }
        else {
            // Return primitive values as-is
            return elementValue;
        }
    }
    // Build the JSON structure
    if (!element.isReference) {
        // For full objects, serialize all property values
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
        // For reference objects, only include workspace and URI
        result.r = element.uri;
        result.w = element.workspace;
    }
    // Include metaclass if present
    if (element.metaClass !== undefined && element.metaClass !== null) {
        result.m = element.metaClass;
    }
    // Always include the local ID
    result.id = element.id;
    // Only include values object if it has content
    if (Object.keys(values).length > 0) {
        result.v = values;
    }
    return result;
}
/**
 * Converts a value from the server's JSON format to TypeScript types.
 *
 * This function intelligently handles different value types:
 * - Primitives: Returns as-is
 * - Arrays: Recursively converts each element
 * - Objects: Converts to DmObject instances
 *
 * @param element The value to convert (can be primitive, array, or object)
 * @returns The converted value in appropriate TypeScript format
 */
export function convertJsonObjectToObjects(element) {
    if (Array.isArray(element)) {
        // Recursively convert array elements
        const arrayResult = [];
        for (let m in element) {
            const inner = element[m];
            arrayResult.push(convertJsonObjectToObjects(inner));
        }
        return arrayResult;
    }
    else if ((typeof element === "object" || typeof element === "function") && (element !== null)) {
        // Convert objects to DmObject
        return convertJsonObjectToDmObject(element);
    }
    // Return primitives as-is
    return element;
}
/**
 * Deserializes JSON from the server into a DmObjectWithSync instance.
 *
 * This function is the counterpart to createJsonFromObject, converting the server's
 * JSON representation back into a fully-featured DmObject. The corresponding C# class
 * is DatenMeister.Modules.Json.MofJsonConverter.Convert.
 *
 * Key behaviors:
 * - Handles both string JSON and parsed objects
 * - Returns undefined for null, undefined, or empty string inputs
 * - Reconstructs property values with isSet tracking
 * - Recursively converts nested objects and arrays
 * - Restores metaclass, URI, workspace, and extent information
 * - Clears sync tracking (assumes object just came from server)
 *
 * JSON structure expected:
 * - v: Property values (array format: [isSet, value])
 * - m: Metaclass information
 * - u: Object URI
 * - id: Local identifier
 * - r: Reference URI (for reference objects)
 * - e: Extent URI
 * - w: Workspace ID
 *
 * @param element The JSON object or string to deserialize
 * @returns A DmObjectWithSync instance, or undefined if input is empty
 */
export function convertJsonObjectToDmObject(element) {
    // Handle null, undefined, or empty inputs
    if (element === undefined || element === null || element === "") {
        return undefined;
    }
    // Parse string JSON if needed
    if (typeof element === 'string' || element instanceof String) {
        element = JSON.parse(element);
    }
    const result = new DmObjectWithSync();
    const elementValues = element["v"];
    // Reconstruct property values
    if (elementValues !== undefined && elementValues !== null) {
        for (let key in elementValues) {
            if (Object.prototype.hasOwnProperty.call(elementValues, key)) {
                let valueArray = elementValues[key];
                const isPropertySet = valueArray[0];
                let value = valueArray[1];
                if (Array.isArray(value)) {
                    // Recursively convert array elements
                    const finalValue = [];
                    for (const m in value) {
                        if (!(value.hasOwnProperty(m))) {
                            continue;
                        }
                        let arrayValue = value[m];
                        if ((typeof arrayValue === "object" || typeof arrayValue === "function") && (arrayValue !== null)) {
                            // Convert nested objects to DmObject
                            arrayValue = convertJsonObjectToDmObject(arrayValue);
                        }
                        finalValue.push(arrayValue);
                    }
                    value = finalValue;
                }
                else if ((typeof value === "object" || typeof value === "function") && (value !== null)) {
                    // Convert nested object to DmObject
                    value = convertJsonObjectToDmObject(value);
                }
                // Store the property value with isSet tracking
                const internalizedKey = DmObject.internalizeKey(key);
                const objectValue = new ObjectValue();
                objectValue.isSet = isPropertySet;
                if (isPropertySet === false) {
                    objectValue.defaultValue = value;
                }
                else {
                    objectValue.value = value;
                }
                result["values"][internalizedKey] = objectValue;
            }
        }
        // Clear sync state (object came from server, so it's in sync)
        result.clearSync();
    }
    // Restore metaclass
    const elementMetaClass = element["m"];
    if (elementMetaClass !== undefined && elementMetaClass !== null) {
        result.metaClass = elementMetaClass;
    }
    // Restore URI
    const elementUri = element["u"];
    if (elementUri !== undefined && elementUri !== null) {
        result.uri = elementUri;
    }
    // Restore local ID
    const elementId = element["id"];
    if (elementId !== undefined && elementId !== null) {
        result.id = elementId;
    }
    // Handle reference objects (r = reference URI)
    const elementReferenceUri = element["r"];
    if (elementReferenceUri !== undefined && elementReferenceUri !== null) {
        result.uri = elementReferenceUri;
        result.isReference = true;
    }
    // Restore extent URI
    const extentUri = element["e"];
    if (extentUri !== undefined && extentUri !== null) {
        result.extentUri = extentUri;
    }
    // Restore workspace
    const workspace = element["w"];
    if (workspace !== undefined && workspace !== null) {
        result.workspace = workspace;
    }
    return result;
}
/**
 * Extracts a display name from a value.
 *
 * This utility function provides a consistent way to get human-readable names from various types:
 * - null/undefined: Returns "Null"
 * - Arrays: Returns comma-separated names of elements
 * - DmObjects: Returns the 'name' property value
 * - Other objects: Attempts to call toString()
 * - Primitives: Returns as-is
 *
 * Commonly used for displaying object names in UI components.
 *
 * @param value The value to extract a name from
 * @returns A string representation of the value's name
 */
export function getName(value) {
    if (value === null || value === undefined) {
        return "Null";
    }
    if (Array.isArray(value)) {
        // Build comma-separated list of array element names
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
        // For DmObjects, get the 'name' property
        return value.get('name');
    }
    if (value.toString !== undefined) {
        // Use toString for objects with that method
        return value.toString();
    }
    // Return primitives directly
    return value;
}
//# sourceMappingURL=Mof.js.map