define(["require", "exports", "./ApiModels"], function (require, exports, ApiModels_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getName = exports.convertJsonObjectToDmObject = exports.convertJsonObjectToObjects = exports.createJsonFromObject = exports.convertToItemWithNameAndId = exports.DmObject = exports.ObjectType = void 0;
    var ObjectType;
    (function (ObjectType) {
        ObjectType[ObjectType["Default"] = 0] = "Default";
        ObjectType[ObjectType["Single"] = 1] = "Single";
        ObjectType[ObjectType["String"] = 2] = "String";
        ObjectType[ObjectType["Array"] = 3] = "Array";
        ObjectType[ObjectType["Boolean"] = 4] = "Boolean";
    })(ObjectType = exports.ObjectType || (exports.ObjectType = {}));
    class DmObject {
        constructor() {
            this.isReference = false;
            this.values = new Array();
        }
        static createFromReference(workspaceId, itemUri) {
            const result = new DmObject();
            result.isReference = true;
            result.workspace = workspaceId;
            result.uri = itemUri;
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
        set(key, value) {
            this.values[DmObject.internalizeKey(key)] = value;
        }
        get(key, objectType) {
            let result = this.values[DmObject.internalizeKey(key)];
            switch (objectType) {
                case ObjectType.Default:
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
                    return resultString.toString();
                case ObjectType.Boolean:
                    if (Array.isArray(result)) {
                        result = result[0];
                    }
                    // Take the standard routine but also check that there is no '0' in the text
                    return (Boolean(result) && result !== "0" && result !== "false");
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
                result[DmObject.externalizeKey(n)] = this.values[n];
            }
            return result;
        }
        isSet(key) {
            return this.values[DmObject.internalizeKey(key)] !== undefined;
        }
        unset(key) {
            this.values[DmObject.internalizeKey(key)] = undefined;
        }
        toString() {
            let values = this.getPropertyValues();
            return DmObject.valueToString(values);
        }
        setMetaClassByUri(metaClassUri) {
            this.metaClass = { uri: metaClassUri };
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
    }
    exports.DmObject = DmObject;
    /**
     * Takes the given object and exports it as an ItemWithNameAndId
     * @param element
     */
    function convertToItemWithNameAndId(element) {
        return {
            uri: element.uri,
            extentUri: element.extentUri,
            workspace: element.workspace,
            ententType: ApiModels_1.EntentType.Item
        };
    }
    exports.convertToItemWithNameAndId = convertToItemWithNameAndId;
    /*
        Converts the given element to a json element that it can be used to send to the webserver
        The receiving function is MofJsonDeconverter.Convert in which the retrieved
        value is returned to MofObject
     */
    function createJsonFromObject(element) {
        const result = { v: {}, m: {}, r: "", w: "" };
        const values = result.v;
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
                // This is an object, so perform the transformation
                return createJsonFromObject(elementValue);
            }
            else {
                return elementValue;
            }
        }
        if (!element.isReference) {
            for (const key in element.getPropertyValues()) {
                let elementValue = element.get(key);
                values[key] = convertValue(elementValue);
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
        return result;
    }
    exports.createJsonFromObject = createJsonFromObject;
    /**
     * Converts the Object as given by the server to the JS-World.
     * In case of native objects, the native object will be returned.
     * In case of arrays, the arrays.
     * In case of elements, the corresponding DmObjects
     */
    function convertJsonObjectToObjects(element) {
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
    exports.convertJsonObjectToObjects = convertJsonObjectToObjects;
    /*
    // Creates the given object from the included json
    // The corresponding C# class is DatenMeister.Modules.Json.MofJsonConverter.Convert
    */
    function convertJsonObjectToDmObject(element) {
        if (element === undefined || element === null) {
            return undefined;
        }
        if (typeof element === 'string' || element instanceof String) {
            element = JSON.parse(element);
        }
        const result = new DmObject();
        const elementValues = element["v"];
        if (elementValues !== undefined && elementValues !== null) {
            for (let key in elementValues) {
                if (Object.prototype.hasOwnProperty.call(elementValues, key)) {
                    let value = elementValues[key];
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
                    result.set(key, value);
                }
            }
        }
        const elementMetaClass = element["m"];
        if (elementMetaClass !== undefined && elementMetaClass !== null) {
            result.metaClass = elementMetaClass;
        }
        const elementUri = element["u"];
        if (elementUri !== undefined && elementUri !== null) {
            result.uri = elementUri;
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
    exports.convertJsonObjectToDmObject = convertJsonObjectToDmObject;
    function getName(value) {
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
    exports.getName = getName;
});
//# sourceMappingURL=Mof.js.map