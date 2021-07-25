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
        } else {
            result = item.toString();
        }

        return result;
    }
}

// Creates the given object from the included json
export function createObjectFromJson(json: string, metaClass?: ItemWithNameAndId): DmObject {
    var result = new DmObject();

    var converted = JSON.parse(json);
    for (var key in converted) {
        if (Object.prototype.hasOwnProperty.call(converted, key)) {
            var value = converted[key];
            result.set(key, value);
        }
    }
    
    if (metaClass !== undefined && metaClass !== null)
    {
        result.metaClass = metaClass;
    }

    return result;
}