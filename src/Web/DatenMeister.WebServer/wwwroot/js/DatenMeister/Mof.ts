export class DmObject
{
    values: Array<object>;

    constructor() {
        this.values = new Array<object>();
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
        var result = "";
        var komma = "";
        let values = this.values;
        for (var key in values) {
            if (Object.prototype.hasOwnProperty.call(values, key)) {
                var value = this.values[key];

                result += komma + "[" + key + "]: " + value;
                komma = ", ";
            }
        }

        return result;
    }
}

// Creates the given object from the included json
export function createObjectFromJson(json: string): DmObject {
    var result = new DmObject();

    var converted = JSON.parse(json);
    for (var key in converted) {
        if (Object.prototype.hasOwnProperty.call(converted, key)) {
            var value = converted[key];
            result.set(key, value);
        }
    }

    return result;
}