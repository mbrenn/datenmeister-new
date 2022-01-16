import {ItemWithNameAndId} from "./ApiModels";

export class DmObject {
    values: Array<any>;

    metaClass: ItemWithNameAndId;

    uri: string;

    extentUri: string;

    workspace: string;

    constructor() {
        this.values = new Array<any>();
    }

    set(key: string, value: any): void {
        this.values[key] = value;
    }

    get(key: string): any {
        return this.values[key];
    }

    getAsArray(key: string): any {
        const value = this.get(key);
        if (Array.isArray(value)) {
            return value;
        }

        if (value === undefined) {
            const newArray = [];
            this.set(key, newArray);
            return newArray;
        } else {
            const newArray = [value];
            this.set(key, newArray);
            return newArray;
        }
    }

    isSet(key: string): boolean {
        return this.values[key] !== undefined;
    }

    unset(key: string): void {
        this.values[key] = undefined;
    }

    toString(): string {
        let values = this.values;

        return DmObject.valueToString(values);
    }

    setMetaClassByUri(metaClassUri: string) {
        this.metaClass = {uri: metaClassUri};
    }

    setMetaClassById(metaClassId: string) {
        this.metaClass = {id: metaClassId};
    }

    static valueToString(item: any, indent: string = ""): string {

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
        } else if ((typeof item === "object" || typeof item === "function") && (item !== null)) {
            for (let key in item) {
                if (Object.prototype.hasOwnProperty.call(item, key)) {
                    const value = item[key];

                    result += `${komma}\r\n${indent}${key}: ${DmObject.valueToString(value, indent + "  ")}`;
                    komma = ", ";
                }
            }
        } else if (typeof item === "string" || item instanceof String) {
            result = `"${item.toString()}"`;
        } else if (item === null) {
            return "null";
        } else if (item === undefined) {
            return "undefined";
        } else {
            result = item.toString();
        }

        return result;
    }
}

/*
    Converts the given element to a json element that it can be used to send to the webserver
    The receiving function is MofJsonDeconverter.Convert in which the retrieved
    value is returned to MofObject
 */
export function createJsonFromObject(element: DmObject) {
    const result = { v: {}, m: {} };
    const values = result.v;

    for (const key in element.values) {
        if (!element.values.hasOwnProperty(key)) {
            continue;
        }

        let elementValue = element.get(key);
        if (Array.isArray(elementValue) ||
            ((typeof elementValue === "object" || typeof elementValue === "function") && (elementValue !== null))) {
            // Do not send out arrays or objects
            continue;
        }

        values[key] = element.get(key);
    }

    if (element.metaClass !== undefined && element.metaClass !== null) {
        result.m = element.metaClass;
    }

    return result;
}

/*
 * Converts the Object as given by the server to the JS-World. 
 * In case of native objects, the native object will be returned. 
 * In case of arrays, the arrays. 
 * In case of elements, the corresponding DmObjects
 */
export function convertJsonObjectToObjects(element: any): any {
    if (Array.isArray(element)) {
        const arrayResult = [];
        for (let m in element) {
            const inner = element[m];
            arrayResult.push(convertJsonObjectToObjects(inner));
        }

        return arrayResult;
    } else if ((typeof element === "object" || typeof element === "function") && (element !== null)) {
        return convertJsonObjectToDmObject(element);
    }

    return element;
}

/*
// Creates the given object from the included json
// The corresponding C# class is DatenMeister.Modules.Json.MofJsonConverter.Convert
*/
export function convertJsonObjectToDmObject(element: object | string): DmObject {
    if (typeof element === 'string' || element instanceof String) {
        element = JSON.parse(element as string);
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
                        if (!((value as object[]).hasOwnProperty(m))) {
                            continue;
                        }

                        let arrayValue = value[m];
                        if ((typeof arrayValue === "object" || typeof arrayValue === "function") && (arrayValue !== null)) {
                            arrayValue = convertJsonObjectToDmObject(arrayValue);
                        }

                        finalValue.push(arrayValue);
                    }

                    value = finalValue;
                } else if ((typeof value === "object" || typeof value === "function") && (value !== null)) {
                    // Converts to DmObject, if item is an object

                    value = convertJsonObjectToDmObject(value);
                }

                result.set(key, value);
            }
        }
    }

    const elementMetaClass = element["m"];
    if (elementMetaClass !== undefined && elementMetaClass !== null) {        
        result.metaClass =  elementMetaClass;
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