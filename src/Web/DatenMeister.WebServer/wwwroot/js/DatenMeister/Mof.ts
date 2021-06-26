export class DmObject
{
    _values: Array<object>;

    constructor() {
        this._values = new Array<object>();
    }

    set(key: string, value: object): void
    {
        this._values[key] = value;
    }

    get(key: string): object
    {
        return this._values[key];
    }

    isSet(key:string): boolean
    {
        return this._values[key] !== undefined;
    }

    unset(key:string): void
    {
        this._values[key] = undefined;
    }
}