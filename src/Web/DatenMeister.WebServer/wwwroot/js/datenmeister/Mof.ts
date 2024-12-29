import {EntentType, ItemWithNameAndId} from "./ApiModels.js";

export enum ObjectType{
    Default, 
    Single, 
    String,
    Array,
    Boolean,
    Number,
    
    Object
}

type DmObjectReturnType<T> = 
    T extends ObjectType.Default ? any : 
        T extends ObjectType.Single ? any:
            T extends ObjectType.String ? string : 
                T extends ObjectType.Array ? Array<any> :
                    T extends ObjectType.Boolean ? boolean :
                        T extends ObjectType.Object ? DmObject :
                            T extends ObjectType.Number ? number : any;


// This id is a running id being used to define IDs of recently created objects
let runningId = 0;
export class DmObject {
    private readonly values: Array<any>;

    metaClass: ItemWithNameAndId;

    uri: string | undefined;

    isReference: boolean = false;

    extentUri: string;

    workspace: string;
    
    id: string; 

    /**
     * Creates a new instance of the MofObject
      * @param metaClassUri A possible metaclass Uri
     * @param metaclassWorkspace the workspace in which the metaclass is residing
     */    
    constructor(metaClassUri?: string | undefined, metaclassWorkspace?: string) {
        this.values = new Array<any>();

        if (metaClassUri !== undefined) {
            this.setMetaClassByUri(metaClassUri, metaclassWorkspace);
        }

        runningId++;
        this.id = 'local_' + runningId.toString();
    }
    
    static createFromReference(workspaceId: string, itemUri: string)
    {
        const result = new DmObject();
        result.isReference = true;
        result.workspace = workspaceId;
        result.uri = itemUri;
        
        return result;
    }

    static createAsReferenceFromLocalId(id: string | DmObject) {
        const result = new DmObject();
        result.isReference = true;

        if ((id as DmObject).id !== undefined) {
            id = (id as DmObject).id;
        }
        result.uri = '#' + id;
        result.id = id as string;

        return result;
    }

    /**
     * Modifies the key that it can be used for internal array storage. 
     * Unfortunately, the array has some functions and these functions cannot be overwritten
     * @param key
     */
    static internalizeKey(key:string)
    {
        return "_" + key;        
    }

    /**
     * Modifies the key that the internal key value can be used for external access. 
     * This is the opposite of internalizeKey
     * @param key
     */
    static externalizeKey(key: string)
    {
        return key.substring(1);
    }

    /**
     * Sets a value and returns whether the value has been changed
     * @param key Key to be set
     * @param value Value to be set
     */
    set(key: string, value: any): boolean {
        var internalizedKey = DmObject.internalizeKey(key);
        const oldValue = this.values[internalizedKey];
        this.values[internalizedKey] = value;
        return !(oldValue === value);
    }

