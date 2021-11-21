define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.convertJsonObjectToDmObject = exports.createJsonFromObject = exports.DmObject = void 0;
    class DmObject {
        constructor() {
            this.values = new Array();
        }
        set(key, value) {
            this.values[key] = value;
        }
        get(key) {
            return this.values[key];
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
        isSet(key) {
            return this.values[key] !== undefined;
        }
        unset(key) {
            this.values[key] = undefined;
        }
        toString() {
            let values = this.values;
            return DmObject.valueToString(values);
        }
        static valueToString(item, indent = "") {
            var result = "";
            var komma = "";
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
                        result += `${komma}\r\n${indent}${key}: ${DmObject.valueToString(value, indent + "  ")}`;
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
    /*
        Converts the given element to a json element that it can be used to send to the webserver
        The receiving function is MofJsonDeconverter.Convert in which the retrieved
        value is returned to MofObject
     */
    function createJsonFromObject(element) {
        const result = { v: {} };
        const values = result.v;
        for (const key in element.values) {
            if (!element.values.hasOwnProperty(key)) {
                continue;
            }
            var elementValue = element.get(key);
            if (Array.isArray(elementValue)
                || ((typeof elementValue === "object" || typeof elementValue === "function") && (elementValue !== null))) {
                // Do not send out arrays or objects
                continue;
            }
            values[key] = element.get(key);
        }
        return result;
    }
    exports.createJsonFromObject = createJsonFromObject;
    /*
    // Creates the given object from the included json
    // The corresponding C# class is DatenMeister.Modules.Json.MofJsonConverter.Convert
    */
    function convertJsonObjectToDmObject(element) {
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
});
//# sourceMappingURL=Mof.js.map