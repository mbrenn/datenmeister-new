export interface ItemWithNameAndId
{
    name: string;
    extentUri: string;
    fullName: string;
    id: string;
}

export class DmObject
{
    values: Object;
    
    metaClass: ItemWithNameAndId;

    constructor() {
        this.values = {};
    }

    set(key: string, value: object): void
    {
        this.values[key] = value;
    }

    get(key: string): object
    {
        return this.values[key];
    }

    isSet(key:string): boolean
    {
        return this.values[key] !== undefined;
    }

    unset(key:string): void
    {
        this.values[key] = undefined;
    }

    toString(): string {
        let values = this.values;

        return DmObject.valueToString(values);
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

// Creates the given object from the included json
// The corresponding C# class is DatenMeister.Modules.Json.MofJsonConverter.Convert
export function createObjectFromJson(json: string, metaClass?: ItemWithNameAndId): DmObject {
    const converted = JSON.parse(json);

    function convertJsonToDmObject(element: object): DmObject {
        const result = new DmObject();
        const elementValues = element["v"];
        
        if(elementValues !== undefined && elementValues !== null) {
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
                                arrayValue = convertJsonToDmObject(arrayValue);
                            }

                            finalValue.push(arrayValue);
                        }

                        value = finalValue;
                    } else if ((typeof value === "object" || typeof value === "function") && (value !== null)) {
                        // Converts to DmObject, if item is an object

                        value = convertJsonToDmObject(value);
                    }

                    result.set(key, value);
                }
            }
        }

        const elementMetaClass = element["m"];
        if (elementMetaClass !== undefined && elementMetaClass !== null) {
            result.metaClass = elementMetaClass;
        }

        return result;
    }

    let result2 = convertJsonToDmObject(converted);

    if (metaClass !== undefined && metaClass !== null) {
        result2.metaClass = metaClass;
    }

    return result2;
}