    get<T extends ObjectType>(key: string, objectType?: T): DmObjectReturnType<T> {
        let result = this.values[DmObject.internalizeKey(key)];

        switch (objectType) {
            case ObjectType.Default:
                return result as DmObjectReturnType<T>;
            case ObjectType.Object:
                return result as DmObjectReturnType<T>;
            case ObjectType.Single:
                if (Array.isArray(result)) {
                    return result[0] as DmObjectReturnType<T>;
                }

                return result;

            case ObjectType.Array:
                if (result === undefined || result === null) {
                    return [] as DmObjectReturnType<T>;
                }
                if (Array.isArray(result)) {
                    return result as DmObjectReturnType<T>;
                }
                return [result] as DmObjectReturnType<T>;

            case ObjectType.String:
                const resultString = this.get(key, ObjectType.Single);
                if (resultString === undefined) {
                    return undefined;
                }
                return resultString.toString() as DmObjectReturnType<T>;

            case ObjectType.Boolean:
                if (Array.isArray(result)) {
                    result = result[0];
                }
                
                // Take the standard routine but also check that there is no '0' in the text
                return (Boolean(result) && result !== "0" && result !== "false") as DmObjectReturnType<T>;

            case ObjectType.Number:
                 return result === undefined ? undefined : Number(result) as DmObjectReturnType<T>;
        }

        return result as DmObjectReturnType<T>;
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

    /**
     * Gets an enumeration of all property values. 
     * This method is used to protect the internal transformation of key values according
     * internalize and externalize
     */
    getPropertyValues() {
        const result = new Array<any>();
        for (let n in this.values) {

            if (!this.values.hasOwnProperty(n)) {
                continue;
            }
            
            result[DmObject.externalizeKey(n)] = this.values[n];
        }

        return result;
    }

    isSet(key: string): boolean {
        return this.values[DmObject.internalizeKey(key)] !== undefined;
    }

    unset(key: string): void {
        this.values[DmObject.internalizeKey(key)] = undefined;
    }

    toString(): string {
        let values = this.getPropertyValues();

        return DmObject.valueToString(values);
    }

    setMetaClassByUri(metaClassUri: string | undefined, workspace?: string | undefined) {
        if (workspace === undefined) {
            workspace = "Types";
        }

        this.metaClass = {uri: metaClassUri, workspace: workspace };
    }

    static valueToString(item: any, indent: string = ""): string {

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
        } else if ((typeof item === "object" || typeof item === "function") && (item !== null)) {
            for (let key in item) {
                if (Object.prototype.hasOwnProperty.call(item, key)) {
                    const value = item[key];
                    const externalKey = DmObject.externalizeKey(key);
                    
                    result += `${komma}\r\n${indent}${externalKey}: ${DmObject.valueToString(value, indent + "  ")}`;
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

export class DmObjectWithSync extends DmObject
{
    /**
     * Defines the list of properties that were set since the last sync
     */
    propertiesSet: boolean[];
    
    isMetaClassSet: boolean;
    
    constructor(metaClass?: string) {
        super(metaClass);
        this.propertiesSet = new Array<boolean>();
        this.isMetaClassSet = true;
    }
    
    clearSync() {
        this.propertiesSet = new Array<boolean>();
    }
    
    unset(key: string): void {
        super.unset(key);
        this.propertiesSet[key] = true;
    }

    set(key: string, value: any): boolean {
        const result = super.set(key, value);
        if (result) {
            this.propertiesSet[key] = true;
        }
        
        return result;
    }

    setMetaClassByUri(metaClassUri: string | undefined, workspace?: string | undefined) {
        super.setMetaClassByUri(metaClassUri, workspace);

        this.isMetaClassSet = true;

    }

    static createFromReference(workspaceId: string, itemUri: string)
    {
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
export function convertToItemWithNameAndId(element: DmObject) {
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
export function createJsonFromObject(element: DmObject) {
    const result = {id: {}, v: {}, m: {}, r: "", w: ""};
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

        } else if (((typeof elementValue === "object" || typeof elementValue === "function") && (elementValue !== null))) {
            // This is an object, so perform the transformation
            return createJsonFromObject(elementValue);

        } else {
            return elementValue;
        }
    }

    if (!element.isReference) {
        for (const key in element.getPropertyValues()) {
            let elementValue = element.get(key);
            values[key] = convertValue(elementValue);
        }
    } else {
        // Object is reference
        result.r = element.uri;
        result.w = element.workspace;
    }

    if (element.metaClass !== undefined && element.metaClass !== null) {
        result.m = element.metaClass;
    }
    
    result.id = element.id;

    return result;
}

/**
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
export function convertJsonObjectToDmObject(element: object | string | undefined): DmObjectWithSync | undefined {
    
    if( element === undefined || element === null || element === "") {
        return undefined;
    }
    
    if (typeof element === 'string' || element instanceof String) {
        element = JSON.parse(element as string);
    }

    const result = new DmObjectWithSync();
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
    
    const elementId= element["id"];
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

export function getName(value: any):string {
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