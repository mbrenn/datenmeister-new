(() => {
  // ../DatenMeister.WebServer/Assets/js/datenmeister/ApiModels.js
  var EntentType;
  (function(EntentType2) {
    EntentType2["Item"] = "Item";
    EntentType2["Extent"] = "Extent";
    EntentType2["Workspace"] = "Workspace";
  })(EntentType || (EntentType = {}));

  // ../DatenMeister.WebServer/Assets/js/datenmeister/Mof.js
  var ObjectType;
  (function(ObjectType2) {
    ObjectType2[ObjectType2["Default"] = 0] = "Default";
    ObjectType2[ObjectType2["Single"] = 1] = "Single";
    ObjectType2[ObjectType2["String"] = 2] = "String";
    ObjectType2[ObjectType2["Array"] = 3] = "Array";
    ObjectType2[ObjectType2["Boolean"] = 4] = "Boolean";
    ObjectType2[ObjectType2["Number"] = 5] = "Number";
    ObjectType2[ObjectType2["Object"] = 6] = "Object";
  })(ObjectType || (ObjectType = {}));
  var runningId = 0;
  var ObjectValue = class {
  };
  var DmObject = class _DmObject {
    /**
     * Creates a new instance of the DmObject.
     * Automatically generates a unique ID for the object.
     *
     * @param metaClassUri Optional URI of the metaclass defining this object's type
     * @param metaclassWorkspace The workspace containing the metaclass (defaults to "Types" if not specified)
     */
    constructor(metaClassUri, metaclassWorkspace) {
      this.uri = "";
      this.isReference = false;
      this.values = new Array();
      if (metaClassUri !== void 0) {
        this.setMetaClassByUri(metaClassUri, metaclassWorkspace);
      }
      runningId++;
      this.id = "local_" + runningId.toString();
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
      const result = new _DmObject();
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
      if (id === void 0)
        return void 0;
      const result = new _DmObject();
      result.isReference = true;
      if (id.id !== void 0) {
        result.referencedObject = id;
        id = id.id;
      }
      result.uri = "#" + id;
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
      const internalizedKey = _DmObject.internalizeKey(key);
      let objectValue = this.values[internalizedKey];
      if (objectValue === void 0) {
        objectValue = new ObjectValue();
        this.values[internalizedKey] = objectValue;
      }
      const oldValue = objectValue.value;
      const oldIsSet = objectValue.isSet;
      objectValue.value = value;
      objectValue.isSet = true;
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
      const objectValue = this.values[_DmObject.internalizeKey(key)];
      let hasValue = true;
      let result = objectValue?.value;
      if (objectValue !== void 0 && objectValue.isSet === false) {
        result = objectValue.defaultValue;
        if (result === void 0 || result === null)
          hasValue = false;
      }
      switch (objectType) {
        case ObjectType.Default:
          if (!hasValue)
            return void 0;
          if (result instanceof _DmObject) {
            if (result.isReference && result.referencedObject !== void 0) {
              return result.referencedObject;
            }
          }
          return result;
        case ObjectType.Object:
          if (!hasValue)
            return void 0;
          if (result instanceof _DmObject) {
            if (result.isReference && result.referencedObject !== void 0) {
              return result.referencedObject;
            }
          }
          return result;
        case ObjectType.Single:
          if (!hasValue)
            return void 0;
          if (Array.isArray(result)) {
            return result[0];
          }
          return result;
        case ObjectType.Array:
          if (!hasValue || result === void 0 || result === null) {
            return [];
          }
          if (Array.isArray(result)) {
            return result;
          }
          return [result];
        case ObjectType.String:
          const resultString = Array.isArray(result) ? result[0] : result;
          if (resultString === void 0 || resultString === null) {
            return void 0;
          }
          return resultString?.toString() ?? "";
        case ObjectType.Boolean:
          if (Array.isArray(result)) {
            result = result[0];
          }
          return Boolean(result) && result !== "0" && result !== "false";
        case ObjectType.Number:
          if (!hasValue || result === void 0 || result === null) {
            return Number(0);
          }
          return Number(result);
      }
      return result;
    }
    isReferenced(key) {
      const objectValue = this.values[_DmObject.internalizeKey(key)];
      if (objectValue instanceof ObjectValue) {
        if (objectValue.value instanceof _DmObject) {
          return objectValue.value.isReference === true;
        }
      }
      return false;
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
      if (value === void 0 || value === null) {
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
      if (existingValue === void 0 || existingValue === null) {
        this.set(key, [value]);
        return;
      } else {
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
        result[_DmObject.externalizeKey(n)] = objectValue.value;
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
      const objectValue = this.values[_DmObject.internalizeKey(key)];
      return objectValue !== void 0 && objectValue.isSet;
    }
    /**
     * Unsets a property, reverting it to its default value.
     * The property will be marked as not explicitly set.
     *
     * @param key The property name to unset
     */
    unset(key) {
      const objectValue = this.values[_DmObject.internalizeKey(key)];
      if (objectValue !== void 0) {
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
      return _DmObject.valueToString(values);
    }
    /**
     * Sets the metaclass of this object by URI.
     * The metaclass defines the type and structure of the object.
     *
     * @param metaClassUri The URI of the metaclass
     * @param workspace The workspace containing the metaclass (defaults to "Types")
     */
    setMetaClassByUri(metaClassUri, workspace) {
      if (metaClassUri === void 0)
        return;
      if (workspace === void 0) {
        workspace = "Types";
      }
      this.metaClass = { uri: metaClassUri, workspace };
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
        result = `\r
${indent}[`;
        for (let n in item) {
          if (Object.prototype.hasOwnProperty.call(item, n)) {
            const value = item[n];
            result += `${komma}${this.valueToString(value, indent + "  ")}`;
            komma = ", ";
          }
        }
        result += "]";
      } else if ((typeof item === "object" || typeof item === "function") && item !== null) {
        for (let key in item) {
          if (Object.prototype.hasOwnProperty.call(item, key)) {
            const value = item[key];
            const externalKey = _DmObject.externalizeKey(key);
            result += `${komma}\r
${indent}${externalKey}: ${_DmObject.valueToString(value, indent + "  ")}`;
            komma = ", ";
          }
        }
      } else if (typeof item === "string" || item instanceof String) {
        result = `"${item.toString()}"`;
      } else if (item === null) {
        return "null";
      } else if (item === void 0) {
        return "undefined";
      } else {
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
      return this.createFromReference(item.workspace ?? "", item.uri);
    }
  };
  var DmObjectWithSync = class _DmObjectWithSync extends DmObject {
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
      const result = new _DmObjectWithSync();
      result.isReference = true;
      result.workspace = workspaceId;
      result.uri = itemUri;
      return result;
    }
  };
  function createJsonFromObject(element) {
    const result = {};
    const values = {};
    function convertValue(elementValue) {
      if (Array.isArray(elementValue)) {
        const value = [];
        for (let n in elementValue) {
          const childItem = elementValue[n];
          value.push(convertValue(childItem));
        }
        return value;
      } else if ((typeof elementValue === "object" || typeof elementValue === "function") && elementValue !== null) {
        if (elementValue instanceof DmObject) {
          return createJsonFromObject(elementValue);
        } else {
          return elementValue;
        }
      } else {
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
        values[key] = [objectValue.isSet, convertValue(objectValue.value)];
      }
    } else {
      result.r = element.uri;
      result.w = element.workspace;
    }
    if (element.metaClass !== void 0 && element.metaClass !== null) {
      result.m = element.metaClass;
    }
    result.id = element.id;
    if (Object.keys(values).length > 0) {
      result.v = values;
    }
    return result;
  }
  function convertJsonObjectToObjects(element, extent = void 0, workspace = void 0) {
    if (Array.isArray(element)) {
      const arrayResult = [];
      for (let m in element) {
        const inner = element[m];
        arrayResult.push(convertJsonObjectToObjects(inner, extent, workspace));
      }
      return arrayResult;
    } else if ((typeof element === "object" || typeof element === "function") && element !== null) {
      return convertJsonObjectToDmObject(element, extent, workspace);
    }
    return element;
  }
  function convertJsonObjectToDmObject(element, extent = void 0, workspace = void 0) {
    if (element === void 0 || element === null || element === "") {
      return void 0;
    }
    if (typeof element === "string" || element instanceof String) {
      element = JSON.parse(element);
    }
    const result = new DmObjectWithSync();
    if (extent !== void 0) {
      result.extentUri = extent;
    }
    if (workspace !== void 0) {
      result.workspace = workspace;
    }
    const extentUri = element["e"];
    if (extentUri !== void 0 && extentUri !== null) {
      result.extentUri = extentUri;
    }
    const elementWorkspace = element["w"];
    if (elementWorkspace !== void 0 && elementWorkspace !== null) {
      result.workspace = elementWorkspace;
    }
    const elementValues = element["v"];
    if (elementValues !== void 0 && elementValues !== null) {
      for (let key in elementValues) {
        if (Object.prototype.hasOwnProperty.call(elementValues, key)) {
          let valueArray = elementValues[key];
          const isPropertySet = valueArray[0];
          let value = valueArray[1];
          if (Array.isArray(value)) {
            const finalValue = [];
            for (const m in value) {
              if (!value.hasOwnProperty(m)) {
                continue;
              }
              let arrayValue = value[m];
              if ((typeof arrayValue === "object" || typeof arrayValue === "function") && arrayValue !== null) {
                arrayValue = convertJsonObjectToDmObject(arrayValue, result.extentUri, result.workspace);
              }
              finalValue.push(arrayValue);
            }
            value = finalValue;
          } else if ((typeof value === "object" || typeof value === "function") && value !== null) {
            if (value.hasOwnProperty("0") && !value.hasOwnProperty("v") && !value.hasOwnProperty("r")) {
              const finalValue = [];
              let index = 0;
              while (value.hasOwnProperty(index.toString())) {
                let item = value[index.toString()];
                if ((typeof item === "object" || typeof item === "function") && item !== null) {
                  item = convertJsonObjectToDmObject(item, result.extentUri, result.workspace);
                }
                finalValue.push(item);
                index++;
              }
              value = finalValue;
            } else {
              value = convertJsonObjectToDmObject(value, result.extentUri, result.workspace);
            }
          }
          const internalizedKey = DmObject.internalizeKey(key);
          const objectValue = new ObjectValue();
          objectValue.isSet = isPropertySet;
          if (isPropertySet === false) {
            objectValue.defaultValue = value;
          } else {
            objectValue.value = value;
          }
          result["values"][internalizedKey] = objectValue;
        }
      }
      result.clearSync();
    }
    const elementMetaClass = element["m"];
    if (elementMetaClass !== void 0 && elementMetaClass !== null) {
      result.metaClass = elementMetaClass;
    }
    const elementUri = element["u"];
    if (elementUri !== void 0 && elementUri !== null) {
      result.uri = elementUri;
    }
    const elementId = element["id"];
    if (elementId !== void 0 && elementId !== null) {
      result.id = elementId;
    }
    const elementReferenceUri = element["r"];
    if (elementReferenceUri !== void 0 && elementReferenceUri !== null) {
      result.uri = elementReferenceUri;
      result.isReference = true;
    }
    return result;
  }
  function getName(value) {
    if (value === null || value === void 0) {
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
    if (typeof value === "object" || typeof value === "function") {
      return value.get("name");
    }
    if (value.toString !== void 0) {
      return value.toString();
    }
    return value;
  }

  // ../DatenMeister.WebServer/Assets/js/burnsystems/Events.js
  function addKeyBindingEvent(button, bindingKey, bindingKeyModifierCtrl = false, bindingKeyModifierShift = false, bindingKeyModifierAlt = false) {
    const parts = [];
    if (bindingKeyModifierCtrl)
      parts.push("Ctrl");
    if (bindingKeyModifierShift)
      parts.push("Shift");
    if (bindingKeyModifierAlt)
      parts.push("Alt");
    parts.push(bindingKey);
    const shortcutLabel = "Shortcut: " + parts.join("+");
    const existingTitle = button.attr("title") ?? "";
    button.attr("title", existingTitle ? `${existingTitle} (${shortcutLabel})` : shortcutLabel);
    $(document).on("keydown", (e) => {
      const tag = e.target.tagName;
      const isEditable = tag === "INPUT" || tag === "TEXTAREA" || tag === "SELECT" || e.target.isContentEditable;
      if (isEditable)
        return;
      if (e.key === bindingKey && e.ctrlKey === bindingKeyModifierCtrl && e.shiftKey === bindingKeyModifierShift && e.altKey === bindingKeyModifierAlt) {
        e.preventDefault();
        button.trigger("click");
      }
    });
  }
  var UserEvent = class {
    constructor() {
      this.assigned = new Array();
    }
    // Adds a new listener to the event handler
    addListener(func) {
      const result = {
        func
      };
      this.assigned.push(result);
      return result;
    }
    // Removes a listener from the event handler
    removeListener(handle) {
      this.assigned = this.assigned.filter((x) => x !== handle);
    }
    // Calls the events
    invoke(data) {
      for (let n in this.assigned) {
        if (Object.prototype.hasOwnProperty.call(this.assigned, n)) {
          const handler = this.assigned[n];
          if (handler !== null && handler !== void 0) {
            handler.func(data);
          }
        }
      }
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/Settings.js
  var baseUrl = "/";
  var WorkspaceManagement = "Management";
  var WorkspaceData = "Data";
  var WorkspaceTypes = "Types";
  var UriExtentUserForm = "dm:///_internal/forms/user";

  // ../DatenMeister.WebServer/Assets/js/datenmeister/ApiConnection.js
  function serverError(x) {
    const error = $("<div></div>").text(x.toString());
    $("#server_errors").append(error);
  }
  function post(uri, data) {
    return new Promise((resolve2, reject) => {
      $.ajax({
        url: uri,
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json",
        method: "POST"
      }).fail((x) => {
        serverError(x.responseText);
        reject(x.responseText);
      }).done((x) => resolve2(x));
    });
  }
  function get(uri) {
    return new Promise((resolve2, reject) => {
      $.ajax({
        url: uri,
        method: "GET"
      }).fail((x) => {
        serverError(x.responseText);
        reject(x.responseText);
      }).done((x) => resolve2(x));
    });
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/client/Elements.js
  function getAllWorkspaces() {
    return load(void 0, void 0);
  }
  function getAllExtents(workspaceId) {
    return load(workspaceId, void 0);
  }
  function getAllRootItems(workspaceId, extentUri) {
    return load(workspaceId, extentUri);
  }
  function getAllChildItems(workspaceId, itemUrl) {
    return load(workspaceId, itemUrl);
  }
  async function load(workspaceId, itemUri) {
    let url = "/api/elements/get_composites?";
    if (workspaceId !== void 0 && workspaceId !== null) {
      url += "w=" + encodeURIComponent(workspaceId);
      if (itemUri !== void 0 && itemUri !== null) {
        url += "&u=" + encodeURIComponent(itemUri);
      }
    } else if (itemUri !== void 0 && itemUri !== null) {
      url += "u=" + encodeURIComponent(itemUri);
    }
    return await get(url);
  }
  async function loadNameByUri(workspaceId, elementUri) {
    if (workspaceId === void 0) {
      workspaceId = "_";
    }
    const getUrl = baseUrl + "api/elements/get_name_by_uri?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(elementUri);
    return await get(getUrl);
  }
  async function queryObject(query, parameters) {
    if (parameters === void 0) {
      parameters = {};
    }
    if (parameters.query === void 0 || parameters.query === null) {
      parameters.query = createJsonFromObject(query);
    }
    const resultFromServer = await post(baseUrl + "api/elements/query_object", parameters);
    const result = new Array();
    for (const n in resultFromServer.result) {
      const v = resultFromServer.result[n];
      const innerResult = convertJsonObjectToDmObject(v);
      if (innerResult !== void 0) {
        result.push(innerResult);
      }
    }
    return {
      result
    };
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/client/Items.js
  async function addToContainer(workspaceId, itemUri, param) {
    const evaluatedParameter = {
      metaClass: param.metaClass,
      properties: void 0
    };
    if (param.properties !== void 0 && param.properties !== null) {
      evaluatedParameter.properties = createJsonFromObject(param.properties);
    }
    return await post(baseUrl + "api/items/add_to_container?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(itemUri), evaluatedParameter);
  }
  async function getObjectByUri(workspace, url) {
    const resultFromServer = await get(baseUrl + "api/items/get?w=" + encodeURIComponent(workspace) + "&u=" + encodeURIComponent(url));
    const result = convertJsonObjectToDmObject(resultFromServer, void 0, workspace);
    if (result === void 0) {
      throw new Error("Object not found");
    }
    return result;
  }
  async function getItemWithNameAndId(workspace, url) {
    return await get(baseUrl + "api/items/get_itemwithnameandid?w=" + encodeURIComponent(workspace) + "&u=" + encodeURIComponent(url));
  }
  async function getContainer(workspaceId, itemUri, self) {
    let uri = baseUrl + "api/items/get_container?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(itemUri);
    if (self === true) {
      uri += "&self=true";
    }
    return await get(uri);
  }
  async function unsetProperty(workspaceId, itemUrl, property) {
    let url = baseUrl + "api/items/unset_property?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(itemUrl);
    return await post(url, { property });
  }
  async function setPropertiesByStringValues(workspaceId, itemUrl, params) {
    let url = baseUrl + "api/items/set_properties?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(itemUrl);
    return await post(url, params);
  }
  async function getProperty(workspaceId, itemUrl, property) {
    if (workspaceId === void 0) {
      throw new Error("Workspace is undefined");
    }
    if (itemUrl === void 0) {
      throw new Error("ItemUrl is undefined");
    }
    if (property === void 0) {
      throw new Error("Property is undefined");
    }
    let url = baseUrl + "api/items/get_property?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(itemUrl) + "&property=" + encodeURIComponent(property);
    const result = await get(url);
    return convertJsonObjectToObjects(result.v[1]);
  }
  async function setMetaclass(workspaceId, itemUrl, newMetaClass) {
    let url = baseUrl + "api/items/set_metaclass?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(itemUrl);
    return await post(url, { metaclass: newMetaClass });
  }
  async function addReferenceToCollection(workspaceId, itemUrl, parameter) {
    let url = baseUrl + "api/items/add_ref_to_collection?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(itemUrl);
    await post(url, {
      property: parameter.property,
      workspaceId: parameter.workspaceId,
      referenceUri: parameter.referenceUri
    });
  }
  async function setPropertyReference(workspaceId, itemUrl, parameter) {
    let url = baseUrl + "api/items/set_property_reference?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(itemUrl);
    return await post(url, {
      property: parameter.property,
      workspaceId: parameter.workspaceId,
      referenceUri: parameter.referenceUri
    });
  }
  async function removeReferenceFromCollection(workspaceId, itemUrl, parameter) {
    let url = baseUrl + "api/items/remove_ref_to_collection?w=" + encodeURIComponent(workspaceId) + "&u=" + encodeURIComponent(itemUrl);
    await post(url, {
      property: parameter.property,
      workspaceId: parameter.referenceWorkspaceId,
      referenceUri: parameter.referenceUri
    });
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/Navigator.js
  function getLinkForNavigateToExtentItems(workspace, extentUri, parameter) {
    if (workspace === void 0 || extentUri === void 0) {
      throw new Error("workspace or extentUri is undefined");
    }
    let urlParameter = "";
    let ampersand = "?";
    const asUrl = new URL(extentUri);
    asUrl.searchParams.delete("metaclass");
    extentUri = asUrl.href;
    if (parameter?.metaClass !== void 0) {
      urlParameter += ampersand + "metaclass=" + encodeURIComponent(parameter.metaClass);
      ampersand = "&";
    }
    return baseUrl + "ItemsOverview/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(extentUri + urlParameter);
  }
  function getLinkForNavigateToItemByUrl(workspace, itemUrl, param) {
    return baseUrl + "Item/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(itemUrl) + parseNavigateToItemParam(param);
  }
  function navigateToItemByUrl(workspace, itemUrl, param) {
    document.location.href = getLinkForNavigateToItemByUrl(workspace, itemUrl, param);
  }
  function parseNavigateToItemParam(param) {
    if (param === void 0) {
      return "";
    }
    let result = "";
    let ampersand = "?";
    if (param.editMode === true) {
      result += ampersand + "edit=true";
      ampersand = "&";
    }
    if (param.formUri !== void 0 && param.formUri !== null && param.formUri !== "") {
      result += ampersand + "formUri=" + encodeURIComponent(param.formUri);
      ampersand = "&";
    }
    return result;
  }
  function getLinkForNavigateToCreateNewItemInExtent(workspace, extentUri, metaclass, metaClassWorkspace) {
    return baseUrl + "ItemAction/Extent.CreateNewItem?workspace=" + encodeURIComponent(workspace) + "&item=" + encodeURIComponent(extentUri) + (metaclass !== void 0 ? "&metaclass=" + encodeURIComponent(metaclass) : "") + (metaClassWorkspace !== void 0 ? "&metaclassworkspace=" + encodeURIComponent(metaClassWorkspace) : "");
  }
  function getLinkForNavigateToCreateItemInProperty(workspace, itemUrl, propertyName, metaclass, metaclassWorkspace, isList) {
    return baseUrl + "ItemAction/Extent.CreateNewItem?workspace=" + encodeURIComponent(workspace) + "&item=" + encodeURIComponent(itemUrl) + "&property=" + encodeURIComponent(propertyName) + (metaclass !== void 0 ? "&metaclass=" + encodeURIComponent(metaclass) : "") + (metaclassWorkspace !== void 0 ? "&metaclassworkspace=" + encodeURIComponent(metaclassWorkspace) : "") + (isList !== void 0 ? isList ? "&islist=true" : "&islist=false" : "");
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/DomHelper.js
  async function injectNameByObject(domElement, element, parameter) {
    return injectNameByUri(domElement, element.workspace, element.uri ?? "", parameter);
  }
  async function injectNameByUri(domElement, workspaceId, elementUri, parameter) {
    try {
      const x = await loadNameByUri(workspaceId, elementUri);
      domElement.empty();
      domElement.append(convertItemWithNameAndIdToDom(x, parameter));
    } catch (error) {
      console.log(error);
      throw error;
    }
  }
  function debugElementToDom(mofElement, domSelector) {
    const domElement = typeof domSelector === "string" ? $(domSelector) : domSelector;
    domElement.empty();
    if (domElement.length > 0) {
      domElement.append(convertToDom(mofElement));
    }
  }
  function convertItemWithNameAndIdToDom(item, params) {
    let result = $("<span></span>");
    const validLinkInformation = item.uri !== void 0 && item.workspace !== void 0 && item.uri !== "" && item.workspace !== "";
    const inhibitLink = params !== void 0 && params.inhibitItemLink;
    const inhibitEditLink = params !== void 0 && params.inhibitEditItemLink;
    if (validLinkInformation) {
      if (!inhibitLink) {
        const linkElement = $("<a></a>");
        linkElement.text(item.name ?? "");
        if (params?.onClick !== void 0) {
          linkElement.attr("href", "#");
          linkElement.on("click", () => {
            if (params?.onClick !== void 0) {
              params.onClick(item);
            }
            return false;
          });
        } else {
          linkElement.attr("href", getLinkForNavigateToItemByUrl(item.workspace ?? "", item.uri));
          linkElement.on("click", () => {
            navigateToItemByUrl(item.workspace ?? "", item.uri);
          });
        }
        result.append(linkElement);
      } else {
        const linkElement = $("<span></span>");
        linkElement.text(item.name ?? "");
        result.append(linkElement);
      }
      if (!inhibitEditLink) {
        const linkElement = $("<a class='dm-item-edit-button'>\u2712\uFE0F</a>");
        linkElement.attr("href", getLinkForNavigateToItemByUrl(item.workspace ?? "", item.uri, { editMode: true }));
        linkElement.on("click", () => {
          navigateToItemByUrl(item.workspace ?? "", item.uri, { editMode: true });
          return false;
        });
        result.append(linkElement);
      }
    } else {
      result.text(item.name ?? "Unknown");
    }
    if (item.metaClassName !== void 0 && item.metaClassName !== null && item.metaClassUri !== void 0 && item.metaClassUri !== null) {
      const metaClassText = $("<span class='dm-metaclass'></span>");
      metaClassText.attr("title", item.metaClassUri).text(" [" + item.metaClassName + "]");
      result.append(metaClassText);
    }
    return result;
  }
  function convertToDom(mofElement, isReferenced) {
    if (Array.isArray(mofElement)) {
      let count = 0;
      const arrayList = $("<ol></ol>");
      for (let m in mofElement) {
        const li = $("<li></li>");
        if (count > 50) {
          li.text("... (total: " + mofElement.length + ")");
          arrayList.append(li);
          break;
        }
        li.append(convertToDom(mofElement[m], isReferenced));
        arrayList.append(li);
        count++;
      }
      return arrayList;
    } else if ((typeof mofElement === "object" || typeof mofElement === "function") && mofElement !== null) {
      const asElement = mofElement;
      const list = $("<ul></ul>");
      isReferenced ?? (isReferenced = asElement.isReference);
      if (asElement.metaClass !== void 0) {
        const name = asElement.metaClass.fullName ?? asElement.metaClass.uri;
        const row = $("<li><em></em></li>");
        $("em", row).attr("title", asElement.metaClass.uri).text("[[MetaClass: " + name + "]]");
        loadNameByUri(asElement.metaClass.workspace ?? "", asElement.metaClass.uri).then((x) => {
          $("em", row).text("[[MetaClass: " + x.name + "]]");
        });
        list.append(row);
      }
      if (asElement.id !== void 0) {
        const isReferenceText = isReferenced ? " [Reference]" : "";
        const row = $("<li><em></em></li>");
        $("em", row).attr("title", asElement.id).text("{{Id: " + asElement.id + "}}" + isReferenceText);
        list.append(row);
      }
      if (asElement.uri !== void 0) {
        const row = $("<li></li>");
        const span = $("<span></span>");
        span.html("<em>Uri</em>: ");
        span.append(convertToDom(asElement.uri));
        row.append(span);
        list.append(row);
      }
      if (isReferenced !== true && asElement.getPropertyValues !== void 0) {
        for (let n in asElement.getPropertyValues()) {
          const isReferenced2 = asElement.isReferenced(n);
          const value = asElement.get(n);
          const row = $("<li></li>");
          const span = $("<span></span>");
          span.text(`[${n}]: `);
          span.append(convertToDom(value, isReferenced2));
          row.append(span);
          list.append(row);
        }
      }
      return list;
    } else {
      const span = $("<span class='dm-debug-property-value'></span>");
      span.text(mofElement);
      return span;
    }
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/controls/SelectItemControlHelper.js
  var SelectItemControlHelperForWorkspaceAndExtent = class {
    constructor(control) {
      this.isDomInitialized = false;
      this.loadedWorkspaces = new Array();
      this.loadedExtents = new Array();
      this.control = control;
    }
    initialize(table) {
      const tthis = this;
      this.htmlWorkspaceDiv = $(".dm-sic-workspace", table);
      this.htmlExtentDiv = $(".dm-sic-extent", table);
      this.htmlWorkspaceSelect = $("<select></select>");
      this.htmlWorkspaceSelect.on("change", async () => await tthis.onWorkspaceChangedByUser());
      this.htmlWorkspaceDiv.append(this.htmlWorkspaceSelect);
      this.htmlExtentSelect = $("<select></select>");
      this.htmlExtentSelect.on("change", () => tthis.onExtentChangedByUser());
      this.htmlExtentDiv.append(this.htmlExtentSelect);
      this.isDomInitialized = true;
    }
    async onExtentChangedByUser() {
      await this.control.onExtentSelected(this.getUserSelectedWorkspaceId(), this.getUserSelectedExtentUri());
    }
    async onWorkspaceChangedByUser() {
      await this.loadExtents();
      await this.control.onWorkspaceSelected(this.getUserSelectedWorkspaceId());
    }
    /**
     * Loads the workspaces and adds them into the control element containing the parameter
     * @private
     */
    async loadWorkspaces() {
      if (this.isDomInitialized) {
        this.htmlWorkspaceSelect.empty();
        let currentlySelectedWorkspace = this.getUserSelectedWorkspaceId();
        if (this.preSelectWorkspaceById !== void 0) {
          currentlySelectedWorkspace = this.preSelectWorkspaceById;
          this.preSelectWorkspaceById = void 0;
        }
        const items = await getAllWorkspaces();
        this.loadedWorkspaces = items;
        const none = $("<option value=''>--- None ---</option>");
        this.htmlWorkspaceSelect.append(none);
        let found = false;
        for (const n in items) {
          if (!items.hasOwnProperty(n))
            continue;
          const item = items[n];
          if (item.id === void 0 || item.name === void 0)
            continue;
          const option = $("<option></option>");
          option.val(item.id);
          option.text(item.name);
          if (item.id === currentlySelectedWorkspace) {
            found = true;
          }
          this.htmlWorkspaceSelect.append(option);
        }
        if (currentlySelectedWorkspace !== void 0 && found) {
          this.htmlWorkspaceSelect.val(currentlySelectedWorkspace);
          await this.control.onWorkspaceSelected(currentlySelectedWorkspace);
        }
        await this.loadExtents();
      }
    }
    /**
     * Loads the extents for the currently selected workspace and (re)populates
     * the extent dropdown. Honors a pending {@link preSelectExtentByUri} value,
     * consuming it in the process. After the dropdown has been refreshed,
     * {@link loadItems} is called so that the child list and breadcrumb stay
     * in sync.
     *
     * @returns A promise that resolves to `true` when the GUI is updated.
     */
    async loadExtents() {
      if (this.isDomInitialized) {
        const tthis = this;
        const workspaceId = this.getUserSelectedWorkspaceId();
        if (workspaceId === void 0) {
          return;
        }
        this.textOfSelectedItem = workspaceId;
        let extentUri = this.getUserSelectedExtentUri();
        if (this.preSelectExtentByUri !== void 0) {
          extentUri = this.preSelectExtentByUri;
          this.preSelectExtentByUri = void 0;
        }
        this.htmlExtentSelect.empty();
        if (workspaceId === "") {
          const select = $("<option value=''>--- Select Workspace ---</option>");
          this.htmlExtentSelect.append(select);
        } else {
          const items = await getAllExtents(workspaceId);
          this.htmlExtentSelect.empty();
          const none = $("<option value=''>--- None ---</option>");
          tthis.htmlExtentSelect.append(none);
          tthis.loadedExtents = items;
          let found = false;
          for (const n in items) {
            if (!items.hasOwnProperty(n))
              continue;
            const item = items[n];
            if (item.extentUri === void 0 || item.name === void 0)
              continue;
            const option = $("<option></option>");
            option.val(item.extentUri);
            option.text(item.name);
            if (item.extentUri === extentUri) {
              found = true;
            }
            tthis.htmlExtentSelect.append(option);
          }
          if (extentUri !== void 0 && found) {
            this.htmlExtentSelect.val(extentUri);
            this.textOfSelectedItem = extentUri;
            await this.control.onExtentSelected(workspaceId, extentUri);
          } else {
            this.htmlExtentSelect.val("");
          }
        }
        return true;
      }
    }
    /**
     * Returns the workspace id currently selected in the dropdown, or the
     * empty string if no workspace is selected. This reflects the *DOM* state
     * — pending pre-selections that have not been applied yet are not visible
     * here.
     */
    getUserSelectedWorkspaceId() {
      if (!this.isDomInitialized)
        return void 0;
      return this.htmlWorkspaceSelect.val()?.toString() ?? void 0;
    }
    /**
     * Returns the extent URI currently selected in the dropdown, or the empty
     * string if no extent is selected. Reflects the DOM state only; see
     * {@link getUserSelectedWorkspaceId} for the same issue.
     */
    getUserSelectedExtentUri() {
      if (!this.isDomInitialized)
        return void 0;
      const extent = this.htmlExtentSelect.val()?.toString() ?? "";
      return extent === "" ? void 0 : extent;
    }
    async setExtentByUri(workspaceId, extentUri) {
      this.preSelectWorkspaceById = workspaceId;
      this.preSelectExtentByUri = extentUri;
      await this.loadWorkspaces();
      await this.loadExtents();
    }
    async setWorkspaceById(workspaceId) {
      this.preSelectWorkspaceById = workspaceId;
      this.preSelectExtentByUri = void 0;
      await this.loadWorkspaces();
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/controls/SelectItemControlByBrowsingControl.js
  var ControlSettings = class {
    constructor() {
      this.showBreadcrumb = true;
      this.showWorkspaceInBreadcrumb = false;
      this.showExtentInBreadcrumb = false;
    }
  };
  var SelectItemControlByBrowsingControl = class {
    constructor() {
      this.isDomInitialized = false;
      this.itemClicked = new UserEvent();
      this.workspaceAndExtent = new SelectItemControlHelperForWorkspaceAndExtent(this);
    }
    /**
     * Renders the control into `parent` and kicks off the asynchronous load of
     * the workspace list. Returns synchronously as soon as the DOM is in
     * place; the workspace dropdown is populated in the background.
     *
     * Use {@link initAsync} if you need to await the initial data load.
     *
     * @param parent The container element that should host the control.
     * @param settings Optional behavior overrides; see {@link ControlSettings}.
     * @returns The root `<table>` of the rendered control.
     */
    init(parent, settings) {
      const result = this.initDom(settings ?? new ControlSettings(), parent);
      const _ = this.workspaceAndExtent.loadWorkspaces();
      return result;
    }
    /**
     * Async variant of {@link init}. Renders the control into `parent` and
     * resolves the returned promise once the workspace list (and any
     * pre-selected extent/item) has been loaded and the GUI reflects it.
     *
     * @param parent The container element that should host the control.
     * @param settings Optional behavior overrides; see {@link ControlSettings}.
     * @returns A promise resolving to the root `<table>` of the rendered control.
     */
    async initAsync(parent, settings) {
      const result = this.initDom(settings ?? new ControlSettings(), parent);
      await this.workspaceAndExtent.loadWorkspaces();
      return result;
    }
    /**
     * This method just creates the DOM and connects the events of the elements to the
     * invocation methods
     * @param settings Settings to be used
     * @param container JQuery-Container Element hosting the content
     * @private
     */
    initDom(settings, container) {
      this.settings = settings ?? new ControlSettings();
      this.htmlSelectedElements = $("<div></div>");
      this.htmlItemsList = $("<ul></ul>");
      const div = $("<table class='dm-sic-bb'><tr><th>Workspace: </th><td class='dm-sic-workspace'></td></tr><tr><th>Extent: </th><td class='dm-sic-extent'></td></tr><tr><th>Selected Item: </th><td><div class='dm-sic-bb-selected'></div></td></tr><tr><th>Children: </th><td><div class='dm-breadcrumb'><nav aria-label='breadcrumb'><ul class='breadcrumb'></ul></nav></div><div class='dm-sic-bb-items'></div></td></tr></table>");
      this.workspaceAndExtent.initialize(div);
      $(".dm-sic-bb-items", div).append(this.htmlItemsList);
      $(".dm-sic-bb-selected", div).append(this.htmlSelectedElements);
      this.htmlBreadcrumbList = $(".breadcrumb", div);
      container.append(div);
      this.containerDiv = div;
      this.isDomInitialized = true;
      this.renderEmptySelectedItem();
      return div;
    }
    showControl() {
      this.containerDiv.show();
    }
    hideControl() {
      this.containerDiv.hide();
    }
    async onWorkspaceSelected(workspaceId) {
      this.selectedItem = void 0;
      this.renderEmptySelectedItem();
    }
    async onExtentSelected(workspaceId, extentUri) {
      this.selectedItem = {
        workspace: workspaceId,
        extentUri,
        ententType: EntentType.Extent,
        uri: extentUri
      };
      await this.loadItems();
    }
    /**
     * Gets the selected item
     */
    getSelectedItem() {
      return this.selectedItem;
    }
    /**
     * This call may also be given before the workspaces has been loaded
     * The promise is resolved when the GUI is updated
     *
     * @param workspaceId ID of the workspace which is preselected.
     */
    async setWorkspaceById(workspaceId) {
      await this.workspaceAndExtent.setWorkspaceById(workspaceId);
      this.selectedItem = void 0;
      this.renderEmptySelectedItem();
    }
    /**
     * This call may also be given before the extents has been loaded
     * The promise is resolved when the GUI is updated
     */
    async setExtentByUri(workspaceId, extentUri) {
      this.selectedItem = {
        workspace: workspaceId,
        extentUri,
        ententType: EntentType.Extent,
        uri: extentUri
      };
      await this.workspaceAndExtent.setExtentByUri(workspaceId, extentUri);
      await this.loadItems();
    }
    /**
     * Pre-selects a specific item by resolving the corresponding workspace and
     * extent from the server, then queueing all three values for the next
     * load. Safe to call before the GUI is initialized.
     *
     * @param workspaceId ID of the workspace containing the item.
     * @param itemUri URI of the item to pre-select.
     * @throws A string error message when no item can be resolved for the given
     *         workspace/URI pair.
     */
    async setItemByUri(workspaceId, itemUri) {
      const item = await loadNameByUri(workspaceId, itemUri);
      if (item === void 0) {
        throw "Item: " + workspaceId + ":" + itemUri + " has not been found";
      }
      await this.workspaceAndExtent.setExtentByUri(workspaceId, item.extentUri);
      this.preSelectItemUri = item.uri;
      if (this.isDomInitialized) {
        await this.loadItems();
      }
    }
    /**
     * Returns the workspace id currently selected in the dropdown, or the
     * empty string if no workspace is selected. This reflects the *DOM* state
     * — pending pre-selections that have not been applied yet are not visible
     * here.
     */
    getUserSelectedWorkspaceId() {
      return this.workspaceAndExtent.getUserSelectedWorkspaceId();
    }
    /**
     * Returns the extent URI currently selected in the dropdown, or the empty
     * string if no extent is selected. Reflects the DOM state only; see
     * {@link getUserSelectedWorkspaceId} for the same caveat.
     */
    getUserSelectedExtentUri() {
      return this.workspaceAndExtent.getUserSelectedExtentUri();
    }
    /**
     * Refreshes the children list and the "Selected Item" display so that
     * they describe the currently selected item.
     *
     * Each rendered `<li>` becomes clickable; a click updates the selection,
     * fires {@link itemClicked} and recursively reloads the list.
     */
    async loadItems() {
      if (this.isDomInitialized) {
        const tthis = this;
        let selectedItem = this.selectedItem;
        if (this.preSelectItemUri !== void 0) {
          const selectedWorkspace = this.getUserSelectedWorkspaceId();
          if (this.preSelectItemUri === "" || selectedWorkspace === void 0) {
            selectedItem = void 0;
          } else {
            selectedItem = {
              uri: this.preSelectItemUri,
              workspace: selectedWorkspace
            };
          }
          this.selectedItem = selectedItem;
          this.preSelectItemUri = void 0;
        }
        const workspaceId = this.getUserSelectedWorkspaceId();
        const extentUri = this.getUserSelectedExtentUri();
        this.htmlItemsList.empty();
        if (workspaceId === "" || workspaceId === void 0 || extentUri === "" || extentUri === void 0 || selectedItem === void 0) {
          this.renderEmptySelectedItem();
        } else {
          const item = await getItemWithNameAndId(selectedItem.workspace, selectedItem.uri);
          if (item !== void 0) {
            this.htmlSelectedElements.empty();
            this.htmlSelectedElements.append(convertItemWithNameAndIdToDom(item));
          }
          const funcElements = (items) => {
            for (const n in items) {
              if (!items.hasOwnProperty(n))
                continue;
              const item2 = items[n];
              const option = $("<li class='dm-sic-bb-item'></li>");
              option.append(convertItemWithNameAndIdToDom(item2, {
                inhibitItemLink: true,
                inhibitEditItemLink: true
              }));
              ((innerItem) => option.on("click", async () => {
                tthis.selectedItem = innerItem;
                tthis.itemClicked.invoke(innerItem);
                await tthis.loadItems();
                tthis.htmlSelectedElements.empty();
                tthis.htmlSelectedElements.append(convertItemWithNameAndIdToDom(item2));
                await tthis.refreshBreadcrumb();
              }))(item2);
              tthis.htmlItemsList.append(option);
            }
            return true;
          };
          if (selectedItem.ententType === EntentType.Extent) {
            tthis.htmlSelectedElements.text(extentUri);
            const rootElements = await getAllRootItems(workspaceId, extentUri);
            funcElements(rootElements);
          } else {
            const childElements = await getAllChildItems(workspaceId, selectedItem.uri);
            funcElements(childElements);
          }
        }
        await tthis.refreshBreadcrumb();
      }
    }
    /**
     * Renders an empty selected item in the UI by appending default elements
     * to the appropriate DOM containers.
     *
     * This method first checks if the DOM is initialized. If true, it adds a
     * placeholder list item to the htmlItemsList and resets the htmlSelectedElements
     * container with a default "None" element.
     *
     * @return {void} This method does not return a value.
     */
    renderEmptySelectedItem() {
      if (this.isDomInitialized === true) {
        const select = $("<li>--- Select Extent ---</li>");
        this.htmlItemsList.empty();
        this.htmlItemsList.append(select);
        this.htmlSelectedElements.empty();
        this.htmlSelectedElements.append($("<em>None</em>"));
      }
    }
    /**
     * Rebuilds the breadcrumb to reflect the current selection.
     */
    async refreshBreadcrumb() {
      const tthis = this;
      const currentWorkspace = this.getUserSelectedWorkspaceId();
      const currentExtent = this.getUserSelectedExtentUri();
      let containerItems = [];
      if (this.selectedItem !== void 0 && this.selectedItem.uri !== void 0 && currentWorkspace !== void 0) {
        containerItems = await getContainer(currentWorkspace, this.selectedItem.uri, true);
      }
      this.htmlBreadcrumbList.empty();
      if (this.settings.showBreadcrumb) {
        if (currentWorkspace === void 0) {
          this.addBreadcrumbItem("No Workspace selected", async () => {
          });
          return;
        }
        if (this.settings.showWorkspaceInBreadcrumb) {
          this.addBreadcrumbItem("Workspaces", async () => {
            await this.workspaceAndExtent.setWorkspaceById(void 0);
            this.selectedItem = void 0;
          });
          if (currentWorkspace !== "" && currentWorkspace !== void 0) {
            this.addBreadcrumbItem(currentWorkspace, async () => {
              await this.workspaceAndExtent.setWorkspaceById(currentWorkspace);
              this.selectedItem = void 0;
            });
          }
        }
        if (this.settings.showExtentInBreadcrumb) {
          if (currentExtent !== "" && currentExtent !== void 0) {
            this.addBreadcrumbItem(currentExtent, async () => {
              await this.workspaceAndExtent.setExtentByUri(this.getUserSelectedExtentUri(), currentExtent);
              this.selectedItem = {
                workspace: currentWorkspace,
                extentUri: currentExtent,
                ententType: EntentType.Extent,
                uri: currentExtent
              };
            });
          }
        }
        if (containerItems !== void 0) {
          for (let n = 0; n < containerItems.length; n++) {
            const item = containerItems[containerItems.length - 1 - n];
            if (item.ententType !== EntentType.Item) {
              continue;
            }
            ((innerItem) => {
              this.addBreadcrumbItem(item.name ?? "Unknown Name", async () => {
                this.selectedItem = innerItem;
                await tthis.loadItems();
              });
            })(item);
          }
        }
      }
    }
    /**
     * Appends a single clickable entry to the breadcrumb list.
     *
     * After invoking `onClick`, the breadcrumb is refreshed so that crumbs
     * "below" the clicked one are removed and the rest of the GUI follows
     * the new selection.
     *
     * @param text Label of the breadcrumb entry.
     * @param onClick Handler invoked when the user clicks the entry.
     */
    addBreadcrumbItem(text, onClick) {
      const tthis = this;
      const breadcrumbItem = $("<li class='breadcrumb-item active'></li>");
      breadcrumbItem.text(text);
      breadcrumbItem.on("click", async () => {
        onClick();
        await tthis.refreshBreadcrumb();
      });
      this.htmlBreadcrumbList.append(breadcrumbItem);
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/models/DatenMeister.class.js
  var _CommonTypes;
  (function(_CommonTypes2) {
    class _DateTime {
    }
    _CommonTypes2._DateTime = _DateTime;
    _CommonTypes2.__DateTime_Uri = "dm:///_internal/types/internal#DateTime";
    let _Default;
    (function(_Default2) {
      class _Package {
      }
      _Package._name_ = "name";
      _Package.packagedElement = "packagedElement";
      _Package.preferredType = "preferredType";
      _Package.preferredPackage = "preferredPackage";
      _Package.defaultViewMode = "defaultViewMode";
      _Default2._Package = _Package;
      _Default2.__Package_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package";
      class _XmiExportContainer {
      }
      _XmiExportContainer.xmi = "xmi";
      _Default2._XmiExportContainer = _XmiExportContainer;
      _Default2.__XmiExportContainer_Uri = "dm:///_internal/types/internal#1c21ea5b-a9ce-4793-b2f9-590ab2c4e4f1";
      class _XmiImportContainer {
      }
      _XmiImportContainer.xmi = "xmi";
      _XmiImportContainer.property = "property";
      _XmiImportContainer.addToCollection = "addToCollection";
      _Default2._XmiImportContainer = _XmiImportContainer;
      _Default2.__XmiImportContainer_Uri = "dm:///_internal/types/internal#73c8c24e-6040-4700-b11d-c60f2379523a";
    })(_Default = _CommonTypes2._Default || (_CommonTypes2._Default = {}));
    let _ExtentManager;
    (function(_ExtentManager2) {
      class _ImportSettings {
      }
      _ImportSettings.filePath = "filePath";
      _ImportSettings.extentUri = "extentUri";
      _ImportSettings.workspaceId = "workspaceId";
      _ExtentManager2._ImportSettings = _ImportSettings;
      _ExtentManager2.__ImportSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings";
      class _ImportException {
      }
      _ImportException.message = "message";
      _ExtentManager2._ImportException = _ImportException;
      _ExtentManager2.__ImportException_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException";
    })(_ExtentManager = _CommonTypes2._ExtentManager || (_CommonTypes2._ExtentManager = {}));
    let _OSIntegration;
    (function(_OSIntegration2) {
      class _CommandLineApplication {
      }
      _CommandLineApplication._name_ = "name";
      _CommandLineApplication.applicationPath = "applicationPath";
      _OSIntegration2._CommandLineApplication = _CommandLineApplication;
      _OSIntegration2.__CommandLineApplication_Uri = "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication";
      class _EnvironmentalVariable {
      }
      _EnvironmentalVariable._name_ = "name";
      _EnvironmentalVariable.value = "value";
      _OSIntegration2._EnvironmentalVariable = _EnvironmentalVariable;
      _OSIntegration2.__EnvironmentalVariable_Uri = "dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable";
    })(_OSIntegration = _CommonTypes2._OSIntegration || (_CommonTypes2._OSIntegration = {}));
  })(_CommonTypes || (_CommonTypes = {}));
  var _Actions;
  (function(_Actions2) {
    class _Action {
    }
    _Action._name_ = "name";
    _Action.isDisabled = "isDisabled";
    _Actions2._Action = _Action;
    _Actions2.__Action_Uri = "dm:///_internal/types/internal#Actions.Action";
    class _ActionSet {
    }
    _ActionSet.action = "action";
    _ActionSet._name_ = "name";
    _ActionSet.isDisabled = "isDisabled";
    _Actions2._ActionSet = _ActionSet;
    _Actions2.__ActionSet_Uri = "dm:///_internal/types/internal#Actions.ActionSet";
    class _ActionResult {
    }
    _ActionResult.isSuccess = "isSuccess";
    _ActionResult.clientActions = "clientActions";
    _Actions2._ActionResult = _ActionResult;
    _Actions2.__ActionResult_Uri = "dm:///_internal/types/internal#899324b1-85dc-40a1-ba95-dec50509040d";
    class _LoggingWriterAction {
    }
    _LoggingWriterAction.message = "message";
    _LoggingWriterAction._name_ = "name";
    _LoggingWriterAction.isDisabled = "isDisabled";
    _Actions2._LoggingWriterAction = _LoggingWriterAction;
    _Actions2.__LoggingWriterAction_Uri = "dm:///_internal/types/internal#Actions.LoggingWriterAction";
    class _LoadExtentAction {
    }
    _LoadExtentAction.configuration = "configuration";
    _LoadExtentAction.dropExisting = "dropExisting";
    _LoadExtentAction._name_ = "name";
    _LoadExtentAction.isDisabled = "isDisabled";
    _Actions2._LoadExtentAction = _LoadExtentAction;
    _Actions2.__LoadExtentAction_Uri = "dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee";
    class _DropExtentAction {
    }
    _DropExtentAction.workspaceId = "workspaceId";
    _DropExtentAction.extentUri = "extentUri";
    _DropExtentAction._name_ = "name";
    _DropExtentAction.isDisabled = "isDisabled";
    _Actions2._DropExtentAction = _DropExtentAction;
    _Actions2.__DropExtentAction_Uri = "dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09";
    class _CreateWorkspaceAction {
    }
    _CreateWorkspaceAction.workspaceId = "workspaceId";
    _CreateWorkspaceAction.annotation = "annotation";
    _CreateWorkspaceAction._name_ = "name";
    _CreateWorkspaceAction.isDisabled = "isDisabled";
    _Actions2._CreateWorkspaceAction = _CreateWorkspaceAction;
    _Actions2.__CreateWorkspaceAction_Uri = "dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe";
    class _DropWorkspaceAction {
    }
    _DropWorkspaceAction.workspaceId = "workspaceId";
    _DropWorkspaceAction._name_ = "name";
    _DropWorkspaceAction.isDisabled = "isDisabled";
    _Actions2._DropWorkspaceAction = _DropWorkspaceAction;
    _Actions2.__DropWorkspaceAction_Uri = "dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8";
    class _CopyElementsAction {
    }
    _CopyElementsAction.sourcePath = "sourcePath";
    _CopyElementsAction.targetPath = "targetPath";
    _CopyElementsAction.moveOnly = "moveOnly";
    _CopyElementsAction.sourceWorkspace = "sourceWorkspace";
    _CopyElementsAction.targetWorkspace = "targetWorkspace";
    _CopyElementsAction.emptyTarget = "emptyTarget";
    _CopyElementsAction._name_ = "name";
    _CopyElementsAction.isDisabled = "isDisabled";
    _Actions2._CopyElementsAction = _CopyElementsAction;
    _Actions2.__CopyElementsAction_Uri = "dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed";
    class _ExportToXmiAction {
    }
    _ExportToXmiAction.sourcePath = "sourcePath";
    _ExportToXmiAction.filePath = "filePath";
    _ExportToXmiAction.sourceWorkspaceId = "sourceWorkspaceId";
    _ExportToXmiAction._name_ = "name";
    _ExportToXmiAction.isDisabled = "isDisabled";
    _Actions2._ExportToXmiAction = _ExportToXmiAction;
    _Actions2.__ExportToXmiAction_Uri = "dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863";
    class _ClearCollectionAction {
    }
    _ClearCollectionAction.workspaceId = "workspaceId";
    _ClearCollectionAction.path = "path";
    _ClearCollectionAction._name_ = "name";
    _ClearCollectionAction.isDisabled = "isDisabled";
    _Actions2._ClearCollectionAction = _ClearCollectionAction;
    _Actions2.__ClearCollectionAction_Uri = "dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae";
    class _TransformItemsAction {
    }
    _TransformItemsAction.metaClass = "metaClass";
    _TransformItemsAction.runtimeClass = "runtimeClass";
    _TransformItemsAction.workspaceId = "workspaceId";
    _TransformItemsAction.path = "path";
    _TransformItemsAction.excludeDescendents = "excludeDescendents";
    _TransformItemsAction._name_ = "name";
    _TransformItemsAction.isDisabled = "isDisabled";
    _Actions2._TransformItemsAction = _TransformItemsAction;
    _Actions2.__TransformItemsAction_Uri = "dm:///_internal/types/internal#Actions.ItemTransformationActionHandler";
    class _EchoAction {
    }
    _EchoAction.shallSuccess = "shallSuccess";
    _EchoAction._name_ = "name";
    _EchoAction.isDisabled = "isDisabled";
    _Actions2._EchoAction = _EchoAction;
    _Actions2.__EchoAction_Uri = "dm:///_internal/types/internal#DatenMeister.Actions.EchoAction";
    class _DocumentOpenAction {
    }
    _DocumentOpenAction.filePath = "filePath";
    _DocumentOpenAction._name_ = "name";
    _DocumentOpenAction.isDisabled = "isDisabled";
    _Actions2._DocumentOpenAction = _DocumentOpenAction;
    _Actions2.__DocumentOpenAction_Uri = "dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74";
    let _Reports2;
    (function(_Reports3) {
      class _SimpleReportAction {
      }
      _SimpleReportAction.path = "path";
      _SimpleReportAction.configuration = "configuration";
      _SimpleReportAction.workspaceId = "workspaceId";
      _SimpleReportAction.filePath = "filePath";
      _SimpleReportAction._name_ = "name";
      _SimpleReportAction.isDisabled = "isDisabled";
      _Reports3._SimpleReportAction = _SimpleReportAction;
      _Reports3.__SimpleReportAction_Uri = "dm:///_internal/types/internal#Actions.SimpleReportAction";
      class _AdocReportAction {
      }
      _AdocReportAction.filePath = "filePath";
      _AdocReportAction.reportInstance = "reportInstance";
      _AdocReportAction._name_ = "name";
      _AdocReportAction.isDisabled = "isDisabled";
      _Reports3._AdocReportAction = _AdocReportAction;
      _Reports3.__AdocReportAction_Uri = "dm:///_internal/types/internal#Actions.AdocReportAction";
      class _HtmlReportAction {
      }
      _HtmlReportAction.filePath = "filePath";
      _HtmlReportAction.reportInstance = "reportInstance";
      _HtmlReportAction._name_ = "name";
      _HtmlReportAction.isDisabled = "isDisabled";
      _Reports3._HtmlReportAction = _HtmlReportAction;
      _Reports3.__HtmlReportAction_Uri = "dm:///_internal/types/internal#Actions.HtmlReportAction";
    })(_Reports2 = _Actions2._Reports || (_Actions2._Reports = {}));
    class _MoveOrCopyAction {
    }
    _MoveOrCopyAction.copyMode = "copyMode";
    _MoveOrCopyAction.target = "target";
    _MoveOrCopyAction.source = "source";
    _Actions2._MoveOrCopyAction = _MoveOrCopyAction;
    _Actions2.__MoveOrCopyAction_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.MoveOrCopyAction";
    let _MoveOrCopyType;
    (function(_MoveOrCopyType2) {
      _MoveOrCopyType2.Copy = "Copy";
      _MoveOrCopyType2.Move = "Move";
    })(_MoveOrCopyType = _Actions2._MoveOrCopyType || (_Actions2._MoveOrCopyType = {}));
    let ___MoveOrCopyType;
    (function(___MoveOrCopyType2) {
      ___MoveOrCopyType2[___MoveOrCopyType2["Copy"] = 0] = "Copy";
      ___MoveOrCopyType2[___MoveOrCopyType2["Move"] = 1] = "Move";
    })(___MoveOrCopyType = _Actions2.___MoveOrCopyType || (_Actions2.___MoveOrCopyType = {}));
    let _MoveDirectionType2;
    (function(_MoveDirectionType3) {
      _MoveDirectionType3.Up = "Up";
      _MoveDirectionType3.Down = "Down";
    })(_MoveDirectionType2 = _Actions2._MoveDirectionType || (_Actions2._MoveDirectionType = {}));
    let ___MoveDirectionType;
    (function(___MoveDirectionType2) {
      ___MoveDirectionType2[___MoveDirectionType2["Up"] = 0] = "Up";
      ___MoveDirectionType2[___MoveDirectionType2["Down"] = 1] = "Down";
    })(___MoveDirectionType = _Actions2.___MoveDirectionType || (_Actions2.___MoveDirectionType = {}));
    class _MoveUpDownAction2 {
    }
    _MoveUpDownAction2.element = "element";
    _MoveUpDownAction2.direction = "direction";
    _MoveUpDownAction2.container = "container";
    _MoveUpDownAction2.property = "property";
    _Actions2._MoveUpDownAction = _MoveUpDownAction2;
    _Actions2.__MoveUpDownAction_Uri = "dm:///_internal/types/internal#bc4952bf-a3f5-4516-be26-5b773e38bd54";
    class _StoreExtentAction {
    }
    _StoreExtentAction.workspaceId = "workspaceId";
    _StoreExtentAction.extentUri = "extentUri";
    _StoreExtentAction._name_ = "name";
    _StoreExtentAction.isDisabled = "isDisabled";
    _Actions2._StoreExtentAction = _StoreExtentAction;
    _Actions2.__StoreExtentAction_Uri = "dm:///_internal/types/internal#43b0764e-b70f-42bb-b37d-ae8586ec45f1";
    class _ImportXmiAction {
    }
    _ImportXmiAction.workspaceId = "workspaceId";
    _ImportXmiAction.itemUri = "itemUri";
    _ImportXmiAction.xmi = "xmi";
    _ImportXmiAction.property = "property";
    _ImportXmiAction.addToCollection = "addToCollection";
    _Actions2._ImportXmiAction = _ImportXmiAction;
    _Actions2.__ImportXmiAction_Uri = "dm:///_internal/types/internal#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d";
    class _DeletePropertyFromCollectionAction {
    }
    _DeletePropertyFromCollectionAction.propertyName = "propertyName";
    _DeletePropertyFromCollectionAction.metaclass = "metaclass";
    _DeletePropertyFromCollectionAction.collectionUrl = "collectionUrl";
    _Actions2._DeletePropertyFromCollectionAction = _DeletePropertyFromCollectionAction;
    _Actions2.__DeletePropertyFromCollectionAction_Uri = "dm:///_internal/types/internal#b631bb00-ab11-4a8a-a148-e28abc398503";
    class _TargetReferenceResult {
    }
    _TargetReferenceResult.targetUrl = "targetUrl";
    _TargetReferenceResult.targetWorkspace = "targetWorkspace";
    _Actions2._TargetReferenceResult = _TargetReferenceResult;
    _Actions2.__TargetReferenceResult_Uri = "dm:///_internal/types/internal#3223e13a-bbb7-4785-8b81-7275be23b0a1";
    let _ParameterTypes;
    (function(_ParameterTypes2) {
      class _NavigationDefineActionParameter {
      }
      _NavigationDefineActionParameter.actionName = "actionName";
      _NavigationDefineActionParameter.formUrl = "formUrl";
      _NavigationDefineActionParameter.metaClassUrl = "metaClassUrl";
      _ParameterTypes2._NavigationDefineActionParameter = _NavigationDefineActionParameter;
      _ParameterTypes2.__NavigationDefineActionParameter_Uri = "dm:///_internal/types/internal#90f61e4e-a5ea-42eb-9caa-912d010fbccd";
      class _LoadExtentActionResult {
      }
      _LoadExtentActionResult.workspaceId = "workspaceId";
      _LoadExtentActionResult.extentUri = "extentUri";
      _ParameterTypes2._LoadExtentActionResult = _LoadExtentActionResult;
      _ParameterTypes2.__LoadExtentActionResult_Uri = "dm:///_internal/types/internal#2863f928-fe69-4d35-8c67-f4f3533b7ae5";
    })(_ParameterTypes = _Actions2._ParameterTypes || (_Actions2._ParameterTypes = {}));
    let _ClientActions;
    (function(_ClientActions2) {
      class _ClientAction {
      }
      _ClientAction.actionName = "actionName";
      _ClientAction.element = "element";
      _ClientAction.parameter = "parameter";
      _ClientAction._name_ = "name";
      _ClientActions2._ClientAction = _ClientAction;
      _ClientActions2.__ClientAction_Uri = "dm:///_internal/types/internal#e07ca80e-2540-4f91-8214-60dbd464e998";
      class _AlertClientAction {
      }
      _AlertClientAction.messageText = "messageText";
      _AlertClientAction.actionName = "actionName";
      _AlertClientAction.element = "element";
      _AlertClientAction.parameter = "parameter";
      _AlertClientAction._name_ = "name";
      _ClientActions2._AlertClientAction = _AlertClientAction;
      _ClientActions2.__AlertClientAction_Uri = "dm:///_internal/types/internal#0ee17f2a-5407-4d38-b1b4-34ead2186971";
      class _NavigateToExtentClientAction {
      }
      _NavigateToExtentClientAction.workspaceId = "workspaceId";
      _NavigateToExtentClientAction.extentUri = "extentUri";
      _ClientActions2._NavigateToExtentClientAction = _NavigateToExtentClientAction;
      _ClientActions2.__NavigateToExtentClientAction_Uri = "dm:///_internal/types/internal#3251783f-2683-4c24-bad5-828930028462";
      class _NavigateToItemClientAction {
      }
      _NavigateToItemClientAction.workspaceId = "workspaceId";
      _NavigateToItemClientAction.itemUrl = "itemUrl";
      _NavigateToItemClientAction.formUri = "formUri";
      _ClientActions2._NavigateToItemClientAction = _NavigateToItemClientAction;
      _ClientActions2.__NavigateToItemClientAction_Uri = "dm:///_internal/types/internal#5f69675e-df58-4ad7-84bf-359cdfba5db4";
      class _NavigateToUrlClientAction {
      }
      _NavigateToUrlClientAction.url = "url";
      _ClientActions2._NavigateToUrlClientAction = _NavigateToUrlClientAction;
      _ClientActions2.__NavigateToUrlClientAction_Uri = "dm:///_internal/types/internal#f1b5a13d-14fa-4ae8-8850-0feddf7ba0e5";
      class _NavigateOpenWindow {
      }
      _NavigateOpenWindow.url = "url";
      _NavigateOpenWindow.isAbsoluteUrl = "isAbsoluteUrl";
      _NavigateOpenWindow.actionName = "actionName";
      _NavigateOpenWindow.element = "element";
      _NavigateOpenWindow.parameter = "parameter";
      _NavigateOpenWindow._name_ = "name";
      _ClientActions2._NavigateOpenWindow = _NavigateOpenWindow;
      _ClientActions2.__NavigateOpenWindow_Uri = "dm:///_internal/types/internal#746ecf7c-63d6-4fdf-95ae-837a13a032ed";
      class _NavigateOpenActionInWindow {
      }
      _NavigateOpenActionInWindow.context = "context";
      _NavigateOpenActionInWindow.actionUrl = "actionUrl";
      _NavigateOpenActionInWindow.workspaceId = "workspaceId";
      _NavigateOpenActionInWindow.actionName = "actionName";
      _NavigateOpenActionInWindow.element = "element";
      _NavigateOpenActionInWindow.parameter = "parameter";
      _NavigateOpenActionInWindow._name_ = "name";
      _ClientActions2._NavigateOpenActionInWindow = _NavigateOpenActionInWindow;
      _ClientActions2.__NavigateOpenActionInWindow_Uri = "dm:///_internal/types/internal#77ff9c01-0651-4657-823a-ce41bbe0e5e5";
      class _RenderFormClientAction {
      }
      _RenderFormClientAction.dataWorkspaceId = "dataWorkspaceId";
      _RenderFormClientAction.dataUri = "dataUri";
      _RenderFormClientAction.formAutoGenerate = "formAutoGenerate";
      _RenderFormClientAction.formType = "formType";
      _RenderFormClientAction.form = "form";
      _RenderFormClientAction.actionName = "actionName";
      _RenderFormClientAction.element = "element";
      _RenderFormClientAction.parameter = "parameter";
      _RenderFormClientAction._name_ = "name";
      _ClientActions2._RenderFormClientAction = _RenderFormClientAction;
      _ClientActions2.__RenderFormClientAction_Uri = "dm:///_internal/types/internal#ffa8c9f8-9454-423d-882d-a44b98eaf4ea";
      class _RenderHtmlClientAction {
      }
      _RenderHtmlClientAction.html = "html";
      _RenderHtmlClientAction.text = "text";
      _RenderHtmlClientAction.actionName = "actionName";
      _RenderHtmlClientAction.element = "element";
      _RenderHtmlClientAction.parameter = "parameter";
      _RenderHtmlClientAction._name_ = "name";
      _ClientActions2._RenderHtmlClientAction = _RenderHtmlClientAction;
      _ClientActions2.__RenderHtmlClientAction_Uri = "dm:///_internal/types/internal#30f7e380-73e1-4ade-b797-1cc948b63c4f";
    })(_ClientActions = _Actions2._ClientActions || (_Actions2._ClientActions = {}));
    let _Forms2;
    (function(_Forms3) {
      class _CreateFormByMetaClass {
      }
      _CreateFormByMetaClass.metaClass = "metaClass";
      _CreateFormByMetaClass.creationMode = "creationMode";
      _CreateFormByMetaClass.targetContainer = "targetContainer";
      _CreateFormByMetaClass._name_ = "name";
      _CreateFormByMetaClass.isDisabled = "isDisabled";
      _Forms3._CreateFormByMetaClass = _CreateFormByMetaClass;
      _Forms3.__CreateFormByMetaClass_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.CreateFormByMetaclass";
      class _GetAttributeOfItem {
      }
      _GetAttributeOfItem.itemUri = "itemUri";
      _GetAttributeOfItem.workspace = "workspace";
      _GetAttributeOfItem.propertyName = "propertyName";
      _GetAttributeOfItem._name_ = "name";
      _GetAttributeOfItem.isDisabled = "isDisabled";
      _Forms3._GetAttributeOfItem = _GetAttributeOfItem;
      _Forms3.__GetAttributeOfItem_Uri = "dm:///_internal/types/internal#d9838e98-fd94-4dc1-883a-4362a2d732d3";
      class _GetAttributeOfItemResult {
      }
      _GetAttributeOfItemResult.isComposite = "isComposite";
      _GetAttributeOfItemResult.metaClassUri = "metaClassUri";
      _GetAttributeOfItemResult.isMultiple = "isMultiple";
      _Forms3._GetAttributeOfItemResult = _GetAttributeOfItemResult;
      _Forms3.__GetAttributeOfItemResult_Uri = "dm:///_internal/types/internal#e144d177-70e2-4418-991a-8262523c62b7";
    })(_Forms2 = _Actions2._Forms || (_Actions2._Forms = {}));
    let _OSIntegration;
    (function(_OSIntegration2) {
      class _CommandExecutionAction {
      }
      _CommandExecutionAction.command = "command";
      _CommandExecutionAction._arguments_ = "arguments";
      _CommandExecutionAction.workingDirectory = "workingDirectory";
      _CommandExecutionAction._name_ = "name";
      _CommandExecutionAction.isDisabled = "isDisabled";
      _OSIntegration2._CommandExecutionAction = _CommandExecutionAction;
      _OSIntegration2.__CommandExecutionAction_Uri = "dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82";
      class _PowershellExecutionAction {
      }
      _PowershellExecutionAction.script = "script";
      _PowershellExecutionAction.workingDirectory = "workingDirectory";
      _PowershellExecutionAction._name_ = "name";
      _PowershellExecutionAction.isDisabled = "isDisabled";
      _OSIntegration2._PowershellExecutionAction = _PowershellExecutionAction;
      _OSIntegration2.__PowershellExecutionAction_Uri = "dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb";
      class _ConsoleWriteAction {
      }
      _ConsoleWriteAction.text = "text";
      _ConsoleWriteAction._name_ = "name";
      _ConsoleWriteAction.isDisabled = "isDisabled";
      _OSIntegration2._ConsoleWriteAction = _ConsoleWriteAction;
      _OSIntegration2.__ConsoleWriteAction_Uri = "dm:///_internal/types/internal#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a";
    })(_OSIntegration = _Actions2._OSIntegration || (_Actions2._OSIntegration = {}));
    class _RefreshTypeIndexAction {
    }
    _RefreshTypeIndexAction.waitForRefresh = "waitForRefresh";
    _Actions2._RefreshTypeIndexAction = _RefreshTypeIndexAction;
    _Actions2.__RefreshTypeIndexAction_Uri = "dm:///_internal/types/internal#9d43decb-aa2f-4461-b680-3ec595b518d1";
    class _StoreElementAction {
    }
    _StoreElementAction.workspace = "workspace";
    _StoreElementAction.url = "url";
    _StoreElementAction.element = "element";
    _StoreElementAction._name_ = "name";
    _StoreElementAction.isDisabled = "isDisabled";
    _Actions2._StoreElementAction = _StoreElementAction;
    _Actions2.__StoreElementAction_Uri = "dm:///_internal/types/internal#c7dcb24c-e53c-46f9-9e8a-3704095193a8";
    let _Data;
    (function(_Data2) {
      class _FreezeViewResultAction {
      }
      _FreezeViewResultAction.viewNode = "viewNode";
      _FreezeViewResultAction._name_ = "name";
      _FreezeViewResultAction.isDisabled = "isDisabled";
      _Data2._FreezeViewResultAction = _FreezeViewResultAction;
      _Data2.__FreezeViewResultAction_Uri = "dm:///_internal/types/internal#fb1fcea9-c1d5-489d-9bd1-f64c6b71a171";
      class _FreezeViewResultInMemory {
      }
      _FreezeViewResultInMemory.extentUri = "extentUri";
      _FreezeViewResultInMemory.viewNode = "viewNode";
      _FreezeViewResultInMemory._name_ = "name";
      _FreezeViewResultInMemory.isDisabled = "isDisabled";
      _Data2._FreezeViewResultInMemory = _FreezeViewResultInMemory;
      _Data2.__FreezeViewResultInMemory_Uri = "dm:///_internal/types/internal#1286c17c-3286-4e8c-93fc-1a4c3e6df48a";
      class _FreezeViewResultInExtent {
      }
      _FreezeViewResultInExtent.extentLoaderConfig = "extentLoaderConfig";
      _FreezeViewResultInExtent.viewNode = "viewNode";
      _FreezeViewResultInExtent._name_ = "name";
      _FreezeViewResultInExtent.isDisabled = "isDisabled";
      _Data2._FreezeViewResultInExtent = _FreezeViewResultInExtent;
      _Data2.__FreezeViewResultInExtent_Uri = "dm:///_internal/types/internal#45cd9f30-fdfa-4f1b-a87c-24c088c075be";
    })(_Data = _Actions2._Data || (_Actions2._Data = {}));
  })(_Actions || (_Actions = {}));
  var _DataViews;
  (function(_DataViews2) {
    class _DataView {
    }
    _DataView._name_ = "name";
    _DataView.workspaceId = "workspaceId";
    _DataView.uri = "uri";
    _DataView.viewNode = "viewNode";
    _DataViews2._DataView = _DataView;
    _DataViews2.__DataView_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView";
    class _ViewNode {
    }
    _ViewNode._name_ = "name";
    _DataViews2._ViewNode = _ViewNode;
    _DataViews2.__ViewNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode";
    let _ComparisonMode;
    (function(_ComparisonMode2) {
      _ComparisonMode2.Equal = "Equal";
      _ComparisonMode2.NotEqual = "NotEqual";
      _ComparisonMode2.Contains = "Contains";
      _ComparisonMode2.DoesNotContain = "DoesNotContain";
      _ComparisonMode2.GreaterThan = "GreaterThan";
      _ComparisonMode2.GreaterOrEqualThan = "GreaterOrEqualThan";
      _ComparisonMode2.LighterThan = "LighterThan";
      _ComparisonMode2.LighterOrEqualThan = "LighterOrEqualThan";
      _ComparisonMode2.RegexMatch = "RegexMatch";
      _ComparisonMode2.RegexNoMatch = "RegexNoMatch";
    })(_ComparisonMode = _DataViews2._ComparisonMode || (_DataViews2._ComparisonMode = {}));
    let ___ComparisonMode;
    (function(___ComparisonMode2) {
      ___ComparisonMode2[___ComparisonMode2["Equal"] = 0] = "Equal";
      ___ComparisonMode2[___ComparisonMode2["NotEqual"] = 1] = "NotEqual";
      ___ComparisonMode2[___ComparisonMode2["Contains"] = 2] = "Contains";
      ___ComparisonMode2[___ComparisonMode2["DoesNotContain"] = 3] = "DoesNotContain";
      ___ComparisonMode2[___ComparisonMode2["GreaterThan"] = 4] = "GreaterThan";
      ___ComparisonMode2[___ComparisonMode2["GreaterOrEqualThan"] = 5] = "GreaterOrEqualThan";
      ___ComparisonMode2[___ComparisonMode2["LighterThan"] = 6] = "LighterThan";
      ___ComparisonMode2[___ComparisonMode2["LighterOrEqualThan"] = 7] = "LighterOrEqualThan";
      ___ComparisonMode2[___ComparisonMode2["RegexMatch"] = 8] = "RegexMatch";
      ___ComparisonMode2[___ComparisonMode2["RegexNoMatch"] = 9] = "RegexNoMatch";
    })(___ComparisonMode = _DataViews2.___ComparisonMode || (_DataViews2.___ComparisonMode = {}));
    class _QueryStatement {
    }
    _QueryStatement.nodes = "nodes";
    _QueryStatement.resultNode = "resultNode";
    _QueryStatement._name_ = "name";
    _DataViews2._QueryStatement = _QueryStatement;
    _DataViews2.__QueryStatement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement";
    let _Row;
    (function(_Row2) {
      class _RowFilterByFreeTextAnywhere {
      }
      _RowFilterByFreeTextAnywhere.freeText = "freeText";
      _RowFilterByFreeTextAnywhere.input = "input";
      _RowFilterByFreeTextAnywhere.propertyName = "propertyName";
      _RowFilterByFreeTextAnywhere._name_ = "name";
      _Row2._RowFilterByFreeTextAnywhere = _RowFilterByFreeTextAnywhere;
      _Row2.__RowFilterByFreeTextAnywhere_Uri = "dm:///_internal/types/internal#5f66ff9a-0a68-4c87-856b-5921c7cae628";
      class _RowFilterByPropertyValueNode {
      }
      _RowFilterByPropertyValueNode.input = "input";
      _RowFilterByPropertyValueNode.property = "property";
      _RowFilterByPropertyValueNode.value = "value";
      _RowFilterByPropertyValueNode.comparisonMode = "comparisonMode";
      _RowFilterByPropertyValueNode._name_ = "name";
      _Row2._RowFilterByPropertyValueNode = _RowFilterByPropertyValueNode;
      _Row2.__RowFilterByPropertyValueNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode";
      class _RowOrderByNode {
      }
      _RowOrderByNode.input = "input";
      _RowOrderByNode.propertyName = "propertyName";
      _RowOrderByNode.orderDescending = "orderDescending";
      _RowOrderByNode._name_ = "name";
      _Row2._RowOrderByNode = _RowOrderByNode;
      _Row2.__RowOrderByNode_Uri = "dm:///_internal/types/internal#e6948145-e1b7-4542-84e5-269dab1aa4c9";
      class _RowFilterOnPositionNode {
      }
      _RowFilterOnPositionNode.input = "input";
      _RowFilterOnPositionNode.amount = "amount";
      _RowFilterOnPositionNode.position = "position";
      _RowFilterOnPositionNode._name_ = "name";
      _Row2._RowFilterOnPositionNode = _RowFilterOnPositionNode;
      _Row2.__RowFilterOnPositionNode_Uri = "dm:///_internal/types/internal#d705b34b-369f-4b44-9a00-013e1daa759f";
      class _RowFlattenNode {
      }
      _RowFlattenNode.input = "input";
      _RowFlattenNode._name_ = "name";
      _Row2._RowFlattenNode = _RowFlattenNode;
      _Row2.__RowFlattenNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode";
      class _RowFilterByMetaclassNode {
      }
      _RowFilterByMetaclassNode.input = "input";
      _RowFilterByMetaclassNode.metaClass = "metaClass";
      _RowFilterByMetaclassNode.includeInherits = "includeInherits";
      _RowFilterByMetaclassNode._name_ = "name";
      _Row2._RowFilterByMetaclassNode = _RowFilterByMetaclassNode;
      _Row2.__RowFilterByMetaclassNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByMetaclassNode";
    })(_Row = _DataViews2._Row || (_DataViews2._Row = {}));
    let _Column;
    (function(_Column2) {
      class _ColumnFilterIncludeOnlyNode {
      }
      _ColumnFilterIncludeOnlyNode.columnNamesComma = "columnNamesComma";
      _ColumnFilterIncludeOnlyNode.input = "input";
      _ColumnFilterIncludeOnlyNode._name_ = "name";
      _Column2._ColumnFilterIncludeOnlyNode = _ColumnFilterIncludeOnlyNode;
      _Column2.__ColumnFilterIncludeOnlyNode_Uri = "dm:///_internal/types/internal#00d223b8-4335-4ee3-9359-92354e2d669d";
      class _ColumnFilterExcludeNode {
      }
      _ColumnFilterExcludeNode.columnNamesComma = "columnNamesComma";
      _ColumnFilterExcludeNode.input = "input";
      _ColumnFilterExcludeNode._name_ = "name";
      _Column2._ColumnFilterExcludeNode = _ColumnFilterExcludeNode;
      _Column2.__ColumnFilterExcludeNode_Uri = "dm:///_internal/types/internal#abca8647-18d7-4322-a803-2e3e1cd123d7";
    })(_Column = _DataViews2._Column || (_DataViews2._Column = {}));
    let _Source;
    (function(_Source2) {
      class _SelectByExtentNode {
      }
      _SelectByExtentNode.extentUri = "extentUri";
      _SelectByExtentNode.workspaceId = "workspaceId";
      _SelectByExtentNode._name_ = "name";
      _Source2._SelectByExtentNode = _SelectByExtentNode;
      _Source2.__SelectByExtentNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByExtentNode";
      class _SelectByPathNode {
      }
      _SelectByPathNode.workspaceId = "workspaceId";
      _SelectByPathNode.path = "path";
      _SelectByPathNode._name_ = "name";
      _Source2._SelectByPathNode = _SelectByPathNode;
      _Source2.__SelectByPathNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByPathNode";
      class _DynamicSourceNode {
      }
      _DynamicSourceNode.nodeName = "nodeName";
      _DynamicSourceNode._name_ = "name";
      _Source2._DynamicSourceNode = _DynamicSourceNode;
      _Source2.__DynamicSourceNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode";
      class _SelectByWorkspaceNode {
      }
      _SelectByWorkspaceNode.workspaceId = "workspaceId";
      _SelectByWorkspaceNode._name_ = "name";
      _Source2._SelectByWorkspaceNode = _SelectByWorkspaceNode;
      _Source2.__SelectByWorkspaceNode_Uri = "dm:///_internal/types/internal#a7276e99-351c-4aed-8ff1-a4b5ee45b0db";
      class _SelectFromAllWorkspacesNode {
      }
      _SelectFromAllWorkspacesNode._name_ = "name";
      _Source2._SelectFromAllWorkspacesNode = _SelectFromAllWorkspacesNode;
      _Source2.__SelectFromAllWorkspacesNode_Uri = "dm:///_internal/types/internal#a890d5ec-2686-4f18-9f9f-7037c7fe226a";
    })(_Source = _DataViews2._Source || (_DataViews2._Source = {}));
    let _Node;
    (function(_Node2) {
      class _ReferenceViewNode {
      }
      _ReferenceViewNode.workspaceId = "workspaceId";
      _ReferenceViewNode.itemUri = "itemUri";
      _ReferenceViewNode._name_ = "name";
      _Node2._ReferenceViewNode = _ReferenceViewNode;
      _Node2.__ReferenceViewNode_Uri = "dm:///_internal/types/internal#e80d4c64-a68e-44a7-893d-1a5100a80370";
    })(_Node = _DataViews2._Node || (_DataViews2._Node = {}));
    class _ValueItem {
    }
    _ValueItem.value = "value";
    _DataViews2._ValueItem = _ValueItem;
    _DataViews2.__ValueItem_Uri = "dm:///_internal/types/internal#4394a28a-0def-4030-b5d0-7a1b5b01c91b";
    let _Transformation;
    (function(_Transformation2) {
      class _SelectByFullNameNode {
      }
      _SelectByFullNameNode.input = "input";
      _SelectByFullNameNode.path = "path";
      _SelectByFullNameNode._name_ = "name";
      _Transformation2._SelectByFullNameNode = _SelectByFullNameNode;
      _Transformation2.__SelectByFullNameNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode";
      class _SelectByProperty {
      }
      _SelectByProperty.input = "input";
      _SelectByProperty.propertyName = "propertyName";
      _SelectByProperty._name_ = "name";
      _Transformation2._SelectByProperty = _SelectByProperty;
      _Transformation2.__SelectByProperty_Uri = "dm:///_internal/types/internal#2a5ac57c-e5d2-498b-b575-7b07354e2645";
    })(_Transformation = _DataViews2._Transformation || (_DataViews2._Transformation = {}));
  })(_DataViews || (_DataViews = {}));
  var _Reports;
  (function(_Reports2) {
    class _ReportDefinition {
    }
    _ReportDefinition._name_ = "name";
    _ReportDefinition.title = "title";
    _ReportDefinition.elements = "elements";
    _Reports2._ReportDefinition = _ReportDefinition;
    _Reports2.__ReportDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition";
    class _ReportInstanceSource {
    }
    _ReportInstanceSource._name_ = "name";
    _ReportInstanceSource.workspaceId = "workspaceId";
    _ReportInstanceSource.path = "path";
    _Reports2._ReportInstanceSource = _ReportInstanceSource;
    _Reports2.__ReportInstanceSource_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource";
    class _ReportInstance {
    }
    _ReportInstance._name_ = "name";
    _ReportInstance.reportDefinition = "reportDefinition";
    _ReportInstance.sources = "sources";
    _Reports2._ReportInstance = _ReportInstance;
    _Reports2.__ReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance";
    class _AdocReportInstance {
    }
    _AdocReportInstance._name_ = "name";
    _AdocReportInstance.reportDefinition = "reportDefinition";
    _AdocReportInstance.sources = "sources";
    _Reports2._AdocReportInstance = _AdocReportInstance;
    _Reports2.__AdocReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance";
    class _HtmlReportInstance {
    }
    _HtmlReportInstance.cssFile = "cssFile";
    _HtmlReportInstance.cssStyleSheet = "cssStyleSheet";
    _HtmlReportInstance._name_ = "name";
    _HtmlReportInstance.reportDefinition = "reportDefinition";
    _HtmlReportInstance.sources = "sources";
    _Reports2._HtmlReportInstance = _HtmlReportInstance;
    _Reports2.__HtmlReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance";
    let _DescendentMode;
    (function(_DescendentMode2) {
      _DescendentMode2.None = "None";
      _DescendentMode2.Inline = "Inline";
      _DescendentMode2.PerPackage = "PerPackage";
    })(_DescendentMode = _Reports2._DescendentMode || (_Reports2._DescendentMode = {}));
    let ___DescendentMode;
    (function(___DescendentMode2) {
      ___DescendentMode2[___DescendentMode2["None"] = 0] = "None";
      ___DescendentMode2[___DescendentMode2["Inline"] = 1] = "Inline";
      ___DescendentMode2[___DescendentMode2["PerPackage"] = 2] = "PerPackage";
    })(___DescendentMode = _Reports2.___DescendentMode || (_Reports2.___DescendentMode = {}));
    class _SimpleReportConfiguration {
    }
    _SimpleReportConfiguration._name_ = "name";
    _SimpleReportConfiguration.showDescendents = "showDescendents";
    _SimpleReportConfiguration.rootElement = "rootElement";
    _SimpleReportConfiguration.showRootElement = "showRootElement";
    _SimpleReportConfiguration.showMetaClasses = "showMetaClasses";
    _SimpleReportConfiguration.showFullName = "showFullName";
    _SimpleReportConfiguration.form = "form";
    _SimpleReportConfiguration.descendentMode = "descendentMode";
    _SimpleReportConfiguration.typeMode = "typeMode";
    _SimpleReportConfiguration.workspaceId = "workspaceId";
    _Reports2._SimpleReportConfiguration = _SimpleReportConfiguration;
    _Reports2.__SimpleReportConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration";
    let _Elements;
    (function(_Elements2) {
      class _ReportElement {
      }
      _ReportElement._name_ = "name";
      _Elements2._ReportElement = _ReportElement;
      _Elements2.__ReportElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement";
      class _ReportHeadline {
      }
      _ReportHeadline.title = "title";
      _ReportHeadline._name_ = "name";
      _Elements2._ReportHeadline = _ReportHeadline;
      _Elements2.__ReportHeadline_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline";
      class _ReportParagraph {
      }
      _ReportParagraph.paragraph = "paragraph";
      _ReportParagraph.cssClass = "cssClass";
      _ReportParagraph.viewNode = "viewNode";
      _ReportParagraph.evalProperties = "evalProperties";
      _ReportParagraph.evalParagraph = "evalParagraph";
      _ReportParagraph._name_ = "name";
      _Elements2._ReportParagraph = _ReportParagraph;
      _Elements2.__ReportParagraph_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph";
      class _ReportTable {
      }
      _ReportTable.cssClass = "cssClass";
      _ReportTable.viewNode = "viewNode";
      _ReportTable.form = "form";
      _ReportTable.evalProperties = "evalProperties";
      _ReportTable._name_ = "name";
      _Elements2._ReportTable = _ReportTable;
      _Elements2.__ReportTable_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable";
      let _ReportTableForTypeMode;
      (function(_ReportTableForTypeMode2) {
        _ReportTableForTypeMode2.PerType = "PerType";
        _ReportTableForTypeMode2.AllTypes = "AllTypes";
      })(_ReportTableForTypeMode = _Elements2._ReportTableForTypeMode || (_Elements2._ReportTableForTypeMode = {}));
      let ___ReportTableForTypeMode;
      (function(___ReportTableForTypeMode2) {
        ___ReportTableForTypeMode2[___ReportTableForTypeMode2["PerType"] = 0] = "PerType";
        ___ReportTableForTypeMode2[___ReportTableForTypeMode2["AllTypes"] = 1] = "AllTypes";
      })(___ReportTableForTypeMode = _Elements2.___ReportTableForTypeMode || (_Elements2.___ReportTableForTypeMode = {}));
      class _ReportLoop {
      }
      _ReportLoop.viewNode = "viewNode";
      _ReportLoop.elements = "elements";
      _ReportLoop._name_ = "name";
      _Elements2._ReportLoop = _ReportLoop;
      _Elements2.__ReportLoop_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop";
    })(_Elements = _Reports2._Elements || (_Reports2._Elements = {}));
  })(_Reports || (_Reports = {}));
  var _ExtentLoaderConfigs;
  (function(_ExtentLoaderConfigs2) {
    class _ExtentLoaderConfig {
    }
    _ExtentLoaderConfig._name_ = "name";
    _ExtentLoaderConfig.extentUri = "extentUri";
    _ExtentLoaderConfig.workspaceId = "workspaceId";
    _ExtentLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._ExtentLoaderConfig = _ExtentLoaderConfig;
    _ExtentLoaderConfigs2.__ExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig";
    class _ExcelLoaderConfig {
    }
    _ExcelLoaderConfig.fixRowCount = "fixRowCount";
    _ExcelLoaderConfig.fixColumnCount = "fixColumnCount";
    _ExcelLoaderConfig.filePath = "filePath";
    _ExcelLoaderConfig.sheetName = "sheetName";
    _ExcelLoaderConfig.offsetRow = "offsetRow";
    _ExcelLoaderConfig.offsetColumn = "offsetColumn";
    _ExcelLoaderConfig.countRows = "countRows";
    _ExcelLoaderConfig.countColumns = "countColumns";
    _ExcelLoaderConfig.hasHeader = "hasHeader";
    _ExcelLoaderConfig.tryMergedHeaderCells = "tryMergedHeaderCells";
    _ExcelLoaderConfig.onlySetColumns = "onlySetColumns";
    _ExcelLoaderConfig.idColumnName = "idColumnName";
    _ExcelLoaderConfig.skipEmptyRowsCount = "skipEmptyRowsCount";
    _ExcelLoaderConfig.columns = "columns";
    _ExcelLoaderConfig._name_ = "name";
    _ExcelLoaderConfig.extentUri = "extentUri";
    _ExcelLoaderConfig.workspaceId = "workspaceId";
    _ExcelLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._ExcelLoaderConfig = _ExcelLoaderConfig;
    _ExtentLoaderConfigs2.__ExcelLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig";
    class _ExcelReadOnlyLoaderConfig {
    }
    _ExcelReadOnlyLoaderConfig.fixRowCount = "fixRowCount";
    _ExcelReadOnlyLoaderConfig.fixColumnCount = "fixColumnCount";
    _ExcelReadOnlyLoaderConfig.filePath = "filePath";
    _ExcelReadOnlyLoaderConfig.sheetName = "sheetName";
    _ExcelReadOnlyLoaderConfig.offsetRow = "offsetRow";
    _ExcelReadOnlyLoaderConfig.offsetColumn = "offsetColumn";
    _ExcelReadOnlyLoaderConfig.countRows = "countRows";
    _ExcelReadOnlyLoaderConfig.countColumns = "countColumns";
    _ExcelReadOnlyLoaderConfig.hasHeader = "hasHeader";
    _ExcelReadOnlyLoaderConfig.tryMergedHeaderCells = "tryMergedHeaderCells";
    _ExcelReadOnlyLoaderConfig.onlySetColumns = "onlySetColumns";
    _ExcelReadOnlyLoaderConfig.idColumnName = "idColumnName";
    _ExcelReadOnlyLoaderConfig.skipEmptyRowsCount = "skipEmptyRowsCount";
    _ExcelReadOnlyLoaderConfig.columns = "columns";
    _ExcelReadOnlyLoaderConfig._name_ = "name";
    _ExcelReadOnlyLoaderConfig.extentUri = "extentUri";
    _ExcelReadOnlyLoaderConfig.workspaceId = "workspaceId";
    _ExcelReadOnlyLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._ExcelReadOnlyLoaderConfig = _ExcelReadOnlyLoaderConfig;
    _ExtentLoaderConfigs2.__ExcelReadOnlyLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig";
    class _ExcelConvertToXmiOnceConfig {
    }
    _ExcelConvertToXmiOnceConfig.xmiFilePath = "xmiFilePath";
    _ExcelConvertToXmiOnceConfig.fixRowCount = "fixRowCount";
    _ExcelConvertToXmiOnceConfig.fixColumnCount = "fixColumnCount";
    _ExcelConvertToXmiOnceConfig.filePath = "filePath";
    _ExcelConvertToXmiOnceConfig.sheetName = "sheetName";
    _ExcelConvertToXmiOnceConfig.offsetRow = "offsetRow";
    _ExcelConvertToXmiOnceConfig.offsetColumn = "offsetColumn";
    _ExcelConvertToXmiOnceConfig.countRows = "countRows";
    _ExcelConvertToXmiOnceConfig.countColumns = "countColumns";
    _ExcelConvertToXmiOnceConfig.hasHeader = "hasHeader";
    _ExcelConvertToXmiOnceConfig.tryMergedHeaderCells = "tryMergedHeaderCells";
    _ExcelConvertToXmiOnceConfig.onlySetColumns = "onlySetColumns";
    _ExcelConvertToXmiOnceConfig.idColumnName = "idColumnName";
    _ExcelConvertToXmiOnceConfig.skipEmptyRowsCount = "skipEmptyRowsCount";
    _ExcelConvertToXmiOnceConfig.columns = "columns";
    _ExcelConvertToXmiOnceConfig._name_ = "name";
    _ExcelConvertToXmiOnceConfig.extentUri = "extentUri";
    _ExcelConvertToXmiOnceConfig.workspaceId = "workspaceId";
    _ExcelConvertToXmiOnceConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._ExcelConvertToXmiOnceConfig = _ExcelConvertToXmiOnceConfig;
    _ExtentLoaderConfigs2.__ExcelConvertToXmiOnceConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig";
    class _ExcelFullSyncLoaderConfig {
    }
    _ExcelFullSyncLoaderConfig.fixRowCount = "fixRowCount";
    _ExcelFullSyncLoaderConfig.fixColumnCount = "fixColumnCount";
    _ExcelFullSyncLoaderConfig.filePath = "filePath";
    _ExcelFullSyncLoaderConfig.sheetName = "sheetName";
    _ExcelFullSyncLoaderConfig.offsetRow = "offsetRow";
    _ExcelFullSyncLoaderConfig.offsetColumn = "offsetColumn";
    _ExcelFullSyncLoaderConfig.countRows = "countRows";
    _ExcelFullSyncLoaderConfig.countColumns = "countColumns";
    _ExcelFullSyncLoaderConfig.hasHeader = "hasHeader";
    _ExcelFullSyncLoaderConfig.tryMergedHeaderCells = "tryMergedHeaderCells";
    _ExcelFullSyncLoaderConfig.onlySetColumns = "onlySetColumns";
    _ExcelFullSyncLoaderConfig.idColumnName = "idColumnName";
    _ExcelFullSyncLoaderConfig.skipEmptyRowsCount = "skipEmptyRowsCount";
    _ExcelFullSyncLoaderConfig.columns = "columns";
    _ExcelFullSyncLoaderConfig._name_ = "name";
    _ExcelFullSyncLoaderConfig.extentUri = "extentUri";
    _ExcelFullSyncLoaderConfig.workspaceId = "workspaceId";
    _ExcelFullSyncLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._ExcelFullSyncLoaderConfig = _ExcelFullSyncLoaderConfig;
    _ExtentLoaderConfigs2.__ExcelFullSyncLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig";
    class _InMemoryLoaderConfig {
    }
    _InMemoryLoaderConfig.isLinkedList = "isLinkedList";
    _InMemoryLoaderConfig._name_ = "name";
    _InMemoryLoaderConfig.extentUri = "extentUri";
    _InMemoryLoaderConfig.workspaceId = "workspaceId";
    _InMemoryLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._InMemoryLoaderConfig = _InMemoryLoaderConfig;
    _ExtentLoaderConfigs2.__InMemoryLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig";
    class _XmlReferenceLoaderConfig {
    }
    _XmlReferenceLoaderConfig.filePath = "filePath";
    _XmlReferenceLoaderConfig.keepNamespaces = "keepNamespaces";
    _XmlReferenceLoaderConfig._name_ = "name";
    _XmlReferenceLoaderConfig.extentUri = "extentUri";
    _XmlReferenceLoaderConfig.workspaceId = "workspaceId";
    _XmlReferenceLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._XmlReferenceLoaderConfig = _XmlReferenceLoaderConfig;
    _ExtentLoaderConfigs2.__XmlReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig";
    class _ExtentFileLoaderConfig {
    }
    _ExtentFileLoaderConfig.filePath = "filePath";
    _ExtentFileLoaderConfig._name_ = "name";
    _ExtentFileLoaderConfig.extentUri = "extentUri";
    _ExtentFileLoaderConfig.workspaceId = "workspaceId";
    _ExtentFileLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._ExtentFileLoaderConfig = _ExtentFileLoaderConfig;
    _ExtentLoaderConfigs2.__ExtentFileLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig";
    class _XmiStorageLoaderConfig {
    }
    _XmiStorageLoaderConfig.filePath = "filePath";
    _XmiStorageLoaderConfig._name_ = "name";
    _XmiStorageLoaderConfig.extentUri = "extentUri";
    _XmiStorageLoaderConfig.workspaceId = "workspaceId";
    _XmiStorageLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._XmiStorageLoaderConfig = _XmiStorageLoaderConfig;
    _ExtentLoaderConfigs2.__XmiStorageLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig";
    class _CsvExtentLoaderConfig {
    }
    _CsvExtentLoaderConfig.settings = "settings";
    _CsvExtentLoaderConfig.filePath = "filePath";
    _CsvExtentLoaderConfig._name_ = "name";
    _CsvExtentLoaderConfig.extentUri = "extentUri";
    _CsvExtentLoaderConfig.workspaceId = "workspaceId";
    _CsvExtentLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._CsvExtentLoaderConfig = _CsvExtentLoaderConfig;
    _ExtentLoaderConfigs2.__CsvExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig";
    class _CsvSettings {
    }
    _CsvSettings.encoding = "encoding";
    _CsvSettings.hasHeader = "hasHeader";
    _CsvSettings.separator = "separator";
    _CsvSettings.columns = "columns";
    _CsvSettings.metaclassUri = "metaclassUri";
    _CsvSettings.trimCells = "trimCells";
    _ExtentLoaderConfigs2._CsvSettings = _CsvSettings;
    _ExtentLoaderConfigs2.__CsvSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings";
    class _ExcelHierarchicalColumnDefinition {
    }
    _ExcelHierarchicalColumnDefinition._name_ = "name";
    _ExcelHierarchicalColumnDefinition.metaClass = "metaClass";
    _ExcelHierarchicalColumnDefinition.property = "property";
    _ExtentLoaderConfigs2._ExcelHierarchicalColumnDefinition = _ExcelHierarchicalColumnDefinition;
    _ExtentLoaderConfigs2.__ExcelHierarchicalColumnDefinition_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition";
    class _ExcelHierarchicalLoaderConfig {
    }
    _ExcelHierarchicalLoaderConfig.hierarchicalColumns = "hierarchicalColumns";
    _ExcelHierarchicalLoaderConfig.skipElementsForLastLevel = "skipElementsForLastLevel";
    _ExcelHierarchicalLoaderConfig.fixRowCount = "fixRowCount";
    _ExcelHierarchicalLoaderConfig.fixColumnCount = "fixColumnCount";
    _ExcelHierarchicalLoaderConfig.filePath = "filePath";
    _ExcelHierarchicalLoaderConfig.sheetName = "sheetName";
    _ExcelHierarchicalLoaderConfig.offsetRow = "offsetRow";
    _ExcelHierarchicalLoaderConfig.offsetColumn = "offsetColumn";
    _ExcelHierarchicalLoaderConfig.countRows = "countRows";
    _ExcelHierarchicalLoaderConfig.countColumns = "countColumns";
    _ExcelHierarchicalLoaderConfig.hasHeader = "hasHeader";
    _ExcelHierarchicalLoaderConfig.tryMergedHeaderCells = "tryMergedHeaderCells";
    _ExcelHierarchicalLoaderConfig.onlySetColumns = "onlySetColumns";
    _ExcelHierarchicalLoaderConfig.idColumnName = "idColumnName";
    _ExcelHierarchicalLoaderConfig.skipEmptyRowsCount = "skipEmptyRowsCount";
    _ExcelHierarchicalLoaderConfig.columns = "columns";
    _ExcelHierarchicalLoaderConfig._name_ = "name";
    _ExcelHierarchicalLoaderConfig.extentUri = "extentUri";
    _ExcelHierarchicalLoaderConfig.workspaceId = "workspaceId";
    _ExcelHierarchicalLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._ExcelHierarchicalLoaderConfig = _ExcelHierarchicalLoaderConfig;
    _ExtentLoaderConfigs2.__ExcelHierarchicalLoaderConfig_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig";
    class _ExcelColumn {
    }
    _ExcelColumn.header = "header";
    _ExcelColumn._name_ = "name";
    _ExtentLoaderConfigs2._ExcelColumn = _ExcelColumn;
    _ExtentLoaderConfigs2.__ExcelColumn_Uri = "dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a";
    class _EnvironmentalVariableLoaderConfig {
    }
    _EnvironmentalVariableLoaderConfig._name_ = "name";
    _EnvironmentalVariableLoaderConfig.extentUri = "extentUri";
    _EnvironmentalVariableLoaderConfig.workspaceId = "workspaceId";
    _EnvironmentalVariableLoaderConfig.dropExisting = "dropExisting";
    _ExtentLoaderConfigs2._EnvironmentalVariableLoaderConfig = _EnvironmentalVariableLoaderConfig;
    _ExtentLoaderConfigs2.__EnvironmentalVariableLoaderConfig_Uri = "dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3";
    let _ExtentLoaderImportType;
    (function(_ExtentLoaderImportType2) {
      _ExtentLoaderImportType2.NoSync = "NoSync";
      _ExtentLoaderImportType2.ReadOnlySync = "ReadOnlySync";
      _ExtentLoaderImportType2.BidirectionalSync = "BidirectionalSync";
    })(_ExtentLoaderImportType = _ExtentLoaderConfigs2._ExtentLoaderImportType || (_ExtentLoaderConfigs2._ExtentLoaderImportType = {}));
    let ___ExtentLoaderImportType;
    (function(___ExtentLoaderImportType2) {
      ___ExtentLoaderImportType2[___ExtentLoaderImportType2["NoSync"] = 0] = "NoSync";
      ___ExtentLoaderImportType2[___ExtentLoaderImportType2["ReadOnlySync"] = 1] = "ReadOnlySync";
      ___ExtentLoaderImportType2[___ExtentLoaderImportType2["BidirectionalSync"] = 2] = "BidirectionalSync";
    })(___ExtentLoaderImportType = _ExtentLoaderConfigs2.___ExtentLoaderImportType || (_ExtentLoaderConfigs2.___ExtentLoaderImportType = {}));
  })(_ExtentLoaderConfigs || (_ExtentLoaderConfigs = {}));
  var _Forms;
  (function(_Forms2) {
    class _SortingOrder {
    }
    _SortingOrder._name_ = "name";
    _SortingOrder.isDescending = "isDescending";
    _Forms2._SortingOrder = _SortingOrder;
    _Forms2.__SortingOrder_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder";
    class _DefaultTypeForNewElement {
    }
    _DefaultTypeForNewElement._name_ = "name";
    _DefaultTypeForNewElement.metaClass = "metaClass";
    _DefaultTypeForNewElement.parentProperty = "parentProperty";
    _Forms2._DefaultTypeForNewElement = _DefaultTypeForNewElement;
    _Forms2.__DefaultTypeForNewElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement";
    class _ViewMode {
    }
    _ViewMode._name_ = "name";
    _ViewMode.id = "id";
    _ViewMode.defaultExtentType = "defaultExtentType";
    _Forms2._ViewMode = _ViewMode;
    _Forms2.__ViewMode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode";
    class _NavigateToFieldsForTestAction {
    }
    _NavigateToFieldsForTestAction._name_ = "name";
    _NavigateToFieldsForTestAction.isDisabled = "isDisabled";
    _Forms2._NavigateToFieldsForTestAction = _NavigateToFieldsForTestAction;
    _Forms2.__NavigateToFieldsForTestAction_Uri = "dm:///_internal/types/internal#ba1403c9-20cd-487d-8147-3937889deeb0";
    let _FormTypes;
    (function(_FormTypes2) {
      let _FormType;
      (function(_FormType2) {
        _FormType2.Object = "Object";
        _FormType2.Collection = "Collection";
        _FormType2.Row = "Row";
        _FormType2.Table = "Table";
        _FormType2.ObjectExtension = "ObjectExtension";
        _FormType2.CollectionExtension = "CollectionExtension";
        _FormType2.RowExtension = "RowExtension";
        _FormType2.TableExtension = "TableExtension";
        _FormType2.Decoupled = "Decoupled";
      })(_FormType = _FormTypes2._FormType || (_FormTypes2._FormType = {}));
      let ___FormType;
      (function(___FormType2) {
        ___FormType2[___FormType2["Object"] = 0] = "Object";
        ___FormType2[___FormType2["Collection"] = 1] = "Collection";
        ___FormType2[___FormType2["Row"] = 2] = "Row";
        ___FormType2[___FormType2["Table"] = 3] = "Table";
        ___FormType2[___FormType2["ObjectExtension"] = 4] = "ObjectExtension";
        ___FormType2[___FormType2["CollectionExtension"] = 5] = "CollectionExtension";
        ___FormType2[___FormType2["RowExtension"] = 6] = "RowExtension";
        ___FormType2[___FormType2["TableExtension"] = 7] = "TableExtension";
        ___FormType2[___FormType2["Decoupled"] = 8] = "Decoupled";
      })(___FormType = _FormTypes2.___FormType || (_FormTypes2.___FormType = {}));
      class _Form {
      }
      _Form._name_ = "name";
      _Form.title = "title";
      _Form.isReadOnly = "isReadOnly";
      _Form.isAutoGenerated = "isAutoGenerated";
      _Form.hideMetaInformation = "hideMetaInformation";
      _Form.originalUri = "originalUri";
      _Form.originalWorkspace = "originalWorkspace";
      _Form.creationProtocol = "creationProtocol";
      _Form.hideReferencingItems = "hideReferencingItems";
      _FormTypes2._Form = _Form;
      _FormTypes2.__Form_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.Form";
      class _RowForm {
      }
      _RowForm.buttonApplyText = "buttonApplyText";
      _RowForm.allowNewProperties = "allowNewProperties";
      _RowForm.defaultWidth = "defaultWidth";
      _RowForm.defaultHeight = "defaultHeight";
      _RowForm.field = "field";
      _RowForm._name_ = "name";
      _RowForm.title = "title";
      _RowForm.isReadOnly = "isReadOnly";
      _RowForm.isAutoGenerated = "isAutoGenerated";
      _RowForm.hideMetaInformation = "hideMetaInformation";
      _RowForm.originalUri = "originalUri";
      _RowForm.originalWorkspace = "originalWorkspace";
      _RowForm.creationProtocol = "creationProtocol";
      _RowForm.hideReferencingItems = "hideReferencingItems";
      _FormTypes2._RowForm = _RowForm;
      _FormTypes2.__RowForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.RowForm";
      class _TableForm2 {
      }
      _TableForm2.property = "property";
      _TableForm2.metaClass = "metaClass";
      _TableForm2.includeDescendents = "includeDescendents";
      _TableForm2.noItemsWithMetaClass = "noItemsWithMetaClass";
      _TableForm2.inhibitNewItems = "inhibitNewItems";
      _TableForm2.inhibitDeleteItems = "inhibitDeleteItems";
      _TableForm2.inhibitEditItems = "inhibitEditItems";
      _TableForm2.defaultTypesForNewElements = "defaultTypesForNewElements";
      _TableForm2.fastViewFilters = "fastViewFilters";
      _TableForm2.field = "field";
      _TableForm2.sortingOrder = "sortingOrder";
      _TableForm2.viewNode = "viewNode";
      _TableForm2.autoGenerateFields = "autoGenerateFields";
      _TableForm2.duplicatePerType = "duplicatePerType";
      _TableForm2.dataUrl = "dataUrl";
      _TableForm2.inhibitNewUnclassifiedItems = "inhibitNewUnclassifiedItems";
      _TableForm2._name_ = "name";
      _TableForm2.title = "title";
      _TableForm2.isReadOnly = "isReadOnly";
      _TableForm2.isAutoGenerated = "isAutoGenerated";
      _TableForm2.hideMetaInformation = "hideMetaInformation";
      _TableForm2.originalUri = "originalUri";
      _TableForm2.originalWorkspace = "originalWorkspace";
      _TableForm2.creationProtocol = "creationProtocol";
      _TableForm2.hideReferencingItems = "hideReferencingItems";
      _FormTypes2._TableForm = _TableForm2;
      _FormTypes2.__TableForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TableForm";
      class _CollectionForm {
      }
      _CollectionForm.tab = "tab";
      _CollectionForm.autoTabs = "autoTabs";
      _CollectionForm.field = "field";
      _CollectionForm._name_ = "name";
      _CollectionForm.title = "title";
      _CollectionForm.isReadOnly = "isReadOnly";
      _CollectionForm.isAutoGenerated = "isAutoGenerated";
      _CollectionForm.hideMetaInformation = "hideMetaInformation";
      _CollectionForm.originalUri = "originalUri";
      _CollectionForm.originalWorkspace = "originalWorkspace";
      _CollectionForm.creationProtocol = "creationProtocol";
      _CollectionForm.hideReferencingItems = "hideReferencingItems";
      _FormTypes2._CollectionForm = _CollectionForm;
      _FormTypes2.__CollectionForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CollectionForm";
      class _ObjectForm {
      }
      _ObjectForm.tab = "tab";
      _ObjectForm.autoTabs = "autoTabs";
      _ObjectForm._name_ = "name";
      _ObjectForm.title = "title";
      _ObjectForm.isReadOnly = "isReadOnly";
      _ObjectForm.isAutoGenerated = "isAutoGenerated";
      _ObjectForm.hideMetaInformation = "hideMetaInformation";
      _ObjectForm.originalUri = "originalUri";
      _ObjectForm.originalWorkspace = "originalWorkspace";
      _ObjectForm.creationProtocol = "creationProtocol";
      _ObjectForm.hideReferencingItems = "hideReferencingItems";
      _FormTypes2._ObjectForm = _ObjectForm;
      _FormTypes2.__ObjectForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ObjectForm";
      class _FormAssociation {
      }
      _FormAssociation._name_ = "name";
      _FormAssociation.formType = "formType";
      _FormAssociation.metaClass = "metaClass";
      _FormAssociation.extentType = "extentType";
      _FormAssociation.viewModeId = "viewModeId";
      _FormAssociation.parentMetaClass = "parentMetaClass";
      _FormAssociation.parentProperty = "parentProperty";
      _FormAssociation.form = "form";
      _FormAssociation.debugActive = "debugActive";
      _FormAssociation.workspaceId = "workspaceId";
      _FormAssociation.extentUri = "extentUri";
      _FormAssociation.includeGeneralization = "includeGeneralization";
      _FormTypes2._FormAssociation = _FormAssociation;
      _FormTypes2.__FormAssociation_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation";
    })(_FormTypes = _Forms2._FormTypes || (_Forms2._FormTypes = {}));
    let _FieldTypes;
    (function(_FieldTypes2) {
      class _FieldData3 {
      }
      _FieldData3.isAttached = "isAttached";
      _FieldData3._name_ = "name";
      _FieldData3.title = "title";
      _FieldData3.isEnumeration = "isEnumeration";
      _FieldData3.defaultValue = "defaultValue";
      _FieldData3.isReadOnly = "isReadOnly";
      _FieldTypes2._FieldData = _FieldData3;
      _FieldTypes2.__FieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData";
      class _DropDownByQueryData {
      }
      _DropDownByQueryData.query = "query";
      _DropDownByQueryData.isAttached = "isAttached";
      _DropDownByQueryData._name_ = "name";
      _DropDownByQueryData.title = "title";
      _DropDownByQueryData.isEnumeration = "isEnumeration";
      _DropDownByQueryData.defaultValue = "defaultValue";
      _DropDownByQueryData.isReadOnly = "isReadOnly";
      _FieldTypes2._DropDownByQueryData = _DropDownByQueryData;
      _FieldTypes2.__DropDownByQueryData_Uri = "dm:///_internal/types/internal#8bb3e235-beed-4eb7-a95e-b5cfa4417bd2";
      class _DropDownByCollection {
      }
      _DropDownByCollection.defaultWorkspace = "defaultWorkspace";
      _DropDownByCollection.collection = "collection";
      _DropDownByCollection.isAttached = "isAttached";
      _DropDownByCollection._name_ = "name";
      _DropDownByCollection.title = "title";
      _DropDownByCollection.isEnumeration = "isEnumeration";
      _DropDownByCollection.defaultValue = "defaultValue";
      _DropDownByCollection.isReadOnly = "isReadOnly";
      _FieldTypes2._DropDownByCollection = _DropDownByCollection;
      _FieldTypes2.__DropDownByCollection_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownByCollection";
      class _UriReferenceFieldData2 {
      }
      _UriReferenceFieldData2.defaultWorkspace = "defaultWorkspace";
      _UriReferenceFieldData2.defaultExtent = "defaultExtent";
      _FieldTypes2._UriReferenceFieldData = _UriReferenceFieldData2;
      _FieldTypes2.__UriReferenceFieldData_Uri = "dm:///_internal/types/internal#26a9c433-ead8-414b-9a8e-bb5a1a8cca00";
      class _FullNameFieldData {
      }
      _FullNameFieldData.isAttached = "isAttached";
      _FullNameFieldData._name_ = "name";
      _FullNameFieldData.title = "title";
      _FullNameFieldData.isEnumeration = "isEnumeration";
      _FullNameFieldData.defaultValue = "defaultValue";
      _FullNameFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._FullNameFieldData = _FullNameFieldData;
      _FieldTypes2.__FullNameFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData";
      class _CheckboxListTaggingFieldData {
      }
      _CheckboxListTaggingFieldData.values = "values";
      _CheckboxListTaggingFieldData.separator = "separator";
      _CheckboxListTaggingFieldData.containsFreeText = "containsFreeText";
      _CheckboxListTaggingFieldData.isAttached = "isAttached";
      _CheckboxListTaggingFieldData._name_ = "name";
      _CheckboxListTaggingFieldData.title = "title";
      _CheckboxListTaggingFieldData.isEnumeration = "isEnumeration";
      _CheckboxListTaggingFieldData.defaultValue = "defaultValue";
      _CheckboxListTaggingFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._CheckboxListTaggingFieldData = _CheckboxListTaggingFieldData;
      _FieldTypes2.__CheckboxListTaggingFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData";
      class _NumberFieldData {
      }
      _NumberFieldData.format = "format";
      _NumberFieldData.isInteger = "isInteger";
      _NumberFieldData.isAttached = "isAttached";
      _NumberFieldData._name_ = "name";
      _NumberFieldData.title = "title";
      _NumberFieldData.isEnumeration = "isEnumeration";
      _NumberFieldData.defaultValue = "defaultValue";
      _NumberFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._NumberFieldData = _NumberFieldData;
      _FieldTypes2.__NumberFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData";
      class _DropDownFieldData {
      }
      _DropDownFieldData.values = "values";
      _DropDownFieldData.valuesByEnumeration = "valuesByEnumeration";
      _DropDownFieldData.isAttached = "isAttached";
      _DropDownFieldData._name_ = "name";
      _DropDownFieldData.title = "title";
      _DropDownFieldData.isEnumeration = "isEnumeration";
      _DropDownFieldData.defaultValue = "defaultValue";
      _DropDownFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._DropDownFieldData = _DropDownFieldData;
      _FieldTypes2.__DropDownFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData";
      class _ValuePair {
      }
      _ValuePair.value = "value";
      _ValuePair._name_ = "name";
      _FieldTypes2._ValuePair = _ValuePair;
      _FieldTypes2.__ValuePair_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair";
      class _MetaClassElementFieldData {
      }
      _MetaClassElementFieldData.isAttached = "isAttached";
      _MetaClassElementFieldData._name_ = "name";
      _MetaClassElementFieldData.title = "title";
      _MetaClassElementFieldData.isEnumeration = "isEnumeration";
      _MetaClassElementFieldData.defaultValue = "defaultValue";
      _MetaClassElementFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._MetaClassElementFieldData = _MetaClassElementFieldData;
      _FieldTypes2.__MetaClassElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData";
      class _ReferenceFieldData {
      }
      _ReferenceFieldData.isSelectionInline = "isSelectionInline";
      _ReferenceFieldData.defaultWorkspace = "defaultWorkspace";
      _ReferenceFieldData.defaultItemUri = "defaultItemUri";
      _ReferenceFieldData.showAllChildren = "showAllChildren";
      _ReferenceFieldData.showWorkspaceSelection = "showWorkspaceSelection";
      _ReferenceFieldData.showExtentSelection = "showExtentSelection";
      _ReferenceFieldData.metaClassFilter = "metaClassFilter";
      _ReferenceFieldData.isAttached = "isAttached";
      _ReferenceFieldData._name_ = "name";
      _ReferenceFieldData.title = "title";
      _ReferenceFieldData.isEnumeration = "isEnumeration";
      _ReferenceFieldData.defaultValue = "defaultValue";
      _ReferenceFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._ReferenceFieldData = _ReferenceFieldData;
      _FieldTypes2.__ReferenceFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData";
      class _SubElementFieldData {
      }
      _SubElementFieldData.metaClass = "metaClass";
      _SubElementFieldData.form = "form";
      _SubElementFieldData.allowOnlyExistingElements = "allowOnlyExistingElements";
      _SubElementFieldData.defaultTypesForNewElements = "defaultTypesForNewElements";
      _SubElementFieldData.includeSpecializationsForDefaultTypes = "includeSpecializationsForDefaultTypes";
      _SubElementFieldData.defaultWorkspaceOfNewElements = "defaultWorkspaceOfNewElements";
      _SubElementFieldData.defaultExtentOfNewElements = "defaultExtentOfNewElements";
      _SubElementFieldData.actionName = "actionName";
      _SubElementFieldData.isAttached = "isAttached";
      _SubElementFieldData._name_ = "name";
      _SubElementFieldData.title = "title";
      _SubElementFieldData.isEnumeration = "isEnumeration";
      _SubElementFieldData.defaultValue = "defaultValue";
      _SubElementFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._SubElementFieldData = _SubElementFieldData;
      _FieldTypes2.__SubElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData";
      class _TextFieldData2 {
      }
      _TextFieldData2.lineHeight = "lineHeight";
      _TextFieldData2.width = "width";
      _TextFieldData2.shortenTextLength = "shortenTextLength";
      _TextFieldData2.supportClipboardCopy = "supportClipboardCopy";
      _TextFieldData2.isAttached = "isAttached";
      _TextFieldData2._name_ = "name";
      _TextFieldData2.title = "title";
      _TextFieldData2.isEnumeration = "isEnumeration";
      _TextFieldData2.defaultValue = "defaultValue";
      _TextFieldData2.isReadOnly = "isReadOnly";
      _FieldTypes2._TextFieldData = _TextFieldData2;
      _FieldTypes2.__TextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData";
      class _EvalTextFieldData {
      }
      _EvalTextFieldData.evalCellProperties = "evalCellProperties";
      _EvalTextFieldData.lineHeight = "lineHeight";
      _EvalTextFieldData.width = "width";
      _EvalTextFieldData.shortenTextLength = "shortenTextLength";
      _EvalTextFieldData.supportClipboardCopy = "supportClipboardCopy";
      _EvalTextFieldData.isAttached = "isAttached";
      _EvalTextFieldData._name_ = "name";
      _EvalTextFieldData.title = "title";
      _EvalTextFieldData.isEnumeration = "isEnumeration";
      _EvalTextFieldData.defaultValue = "defaultValue";
      _EvalTextFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._EvalTextFieldData = _EvalTextFieldData;
      _FieldTypes2.__EvalTextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData";
      class _SeparatorLineFieldData {
      }
      _SeparatorLineFieldData.Height = "Height";
      _FieldTypes2._SeparatorLineFieldData = _SeparatorLineFieldData;
      _FieldTypes2.__SeparatorLineFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData";
      class _FileSelectionFieldData {
      }
      _FileSelectionFieldData.defaultExtension = "defaultExtension";
      _FileSelectionFieldData.isSaving = "isSaving";
      _FileSelectionFieldData.initialPathToDirectory = "initialPathToDirectory";
      _FileSelectionFieldData.filter = "filter";
      _FileSelectionFieldData.isAttached = "isAttached";
      _FileSelectionFieldData._name_ = "name";
      _FileSelectionFieldData.title = "title";
      _FileSelectionFieldData.isEnumeration = "isEnumeration";
      _FileSelectionFieldData.defaultValue = "defaultValue";
      _FileSelectionFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._FileSelectionFieldData = _FileSelectionFieldData;
      _FieldTypes2.__FileSelectionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData";
      class _AnyDataFieldData {
      }
      _AnyDataFieldData.isAttached = "isAttached";
      _AnyDataFieldData._name_ = "name";
      _AnyDataFieldData.title = "title";
      _AnyDataFieldData.isEnumeration = "isEnumeration";
      _AnyDataFieldData.defaultValue = "defaultValue";
      _AnyDataFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._AnyDataFieldData = _AnyDataFieldData;
      _FieldTypes2.__AnyDataFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData";
      class _CheckboxFieldData {
      }
      _CheckboxFieldData.isAttached = "isAttached";
      _CheckboxFieldData._name_ = "name";
      _CheckboxFieldData.title = "title";
      _CheckboxFieldData.isEnumeration = "isEnumeration";
      _CheckboxFieldData.defaultValue = "defaultValue";
      _CheckboxFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._CheckboxFieldData = _CheckboxFieldData;
      _FieldTypes2.__CheckboxFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData";
      class _ActionFieldData {
      }
      _ActionFieldData.actionName = "actionName";
      _ActionFieldData.parameter = "parameter";
      _ActionFieldData.buttonText = "buttonText";
      _ActionFieldData.bindingKey = "bindingKey";
      _ActionFieldData.bindingKeyModifierCtrl = "bindingKeyModifierCtrl";
      _ActionFieldData.bindingKeyModifierShift = "bindingKeyModifierShift";
      _ActionFieldData.bindingKeyModifierAlt = "bindingKeyModifierAlt";
      _ActionFieldData.isAttached = "isAttached";
      _ActionFieldData._name_ = "name";
      _ActionFieldData.title = "title";
      _ActionFieldData.isEnumeration = "isEnumeration";
      _ActionFieldData.defaultValue = "defaultValue";
      _ActionFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._ActionFieldData = _ActionFieldData;
      _FieldTypes2.__ActionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData";
      class _DateTimeFieldData {
      }
      _DateTimeFieldData.hideDate = "hideDate";
      _DateTimeFieldData.hideTime = "hideTime";
      _DateTimeFieldData.isAttached = "isAttached";
      _DateTimeFieldData._name_ = "name";
      _DateTimeFieldData.title = "title";
      _DateTimeFieldData.isEnumeration = "isEnumeration";
      _DateTimeFieldData.defaultValue = "defaultValue";
      _DateTimeFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._DateTimeFieldData = _DateTimeFieldData;
      _FieldTypes2.__DateTimeFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData";
      class _MergedFieldsInCellData {
      }
      _MergedFieldsInCellData.fields = "fields";
      _MergedFieldsInCellData.isAttached = "isAttached";
      _MergedFieldsInCellData._name_ = "name";
      _MergedFieldsInCellData.title = "title";
      _MergedFieldsInCellData.isEnumeration = "isEnumeration";
      _MergedFieldsInCellData.defaultValue = "defaultValue";
      _MergedFieldsInCellData.isReadOnly = "isReadOnly";
      _FieldTypes2._MergedFieldsInCellData = _MergedFieldsInCellData;
      _FieldTypes2.__MergedFieldsInCellData_Uri = "dm:///_internal/types/internal#b11ab14e-b620-486e-b795-fb559479674b";
      class _CompositeFieldData {
      }
      _CompositeFieldData.metaclass = "metaclass";
      _CompositeFieldData.isAttached = "isAttached";
      _CompositeFieldData._name_ = "name";
      _CompositeFieldData.title = "title";
      _CompositeFieldData.isEnumeration = "isEnumeration";
      _CompositeFieldData.defaultValue = "defaultValue";
      _CompositeFieldData.isReadOnly = "isReadOnly";
      _FieldTypes2._CompositeFieldData = _CompositeFieldData;
      _FieldTypes2.__CompositeFieldData_Uri = "dm:///_internal/types/internal#3769ef41-6cb7-4cc5-99e9-4de89198e644";
    })(_FieldTypes = _Forms2._FieldTypes || (_Forms2._FieldTypes = {}));
  })(_Forms || (_Forms = {}));
  var _AttachedExtent;
  (function(_AttachedExtent2) {
    class _AttachedExtentConfiguration {
    }
    _AttachedExtentConfiguration._name_ = "name";
    _AttachedExtentConfiguration.referencedWorkspace = "referencedWorkspace";
    _AttachedExtentConfiguration.referencedExtent = "referencedExtent";
    _AttachedExtentConfiguration.referenceType = "referenceType";
    _AttachedExtentConfiguration.referenceProperty = "referenceProperty";
    _AttachedExtent2._AttachedExtentConfiguration = _AttachedExtentConfiguration;
    _AttachedExtent2.__AttachedExtentConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration";
  })(_AttachedExtent || (_AttachedExtent = {}));
  var _Management;
  (function(_Management2) {
    let _ExtentLoadingState;
    (function(_ExtentLoadingState2) {
      _ExtentLoadingState2.Unknown = "Unknown";
      _ExtentLoadingState2.Unloaded = "Unloaded";
      _ExtentLoadingState2.Loaded = "Loaded";
      _ExtentLoadingState2.Failed = "Failed";
      _ExtentLoadingState2.LoadedReadOnly = "LoadedReadOnly";
    })(_ExtentLoadingState = _Management2._ExtentLoadingState || (_Management2._ExtentLoadingState = {}));
    let ___ExtentLoadingState;
    (function(___ExtentLoadingState2) {
      ___ExtentLoadingState2[___ExtentLoadingState2["Unknown"] = 0] = "Unknown";
      ___ExtentLoadingState2[___ExtentLoadingState2["Unloaded"] = 1] = "Unloaded";
      ___ExtentLoadingState2[___ExtentLoadingState2["Loaded"] = 2] = "Loaded";
      ___ExtentLoadingState2[___ExtentLoadingState2["Failed"] = 3] = "Failed";
      ___ExtentLoadingState2[___ExtentLoadingState2["LoadedReadOnly"] = 4] = "LoadedReadOnly";
    })(___ExtentLoadingState = _Management2.___ExtentLoadingState || (_Management2.___ExtentLoadingState = {}));
    class _Extent {
    }
    _Extent._name_ = "name";
    _Extent.uri = "uri";
    _Extent.workspaceId = "workspaceId";
    _Extent.count = "count";
    _Extent.totalCount = "totalCount";
    _Extent.type = "type";
    _Extent.extentType = "extentType";
    _Extent.isModified = "isModified";
    _Extent.alternativeUris = "alternativeUris";
    _Extent.autoEnumerateType = "autoEnumerateType";
    _Extent.state = "state";
    _Extent.failMessage = "failMessage";
    _Extent.properties = "properties";
    _Extent.loadingConfiguration = "loadingConfiguration";
    _Management2._Extent = _Extent;
    _Management2.__Extent_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent";
    class _Workspace {
    }
    _Workspace.id = "id";
    _Workspace.annotation = "annotation";
    _Workspace.extents = "extents";
    _Management2._Workspace = _Workspace;
    _Management2.__Workspace_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace";
    class _CreateNewWorkspaceModel {
    }
    _CreateNewWorkspaceModel.id = "id";
    _CreateNewWorkspaceModel.annotation = "annotation";
    _Management2._CreateNewWorkspaceModel = _CreateNewWorkspaceModel;
    _Management2.__CreateNewWorkspaceModel_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel";
    class _ExtentTypeSetting {
    }
    _ExtentTypeSetting._name_ = "name";
    _ExtentTypeSetting.rootElementMetaClasses = "rootElementMetaClasses";
    _Management2._ExtentTypeSetting = _ExtentTypeSetting;
    _Management2.__ExtentTypeSetting_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting";
    class _ExtentProperties {
    }
    _ExtentProperties._name_ = "name";
    _ExtentProperties.uri = "uri";
    _ExtentProperties.workspaceId = "workspaceId";
    _ExtentProperties.count = "count";
    _ExtentProperties.totalCount = "totalCount";
    _ExtentProperties.type = "type";
    _ExtentProperties.extentType = "extentType";
    _ExtentProperties.isModified = "isModified";
    _ExtentProperties.alternativeUris = "alternativeUris";
    _ExtentProperties.autoEnumerateType = "autoEnumerateType";
    _ExtentProperties.state = "state";
    _ExtentProperties.failMessage = "failMessage";
    _ExtentProperties.properties = "properties";
    _ExtentProperties.loadingConfiguration = "loadingConfiguration";
    _Management2._ExtentProperties = _ExtentProperties;
    _Management2.__ExtentProperties_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties";
    class _ExtentPropertyDefinition {
    }
    _ExtentPropertyDefinition._name_ = "name";
    _ExtentPropertyDefinition.title = "title";
    _ExtentPropertyDefinition.metaClass = "metaClass";
    _Management2._ExtentPropertyDefinition = _ExtentPropertyDefinition;
    _Management2.__ExtentPropertyDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition";
    class _ExtentSettings {
    }
    _ExtentSettings.extentTypeSettings = "extentTypeSettings";
    _ExtentSettings.propertyDefinitions = "propertyDefinitions";
    _Management2._ExtentSettings = _ExtentSettings;
    _Management2.__ExtentSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings";
  })(_Management || (_Management = {}));
  var _FastViewFilters;
  (function(_FastViewFilters2) {
    let _ComparisonType;
    (function(_ComparisonType2) {
      _ComparisonType2.Equal = "Equal";
      _ComparisonType2.GreaterThan = "GreaterThan";
      _ComparisonType2.LighterThan = "LighterThan";
      _ComparisonType2.GreaterOrEqualThan = "GreaterOrEqualThan";
      _ComparisonType2.LighterOrEqualThan = "LighterOrEqualThan";
    })(_ComparisonType = _FastViewFilters2._ComparisonType || (_FastViewFilters2._ComparisonType = {}));
    let ___ComparisonType;
    (function(___ComparisonType2) {
      ___ComparisonType2[___ComparisonType2["Equal"] = 0] = "Equal";
      ___ComparisonType2[___ComparisonType2["GreaterThan"] = 1] = "GreaterThan";
      ___ComparisonType2[___ComparisonType2["LighterThan"] = 2] = "LighterThan";
      ___ComparisonType2[___ComparisonType2["GreaterOrEqualThan"] = 3] = "GreaterOrEqualThan";
      ___ComparisonType2[___ComparisonType2["LighterOrEqualThan"] = 4] = "LighterOrEqualThan";
    })(___ComparisonType = _FastViewFilters2.___ComparisonType || (_FastViewFilters2.___ComparisonType = {}));
    class _PropertyComparisonFilter {
    }
    _PropertyComparisonFilter.Property = "Property";
    _PropertyComparisonFilter.ComparisonType = "ComparisonType";
    _PropertyComparisonFilter.Value = "Value";
    _FastViewFilters2._PropertyComparisonFilter = _PropertyComparisonFilter;
    _FastViewFilters2.__PropertyComparisonFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter";
    class _PropertyContainsFilter {
    }
    _PropertyContainsFilter.Property = "Property";
    _PropertyContainsFilter.Value = "Value";
    _FastViewFilters2._PropertyContainsFilter = _PropertyContainsFilter;
    _FastViewFilters2.__PropertyContainsFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter";
  })(_FastViewFilters || (_FastViewFilters = {}));
  var _DynamicRuntimeProvider;
  (function(_DynamicRuntimeProvider2) {
    class _DynamicRuntimeLoaderConfig {
    }
    _DynamicRuntimeLoaderConfig.runtimeClass = "runtimeClass";
    _DynamicRuntimeLoaderConfig.configuration = "configuration";
    _DynamicRuntimeLoaderConfig._name_ = "name";
    _DynamicRuntimeLoaderConfig.extentUri = "extentUri";
    _DynamicRuntimeLoaderConfig.workspaceId = "workspaceId";
    _DynamicRuntimeLoaderConfig.dropExisting = "dropExisting";
    _DynamicRuntimeProvider2._DynamicRuntimeLoaderConfig = _DynamicRuntimeLoaderConfig;
    _DynamicRuntimeProvider2.__DynamicRuntimeLoaderConfig_Uri = "dm:///_internal/types/internal#DynamicRuntimeProvider.DynamicRuntimeLoaderConfig";
    let _Examples;
    (function(_Examples2) {
      class _NumberProviderSettings {
      }
      _NumberProviderSettings._name_ = "name";
      _NumberProviderSettings.start = "start";
      _NumberProviderSettings.end = "end";
      _Examples2._NumberProviderSettings = _NumberProviderSettings;
      _Examples2.__NumberProviderSettings_Uri = "dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a";
      class _NumberRepresentation {
      }
      _NumberRepresentation.binary = "binary";
      _NumberRepresentation.octal = "octal";
      _NumberRepresentation.decimal = "decimal";
      _NumberRepresentation.hexadecimal = "hexadecimal";
      _Examples2._NumberRepresentation = _NumberRepresentation;
      _Examples2.__NumberRepresentation_Uri = "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation";
    })(_Examples = _DynamicRuntimeProvider2._Examples || (_DynamicRuntimeProvider2._Examples = {}));
  })(_DynamicRuntimeProvider || (_DynamicRuntimeProvider = {}));
  var _Verifier;
  (function(_Verifier2) {
    class _VerifyEntry {
    }
    _VerifyEntry.workspaceId = "workspaceId";
    _VerifyEntry.itemUri = "itemUri";
    _VerifyEntry.category = "category";
    _VerifyEntry.message = "message";
    _Verifier2._VerifyEntry = _VerifyEntry;
    _Verifier2.__VerifyEntry_Uri = "dm:///_internal/types/internal#d19d742f-9bba-4bef-b310-05ef96153768";
  })(_Verifier || (_Verifier = {}));

  // ../DatenMeister.WebServer/Assets/js/datenmeister/modules/QueryEngine.js
  var QueryBuilder = class {
    constructor() {
      this.queryStatement = new DmObject(_DataViews.__QueryStatement_Uri);
    }
    addNode(node) {
      this.queryStatement.appendToArray(_DataViews._QueryStatement.nodes, node);
    }
    setResultNode(node) {
      this.queryStatement.set(_DataViews._QueryStatement.resultNode, DmObject.createAsReferenceFromLocalId(node));
    }
    getResultNode() {
      return this.queryStatement.get(_DataViews._QueryStatement.resultNode, ObjectType.Object);
    }
  };
  function createForReferenceExistingNode(workspaceId, nodeUri) {
    const viewNode = new DmObject(_DataViews._Node.__ReferenceViewNode_Uri);
    viewNode.set(_DataViews._Node._ReferenceViewNode.workspaceId, workspaceId);
    viewNode.set(_DataViews._Node._ReferenceViewNode.itemUri, nodeUri);
    viewNode.set(_DataViews._Node._ReferenceViewNode._name_, "Reference to " + nodeUri);
    return viewNode;
  }
  function referenceExistingNode(builder, workspaceId, nodeUri) {
    const viewNode = createForReferenceExistingNode(workspaceId, nodeUri);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
  }
  function createForOrderByProperty(input, property, descending) {
    const viewNode = new DmObject(_DataViews._Row.__RowOrderByNode_Uri);
    viewNode.set(_DataViews._Row._RowOrderByNode.input, DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DataViews._Row._RowOrderByNode.propertyName, property);
    viewNode.set(_DataViews._Row._RowOrderByNode.orderDescending, descending);
    viewNode.set(_DataViews._Row._RowOrderByNode._name_, "Order by " + property + (descending ? " descending" : " ascending"));
    return viewNode;
  }
  function orderByProperty(builder, property, descending = false) {
    const viewNode = createForOrderByProperty(builder.getResultNode(), property, descending);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
  }
  function createForLimit(input, limit2) {
    const viewNode = new DmObject(_DataViews._Row.__RowFilterOnPositionNode_Uri);
    viewNode.set(_DataViews._Row._RowFilterOnPositionNode.input, DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DataViews._Row._RowFilterOnPositionNode.amount, limit2);
    viewNode.set(_DataViews._Row._RowFilterOnPositionNode._name_, "Limit to " + limit2 + " elements");
    return viewNode;
  }
  function limit(builder, limitValue) {
    const viewNode = createForLimit(builder.getResultNode(), limitValue);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
  }
  function createForFilterByFreetext(input, freeText, propertyName) {
    const viewNode = new DmObject(_DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
    viewNode.set(_DataViews._Row._RowFilterByFreeTextAnywhere.input, DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DataViews._Row._RowFilterByFreeTextAnywhere.freeText, freeText);
    viewNode.set(_DataViews._Row._RowFilterByFreeTextAnywhere._name_, "Filter by free text " + freeText);
    if (propertyName !== void 0) {
      viewNode.set(_DataViews._Row._RowFilterByFreeTextAnywhere.propertyName, propertyName);
    }
    return viewNode;
  }
  function filterByFreetext(builder, freeText, propertyName) {
    const viewNode = createForFilterByFreetext(builder.getResultNode(), freeText, propertyName);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
  }
  function createForFilterByProperty(input, property, value, comparisonMode) {
    const viewNode = new DmObject(_DataViews._Row.__RowFilterByPropertyValueNode_Uri);
    viewNode.set(_DataViews._Row._RowFilterByPropertyValueNode.input, DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DataViews._Row._RowFilterByPropertyValueNode.property, property);
    viewNode.set(_DataViews._Row._RowFilterByPropertyValueNode.value, value);
    viewNode.set(_DataViews._Row._RowFilterByPropertyValueNode.comparisonMode, comparisonMode ?? _DataViews._ComparisonMode.Equal);
    viewNode.set(_DataViews._Row._RowFilterByPropertyValueNode._name_, "Filter by property " + property + " with value '" + value + "' (" + comparisonMode + ")");
    return viewNode;
  }
  function createForFlatten(input) {
    const viewNode = new DmObject(_DataViews._Row.__RowFlattenNode_Uri);
    viewNode.set(_DataViews._Row._RowFlattenNode.input, DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DataViews._Row._RowFlattenNode._name_, "Flatten");
    return viewNode;
  }
  function flatten(builder) {
    const viewNode = createForFlatten(builder.getResultNode());
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
  }
  function createForAddDynamicSource(name) {
    const dynamicSource = new DmObject(_DataViews._Source.__DynamicSourceNode_Uri);
    dynamicSource.set(_DataViews._Source._DynamicSourceNode.nodeName, name);
    dynamicSource.set(_DataViews._Source._DynamicSourceNode._name_, "Dynamic source " + name);
    return dynamicSource;
  }
  function addDynamicSource(builder, name) {
    const dynamicSource = createForAddDynamicSource(name);
    builder.addNode(dynamicSource);
    builder.setResultNode(dynamicSource);
    return dynamicSource;
  }
  function createForGetElementsOfWorkspace(workspaceId) {
    const viewNode = new DmObject(_DataViews._Source.__SelectByWorkspaceNode_Uri);
    viewNode.set(_DataViews._Source._SelectByWorkspaceNode.workspaceId, workspaceId);
    viewNode.set(_DataViews._Source._SelectByWorkspaceNode._name_, "Select by workspace " + workspaceId);
    return viewNode;
  }
  function getElementsOfWorkspace(builder, workspaceId) {
    const viewNode = createForGetElementsOfWorkspace(workspaceId);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
  }
  function createForGetElementsOfExtent(workspaceId, extentUrl) {
    const viewNode = new DmObject(_DataViews._Source.__SelectByExtentNode_Uri);
    viewNode.set(_DataViews._Source._SelectByExtentNode.workspaceId, workspaceId);
    viewNode.set(_DataViews._Source._SelectByExtentNode.extentUri, extentUrl);
    viewNode.set(_DataViews._Source._SelectByExtentNode._name_, "Select by extent " + extentUrl);
    return viewNode;
  }
  function getElementsOfExtent(builder, workspaceId, extentUrl) {
    const viewNode = createForGetElementsOfExtent(workspaceId, extentUrl);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
  }
  function createForGetElementsByPath(workspaceId, path) {
    const viewNode = new DmObject(_DataViews._Source.__SelectByPathNode_Uri);
    viewNode.set(_DataViews._Source._SelectByPathNode.workspaceId, workspaceId);
    viewNode.set(_DataViews._Source._SelectByPathNode.path, path);
    viewNode.set(_DataViews._Source._SelectByPathNode._name_, "Select by path " + path);
    return viewNode;
  }
  function getElementsByPath(builder, workspaceId, path) {
    const viewNode = createForGetElementsByPath(workspaceId, path);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/controls/SelectItemControlBySearch.js
  var ControlSettings2 = class {
    constructor() {
      this.maxItemsPerSearch = 50;
    }
  };
  var SelectItemControlBySearch = class {
    constructor() {
      this.itemClicked = new UserEvent();
      this.workspaceAndExtent = new SelectItemControlHelperForWorkspaceAndExtent(this);
      this.lastLoadIndex = 0;
    }
    init(parent, settings) {
      const div = this.initDom(settings ?? new ControlSettings2(), parent);
      const _ = this.workspaceAndExtent.loadWorkspaces();
      return div;
    }
    async initAsync(parent, settings) {
      const div = this.initDom(settings ?? new ControlSettings2(), parent);
      await this.workspaceAndExtent.loadWorkspaces();
      return div;
    }
    /**
     * This method just creates the DOM and connects the events of the elements to the
     * invocation methods
     * @param settings Settings to be used
     * @param container JQuery-Container Element hosting the content
     * @private
     */
    initDom(settings, container) {
      this.settings = settings;
      const tthis = this;
      const div = $("<div class='dm-sic-search'><table><tr><th><span>Workspace:</span></th><td><div class='dm-sic-workspace'></div></td></tr><tr><th><span>Extent:</span></th><td><div class='dm-sic-extent'></div></td></tr><tr><th><span>Search text:</span></th><td><div class='dm-sic-search-input'><input type='text' /></div></td></tr><tr><td colspan='2'><div class='dm-sic-search-results'></div></td></tr></table></div>");
      this.workspaceAndExtent.initialize(div);
      this.inputBoxDiv = $(".dm-sic-search-input input", div);
      this.resultsDiv = $(".dm-sic-search-results", div);
      this.inputBoxDiv.on("input", () => tthis.onInputChanged());
      container.append(div);
      this.containerDiv = div;
      return div;
    }
    getUserSelectedWorkspaceId() {
      return this.workspaceAndExtent.getUserSelectedWorkspaceId();
    }
    getUserSelectedExtentUri() {
      return this.workspaceAndExtent.getUserSelectedExtentUri();
    }
    async onWorkspaceSelected(workspaceId) {
      this.inputBoxDiv.trigger("focus");
      this.inputBoxDiv.val("");
    }
    async onExtentSelected(workspaceId, extentUri) {
      this.inputBoxDiv.trigger("focus");
      this.inputBoxDiv.val("");
    }
    showControl() {
      this.containerDiv.show();
    }
    hideControl() {
      this.containerDiv.hide();
    }
    getSelectedItem() {
      return this.selectedItem;
    }
    async onInputChanged() {
      const tthis = this;
      this.lastLoadIndex++;
      const loadIndex = this.lastLoadIndex;
      const selectWorkspace = this.getUserSelectedWorkspaceId();
      const text = this.inputBoxDiv.val()?.toString();
      if (text === "" || text === null || text === void 0) {
        this.resultsDiv.empty();
        this.resultsDiv.hide();
      } else if (selectWorkspace === void 0 || selectWorkspace === "") {
        this.resultsDiv.empty();
        this.resultsDiv.append("<div>Please select a workspace</div>");
        this.resultsDiv.show();
      } else {
        const extentName = this.getUserSelectedExtentUri();
        const queryBuilder = new QueryBuilder();
        if (extentName === void 0 || extentName === null || extentName === "") {
          getElementsOfWorkspace(queryBuilder, selectWorkspace);
        } else {
          getElementsOfExtent(queryBuilder, selectWorkspace, extentName);
        }
        flatten(queryBuilder);
        filterByFreetext(queryBuilder, text, "name");
        orderByProperty(queryBuilder, "name");
        limit(queryBuilder, this.settings.maxItemsPerSearch + 1);
        const result = await queryObject(queryBuilder.queryStatement);
        if (this.lastLoadIndex == loadIndex) {
          this.resultsDiv.empty();
          let found = 0;
          for (const n in result.result) {
            if (!result.result.hasOwnProperty(n))
              continue;
            if (found >= this.settings.maxItemsPerSearch) {
              const itemDiv2 = $("<div class='more'><em>... and more ...</em></div>");
              this.resultsDiv.append(itemDiv2);
              break;
            }
            found++;
            const item = result.result[n];
            const itemDiv = $("<div>" + item.get("name", ObjectType.String) + "</div>");
            const _ = injectNameByObject(itemDiv, item, {
              inhibitItemLink: true
            });
            ((innerItem) => itemDiv.on("click", () => {
              if (this.lastSelectedDiv !== void 0) {
                tthis.lastSelectedDiv.removeClass("selected");
              }
              itemDiv.addClass("selected");
              tthis.lastSelectedDiv = itemDiv;
              tthis.selectedItem = innerItem;
              tthis.itemClicked.invoke(innerItem);
            }))(item);
            this.resultsDiv.append(itemDiv);
          }
        }
        this.resultsDiv.show();
      }
    }
    async setExtentByUri(workspaceId, extentUri) {
      await this.workspaceAndExtent.setExtentByUri(workspaceId, extentUri);
    }
    async setWorkspaceById(workspaceId) {
      await this.workspaceAndExtent.setWorkspaceById(workspaceId);
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/controls/SelectItemControl.js
  var ContainerSettings = class {
    constructor() {
      this.headline = void 0;
      this.hideAtStartup = false;
      this.browseSettings = new ControlSettings();
      this.searchSettings = new ControlSettings2();
      this.showCancelButton = true;
      this.hideButtonRow = false;
      this.setButtonText = "Set";
    }
  };
  var SelectItemControl = class {
    constructor() {
      this.byBrowseControl = new SelectItemControlByBrowsingControl();
      this.bySearchControl = new SelectItemControlBySearch();
      this.itemSelected = new UserEvent();
      this.itemClicked = new UserEvent();
      this.selectionCancelled = new UserEvent();
      this.currentTab = "byBrowse";
    }
    /**
     * Async variant of {@link init}. Renders the control into `parent` and
     * resolves the returned promise once the workspace list (and any
     * pre-selected extent/item) has been loaded and the GUI reflects it.
     *
     * @param parent The container element that should host the control.
     * @param settings Optional behavior overrides; see {@link ControlSettings}.
     * @returns A promise resolving to the root `<table>` of the rendered control.
     */
    async initAsync(parent, settings) {
      return await this.initDom(settings, parent);
    }
    /**
     * This method just creates the DOM and connects the events of the elements to the
     * invocation methods
     * @param settings Settings to be used
     * @param container JQuery-Container Element hosting the content
     * @private
     */
    async initDom(settings, container) {
      this.settings = settings ?? new ContainerSettings();
      this.containerDiv = container;
      const div = $("<div class='dm-selectitemcontrol'><div class='dm-selectitemcontrol-headline'>Select item:</div><div class='dm-selectitemcontrol-tabs'><div class='dm-selectitemcontrol-tab-bybrowse'><button>Browse</button></div><div class='dm-selectitemcontrol-tab-search'><button>Search</button></div></div><div class='dm-selectitemcontrol-bybrowse'></div><div class='dm-selectitemcontrol-search'></div>" + (this.settings.hideButtonRow !== true ? "<div class='dm-selectitemcontrol-buttons'>" + (this.settings.showCancelButton ? "<button class='btn btn-secondary dm-sic-cancelbtn' type='button'>Cancel</button>" : "") + "<button class='btn btn-primary dm-sic-button' type='button'>Set</button></div>" : "") + "</div>");
      this.containerDiv.append(div);
      const setButton = $(".dm-sic-button", div);
      const cancelButton = $(".dm-sic-cancelbtn", div);
      const byBrowseButton = $(".dm-selectitemcontrol-tab-bybrowse button", div);
      const bySearchButton = $(".dm-selectitemcontrol-tab-search button", div);
      this.byBrowserControlDiv = $(".dm-selectitemcontrol-bybrowse", div);
      this.bySearchControlDiv = $(".dm-selectitemcontrol-search", div);
      byBrowseButton.on("click", () => {
        this.updateTabStatus("byBrowse");
      });
      bySearchButton.on("click", () => {
        this.updateTabStatus("bySearch");
      });
      setButton.text(this.settings.setButtonText);
      setButton.on("click", () => {
        const selectedItem = this.getSelectedItem();
        if (selectedItem !== void 0) {
          tthis.itemSelected.invoke(selectedItem);
        }
      });
      cancelButton.on("click", () => {
        this.selectionCancelled.invoke();
      });
      this.selectionCancelled.addListener(() => tthis.removeControl());
      if (this.settings.headline !== void 0) {
        $(".dm-selectitemcontrol-headline", div).text(this.settings.headline);
      }
      const byBrowseDiv = $(".dm-selectitemcontrol-bybrowse", div);
      await this.byBrowseControl.initAsync(byBrowseDiv, this.settings.browseSettings);
      const tthis = this;
      this.byBrowseControl.itemClicked.addListener((x) => tthis.itemClicked.invoke(x));
      this.bySearchControl.itemClicked.addListener((x) => tthis.itemClicked.invoke(x));
      const bySearchDiv = $(".dm-selectitemcontrol-search", div);
      await this.bySearchControl.initAsync(bySearchDiv, this.settings.searchSettings);
      await this.updateTabStatus();
      if (settings?.hideAtStartup) {
        this.hideControl();
      }
      return div;
    }
    async updateTabStatus(nextTab) {
      const currentWorkspace = this.getCurrentlySelectedWorkspace();
      const currentExtentUri = this.getCurrentlySelectedExtentUri();
      if (nextTab !== void 0) {
        this.currentTab = nextTab;
      }
      switch (this.currentTab) {
        case "byBrowse":
          this.bySearchControlDiv.hide();
          this.byBrowserControlDiv.show();
          if (currentWorkspace !== void 0 && currentWorkspace !== "" && currentWorkspace !== null) {
            if (currentExtentUri !== void 0 && currentExtentUri !== "" && currentExtentUri !== null) {
              await this.byBrowseControl.setExtentByUri(currentWorkspace, currentExtentUri);
            } else {
              await this.byBrowseControl.setWorkspaceById(currentWorkspace);
            }
          }
          break;
        case "bySearch":
          this.byBrowserControlDiv.hide();
          this.bySearchControlDiv.show();
          if (currentWorkspace !== void 0 && currentWorkspace !== "" && currentWorkspace !== null) {
            if (currentExtentUri !== void 0 && currentExtentUri !== "" && currentExtentUri !== null) {
              await this.bySearchControl.setExtentByUri(currentWorkspace, currentExtentUri);
            } else {
              await this.bySearchControl.setWorkspaceById(currentWorkspace);
            }
          }
          break;
      }
    }
    async setWorkspaceById(workspaceId) {
      switch (this.currentTab) {
        case "byBrowse":
          await this.byBrowseControl.setWorkspaceById(workspaceId);
          break;
        case "bySearch":
          await this.bySearchControl.setWorkspaceById(workspaceId);
          break;
      }
    }
    async setExtentByUri(workspaceId, extentUri) {
      switch (this.currentTab) {
        case "byBrowse":
          await this.byBrowseControl.setExtentByUri(workspaceId, extentUri);
          break;
        case "bySearch":
          await this.bySearchControl.setExtentByUri(workspaceId, extentUri);
          break;
      }
    }
    async setItemByUri(workspaceId, itemUri) {
      await this.byBrowseControl.setItemByUri(workspaceId, itemUri);
    }
    getSelectedItem() {
      switch (this.currentTab) {
        case "byBrowse":
          return this.byBrowseControl.getSelectedItem();
        case "bySearch":
          return this.bySearchControl.getSelectedItem();
      }
    }
    getCurrentlySelectedWorkspace() {
      switch (this.currentTab) {
        case "byBrowse":
          return this.byBrowseControl.getUserSelectedWorkspaceId();
        case "bySearch":
          return this.bySearchControl.getUserSelectedWorkspaceId();
      }
    }
    getCurrentlySelectedExtentUri() {
      switch (this.currentTab) {
        case "byBrowse":
          return this.byBrowseControl.getUserSelectedExtentUri();
        case "bySearch":
          return this.bySearchControl.getUserSelectedExtentUri();
      }
    }
    showControl() {
      if (this.containerDiv !== void 0) {
        this.containerDiv.show();
      }
    }
    hideControl() {
      if (this.containerDiv !== void 0) {
        this.containerDiv.hide();
      }
    }
    /**
     * Removes the control. This means that 'init(Async)' must be called again
     */
    removeControl() {
      this.containerDiv?.remove();
      this.containerDiv = void 0;
    }
  };
  function selectPackage(changeContainerCell) {
    return selectItem(changeContainerCell, { workspaceId: WorkspaceData, title: "Select Package in which the item shall be created:" });
  }
  function selectType(changeContainerCell) {
    return selectItem(changeContainerCell, { workspaceId: WorkspaceTypes, title: "Select type of new item:" });
  }
  function selectItem(changeContainerCell, parameter) {
    return new Promise(async (resolve2, reject) => {
      const workspaceId = parameter.workspaceId;
      if (workspaceId === void 0) {
        reject("WorkspaceId is undefined");
        return;
      }
      const title = parameter.title;
      changeContainerCell.empty();
      const selectItem2 = new SelectItemControl();
      const settings = new ContainerSettings();
      settings.browseSettings.showWorkspaceInBreadcrumb = true;
      settings.browseSettings.showExtentInBreadcrumb = true;
      if (title !== void 0)
        settings.headline = title;
      await selectItem2.setWorkspaceById(workspaceId);
      selectItem2.itemSelected.addListener((selectedItem) => {
        if (selectedItem === void 0 || selectedItem.uri === void 0) {
          alert("Nothing is selected.");
          reject("Nothing is selected");
          return;
        }
        resolve2({
          workspace: selectedItem.workspace,
          uri: selectedItem.uri
        });
      });
      await selectItem2.initAsync(changeContainerCell, settings);
    });
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/Interfaces.js
  var BaseField = class {
    get configuration() {
      return this.context.configuration;
    }
    get field() {
      return this.context.field;
    }
    get isReadOnly() {
      return this.context.isReadOnly;
    }
    get form() {
      return this.context.form;
    }
    get itemUrl() {
      return this.context.itemUrl ?? "";
    }
    constructor(context) {
      this.context = context;
    }
    async evaluateDom(dmElement) {
      return;
    }
    /**
     * Gets a value indicating whether the value of the field is shown.
     * Default implementation returns true.
     * @returns True
     */
    showValue() {
      return true;
    }
    /**
     * This callback will be called in case of the content of the field is changed
     * This allows creator of a field to explicitly react upon changes within the field
     * Default implementation does nothing.
     */
    callbackUpdateField() {
      return;
    }
  };

  // ../DatenMeister.WebServer/Assets/js/burnsystems/StringManipulation.js
  function truncateText(value, parameter) {
    let added = false;
    if (parameter?.maxLength !== void 0) {
      if (parameter.maxLength > 0 && value.length > parameter.maxLength) {
        value = value.substring(0, parameter.maxLength);
        added = true;
        if (parameter?.useWordBoundary === true) {
          value = value.slice(0, value.lastIndexOf(" "));
        }
      }
    }
    if (parameter?.maxLines !== void 0 && parameter.maxLines > 0) {
      let lines = value.split("\n");
      if (lines.length > parameter.maxLines) {
        value = lines.splice(0, parameter.maxLines).join("\n").trim();
        value += "\n";
        added = true;
      }
    }
    if (added) {
      const ellipses = parameter?.truncateEllipsis === void 0 ? " \u2026" : parameter.truncateEllipsis;
      value += ellipses;
    }
    return value;
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/TextField.js
  var _TextFieldData = _Forms._FieldTypes._TextFieldData;
  var Field = class extends BaseField {
    constructor(context) {
      super(context);
    }
    async createDom(dmElement) {
      const fieldName = this.field.get("name")?.toString() ?? "";
      let value = dmElement.get(fieldName, ObjectType.String) ?? "";
      const originalValue = value;
      if (this.form.tableParameter?.shortenFullText === true) {
        value = truncateText(value, {
          useWordBoundary: true,
          maxLines: 3,
          maxLength: 100
        });
      } else {
        const shortenTextLength = this.field.get(_Forms._FieldTypes._TextFieldData.shortenTextLength, ObjectType.Number);
        if (shortenTextLength !== void 0 && shortenTextLength > 0) {
          value = truncateText(value, { useWordBoundary: true, maxLength: shortenTextLength });
        }
      }
      if (Array.isArray(value)) {
        let enumeration = $("<ul class='list-unstyled'></ul>");
        for (let m in value) {
          if (Object.prototype.hasOwnProperty.call(value, m)) {
            let innerValue = value[m];
            let item = $("<li></li>");
            item.text(getName(innerValue));
            enumeration.append(item);
          }
        }
        return enumeration;
      }
      if (this.isReadOnly) {
        const divContainer = $("<div class='dm-textfield-container'></div>");
        if (this.field.get(_Forms._FieldTypes._TextFieldData.supportClipboardCopy, ObjectType.Boolean)) {
          const button = $("<button class='btn btn-secondary'>Copy to Clipboard</button>");
          button.on("click", () => {
            if (navigator.clipboard !== void 0 && false) {
              navigator.clipboard.writeText(originalValue);
              alert("Text copied to clipboard");
            } else {
              const divBox = $("<div class='dm-textfield-clipboard-container'></div>");
              const infoText = $("<div class='dm-textfield-clipboard-info'>Copy to clipboard is not <a href='https://developer.mozilla.org/en-US/docs/Web/API/Window/isSecureContext' target='_blank'>supported in your browser</a>. Please copy the text manually.</div>");
              divBox.append(infoText);
              const textbox = $("<textarea type='text' readonly class='dm-textfield-clipboard'/>");
              textbox.val(originalValue);
              divBox.append(textbox);
              divContainer.append(divBox);
            }
          });
          divContainer.append(button);
        }
        const div = $("<div class='dm-textfield'/>");
        if (value === void 0) {
          div.append($("<em class='dm-undefined'>undefined</em>"));
        } else {
          div.text(value ?? "undefined");
        }
        if (fieldName === "name") {
          let _ = injectNameByUri(div, dmElement.workspace, dmElement.uri, {});
        }
        divContainer.append(div);
        return divContainer;
      } else {
        const lineHeight = this.field.get(_TextFieldData.lineHeight, ObjectType.Number);
        const width = this.field.get(_TextFieldData.width, ObjectType.Number);
        if (lineHeight === void 0 || Number.isNaN(lineHeight) || lineHeight <= 1) {
          this._textBox = $("<input class='dm-textfield' />");
          if (width !== void 0 && !Number.isNaN(width) && width > 0) {
            this._textBox.attr("size", width);
          } else {
            this._textBox.attr("size", 70);
          }
        } else {
          this._textBox = $("<textarea class='dm-textfield' />");
          this._textBox.attr("rows", lineHeight);
          if (width !== void 0 && !Number.isNaN(width) && width > 0) {
            this._textBox.attr("cols", width);
          } else {
            this._textBox.attr("cols", 80);
          }
        }
        this._textBox.val(value);
        this._textBox.on("input change", () => {
          if (this.callbackUpdateField !== void 0) {
            this.callbackUpdateField();
          }
        });
        return this._textBox;
      }
    }
    async evaluateDom(dmElement) {
      if (this._textBox !== void 0 && this._textBox !== null) {
        let fieldName;
        if (this.OverridePropertyValue === void 0) {
          fieldName = this.field.get("name").toString();
        } else {
          fieldName = this.OverridePropertyValue();
        }
        dmElement.set(fieldName, this._textBox.val());
      }
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/CheckboxField.js
  var Field2 = class extends BaseField {
    constructor(context) {
      super(context);
    }
    async createDom(dmElement) {
      this._checkbox = $("<input type='checkbox'/>");
      const fieldName = this.field.get("name").toString();
      if (dmElement.get(fieldName, ObjectType.Boolean)) {
        this._checkbox.prop("checked", true);
      }
      if (this.isReadOnly) {
        this._checkbox.prop("disabled", "disabled");
      }
      this._checkbox.on("change", () => {
        if (this.callbackUpdateField !== void 0) {
          this.callbackUpdateField();
        }
      });
      return this._checkbox;
    }
    async evaluateDom(dmElement) {
      if (this._checkbox !== void 0 && this._checkbox !== null) {
        const fieldName = this.field.get("name").toString();
        dmElement.set(fieldName, this._checkbox.prop("checked"));
      }
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/CheckboxListTaggingField.js
  var Field3 = class extends BaseField {
    constructor(context) {
      super(context);
      this.checkboxes = new Array();
    }
    async createDom(dmElement) {
      this.name = this.field.get(_Forms._FieldTypes._CheckboxListTaggingFieldData._name_, ObjectType.String);
      this.separator = this.field.get(_Forms._FieldTypes._CheckboxListTaggingFieldData.separator, ObjectType.String);
      if (this.separator === "" || this.separator === void 0) {
        this.separator = ",";
      }
      const containsFreeText = this.field.get(_Forms._FieldTypes._CheckboxListTaggingFieldData.containsFreeText, ObjectType.Boolean);
      const valuePairs = this.field.get(_Forms._FieldTypes._CheckboxListTaggingFieldData.values, ObjectType.Array);
      this.isFieldReadOnly = this.field.get(_Forms._FieldTypes._CheckboxListTaggingFieldData.isReadOnly, ObjectType.Boolean);
      const currentValue = dmElement.get(this.name, ObjectType.String) ?? "";
      const currentList = currentValue.split(this.separator);
      const result = $("<table></table>");
      for (let n in valuePairs) {
        if (!Object.prototype.hasOwnProperty.call(valuePairs, n))
          continue;
        const valuePair = valuePairs[n];
        const valueName = valuePair.get(_Forms._FieldTypes._ValuePair._name_, ObjectType.String);
        const valueContent = valuePair.get(_Forms._FieldTypes._ValuePair.value, ObjectType.String);
        const checkbox = $("<input type='checkbox' />");
        checkbox.attr("name", valueName);
        checkbox.attr("data-value", valueContent);
        checkbox.on("change", () => {
          if (this.callbackUpdateField !== void 0) {
            this.callbackUpdateField();
          }
        });
        if (this.isFieldReadOnly || this.isReadOnly) {
          checkbox.attr("disabled", "disabled");
        }
        const label = $("<label></label>");
        label.text(valueName);
        const index = currentList.indexOf(valueContent);
        if (index !== -1) {
          checkbox.attr("checked", "checked");
          currentList.splice(index, 1);
        }
        this.checkboxes.push(checkbox);
        const row = $("<tr><td></td></tr>");
        $("td", row).append(checkbox);
        $("td", row).append(label);
        result.append(row);
      }
      if (containsFreeText) {
        this.freeText = $("<input type='text' />");
        if (this.isFieldReadOnly || this.isReadOnly) {
          this.freeText.attr("disabled", "disabled");
        }
        let freeTextText = "";
        let komma = "";
        for (let n in currentList) {
          const listItem = currentList[n];
          freeTextText += komma;
          freeTextText += listItem;
          komma = ",";
        }
        this.freeText.val(freeTextText);
        const rowOptions = $("<tr><td class='small'>Other:</td></tr>");
        $("td", rowOptions).append(this.freeText);
        result.append(rowOptions);
        const row = $("<tr><td></td></tr>");
        $("td", row).append(this.freeText);
        result.append(row);
      }
      return result;
    }
    /**
     * Evaluates the user input and checks whether the data can be correctly set
     * @param dmElement Element to which the data shall be added
     */
    async evaluateDom(dmElement) {
      if (this.isReadOnly || this.isFieldReadOnly)
        return;
      let result = "";
      let komma = "";
      for (let n in this.checkboxes) {
        const checkbox = this.checkboxes[n];
        if (checkbox.prop("checked")) {
          result += komma;
          result += checkbox.attr("data-value");
          komma = this.separator;
        }
      }
      if (this.freeText !== void 0) {
        result += komma;
        result += this.freeText.val();
      }
      dmElement.set(this.name, result);
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/CompositeField.js
  var Field4 = class extends BaseField {
    constructor(context) {
      super(context);
      this.reloadValuesFromServer = async () => {
        const valueDom = $(".dm-field-composite-value", this.resultingDom);
        const buttonsDom = $(".dm-field-composite-buttons", this.resultingDom);
        valueDom.empty();
        buttonsDom.empty();
        const propertyName = this.field.get("name");
        let value = await getProperty(this.element.workspace, this.element.uri, propertyName);
        if (Array.isArray(value)) {
          value = value[0];
        }
        let isUndefined = false;
        if (value === void 0 || value === null) {
          valueDom.append($("<em class='dm-undefined'>undefined</em>"));
          isUndefined = true;
        } else {
          let _ = injectNameByUri(valueDom, value.workspace, value.uri);
        }
        const tthis = this;
        if (!this.isReadOnly) {
          if (isUndefined) {
            const createButton = $("<btn class='btn btn-secondary'>Create</btn>");
            createButton.on("click", async () => {
              const containerDiv = $(".dm-compositefield-attachitem-box", this.resultingDom);
              containerDiv.empty();
              const selectItem2 = new SelectItemControl();
              const settings = new ContainerSettings();
              settings.browseSettings.showWorkspaceInBreadcrumb = true;
              settings.browseSettings.showExtentInBreadcrumb = true;
              await selectItem2.setWorkspaceById(WorkspaceTypes);
              selectItem2.itemSelected.addListener((selectedItem) => {
                if (selectedItem === void 0 || selectedItem.uri === void 0) {
                  alert("Nothing is selected.");
                  return;
                }
                document.location.href = getLinkForNavigateToCreateItemInProperty(tthis.form.workspace, tthis.itemUrl, propertyName, selectedItem.uri, selectedItem.workspace, false);
              });
              await selectItem2.initAsync(containerDiv, settings);
              return false;
            });
            buttonsDom.append(createButton);
          } else {
            const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
            unsetCell.on("click", () => {
              unsetProperty(tthis.form.workspace, tthis.itemUrl, propertyName).then(async () => {
                await tthis.reloadValuesFromServer();
              });
            });
            buttonsDom.append(unsetCell);
          }
        }
      };
    }
    async createDom(dmElement) {
      if (this.configuration?.isNewItem) {
        return $("<div><em>New item, content can only be set after saving</em></div>");
      }
      this.element = dmElement;
      this.resultingDom = $('<div class="dm-field-composite">  <div class="dm-field-composite-value"></div>  <div class="dm-field-composite-button-container">    <div class="dm-field-composite-buttons"></div>    <div class="dm-compositefield-attachitem-box"></div>  </div></div>');
      await this.reloadValuesFromServer();
      return this.resultingDom;
    }
    async evaluateDom(dmElement) {
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/DateTimeField.js
  var Field5 = class extends BaseField {
    constructor(context) {
      super(context);
    }
    async createDom(dmElement) {
      const fieldName = this.field.get("name")?.toString() ?? "";
      const hideDate = this.field.get(_Forms._FieldTypes._DateTimeFieldData.hideDate, ObjectType.Boolean) ?? false;
      const hideTime = this.field.get(_Forms._FieldTypes._DateTimeFieldData.hideTime, ObjectType.Boolean) ?? false;
      const value = dmElement.get(fieldName, ObjectType.String) ?? "";
      let dateObj = null;
      if (value) {
        if (value.indexOf("T") !== -1 && value.indexOf("Z") === -1 && value.indexOf("+") === -1) {
          dateObj = /* @__PURE__ */ new Date(value + "Z");
        } else {
          dateObj = new Date(value);
        }
        if (isNaN(dateObj.getTime())) {
          dateObj = null;
        }
      }
      const pad = (n) => n < 10 ? "0" + n : n;
      if (this.isReadOnly) {
        const container2 = $("<div />");
        if (!dateObj) {
          container2.append($("<em class='dm-undefined'>undefined</em>"));
          return container2;
        }
        let text = "";
        if (!hideDate) {
          text += `${dateObj.getFullYear()}-${pad(dateObj.getMonth() + 1)}-${pad(dateObj.getDate())}`;
        }
        if (!hideTime) {
          if (text !== "")
            text += " ";
          text += `${pad(dateObj.getHours())}:${pad(dateObj.getMinutes())}`;
        }
        container2.text(text);
        return container2;
      }
      const container = $("<div />");
      if (!hideDate) {
        const dateContainer = $("<div class='d-flex align-items-center' />");
        dateContainer.append($("<span class='me-1'>Date: </span>"));
        this._dateInput = $("<input type='date' class='form-control' />");
        if (dateObj) {
          this._dateInput.val(`${dateObj.getFullYear()}-${pad(dateObj.getMonth() + 1)}-${pad(dateObj.getDate())}`);
        }
        dateContainer.append(this._dateInput);
        container.append(dateContainer);
      }
      if (!hideDate && !hideTime) {
        container.append($("<br/>"));
      }
      if (!hideTime) {
        const timeContainer = $("<div class='d-flex align-items-center' />");
        timeContainer.append($("<span class='me-1'>Time: </span>"));
        this._timeInput = $("<input type='time' class='form-control' />");
        if (dateObj) {
          this._timeInput.val(`${pad(dateObj.getHours())}:${pad(dateObj.getMinutes())}`);
        }
        timeContainer.append(this._timeInput);
        container.append(timeContainer);
      }
      if (!this.isReadOnly) {
        const nowBtn = $("<button class='btn btn-outline-secondary btn-sm mt-1'>Now</button>");
        nowBtn.on("click", () => {
          const now = /* @__PURE__ */ new Date();
          if (this._dateInput) {
            this._dateInput.val(`${now.getFullYear()}-${pad(now.getMonth() + 1)}-${pad(now.getDate())}`);
          }
          if (this._timeInput) {
            this._timeInput.val(`${pad(now.getHours())}:${pad(now.getMinutes())}`);
          }
          if (this.callbackUpdateField)
            this.callbackUpdateField();
        });
        container.append(nowBtn);
        const changeHandler = () => {
          if (this.callbackUpdateField)
            this.callbackUpdateField();
        };
        if (this._dateInput)
          this._dateInput.on("change", changeHandler);
        if (this._timeInput)
          this._timeInput.on("change", changeHandler);
      }
      return container;
    }
    async evaluateDom(dmElement) {
      const fieldName = this.field.get("name")?.toString() ?? "";
      const datePart = this._dateInput?.val()?.toString() || "";
      const timePart = this._timeInput?.val()?.toString() || "";
      if (!datePart && !timePart) {
        return;
      }
      const now = /* @__PURE__ */ new Date();
      const year = datePart ? parseInt(datePart.substring(0, 4)) : now.getFullYear();
      const month = datePart ? parseInt(datePart.substring(5, 7)) - 1 : now.getMonth();
      const day = datePart ? parseInt(datePart.substring(8, 10)) : now.getDate();
      const hours = timePart ? parseInt(timePart.substring(0, 2)) : 0;
      const minutes = timePart ? parseInt(timePart.substring(3, 5)) : 0;
      const dateObj = new Date(year, month, day, hours, minutes, 0);
      dmElement.set(fieldName, dateObj.toISOString());
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/DropDownBaseField.js
  var FieldType;
  (function(FieldType2) {
    FieldType2[FieldType2["Strings"] = 0] = "Strings";
    FieldType2[FieldType2["References"] = 1] = "References";
  })(FieldType || (FieldType = {}));
  var DropDownBaseField = class extends BaseField {
    constructor(context) {
      super(context);
      this.loadedFields = [];
      this.fieldType = FieldType.Strings;
      this.counter = 0;
      this._loadedFields = [];
    }
    async createDom(dmElement) {
      if (this.configuration?.isNewItem) {
        return $("<div><em>New item, content can only be set after saving</em></div>");
      }
      const fieldName = this.field.get("name")?.toString() ?? "";
      this._element = dmElement;
      if (this.isReadOnly) {
        if (this.fieldType === FieldType.Strings) {
          let value = dmElement.get(fieldName);
          const result = $("<span></span>");
          if (value === void 0) {
            result.html("<em>Not set</em>");
          } else {
            result.text(value);
          }
          return result;
        } else if (this.fieldType === FieldType.References) {
          let value = dmElement.get(fieldName, ObjectType.Object);
          if (value === void 0 || value === null) {
            return $("<em>Not set</em>");
          }
          const textValue = await getItemWithNameAndId(value.workspace, value.uri);
          const result = $("<span></span>");
          result.text(textValue.name ?? "Unknown Name");
          return result;
        } else {
          return $(`<span><em>Unknown field type:${this.fieldType}</em></span>`);
        }
      } else {
        this._loadedFields = await this.loadFields();
        for (const field of this._loadedFields) {
          field.key = "item_" + this.counter.toString();
          this.counter++;
        }
        if (this.fieldType === FieldType.Strings) {
          let value = dmElement.get(fieldName);
          if (Array.isArray(this._loadedFields)) {
            this._dropDown = $("<select></select>");
            for (const field of this._loadedFields) {
              if (field.value === void 0) {
                continue;
              }
              const option = $("<option></option>");
              option.text(field.title);
              option.val(field.value);
              this._dropDown.append(option);
            }
            if (value !== void 0) {
              this._dropDown.val(value);
            }
            this._dropDown.on("change", () => {
              if (this.callbackUpdateField !== void 0) {
                this.callbackUpdateField();
              }
            });
            return this._dropDown;
          } else {
            return $("<span><em>No values given</em></span>");
          }
        } else if (this.fieldType === FieldType.References) {
          let value = dmElement.get(fieldName, ObjectType.Object);
          this._dropDown = $("<select></select>");
          let anySelected = false;
          const notSelected = $("<option value=''>--- No selection ---</option>");
          this._dropDown.append(notSelected);
          for (const n in this._loadedFields) {
            const item = this._loadedFields[n];
            if (item.key === void 0)
              continue;
            const option = $("<option></option>");
            option.attr("value", item.key);
            if (value !== void 0 && value !== null && value.uri === item.itemUrl && value.workspace === item.workspace) {
              option.attr("selected", "selected");
              anySelected = true;
            }
            option.text(item.title);
            this._dropDown.append(option);
          }
          if (!anySelected) {
            notSelected.attr("selected", "selected");
          }
          this._dropDown.on("change", () => {
            if (this.callbackUpdateField !== void 0) {
              this.callbackUpdateField();
            }
          });
          return this._dropDown;
        } else {
          return $("<span><em>Unknown field type</em></span>");
        }
      }
    }
    async evaluateDom(dmElement) {
      if (this.fieldType === FieldType.Strings) {
        const fieldName = this.field.get("name").toString();
        dmElement.set(fieldName, this._dropDown?.val());
      } else if (this.fieldType === FieldType.References && this._dropDown !== void 0) {
        const fieldName = this.field.get("name").toString();
        const fieldValue = this._dropDown.val();
        if (fieldValue === "" || fieldValue === void 0) {
          dmElement.unset(fieldName);
        } else {
          const field = this._loadedFields.find((x) => x.key === fieldValue);
          if (field !== void 0) {
            dmElement.set(fieldName, DmObject.createFromReference(field.workspace ?? "", field.itemUrl ?? ""));
          }
        }
      }
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/DropDownField.js
  var Field6 = class extends DropDownBaseField {
    constructor(context) {
      super(context);
      this.fieldType = FieldType.Strings;
    }
    async loadFields() {
      const values = this.field.get("values");
      if (Array.isArray(values)) {
        return values.map((x) => {
          return {
            title: x.get(_Forms._FieldTypes._ValuePair._name_).toString(),
            value: x.get(_Forms._FieldTypes._ValuePair.value).toString()
          };
        });
      } else {
        return [];
      }
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/MetaClassElementField.js
  var Field7 = class extends BaseField {
    constructor(context) {
      super(context);
    }
    async createDom(dmElement) {
      const tthis = this;
      const divContainer = $("<div />");
      const div = $("<div />");
      if (dmElement !== void 0 && dmElement.metaClass !== void 0 && dmElement.metaClass !== null) {
        if (dmElement.metaClass.uri !== null) {
          div.text(dmElement.metaClass.id ?? dmElement.metaClass.uri);
          injectNameByUri(div, dmElement.metaClass.workspace, encodeURIComponent(dmElement.metaClass.uri));
        } else if (dmElement.metaClass.id !== null && dmElement.metaClass.extentUri !== null) {
          div.text(dmElement.metaClass?.id ?? "Unknown id");
          injectNameByUri(div, dmElement.metaClass.workspace, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
        } else {
          div.append($("<em>unknown</em>"));
        }
      } else {
        div.append($("<em>unknown</em>"));
      }
      divContainer.append(div);
      if (!this.isReadOnly) {
        const changeMetaClassDiv = $("<div></div>");
        const button = $("<button class='btn btn-secondary' type='button'></button>");
        button.text("Change MetaClass");
        button.on("click", async () => {
          changeMetaClassDiv.empty();
          const selectItemCtrl = new SelectItemControl();
          const divSelectItem = await selectItemCtrl.initAsync(changeMetaClassDiv);
          await selectItemCtrl.setExtentByUri("Types", "dm:///_internal/types/internal");
          selectItemCtrl.itemSelected.addListener((selectedItem) => {
            setMetaclass(tthis.form.workspace, tthis.itemUrl, selectedItem.uri).then(() => divSelectItem.remove()).then(() => {
              if (tthis.configuration?.refreshForm !== void 0) {
                tthis.configuration.refreshForm();
              }
            });
          });
        });
        divContainer.append(button);
        divContainer.append(changeMetaClassDiv);
      }
      return divContainer;
    }
    async evaluateDom(dmElement) {
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/forms/Forms.js
  var FormModel;
  (function(FormModel2) {
    function createEmptyFormObject() {
      const form = new DmObject();
      form.metaClass = {
        id: _Forms._FormTypes.__ObjectForm_Uri,
        uri: _Forms._FormTypes.__ObjectForm_Uri,
        workspace: WorkspaceTypes
      };
      const detailForm = new DmObject();
      detailForm.metaClass = {
        id: _Forms._FormTypes.__RowForm_Uri,
        uri: _Forms._FormTypes.__RowForm_Uri,
        workspace: WorkspaceTypes
      };
      form.set("tab", [detailForm]);
      return form;
    }
    FormModel2.createEmptyFormObject = createEmptyFormObject;
  })(FormModel || (FormModel = {}));
  var FormMode;
  (function(FormMode2) {
    FormMode2[FormMode2["ViewMode"] = 0] = "ViewMode";
    FormMode2[FormMode2["EditMode"] = 1] = "EditMode";
  })(FormMode || (FormMode = {}));
  var SubmitMethod;
  (function(SubmitMethod2) {
    SubmitMethod2[SubmitMethod2["Save"] = 0] = "Save";
    SubmitMethod2[SubmitMethod2["SaveAndClose"] = 1] = "SaveAndClose";
    SubmitMethod2[SubmitMethod2["UserDefined1"] = 2] = "UserDefined1";
    SubmitMethod2[SubmitMethod2["UserDefined2"] = 3] = "UserDefined2";
    SubmitMethod2[SubmitMethod2["UserDefined3"] = 4] = "UserDefined3";
  })(SubmitMethod || (SubmitMethod = {}));

  // ../DatenMeister.WebServer/Assets/js/datenmeister/client/Actions.js
  async function executeActionDirectly(actionName, parameter) {
    let url = baseUrl + "api/action/execute_directly/" + encodeURIComponent(actionName);
    const result = await post(url, { parameter: createJsonFromObject(parameter.parameter) });
    const resultingObject = convertJsonObjectToDmObject(result.result);
    const resultAsDmObject = {
      success: result.success,
      result: result.result,
      reason: result.reason,
      stackTrace: result.stackTrace,
      resultAsDmObject: resultingObject
    };
    return resultAsDmObject;
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/FormActions.js
  var modules = new Array();
  function getModule(actionName) {
    for (let n in modules) {
      const module = modules[n];
      if (module.actionName === actionName) {
        return module;
      }
    }
    return void 0;
  }
  async function execute(actionName, form, element, parameter, submitMethod) {
    const foundModule = getModule(actionName);
    if (foundModule !== void 0) {
      return foundModule.execute(form, element, parameter, submitMethod);
    }
    alert("Unknown action type: " + actionName);
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/MofSync.js
  async function sync(element) {
    const paras = new Array();
    for (const key in element.propertiesSet) {
      const value = element.get(key, ObjectType.Default);
      if (value === void 0 || value === null || Array.isArray(value) && value.length === 0) {
        await unsetProperty(element.workspace, element.uri, key);
        console.log("MofSync: Unsetting: " + element.uri + " - " + key);
      } else if ((typeof value === "object" || value === "function") && value !== null) {
        const referenceValue = value;
        if (referenceValue.workspace === void 0 || referenceValue.uri === void 0)
          throw new Error("referenceValue.workspace or referenceValue.uri is undefined");
        await setPropertyReference(element.workspace, element.uri, {
          property: key,
          workspaceId: referenceValue.workspace,
          referenceUri: referenceValue.uri
        });
        console.log("MofSync: Setting Reference for: " + element.uri + " - " + key);
      } else {
        paras.push({
          key,
          value: value?.toString() ?? ""
        });
        console.log("MofSync: Setting: " + element.uri + " - " + key);
      }
    }
    if (element.isMetaClassSet === true && element.metaClass?.uri !== void 0) {
      await setMetaclass(element.workspace, element.uri, element.metaClass.uri);
    }
    if (paras.length > 0) {
      await setPropertiesByStringValues(element.workspace, element.uri, {
        properties: paras
      });
    }
    element.clearSync();
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/ActionField.js
  var Field8 = class extends BaseField {
    constructor(context) {
      super(context);
    }
    async createDom(dmElement) {
      const tthis = this;
      const title = this.field.get(_Forms._FieldTypes._ActionFieldData.title, ObjectType.String);
      const action = this.field.get(_Forms._FieldTypes._ActionFieldData.actionName, ObjectType.String);
      const parameter = this.field.get(_Forms._FieldTypes._ActionFieldData.parameter, ObjectType.Single);
      const buttonText = this.field.get(_Forms._FieldTypes._ActionFieldData.buttonText, ObjectType.String);
      const module = getModule(action);
      this.inConfirmation = false;
      const requireConfirmation = module?.requiresConfirmation === true;
      this.button = $("<button class='btn btn-secondary' type='button'></button>");
      this.button.text(buttonText ?? title ?? action);
      const bindingKey = this.field.get(_Forms._FieldTypes._ActionFieldData.bindingKey, ObjectType.String);
      if (bindingKey) {
        const bindingKeyModifierCtrl = this.field.get(_Forms._FieldTypes._ActionFieldData.bindingKeyModifierCtrl, ObjectType.Boolean);
        const bindingKeyModifierShift = this.field.get(_Forms._FieldTypes._ActionFieldData.bindingKeyModifierShift, ObjectType.Boolean);
        const bindingKeyModifierAlt = this.field.get(_Forms._FieldTypes._ActionFieldData.bindingKeyModifierAlt, ObjectType.Boolean);
        addKeyBindingEvent(this.button, bindingKey, bindingKeyModifierCtrl, bindingKeyModifierShift, bindingKeyModifierAlt);
      }
      this.button.on("click", async () => {
        if (!requireConfirmation || tthis.inConfirmation) {
          if (tthis.form.storeFormValuesIntoDom !== void 0) {
            await tthis.form.storeFormValuesIntoDom(true);
          }
          const mofWithSync = dmElement;
          if (module?.skipSaving !== true && mofWithSync.propertiesSet !== void 0) {
            await sync(mofWithSync);
          }
          await execute(action, tthis.form, dmElement, parameter);
        }
        if (requireConfirmation && !tthis.inConfirmation) {
          this.button.text("Are you sure?");
          tthis.inConfirmation = true;
        }
      });
      return this.button;
    }
    async evaluateDom(dmElement) {
    }
    showValue() {
      return false;
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/MergedFieldsInCell.js
  var Field9 = class extends BaseField {
    constructor(context) {
      super(context);
      this.childFields = [];
    }
    /**
     * Inhibits the presentation of the checkbox indicating whether the element is set.
     */
    showValue() {
      return false;
    }
    async createDom(dmElement) {
      const fields = this.field.get(_Forms._FieldTypes._MergedFieldsInCellData.fields, ObjectType.Array) ?? [];
      const container = $("<div class='dm-merged-fields-in-cell d-flex flex-wrap gap-2 align-items-center'></div>");
      this.childFields = [];
      for (const field of fields) {
        if (!(field instanceof DmObject)) {
          continue;
        }
        const isFieldReadOnly = this.isReadOnly || field.get(_Forms._FieldTypes._FieldData.isReadOnly, ObjectType.Boolean);
        const fieldElement = createField({
          configuration: this.configuration,
          field,
          itemUrl: this.itemUrl,
          isReadOnly: isFieldReadOnly,
          form: this.form
        });
        fieldElement.callbackUpdateField = () => {
          if (this.callbackUpdateField !== void 0) {
            this.callbackUpdateField();
          }
        };
        this.childFields.push(fieldElement);
        container.append(await fieldElement.createDom(dmElement));
      }
      return container;
    }
    async evaluateDom(dmElement) {
      for (const field of this.childFields) {
        await field.evaluateDom(dmElement);
      }
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/MofResolver.js
  function resolve(value) {
    return new Promise((resolve2) => {
      if (Array.isArray(value)) {
        resolve2(value);
      } else if ((typeof value === "object" || typeof value === "function") && value !== null) {
        const asDmObject = value;
        if (asDmObject.isReference) {
          const workspace = asDmObject.workspace;
          if (workspace === void 0) {
            alert("Workspace is undefined");
            asDmObject.workspace = "_";
          }
          getObjectByUri(asDmObject.workspace, asDmObject.uri).then((loadedValue) => resolve2(loadedValue));
        } else {
          resolve2(value);
        }
      } else {
        resolve2(value);
      }
    });
  }
  function forceResolve(value) {
    return new Promise((resolve2) => {
      if (Array.isArray(value)) {
        resolve2(value);
      } else if ((typeof value === "object" || typeof value === "function") && value !== null) {
        const asDmObject = value;
        if (true) {
          const workspace = asDmObject.workspace;
          if (workspace === void 0) {
            alert("Workspace is undefined");
            asDmObject.workspace = "_";
          }
          getObjectByUri(asDmObject.workspace, asDmObject.uri).then((loadedValue) => resolve2(loadedValue));
        } else {
          resolve2(value);
        }
      } else {
        resolve2(value);
      }
    });
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/client/Types.js
  async function getPropertyType(workspace, metaClass, propertyName) {
    try {
      return await get(baseUrl + "api/types/propertytype/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(metaClass) + "/" + encodeURIComponent(propertyName));
    } catch (error) {
      console.log(error);
      return void 0;
    }
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/forms/Interfaces.js
  var FormType;
  (function(FormType3) {
    FormType3["Object"] = "object";
    FormType3["Collection"] = "collection";
  })(FormType || (FormType = {}));

  // ../DatenMeister.WebServer/Assets/js/datenmeister/controls/TypeSelectionControl.js
  var TypeSelectionControl = class {
    /**
     * The constructor for this control
     * @param container The element in which the element shall be contained
     */
    constructor(container) {
      this.typeSelected = new UserEvent();
      this._container = container;
    }
    /**
     * Sets the current type url..
     * This method must be called before calling createControl
     * @param formUrl
     */
    setCurrentTypeUrl(formUrl) {
      this._currentTypeUrl = formUrl;
    }
    async createControl() {
      const result = $("<div><div class='dm-form-selection-control-select'></div></div>");
      const controlSelect = $(".dm-form-selection-control-select", result);
      this.selectionField = new SelectItemControl();
      this.selectionField.itemSelected.addListener(async (selectedItem) => {
        if (selectedItem !== void 0) {
          const foundItem = await getObjectByUri(selectedItem.workspace, selectedItem.uri);
          this.typeSelected.invoke({
            selectedType: foundItem
          });
        } else {
          alert("No valid type has been selected");
        }
      });
      if (this._currentTypeUrl !== void 0) {
        await this.selectionField.setItemByUri(this._currentTypeUrl.workspace, this._currentTypeUrl.uri);
      } else {
        await this.selectionField.setExtentByUri("Types", "dm:///_internal/types/internal");
      }
      const settings = new ContainerSettings();
      settings.setButtonText = "Use Type";
      settings.headline = "Select Type:";
      await this.selectionField.initAsync(controlSelect, settings);
      this._container.append(result);
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/client/Actions.Items.js
  var _MoveUpDownAction = _Actions._MoveUpDownAction;
  var _MoveDirectionType = _Actions._MoveDirectionType;
  async function moveItemInCollection(workspace, parentUri, propertyName, elementUri, direction) {
    const action = new DmObject(_Actions.__MoveUpDownAction_Uri);
    action.set(_MoveUpDownAction.container, DmObject.createFromReference(workspace, parentUri));
    action.set(_MoveUpDownAction.property, propertyName);
    action.set(_MoveUpDownAction.element, DmObject.createFromReference(workspace, elementUri));
    action.set(_MoveUpDownAction.direction, direction);
    const parameter = {
      parameter: action
    };
    await executeActionDirectly("Execute", parameter);
  }
  async function moveItemInCollectionDown(workspace, parentUri, propertyName, elementUri) {
    await moveItemInCollection(workspace, parentUri, propertyName, elementUri, _MoveDirectionType.Down);
  }
  async function moveItemInCollectionUp(workspace, parentUri, propertyName, elementUri) {
    await moveItemInCollection(workspace, parentUri, propertyName, elementUri, _MoveDirectionType.Up);
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/models/UML.js
  var _UML;
  (function(_UML2) {
    let _Activities;
    (function(_Activities2) {
      class _Activity {
      }
      _Activity.edge = "edge";
      _Activity.group = "group";
      _Activity.isReadOnly = "isReadOnly";
      _Activity.isSingleExecution = "isSingleExecution";
      _Activity.node = "node";
      _Activity.partition = "partition";
      _Activity.structuredNode = "structuredNode";
      _Activity.variable = "variable";
      _Activity.context = "context";
      _Activity.isReentrant = "isReentrant";
      _Activity.ownedParameter = "ownedParameter";
      _Activity.ownedParameterSet = "ownedParameterSet";
      _Activity.postcondition = "postcondition";
      _Activity.precondition = "precondition";
      _Activity.specification = "specification";
      _Activity.redefinedBehavior = "redefinedBehavior";
      _Activity.extension = "extension";
      _Activity.isAbstract = "isAbstract";
      _Activity.isActive = "isActive";
      _Activity.nestedClassifier = "nestedClassifier";
      _Activity.ownedAttribute = "ownedAttribute";
      _Activity.ownedOperation = "ownedOperation";
      _Activity.ownedReception = "ownedReception";
      _Activity.superClass = "superClass";
      _Activity.classifierBehavior = "classifierBehavior";
      _Activity.interfaceRealization = "interfaceRealization";
      _Activity.ownedBehavior = "ownedBehavior";
      _Activity.attribute = "attribute";
      _Activity.collaborationUse = "collaborationUse";
      _Activity.feature = "feature";
      _Activity.general = "general";
      _Activity.generalization = "generalization";
      _Activity.inheritedMember = "inheritedMember";
      _Activity.isFinalSpecialization = "isFinalSpecialization";
      _Activity.ownedTemplateSignature = "ownedTemplateSignature";
      _Activity.ownedUseCase = "ownedUseCase";
      _Activity.powertypeExtent = "powertypeExtent";
      _Activity.redefinedClassifier = "redefinedClassifier";
      _Activity.representation = "representation";
      _Activity.substitution = "substitution";
      _Activity.templateParameter = "templateParameter";
      _Activity.useCase = "useCase";
      _Activity.elementImport = "elementImport";
      _Activity.importedMember = "importedMember";
      _Activity.member = "member";
      _Activity.ownedMember = "ownedMember";
      _Activity.ownedRule = "ownedRule";
      _Activity.packageImport = "packageImport";
      _Activity.clientDependency = "clientDependency";
      _Activity._name_ = "name";
      _Activity.nameExpression = "nameExpression";
      _Activity.namespace = "namespace";
      _Activity.qualifiedName = "qualifiedName";
      _Activity.visibility = "visibility";
      _Activity.ownedComment = "ownedComment";
      _Activity.ownedElement = "ownedElement";
      _Activity.owner = "owner";
      _Activity._package_ = "package";
      _Activity.owningTemplateParameter = "owningTemplateParameter";
      _Activity.templateBinding = "templateBinding";
      _Activity.isLeaf = "isLeaf";
      _Activity.redefinedElement = "redefinedElement";
      _Activity.redefinitionContext = "redefinitionContext";
      _Activity.ownedPort = "ownedPort";
      _Activity.ownedConnector = "ownedConnector";
      _Activity.part = "part";
      _Activity.role = "role";
      _Activities2._Activity = _Activity;
      _Activities2.__Activity_Uri = "dm:///_internal/model/uml#Activity";
      class _ActivityEdge {
      }
      _ActivityEdge.activity = "activity";
      _ActivityEdge.guard = "guard";
      _ActivityEdge.inGroup = "inGroup";
      _ActivityEdge.inPartition = "inPartition";
      _ActivityEdge.inStructuredNode = "inStructuredNode";
      _ActivityEdge.interrupts = "interrupts";
      _ActivityEdge.redefinedEdge = "redefinedEdge";
      _ActivityEdge.source = "source";
      _ActivityEdge.target = "target";
      _ActivityEdge.weight = "weight";
      _ActivityEdge.isLeaf = "isLeaf";
      _ActivityEdge.redefinedElement = "redefinedElement";
      _ActivityEdge.redefinitionContext = "redefinitionContext";
      _ActivityEdge.clientDependency = "clientDependency";
      _ActivityEdge._name_ = "name";
      _ActivityEdge.nameExpression = "nameExpression";
      _ActivityEdge.namespace = "namespace";
      _ActivityEdge.qualifiedName = "qualifiedName";
      _ActivityEdge.visibility = "visibility";
      _ActivityEdge.ownedComment = "ownedComment";
      _ActivityEdge.ownedElement = "ownedElement";
      _ActivityEdge.owner = "owner";
      _Activities2._ActivityEdge = _ActivityEdge;
      _Activities2.__ActivityEdge_Uri = "dm:///_internal/model/uml#ActivityEdge";
      class _ActivityFinalNode {
      }
      _ActivityFinalNode.activity = "activity";
      _ActivityFinalNode.inGroup = "inGroup";
      _ActivityFinalNode.inInterruptibleRegion = "inInterruptibleRegion";
      _ActivityFinalNode.inPartition = "inPartition";
      _ActivityFinalNode.inStructuredNode = "inStructuredNode";
      _ActivityFinalNode.incoming = "incoming";
      _ActivityFinalNode.outgoing = "outgoing";
      _ActivityFinalNode.redefinedNode = "redefinedNode";
      _ActivityFinalNode.isLeaf = "isLeaf";
      _ActivityFinalNode.redefinedElement = "redefinedElement";
      _ActivityFinalNode.redefinitionContext = "redefinitionContext";
      _ActivityFinalNode.clientDependency = "clientDependency";
      _ActivityFinalNode._name_ = "name";
      _ActivityFinalNode.nameExpression = "nameExpression";
      _ActivityFinalNode.namespace = "namespace";
      _ActivityFinalNode.qualifiedName = "qualifiedName";
      _ActivityFinalNode.visibility = "visibility";
      _ActivityFinalNode.ownedComment = "ownedComment";
      _ActivityFinalNode.ownedElement = "ownedElement";
      _ActivityFinalNode.owner = "owner";
      _Activities2._ActivityFinalNode = _ActivityFinalNode;
      _Activities2.__ActivityFinalNode_Uri = "dm:///_internal/model/uml#ActivityFinalNode";
      class _ActivityGroup {
      }
      _ActivityGroup.containedEdge = "containedEdge";
      _ActivityGroup.containedNode = "containedNode";
      _ActivityGroup.inActivity = "inActivity";
      _ActivityGroup.subgroup = "subgroup";
      _ActivityGroup.superGroup = "superGroup";
      _ActivityGroup.clientDependency = "clientDependency";
      _ActivityGroup._name_ = "name";
      _ActivityGroup.nameExpression = "nameExpression";
      _ActivityGroup.namespace = "namespace";
      _ActivityGroup.qualifiedName = "qualifiedName";
      _ActivityGroup.visibility = "visibility";
      _ActivityGroup.ownedComment = "ownedComment";
      _ActivityGroup.ownedElement = "ownedElement";
      _ActivityGroup.owner = "owner";
      _Activities2._ActivityGroup = _ActivityGroup;
      _Activities2.__ActivityGroup_Uri = "dm:///_internal/model/uml#ActivityGroup";
      class _ActivityNode {
      }
      _ActivityNode.activity = "activity";
      _ActivityNode.inGroup = "inGroup";
      _ActivityNode.inInterruptibleRegion = "inInterruptibleRegion";
      _ActivityNode.inPartition = "inPartition";
      _ActivityNode.inStructuredNode = "inStructuredNode";
      _ActivityNode.incoming = "incoming";
      _ActivityNode.outgoing = "outgoing";
      _ActivityNode.redefinedNode = "redefinedNode";
      _ActivityNode.isLeaf = "isLeaf";
      _ActivityNode.redefinedElement = "redefinedElement";
      _ActivityNode.redefinitionContext = "redefinitionContext";
      _ActivityNode.clientDependency = "clientDependency";
      _ActivityNode._name_ = "name";
      _ActivityNode.nameExpression = "nameExpression";
      _ActivityNode.namespace = "namespace";
      _ActivityNode.qualifiedName = "qualifiedName";
      _ActivityNode.visibility = "visibility";
      _ActivityNode.ownedComment = "ownedComment";
      _ActivityNode.ownedElement = "ownedElement";
      _ActivityNode.owner = "owner";
      _Activities2._ActivityNode = _ActivityNode;
      _Activities2.__ActivityNode_Uri = "dm:///_internal/model/uml#ActivityNode";
      class _ActivityParameterNode {
      }
      _ActivityParameterNode.parameter = "parameter";
      _ActivityParameterNode.inState = "inState";
      _ActivityParameterNode.isControlType = "isControlType";
      _ActivityParameterNode.ordering = "ordering";
      _ActivityParameterNode.selection = "selection";
      _ActivityParameterNode.upperBound = "upperBound";
      _ActivityParameterNode.type = "type";
      _ActivityParameterNode.clientDependency = "clientDependency";
      _ActivityParameterNode._name_ = "name";
      _ActivityParameterNode.nameExpression = "nameExpression";
      _ActivityParameterNode.namespace = "namespace";
      _ActivityParameterNode.qualifiedName = "qualifiedName";
      _ActivityParameterNode.visibility = "visibility";
      _ActivityParameterNode.ownedComment = "ownedComment";
      _ActivityParameterNode.ownedElement = "ownedElement";
      _ActivityParameterNode.owner = "owner";
      _ActivityParameterNode.activity = "activity";
      _ActivityParameterNode.inGroup = "inGroup";
      _ActivityParameterNode.inInterruptibleRegion = "inInterruptibleRegion";
      _ActivityParameterNode.inPartition = "inPartition";
      _ActivityParameterNode.inStructuredNode = "inStructuredNode";
      _ActivityParameterNode.incoming = "incoming";
      _ActivityParameterNode.outgoing = "outgoing";
      _ActivityParameterNode.redefinedNode = "redefinedNode";
      _ActivityParameterNode.isLeaf = "isLeaf";
      _ActivityParameterNode.redefinedElement = "redefinedElement";
      _ActivityParameterNode.redefinitionContext = "redefinitionContext";
      _Activities2._ActivityParameterNode = _ActivityParameterNode;
      _Activities2.__ActivityParameterNode_Uri = "dm:///_internal/model/uml#ActivityParameterNode";
      class _ActivityPartition {
      }
      _ActivityPartition.edge = "edge";
      _ActivityPartition.isDimension = "isDimension";
      _ActivityPartition.isExternal = "isExternal";
      _ActivityPartition.node = "node";
      _ActivityPartition.represents = "represents";
      _ActivityPartition.subpartition = "subpartition";
      _ActivityPartition.superPartition = "superPartition";
      _ActivityPartition.containedEdge = "containedEdge";
      _ActivityPartition.containedNode = "containedNode";
      _ActivityPartition.inActivity = "inActivity";
      _ActivityPartition.subgroup = "subgroup";
      _ActivityPartition.superGroup = "superGroup";
      _ActivityPartition.clientDependency = "clientDependency";
      _ActivityPartition._name_ = "name";
      _ActivityPartition.nameExpression = "nameExpression";
      _ActivityPartition.namespace = "namespace";
      _ActivityPartition.qualifiedName = "qualifiedName";
      _ActivityPartition.visibility = "visibility";
      _ActivityPartition.ownedComment = "ownedComment";
      _ActivityPartition.ownedElement = "ownedElement";
      _ActivityPartition.owner = "owner";
      _Activities2._ActivityPartition = _ActivityPartition;
      _Activities2.__ActivityPartition_Uri = "dm:///_internal/model/uml#ActivityPartition";
      class _CentralBufferNode {
      }
      _CentralBufferNode.inState = "inState";
      _CentralBufferNode.isControlType = "isControlType";
      _CentralBufferNode.ordering = "ordering";
      _CentralBufferNode.selection = "selection";
      _CentralBufferNode.upperBound = "upperBound";
      _CentralBufferNode.type = "type";
      _CentralBufferNode.clientDependency = "clientDependency";
      _CentralBufferNode._name_ = "name";
      _CentralBufferNode.nameExpression = "nameExpression";
      _CentralBufferNode.namespace = "namespace";
      _CentralBufferNode.qualifiedName = "qualifiedName";
      _CentralBufferNode.visibility = "visibility";
      _CentralBufferNode.ownedComment = "ownedComment";
      _CentralBufferNode.ownedElement = "ownedElement";
      _CentralBufferNode.owner = "owner";
      _CentralBufferNode.activity = "activity";
      _CentralBufferNode.inGroup = "inGroup";
      _CentralBufferNode.inInterruptibleRegion = "inInterruptibleRegion";
      _CentralBufferNode.inPartition = "inPartition";
      _CentralBufferNode.inStructuredNode = "inStructuredNode";
      _CentralBufferNode.incoming = "incoming";
      _CentralBufferNode.outgoing = "outgoing";
      _CentralBufferNode.redefinedNode = "redefinedNode";
      _CentralBufferNode.isLeaf = "isLeaf";
      _CentralBufferNode.redefinedElement = "redefinedElement";
      _CentralBufferNode.redefinitionContext = "redefinitionContext";
      _Activities2._CentralBufferNode = _CentralBufferNode;
      _Activities2.__CentralBufferNode_Uri = "dm:///_internal/model/uml#CentralBufferNode";
      class _ControlFlow {
      }
      _ControlFlow.activity = "activity";
      _ControlFlow.guard = "guard";
      _ControlFlow.inGroup = "inGroup";
      _ControlFlow.inPartition = "inPartition";
      _ControlFlow.inStructuredNode = "inStructuredNode";
      _ControlFlow.interrupts = "interrupts";
      _ControlFlow.redefinedEdge = "redefinedEdge";
      _ControlFlow.source = "source";
      _ControlFlow.target = "target";
      _ControlFlow.weight = "weight";
      _ControlFlow.isLeaf = "isLeaf";
      _ControlFlow.redefinedElement = "redefinedElement";
      _ControlFlow.redefinitionContext = "redefinitionContext";
      _ControlFlow.clientDependency = "clientDependency";
      _ControlFlow._name_ = "name";
      _ControlFlow.nameExpression = "nameExpression";
      _ControlFlow.namespace = "namespace";
      _ControlFlow.qualifiedName = "qualifiedName";
      _ControlFlow.visibility = "visibility";
      _ControlFlow.ownedComment = "ownedComment";
      _ControlFlow.ownedElement = "ownedElement";
      _ControlFlow.owner = "owner";
      _Activities2._ControlFlow = _ControlFlow;
      _Activities2.__ControlFlow_Uri = "dm:///_internal/model/uml#ControlFlow";
      class _ControlNode {
      }
      _ControlNode.activity = "activity";
      _ControlNode.inGroup = "inGroup";
      _ControlNode.inInterruptibleRegion = "inInterruptibleRegion";
      _ControlNode.inPartition = "inPartition";
      _ControlNode.inStructuredNode = "inStructuredNode";
      _ControlNode.incoming = "incoming";
      _ControlNode.outgoing = "outgoing";
      _ControlNode.redefinedNode = "redefinedNode";
      _ControlNode.isLeaf = "isLeaf";
      _ControlNode.redefinedElement = "redefinedElement";
      _ControlNode.redefinitionContext = "redefinitionContext";
      _ControlNode.clientDependency = "clientDependency";
      _ControlNode._name_ = "name";
      _ControlNode.nameExpression = "nameExpression";
      _ControlNode.namespace = "namespace";
      _ControlNode.qualifiedName = "qualifiedName";
      _ControlNode.visibility = "visibility";
      _ControlNode.ownedComment = "ownedComment";
      _ControlNode.ownedElement = "ownedElement";
      _ControlNode.owner = "owner";
      _Activities2._ControlNode = _ControlNode;
      _Activities2.__ControlNode_Uri = "dm:///_internal/model/uml#ControlNode";
      class _DataStoreNode {
      }
      _DataStoreNode.inState = "inState";
      _DataStoreNode.isControlType = "isControlType";
      _DataStoreNode.ordering = "ordering";
      _DataStoreNode.selection = "selection";
      _DataStoreNode.upperBound = "upperBound";
      _DataStoreNode.type = "type";
      _DataStoreNode.clientDependency = "clientDependency";
      _DataStoreNode._name_ = "name";
      _DataStoreNode.nameExpression = "nameExpression";
      _DataStoreNode.namespace = "namespace";
      _DataStoreNode.qualifiedName = "qualifiedName";
      _DataStoreNode.visibility = "visibility";
      _DataStoreNode.ownedComment = "ownedComment";
      _DataStoreNode.ownedElement = "ownedElement";
      _DataStoreNode.owner = "owner";
      _DataStoreNode.activity = "activity";
      _DataStoreNode.inGroup = "inGroup";
      _DataStoreNode.inInterruptibleRegion = "inInterruptibleRegion";
      _DataStoreNode.inPartition = "inPartition";
      _DataStoreNode.inStructuredNode = "inStructuredNode";
      _DataStoreNode.incoming = "incoming";
      _DataStoreNode.outgoing = "outgoing";
      _DataStoreNode.redefinedNode = "redefinedNode";
      _DataStoreNode.isLeaf = "isLeaf";
      _DataStoreNode.redefinedElement = "redefinedElement";
      _DataStoreNode.redefinitionContext = "redefinitionContext";
      _Activities2._DataStoreNode = _DataStoreNode;
      _Activities2.__DataStoreNode_Uri = "dm:///_internal/model/uml#DataStoreNode";
      class _DecisionNode {
      }
      _DecisionNode.decisionInput = "decisionInput";
      _DecisionNode.decisionInputFlow = "decisionInputFlow";
      _DecisionNode.activity = "activity";
      _DecisionNode.inGroup = "inGroup";
      _DecisionNode.inInterruptibleRegion = "inInterruptibleRegion";
      _DecisionNode.inPartition = "inPartition";
      _DecisionNode.inStructuredNode = "inStructuredNode";
      _DecisionNode.incoming = "incoming";
      _DecisionNode.outgoing = "outgoing";
      _DecisionNode.redefinedNode = "redefinedNode";
      _DecisionNode.isLeaf = "isLeaf";
      _DecisionNode.redefinedElement = "redefinedElement";
      _DecisionNode.redefinitionContext = "redefinitionContext";
      _DecisionNode.clientDependency = "clientDependency";
      _DecisionNode._name_ = "name";
      _DecisionNode.nameExpression = "nameExpression";
      _DecisionNode.namespace = "namespace";
      _DecisionNode.qualifiedName = "qualifiedName";
      _DecisionNode.visibility = "visibility";
      _DecisionNode.ownedComment = "ownedComment";
      _DecisionNode.ownedElement = "ownedElement";
      _DecisionNode.owner = "owner";
      _Activities2._DecisionNode = _DecisionNode;
      _Activities2.__DecisionNode_Uri = "dm:///_internal/model/uml#DecisionNode";
      class _ExceptionHandler {
      }
      _ExceptionHandler.exceptionInput = "exceptionInput";
      _ExceptionHandler.exceptionType = "exceptionType";
      _ExceptionHandler.handlerBody = "handlerBody";
      _ExceptionHandler.protectedNode = "protectedNode";
      _ExceptionHandler.ownedComment = "ownedComment";
      _ExceptionHandler.ownedElement = "ownedElement";
      _ExceptionHandler.owner = "owner";
      _Activities2._ExceptionHandler = _ExceptionHandler;
      _Activities2.__ExceptionHandler_Uri = "dm:///_internal/model/uml#ExceptionHandler";
      class _ExecutableNode {
      }
      _ExecutableNode.handler = "handler";
      _ExecutableNode.activity = "activity";
      _ExecutableNode.inGroup = "inGroup";
      _ExecutableNode.inInterruptibleRegion = "inInterruptibleRegion";
      _ExecutableNode.inPartition = "inPartition";
      _ExecutableNode.inStructuredNode = "inStructuredNode";
      _ExecutableNode.incoming = "incoming";
      _ExecutableNode.outgoing = "outgoing";
      _ExecutableNode.redefinedNode = "redefinedNode";
      _ExecutableNode.isLeaf = "isLeaf";
      _ExecutableNode.redefinedElement = "redefinedElement";
      _ExecutableNode.redefinitionContext = "redefinitionContext";
      _ExecutableNode.clientDependency = "clientDependency";
      _ExecutableNode._name_ = "name";
      _ExecutableNode.nameExpression = "nameExpression";
      _ExecutableNode.namespace = "namespace";
      _ExecutableNode.qualifiedName = "qualifiedName";
      _ExecutableNode.visibility = "visibility";
      _ExecutableNode.ownedComment = "ownedComment";
      _ExecutableNode.ownedElement = "ownedElement";
      _ExecutableNode.owner = "owner";
      _Activities2._ExecutableNode = _ExecutableNode;
      _Activities2.__ExecutableNode_Uri = "dm:///_internal/model/uml#ExecutableNode";
      class _FinalNode {
      }
      _FinalNode.activity = "activity";
      _FinalNode.inGroup = "inGroup";
      _FinalNode.inInterruptibleRegion = "inInterruptibleRegion";
      _FinalNode.inPartition = "inPartition";
      _FinalNode.inStructuredNode = "inStructuredNode";
      _FinalNode.incoming = "incoming";
      _FinalNode.outgoing = "outgoing";
      _FinalNode.redefinedNode = "redefinedNode";
      _FinalNode.isLeaf = "isLeaf";
      _FinalNode.redefinedElement = "redefinedElement";
      _FinalNode.redefinitionContext = "redefinitionContext";
      _FinalNode.clientDependency = "clientDependency";
      _FinalNode._name_ = "name";
      _FinalNode.nameExpression = "nameExpression";
      _FinalNode.namespace = "namespace";
      _FinalNode.qualifiedName = "qualifiedName";
      _FinalNode.visibility = "visibility";
      _FinalNode.ownedComment = "ownedComment";
      _FinalNode.ownedElement = "ownedElement";
      _FinalNode.owner = "owner";
      _Activities2._FinalNode = _FinalNode;
      _Activities2.__FinalNode_Uri = "dm:///_internal/model/uml#FinalNode";
      class _FlowFinalNode {
      }
      _FlowFinalNode.activity = "activity";
      _FlowFinalNode.inGroup = "inGroup";
      _FlowFinalNode.inInterruptibleRegion = "inInterruptibleRegion";
      _FlowFinalNode.inPartition = "inPartition";
      _FlowFinalNode.inStructuredNode = "inStructuredNode";
      _FlowFinalNode.incoming = "incoming";
      _FlowFinalNode.outgoing = "outgoing";
      _FlowFinalNode.redefinedNode = "redefinedNode";
      _FlowFinalNode.isLeaf = "isLeaf";
      _FlowFinalNode.redefinedElement = "redefinedElement";
      _FlowFinalNode.redefinitionContext = "redefinitionContext";
      _FlowFinalNode.clientDependency = "clientDependency";
      _FlowFinalNode._name_ = "name";
      _FlowFinalNode.nameExpression = "nameExpression";
      _FlowFinalNode.namespace = "namespace";
      _FlowFinalNode.qualifiedName = "qualifiedName";
      _FlowFinalNode.visibility = "visibility";
      _FlowFinalNode.ownedComment = "ownedComment";
      _FlowFinalNode.ownedElement = "ownedElement";
      _FlowFinalNode.owner = "owner";
      _Activities2._FlowFinalNode = _FlowFinalNode;
      _Activities2.__FlowFinalNode_Uri = "dm:///_internal/model/uml#FlowFinalNode";
      class _ForkNode {
      }
      _ForkNode.activity = "activity";
      _ForkNode.inGroup = "inGroup";
      _ForkNode.inInterruptibleRegion = "inInterruptibleRegion";
      _ForkNode.inPartition = "inPartition";
      _ForkNode.inStructuredNode = "inStructuredNode";
      _ForkNode.incoming = "incoming";
      _ForkNode.outgoing = "outgoing";
      _ForkNode.redefinedNode = "redefinedNode";
      _ForkNode.isLeaf = "isLeaf";
      _ForkNode.redefinedElement = "redefinedElement";
      _ForkNode.redefinitionContext = "redefinitionContext";
      _ForkNode.clientDependency = "clientDependency";
      _ForkNode._name_ = "name";
      _ForkNode.nameExpression = "nameExpression";
      _ForkNode.namespace = "namespace";
      _ForkNode.qualifiedName = "qualifiedName";
      _ForkNode.visibility = "visibility";
      _ForkNode.ownedComment = "ownedComment";
      _ForkNode.ownedElement = "ownedElement";
      _ForkNode.owner = "owner";
      _Activities2._ForkNode = _ForkNode;
      _Activities2.__ForkNode_Uri = "dm:///_internal/model/uml#ForkNode";
      class _InitialNode {
      }
      _InitialNode.activity = "activity";
      _InitialNode.inGroup = "inGroup";
      _InitialNode.inInterruptibleRegion = "inInterruptibleRegion";
      _InitialNode.inPartition = "inPartition";
      _InitialNode.inStructuredNode = "inStructuredNode";
      _InitialNode.incoming = "incoming";
      _InitialNode.outgoing = "outgoing";
      _InitialNode.redefinedNode = "redefinedNode";
      _InitialNode.isLeaf = "isLeaf";
      _InitialNode.redefinedElement = "redefinedElement";
      _InitialNode.redefinitionContext = "redefinitionContext";
      _InitialNode.clientDependency = "clientDependency";
      _InitialNode._name_ = "name";
      _InitialNode.nameExpression = "nameExpression";
      _InitialNode.namespace = "namespace";
      _InitialNode.qualifiedName = "qualifiedName";
      _InitialNode.visibility = "visibility";
      _InitialNode.ownedComment = "ownedComment";
      _InitialNode.ownedElement = "ownedElement";
      _InitialNode.owner = "owner";
      _Activities2._InitialNode = _InitialNode;
      _Activities2.__InitialNode_Uri = "dm:///_internal/model/uml#InitialNode";
      class _InterruptibleActivityRegion {
      }
      _InterruptibleActivityRegion.interruptingEdge = "interruptingEdge";
      _InterruptibleActivityRegion.node = "node";
      _InterruptibleActivityRegion.containedEdge = "containedEdge";
      _InterruptibleActivityRegion.containedNode = "containedNode";
      _InterruptibleActivityRegion.inActivity = "inActivity";
      _InterruptibleActivityRegion.subgroup = "subgroup";
      _InterruptibleActivityRegion.superGroup = "superGroup";
      _InterruptibleActivityRegion.clientDependency = "clientDependency";
      _InterruptibleActivityRegion._name_ = "name";
      _InterruptibleActivityRegion.nameExpression = "nameExpression";
      _InterruptibleActivityRegion.namespace = "namespace";
      _InterruptibleActivityRegion.qualifiedName = "qualifiedName";
      _InterruptibleActivityRegion.visibility = "visibility";
      _InterruptibleActivityRegion.ownedComment = "ownedComment";
      _InterruptibleActivityRegion.ownedElement = "ownedElement";
      _InterruptibleActivityRegion.owner = "owner";
      _Activities2._InterruptibleActivityRegion = _InterruptibleActivityRegion;
      _Activities2.__InterruptibleActivityRegion_Uri = "dm:///_internal/model/uml#InterruptibleActivityRegion";
      class _JoinNode {
      }
      _JoinNode.isCombineDuplicate = "isCombineDuplicate";
      _JoinNode.joinSpec = "joinSpec";
      _JoinNode.activity = "activity";
      _JoinNode.inGroup = "inGroup";
      _JoinNode.inInterruptibleRegion = "inInterruptibleRegion";
      _JoinNode.inPartition = "inPartition";
      _JoinNode.inStructuredNode = "inStructuredNode";
      _JoinNode.incoming = "incoming";
      _JoinNode.outgoing = "outgoing";
      _JoinNode.redefinedNode = "redefinedNode";
      _JoinNode.isLeaf = "isLeaf";
      _JoinNode.redefinedElement = "redefinedElement";
      _JoinNode.redefinitionContext = "redefinitionContext";
      _JoinNode.clientDependency = "clientDependency";
      _JoinNode._name_ = "name";
      _JoinNode.nameExpression = "nameExpression";
      _JoinNode.namespace = "namespace";
      _JoinNode.qualifiedName = "qualifiedName";
      _JoinNode.visibility = "visibility";
      _JoinNode.ownedComment = "ownedComment";
      _JoinNode.ownedElement = "ownedElement";
      _JoinNode.owner = "owner";
      _Activities2._JoinNode = _JoinNode;
      _Activities2.__JoinNode_Uri = "dm:///_internal/model/uml#JoinNode";
      class _MergeNode {
      }
      _MergeNode.activity = "activity";
      _MergeNode.inGroup = "inGroup";
      _MergeNode.inInterruptibleRegion = "inInterruptibleRegion";
      _MergeNode.inPartition = "inPartition";
      _MergeNode.inStructuredNode = "inStructuredNode";
      _MergeNode.incoming = "incoming";
      _MergeNode.outgoing = "outgoing";
      _MergeNode.redefinedNode = "redefinedNode";
      _MergeNode.isLeaf = "isLeaf";
      _MergeNode.redefinedElement = "redefinedElement";
      _MergeNode.redefinitionContext = "redefinitionContext";
      _MergeNode.clientDependency = "clientDependency";
      _MergeNode._name_ = "name";
      _MergeNode.nameExpression = "nameExpression";
      _MergeNode.namespace = "namespace";
      _MergeNode.qualifiedName = "qualifiedName";
      _MergeNode.visibility = "visibility";
      _MergeNode.ownedComment = "ownedComment";
      _MergeNode.ownedElement = "ownedElement";
      _MergeNode.owner = "owner";
      _Activities2._MergeNode = _MergeNode;
      _Activities2.__MergeNode_Uri = "dm:///_internal/model/uml#MergeNode";
      class _ObjectFlow {
      }
      _ObjectFlow.isMulticast = "isMulticast";
      _ObjectFlow.isMultireceive = "isMultireceive";
      _ObjectFlow.selection = "selection";
      _ObjectFlow.transformation = "transformation";
      _ObjectFlow.activity = "activity";
      _ObjectFlow.guard = "guard";
      _ObjectFlow.inGroup = "inGroup";
      _ObjectFlow.inPartition = "inPartition";
      _ObjectFlow.inStructuredNode = "inStructuredNode";
      _ObjectFlow.interrupts = "interrupts";
      _ObjectFlow.redefinedEdge = "redefinedEdge";
      _ObjectFlow.source = "source";
      _ObjectFlow.target = "target";
      _ObjectFlow.weight = "weight";
      _ObjectFlow.isLeaf = "isLeaf";
      _ObjectFlow.redefinedElement = "redefinedElement";
      _ObjectFlow.redefinitionContext = "redefinitionContext";
      _ObjectFlow.clientDependency = "clientDependency";
      _ObjectFlow._name_ = "name";
      _ObjectFlow.nameExpression = "nameExpression";
      _ObjectFlow.namespace = "namespace";
      _ObjectFlow.qualifiedName = "qualifiedName";
      _ObjectFlow.visibility = "visibility";
      _ObjectFlow.ownedComment = "ownedComment";
      _ObjectFlow.ownedElement = "ownedElement";
      _ObjectFlow.owner = "owner";
      _Activities2._ObjectFlow = _ObjectFlow;
      _Activities2.__ObjectFlow_Uri = "dm:///_internal/model/uml#ObjectFlow";
      class _ObjectNode {
      }
      _ObjectNode.inState = "inState";
      _ObjectNode.isControlType = "isControlType";
      _ObjectNode.ordering = "ordering";
      _ObjectNode.selection = "selection";
      _ObjectNode.upperBound = "upperBound";
      _ObjectNode.type = "type";
      _ObjectNode.clientDependency = "clientDependency";
      _ObjectNode._name_ = "name";
      _ObjectNode.nameExpression = "nameExpression";
      _ObjectNode.namespace = "namespace";
      _ObjectNode.qualifiedName = "qualifiedName";
      _ObjectNode.visibility = "visibility";
      _ObjectNode.ownedComment = "ownedComment";
      _ObjectNode.ownedElement = "ownedElement";
      _ObjectNode.owner = "owner";
      _ObjectNode.activity = "activity";
      _ObjectNode.inGroup = "inGroup";
      _ObjectNode.inInterruptibleRegion = "inInterruptibleRegion";
      _ObjectNode.inPartition = "inPartition";
      _ObjectNode.inStructuredNode = "inStructuredNode";
      _ObjectNode.incoming = "incoming";
      _ObjectNode.outgoing = "outgoing";
      _ObjectNode.redefinedNode = "redefinedNode";
      _ObjectNode.isLeaf = "isLeaf";
      _ObjectNode.redefinedElement = "redefinedElement";
      _ObjectNode.redefinitionContext = "redefinitionContext";
      _Activities2._ObjectNode = _ObjectNode;
      _Activities2.__ObjectNode_Uri = "dm:///_internal/model/uml#ObjectNode";
      class _Variable {
      }
      _Variable.activityScope = "activityScope";
      _Variable.scope = "scope";
      _Variable.end = "end";
      _Variable.templateParameter = "templateParameter";
      _Variable.type = "type";
      _Variable.clientDependency = "clientDependency";
      _Variable._name_ = "name";
      _Variable.nameExpression = "nameExpression";
      _Variable.namespace = "namespace";
      _Variable.qualifiedName = "qualifiedName";
      _Variable.visibility = "visibility";
      _Variable.ownedComment = "ownedComment";
      _Variable.ownedElement = "ownedElement";
      _Variable.owner = "owner";
      _Variable.owningTemplateParameter = "owningTemplateParameter";
      _Variable.isOrdered = "isOrdered";
      _Variable.isUnique = "isUnique";
      _Variable.lower = "lower";
      _Variable.lowerValue = "lowerValue";
      _Variable.upper = "upper";
      _Variable.upperValue = "upperValue";
      _Activities2._Variable = _Variable;
      _Activities2.__Variable_Uri = "dm:///_internal/model/uml#Variable";
      let _ObjectNodeOrderingKind;
      (function(_ObjectNodeOrderingKind2) {
        _ObjectNodeOrderingKind2.unordered = "unordered";
        _ObjectNodeOrderingKind2.ordered = "ordered";
        _ObjectNodeOrderingKind2.LIFO = "LIFO";
        _ObjectNodeOrderingKind2.FIFO = "FIFO";
      })(_ObjectNodeOrderingKind = _Activities2._ObjectNodeOrderingKind || (_Activities2._ObjectNodeOrderingKind = {}));
      let ___ObjectNodeOrderingKind;
      (function(___ObjectNodeOrderingKind2) {
        ___ObjectNodeOrderingKind2[___ObjectNodeOrderingKind2["unordered"] = 0] = "unordered";
        ___ObjectNodeOrderingKind2[___ObjectNodeOrderingKind2["ordered"] = 1] = "ordered";
        ___ObjectNodeOrderingKind2[___ObjectNodeOrderingKind2["LIFO"] = 2] = "LIFO";
        ___ObjectNodeOrderingKind2[___ObjectNodeOrderingKind2["FIFO"] = 3] = "FIFO";
      })(___ObjectNodeOrderingKind = _Activities2.___ObjectNodeOrderingKind || (_Activities2.___ObjectNodeOrderingKind = {}));
    })(_Activities = _UML2._Activities || (_UML2._Activities = {}));
    let _Values;
    (function(_Values2) {
      class _Duration {
      }
      _Duration.expr = "expr";
      _Duration.observation = "observation";
      _Duration.type = "type";
      _Duration.clientDependency = "clientDependency";
      _Duration._name_ = "name";
      _Duration.nameExpression = "nameExpression";
      _Duration.namespace = "namespace";
      _Duration.qualifiedName = "qualifiedName";
      _Duration.visibility = "visibility";
      _Duration.ownedComment = "ownedComment";
      _Duration.ownedElement = "ownedElement";
      _Duration.owner = "owner";
      _Duration.owningTemplateParameter = "owningTemplateParameter";
      _Duration.templateParameter = "templateParameter";
      _Values2._Duration = _Duration;
      _Values2.__Duration_Uri = "dm:///_internal/model/uml#Duration";
      class _DurationConstraint {
      }
      _DurationConstraint.firstEvent = "firstEvent";
      _DurationConstraint.specification = "specification";
      _DurationConstraint.constrainedElement = "constrainedElement";
      _DurationConstraint.context = "context";
      _DurationConstraint.visibility = "visibility";
      _DurationConstraint.owningTemplateParameter = "owningTemplateParameter";
      _DurationConstraint.templateParameter = "templateParameter";
      _DurationConstraint.ownedComment = "ownedComment";
      _DurationConstraint.ownedElement = "ownedElement";
      _DurationConstraint.owner = "owner";
      _DurationConstraint.clientDependency = "clientDependency";
      _DurationConstraint._name_ = "name";
      _DurationConstraint.nameExpression = "nameExpression";
      _DurationConstraint.namespace = "namespace";
      _DurationConstraint.qualifiedName = "qualifiedName";
      _Values2._DurationConstraint = _DurationConstraint;
      _Values2.__DurationConstraint_Uri = "dm:///_internal/model/uml#DurationConstraint";
      class _DurationInterval {
      }
      _DurationInterval.max = "max";
      _DurationInterval.min = "min";
      _DurationInterval.type = "type";
      _DurationInterval.clientDependency = "clientDependency";
      _DurationInterval._name_ = "name";
      _DurationInterval.nameExpression = "nameExpression";
      _DurationInterval.namespace = "namespace";
      _DurationInterval.qualifiedName = "qualifiedName";
      _DurationInterval.visibility = "visibility";
      _DurationInterval.ownedComment = "ownedComment";
      _DurationInterval.ownedElement = "ownedElement";
      _DurationInterval.owner = "owner";
      _DurationInterval.owningTemplateParameter = "owningTemplateParameter";
      _DurationInterval.templateParameter = "templateParameter";
      _Values2._DurationInterval = _DurationInterval;
      _Values2.__DurationInterval_Uri = "dm:///_internal/model/uml#DurationInterval";
      class _DurationObservation {
      }
      _DurationObservation.event = "event";
      _DurationObservation.firstEvent = "firstEvent";
      _DurationObservation.visibility = "visibility";
      _DurationObservation.owningTemplateParameter = "owningTemplateParameter";
      _DurationObservation.templateParameter = "templateParameter";
      _DurationObservation.ownedComment = "ownedComment";
      _DurationObservation.ownedElement = "ownedElement";
      _DurationObservation.owner = "owner";
      _DurationObservation.clientDependency = "clientDependency";
      _DurationObservation._name_ = "name";
      _DurationObservation.nameExpression = "nameExpression";
      _DurationObservation.namespace = "namespace";
      _DurationObservation.qualifiedName = "qualifiedName";
      _Values2._DurationObservation = _DurationObservation;
      _Values2.__DurationObservation_Uri = "dm:///_internal/model/uml#DurationObservation";
      class _Expression {
      }
      _Expression.operand = "operand";
      _Expression.symbol = "symbol";
      _Expression.type = "type";
      _Expression.clientDependency = "clientDependency";
      _Expression._name_ = "name";
      _Expression.nameExpression = "nameExpression";
      _Expression.namespace = "namespace";
      _Expression.qualifiedName = "qualifiedName";
      _Expression.visibility = "visibility";
      _Expression.ownedComment = "ownedComment";
      _Expression.ownedElement = "ownedElement";
      _Expression.owner = "owner";
      _Expression.owningTemplateParameter = "owningTemplateParameter";
      _Expression.templateParameter = "templateParameter";
      _Values2._Expression = _Expression;
      _Values2.__Expression_Uri = "dm:///_internal/model/uml#Expression";
      class _Interval {
      }
      _Interval.max = "max";
      _Interval.min = "min";
      _Interval.type = "type";
      _Interval.clientDependency = "clientDependency";
      _Interval._name_ = "name";
      _Interval.nameExpression = "nameExpression";
      _Interval.namespace = "namespace";
      _Interval.qualifiedName = "qualifiedName";
      _Interval.visibility = "visibility";
      _Interval.ownedComment = "ownedComment";
      _Interval.ownedElement = "ownedElement";
      _Interval.owner = "owner";
      _Interval.owningTemplateParameter = "owningTemplateParameter";
      _Interval.templateParameter = "templateParameter";
      _Values2._Interval = _Interval;
      _Values2.__Interval_Uri = "dm:///_internal/model/uml#Interval";
      class _IntervalConstraint {
      }
      _IntervalConstraint.specification = "specification";
      _IntervalConstraint.constrainedElement = "constrainedElement";
      _IntervalConstraint.context = "context";
      _IntervalConstraint.visibility = "visibility";
      _IntervalConstraint.owningTemplateParameter = "owningTemplateParameter";
      _IntervalConstraint.templateParameter = "templateParameter";
      _IntervalConstraint.ownedComment = "ownedComment";
      _IntervalConstraint.ownedElement = "ownedElement";
      _IntervalConstraint.owner = "owner";
      _IntervalConstraint.clientDependency = "clientDependency";
      _IntervalConstraint._name_ = "name";
      _IntervalConstraint.nameExpression = "nameExpression";
      _IntervalConstraint.namespace = "namespace";
      _IntervalConstraint.qualifiedName = "qualifiedName";
      _Values2._IntervalConstraint = _IntervalConstraint;
      _Values2.__IntervalConstraint_Uri = "dm:///_internal/model/uml#IntervalConstraint";
      class _LiteralBoolean {
      }
      _LiteralBoolean.value = "value";
      _LiteralBoolean.type = "type";
      _LiteralBoolean.clientDependency = "clientDependency";
      _LiteralBoolean._name_ = "name";
      _LiteralBoolean.nameExpression = "nameExpression";
      _LiteralBoolean.namespace = "namespace";
      _LiteralBoolean.qualifiedName = "qualifiedName";
      _LiteralBoolean.visibility = "visibility";
      _LiteralBoolean.ownedComment = "ownedComment";
      _LiteralBoolean.ownedElement = "ownedElement";
      _LiteralBoolean.owner = "owner";
      _LiteralBoolean.owningTemplateParameter = "owningTemplateParameter";
      _LiteralBoolean.templateParameter = "templateParameter";
      _Values2._LiteralBoolean = _LiteralBoolean;
      _Values2.__LiteralBoolean_Uri = "dm:///_internal/model/uml#LiteralBoolean";
      class _LiteralInteger {
      }
      _LiteralInteger.value = "value";
      _LiteralInteger.type = "type";
      _LiteralInteger.clientDependency = "clientDependency";
      _LiteralInteger._name_ = "name";
      _LiteralInteger.nameExpression = "nameExpression";
      _LiteralInteger.namespace = "namespace";
      _LiteralInteger.qualifiedName = "qualifiedName";
      _LiteralInteger.visibility = "visibility";
      _LiteralInteger.ownedComment = "ownedComment";
      _LiteralInteger.ownedElement = "ownedElement";
      _LiteralInteger.owner = "owner";
      _LiteralInteger.owningTemplateParameter = "owningTemplateParameter";
      _LiteralInteger.templateParameter = "templateParameter";
      _Values2._LiteralInteger = _LiteralInteger;
      _Values2.__LiteralInteger_Uri = "dm:///_internal/model/uml#LiteralInteger";
      class _LiteralNull {
      }
      _LiteralNull.type = "type";
      _LiteralNull.clientDependency = "clientDependency";
      _LiteralNull._name_ = "name";
      _LiteralNull.nameExpression = "nameExpression";
      _LiteralNull.namespace = "namespace";
      _LiteralNull.qualifiedName = "qualifiedName";
      _LiteralNull.visibility = "visibility";
      _LiteralNull.ownedComment = "ownedComment";
      _LiteralNull.ownedElement = "ownedElement";
      _LiteralNull.owner = "owner";
      _LiteralNull.owningTemplateParameter = "owningTemplateParameter";
      _LiteralNull.templateParameter = "templateParameter";
      _Values2._LiteralNull = _LiteralNull;
      _Values2.__LiteralNull_Uri = "dm:///_internal/model/uml#LiteralNull";
      class _LiteralReal {
      }
      _LiteralReal.value = "value";
      _LiteralReal.type = "type";
      _LiteralReal.clientDependency = "clientDependency";
      _LiteralReal._name_ = "name";
      _LiteralReal.nameExpression = "nameExpression";
      _LiteralReal.namespace = "namespace";
      _LiteralReal.qualifiedName = "qualifiedName";
      _LiteralReal.visibility = "visibility";
      _LiteralReal.ownedComment = "ownedComment";
      _LiteralReal.ownedElement = "ownedElement";
      _LiteralReal.owner = "owner";
      _LiteralReal.owningTemplateParameter = "owningTemplateParameter";
      _LiteralReal.templateParameter = "templateParameter";
      _Values2._LiteralReal = _LiteralReal;
      _Values2.__LiteralReal_Uri = "dm:///_internal/model/uml#LiteralReal";
      class _LiteralSpecification {
      }
      _LiteralSpecification.type = "type";
      _LiteralSpecification.clientDependency = "clientDependency";
      _LiteralSpecification._name_ = "name";
      _LiteralSpecification.nameExpression = "nameExpression";
      _LiteralSpecification.namespace = "namespace";
      _LiteralSpecification.qualifiedName = "qualifiedName";
      _LiteralSpecification.visibility = "visibility";
      _LiteralSpecification.ownedComment = "ownedComment";
      _LiteralSpecification.ownedElement = "ownedElement";
      _LiteralSpecification.owner = "owner";
      _LiteralSpecification.owningTemplateParameter = "owningTemplateParameter";
      _LiteralSpecification.templateParameter = "templateParameter";
      _Values2._LiteralSpecification = _LiteralSpecification;
      _Values2.__LiteralSpecification_Uri = "dm:///_internal/model/uml#LiteralSpecification";
      class _LiteralString {
      }
      _LiteralString.value = "value";
      _LiteralString.type = "type";
      _LiteralString.clientDependency = "clientDependency";
      _LiteralString._name_ = "name";
      _LiteralString.nameExpression = "nameExpression";
      _LiteralString.namespace = "namespace";
      _LiteralString.qualifiedName = "qualifiedName";
      _LiteralString.visibility = "visibility";
      _LiteralString.ownedComment = "ownedComment";
      _LiteralString.ownedElement = "ownedElement";
      _LiteralString.owner = "owner";
      _LiteralString.owningTemplateParameter = "owningTemplateParameter";
      _LiteralString.templateParameter = "templateParameter";
      _Values2._LiteralString = _LiteralString;
      _Values2.__LiteralString_Uri = "dm:///_internal/model/uml#LiteralString";
      class _LiteralUnlimitedNatural {
      }
      _LiteralUnlimitedNatural.value = "value";
      _LiteralUnlimitedNatural.type = "type";
      _LiteralUnlimitedNatural.clientDependency = "clientDependency";
      _LiteralUnlimitedNatural._name_ = "name";
      _LiteralUnlimitedNatural.nameExpression = "nameExpression";
      _LiteralUnlimitedNatural.namespace = "namespace";
      _LiteralUnlimitedNatural.qualifiedName = "qualifiedName";
      _LiteralUnlimitedNatural.visibility = "visibility";
      _LiteralUnlimitedNatural.ownedComment = "ownedComment";
      _LiteralUnlimitedNatural.ownedElement = "ownedElement";
      _LiteralUnlimitedNatural.owner = "owner";
      _LiteralUnlimitedNatural.owningTemplateParameter = "owningTemplateParameter";
      _LiteralUnlimitedNatural.templateParameter = "templateParameter";
      _Values2._LiteralUnlimitedNatural = _LiteralUnlimitedNatural;
      _Values2.__LiteralUnlimitedNatural_Uri = "dm:///_internal/model/uml#LiteralUnlimitedNatural";
      class _Observation {
      }
      _Observation.visibility = "visibility";
      _Observation.owningTemplateParameter = "owningTemplateParameter";
      _Observation.templateParameter = "templateParameter";
      _Observation.ownedComment = "ownedComment";
      _Observation.ownedElement = "ownedElement";
      _Observation.owner = "owner";
      _Observation.clientDependency = "clientDependency";
      _Observation._name_ = "name";
      _Observation.nameExpression = "nameExpression";
      _Observation.namespace = "namespace";
      _Observation.qualifiedName = "qualifiedName";
      _Values2._Observation = _Observation;
      _Values2.__Observation_Uri = "dm:///_internal/model/uml#Observation";
      class _OpaqueExpression {
      }
      _OpaqueExpression.behavior = "behavior";
      _OpaqueExpression.body = "body";
      _OpaqueExpression.language = "language";
      _OpaqueExpression.result = "result";
      _OpaqueExpression.type = "type";
      _OpaqueExpression.clientDependency = "clientDependency";
      _OpaqueExpression._name_ = "name";
      _OpaqueExpression.nameExpression = "nameExpression";
      _OpaqueExpression.namespace = "namespace";
      _OpaqueExpression.qualifiedName = "qualifiedName";
      _OpaqueExpression.visibility = "visibility";
      _OpaqueExpression.ownedComment = "ownedComment";
      _OpaqueExpression.ownedElement = "ownedElement";
      _OpaqueExpression.owner = "owner";
      _OpaqueExpression.owningTemplateParameter = "owningTemplateParameter";
      _OpaqueExpression.templateParameter = "templateParameter";
      _Values2._OpaqueExpression = _OpaqueExpression;
      _Values2.__OpaqueExpression_Uri = "dm:///_internal/model/uml#OpaqueExpression";
      class _StringExpression {
      }
      _StringExpression.owningExpression = "owningExpression";
      _StringExpression.subExpression = "subExpression";
      _StringExpression.ownedTemplateSignature = "ownedTemplateSignature";
      _StringExpression.templateBinding = "templateBinding";
      _StringExpression.ownedComment = "ownedComment";
      _StringExpression.ownedElement = "ownedElement";
      _StringExpression.owner = "owner";
      _StringExpression.operand = "operand";
      _StringExpression.symbol = "symbol";
      _StringExpression.type = "type";
      _StringExpression.clientDependency = "clientDependency";
      _StringExpression._name_ = "name";
      _StringExpression.nameExpression = "nameExpression";
      _StringExpression.namespace = "namespace";
      _StringExpression.qualifiedName = "qualifiedName";
      _StringExpression.visibility = "visibility";
      _StringExpression.owningTemplateParameter = "owningTemplateParameter";
      _StringExpression.templateParameter = "templateParameter";
      _Values2._StringExpression = _StringExpression;
      _Values2.__StringExpression_Uri = "dm:///_internal/model/uml#StringExpression";
      class _TimeConstraint {
      }
      _TimeConstraint.firstEvent = "firstEvent";
      _TimeConstraint.specification = "specification";
      _TimeConstraint.constrainedElement = "constrainedElement";
      _TimeConstraint.context = "context";
      _TimeConstraint.visibility = "visibility";
      _TimeConstraint.owningTemplateParameter = "owningTemplateParameter";
      _TimeConstraint.templateParameter = "templateParameter";
      _TimeConstraint.ownedComment = "ownedComment";
      _TimeConstraint.ownedElement = "ownedElement";
      _TimeConstraint.owner = "owner";
      _TimeConstraint.clientDependency = "clientDependency";
      _TimeConstraint._name_ = "name";
      _TimeConstraint.nameExpression = "nameExpression";
      _TimeConstraint.namespace = "namespace";
      _TimeConstraint.qualifiedName = "qualifiedName";
      _Values2._TimeConstraint = _TimeConstraint;
      _Values2.__TimeConstraint_Uri = "dm:///_internal/model/uml#TimeConstraint";
      class _TimeExpression {
      }
      _TimeExpression.expr = "expr";
      _TimeExpression.observation = "observation";
      _TimeExpression.type = "type";
      _TimeExpression.clientDependency = "clientDependency";
      _TimeExpression._name_ = "name";
      _TimeExpression.nameExpression = "nameExpression";
      _TimeExpression.namespace = "namespace";
      _TimeExpression.qualifiedName = "qualifiedName";
      _TimeExpression.visibility = "visibility";
      _TimeExpression.ownedComment = "ownedComment";
      _TimeExpression.ownedElement = "ownedElement";
      _TimeExpression.owner = "owner";
      _TimeExpression.owningTemplateParameter = "owningTemplateParameter";
      _TimeExpression.templateParameter = "templateParameter";
      _Values2._TimeExpression = _TimeExpression;
      _Values2.__TimeExpression_Uri = "dm:///_internal/model/uml#TimeExpression";
      class _TimeInterval {
      }
      _TimeInterval.max = "max";
      _TimeInterval.min = "min";
      _TimeInterval.type = "type";
      _TimeInterval.clientDependency = "clientDependency";
      _TimeInterval._name_ = "name";
      _TimeInterval.nameExpression = "nameExpression";
      _TimeInterval.namespace = "namespace";
      _TimeInterval.qualifiedName = "qualifiedName";
      _TimeInterval.visibility = "visibility";
      _TimeInterval.ownedComment = "ownedComment";
      _TimeInterval.ownedElement = "ownedElement";
      _TimeInterval.owner = "owner";
      _TimeInterval.owningTemplateParameter = "owningTemplateParameter";
      _TimeInterval.templateParameter = "templateParameter";
      _Values2._TimeInterval = _TimeInterval;
      _Values2.__TimeInterval_Uri = "dm:///_internal/model/uml#TimeInterval";
      class _TimeObservation {
      }
      _TimeObservation.event = "event";
      _TimeObservation.firstEvent = "firstEvent";
      _TimeObservation.visibility = "visibility";
      _TimeObservation.owningTemplateParameter = "owningTemplateParameter";
      _TimeObservation.templateParameter = "templateParameter";
      _TimeObservation.ownedComment = "ownedComment";
      _TimeObservation.ownedElement = "ownedElement";
      _TimeObservation.owner = "owner";
      _TimeObservation.clientDependency = "clientDependency";
      _TimeObservation._name_ = "name";
      _TimeObservation.nameExpression = "nameExpression";
      _TimeObservation.namespace = "namespace";
      _TimeObservation.qualifiedName = "qualifiedName";
      _Values2._TimeObservation = _TimeObservation;
      _Values2.__TimeObservation_Uri = "dm:///_internal/model/uml#TimeObservation";
      class _ValueSpecification {
      }
      _ValueSpecification.type = "type";
      _ValueSpecification.clientDependency = "clientDependency";
      _ValueSpecification._name_ = "name";
      _ValueSpecification.nameExpression = "nameExpression";
      _ValueSpecification.namespace = "namespace";
      _ValueSpecification.qualifiedName = "qualifiedName";
      _ValueSpecification.visibility = "visibility";
      _ValueSpecification.ownedComment = "ownedComment";
      _ValueSpecification.ownedElement = "ownedElement";
      _ValueSpecification.owner = "owner";
      _ValueSpecification.owningTemplateParameter = "owningTemplateParameter";
      _ValueSpecification.templateParameter = "templateParameter";
      _Values2._ValueSpecification = _ValueSpecification;
      _Values2.__ValueSpecification_Uri = "dm:///_internal/model/uml#ValueSpecification";
    })(_Values = _UML2._Values || (_UML2._Values = {}));
    let _UseCases;
    (function(_UseCases2) {
      class _Actor {
      }
      _Actor.classifierBehavior = "classifierBehavior";
      _Actor.interfaceRealization = "interfaceRealization";
      _Actor.ownedBehavior = "ownedBehavior";
      _Actor.attribute = "attribute";
      _Actor.collaborationUse = "collaborationUse";
      _Actor.feature = "feature";
      _Actor.general = "general";
      _Actor.generalization = "generalization";
      _Actor.inheritedMember = "inheritedMember";
      _Actor.isAbstract = "isAbstract";
      _Actor.isFinalSpecialization = "isFinalSpecialization";
      _Actor.ownedTemplateSignature = "ownedTemplateSignature";
      _Actor.ownedUseCase = "ownedUseCase";
      _Actor.powertypeExtent = "powertypeExtent";
      _Actor.redefinedClassifier = "redefinedClassifier";
      _Actor.representation = "representation";
      _Actor.substitution = "substitution";
      _Actor.templateParameter = "templateParameter";
      _Actor.useCase = "useCase";
      _Actor.elementImport = "elementImport";
      _Actor.importedMember = "importedMember";
      _Actor.member = "member";
      _Actor.ownedMember = "ownedMember";
      _Actor.ownedRule = "ownedRule";
      _Actor.packageImport = "packageImport";
      _Actor.clientDependency = "clientDependency";
      _Actor._name_ = "name";
      _Actor.nameExpression = "nameExpression";
      _Actor.namespace = "namespace";
      _Actor.qualifiedName = "qualifiedName";
      _Actor.visibility = "visibility";
      _Actor.ownedComment = "ownedComment";
      _Actor.ownedElement = "ownedElement";
      _Actor.owner = "owner";
      _Actor._package_ = "package";
      _Actor.owningTemplateParameter = "owningTemplateParameter";
      _Actor.templateBinding = "templateBinding";
      _Actor.isLeaf = "isLeaf";
      _Actor.redefinedElement = "redefinedElement";
      _Actor.redefinitionContext = "redefinitionContext";
      _UseCases2._Actor = _Actor;
      _UseCases2.__Actor_Uri = "dm:///_internal/model/uml#Actor";
      class _Extend {
      }
      _Extend.condition = "condition";
      _Extend.extendedCase = "extendedCase";
      _Extend.extension = "extension";
      _Extend.extensionLocation = "extensionLocation";
      _Extend.clientDependency = "clientDependency";
      _Extend._name_ = "name";
      _Extend.nameExpression = "nameExpression";
      _Extend.namespace = "namespace";
      _Extend.qualifiedName = "qualifiedName";
      _Extend.visibility = "visibility";
      _Extend.ownedComment = "ownedComment";
      _Extend.ownedElement = "ownedElement";
      _Extend.owner = "owner";
      _Extend.source = "source";
      _Extend.target = "target";
      _Extend.relatedElement = "relatedElement";
      _UseCases2._Extend = _Extend;
      _UseCases2.__Extend_Uri = "dm:///_internal/model/uml#Extend";
      class _ExtensionPoint {
      }
      _ExtensionPoint.useCase = "useCase";
      _ExtensionPoint.isLeaf = "isLeaf";
      _ExtensionPoint.redefinedElement = "redefinedElement";
      _ExtensionPoint.redefinitionContext = "redefinitionContext";
      _ExtensionPoint.clientDependency = "clientDependency";
      _ExtensionPoint._name_ = "name";
      _ExtensionPoint.nameExpression = "nameExpression";
      _ExtensionPoint.namespace = "namespace";
      _ExtensionPoint.qualifiedName = "qualifiedName";
      _ExtensionPoint.visibility = "visibility";
      _ExtensionPoint.ownedComment = "ownedComment";
      _ExtensionPoint.ownedElement = "ownedElement";
      _ExtensionPoint.owner = "owner";
      _UseCases2._ExtensionPoint = _ExtensionPoint;
      _UseCases2.__ExtensionPoint_Uri = "dm:///_internal/model/uml#ExtensionPoint";
      class _Include {
      }
      _Include.addition = "addition";
      _Include.includingCase = "includingCase";
      _Include.source = "source";
      _Include.target = "target";
      _Include.relatedElement = "relatedElement";
      _Include.ownedComment = "ownedComment";
      _Include.ownedElement = "ownedElement";
      _Include.owner = "owner";
      _Include.clientDependency = "clientDependency";
      _Include._name_ = "name";
      _Include.nameExpression = "nameExpression";
      _Include.namespace = "namespace";
      _Include.qualifiedName = "qualifiedName";
      _Include.visibility = "visibility";
      _UseCases2._Include = _Include;
      _UseCases2.__Include_Uri = "dm:///_internal/model/uml#Include";
      class _UseCase {
      }
      _UseCase.extend = "extend";
      _UseCase.extensionPoint = "extensionPoint";
      _UseCase.include = "include";
      _UseCase.subject = "subject";
      _UseCase.classifierBehavior = "classifierBehavior";
      _UseCase.interfaceRealization = "interfaceRealization";
      _UseCase.ownedBehavior = "ownedBehavior";
      _UseCase.attribute = "attribute";
      _UseCase.collaborationUse = "collaborationUse";
      _UseCase.feature = "feature";
      _UseCase.general = "general";
      _UseCase.generalization = "generalization";
      _UseCase.inheritedMember = "inheritedMember";
      _UseCase.isAbstract = "isAbstract";
      _UseCase.isFinalSpecialization = "isFinalSpecialization";
      _UseCase.ownedTemplateSignature = "ownedTemplateSignature";
      _UseCase.ownedUseCase = "ownedUseCase";
      _UseCase.powertypeExtent = "powertypeExtent";
      _UseCase.redefinedClassifier = "redefinedClassifier";
      _UseCase.representation = "representation";
      _UseCase.substitution = "substitution";
      _UseCase.templateParameter = "templateParameter";
      _UseCase.useCase = "useCase";
      _UseCase.elementImport = "elementImport";
      _UseCase.importedMember = "importedMember";
      _UseCase.member = "member";
      _UseCase.ownedMember = "ownedMember";
      _UseCase.ownedRule = "ownedRule";
      _UseCase.packageImport = "packageImport";
      _UseCase.clientDependency = "clientDependency";
      _UseCase._name_ = "name";
      _UseCase.nameExpression = "nameExpression";
      _UseCase.namespace = "namespace";
      _UseCase.qualifiedName = "qualifiedName";
      _UseCase.visibility = "visibility";
      _UseCase.ownedComment = "ownedComment";
      _UseCase.ownedElement = "ownedElement";
      _UseCase.owner = "owner";
      _UseCase._package_ = "package";
      _UseCase.owningTemplateParameter = "owningTemplateParameter";
      _UseCase.templateBinding = "templateBinding";
      _UseCase.isLeaf = "isLeaf";
      _UseCase.redefinedElement = "redefinedElement";
      _UseCase.redefinitionContext = "redefinitionContext";
      _UseCases2._UseCase = _UseCase;
      _UseCases2.__UseCase_Uri = "dm:///_internal/model/uml#UseCase";
    })(_UseCases = _UML2._UseCases || (_UML2._UseCases = {}));
    let _StructuredClassifiers;
    (function(_StructuredClassifiers2) {
      class _Association {
      }
      _Association.endType = "endType";
      _Association.isDerived = "isDerived";
      _Association.memberEnd = "memberEnd";
      _Association.navigableOwnedEnd = "navigableOwnedEnd";
      _Association.ownedEnd = "ownedEnd";
      _Association.relatedElement = "relatedElement";
      _Association.ownedComment = "ownedComment";
      _Association.ownedElement = "ownedElement";
      _Association.owner = "owner";
      _Association.attribute = "attribute";
      _Association.collaborationUse = "collaborationUse";
      _Association.feature = "feature";
      _Association.general = "general";
      _Association.generalization = "generalization";
      _Association.inheritedMember = "inheritedMember";
      _Association.isAbstract = "isAbstract";
      _Association.isFinalSpecialization = "isFinalSpecialization";
      _Association.ownedTemplateSignature = "ownedTemplateSignature";
      _Association.ownedUseCase = "ownedUseCase";
      _Association.powertypeExtent = "powertypeExtent";
      _Association.redefinedClassifier = "redefinedClassifier";
      _Association.representation = "representation";
      _Association.substitution = "substitution";
      _Association.templateParameter = "templateParameter";
      _Association.useCase = "useCase";
      _Association.elementImport = "elementImport";
      _Association.importedMember = "importedMember";
      _Association.member = "member";
      _Association.ownedMember = "ownedMember";
      _Association.ownedRule = "ownedRule";
      _Association.packageImport = "packageImport";
      _Association.clientDependency = "clientDependency";
      _Association._name_ = "name";
      _Association.nameExpression = "nameExpression";
      _Association.namespace = "namespace";
      _Association.qualifiedName = "qualifiedName";
      _Association.visibility = "visibility";
      _Association._package_ = "package";
      _Association.owningTemplateParameter = "owningTemplateParameter";
      _Association.templateBinding = "templateBinding";
      _Association.isLeaf = "isLeaf";
      _Association.redefinedElement = "redefinedElement";
      _Association.redefinitionContext = "redefinitionContext";
      _StructuredClassifiers2._Association = _Association;
      _StructuredClassifiers2.__Association_Uri = "dm:///_internal/model/uml#Association";
      class _AssociationClass {
      }
      _AssociationClass.extension = "extension";
      _AssociationClass.isAbstract = "isAbstract";
      _AssociationClass.isActive = "isActive";
      _AssociationClass.nestedClassifier = "nestedClassifier";
      _AssociationClass.ownedAttribute = "ownedAttribute";
      _AssociationClass.ownedOperation = "ownedOperation";
      _AssociationClass.ownedReception = "ownedReception";
      _AssociationClass.superClass = "superClass";
      _AssociationClass.classifierBehavior = "classifierBehavior";
      _AssociationClass.interfaceRealization = "interfaceRealization";
      _AssociationClass.ownedBehavior = "ownedBehavior";
      _AssociationClass.attribute = "attribute";
      _AssociationClass.collaborationUse = "collaborationUse";
      _AssociationClass.feature = "feature";
      _AssociationClass.general = "general";
      _AssociationClass.generalization = "generalization";
      _AssociationClass.inheritedMember = "inheritedMember";
      _AssociationClass.isFinalSpecialization = "isFinalSpecialization";
      _AssociationClass.ownedTemplateSignature = "ownedTemplateSignature";
      _AssociationClass.ownedUseCase = "ownedUseCase";
      _AssociationClass.powertypeExtent = "powertypeExtent";
      _AssociationClass.redefinedClassifier = "redefinedClassifier";
      _AssociationClass.representation = "representation";
      _AssociationClass.substitution = "substitution";
      _AssociationClass.templateParameter = "templateParameter";
      _AssociationClass.useCase = "useCase";
      _AssociationClass.elementImport = "elementImport";
      _AssociationClass.importedMember = "importedMember";
      _AssociationClass.member = "member";
      _AssociationClass.ownedMember = "ownedMember";
      _AssociationClass.ownedRule = "ownedRule";
      _AssociationClass.packageImport = "packageImport";
      _AssociationClass.clientDependency = "clientDependency";
      _AssociationClass._name_ = "name";
      _AssociationClass.nameExpression = "nameExpression";
      _AssociationClass.namespace = "namespace";
      _AssociationClass.qualifiedName = "qualifiedName";
      _AssociationClass.visibility = "visibility";
      _AssociationClass.ownedComment = "ownedComment";
      _AssociationClass.ownedElement = "ownedElement";
      _AssociationClass.owner = "owner";
      _AssociationClass._package_ = "package";
      _AssociationClass.owningTemplateParameter = "owningTemplateParameter";
      _AssociationClass.templateBinding = "templateBinding";
      _AssociationClass.isLeaf = "isLeaf";
      _AssociationClass.redefinedElement = "redefinedElement";
      _AssociationClass.redefinitionContext = "redefinitionContext";
      _AssociationClass.ownedPort = "ownedPort";
      _AssociationClass.ownedConnector = "ownedConnector";
      _AssociationClass.part = "part";
      _AssociationClass.role = "role";
      _AssociationClass.endType = "endType";
      _AssociationClass.isDerived = "isDerived";
      _AssociationClass.memberEnd = "memberEnd";
      _AssociationClass.navigableOwnedEnd = "navigableOwnedEnd";
      _AssociationClass.ownedEnd = "ownedEnd";
      _AssociationClass.relatedElement = "relatedElement";
      _StructuredClassifiers2._AssociationClass = _AssociationClass;
      _StructuredClassifiers2.__AssociationClass_Uri = "dm:///_internal/model/uml#AssociationClass";
      class _Class {
      }
      _Class.extension = "extension";
      _Class.isAbstract = "isAbstract";
      _Class.isActive = "isActive";
      _Class.nestedClassifier = "nestedClassifier";
      _Class.ownedAttribute = "ownedAttribute";
      _Class.ownedOperation = "ownedOperation";
      _Class.ownedReception = "ownedReception";
      _Class.superClass = "superClass";
      _Class.classifierBehavior = "classifierBehavior";
      _Class.interfaceRealization = "interfaceRealization";
      _Class.ownedBehavior = "ownedBehavior";
      _Class.attribute = "attribute";
      _Class.collaborationUse = "collaborationUse";
      _Class.feature = "feature";
      _Class.general = "general";
      _Class.generalization = "generalization";
      _Class.inheritedMember = "inheritedMember";
      _Class.isFinalSpecialization = "isFinalSpecialization";
      _Class.ownedTemplateSignature = "ownedTemplateSignature";
      _Class.ownedUseCase = "ownedUseCase";
      _Class.powertypeExtent = "powertypeExtent";
      _Class.redefinedClassifier = "redefinedClassifier";
      _Class.representation = "representation";
      _Class.substitution = "substitution";
      _Class.templateParameter = "templateParameter";
      _Class.useCase = "useCase";
      _Class.elementImport = "elementImport";
      _Class.importedMember = "importedMember";
      _Class.member = "member";
      _Class.ownedMember = "ownedMember";
      _Class.ownedRule = "ownedRule";
      _Class.packageImport = "packageImport";
      _Class.clientDependency = "clientDependency";
      _Class._name_ = "name";
      _Class.nameExpression = "nameExpression";
      _Class.namespace = "namespace";
      _Class.qualifiedName = "qualifiedName";
      _Class.visibility = "visibility";
      _Class.ownedComment = "ownedComment";
      _Class.ownedElement = "ownedElement";
      _Class.owner = "owner";
      _Class._package_ = "package";
      _Class.owningTemplateParameter = "owningTemplateParameter";
      _Class.templateBinding = "templateBinding";
      _Class.isLeaf = "isLeaf";
      _Class.redefinedElement = "redefinedElement";
      _Class.redefinitionContext = "redefinitionContext";
      _Class.ownedPort = "ownedPort";
      _Class.ownedConnector = "ownedConnector";
      _Class.part = "part";
      _Class.role = "role";
      _StructuredClassifiers2._Class = _Class;
      _StructuredClassifiers2.__Class_Uri = "dm:///_internal/model/uml#Class";
      class _Collaboration {
      }
      _Collaboration.collaborationRole = "collaborationRole";
      _Collaboration.ownedAttribute = "ownedAttribute";
      _Collaboration.ownedConnector = "ownedConnector";
      _Collaboration.part = "part";
      _Collaboration.role = "role";
      _Collaboration.attribute = "attribute";
      _Collaboration.collaborationUse = "collaborationUse";
      _Collaboration.feature = "feature";
      _Collaboration.general = "general";
      _Collaboration.generalization = "generalization";
      _Collaboration.inheritedMember = "inheritedMember";
      _Collaboration.isAbstract = "isAbstract";
      _Collaboration.isFinalSpecialization = "isFinalSpecialization";
      _Collaboration.ownedTemplateSignature = "ownedTemplateSignature";
      _Collaboration.ownedUseCase = "ownedUseCase";
      _Collaboration.powertypeExtent = "powertypeExtent";
      _Collaboration.redefinedClassifier = "redefinedClassifier";
      _Collaboration.representation = "representation";
      _Collaboration.substitution = "substitution";
      _Collaboration.templateParameter = "templateParameter";
      _Collaboration.useCase = "useCase";
      _Collaboration.elementImport = "elementImport";
      _Collaboration.importedMember = "importedMember";
      _Collaboration.member = "member";
      _Collaboration.ownedMember = "ownedMember";
      _Collaboration.ownedRule = "ownedRule";
      _Collaboration.packageImport = "packageImport";
      _Collaboration.clientDependency = "clientDependency";
      _Collaboration._name_ = "name";
      _Collaboration.nameExpression = "nameExpression";
      _Collaboration.namespace = "namespace";
      _Collaboration.qualifiedName = "qualifiedName";
      _Collaboration.visibility = "visibility";
      _Collaboration.ownedComment = "ownedComment";
      _Collaboration.ownedElement = "ownedElement";
      _Collaboration.owner = "owner";
      _Collaboration._package_ = "package";
      _Collaboration.owningTemplateParameter = "owningTemplateParameter";
      _Collaboration.templateBinding = "templateBinding";
      _Collaboration.isLeaf = "isLeaf";
      _Collaboration.redefinedElement = "redefinedElement";
      _Collaboration.redefinitionContext = "redefinitionContext";
      _Collaboration.classifierBehavior = "classifierBehavior";
      _Collaboration.interfaceRealization = "interfaceRealization";
      _Collaboration.ownedBehavior = "ownedBehavior";
      _StructuredClassifiers2._Collaboration = _Collaboration;
      _StructuredClassifiers2.__Collaboration_Uri = "dm:///_internal/model/uml#Collaboration";
      class _CollaborationUse {
      }
      _CollaborationUse.roleBinding = "roleBinding";
      _CollaborationUse.type = "type";
      _CollaborationUse.clientDependency = "clientDependency";
      _CollaborationUse._name_ = "name";
      _CollaborationUse.nameExpression = "nameExpression";
      _CollaborationUse.namespace = "namespace";
      _CollaborationUse.qualifiedName = "qualifiedName";
      _CollaborationUse.visibility = "visibility";
      _CollaborationUse.ownedComment = "ownedComment";
      _CollaborationUse.ownedElement = "ownedElement";
      _CollaborationUse.owner = "owner";
      _StructuredClassifiers2._CollaborationUse = _CollaborationUse;
      _StructuredClassifiers2.__CollaborationUse_Uri = "dm:///_internal/model/uml#CollaborationUse";
      class _Component {
      }
      _Component.isIndirectlyInstantiated = "isIndirectlyInstantiated";
      _Component.packagedElement = "packagedElement";
      _Component.provided = "provided";
      _Component.realization = "realization";
      _Component.required = "required";
      _Component.extension = "extension";
      _Component.isAbstract = "isAbstract";
      _Component.isActive = "isActive";
      _Component.nestedClassifier = "nestedClassifier";
      _Component.ownedAttribute = "ownedAttribute";
      _Component.ownedOperation = "ownedOperation";
      _Component.ownedReception = "ownedReception";
      _Component.superClass = "superClass";
      _Component.classifierBehavior = "classifierBehavior";
      _Component.interfaceRealization = "interfaceRealization";
      _Component.ownedBehavior = "ownedBehavior";
      _Component.attribute = "attribute";
      _Component.collaborationUse = "collaborationUse";
      _Component.feature = "feature";
      _Component.general = "general";
      _Component.generalization = "generalization";
      _Component.inheritedMember = "inheritedMember";
      _Component.isFinalSpecialization = "isFinalSpecialization";
      _Component.ownedTemplateSignature = "ownedTemplateSignature";
      _Component.ownedUseCase = "ownedUseCase";
      _Component.powertypeExtent = "powertypeExtent";
      _Component.redefinedClassifier = "redefinedClassifier";
      _Component.representation = "representation";
      _Component.substitution = "substitution";
      _Component.templateParameter = "templateParameter";
      _Component.useCase = "useCase";
      _Component.elementImport = "elementImport";
      _Component.importedMember = "importedMember";
      _Component.member = "member";
      _Component.ownedMember = "ownedMember";
      _Component.ownedRule = "ownedRule";
      _Component.packageImport = "packageImport";
      _Component.clientDependency = "clientDependency";
      _Component._name_ = "name";
      _Component.nameExpression = "nameExpression";
      _Component.namespace = "namespace";
      _Component.qualifiedName = "qualifiedName";
      _Component.visibility = "visibility";
      _Component.ownedComment = "ownedComment";
      _Component.ownedElement = "ownedElement";
      _Component.owner = "owner";
      _Component._package_ = "package";
      _Component.owningTemplateParameter = "owningTemplateParameter";
      _Component.templateBinding = "templateBinding";
      _Component.isLeaf = "isLeaf";
      _Component.redefinedElement = "redefinedElement";
      _Component.redefinitionContext = "redefinitionContext";
      _Component.ownedPort = "ownedPort";
      _Component.ownedConnector = "ownedConnector";
      _Component.part = "part";
      _Component.role = "role";
      _StructuredClassifiers2._Component = _Component;
      _StructuredClassifiers2.__Component_Uri = "dm:///_internal/model/uml#Component";
      class _ComponentRealization {
      }
      _ComponentRealization.abstraction = "abstraction";
      _ComponentRealization.realizingClassifier = "realizingClassifier";
      _ComponentRealization.mapping = "mapping";
      _ComponentRealization.client = "client";
      _ComponentRealization.supplier = "supplier";
      _ComponentRealization.source = "source";
      _ComponentRealization.target = "target";
      _ComponentRealization.relatedElement = "relatedElement";
      _ComponentRealization.ownedComment = "ownedComment";
      _ComponentRealization.ownedElement = "ownedElement";
      _ComponentRealization.owner = "owner";
      _ComponentRealization.visibility = "visibility";
      _ComponentRealization.owningTemplateParameter = "owningTemplateParameter";
      _ComponentRealization.templateParameter = "templateParameter";
      _ComponentRealization.clientDependency = "clientDependency";
      _ComponentRealization._name_ = "name";
      _ComponentRealization.nameExpression = "nameExpression";
      _ComponentRealization.namespace = "namespace";
      _ComponentRealization.qualifiedName = "qualifiedName";
      _StructuredClassifiers2._ComponentRealization = _ComponentRealization;
      _StructuredClassifiers2.__ComponentRealization_Uri = "dm:///_internal/model/uml#ComponentRealization";
      class _ConnectableElement {
      }
      _ConnectableElement.end = "end";
      _ConnectableElement.templateParameter = "templateParameter";
      _ConnectableElement.type = "type";
      _ConnectableElement.clientDependency = "clientDependency";
      _ConnectableElement._name_ = "name";
      _ConnectableElement.nameExpression = "nameExpression";
      _ConnectableElement.namespace = "namespace";
      _ConnectableElement.qualifiedName = "qualifiedName";
      _ConnectableElement.visibility = "visibility";
      _ConnectableElement.ownedComment = "ownedComment";
      _ConnectableElement.ownedElement = "ownedElement";
      _ConnectableElement.owner = "owner";
      _ConnectableElement.owningTemplateParameter = "owningTemplateParameter";
      _StructuredClassifiers2._ConnectableElement = _ConnectableElement;
      _StructuredClassifiers2.__ConnectableElement_Uri = "dm:///_internal/model/uml#ConnectableElement";
      class _ConnectableElementTemplateParameter {
      }
      _ConnectableElementTemplateParameter.parameteredElement = "parameteredElement";
      _ConnectableElementTemplateParameter._default_ = "default";
      _ConnectableElementTemplateParameter.ownedDefault = "ownedDefault";
      _ConnectableElementTemplateParameter.ownedParameteredElement = "ownedParameteredElement";
      _ConnectableElementTemplateParameter.signature = "signature";
      _ConnectableElementTemplateParameter.ownedComment = "ownedComment";
      _ConnectableElementTemplateParameter.ownedElement = "ownedElement";
      _ConnectableElementTemplateParameter.owner = "owner";
      _StructuredClassifiers2._ConnectableElementTemplateParameter = _ConnectableElementTemplateParameter;
      _StructuredClassifiers2.__ConnectableElementTemplateParameter_Uri = "dm:///_internal/model/uml#ConnectableElementTemplateParameter";
      class _Connector {
      }
      _Connector.contract = "contract";
      _Connector.end = "end";
      _Connector.kind = "kind";
      _Connector.redefinedConnector = "redefinedConnector";
      _Connector.type = "type";
      _Connector.featuringClassifier = "featuringClassifier";
      _Connector.isStatic = "isStatic";
      _Connector.isLeaf = "isLeaf";
      _Connector.redefinedElement = "redefinedElement";
      _Connector.redefinitionContext = "redefinitionContext";
      _Connector.clientDependency = "clientDependency";
      _Connector._name_ = "name";
      _Connector.nameExpression = "nameExpression";
      _Connector.namespace = "namespace";
      _Connector.qualifiedName = "qualifiedName";
      _Connector.visibility = "visibility";
      _Connector.ownedComment = "ownedComment";
      _Connector.ownedElement = "ownedElement";
      _Connector.owner = "owner";
      _StructuredClassifiers2._Connector = _Connector;
      _StructuredClassifiers2.__Connector_Uri = "dm:///_internal/model/uml#Connector";
      class _ConnectorEnd {
      }
      _ConnectorEnd.definingEnd = "definingEnd";
      _ConnectorEnd.partWithPort = "partWithPort";
      _ConnectorEnd.role = "role";
      _ConnectorEnd.isOrdered = "isOrdered";
      _ConnectorEnd.isUnique = "isUnique";
      _ConnectorEnd.lower = "lower";
      _ConnectorEnd.lowerValue = "lowerValue";
      _ConnectorEnd.upper = "upper";
      _ConnectorEnd.upperValue = "upperValue";
      _ConnectorEnd.ownedComment = "ownedComment";
      _ConnectorEnd.ownedElement = "ownedElement";
      _ConnectorEnd.owner = "owner";
      _StructuredClassifiers2._ConnectorEnd = _ConnectorEnd;
      _StructuredClassifiers2.__ConnectorEnd_Uri = "dm:///_internal/model/uml#ConnectorEnd";
      class _EncapsulatedClassifier {
      }
      _EncapsulatedClassifier.ownedPort = "ownedPort";
      _EncapsulatedClassifier.ownedAttribute = "ownedAttribute";
      _EncapsulatedClassifier.ownedConnector = "ownedConnector";
      _EncapsulatedClassifier.part = "part";
      _EncapsulatedClassifier.role = "role";
      _EncapsulatedClassifier.attribute = "attribute";
      _EncapsulatedClassifier.collaborationUse = "collaborationUse";
      _EncapsulatedClassifier.feature = "feature";
      _EncapsulatedClassifier.general = "general";
      _EncapsulatedClassifier.generalization = "generalization";
      _EncapsulatedClassifier.inheritedMember = "inheritedMember";
      _EncapsulatedClassifier.isAbstract = "isAbstract";
      _EncapsulatedClassifier.isFinalSpecialization = "isFinalSpecialization";
      _EncapsulatedClassifier.ownedTemplateSignature = "ownedTemplateSignature";
      _EncapsulatedClassifier.ownedUseCase = "ownedUseCase";
      _EncapsulatedClassifier.powertypeExtent = "powertypeExtent";
      _EncapsulatedClassifier.redefinedClassifier = "redefinedClassifier";
      _EncapsulatedClassifier.representation = "representation";
      _EncapsulatedClassifier.substitution = "substitution";
      _EncapsulatedClassifier.templateParameter = "templateParameter";
      _EncapsulatedClassifier.useCase = "useCase";
      _EncapsulatedClassifier.elementImport = "elementImport";
      _EncapsulatedClassifier.importedMember = "importedMember";
      _EncapsulatedClassifier.member = "member";
      _EncapsulatedClassifier.ownedMember = "ownedMember";
      _EncapsulatedClassifier.ownedRule = "ownedRule";
      _EncapsulatedClassifier.packageImport = "packageImport";
      _EncapsulatedClassifier.clientDependency = "clientDependency";
      _EncapsulatedClassifier._name_ = "name";
      _EncapsulatedClassifier.nameExpression = "nameExpression";
      _EncapsulatedClassifier.namespace = "namespace";
      _EncapsulatedClassifier.qualifiedName = "qualifiedName";
      _EncapsulatedClassifier.visibility = "visibility";
      _EncapsulatedClassifier.ownedComment = "ownedComment";
      _EncapsulatedClassifier.ownedElement = "ownedElement";
      _EncapsulatedClassifier.owner = "owner";
      _EncapsulatedClassifier._package_ = "package";
      _EncapsulatedClassifier.owningTemplateParameter = "owningTemplateParameter";
      _EncapsulatedClassifier.templateBinding = "templateBinding";
      _EncapsulatedClassifier.isLeaf = "isLeaf";
      _EncapsulatedClassifier.redefinedElement = "redefinedElement";
      _EncapsulatedClassifier.redefinitionContext = "redefinitionContext";
      _StructuredClassifiers2._EncapsulatedClassifier = _EncapsulatedClassifier;
      _StructuredClassifiers2.__EncapsulatedClassifier_Uri = "dm:///_internal/model/uml#EncapsulatedClassifier";
      class _Port {
      }
      _Port.isBehavior = "isBehavior";
      _Port.isConjugated = "isConjugated";
      _Port.isService = "isService";
      _Port.protocol = "protocol";
      _Port.provided = "provided";
      _Port.redefinedPort = "redefinedPort";
      _Port.required = "required";
      _Port.aggregation = "aggregation";
      _Port.association = "association";
      _Port.associationEnd = "associationEnd";
      _Port._class_ = "class";
      _Port.datatype = "datatype";
      _Port.defaultValue = "defaultValue";
      _Port._interface_ = "interface";
      _Port.isComposite = "isComposite";
      _Port.isDerived = "isDerived";
      _Port.isDerivedUnion = "isDerivedUnion";
      _Port.isID = "isID";
      _Port.opposite = "opposite";
      _Port.owningAssociation = "owningAssociation";
      _Port.qualifier = "qualifier";
      _Port.redefinedProperty = "redefinedProperty";
      _Port.subsettedProperty = "subsettedProperty";
      _Port.end = "end";
      _Port.templateParameter = "templateParameter";
      _Port.type = "type";
      _Port.clientDependency = "clientDependency";
      _Port._name_ = "name";
      _Port.nameExpression = "nameExpression";
      _Port.namespace = "namespace";
      _Port.qualifiedName = "qualifiedName";
      _Port.visibility = "visibility";
      _Port.ownedComment = "ownedComment";
      _Port.ownedElement = "ownedElement";
      _Port.owner = "owner";
      _Port.owningTemplateParameter = "owningTemplateParameter";
      _Port.deployedElement = "deployedElement";
      _Port.deployment = "deployment";
      _Port.isReadOnly = "isReadOnly";
      _Port.isOrdered = "isOrdered";
      _Port.isUnique = "isUnique";
      _Port.lower = "lower";
      _Port.lowerValue = "lowerValue";
      _Port.upper = "upper";
      _Port.upperValue = "upperValue";
      _Port.featuringClassifier = "featuringClassifier";
      _Port.isStatic = "isStatic";
      _Port.isLeaf = "isLeaf";
      _Port.redefinedElement = "redefinedElement";
      _Port.redefinitionContext = "redefinitionContext";
      _StructuredClassifiers2._Port = _Port;
      _StructuredClassifiers2.__Port_Uri = "dm:///_internal/model/uml#Port";
      class _StructuredClassifier {
      }
      _StructuredClassifier.ownedAttribute = "ownedAttribute";
      _StructuredClassifier.ownedConnector = "ownedConnector";
      _StructuredClassifier.part = "part";
      _StructuredClassifier.role = "role";
      _StructuredClassifier.attribute = "attribute";
      _StructuredClassifier.collaborationUse = "collaborationUse";
      _StructuredClassifier.feature = "feature";
      _StructuredClassifier.general = "general";
      _StructuredClassifier.generalization = "generalization";
      _StructuredClassifier.inheritedMember = "inheritedMember";
      _StructuredClassifier.isAbstract = "isAbstract";
      _StructuredClassifier.isFinalSpecialization = "isFinalSpecialization";
      _StructuredClassifier.ownedTemplateSignature = "ownedTemplateSignature";
      _StructuredClassifier.ownedUseCase = "ownedUseCase";
      _StructuredClassifier.powertypeExtent = "powertypeExtent";
      _StructuredClassifier.redefinedClassifier = "redefinedClassifier";
      _StructuredClassifier.representation = "representation";
      _StructuredClassifier.substitution = "substitution";
      _StructuredClassifier.templateParameter = "templateParameter";
      _StructuredClassifier.useCase = "useCase";
      _StructuredClassifier.elementImport = "elementImport";
      _StructuredClassifier.importedMember = "importedMember";
      _StructuredClassifier.member = "member";
      _StructuredClassifier.ownedMember = "ownedMember";
      _StructuredClassifier.ownedRule = "ownedRule";
      _StructuredClassifier.packageImport = "packageImport";
      _StructuredClassifier.clientDependency = "clientDependency";
      _StructuredClassifier._name_ = "name";
      _StructuredClassifier.nameExpression = "nameExpression";
      _StructuredClassifier.namespace = "namespace";
      _StructuredClassifier.qualifiedName = "qualifiedName";
      _StructuredClassifier.visibility = "visibility";
      _StructuredClassifier.ownedComment = "ownedComment";
      _StructuredClassifier.ownedElement = "ownedElement";
      _StructuredClassifier.owner = "owner";
      _StructuredClassifier._package_ = "package";
      _StructuredClassifier.owningTemplateParameter = "owningTemplateParameter";
      _StructuredClassifier.templateBinding = "templateBinding";
      _StructuredClassifier.isLeaf = "isLeaf";
      _StructuredClassifier.redefinedElement = "redefinedElement";
      _StructuredClassifier.redefinitionContext = "redefinitionContext";
      _StructuredClassifiers2._StructuredClassifier = _StructuredClassifier;
      _StructuredClassifiers2.__StructuredClassifier_Uri = "dm:///_internal/model/uml#StructuredClassifier";
      let _ConnectorKind;
      (function(_ConnectorKind2) {
        _ConnectorKind2.assembly = "assembly";
        _ConnectorKind2.delegation = "delegation";
      })(_ConnectorKind = _StructuredClassifiers2._ConnectorKind || (_StructuredClassifiers2._ConnectorKind = {}));
      let ___ConnectorKind;
      (function(___ConnectorKind2) {
        ___ConnectorKind2[___ConnectorKind2["assembly"] = 0] = "assembly";
        ___ConnectorKind2[___ConnectorKind2["delegation"] = 1] = "delegation";
      })(___ConnectorKind = _StructuredClassifiers2.___ConnectorKind || (_StructuredClassifiers2.___ConnectorKind = {}));
    })(_StructuredClassifiers = _UML2._StructuredClassifiers || (_UML2._StructuredClassifiers = {}));
    let _StateMachines;
    (function(_StateMachines2) {
      class _ConnectionPointReference {
      }
      _ConnectionPointReference.entry = "entry";
      _ConnectionPointReference.exit = "exit";
      _ConnectionPointReference.state = "state";
      _ConnectionPointReference.container = "container";
      _ConnectionPointReference.incoming = "incoming";
      _ConnectionPointReference.outgoing = "outgoing";
      _ConnectionPointReference.clientDependency = "clientDependency";
      _ConnectionPointReference._name_ = "name";
      _ConnectionPointReference.nameExpression = "nameExpression";
      _ConnectionPointReference.namespace = "namespace";
      _ConnectionPointReference.qualifiedName = "qualifiedName";
      _ConnectionPointReference.visibility = "visibility";
      _ConnectionPointReference.ownedComment = "ownedComment";
      _ConnectionPointReference.ownedElement = "ownedElement";
      _ConnectionPointReference.owner = "owner";
      _StateMachines2._ConnectionPointReference = _ConnectionPointReference;
      _StateMachines2.__ConnectionPointReference_Uri = "dm:///_internal/model/uml#ConnectionPointReference";
      class _FinalState {
      }
      _FinalState.connection = "connection";
      _FinalState.connectionPoint = "connectionPoint";
      _FinalState.deferrableTrigger = "deferrableTrigger";
      _FinalState.doActivity = "doActivity";
      _FinalState.entry = "entry";
      _FinalState.exit = "exit";
      _FinalState.isComposite = "isComposite";
      _FinalState.isOrthogonal = "isOrthogonal";
      _FinalState.isSimple = "isSimple";
      _FinalState.isSubmachineState = "isSubmachineState";
      _FinalState.redefinedState = "redefinedState";
      _FinalState.redefinitionContext = "redefinitionContext";
      _FinalState.region = "region";
      _FinalState.stateInvariant = "stateInvariant";
      _FinalState.submachine = "submachine";
      _FinalState.isLeaf = "isLeaf";
      _FinalState.redefinedElement = "redefinedElement";
      _FinalState.clientDependency = "clientDependency";
      _FinalState._name_ = "name";
      _FinalState.nameExpression = "nameExpression";
      _FinalState.namespace = "namespace";
      _FinalState.qualifiedName = "qualifiedName";
      _FinalState.visibility = "visibility";
      _FinalState.ownedComment = "ownedComment";
      _FinalState.ownedElement = "ownedElement";
      _FinalState.owner = "owner";
      _FinalState.elementImport = "elementImport";
      _FinalState.importedMember = "importedMember";
      _FinalState.member = "member";
      _FinalState.ownedMember = "ownedMember";
      _FinalState.ownedRule = "ownedRule";
      _FinalState.packageImport = "packageImport";
      _FinalState.container = "container";
      _FinalState.incoming = "incoming";
      _FinalState.outgoing = "outgoing";
      _StateMachines2._FinalState = _FinalState;
      _StateMachines2.__FinalState_Uri = "dm:///_internal/model/uml#FinalState";
      class _ProtocolConformance {
      }
      _ProtocolConformance.generalMachine = "generalMachine";
      _ProtocolConformance.specificMachine = "specificMachine";
      _ProtocolConformance.source = "source";
      _ProtocolConformance.target = "target";
      _ProtocolConformance.relatedElement = "relatedElement";
      _ProtocolConformance.ownedComment = "ownedComment";
      _ProtocolConformance.ownedElement = "ownedElement";
      _ProtocolConformance.owner = "owner";
      _StateMachines2._ProtocolConformance = _ProtocolConformance;
      _StateMachines2.__ProtocolConformance_Uri = "dm:///_internal/model/uml#ProtocolConformance";
      class _ProtocolStateMachine {
      }
      _ProtocolStateMachine.conformance = "conformance";
      _ProtocolStateMachine.connectionPoint = "connectionPoint";
      _ProtocolStateMachine.extendedStateMachine = "extendedStateMachine";
      _ProtocolStateMachine.region = "region";
      _ProtocolStateMachine.submachineState = "submachineState";
      _ProtocolStateMachine.context = "context";
      _ProtocolStateMachine.isReentrant = "isReentrant";
      _ProtocolStateMachine.ownedParameter = "ownedParameter";
      _ProtocolStateMachine.ownedParameterSet = "ownedParameterSet";
      _ProtocolStateMachine.postcondition = "postcondition";
      _ProtocolStateMachine.precondition = "precondition";
      _ProtocolStateMachine.specification = "specification";
      _ProtocolStateMachine.redefinedBehavior = "redefinedBehavior";
      _ProtocolStateMachine.extension = "extension";
      _ProtocolStateMachine.isAbstract = "isAbstract";
      _ProtocolStateMachine.isActive = "isActive";
      _ProtocolStateMachine.nestedClassifier = "nestedClassifier";
      _ProtocolStateMachine.ownedAttribute = "ownedAttribute";
      _ProtocolStateMachine.ownedOperation = "ownedOperation";
      _ProtocolStateMachine.ownedReception = "ownedReception";
      _ProtocolStateMachine.superClass = "superClass";
      _ProtocolStateMachine.classifierBehavior = "classifierBehavior";
      _ProtocolStateMachine.interfaceRealization = "interfaceRealization";
      _ProtocolStateMachine.ownedBehavior = "ownedBehavior";
      _ProtocolStateMachine.attribute = "attribute";
      _ProtocolStateMachine.collaborationUse = "collaborationUse";
      _ProtocolStateMachine.feature = "feature";
      _ProtocolStateMachine.general = "general";
      _ProtocolStateMachine.generalization = "generalization";
      _ProtocolStateMachine.inheritedMember = "inheritedMember";
      _ProtocolStateMachine.isFinalSpecialization = "isFinalSpecialization";
      _ProtocolStateMachine.ownedTemplateSignature = "ownedTemplateSignature";
      _ProtocolStateMachine.ownedUseCase = "ownedUseCase";
      _ProtocolStateMachine.powertypeExtent = "powertypeExtent";
      _ProtocolStateMachine.redefinedClassifier = "redefinedClassifier";
      _ProtocolStateMachine.representation = "representation";
      _ProtocolStateMachine.substitution = "substitution";
      _ProtocolStateMachine.templateParameter = "templateParameter";
      _ProtocolStateMachine.useCase = "useCase";
      _ProtocolStateMachine.elementImport = "elementImport";
      _ProtocolStateMachine.importedMember = "importedMember";
      _ProtocolStateMachine.member = "member";
      _ProtocolStateMachine.ownedMember = "ownedMember";
      _ProtocolStateMachine.ownedRule = "ownedRule";
      _ProtocolStateMachine.packageImport = "packageImport";
      _ProtocolStateMachine.clientDependency = "clientDependency";
      _ProtocolStateMachine._name_ = "name";
      _ProtocolStateMachine.nameExpression = "nameExpression";
      _ProtocolStateMachine.namespace = "namespace";
      _ProtocolStateMachine.qualifiedName = "qualifiedName";
      _ProtocolStateMachine.visibility = "visibility";
      _ProtocolStateMachine.ownedComment = "ownedComment";
      _ProtocolStateMachine.ownedElement = "ownedElement";
      _ProtocolStateMachine.owner = "owner";
      _ProtocolStateMachine._package_ = "package";
      _ProtocolStateMachine.owningTemplateParameter = "owningTemplateParameter";
      _ProtocolStateMachine.templateBinding = "templateBinding";
      _ProtocolStateMachine.isLeaf = "isLeaf";
      _ProtocolStateMachine.redefinedElement = "redefinedElement";
      _ProtocolStateMachine.redefinitionContext = "redefinitionContext";
      _ProtocolStateMachine.ownedPort = "ownedPort";
      _ProtocolStateMachine.ownedConnector = "ownedConnector";
      _ProtocolStateMachine.part = "part";
      _ProtocolStateMachine.role = "role";
      _StateMachines2._ProtocolStateMachine = _ProtocolStateMachine;
      _StateMachines2.__ProtocolStateMachine_Uri = "dm:///_internal/model/uml#ProtocolStateMachine";
      class _ProtocolTransition {
      }
      _ProtocolTransition.postCondition = "postCondition";
      _ProtocolTransition.preCondition = "preCondition";
      _ProtocolTransition.referred = "referred";
      _ProtocolTransition.container = "container";
      _ProtocolTransition.effect = "effect";
      _ProtocolTransition.guard = "guard";
      _ProtocolTransition.kind = "kind";
      _ProtocolTransition.redefinedTransition = "redefinedTransition";
      _ProtocolTransition.redefinitionContext = "redefinitionContext";
      _ProtocolTransition.source = "source";
      _ProtocolTransition.target = "target";
      _ProtocolTransition.trigger = "trigger";
      _ProtocolTransition.elementImport = "elementImport";
      _ProtocolTransition.importedMember = "importedMember";
      _ProtocolTransition.member = "member";
      _ProtocolTransition.ownedMember = "ownedMember";
      _ProtocolTransition.ownedRule = "ownedRule";
      _ProtocolTransition.packageImport = "packageImport";
      _ProtocolTransition.clientDependency = "clientDependency";
      _ProtocolTransition._name_ = "name";
      _ProtocolTransition.nameExpression = "nameExpression";
      _ProtocolTransition.namespace = "namespace";
      _ProtocolTransition.qualifiedName = "qualifiedName";
      _ProtocolTransition.visibility = "visibility";
      _ProtocolTransition.ownedComment = "ownedComment";
      _ProtocolTransition.ownedElement = "ownedElement";
      _ProtocolTransition.owner = "owner";
      _ProtocolTransition.isLeaf = "isLeaf";
      _ProtocolTransition.redefinedElement = "redefinedElement";
      _StateMachines2._ProtocolTransition = _ProtocolTransition;
      _StateMachines2.__ProtocolTransition_Uri = "dm:///_internal/model/uml#ProtocolTransition";
      class _Pseudostate {
      }
      _Pseudostate.kind = "kind";
      _Pseudostate.state = "state";
      _Pseudostate.stateMachine = "stateMachine";
      _Pseudostate.container = "container";
      _Pseudostate.incoming = "incoming";
      _Pseudostate.outgoing = "outgoing";
      _Pseudostate.clientDependency = "clientDependency";
      _Pseudostate._name_ = "name";
      _Pseudostate.nameExpression = "nameExpression";
      _Pseudostate.namespace = "namespace";
      _Pseudostate.qualifiedName = "qualifiedName";
      _Pseudostate.visibility = "visibility";
      _Pseudostate.ownedComment = "ownedComment";
      _Pseudostate.ownedElement = "ownedElement";
      _Pseudostate.owner = "owner";
      _StateMachines2._Pseudostate = _Pseudostate;
      _StateMachines2.__Pseudostate_Uri = "dm:///_internal/model/uml#Pseudostate";
      class _Region {
      }
      _Region.extendedRegion = "extendedRegion";
      _Region.redefinitionContext = "redefinitionContext";
      _Region.state = "state";
      _Region.stateMachine = "stateMachine";
      _Region.subvertex = "subvertex";
      _Region.transition = "transition";
      _Region.elementImport = "elementImport";
      _Region.importedMember = "importedMember";
      _Region.member = "member";
      _Region.ownedMember = "ownedMember";
      _Region.ownedRule = "ownedRule";
      _Region.packageImport = "packageImport";
      _Region.clientDependency = "clientDependency";
      _Region._name_ = "name";
      _Region.nameExpression = "nameExpression";
      _Region.namespace = "namespace";
      _Region.qualifiedName = "qualifiedName";
      _Region.visibility = "visibility";
      _Region.ownedComment = "ownedComment";
      _Region.ownedElement = "ownedElement";
      _Region.owner = "owner";
      _Region.isLeaf = "isLeaf";
      _Region.redefinedElement = "redefinedElement";
      _StateMachines2._Region = _Region;
      _StateMachines2.__Region_Uri = "dm:///_internal/model/uml#Region";
      class _State {
      }
      _State.connection = "connection";
      _State.connectionPoint = "connectionPoint";
      _State.deferrableTrigger = "deferrableTrigger";
      _State.doActivity = "doActivity";
      _State.entry = "entry";
      _State.exit = "exit";
      _State.isComposite = "isComposite";
      _State.isOrthogonal = "isOrthogonal";
      _State.isSimple = "isSimple";
      _State.isSubmachineState = "isSubmachineState";
      _State.redefinedState = "redefinedState";
      _State.redefinitionContext = "redefinitionContext";
      _State.region = "region";
      _State.stateInvariant = "stateInvariant";
      _State.submachine = "submachine";
      _State.isLeaf = "isLeaf";
      _State.redefinedElement = "redefinedElement";
      _State.clientDependency = "clientDependency";
      _State._name_ = "name";
      _State.nameExpression = "nameExpression";
      _State.namespace = "namespace";
      _State.qualifiedName = "qualifiedName";
      _State.visibility = "visibility";
      _State.ownedComment = "ownedComment";
      _State.ownedElement = "ownedElement";
      _State.owner = "owner";
      _State.elementImport = "elementImport";
      _State.importedMember = "importedMember";
      _State.member = "member";
      _State.ownedMember = "ownedMember";
      _State.ownedRule = "ownedRule";
      _State.packageImport = "packageImport";
      _State.container = "container";
      _State.incoming = "incoming";
      _State.outgoing = "outgoing";
      _StateMachines2._State = _State;
      _StateMachines2.__State_Uri = "dm:///_internal/model/uml#State";
      class _StateMachine {
      }
      _StateMachine.connectionPoint = "connectionPoint";
      _StateMachine.extendedStateMachine = "extendedStateMachine";
      _StateMachine.region = "region";
      _StateMachine.submachineState = "submachineState";
      _StateMachine.context = "context";
      _StateMachine.isReentrant = "isReentrant";
      _StateMachine.ownedParameter = "ownedParameter";
      _StateMachine.ownedParameterSet = "ownedParameterSet";
      _StateMachine.postcondition = "postcondition";
      _StateMachine.precondition = "precondition";
      _StateMachine.specification = "specification";
      _StateMachine.redefinedBehavior = "redefinedBehavior";
      _StateMachine.extension = "extension";
      _StateMachine.isAbstract = "isAbstract";
      _StateMachine.isActive = "isActive";
      _StateMachine.nestedClassifier = "nestedClassifier";
      _StateMachine.ownedAttribute = "ownedAttribute";
      _StateMachine.ownedOperation = "ownedOperation";
      _StateMachine.ownedReception = "ownedReception";
      _StateMachine.superClass = "superClass";
      _StateMachine.classifierBehavior = "classifierBehavior";
      _StateMachine.interfaceRealization = "interfaceRealization";
      _StateMachine.ownedBehavior = "ownedBehavior";
      _StateMachine.attribute = "attribute";
      _StateMachine.collaborationUse = "collaborationUse";
      _StateMachine.feature = "feature";
      _StateMachine.general = "general";
      _StateMachine.generalization = "generalization";
      _StateMachine.inheritedMember = "inheritedMember";
      _StateMachine.isFinalSpecialization = "isFinalSpecialization";
      _StateMachine.ownedTemplateSignature = "ownedTemplateSignature";
      _StateMachine.ownedUseCase = "ownedUseCase";
      _StateMachine.powertypeExtent = "powertypeExtent";
      _StateMachine.redefinedClassifier = "redefinedClassifier";
      _StateMachine.representation = "representation";
      _StateMachine.substitution = "substitution";
      _StateMachine.templateParameter = "templateParameter";
      _StateMachine.useCase = "useCase";
      _StateMachine.elementImport = "elementImport";
      _StateMachine.importedMember = "importedMember";
      _StateMachine.member = "member";
      _StateMachine.ownedMember = "ownedMember";
      _StateMachine.ownedRule = "ownedRule";
      _StateMachine.packageImport = "packageImport";
      _StateMachine.clientDependency = "clientDependency";
      _StateMachine._name_ = "name";
      _StateMachine.nameExpression = "nameExpression";
      _StateMachine.namespace = "namespace";
      _StateMachine.qualifiedName = "qualifiedName";
      _StateMachine.visibility = "visibility";
      _StateMachine.ownedComment = "ownedComment";
      _StateMachine.ownedElement = "ownedElement";
      _StateMachine.owner = "owner";
      _StateMachine._package_ = "package";
      _StateMachine.owningTemplateParameter = "owningTemplateParameter";
      _StateMachine.templateBinding = "templateBinding";
      _StateMachine.isLeaf = "isLeaf";
      _StateMachine.redefinedElement = "redefinedElement";
      _StateMachine.redefinitionContext = "redefinitionContext";
      _StateMachine.ownedPort = "ownedPort";
      _StateMachine.ownedConnector = "ownedConnector";
      _StateMachine.part = "part";
      _StateMachine.role = "role";
      _StateMachines2._StateMachine = _StateMachine;
      _StateMachines2.__StateMachine_Uri = "dm:///_internal/model/uml#StateMachine";
      class _Transition {
      }
      _Transition.container = "container";
      _Transition.effect = "effect";
      _Transition.guard = "guard";
      _Transition.kind = "kind";
      _Transition.redefinedTransition = "redefinedTransition";
      _Transition.redefinitionContext = "redefinitionContext";
      _Transition.source = "source";
      _Transition.target = "target";
      _Transition.trigger = "trigger";
      _Transition.elementImport = "elementImport";
      _Transition.importedMember = "importedMember";
      _Transition.member = "member";
      _Transition.ownedMember = "ownedMember";
      _Transition.ownedRule = "ownedRule";
      _Transition.packageImport = "packageImport";
      _Transition.clientDependency = "clientDependency";
      _Transition._name_ = "name";
      _Transition.nameExpression = "nameExpression";
      _Transition.namespace = "namespace";
      _Transition.qualifiedName = "qualifiedName";
      _Transition.visibility = "visibility";
      _Transition.ownedComment = "ownedComment";
      _Transition.ownedElement = "ownedElement";
      _Transition.owner = "owner";
      _Transition.isLeaf = "isLeaf";
      _Transition.redefinedElement = "redefinedElement";
      _StateMachines2._Transition = _Transition;
      _StateMachines2.__Transition_Uri = "dm:///_internal/model/uml#Transition";
      class _Vertex {
      }
      _Vertex.container = "container";
      _Vertex.incoming = "incoming";
      _Vertex.outgoing = "outgoing";
      _Vertex.clientDependency = "clientDependency";
      _Vertex._name_ = "name";
      _Vertex.nameExpression = "nameExpression";
      _Vertex.namespace = "namespace";
      _Vertex.qualifiedName = "qualifiedName";
      _Vertex.visibility = "visibility";
      _Vertex.ownedComment = "ownedComment";
      _Vertex.ownedElement = "ownedElement";
      _Vertex.owner = "owner";
      _StateMachines2._Vertex = _Vertex;
      _StateMachines2.__Vertex_Uri = "dm:///_internal/model/uml#Vertex";
      let _PseudostateKind;
      (function(_PseudostateKind2) {
        _PseudostateKind2.initial = "initial";
        _PseudostateKind2.deepHistory = "deepHistory";
        _PseudostateKind2.shallowHistory = "shallowHistory";
        _PseudostateKind2.join = "join";
        _PseudostateKind2.fork = "fork";
        _PseudostateKind2.junction = "junction";
        _PseudostateKind2.choice = "choice";
        _PseudostateKind2.entryPoint = "entryPoint";
        _PseudostateKind2.exitPoint = "exitPoint";
        _PseudostateKind2.terminate = "terminate";
      })(_PseudostateKind = _StateMachines2._PseudostateKind || (_StateMachines2._PseudostateKind = {}));
      let ___PseudostateKind;
      (function(___PseudostateKind2) {
        ___PseudostateKind2[___PseudostateKind2["initial"] = 0] = "initial";
        ___PseudostateKind2[___PseudostateKind2["deepHistory"] = 1] = "deepHistory";
        ___PseudostateKind2[___PseudostateKind2["shallowHistory"] = 2] = "shallowHistory";
        ___PseudostateKind2[___PseudostateKind2["join"] = 3] = "join";
        ___PseudostateKind2[___PseudostateKind2["fork"] = 4] = "fork";
        ___PseudostateKind2[___PseudostateKind2["junction"] = 5] = "junction";
        ___PseudostateKind2[___PseudostateKind2["choice"] = 6] = "choice";
        ___PseudostateKind2[___PseudostateKind2["entryPoint"] = 7] = "entryPoint";
        ___PseudostateKind2[___PseudostateKind2["exitPoint"] = 8] = "exitPoint";
        ___PseudostateKind2[___PseudostateKind2["terminate"] = 9] = "terminate";
      })(___PseudostateKind = _StateMachines2.___PseudostateKind || (_StateMachines2.___PseudostateKind = {}));
      let _TransitionKind;
      (function(_TransitionKind2) {
        _TransitionKind2.internal = "internal";
        _TransitionKind2.local = "local";
        _TransitionKind2.external = "external";
      })(_TransitionKind = _StateMachines2._TransitionKind || (_StateMachines2._TransitionKind = {}));
      let ___TransitionKind;
      (function(___TransitionKind2) {
        ___TransitionKind2[___TransitionKind2["internal"] = 0] = "internal";
        ___TransitionKind2[___TransitionKind2["local"] = 1] = "local";
        ___TransitionKind2[___TransitionKind2["external"] = 2] = "external";
      })(___TransitionKind = _StateMachines2.___TransitionKind || (_StateMachines2.___TransitionKind = {}));
    })(_StateMachines = _UML2._StateMachines || (_UML2._StateMachines = {}));
    let _SimpleClassifiers;
    (function(_SimpleClassifiers2) {
      class _BehavioredClassifier {
      }
      _BehavioredClassifier.classifierBehavior = "classifierBehavior";
      _BehavioredClassifier.interfaceRealization = "interfaceRealization";
      _BehavioredClassifier.ownedBehavior = "ownedBehavior";
      _BehavioredClassifier.attribute = "attribute";
      _BehavioredClassifier.collaborationUse = "collaborationUse";
      _BehavioredClassifier.feature = "feature";
      _BehavioredClassifier.general = "general";
      _BehavioredClassifier.generalization = "generalization";
      _BehavioredClassifier.inheritedMember = "inheritedMember";
      _BehavioredClassifier.isAbstract = "isAbstract";
      _BehavioredClassifier.isFinalSpecialization = "isFinalSpecialization";
      _BehavioredClassifier.ownedTemplateSignature = "ownedTemplateSignature";
      _BehavioredClassifier.ownedUseCase = "ownedUseCase";
      _BehavioredClassifier.powertypeExtent = "powertypeExtent";
      _BehavioredClassifier.redefinedClassifier = "redefinedClassifier";
      _BehavioredClassifier.representation = "representation";
      _BehavioredClassifier.substitution = "substitution";
      _BehavioredClassifier.templateParameter = "templateParameter";
      _BehavioredClassifier.useCase = "useCase";
      _BehavioredClassifier.elementImport = "elementImport";
      _BehavioredClassifier.importedMember = "importedMember";
      _BehavioredClassifier.member = "member";
      _BehavioredClassifier.ownedMember = "ownedMember";
      _BehavioredClassifier.ownedRule = "ownedRule";
      _BehavioredClassifier.packageImport = "packageImport";
      _BehavioredClassifier.clientDependency = "clientDependency";
      _BehavioredClassifier._name_ = "name";
      _BehavioredClassifier.nameExpression = "nameExpression";
      _BehavioredClassifier.namespace = "namespace";
      _BehavioredClassifier.qualifiedName = "qualifiedName";
      _BehavioredClassifier.visibility = "visibility";
      _BehavioredClassifier.ownedComment = "ownedComment";
      _BehavioredClassifier.ownedElement = "ownedElement";
      _BehavioredClassifier.owner = "owner";
      _BehavioredClassifier._package_ = "package";
      _BehavioredClassifier.owningTemplateParameter = "owningTemplateParameter";
      _BehavioredClassifier.templateBinding = "templateBinding";
      _BehavioredClassifier.isLeaf = "isLeaf";
      _BehavioredClassifier.redefinedElement = "redefinedElement";
      _BehavioredClassifier.redefinitionContext = "redefinitionContext";
      _SimpleClassifiers2._BehavioredClassifier = _BehavioredClassifier;
      _SimpleClassifiers2.__BehavioredClassifier_Uri = "dm:///_internal/model/uml#BehavioredClassifier";
      class _DataType {
      }
      _DataType.ownedAttribute = "ownedAttribute";
      _DataType.ownedOperation = "ownedOperation";
      _DataType.attribute = "attribute";
      _DataType.collaborationUse = "collaborationUse";
      _DataType.feature = "feature";
      _DataType.general = "general";
      _DataType.generalization = "generalization";
      _DataType.inheritedMember = "inheritedMember";
      _DataType.isAbstract = "isAbstract";
      _DataType.isFinalSpecialization = "isFinalSpecialization";
      _DataType.ownedTemplateSignature = "ownedTemplateSignature";
      _DataType.ownedUseCase = "ownedUseCase";
      _DataType.powertypeExtent = "powertypeExtent";
      _DataType.redefinedClassifier = "redefinedClassifier";
      _DataType.representation = "representation";
      _DataType.substitution = "substitution";
      _DataType.templateParameter = "templateParameter";
      _DataType.useCase = "useCase";
      _DataType.elementImport = "elementImport";
      _DataType.importedMember = "importedMember";
      _DataType.member = "member";
      _DataType.ownedMember = "ownedMember";
      _DataType.ownedRule = "ownedRule";
      _DataType.packageImport = "packageImport";
      _DataType.clientDependency = "clientDependency";
      _DataType._name_ = "name";
      _DataType.nameExpression = "nameExpression";
      _DataType.namespace = "namespace";
      _DataType.qualifiedName = "qualifiedName";
      _DataType.visibility = "visibility";
      _DataType.ownedComment = "ownedComment";
      _DataType.ownedElement = "ownedElement";
      _DataType.owner = "owner";
      _DataType._package_ = "package";
      _DataType.owningTemplateParameter = "owningTemplateParameter";
      _DataType.templateBinding = "templateBinding";
      _DataType.isLeaf = "isLeaf";
      _DataType.redefinedElement = "redefinedElement";
      _DataType.redefinitionContext = "redefinitionContext";
      _SimpleClassifiers2._DataType = _DataType;
      _SimpleClassifiers2.__DataType_Uri = "dm:///_internal/model/uml#DataType";
      class _Enumeration {
      }
      _Enumeration.ownedLiteral = "ownedLiteral";
      _Enumeration.ownedAttribute = "ownedAttribute";
      _Enumeration.ownedOperation = "ownedOperation";
      _Enumeration.attribute = "attribute";
      _Enumeration.collaborationUse = "collaborationUse";
      _Enumeration.feature = "feature";
      _Enumeration.general = "general";
      _Enumeration.generalization = "generalization";
      _Enumeration.inheritedMember = "inheritedMember";
      _Enumeration.isAbstract = "isAbstract";
      _Enumeration.isFinalSpecialization = "isFinalSpecialization";
      _Enumeration.ownedTemplateSignature = "ownedTemplateSignature";
      _Enumeration.ownedUseCase = "ownedUseCase";
      _Enumeration.powertypeExtent = "powertypeExtent";
      _Enumeration.redefinedClassifier = "redefinedClassifier";
      _Enumeration.representation = "representation";
      _Enumeration.substitution = "substitution";
      _Enumeration.templateParameter = "templateParameter";
      _Enumeration.useCase = "useCase";
      _Enumeration.elementImport = "elementImport";
      _Enumeration.importedMember = "importedMember";
      _Enumeration.member = "member";
      _Enumeration.ownedMember = "ownedMember";
      _Enumeration.ownedRule = "ownedRule";
      _Enumeration.packageImport = "packageImport";
      _Enumeration.clientDependency = "clientDependency";
      _Enumeration._name_ = "name";
      _Enumeration.nameExpression = "nameExpression";
      _Enumeration.namespace = "namespace";
      _Enumeration.qualifiedName = "qualifiedName";
      _Enumeration.visibility = "visibility";
      _Enumeration.ownedComment = "ownedComment";
      _Enumeration.ownedElement = "ownedElement";
      _Enumeration.owner = "owner";
      _Enumeration._package_ = "package";
      _Enumeration.owningTemplateParameter = "owningTemplateParameter";
      _Enumeration.templateBinding = "templateBinding";
      _Enumeration.isLeaf = "isLeaf";
      _Enumeration.redefinedElement = "redefinedElement";
      _Enumeration.redefinitionContext = "redefinitionContext";
      _SimpleClassifiers2._Enumeration = _Enumeration;
      _SimpleClassifiers2.__Enumeration_Uri = "dm:///_internal/model/uml#Enumeration";
      class _EnumerationLiteral {
      }
      _EnumerationLiteral.classifier = "classifier";
      _EnumerationLiteral.enumeration = "enumeration";
      _EnumerationLiteral.slot = "slot";
      _EnumerationLiteral.specification = "specification";
      _EnumerationLiteral.deployedElement = "deployedElement";
      _EnumerationLiteral.deployment = "deployment";
      _EnumerationLiteral.clientDependency = "clientDependency";
      _EnumerationLiteral._name_ = "name";
      _EnumerationLiteral.nameExpression = "nameExpression";
      _EnumerationLiteral.namespace = "namespace";
      _EnumerationLiteral.qualifiedName = "qualifiedName";
      _EnumerationLiteral.visibility = "visibility";
      _EnumerationLiteral.ownedComment = "ownedComment";
      _EnumerationLiteral.ownedElement = "ownedElement";
      _EnumerationLiteral.owner = "owner";
      _EnumerationLiteral.owningTemplateParameter = "owningTemplateParameter";
      _EnumerationLiteral.templateParameter = "templateParameter";
      _SimpleClassifiers2._EnumerationLiteral = _EnumerationLiteral;
      _SimpleClassifiers2.__EnumerationLiteral_Uri = "dm:///_internal/model/uml#EnumerationLiteral";
      class _Interface {
      }
      _Interface.nestedClassifier = "nestedClassifier";
      _Interface.ownedAttribute = "ownedAttribute";
      _Interface.ownedOperation = "ownedOperation";
      _Interface.ownedReception = "ownedReception";
      _Interface.protocol = "protocol";
      _Interface.redefinedInterface = "redefinedInterface";
      _Interface.attribute = "attribute";
      _Interface.collaborationUse = "collaborationUse";
      _Interface.feature = "feature";
      _Interface.general = "general";
      _Interface.generalization = "generalization";
      _Interface.inheritedMember = "inheritedMember";
      _Interface.isAbstract = "isAbstract";
      _Interface.isFinalSpecialization = "isFinalSpecialization";
      _Interface.ownedTemplateSignature = "ownedTemplateSignature";
      _Interface.ownedUseCase = "ownedUseCase";
      _Interface.powertypeExtent = "powertypeExtent";
      _Interface.redefinedClassifier = "redefinedClassifier";
      _Interface.representation = "representation";
      _Interface.substitution = "substitution";
      _Interface.templateParameter = "templateParameter";
      _Interface.useCase = "useCase";
      _Interface.elementImport = "elementImport";
      _Interface.importedMember = "importedMember";
      _Interface.member = "member";
      _Interface.ownedMember = "ownedMember";
      _Interface.ownedRule = "ownedRule";
      _Interface.packageImport = "packageImport";
      _Interface.clientDependency = "clientDependency";
      _Interface._name_ = "name";
      _Interface.nameExpression = "nameExpression";
      _Interface.namespace = "namespace";
      _Interface.qualifiedName = "qualifiedName";
      _Interface.visibility = "visibility";
      _Interface.ownedComment = "ownedComment";
      _Interface.ownedElement = "ownedElement";
      _Interface.owner = "owner";
      _Interface._package_ = "package";
      _Interface.owningTemplateParameter = "owningTemplateParameter";
      _Interface.templateBinding = "templateBinding";
      _Interface.isLeaf = "isLeaf";
      _Interface.redefinedElement = "redefinedElement";
      _Interface.redefinitionContext = "redefinitionContext";
      _SimpleClassifiers2._Interface = _Interface;
      _SimpleClassifiers2.__Interface_Uri = "dm:///_internal/model/uml#Interface";
      class _InterfaceRealization {
      }
      _InterfaceRealization.contract = "contract";
      _InterfaceRealization.implementingClassifier = "implementingClassifier";
      _InterfaceRealization.mapping = "mapping";
      _InterfaceRealization.client = "client";
      _InterfaceRealization.supplier = "supplier";
      _InterfaceRealization.source = "source";
      _InterfaceRealization.target = "target";
      _InterfaceRealization.relatedElement = "relatedElement";
      _InterfaceRealization.ownedComment = "ownedComment";
      _InterfaceRealization.ownedElement = "ownedElement";
      _InterfaceRealization.owner = "owner";
      _InterfaceRealization.visibility = "visibility";
      _InterfaceRealization.owningTemplateParameter = "owningTemplateParameter";
      _InterfaceRealization.templateParameter = "templateParameter";
      _InterfaceRealization.clientDependency = "clientDependency";
      _InterfaceRealization._name_ = "name";
      _InterfaceRealization.nameExpression = "nameExpression";
      _InterfaceRealization.namespace = "namespace";
      _InterfaceRealization.qualifiedName = "qualifiedName";
      _SimpleClassifiers2._InterfaceRealization = _InterfaceRealization;
      _SimpleClassifiers2.__InterfaceRealization_Uri = "dm:///_internal/model/uml#InterfaceRealization";
      class _PrimitiveType {
      }
      _PrimitiveType.ownedAttribute = "ownedAttribute";
      _PrimitiveType.ownedOperation = "ownedOperation";
      _PrimitiveType.attribute = "attribute";
      _PrimitiveType.collaborationUse = "collaborationUse";
      _PrimitiveType.feature = "feature";
      _PrimitiveType.general = "general";
      _PrimitiveType.generalization = "generalization";
      _PrimitiveType.inheritedMember = "inheritedMember";
      _PrimitiveType.isAbstract = "isAbstract";
      _PrimitiveType.isFinalSpecialization = "isFinalSpecialization";
      _PrimitiveType.ownedTemplateSignature = "ownedTemplateSignature";
      _PrimitiveType.ownedUseCase = "ownedUseCase";
      _PrimitiveType.powertypeExtent = "powertypeExtent";
      _PrimitiveType.redefinedClassifier = "redefinedClassifier";
      _PrimitiveType.representation = "representation";
      _PrimitiveType.substitution = "substitution";
      _PrimitiveType.templateParameter = "templateParameter";
      _PrimitiveType.useCase = "useCase";
      _PrimitiveType.elementImport = "elementImport";
      _PrimitiveType.importedMember = "importedMember";
      _PrimitiveType.member = "member";
      _PrimitiveType.ownedMember = "ownedMember";
      _PrimitiveType.ownedRule = "ownedRule";
      _PrimitiveType.packageImport = "packageImport";
      _PrimitiveType.clientDependency = "clientDependency";
      _PrimitiveType._name_ = "name";
      _PrimitiveType.nameExpression = "nameExpression";
      _PrimitiveType.namespace = "namespace";
      _PrimitiveType.qualifiedName = "qualifiedName";
      _PrimitiveType.visibility = "visibility";
      _PrimitiveType.ownedComment = "ownedComment";
      _PrimitiveType.ownedElement = "ownedElement";
      _PrimitiveType.owner = "owner";
      _PrimitiveType._package_ = "package";
      _PrimitiveType.owningTemplateParameter = "owningTemplateParameter";
      _PrimitiveType.templateBinding = "templateBinding";
      _PrimitiveType.isLeaf = "isLeaf";
      _PrimitiveType.redefinedElement = "redefinedElement";
      _PrimitiveType.redefinitionContext = "redefinitionContext";
      _SimpleClassifiers2._PrimitiveType = _PrimitiveType;
      _SimpleClassifiers2.__PrimitiveType_Uri = "dm:///_internal/model/uml#PrimitiveType";
      class _Reception {
      }
      _Reception.signal = "signal";
      _Reception.concurrency = "concurrency";
      _Reception.isAbstract = "isAbstract";
      _Reception.method = "method";
      _Reception.ownedParameter = "ownedParameter";
      _Reception.ownedParameterSet = "ownedParameterSet";
      _Reception.raisedException = "raisedException";
      _Reception.featuringClassifier = "featuringClassifier";
      _Reception.isStatic = "isStatic";
      _Reception.isLeaf = "isLeaf";
      _Reception.redefinedElement = "redefinedElement";
      _Reception.redefinitionContext = "redefinitionContext";
      _Reception.clientDependency = "clientDependency";
      _Reception._name_ = "name";
      _Reception.nameExpression = "nameExpression";
      _Reception.namespace = "namespace";
      _Reception.qualifiedName = "qualifiedName";
      _Reception.visibility = "visibility";
      _Reception.ownedComment = "ownedComment";
      _Reception.ownedElement = "ownedElement";
      _Reception.owner = "owner";
      _Reception.elementImport = "elementImport";
      _Reception.importedMember = "importedMember";
      _Reception.member = "member";
      _Reception.ownedMember = "ownedMember";
      _Reception.ownedRule = "ownedRule";
      _Reception.packageImport = "packageImport";
      _SimpleClassifiers2._Reception = _Reception;
      _SimpleClassifiers2.__Reception_Uri = "dm:///_internal/model/uml#Reception";
      class _Signal {
      }
      _Signal.ownedAttribute = "ownedAttribute";
      _Signal.attribute = "attribute";
      _Signal.collaborationUse = "collaborationUse";
      _Signal.feature = "feature";
      _Signal.general = "general";
      _Signal.generalization = "generalization";
      _Signal.inheritedMember = "inheritedMember";
      _Signal.isAbstract = "isAbstract";
      _Signal.isFinalSpecialization = "isFinalSpecialization";
      _Signal.ownedTemplateSignature = "ownedTemplateSignature";
      _Signal.ownedUseCase = "ownedUseCase";
      _Signal.powertypeExtent = "powertypeExtent";
      _Signal.redefinedClassifier = "redefinedClassifier";
      _Signal.representation = "representation";
      _Signal.substitution = "substitution";
      _Signal.templateParameter = "templateParameter";
      _Signal.useCase = "useCase";
      _Signal.elementImport = "elementImport";
      _Signal.importedMember = "importedMember";
      _Signal.member = "member";
      _Signal.ownedMember = "ownedMember";
      _Signal.ownedRule = "ownedRule";
      _Signal.packageImport = "packageImport";
      _Signal.clientDependency = "clientDependency";
      _Signal._name_ = "name";
      _Signal.nameExpression = "nameExpression";
      _Signal.namespace = "namespace";
      _Signal.qualifiedName = "qualifiedName";
      _Signal.visibility = "visibility";
      _Signal.ownedComment = "ownedComment";
      _Signal.ownedElement = "ownedElement";
      _Signal.owner = "owner";
      _Signal._package_ = "package";
      _Signal.owningTemplateParameter = "owningTemplateParameter";
      _Signal.templateBinding = "templateBinding";
      _Signal.isLeaf = "isLeaf";
      _Signal.redefinedElement = "redefinedElement";
      _Signal.redefinitionContext = "redefinitionContext";
      _SimpleClassifiers2._Signal = _Signal;
      _SimpleClassifiers2.__Signal_Uri = "dm:///_internal/model/uml#Signal";
    })(_SimpleClassifiers = _UML2._SimpleClassifiers || (_UML2._SimpleClassifiers = {}));
    let _Packages;
    (function(_Packages2) {
      class _Extension {
      }
      _Extension.isRequired = "isRequired";
      _Extension.metaclass = "metaclass";
      _Extension.ownedEnd = "ownedEnd";
      _Extension.endType = "endType";
      _Extension.isDerived = "isDerived";
      _Extension.memberEnd = "memberEnd";
      _Extension.navigableOwnedEnd = "navigableOwnedEnd";
      _Extension.relatedElement = "relatedElement";
      _Extension.ownedComment = "ownedComment";
      _Extension.ownedElement = "ownedElement";
      _Extension.owner = "owner";
      _Extension.attribute = "attribute";
      _Extension.collaborationUse = "collaborationUse";
      _Extension.feature = "feature";
      _Extension.general = "general";
      _Extension.generalization = "generalization";
      _Extension.inheritedMember = "inheritedMember";
      _Extension.isAbstract = "isAbstract";
      _Extension.isFinalSpecialization = "isFinalSpecialization";
      _Extension.ownedTemplateSignature = "ownedTemplateSignature";
      _Extension.ownedUseCase = "ownedUseCase";
      _Extension.powertypeExtent = "powertypeExtent";
      _Extension.redefinedClassifier = "redefinedClassifier";
      _Extension.representation = "representation";
      _Extension.substitution = "substitution";
      _Extension.templateParameter = "templateParameter";
      _Extension.useCase = "useCase";
      _Extension.elementImport = "elementImport";
      _Extension.importedMember = "importedMember";
      _Extension.member = "member";
      _Extension.ownedMember = "ownedMember";
      _Extension.ownedRule = "ownedRule";
      _Extension.packageImport = "packageImport";
      _Extension.clientDependency = "clientDependency";
      _Extension._name_ = "name";
      _Extension.nameExpression = "nameExpression";
      _Extension.namespace = "namespace";
      _Extension.qualifiedName = "qualifiedName";
      _Extension.visibility = "visibility";
      _Extension._package_ = "package";
      _Extension.owningTemplateParameter = "owningTemplateParameter";
      _Extension.templateBinding = "templateBinding";
      _Extension.isLeaf = "isLeaf";
      _Extension.redefinedElement = "redefinedElement";
      _Extension.redefinitionContext = "redefinitionContext";
      _Packages2._Extension = _Extension;
      _Packages2.__Extension_Uri = "dm:///_internal/model/uml#Extension";
      class _ExtensionEnd {
      }
      _ExtensionEnd.lower = "lower";
      _ExtensionEnd.type = "type";
      _ExtensionEnd.aggregation = "aggregation";
      _ExtensionEnd.association = "association";
      _ExtensionEnd.associationEnd = "associationEnd";
      _ExtensionEnd._class_ = "class";
      _ExtensionEnd.datatype = "datatype";
      _ExtensionEnd.defaultValue = "defaultValue";
      _ExtensionEnd._interface_ = "interface";
      _ExtensionEnd.isComposite = "isComposite";
      _ExtensionEnd.isDerived = "isDerived";
      _ExtensionEnd.isDerivedUnion = "isDerivedUnion";
      _ExtensionEnd.isID = "isID";
      _ExtensionEnd.opposite = "opposite";
      _ExtensionEnd.owningAssociation = "owningAssociation";
      _ExtensionEnd.qualifier = "qualifier";
      _ExtensionEnd.redefinedProperty = "redefinedProperty";
      _ExtensionEnd.subsettedProperty = "subsettedProperty";
      _ExtensionEnd.end = "end";
      _ExtensionEnd.templateParameter = "templateParameter";
      _ExtensionEnd.clientDependency = "clientDependency";
      _ExtensionEnd._name_ = "name";
      _ExtensionEnd.nameExpression = "nameExpression";
      _ExtensionEnd.namespace = "namespace";
      _ExtensionEnd.qualifiedName = "qualifiedName";
      _ExtensionEnd.visibility = "visibility";
      _ExtensionEnd.ownedComment = "ownedComment";
      _ExtensionEnd.ownedElement = "ownedElement";
      _ExtensionEnd.owner = "owner";
      _ExtensionEnd.owningTemplateParameter = "owningTemplateParameter";
      _ExtensionEnd.deployedElement = "deployedElement";
      _ExtensionEnd.deployment = "deployment";
      _ExtensionEnd.isReadOnly = "isReadOnly";
      _ExtensionEnd.isOrdered = "isOrdered";
      _ExtensionEnd.isUnique = "isUnique";
      _ExtensionEnd.lowerValue = "lowerValue";
      _ExtensionEnd.upper = "upper";
      _ExtensionEnd.upperValue = "upperValue";
      _ExtensionEnd.featuringClassifier = "featuringClassifier";
      _ExtensionEnd.isStatic = "isStatic";
      _ExtensionEnd.isLeaf = "isLeaf";
      _ExtensionEnd.redefinedElement = "redefinedElement";
      _ExtensionEnd.redefinitionContext = "redefinitionContext";
      _Packages2._ExtensionEnd = _ExtensionEnd;
      _Packages2.__ExtensionEnd_Uri = "dm:///_internal/model/uml#ExtensionEnd";
      class _Image {
      }
      _Image.content = "content";
      _Image.format = "format";
      _Image.location = "location";
      _Image.ownedComment = "ownedComment";
      _Image.ownedElement = "ownedElement";
      _Image.owner = "owner";
      _Packages2._Image = _Image;
      _Packages2.__Image_Uri = "dm:///_internal/model/uml#Image";
      class _Model {
      }
      _Model.viewpoint = "viewpoint";
      _Model.URI = "URI";
      _Model.nestedPackage = "nestedPackage";
      _Model.nestingPackage = "nestingPackage";
      _Model.ownedStereotype = "ownedStereotype";
      _Model.ownedType = "ownedType";
      _Model.packageMerge = "packageMerge";
      _Model.packagedElement = "packagedElement";
      _Model.profileApplication = "profileApplication";
      _Model.visibility = "visibility";
      _Model.owningTemplateParameter = "owningTemplateParameter";
      _Model.templateParameter = "templateParameter";
      _Model.ownedComment = "ownedComment";
      _Model.ownedElement = "ownedElement";
      _Model.owner = "owner";
      _Model.clientDependency = "clientDependency";
      _Model._name_ = "name";
      _Model.nameExpression = "nameExpression";
      _Model.namespace = "namespace";
      _Model.qualifiedName = "qualifiedName";
      _Model.ownedTemplateSignature = "ownedTemplateSignature";
      _Model.templateBinding = "templateBinding";
      _Model.elementImport = "elementImport";
      _Model.importedMember = "importedMember";
      _Model.member = "member";
      _Model.ownedMember = "ownedMember";
      _Model.ownedRule = "ownedRule";
      _Model.packageImport = "packageImport";
      _Packages2._Model = _Model;
      _Packages2.__Model_Uri = "dm:///_internal/model/uml#Model";
      class _Package {
      }
      _Package.URI = "URI";
      _Package.nestedPackage = "nestedPackage";
      _Package.nestingPackage = "nestingPackage";
      _Package.ownedStereotype = "ownedStereotype";
      _Package.ownedType = "ownedType";
      _Package.packageMerge = "packageMerge";
      _Package.packagedElement = "packagedElement";
      _Package.profileApplication = "profileApplication";
      _Package.visibility = "visibility";
      _Package.owningTemplateParameter = "owningTemplateParameter";
      _Package.templateParameter = "templateParameter";
      _Package.ownedComment = "ownedComment";
      _Package.ownedElement = "ownedElement";
      _Package.owner = "owner";
      _Package.clientDependency = "clientDependency";
      _Package._name_ = "name";
      _Package.nameExpression = "nameExpression";
      _Package.namespace = "namespace";
      _Package.qualifiedName = "qualifiedName";
      _Package.ownedTemplateSignature = "ownedTemplateSignature";
      _Package.templateBinding = "templateBinding";
      _Package.elementImport = "elementImport";
      _Package.importedMember = "importedMember";
      _Package.member = "member";
      _Package.ownedMember = "ownedMember";
      _Package.ownedRule = "ownedRule";
      _Package.packageImport = "packageImport";
      _Packages2._Package = _Package;
      _Packages2.__Package_Uri = "dm:///_internal/model/uml#Package";
      class _PackageMerge {
      }
      _PackageMerge.mergedPackage = "mergedPackage";
      _PackageMerge.receivingPackage = "receivingPackage";
      _PackageMerge.source = "source";
      _PackageMerge.target = "target";
      _PackageMerge.relatedElement = "relatedElement";
      _PackageMerge.ownedComment = "ownedComment";
      _PackageMerge.ownedElement = "ownedElement";
      _PackageMerge.owner = "owner";
      _Packages2._PackageMerge = _PackageMerge;
      _Packages2.__PackageMerge_Uri = "dm:///_internal/model/uml#PackageMerge";
      class _Profile {
      }
      _Profile.metaclassReference = "metaclassReference";
      _Profile.metamodelReference = "metamodelReference";
      _Profile.URI = "URI";
      _Profile.nestedPackage = "nestedPackage";
      _Profile.nestingPackage = "nestingPackage";
      _Profile.ownedStereotype = "ownedStereotype";
      _Profile.ownedType = "ownedType";
      _Profile.packageMerge = "packageMerge";
      _Profile.packagedElement = "packagedElement";
      _Profile.profileApplication = "profileApplication";
      _Profile.visibility = "visibility";
      _Profile.owningTemplateParameter = "owningTemplateParameter";
      _Profile.templateParameter = "templateParameter";
      _Profile.ownedComment = "ownedComment";
      _Profile.ownedElement = "ownedElement";
      _Profile.owner = "owner";
      _Profile.clientDependency = "clientDependency";
      _Profile._name_ = "name";
      _Profile.nameExpression = "nameExpression";
      _Profile.namespace = "namespace";
      _Profile.qualifiedName = "qualifiedName";
      _Profile.ownedTemplateSignature = "ownedTemplateSignature";
      _Profile.templateBinding = "templateBinding";
      _Profile.elementImport = "elementImport";
      _Profile.importedMember = "importedMember";
      _Profile.member = "member";
      _Profile.ownedMember = "ownedMember";
      _Profile.ownedRule = "ownedRule";
      _Profile.packageImport = "packageImport";
      _Packages2._Profile = _Profile;
      _Packages2.__Profile_Uri = "dm:///_internal/model/uml#Profile";
      class _ProfileApplication {
      }
      _ProfileApplication.appliedProfile = "appliedProfile";
      _ProfileApplication.applyingPackage = "applyingPackage";
      _ProfileApplication.isStrict = "isStrict";
      _ProfileApplication.source = "source";
      _ProfileApplication.target = "target";
      _ProfileApplication.relatedElement = "relatedElement";
      _ProfileApplication.ownedComment = "ownedComment";
      _ProfileApplication.ownedElement = "ownedElement";
      _ProfileApplication.owner = "owner";
      _Packages2._ProfileApplication = _ProfileApplication;
      _Packages2.__ProfileApplication_Uri = "dm:///_internal/model/uml#ProfileApplication";
      class _Stereotype {
      }
      _Stereotype.icon = "icon";
      _Stereotype.profile = "profile";
      _Stereotype.extension = "extension";
      _Stereotype.isAbstract = "isAbstract";
      _Stereotype.isActive = "isActive";
      _Stereotype.nestedClassifier = "nestedClassifier";
      _Stereotype.ownedAttribute = "ownedAttribute";
      _Stereotype.ownedOperation = "ownedOperation";
      _Stereotype.ownedReception = "ownedReception";
      _Stereotype.superClass = "superClass";
      _Stereotype.classifierBehavior = "classifierBehavior";
      _Stereotype.interfaceRealization = "interfaceRealization";
      _Stereotype.ownedBehavior = "ownedBehavior";
      _Stereotype.attribute = "attribute";
      _Stereotype.collaborationUse = "collaborationUse";
      _Stereotype.feature = "feature";
      _Stereotype.general = "general";
      _Stereotype.generalization = "generalization";
      _Stereotype.inheritedMember = "inheritedMember";
      _Stereotype.isFinalSpecialization = "isFinalSpecialization";
      _Stereotype.ownedTemplateSignature = "ownedTemplateSignature";
      _Stereotype.ownedUseCase = "ownedUseCase";
      _Stereotype.powertypeExtent = "powertypeExtent";
      _Stereotype.redefinedClassifier = "redefinedClassifier";
      _Stereotype.representation = "representation";
      _Stereotype.substitution = "substitution";
      _Stereotype.templateParameter = "templateParameter";
      _Stereotype.useCase = "useCase";
      _Stereotype.elementImport = "elementImport";
      _Stereotype.importedMember = "importedMember";
      _Stereotype.member = "member";
      _Stereotype.ownedMember = "ownedMember";
      _Stereotype.ownedRule = "ownedRule";
      _Stereotype.packageImport = "packageImport";
      _Stereotype.clientDependency = "clientDependency";
      _Stereotype._name_ = "name";
      _Stereotype.nameExpression = "nameExpression";
      _Stereotype.namespace = "namespace";
      _Stereotype.qualifiedName = "qualifiedName";
      _Stereotype.visibility = "visibility";
      _Stereotype.ownedComment = "ownedComment";
      _Stereotype.ownedElement = "ownedElement";
      _Stereotype.owner = "owner";
      _Stereotype._package_ = "package";
      _Stereotype.owningTemplateParameter = "owningTemplateParameter";
      _Stereotype.templateBinding = "templateBinding";
      _Stereotype.isLeaf = "isLeaf";
      _Stereotype.redefinedElement = "redefinedElement";
      _Stereotype.redefinitionContext = "redefinitionContext";
      _Stereotype.ownedPort = "ownedPort";
      _Stereotype.ownedConnector = "ownedConnector";
      _Stereotype.part = "part";
      _Stereotype.role = "role";
      _Packages2._Stereotype = _Stereotype;
      _Packages2.__Stereotype_Uri = "dm:///_internal/model/uml#Stereotype";
    })(_Packages = _UML2._Packages || (_UML2._Packages = {}));
    let _Interactions;
    (function(_Interactions2) {
      class _ActionExecutionSpecification {
      }
      _ActionExecutionSpecification.action = "action";
      _ActionExecutionSpecification.finish = "finish";
      _ActionExecutionSpecification.start = "start";
      _ActionExecutionSpecification.covered = "covered";
      _ActionExecutionSpecification.enclosingInteraction = "enclosingInteraction";
      _ActionExecutionSpecification.enclosingOperand = "enclosingOperand";
      _ActionExecutionSpecification.generalOrdering = "generalOrdering";
      _ActionExecutionSpecification.clientDependency = "clientDependency";
      _ActionExecutionSpecification._name_ = "name";
      _ActionExecutionSpecification.nameExpression = "nameExpression";
      _ActionExecutionSpecification.namespace = "namespace";
      _ActionExecutionSpecification.qualifiedName = "qualifiedName";
      _ActionExecutionSpecification.visibility = "visibility";
      _ActionExecutionSpecification.ownedComment = "ownedComment";
      _ActionExecutionSpecification.ownedElement = "ownedElement";
      _ActionExecutionSpecification.owner = "owner";
      _Interactions2._ActionExecutionSpecification = _ActionExecutionSpecification;
      _Interactions2.__ActionExecutionSpecification_Uri = "dm:///_internal/model/uml#ActionExecutionSpecification";
      class _BehaviorExecutionSpecification {
      }
      _BehaviorExecutionSpecification.behavior = "behavior";
      _BehaviorExecutionSpecification.finish = "finish";
      _BehaviorExecutionSpecification.start = "start";
      _BehaviorExecutionSpecification.covered = "covered";
      _BehaviorExecutionSpecification.enclosingInteraction = "enclosingInteraction";
      _BehaviorExecutionSpecification.enclosingOperand = "enclosingOperand";
      _BehaviorExecutionSpecification.generalOrdering = "generalOrdering";
      _BehaviorExecutionSpecification.clientDependency = "clientDependency";
      _BehaviorExecutionSpecification._name_ = "name";
      _BehaviorExecutionSpecification.nameExpression = "nameExpression";
      _BehaviorExecutionSpecification.namespace = "namespace";
      _BehaviorExecutionSpecification.qualifiedName = "qualifiedName";
      _BehaviorExecutionSpecification.visibility = "visibility";
      _BehaviorExecutionSpecification.ownedComment = "ownedComment";
      _BehaviorExecutionSpecification.ownedElement = "ownedElement";
      _BehaviorExecutionSpecification.owner = "owner";
      _Interactions2._BehaviorExecutionSpecification = _BehaviorExecutionSpecification;
      _Interactions2.__BehaviorExecutionSpecification_Uri = "dm:///_internal/model/uml#BehaviorExecutionSpecification";
      class _CombinedFragment {
      }
      _CombinedFragment.cfragmentGate = "cfragmentGate";
      _CombinedFragment.interactionOperator = "interactionOperator";
      _CombinedFragment.operand = "operand";
      _CombinedFragment.covered = "covered";
      _CombinedFragment.enclosingInteraction = "enclosingInteraction";
      _CombinedFragment.enclosingOperand = "enclosingOperand";
      _CombinedFragment.generalOrdering = "generalOrdering";
      _CombinedFragment.clientDependency = "clientDependency";
      _CombinedFragment._name_ = "name";
      _CombinedFragment.nameExpression = "nameExpression";
      _CombinedFragment.namespace = "namespace";
      _CombinedFragment.qualifiedName = "qualifiedName";
      _CombinedFragment.visibility = "visibility";
      _CombinedFragment.ownedComment = "ownedComment";
      _CombinedFragment.ownedElement = "ownedElement";
      _CombinedFragment.owner = "owner";
      _Interactions2._CombinedFragment = _CombinedFragment;
      _Interactions2.__CombinedFragment_Uri = "dm:///_internal/model/uml#CombinedFragment";
      class _ConsiderIgnoreFragment {
      }
      _ConsiderIgnoreFragment.message = "message";
      _ConsiderIgnoreFragment.cfragmentGate = "cfragmentGate";
      _ConsiderIgnoreFragment.interactionOperator = "interactionOperator";
      _ConsiderIgnoreFragment.operand = "operand";
      _ConsiderIgnoreFragment.covered = "covered";
      _ConsiderIgnoreFragment.enclosingInteraction = "enclosingInteraction";
      _ConsiderIgnoreFragment.enclosingOperand = "enclosingOperand";
      _ConsiderIgnoreFragment.generalOrdering = "generalOrdering";
      _ConsiderIgnoreFragment.clientDependency = "clientDependency";
      _ConsiderIgnoreFragment._name_ = "name";
      _ConsiderIgnoreFragment.nameExpression = "nameExpression";
      _ConsiderIgnoreFragment.namespace = "namespace";
      _ConsiderIgnoreFragment.qualifiedName = "qualifiedName";
      _ConsiderIgnoreFragment.visibility = "visibility";
      _ConsiderIgnoreFragment.ownedComment = "ownedComment";
      _ConsiderIgnoreFragment.ownedElement = "ownedElement";
      _ConsiderIgnoreFragment.owner = "owner";
      _Interactions2._ConsiderIgnoreFragment = _ConsiderIgnoreFragment;
      _Interactions2.__ConsiderIgnoreFragment_Uri = "dm:///_internal/model/uml#ConsiderIgnoreFragment";
      class _Continuation {
      }
      _Continuation.setting = "setting";
      _Continuation.covered = "covered";
      _Continuation.enclosingInteraction = "enclosingInteraction";
      _Continuation.enclosingOperand = "enclosingOperand";
      _Continuation.generalOrdering = "generalOrdering";
      _Continuation.clientDependency = "clientDependency";
      _Continuation._name_ = "name";
      _Continuation.nameExpression = "nameExpression";
      _Continuation.namespace = "namespace";
      _Continuation.qualifiedName = "qualifiedName";
      _Continuation.visibility = "visibility";
      _Continuation.ownedComment = "ownedComment";
      _Continuation.ownedElement = "ownedElement";
      _Continuation.owner = "owner";
      _Interactions2._Continuation = _Continuation;
      _Interactions2.__Continuation_Uri = "dm:///_internal/model/uml#Continuation";
      class _DestructionOccurrenceSpecification {
      }
      _DestructionOccurrenceSpecification.message = "message";
      _DestructionOccurrenceSpecification.clientDependency = "clientDependency";
      _DestructionOccurrenceSpecification._name_ = "name";
      _DestructionOccurrenceSpecification.nameExpression = "nameExpression";
      _DestructionOccurrenceSpecification.namespace = "namespace";
      _DestructionOccurrenceSpecification.qualifiedName = "qualifiedName";
      _DestructionOccurrenceSpecification.visibility = "visibility";
      _DestructionOccurrenceSpecification.ownedComment = "ownedComment";
      _DestructionOccurrenceSpecification.ownedElement = "ownedElement";
      _DestructionOccurrenceSpecification.owner = "owner";
      _DestructionOccurrenceSpecification.covered = "covered";
      _DestructionOccurrenceSpecification.toAfter = "toAfter";
      _DestructionOccurrenceSpecification.toBefore = "toBefore";
      _DestructionOccurrenceSpecification.enclosingInteraction = "enclosingInteraction";
      _DestructionOccurrenceSpecification.enclosingOperand = "enclosingOperand";
      _DestructionOccurrenceSpecification.generalOrdering = "generalOrdering";
      _Interactions2._DestructionOccurrenceSpecification = _DestructionOccurrenceSpecification;
      _Interactions2.__DestructionOccurrenceSpecification_Uri = "dm:///_internal/model/uml#DestructionOccurrenceSpecification";
      class _ExecutionOccurrenceSpecification {
      }
      _ExecutionOccurrenceSpecification.execution = "execution";
      _ExecutionOccurrenceSpecification.covered = "covered";
      _ExecutionOccurrenceSpecification.toAfter = "toAfter";
      _ExecutionOccurrenceSpecification.toBefore = "toBefore";
      _ExecutionOccurrenceSpecification.enclosingInteraction = "enclosingInteraction";
      _ExecutionOccurrenceSpecification.enclosingOperand = "enclosingOperand";
      _ExecutionOccurrenceSpecification.generalOrdering = "generalOrdering";
      _ExecutionOccurrenceSpecification.clientDependency = "clientDependency";
      _ExecutionOccurrenceSpecification._name_ = "name";
      _ExecutionOccurrenceSpecification.nameExpression = "nameExpression";
      _ExecutionOccurrenceSpecification.namespace = "namespace";
      _ExecutionOccurrenceSpecification.qualifiedName = "qualifiedName";
      _ExecutionOccurrenceSpecification.visibility = "visibility";
      _ExecutionOccurrenceSpecification.ownedComment = "ownedComment";
      _ExecutionOccurrenceSpecification.ownedElement = "ownedElement";
      _ExecutionOccurrenceSpecification.owner = "owner";
      _Interactions2._ExecutionOccurrenceSpecification = _ExecutionOccurrenceSpecification;
      _Interactions2.__ExecutionOccurrenceSpecification_Uri = "dm:///_internal/model/uml#ExecutionOccurrenceSpecification";
      class _ExecutionSpecification {
      }
      _ExecutionSpecification.finish = "finish";
      _ExecutionSpecification.start = "start";
      _ExecutionSpecification.covered = "covered";
      _ExecutionSpecification.enclosingInteraction = "enclosingInteraction";
      _ExecutionSpecification.enclosingOperand = "enclosingOperand";
      _ExecutionSpecification.generalOrdering = "generalOrdering";
      _ExecutionSpecification.clientDependency = "clientDependency";
      _ExecutionSpecification._name_ = "name";
      _ExecutionSpecification.nameExpression = "nameExpression";
      _ExecutionSpecification.namespace = "namespace";
      _ExecutionSpecification.qualifiedName = "qualifiedName";
      _ExecutionSpecification.visibility = "visibility";
      _ExecutionSpecification.ownedComment = "ownedComment";
      _ExecutionSpecification.ownedElement = "ownedElement";
      _ExecutionSpecification.owner = "owner";
      _Interactions2._ExecutionSpecification = _ExecutionSpecification;
      _Interactions2.__ExecutionSpecification_Uri = "dm:///_internal/model/uml#ExecutionSpecification";
      class _Gate {
      }
      _Gate.message = "message";
      _Gate.clientDependency = "clientDependency";
      _Gate._name_ = "name";
      _Gate.nameExpression = "nameExpression";
      _Gate.namespace = "namespace";
      _Gate.qualifiedName = "qualifiedName";
      _Gate.visibility = "visibility";
      _Gate.ownedComment = "ownedComment";
      _Gate.ownedElement = "ownedElement";
      _Gate.owner = "owner";
      _Interactions2._Gate = _Gate;
      _Interactions2.__Gate_Uri = "dm:///_internal/model/uml#Gate";
      class _GeneralOrdering {
      }
      _GeneralOrdering.after = "after";
      _GeneralOrdering.before = "before";
      _GeneralOrdering.clientDependency = "clientDependency";
      _GeneralOrdering._name_ = "name";
      _GeneralOrdering.nameExpression = "nameExpression";
      _GeneralOrdering.namespace = "namespace";
      _GeneralOrdering.qualifiedName = "qualifiedName";
      _GeneralOrdering.visibility = "visibility";
      _GeneralOrdering.ownedComment = "ownedComment";
      _GeneralOrdering.ownedElement = "ownedElement";
      _GeneralOrdering.owner = "owner";
      _Interactions2._GeneralOrdering = _GeneralOrdering;
      _Interactions2.__GeneralOrdering_Uri = "dm:///_internal/model/uml#GeneralOrdering";
      class _Interaction {
      }
      _Interaction.action = "action";
      _Interaction.formalGate = "formalGate";
      _Interaction.fragment = "fragment";
      _Interaction.lifeline = "lifeline";
      _Interaction.message = "message";
      _Interaction.covered = "covered";
      _Interaction.enclosingInteraction = "enclosingInteraction";
      _Interaction.enclosingOperand = "enclosingOperand";
      _Interaction.generalOrdering = "generalOrdering";
      _Interaction.clientDependency = "clientDependency";
      _Interaction._name_ = "name";
      _Interaction.nameExpression = "nameExpression";
      _Interaction.namespace = "namespace";
      _Interaction.qualifiedName = "qualifiedName";
      _Interaction.visibility = "visibility";
      _Interaction.ownedComment = "ownedComment";
      _Interaction.ownedElement = "ownedElement";
      _Interaction.owner = "owner";
      _Interaction.context = "context";
      _Interaction.isReentrant = "isReentrant";
      _Interaction.ownedParameter = "ownedParameter";
      _Interaction.ownedParameterSet = "ownedParameterSet";
      _Interaction.postcondition = "postcondition";
      _Interaction.precondition = "precondition";
      _Interaction.specification = "specification";
      _Interaction.redefinedBehavior = "redefinedBehavior";
      _Interaction.extension = "extension";
      _Interaction.isAbstract = "isAbstract";
      _Interaction.isActive = "isActive";
      _Interaction.nestedClassifier = "nestedClassifier";
      _Interaction.ownedAttribute = "ownedAttribute";
      _Interaction.ownedOperation = "ownedOperation";
      _Interaction.ownedReception = "ownedReception";
      _Interaction.superClass = "superClass";
      _Interaction.classifierBehavior = "classifierBehavior";
      _Interaction.interfaceRealization = "interfaceRealization";
      _Interaction.ownedBehavior = "ownedBehavior";
      _Interaction.attribute = "attribute";
      _Interaction.collaborationUse = "collaborationUse";
      _Interaction.feature = "feature";
      _Interaction.general = "general";
      _Interaction.generalization = "generalization";
      _Interaction.inheritedMember = "inheritedMember";
      _Interaction.isFinalSpecialization = "isFinalSpecialization";
      _Interaction.ownedTemplateSignature = "ownedTemplateSignature";
      _Interaction.ownedUseCase = "ownedUseCase";
      _Interaction.powertypeExtent = "powertypeExtent";
      _Interaction.redefinedClassifier = "redefinedClassifier";
      _Interaction.representation = "representation";
      _Interaction.substitution = "substitution";
      _Interaction.templateParameter = "templateParameter";
      _Interaction.useCase = "useCase";
      _Interaction.elementImport = "elementImport";
      _Interaction.importedMember = "importedMember";
      _Interaction.member = "member";
      _Interaction.ownedMember = "ownedMember";
      _Interaction.ownedRule = "ownedRule";
      _Interaction.packageImport = "packageImport";
      _Interaction._package_ = "package";
      _Interaction.owningTemplateParameter = "owningTemplateParameter";
      _Interaction.templateBinding = "templateBinding";
      _Interaction.isLeaf = "isLeaf";
      _Interaction.redefinedElement = "redefinedElement";
      _Interaction.redefinitionContext = "redefinitionContext";
      _Interaction.ownedPort = "ownedPort";
      _Interaction.ownedConnector = "ownedConnector";
      _Interaction.part = "part";
      _Interaction.role = "role";
      _Interactions2._Interaction = _Interaction;
      _Interactions2.__Interaction_Uri = "dm:///_internal/model/uml#Interaction";
      class _InteractionConstraint {
      }
      _InteractionConstraint.maxint = "maxint";
      _InteractionConstraint.minint = "minint";
      _InteractionConstraint.constrainedElement = "constrainedElement";
      _InteractionConstraint.context = "context";
      _InteractionConstraint.specification = "specification";
      _InteractionConstraint.visibility = "visibility";
      _InteractionConstraint.owningTemplateParameter = "owningTemplateParameter";
      _InteractionConstraint.templateParameter = "templateParameter";
      _InteractionConstraint.ownedComment = "ownedComment";
      _InteractionConstraint.ownedElement = "ownedElement";
      _InteractionConstraint.owner = "owner";
      _InteractionConstraint.clientDependency = "clientDependency";
      _InteractionConstraint._name_ = "name";
      _InteractionConstraint.nameExpression = "nameExpression";
      _InteractionConstraint.namespace = "namespace";
      _InteractionConstraint.qualifiedName = "qualifiedName";
      _Interactions2._InteractionConstraint = _InteractionConstraint;
      _Interactions2.__InteractionConstraint_Uri = "dm:///_internal/model/uml#InteractionConstraint";
      class _InteractionFragment {
      }
      _InteractionFragment.covered = "covered";
      _InteractionFragment.enclosingInteraction = "enclosingInteraction";
      _InteractionFragment.enclosingOperand = "enclosingOperand";
      _InteractionFragment.generalOrdering = "generalOrdering";
      _InteractionFragment.clientDependency = "clientDependency";
      _InteractionFragment._name_ = "name";
      _InteractionFragment.nameExpression = "nameExpression";
      _InteractionFragment.namespace = "namespace";
      _InteractionFragment.qualifiedName = "qualifiedName";
      _InteractionFragment.visibility = "visibility";
      _InteractionFragment.ownedComment = "ownedComment";
      _InteractionFragment.ownedElement = "ownedElement";
      _InteractionFragment.owner = "owner";
      _Interactions2._InteractionFragment = _InteractionFragment;
      _Interactions2.__InteractionFragment_Uri = "dm:///_internal/model/uml#InteractionFragment";
      class _InteractionOperand {
      }
      _InteractionOperand.fragment = "fragment";
      _InteractionOperand.guard = "guard";
      _InteractionOperand.covered = "covered";
      _InteractionOperand.enclosingInteraction = "enclosingInteraction";
      _InteractionOperand.enclosingOperand = "enclosingOperand";
      _InteractionOperand.generalOrdering = "generalOrdering";
      _InteractionOperand.clientDependency = "clientDependency";
      _InteractionOperand._name_ = "name";
      _InteractionOperand.nameExpression = "nameExpression";
      _InteractionOperand.namespace = "namespace";
      _InteractionOperand.qualifiedName = "qualifiedName";
      _InteractionOperand.visibility = "visibility";
      _InteractionOperand.ownedComment = "ownedComment";
      _InteractionOperand.ownedElement = "ownedElement";
      _InteractionOperand.owner = "owner";
      _InteractionOperand.elementImport = "elementImport";
      _InteractionOperand.importedMember = "importedMember";
      _InteractionOperand.member = "member";
      _InteractionOperand.ownedMember = "ownedMember";
      _InteractionOperand.ownedRule = "ownedRule";
      _InteractionOperand.packageImport = "packageImport";
      _Interactions2._InteractionOperand = _InteractionOperand;
      _Interactions2.__InteractionOperand_Uri = "dm:///_internal/model/uml#InteractionOperand";
      class _InteractionUse {
      }
      _InteractionUse.actualGate = "actualGate";
      _InteractionUse.argument = "argument";
      _InteractionUse.refersTo = "refersTo";
      _InteractionUse.returnValue = "returnValue";
      _InteractionUse.returnValueRecipient = "returnValueRecipient";
      _InteractionUse.covered = "covered";
      _InteractionUse.enclosingInteraction = "enclosingInteraction";
      _InteractionUse.enclosingOperand = "enclosingOperand";
      _InteractionUse.generalOrdering = "generalOrdering";
      _InteractionUse.clientDependency = "clientDependency";
      _InteractionUse._name_ = "name";
      _InteractionUse.nameExpression = "nameExpression";
      _InteractionUse.namespace = "namespace";
      _InteractionUse.qualifiedName = "qualifiedName";
      _InteractionUse.visibility = "visibility";
      _InteractionUse.ownedComment = "ownedComment";
      _InteractionUse.ownedElement = "ownedElement";
      _InteractionUse.owner = "owner";
      _Interactions2._InteractionUse = _InteractionUse;
      _Interactions2.__InteractionUse_Uri = "dm:///_internal/model/uml#InteractionUse";
      class _Lifeline {
      }
      _Lifeline.coveredBy = "coveredBy";
      _Lifeline.decomposedAs = "decomposedAs";
      _Lifeline.interaction = "interaction";
      _Lifeline.represents = "represents";
      _Lifeline.selector = "selector";
      _Lifeline.clientDependency = "clientDependency";
      _Lifeline._name_ = "name";
      _Lifeline.nameExpression = "nameExpression";
      _Lifeline.namespace = "namespace";
      _Lifeline.qualifiedName = "qualifiedName";
      _Lifeline.visibility = "visibility";
      _Lifeline.ownedComment = "ownedComment";
      _Lifeline.ownedElement = "ownedElement";
      _Lifeline.owner = "owner";
      _Interactions2._Lifeline = _Lifeline;
      _Interactions2.__Lifeline_Uri = "dm:///_internal/model/uml#Lifeline";
      class _Message {
      }
      _Message.argument = "argument";
      _Message.connector = "connector";
      _Message.interaction = "interaction";
      _Message.messageKind = "messageKind";
      _Message.messageSort = "messageSort";
      _Message.receiveEvent = "receiveEvent";
      _Message.sendEvent = "sendEvent";
      _Message.signature = "signature";
      _Message.clientDependency = "clientDependency";
      _Message._name_ = "name";
      _Message.nameExpression = "nameExpression";
      _Message.namespace = "namespace";
      _Message.qualifiedName = "qualifiedName";
      _Message.visibility = "visibility";
      _Message.ownedComment = "ownedComment";
      _Message.ownedElement = "ownedElement";
      _Message.owner = "owner";
      _Interactions2._Message = _Message;
      _Interactions2.__Message_Uri = "dm:///_internal/model/uml#Message";
      class _MessageEnd {
      }
      _MessageEnd.message = "message";
      _MessageEnd.clientDependency = "clientDependency";
      _MessageEnd._name_ = "name";
      _MessageEnd.nameExpression = "nameExpression";
      _MessageEnd.namespace = "namespace";
      _MessageEnd.qualifiedName = "qualifiedName";
      _MessageEnd.visibility = "visibility";
      _MessageEnd.ownedComment = "ownedComment";
      _MessageEnd.ownedElement = "ownedElement";
      _MessageEnd.owner = "owner";
      _Interactions2._MessageEnd = _MessageEnd;
      _Interactions2.__MessageEnd_Uri = "dm:///_internal/model/uml#MessageEnd";
      class _MessageOccurrenceSpecification {
      }
      _MessageOccurrenceSpecification.message = "message";
      _MessageOccurrenceSpecification.clientDependency = "clientDependency";
      _MessageOccurrenceSpecification._name_ = "name";
      _MessageOccurrenceSpecification.nameExpression = "nameExpression";
      _MessageOccurrenceSpecification.namespace = "namespace";
      _MessageOccurrenceSpecification.qualifiedName = "qualifiedName";
      _MessageOccurrenceSpecification.visibility = "visibility";
      _MessageOccurrenceSpecification.ownedComment = "ownedComment";
      _MessageOccurrenceSpecification.ownedElement = "ownedElement";
      _MessageOccurrenceSpecification.owner = "owner";
      _MessageOccurrenceSpecification.covered = "covered";
      _MessageOccurrenceSpecification.toAfter = "toAfter";
      _MessageOccurrenceSpecification.toBefore = "toBefore";
      _MessageOccurrenceSpecification.enclosingInteraction = "enclosingInteraction";
      _MessageOccurrenceSpecification.enclosingOperand = "enclosingOperand";
      _MessageOccurrenceSpecification.generalOrdering = "generalOrdering";
      _Interactions2._MessageOccurrenceSpecification = _MessageOccurrenceSpecification;
      _Interactions2.__MessageOccurrenceSpecification_Uri = "dm:///_internal/model/uml#MessageOccurrenceSpecification";
      class _OccurrenceSpecification {
      }
      _OccurrenceSpecification.covered = "covered";
      _OccurrenceSpecification.toAfter = "toAfter";
      _OccurrenceSpecification.toBefore = "toBefore";
      _OccurrenceSpecification.enclosingInteraction = "enclosingInteraction";
      _OccurrenceSpecification.enclosingOperand = "enclosingOperand";
      _OccurrenceSpecification.generalOrdering = "generalOrdering";
      _OccurrenceSpecification.clientDependency = "clientDependency";
      _OccurrenceSpecification._name_ = "name";
      _OccurrenceSpecification.nameExpression = "nameExpression";
      _OccurrenceSpecification.namespace = "namespace";
      _OccurrenceSpecification.qualifiedName = "qualifiedName";
      _OccurrenceSpecification.visibility = "visibility";
      _OccurrenceSpecification.ownedComment = "ownedComment";
      _OccurrenceSpecification.ownedElement = "ownedElement";
      _OccurrenceSpecification.owner = "owner";
      _Interactions2._OccurrenceSpecification = _OccurrenceSpecification;
      _Interactions2.__OccurrenceSpecification_Uri = "dm:///_internal/model/uml#OccurrenceSpecification";
      class _PartDecomposition {
      }
      _PartDecomposition.actualGate = "actualGate";
      _PartDecomposition.argument = "argument";
      _PartDecomposition.refersTo = "refersTo";
      _PartDecomposition.returnValue = "returnValue";
      _PartDecomposition.returnValueRecipient = "returnValueRecipient";
      _PartDecomposition.covered = "covered";
      _PartDecomposition.enclosingInteraction = "enclosingInteraction";
      _PartDecomposition.enclosingOperand = "enclosingOperand";
      _PartDecomposition.generalOrdering = "generalOrdering";
      _PartDecomposition.clientDependency = "clientDependency";
      _PartDecomposition._name_ = "name";
      _PartDecomposition.nameExpression = "nameExpression";
      _PartDecomposition.namespace = "namespace";
      _PartDecomposition.qualifiedName = "qualifiedName";
      _PartDecomposition.visibility = "visibility";
      _PartDecomposition.ownedComment = "ownedComment";
      _PartDecomposition.ownedElement = "ownedElement";
      _PartDecomposition.owner = "owner";
      _Interactions2._PartDecomposition = _PartDecomposition;
      _Interactions2.__PartDecomposition_Uri = "dm:///_internal/model/uml#PartDecomposition";
      class _StateInvariant {
      }
      _StateInvariant.covered = "covered";
      _StateInvariant.invariant = "invariant";
      _StateInvariant.enclosingInteraction = "enclosingInteraction";
      _StateInvariant.enclosingOperand = "enclosingOperand";
      _StateInvariant.generalOrdering = "generalOrdering";
      _StateInvariant.clientDependency = "clientDependency";
      _StateInvariant._name_ = "name";
      _StateInvariant.nameExpression = "nameExpression";
      _StateInvariant.namespace = "namespace";
      _StateInvariant.qualifiedName = "qualifiedName";
      _StateInvariant.visibility = "visibility";
      _StateInvariant.ownedComment = "ownedComment";
      _StateInvariant.ownedElement = "ownedElement";
      _StateInvariant.owner = "owner";
      _Interactions2._StateInvariant = _StateInvariant;
      _Interactions2.__StateInvariant_Uri = "dm:///_internal/model/uml#StateInvariant";
      let _InteractionOperatorKind;
      (function(_InteractionOperatorKind2) {
        _InteractionOperatorKind2.seq = "seq";
        _InteractionOperatorKind2.alt = "alt";
        _InteractionOperatorKind2.opt = "opt";
        _InteractionOperatorKind2._break_ = "break";
        _InteractionOperatorKind2.par = "par";
        _InteractionOperatorKind2.strict = "strict";
        _InteractionOperatorKind2.loop = "loop";
        _InteractionOperatorKind2.critical = "critical";
        _InteractionOperatorKind2.neg = "neg";
        _InteractionOperatorKind2.assert = "assert";
        _InteractionOperatorKind2.ignore = "ignore";
        _InteractionOperatorKind2.consider = "consider";
      })(_InteractionOperatorKind = _Interactions2._InteractionOperatorKind || (_Interactions2._InteractionOperatorKind = {}));
      let ___InteractionOperatorKind;
      (function(___InteractionOperatorKind2) {
        ___InteractionOperatorKind2[___InteractionOperatorKind2["seq"] = 0] = "seq";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["alt"] = 1] = "alt";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["opt"] = 2] = "opt";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["_break_"] = 3] = "_break_";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["par"] = 4] = "par";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["strict"] = 5] = "strict";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["loop"] = 6] = "loop";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["critical"] = 7] = "critical";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["neg"] = 8] = "neg";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["assert"] = 9] = "assert";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["ignore"] = 10] = "ignore";
        ___InteractionOperatorKind2[___InteractionOperatorKind2["consider"] = 11] = "consider";
      })(___InteractionOperatorKind = _Interactions2.___InteractionOperatorKind || (_Interactions2.___InteractionOperatorKind = {}));
      let _MessageKind;
      (function(_MessageKind2) {
        _MessageKind2.complete = "complete";
        _MessageKind2.lost = "lost";
        _MessageKind2.found = "found";
        _MessageKind2.unknown = "unknown";
      })(_MessageKind = _Interactions2._MessageKind || (_Interactions2._MessageKind = {}));
      let ___MessageKind;
      (function(___MessageKind2) {
        ___MessageKind2[___MessageKind2["complete"] = 0] = "complete";
        ___MessageKind2[___MessageKind2["lost"] = 1] = "lost";
        ___MessageKind2[___MessageKind2["found"] = 2] = "found";
        ___MessageKind2[___MessageKind2["unknown"] = 3] = "unknown";
      })(___MessageKind = _Interactions2.___MessageKind || (_Interactions2.___MessageKind = {}));
      let _MessageSort;
      (function(_MessageSort2) {
        _MessageSort2.synchCall = "synchCall";
        _MessageSort2.asynchCall = "asynchCall";
        _MessageSort2.asynchSignal = "asynchSignal";
        _MessageSort2.createMessage = "createMessage";
        _MessageSort2.deleteMessage = "deleteMessage";
        _MessageSort2.reply = "reply";
      })(_MessageSort = _Interactions2._MessageSort || (_Interactions2._MessageSort = {}));
      let ___MessageSort;
      (function(___MessageSort2) {
        ___MessageSort2[___MessageSort2["synchCall"] = 0] = "synchCall";
        ___MessageSort2[___MessageSort2["asynchCall"] = 1] = "asynchCall";
        ___MessageSort2[___MessageSort2["asynchSignal"] = 2] = "asynchSignal";
        ___MessageSort2[___MessageSort2["createMessage"] = 3] = "createMessage";
        ___MessageSort2[___MessageSort2["deleteMessage"] = 4] = "deleteMessage";
        ___MessageSort2[___MessageSort2["reply"] = 5] = "reply";
      })(___MessageSort = _Interactions2.___MessageSort || (_Interactions2.___MessageSort = {}));
    })(_Interactions = _UML2._Interactions || (_UML2._Interactions = {}));
    let _InformationFlows;
    (function(_InformationFlows2) {
      class _InformationFlow {
      }
      _InformationFlow.conveyed = "conveyed";
      _InformationFlow.informationSource = "informationSource";
      _InformationFlow.informationTarget = "informationTarget";
      _InformationFlow.realization = "realization";
      _InformationFlow.realizingActivityEdge = "realizingActivityEdge";
      _InformationFlow.realizingConnector = "realizingConnector";
      _InformationFlow.realizingMessage = "realizingMessage";
      _InformationFlow.source = "source";
      _InformationFlow.target = "target";
      _InformationFlow.relatedElement = "relatedElement";
      _InformationFlow.ownedComment = "ownedComment";
      _InformationFlow.ownedElement = "ownedElement";
      _InformationFlow.owner = "owner";
      _InformationFlow.visibility = "visibility";
      _InformationFlow.owningTemplateParameter = "owningTemplateParameter";
      _InformationFlow.templateParameter = "templateParameter";
      _InformationFlow.clientDependency = "clientDependency";
      _InformationFlow._name_ = "name";
      _InformationFlow.nameExpression = "nameExpression";
      _InformationFlow.namespace = "namespace";
      _InformationFlow.qualifiedName = "qualifiedName";
      _InformationFlows2._InformationFlow = _InformationFlow;
      _InformationFlows2.__InformationFlow_Uri = "dm:///_internal/model/uml#InformationFlow";
      class _InformationItem {
      }
      _InformationItem.represented = "represented";
      _InformationItem.attribute = "attribute";
      _InformationItem.collaborationUse = "collaborationUse";
      _InformationItem.feature = "feature";
      _InformationItem.general = "general";
      _InformationItem.generalization = "generalization";
      _InformationItem.inheritedMember = "inheritedMember";
      _InformationItem.isAbstract = "isAbstract";
      _InformationItem.isFinalSpecialization = "isFinalSpecialization";
      _InformationItem.ownedTemplateSignature = "ownedTemplateSignature";
      _InformationItem.ownedUseCase = "ownedUseCase";
      _InformationItem.powertypeExtent = "powertypeExtent";
      _InformationItem.redefinedClassifier = "redefinedClassifier";
      _InformationItem.representation = "representation";
      _InformationItem.substitution = "substitution";
      _InformationItem.templateParameter = "templateParameter";
      _InformationItem.useCase = "useCase";
      _InformationItem.elementImport = "elementImport";
      _InformationItem.importedMember = "importedMember";
      _InformationItem.member = "member";
      _InformationItem.ownedMember = "ownedMember";
      _InformationItem.ownedRule = "ownedRule";
      _InformationItem.packageImport = "packageImport";
      _InformationItem.clientDependency = "clientDependency";
      _InformationItem._name_ = "name";
      _InformationItem.nameExpression = "nameExpression";
      _InformationItem.namespace = "namespace";
      _InformationItem.qualifiedName = "qualifiedName";
      _InformationItem.visibility = "visibility";
      _InformationItem.ownedComment = "ownedComment";
      _InformationItem.ownedElement = "ownedElement";
      _InformationItem.owner = "owner";
      _InformationItem._package_ = "package";
      _InformationItem.owningTemplateParameter = "owningTemplateParameter";
      _InformationItem.templateBinding = "templateBinding";
      _InformationItem.isLeaf = "isLeaf";
      _InformationItem.redefinedElement = "redefinedElement";
      _InformationItem.redefinitionContext = "redefinitionContext";
      _InformationFlows2._InformationItem = _InformationItem;
      _InformationFlows2.__InformationItem_Uri = "dm:///_internal/model/uml#InformationItem";
    })(_InformationFlows = _UML2._InformationFlows || (_UML2._InformationFlows = {}));
    let _Deployments;
    (function(_Deployments2) {
      class _Artifact {
      }
      _Artifact.fileName = "fileName";
      _Artifact.manifestation = "manifestation";
      _Artifact.nestedArtifact = "nestedArtifact";
      _Artifact.ownedAttribute = "ownedAttribute";
      _Artifact.ownedOperation = "ownedOperation";
      _Artifact.attribute = "attribute";
      _Artifact.collaborationUse = "collaborationUse";
      _Artifact.feature = "feature";
      _Artifact.general = "general";
      _Artifact.generalization = "generalization";
      _Artifact.inheritedMember = "inheritedMember";
      _Artifact.isAbstract = "isAbstract";
      _Artifact.isFinalSpecialization = "isFinalSpecialization";
      _Artifact.ownedTemplateSignature = "ownedTemplateSignature";
      _Artifact.ownedUseCase = "ownedUseCase";
      _Artifact.powertypeExtent = "powertypeExtent";
      _Artifact.redefinedClassifier = "redefinedClassifier";
      _Artifact.representation = "representation";
      _Artifact.substitution = "substitution";
      _Artifact.templateParameter = "templateParameter";
      _Artifact.useCase = "useCase";
      _Artifact.elementImport = "elementImport";
      _Artifact.importedMember = "importedMember";
      _Artifact.member = "member";
      _Artifact.ownedMember = "ownedMember";
      _Artifact.ownedRule = "ownedRule";
      _Artifact.packageImport = "packageImport";
      _Artifact.clientDependency = "clientDependency";
      _Artifact._name_ = "name";
      _Artifact.nameExpression = "nameExpression";
      _Artifact.namespace = "namespace";
      _Artifact.qualifiedName = "qualifiedName";
      _Artifact.visibility = "visibility";
      _Artifact.ownedComment = "ownedComment";
      _Artifact.ownedElement = "ownedElement";
      _Artifact.owner = "owner";
      _Artifact._package_ = "package";
      _Artifact.owningTemplateParameter = "owningTemplateParameter";
      _Artifact.templateBinding = "templateBinding";
      _Artifact.isLeaf = "isLeaf";
      _Artifact.redefinedElement = "redefinedElement";
      _Artifact.redefinitionContext = "redefinitionContext";
      _Deployments2._Artifact = _Artifact;
      _Deployments2.__Artifact_Uri = "dm:///_internal/model/uml#Artifact";
      class _CommunicationPath {
      }
      _CommunicationPath.endType = "endType";
      _CommunicationPath.isDerived = "isDerived";
      _CommunicationPath.memberEnd = "memberEnd";
      _CommunicationPath.navigableOwnedEnd = "navigableOwnedEnd";
      _CommunicationPath.ownedEnd = "ownedEnd";
      _CommunicationPath.relatedElement = "relatedElement";
      _CommunicationPath.ownedComment = "ownedComment";
      _CommunicationPath.ownedElement = "ownedElement";
      _CommunicationPath.owner = "owner";
      _CommunicationPath.attribute = "attribute";
      _CommunicationPath.collaborationUse = "collaborationUse";
      _CommunicationPath.feature = "feature";
      _CommunicationPath.general = "general";
      _CommunicationPath.generalization = "generalization";
      _CommunicationPath.inheritedMember = "inheritedMember";
      _CommunicationPath.isAbstract = "isAbstract";
      _CommunicationPath.isFinalSpecialization = "isFinalSpecialization";
      _CommunicationPath.ownedTemplateSignature = "ownedTemplateSignature";
      _CommunicationPath.ownedUseCase = "ownedUseCase";
      _CommunicationPath.powertypeExtent = "powertypeExtent";
      _CommunicationPath.redefinedClassifier = "redefinedClassifier";
      _CommunicationPath.representation = "representation";
      _CommunicationPath.substitution = "substitution";
      _CommunicationPath.templateParameter = "templateParameter";
      _CommunicationPath.useCase = "useCase";
      _CommunicationPath.elementImport = "elementImport";
      _CommunicationPath.importedMember = "importedMember";
      _CommunicationPath.member = "member";
      _CommunicationPath.ownedMember = "ownedMember";
      _CommunicationPath.ownedRule = "ownedRule";
      _CommunicationPath.packageImport = "packageImport";
      _CommunicationPath.clientDependency = "clientDependency";
      _CommunicationPath._name_ = "name";
      _CommunicationPath.nameExpression = "nameExpression";
      _CommunicationPath.namespace = "namespace";
      _CommunicationPath.qualifiedName = "qualifiedName";
      _CommunicationPath.visibility = "visibility";
      _CommunicationPath._package_ = "package";
      _CommunicationPath.owningTemplateParameter = "owningTemplateParameter";
      _CommunicationPath.templateBinding = "templateBinding";
      _CommunicationPath.isLeaf = "isLeaf";
      _CommunicationPath.redefinedElement = "redefinedElement";
      _CommunicationPath.redefinitionContext = "redefinitionContext";
      _Deployments2._CommunicationPath = _CommunicationPath;
      _Deployments2.__CommunicationPath_Uri = "dm:///_internal/model/uml#CommunicationPath";
      class _DeployedArtifact {
      }
      _DeployedArtifact.clientDependency = "clientDependency";
      _DeployedArtifact._name_ = "name";
      _DeployedArtifact.nameExpression = "nameExpression";
      _DeployedArtifact.namespace = "namespace";
      _DeployedArtifact.qualifiedName = "qualifiedName";
      _DeployedArtifact.visibility = "visibility";
      _DeployedArtifact.ownedComment = "ownedComment";
      _DeployedArtifact.ownedElement = "ownedElement";
      _DeployedArtifact.owner = "owner";
      _Deployments2._DeployedArtifact = _DeployedArtifact;
      _Deployments2.__DeployedArtifact_Uri = "dm:///_internal/model/uml#DeployedArtifact";
      class _Deployment {
      }
      _Deployment.configuration = "configuration";
      _Deployment.deployedArtifact = "deployedArtifact";
      _Deployment.location = "location";
      _Deployment.client = "client";
      _Deployment.supplier = "supplier";
      _Deployment.source = "source";
      _Deployment.target = "target";
      _Deployment.relatedElement = "relatedElement";
      _Deployment.ownedComment = "ownedComment";
      _Deployment.ownedElement = "ownedElement";
      _Deployment.owner = "owner";
      _Deployment.visibility = "visibility";
      _Deployment.owningTemplateParameter = "owningTemplateParameter";
      _Deployment.templateParameter = "templateParameter";
      _Deployment.clientDependency = "clientDependency";
      _Deployment._name_ = "name";
      _Deployment.nameExpression = "nameExpression";
      _Deployment.namespace = "namespace";
      _Deployment.qualifiedName = "qualifiedName";
      _Deployments2._Deployment = _Deployment;
      _Deployments2.__Deployment_Uri = "dm:///_internal/model/uml#Deployment";
      class _DeploymentSpecification {
      }
      _DeploymentSpecification.deployment = "deployment";
      _DeploymentSpecification.deploymentLocation = "deploymentLocation";
      _DeploymentSpecification.executionLocation = "executionLocation";
      _DeploymentSpecification.fileName = "fileName";
      _DeploymentSpecification.manifestation = "manifestation";
      _DeploymentSpecification.nestedArtifact = "nestedArtifact";
      _DeploymentSpecification.ownedAttribute = "ownedAttribute";
      _DeploymentSpecification.ownedOperation = "ownedOperation";
      _DeploymentSpecification.attribute = "attribute";
      _DeploymentSpecification.collaborationUse = "collaborationUse";
      _DeploymentSpecification.feature = "feature";
      _DeploymentSpecification.general = "general";
      _DeploymentSpecification.generalization = "generalization";
      _DeploymentSpecification.inheritedMember = "inheritedMember";
      _DeploymentSpecification.isAbstract = "isAbstract";
      _DeploymentSpecification.isFinalSpecialization = "isFinalSpecialization";
      _DeploymentSpecification.ownedTemplateSignature = "ownedTemplateSignature";
      _DeploymentSpecification.ownedUseCase = "ownedUseCase";
      _DeploymentSpecification.powertypeExtent = "powertypeExtent";
      _DeploymentSpecification.redefinedClassifier = "redefinedClassifier";
      _DeploymentSpecification.representation = "representation";
      _DeploymentSpecification.substitution = "substitution";
      _DeploymentSpecification.templateParameter = "templateParameter";
      _DeploymentSpecification.useCase = "useCase";
      _DeploymentSpecification.elementImport = "elementImport";
      _DeploymentSpecification.importedMember = "importedMember";
      _DeploymentSpecification.member = "member";
      _DeploymentSpecification.ownedMember = "ownedMember";
      _DeploymentSpecification.ownedRule = "ownedRule";
      _DeploymentSpecification.packageImport = "packageImport";
      _DeploymentSpecification.clientDependency = "clientDependency";
      _DeploymentSpecification._name_ = "name";
      _DeploymentSpecification.nameExpression = "nameExpression";
      _DeploymentSpecification.namespace = "namespace";
      _DeploymentSpecification.qualifiedName = "qualifiedName";
      _DeploymentSpecification.visibility = "visibility";
      _DeploymentSpecification.ownedComment = "ownedComment";
      _DeploymentSpecification.ownedElement = "ownedElement";
      _DeploymentSpecification.owner = "owner";
      _DeploymentSpecification._package_ = "package";
      _DeploymentSpecification.owningTemplateParameter = "owningTemplateParameter";
      _DeploymentSpecification.templateBinding = "templateBinding";
      _DeploymentSpecification.isLeaf = "isLeaf";
      _DeploymentSpecification.redefinedElement = "redefinedElement";
      _DeploymentSpecification.redefinitionContext = "redefinitionContext";
      _Deployments2._DeploymentSpecification = _DeploymentSpecification;
      _Deployments2.__DeploymentSpecification_Uri = "dm:///_internal/model/uml#DeploymentSpecification";
      class _DeploymentTarget {
      }
      _DeploymentTarget.deployedElement = "deployedElement";
      _DeploymentTarget.deployment = "deployment";
      _DeploymentTarget.clientDependency = "clientDependency";
      _DeploymentTarget._name_ = "name";
      _DeploymentTarget.nameExpression = "nameExpression";
      _DeploymentTarget.namespace = "namespace";
      _DeploymentTarget.qualifiedName = "qualifiedName";
      _DeploymentTarget.visibility = "visibility";
      _DeploymentTarget.ownedComment = "ownedComment";
      _DeploymentTarget.ownedElement = "ownedElement";
      _DeploymentTarget.owner = "owner";
      _Deployments2._DeploymentTarget = _DeploymentTarget;
      _Deployments2.__DeploymentTarget_Uri = "dm:///_internal/model/uml#DeploymentTarget";
      class _Device {
      }
      _Device.nestedNode = "nestedNode";
      _Device.extension = "extension";
      _Device.isAbstract = "isAbstract";
      _Device.isActive = "isActive";
      _Device.nestedClassifier = "nestedClassifier";
      _Device.ownedAttribute = "ownedAttribute";
      _Device.ownedOperation = "ownedOperation";
      _Device.ownedReception = "ownedReception";
      _Device.superClass = "superClass";
      _Device.classifierBehavior = "classifierBehavior";
      _Device.interfaceRealization = "interfaceRealization";
      _Device.ownedBehavior = "ownedBehavior";
      _Device.attribute = "attribute";
      _Device.collaborationUse = "collaborationUse";
      _Device.feature = "feature";
      _Device.general = "general";
      _Device.generalization = "generalization";
      _Device.inheritedMember = "inheritedMember";
      _Device.isFinalSpecialization = "isFinalSpecialization";
      _Device.ownedTemplateSignature = "ownedTemplateSignature";
      _Device.ownedUseCase = "ownedUseCase";
      _Device.powertypeExtent = "powertypeExtent";
      _Device.redefinedClassifier = "redefinedClassifier";
      _Device.representation = "representation";
      _Device.substitution = "substitution";
      _Device.templateParameter = "templateParameter";
      _Device.useCase = "useCase";
      _Device.elementImport = "elementImport";
      _Device.importedMember = "importedMember";
      _Device.member = "member";
      _Device.ownedMember = "ownedMember";
      _Device.ownedRule = "ownedRule";
      _Device.packageImport = "packageImport";
      _Device.clientDependency = "clientDependency";
      _Device._name_ = "name";
      _Device.nameExpression = "nameExpression";
      _Device.namespace = "namespace";
      _Device.qualifiedName = "qualifiedName";
      _Device.visibility = "visibility";
      _Device.ownedComment = "ownedComment";
      _Device.ownedElement = "ownedElement";
      _Device.owner = "owner";
      _Device._package_ = "package";
      _Device.owningTemplateParameter = "owningTemplateParameter";
      _Device.templateBinding = "templateBinding";
      _Device.isLeaf = "isLeaf";
      _Device.redefinedElement = "redefinedElement";
      _Device.redefinitionContext = "redefinitionContext";
      _Device.ownedPort = "ownedPort";
      _Device.ownedConnector = "ownedConnector";
      _Device.part = "part";
      _Device.role = "role";
      _Device.deployedElement = "deployedElement";
      _Device.deployment = "deployment";
      _Deployments2._Device = _Device;
      _Deployments2.__Device_Uri = "dm:///_internal/model/uml#Device";
      class _ExecutionEnvironment {
      }
      _ExecutionEnvironment.nestedNode = "nestedNode";
      _ExecutionEnvironment.extension = "extension";
      _ExecutionEnvironment.isAbstract = "isAbstract";
      _ExecutionEnvironment.isActive = "isActive";
      _ExecutionEnvironment.nestedClassifier = "nestedClassifier";
      _ExecutionEnvironment.ownedAttribute = "ownedAttribute";
      _ExecutionEnvironment.ownedOperation = "ownedOperation";
      _ExecutionEnvironment.ownedReception = "ownedReception";
      _ExecutionEnvironment.superClass = "superClass";
      _ExecutionEnvironment.classifierBehavior = "classifierBehavior";
      _ExecutionEnvironment.interfaceRealization = "interfaceRealization";
      _ExecutionEnvironment.ownedBehavior = "ownedBehavior";
      _ExecutionEnvironment.attribute = "attribute";
      _ExecutionEnvironment.collaborationUse = "collaborationUse";
      _ExecutionEnvironment.feature = "feature";
      _ExecutionEnvironment.general = "general";
      _ExecutionEnvironment.generalization = "generalization";
      _ExecutionEnvironment.inheritedMember = "inheritedMember";
      _ExecutionEnvironment.isFinalSpecialization = "isFinalSpecialization";
      _ExecutionEnvironment.ownedTemplateSignature = "ownedTemplateSignature";
      _ExecutionEnvironment.ownedUseCase = "ownedUseCase";
      _ExecutionEnvironment.powertypeExtent = "powertypeExtent";
      _ExecutionEnvironment.redefinedClassifier = "redefinedClassifier";
      _ExecutionEnvironment.representation = "representation";
      _ExecutionEnvironment.substitution = "substitution";
      _ExecutionEnvironment.templateParameter = "templateParameter";
      _ExecutionEnvironment.useCase = "useCase";
      _ExecutionEnvironment.elementImport = "elementImport";
      _ExecutionEnvironment.importedMember = "importedMember";
      _ExecutionEnvironment.member = "member";
      _ExecutionEnvironment.ownedMember = "ownedMember";
      _ExecutionEnvironment.ownedRule = "ownedRule";
      _ExecutionEnvironment.packageImport = "packageImport";
      _ExecutionEnvironment.clientDependency = "clientDependency";
      _ExecutionEnvironment._name_ = "name";
      _ExecutionEnvironment.nameExpression = "nameExpression";
      _ExecutionEnvironment.namespace = "namespace";
      _ExecutionEnvironment.qualifiedName = "qualifiedName";
      _ExecutionEnvironment.visibility = "visibility";
      _ExecutionEnvironment.ownedComment = "ownedComment";
      _ExecutionEnvironment.ownedElement = "ownedElement";
      _ExecutionEnvironment.owner = "owner";
      _ExecutionEnvironment._package_ = "package";
      _ExecutionEnvironment.owningTemplateParameter = "owningTemplateParameter";
      _ExecutionEnvironment.templateBinding = "templateBinding";
      _ExecutionEnvironment.isLeaf = "isLeaf";
      _ExecutionEnvironment.redefinedElement = "redefinedElement";
      _ExecutionEnvironment.redefinitionContext = "redefinitionContext";
      _ExecutionEnvironment.ownedPort = "ownedPort";
      _ExecutionEnvironment.ownedConnector = "ownedConnector";
      _ExecutionEnvironment.part = "part";
      _ExecutionEnvironment.role = "role";
      _ExecutionEnvironment.deployedElement = "deployedElement";
      _ExecutionEnvironment.deployment = "deployment";
      _Deployments2._ExecutionEnvironment = _ExecutionEnvironment;
      _Deployments2.__ExecutionEnvironment_Uri = "dm:///_internal/model/uml#ExecutionEnvironment";
      class _Manifestation {
      }
      _Manifestation.utilizedElement = "utilizedElement";
      _Manifestation.mapping = "mapping";
      _Manifestation.client = "client";
      _Manifestation.supplier = "supplier";
      _Manifestation.source = "source";
      _Manifestation.target = "target";
      _Manifestation.relatedElement = "relatedElement";
      _Manifestation.ownedComment = "ownedComment";
      _Manifestation.ownedElement = "ownedElement";
      _Manifestation.owner = "owner";
      _Manifestation.visibility = "visibility";
      _Manifestation.owningTemplateParameter = "owningTemplateParameter";
      _Manifestation.templateParameter = "templateParameter";
      _Manifestation.clientDependency = "clientDependency";
      _Manifestation._name_ = "name";
      _Manifestation.nameExpression = "nameExpression";
      _Manifestation.namespace = "namespace";
      _Manifestation.qualifiedName = "qualifiedName";
      _Deployments2._Manifestation = _Manifestation;
      _Deployments2.__Manifestation_Uri = "dm:///_internal/model/uml#Manifestation";
      class _Node {
      }
      _Node.nestedNode = "nestedNode";
      _Node.extension = "extension";
      _Node.isAbstract = "isAbstract";
      _Node.isActive = "isActive";
      _Node.nestedClassifier = "nestedClassifier";
      _Node.ownedAttribute = "ownedAttribute";
      _Node.ownedOperation = "ownedOperation";
      _Node.ownedReception = "ownedReception";
      _Node.superClass = "superClass";
      _Node.classifierBehavior = "classifierBehavior";
      _Node.interfaceRealization = "interfaceRealization";
      _Node.ownedBehavior = "ownedBehavior";
      _Node.attribute = "attribute";
      _Node.collaborationUse = "collaborationUse";
      _Node.feature = "feature";
      _Node.general = "general";
      _Node.generalization = "generalization";
      _Node.inheritedMember = "inheritedMember";
      _Node.isFinalSpecialization = "isFinalSpecialization";
      _Node.ownedTemplateSignature = "ownedTemplateSignature";
      _Node.ownedUseCase = "ownedUseCase";
      _Node.powertypeExtent = "powertypeExtent";
      _Node.redefinedClassifier = "redefinedClassifier";
      _Node.representation = "representation";
      _Node.substitution = "substitution";
      _Node.templateParameter = "templateParameter";
      _Node.useCase = "useCase";
      _Node.elementImport = "elementImport";
      _Node.importedMember = "importedMember";
      _Node.member = "member";
      _Node.ownedMember = "ownedMember";
      _Node.ownedRule = "ownedRule";
      _Node.packageImport = "packageImport";
      _Node.clientDependency = "clientDependency";
      _Node._name_ = "name";
      _Node.nameExpression = "nameExpression";
      _Node.namespace = "namespace";
      _Node.qualifiedName = "qualifiedName";
      _Node.visibility = "visibility";
      _Node.ownedComment = "ownedComment";
      _Node.ownedElement = "ownedElement";
      _Node.owner = "owner";
      _Node._package_ = "package";
      _Node.owningTemplateParameter = "owningTemplateParameter";
      _Node.templateBinding = "templateBinding";
      _Node.isLeaf = "isLeaf";
      _Node.redefinedElement = "redefinedElement";
      _Node.redefinitionContext = "redefinitionContext";
      _Node.ownedPort = "ownedPort";
      _Node.ownedConnector = "ownedConnector";
      _Node.part = "part";
      _Node.role = "role";
      _Node.deployedElement = "deployedElement";
      _Node.deployment = "deployment";
      _Deployments2._Node = _Node;
      _Deployments2.__Node_Uri = "dm:///_internal/model/uml#Node";
    })(_Deployments = _UML2._Deployments || (_UML2._Deployments = {}));
    let _CommonStructure;
    (function(_CommonStructure2) {
      class _Abstraction {
      }
      _Abstraction.mapping = "mapping";
      _Abstraction.client = "client";
      _Abstraction.supplier = "supplier";
      _Abstraction.source = "source";
      _Abstraction.target = "target";
      _Abstraction.relatedElement = "relatedElement";
      _Abstraction.ownedComment = "ownedComment";
      _Abstraction.ownedElement = "ownedElement";
      _Abstraction.owner = "owner";
      _Abstraction.visibility = "visibility";
      _Abstraction.owningTemplateParameter = "owningTemplateParameter";
      _Abstraction.templateParameter = "templateParameter";
      _Abstraction.clientDependency = "clientDependency";
      _Abstraction._name_ = "name";
      _Abstraction.nameExpression = "nameExpression";
      _Abstraction.namespace = "namespace";
      _Abstraction.qualifiedName = "qualifiedName";
      _CommonStructure2._Abstraction = _Abstraction;
      _CommonStructure2.__Abstraction_Uri = "dm:///_internal/model/uml#Abstraction";
      class _Comment {
      }
      _Comment.annotatedElement = "annotatedElement";
      _Comment.body = "body";
      _Comment.ownedComment = "ownedComment";
      _Comment.ownedElement = "ownedElement";
      _Comment.owner = "owner";
      _CommonStructure2._Comment = _Comment;
      _CommonStructure2.__Comment_Uri = "dm:///_internal/model/uml#Comment";
      class _Constraint {
      }
      _Constraint.constrainedElement = "constrainedElement";
      _Constraint.context = "context";
      _Constraint.specification = "specification";
      _Constraint.visibility = "visibility";
      _Constraint.owningTemplateParameter = "owningTemplateParameter";
      _Constraint.templateParameter = "templateParameter";
      _Constraint.ownedComment = "ownedComment";
      _Constraint.ownedElement = "ownedElement";
      _Constraint.owner = "owner";
      _Constraint.clientDependency = "clientDependency";
      _Constraint._name_ = "name";
      _Constraint.nameExpression = "nameExpression";
      _Constraint.namespace = "namespace";
      _Constraint.qualifiedName = "qualifiedName";
      _CommonStructure2._Constraint = _Constraint;
      _CommonStructure2.__Constraint_Uri = "dm:///_internal/model/uml#Constraint";
      class _Dependency {
      }
      _Dependency.client = "client";
      _Dependency.supplier = "supplier";
      _Dependency.source = "source";
      _Dependency.target = "target";
      _Dependency.relatedElement = "relatedElement";
      _Dependency.ownedComment = "ownedComment";
      _Dependency.ownedElement = "ownedElement";
      _Dependency.owner = "owner";
      _Dependency.visibility = "visibility";
      _Dependency.owningTemplateParameter = "owningTemplateParameter";
      _Dependency.templateParameter = "templateParameter";
      _Dependency.clientDependency = "clientDependency";
      _Dependency._name_ = "name";
      _Dependency.nameExpression = "nameExpression";
      _Dependency.namespace = "namespace";
      _Dependency.qualifiedName = "qualifiedName";
      _CommonStructure2._Dependency = _Dependency;
      _CommonStructure2.__Dependency_Uri = "dm:///_internal/model/uml#Dependency";
      class _DirectedRelationship {
      }
      _DirectedRelationship.source = "source";
      _DirectedRelationship.target = "target";
      _DirectedRelationship.relatedElement = "relatedElement";
      _DirectedRelationship.ownedComment = "ownedComment";
      _DirectedRelationship.ownedElement = "ownedElement";
      _DirectedRelationship.owner = "owner";
      _CommonStructure2._DirectedRelationship = _DirectedRelationship;
      _CommonStructure2.__DirectedRelationship_Uri = "dm:///_internal/model/uml#DirectedRelationship";
      class _Element {
      }
      _Element.ownedComment = "ownedComment";
      _Element.ownedElement = "ownedElement";
      _Element.owner = "owner";
      _CommonStructure2._Element = _Element;
      _CommonStructure2.__Element_Uri = "dm:///_internal/model/uml#Element";
      class _ElementImport {
      }
      _ElementImport.alias = "alias";
      _ElementImport.importedElement = "importedElement";
      _ElementImport.importingNamespace = "importingNamespace";
      _ElementImport.visibility = "visibility";
      _ElementImport.source = "source";
      _ElementImport.target = "target";
      _ElementImport.relatedElement = "relatedElement";
      _ElementImport.ownedComment = "ownedComment";
      _ElementImport.ownedElement = "ownedElement";
      _ElementImport.owner = "owner";
      _CommonStructure2._ElementImport = _ElementImport;
      _CommonStructure2.__ElementImport_Uri = "dm:///_internal/model/uml#ElementImport";
      class _MultiplicityElement {
      }
      _MultiplicityElement.isOrdered = "isOrdered";
      _MultiplicityElement.isUnique = "isUnique";
      _MultiplicityElement.lower = "lower";
      _MultiplicityElement.lowerValue = "lowerValue";
      _MultiplicityElement.upper = "upper";
      _MultiplicityElement.upperValue = "upperValue";
      _MultiplicityElement.ownedComment = "ownedComment";
      _MultiplicityElement.ownedElement = "ownedElement";
      _MultiplicityElement.owner = "owner";
      _CommonStructure2._MultiplicityElement = _MultiplicityElement;
      _CommonStructure2.__MultiplicityElement_Uri = "dm:///_internal/model/uml#MultiplicityElement";
      class _NamedElement2 {
      }
      _NamedElement2.clientDependency = "clientDependency";
      _NamedElement2._name_ = "name";
      _NamedElement2.nameExpression = "nameExpression";
      _NamedElement2.namespace = "namespace";
      _NamedElement2.qualifiedName = "qualifiedName";
      _NamedElement2.visibility = "visibility";
      _NamedElement2.ownedComment = "ownedComment";
      _NamedElement2.ownedElement = "ownedElement";
      _NamedElement2.owner = "owner";
      _CommonStructure2._NamedElement = _NamedElement2;
      _CommonStructure2.__NamedElement_Uri = "dm:///_internal/model/uml#NamedElement";
      class _Namespace {
      }
      _Namespace.elementImport = "elementImport";
      _Namespace.importedMember = "importedMember";
      _Namespace.member = "member";
      _Namespace.ownedMember = "ownedMember";
      _Namespace.ownedRule = "ownedRule";
      _Namespace.packageImport = "packageImport";
      _Namespace.clientDependency = "clientDependency";
      _Namespace._name_ = "name";
      _Namespace.nameExpression = "nameExpression";
      _Namespace.namespace = "namespace";
      _Namespace.qualifiedName = "qualifiedName";
      _Namespace.visibility = "visibility";
      _Namespace.ownedComment = "ownedComment";
      _Namespace.ownedElement = "ownedElement";
      _Namespace.owner = "owner";
      _CommonStructure2._Namespace = _Namespace;
      _CommonStructure2.__Namespace_Uri = "dm:///_internal/model/uml#Namespace";
      class _PackageableElement {
      }
      _PackageableElement.visibility = "visibility";
      _PackageableElement.owningTemplateParameter = "owningTemplateParameter";
      _PackageableElement.templateParameter = "templateParameter";
      _PackageableElement.ownedComment = "ownedComment";
      _PackageableElement.ownedElement = "ownedElement";
      _PackageableElement.owner = "owner";
      _PackageableElement.clientDependency = "clientDependency";
      _PackageableElement._name_ = "name";
      _PackageableElement.nameExpression = "nameExpression";
      _PackageableElement.namespace = "namespace";
      _PackageableElement.qualifiedName = "qualifiedName";
      _CommonStructure2._PackageableElement = _PackageableElement;
      _CommonStructure2.__PackageableElement_Uri = "dm:///_internal/model/uml#PackageableElement";
      class _PackageImport {
      }
      _PackageImport.importedPackage = "importedPackage";
      _PackageImport.importingNamespace = "importingNamespace";
      _PackageImport.visibility = "visibility";
      _PackageImport.source = "source";
      _PackageImport.target = "target";
      _PackageImport.relatedElement = "relatedElement";
      _PackageImport.ownedComment = "ownedComment";
      _PackageImport.ownedElement = "ownedElement";
      _PackageImport.owner = "owner";
      _CommonStructure2._PackageImport = _PackageImport;
      _CommonStructure2.__PackageImport_Uri = "dm:///_internal/model/uml#PackageImport";
      class _ParameterableElement {
      }
      _ParameterableElement.owningTemplateParameter = "owningTemplateParameter";
      _ParameterableElement.templateParameter = "templateParameter";
      _ParameterableElement.ownedComment = "ownedComment";
      _ParameterableElement.ownedElement = "ownedElement";
      _ParameterableElement.owner = "owner";
      _CommonStructure2._ParameterableElement = _ParameterableElement;
      _CommonStructure2.__ParameterableElement_Uri = "dm:///_internal/model/uml#ParameterableElement";
      class _Realization {
      }
      _Realization.mapping = "mapping";
      _Realization.client = "client";
      _Realization.supplier = "supplier";
      _Realization.source = "source";
      _Realization.target = "target";
      _Realization.relatedElement = "relatedElement";
      _Realization.ownedComment = "ownedComment";
      _Realization.ownedElement = "ownedElement";
      _Realization.owner = "owner";
      _Realization.visibility = "visibility";
      _Realization.owningTemplateParameter = "owningTemplateParameter";
      _Realization.templateParameter = "templateParameter";
      _Realization.clientDependency = "clientDependency";
      _Realization._name_ = "name";
      _Realization.nameExpression = "nameExpression";
      _Realization.namespace = "namespace";
      _Realization.qualifiedName = "qualifiedName";
      _CommonStructure2._Realization = _Realization;
      _CommonStructure2.__Realization_Uri = "dm:///_internal/model/uml#Realization";
      class _Relationship {
      }
      _Relationship.relatedElement = "relatedElement";
      _Relationship.ownedComment = "ownedComment";
      _Relationship.ownedElement = "ownedElement";
      _Relationship.owner = "owner";
      _CommonStructure2._Relationship = _Relationship;
      _CommonStructure2.__Relationship_Uri = "dm:///_internal/model/uml#Relationship";
      class _TemplateableElement {
      }
      _TemplateableElement.ownedTemplateSignature = "ownedTemplateSignature";
      _TemplateableElement.templateBinding = "templateBinding";
      _TemplateableElement.ownedComment = "ownedComment";
      _TemplateableElement.ownedElement = "ownedElement";
      _TemplateableElement.owner = "owner";
      _CommonStructure2._TemplateableElement = _TemplateableElement;
      _CommonStructure2.__TemplateableElement_Uri = "dm:///_internal/model/uml#TemplateableElement";
      class _TemplateBinding {
      }
      _TemplateBinding.boundElement = "boundElement";
      _TemplateBinding.parameterSubstitution = "parameterSubstitution";
      _TemplateBinding.signature = "signature";
      _TemplateBinding.source = "source";
      _TemplateBinding.target = "target";
      _TemplateBinding.relatedElement = "relatedElement";
      _TemplateBinding.ownedComment = "ownedComment";
      _TemplateBinding.ownedElement = "ownedElement";
      _TemplateBinding.owner = "owner";
      _CommonStructure2._TemplateBinding = _TemplateBinding;
      _CommonStructure2.__TemplateBinding_Uri = "dm:///_internal/model/uml#TemplateBinding";
      class _TemplateParameter {
      }
      _TemplateParameter._default_ = "default";
      _TemplateParameter.ownedDefault = "ownedDefault";
      _TemplateParameter.ownedParameteredElement = "ownedParameteredElement";
      _TemplateParameter.parameteredElement = "parameteredElement";
      _TemplateParameter.signature = "signature";
      _TemplateParameter.ownedComment = "ownedComment";
      _TemplateParameter.ownedElement = "ownedElement";
      _TemplateParameter.owner = "owner";
      _CommonStructure2._TemplateParameter = _TemplateParameter;
      _CommonStructure2.__TemplateParameter_Uri = "dm:///_internal/model/uml#TemplateParameter";
      class _TemplateParameterSubstitution {
      }
      _TemplateParameterSubstitution.actual = "actual";
      _TemplateParameterSubstitution.formal = "formal";
      _TemplateParameterSubstitution.ownedActual = "ownedActual";
      _TemplateParameterSubstitution.templateBinding = "templateBinding";
      _TemplateParameterSubstitution.ownedComment = "ownedComment";
      _TemplateParameterSubstitution.ownedElement = "ownedElement";
      _TemplateParameterSubstitution.owner = "owner";
      _CommonStructure2._TemplateParameterSubstitution = _TemplateParameterSubstitution;
      _CommonStructure2.__TemplateParameterSubstitution_Uri = "dm:///_internal/model/uml#TemplateParameterSubstitution";
      class _TemplateSignature {
      }
      _TemplateSignature.ownedParameter = "ownedParameter";
      _TemplateSignature.parameter = "parameter";
      _TemplateSignature.template = "template";
      _TemplateSignature.ownedComment = "ownedComment";
      _TemplateSignature.ownedElement = "ownedElement";
      _TemplateSignature.owner = "owner";
      _CommonStructure2._TemplateSignature = _TemplateSignature;
      _CommonStructure2.__TemplateSignature_Uri = "dm:///_internal/model/uml#TemplateSignature";
      class _Type {
      }
      _Type._package_ = "package";
      _Type.visibility = "visibility";
      _Type.owningTemplateParameter = "owningTemplateParameter";
      _Type.templateParameter = "templateParameter";
      _Type.ownedComment = "ownedComment";
      _Type.ownedElement = "ownedElement";
      _Type.owner = "owner";
      _Type.clientDependency = "clientDependency";
      _Type._name_ = "name";
      _Type.nameExpression = "nameExpression";
      _Type.namespace = "namespace";
      _Type.qualifiedName = "qualifiedName";
      _CommonStructure2._Type = _Type;
      _CommonStructure2.__Type_Uri = "dm:///_internal/model/uml#Type";
      class _TypedElement {
      }
      _TypedElement.type = "type";
      _TypedElement.clientDependency = "clientDependency";
      _TypedElement._name_ = "name";
      _TypedElement.nameExpression = "nameExpression";
      _TypedElement.namespace = "namespace";
      _TypedElement.qualifiedName = "qualifiedName";
      _TypedElement.visibility = "visibility";
      _TypedElement.ownedComment = "ownedComment";
      _TypedElement.ownedElement = "ownedElement";
      _TypedElement.owner = "owner";
      _CommonStructure2._TypedElement = _TypedElement;
      _CommonStructure2.__TypedElement_Uri = "dm:///_internal/model/uml#TypedElement";
      class _Usage {
      }
      _Usage.client = "client";
      _Usage.supplier = "supplier";
      _Usage.source = "source";
      _Usage.target = "target";
      _Usage.relatedElement = "relatedElement";
      _Usage.ownedComment = "ownedComment";
      _Usage.ownedElement = "ownedElement";
      _Usage.owner = "owner";
      _Usage.visibility = "visibility";
      _Usage.owningTemplateParameter = "owningTemplateParameter";
      _Usage.templateParameter = "templateParameter";
      _Usage.clientDependency = "clientDependency";
      _Usage._name_ = "name";
      _Usage.nameExpression = "nameExpression";
      _Usage.namespace = "namespace";
      _Usage.qualifiedName = "qualifiedName";
      _CommonStructure2._Usage = _Usage;
      _CommonStructure2.__Usage_Uri = "dm:///_internal/model/uml#Usage";
      let _VisibilityKind;
      (function(_VisibilityKind2) {
        _VisibilityKind2._public_ = "public";
        _VisibilityKind2._private_ = "private";
        _VisibilityKind2._protected_ = "protected";
        _VisibilityKind2._package_ = "package";
      })(_VisibilityKind = _CommonStructure2._VisibilityKind || (_CommonStructure2._VisibilityKind = {}));
      let ___VisibilityKind;
      (function(___VisibilityKind2) {
        ___VisibilityKind2[___VisibilityKind2["_public_"] = 0] = "_public_";
        ___VisibilityKind2[___VisibilityKind2["_private_"] = 1] = "_private_";
        ___VisibilityKind2[___VisibilityKind2["_protected_"] = 2] = "_protected_";
        ___VisibilityKind2[___VisibilityKind2["_package_"] = 3] = "_package_";
      })(___VisibilityKind = _CommonStructure2.___VisibilityKind || (_CommonStructure2.___VisibilityKind = {}));
    })(_CommonStructure = _UML2._CommonStructure || (_UML2._CommonStructure = {}));
    let _CommonBehavior;
    (function(_CommonBehavior2) {
      class _AnyReceiveEvent {
      }
      _AnyReceiveEvent.visibility = "visibility";
      _AnyReceiveEvent.owningTemplateParameter = "owningTemplateParameter";
      _AnyReceiveEvent.templateParameter = "templateParameter";
      _AnyReceiveEvent.ownedComment = "ownedComment";
      _AnyReceiveEvent.ownedElement = "ownedElement";
      _AnyReceiveEvent.owner = "owner";
      _AnyReceiveEvent.clientDependency = "clientDependency";
      _AnyReceiveEvent._name_ = "name";
      _AnyReceiveEvent.nameExpression = "nameExpression";
      _AnyReceiveEvent.namespace = "namespace";
      _AnyReceiveEvent.qualifiedName = "qualifiedName";
      _CommonBehavior2._AnyReceiveEvent = _AnyReceiveEvent;
      _CommonBehavior2.__AnyReceiveEvent_Uri = "dm:///_internal/model/uml#AnyReceiveEvent";
      class _Behavior {
      }
      _Behavior.context = "context";
      _Behavior.isReentrant = "isReentrant";
      _Behavior.ownedParameter = "ownedParameter";
      _Behavior.ownedParameterSet = "ownedParameterSet";
      _Behavior.postcondition = "postcondition";
      _Behavior.precondition = "precondition";
      _Behavior.specification = "specification";
      _Behavior.redefinedBehavior = "redefinedBehavior";
      _Behavior.extension = "extension";
      _Behavior.isAbstract = "isAbstract";
      _Behavior.isActive = "isActive";
      _Behavior.nestedClassifier = "nestedClassifier";
      _Behavior.ownedAttribute = "ownedAttribute";
      _Behavior.ownedOperation = "ownedOperation";
      _Behavior.ownedReception = "ownedReception";
      _Behavior.superClass = "superClass";
      _Behavior.classifierBehavior = "classifierBehavior";
      _Behavior.interfaceRealization = "interfaceRealization";
      _Behavior.ownedBehavior = "ownedBehavior";
      _Behavior.attribute = "attribute";
      _Behavior.collaborationUse = "collaborationUse";
      _Behavior.feature = "feature";
      _Behavior.general = "general";
      _Behavior.generalization = "generalization";
      _Behavior.inheritedMember = "inheritedMember";
      _Behavior.isFinalSpecialization = "isFinalSpecialization";
      _Behavior.ownedTemplateSignature = "ownedTemplateSignature";
      _Behavior.ownedUseCase = "ownedUseCase";
      _Behavior.powertypeExtent = "powertypeExtent";
      _Behavior.redefinedClassifier = "redefinedClassifier";
      _Behavior.representation = "representation";
      _Behavior.substitution = "substitution";
      _Behavior.templateParameter = "templateParameter";
      _Behavior.useCase = "useCase";
      _Behavior.elementImport = "elementImport";
      _Behavior.importedMember = "importedMember";
      _Behavior.member = "member";
      _Behavior.ownedMember = "ownedMember";
      _Behavior.ownedRule = "ownedRule";
      _Behavior.packageImport = "packageImport";
      _Behavior.clientDependency = "clientDependency";
      _Behavior._name_ = "name";
      _Behavior.nameExpression = "nameExpression";
      _Behavior.namespace = "namespace";
      _Behavior.qualifiedName = "qualifiedName";
      _Behavior.visibility = "visibility";
      _Behavior.ownedComment = "ownedComment";
      _Behavior.ownedElement = "ownedElement";
      _Behavior.owner = "owner";
      _Behavior._package_ = "package";
      _Behavior.owningTemplateParameter = "owningTemplateParameter";
      _Behavior.templateBinding = "templateBinding";
      _Behavior.isLeaf = "isLeaf";
      _Behavior.redefinedElement = "redefinedElement";
      _Behavior.redefinitionContext = "redefinitionContext";
      _Behavior.ownedPort = "ownedPort";
      _Behavior.ownedConnector = "ownedConnector";
      _Behavior.part = "part";
      _Behavior.role = "role";
      _CommonBehavior2._Behavior = _Behavior;
      _CommonBehavior2.__Behavior_Uri = "dm:///_internal/model/uml#Behavior";
      class _CallEvent {
      }
      _CallEvent.operation = "operation";
      _CallEvent.visibility = "visibility";
      _CallEvent.owningTemplateParameter = "owningTemplateParameter";
      _CallEvent.templateParameter = "templateParameter";
      _CallEvent.ownedComment = "ownedComment";
      _CallEvent.ownedElement = "ownedElement";
      _CallEvent.owner = "owner";
      _CallEvent.clientDependency = "clientDependency";
      _CallEvent._name_ = "name";
      _CallEvent.nameExpression = "nameExpression";
      _CallEvent.namespace = "namespace";
      _CallEvent.qualifiedName = "qualifiedName";
      _CommonBehavior2._CallEvent = _CallEvent;
      _CommonBehavior2.__CallEvent_Uri = "dm:///_internal/model/uml#CallEvent";
      class _ChangeEvent {
      }
      _ChangeEvent.changeExpression = "changeExpression";
      _ChangeEvent.visibility = "visibility";
      _ChangeEvent.owningTemplateParameter = "owningTemplateParameter";
      _ChangeEvent.templateParameter = "templateParameter";
      _ChangeEvent.ownedComment = "ownedComment";
      _ChangeEvent.ownedElement = "ownedElement";
      _ChangeEvent.owner = "owner";
      _ChangeEvent.clientDependency = "clientDependency";
      _ChangeEvent._name_ = "name";
      _ChangeEvent.nameExpression = "nameExpression";
      _ChangeEvent.namespace = "namespace";
      _ChangeEvent.qualifiedName = "qualifiedName";
      _CommonBehavior2._ChangeEvent = _ChangeEvent;
      _CommonBehavior2.__ChangeEvent_Uri = "dm:///_internal/model/uml#ChangeEvent";
      class _Event {
      }
      _Event.visibility = "visibility";
      _Event.owningTemplateParameter = "owningTemplateParameter";
      _Event.templateParameter = "templateParameter";
      _Event.ownedComment = "ownedComment";
      _Event.ownedElement = "ownedElement";
      _Event.owner = "owner";
      _Event.clientDependency = "clientDependency";
      _Event._name_ = "name";
      _Event.nameExpression = "nameExpression";
      _Event.namespace = "namespace";
      _Event.qualifiedName = "qualifiedName";
      _CommonBehavior2._Event = _Event;
      _CommonBehavior2.__Event_Uri = "dm:///_internal/model/uml#Event";
      class _FunctionBehavior {
      }
      _FunctionBehavior.body = "body";
      _FunctionBehavior.language = "language";
      _FunctionBehavior.context = "context";
      _FunctionBehavior.isReentrant = "isReentrant";
      _FunctionBehavior.ownedParameter = "ownedParameter";
      _FunctionBehavior.ownedParameterSet = "ownedParameterSet";
      _FunctionBehavior.postcondition = "postcondition";
      _FunctionBehavior.precondition = "precondition";
      _FunctionBehavior.specification = "specification";
      _FunctionBehavior.redefinedBehavior = "redefinedBehavior";
      _FunctionBehavior.extension = "extension";
      _FunctionBehavior.isAbstract = "isAbstract";
      _FunctionBehavior.isActive = "isActive";
      _FunctionBehavior.nestedClassifier = "nestedClassifier";
      _FunctionBehavior.ownedAttribute = "ownedAttribute";
      _FunctionBehavior.ownedOperation = "ownedOperation";
      _FunctionBehavior.ownedReception = "ownedReception";
      _FunctionBehavior.superClass = "superClass";
      _FunctionBehavior.classifierBehavior = "classifierBehavior";
      _FunctionBehavior.interfaceRealization = "interfaceRealization";
      _FunctionBehavior.ownedBehavior = "ownedBehavior";
      _FunctionBehavior.attribute = "attribute";
      _FunctionBehavior.collaborationUse = "collaborationUse";
      _FunctionBehavior.feature = "feature";
      _FunctionBehavior.general = "general";
      _FunctionBehavior.generalization = "generalization";
      _FunctionBehavior.inheritedMember = "inheritedMember";
      _FunctionBehavior.isFinalSpecialization = "isFinalSpecialization";
      _FunctionBehavior.ownedTemplateSignature = "ownedTemplateSignature";
      _FunctionBehavior.ownedUseCase = "ownedUseCase";
      _FunctionBehavior.powertypeExtent = "powertypeExtent";
      _FunctionBehavior.redefinedClassifier = "redefinedClassifier";
      _FunctionBehavior.representation = "representation";
      _FunctionBehavior.substitution = "substitution";
      _FunctionBehavior.templateParameter = "templateParameter";
      _FunctionBehavior.useCase = "useCase";
      _FunctionBehavior.elementImport = "elementImport";
      _FunctionBehavior.importedMember = "importedMember";
      _FunctionBehavior.member = "member";
      _FunctionBehavior.ownedMember = "ownedMember";
      _FunctionBehavior.ownedRule = "ownedRule";
      _FunctionBehavior.packageImport = "packageImport";
      _FunctionBehavior.clientDependency = "clientDependency";
      _FunctionBehavior._name_ = "name";
      _FunctionBehavior.nameExpression = "nameExpression";
      _FunctionBehavior.namespace = "namespace";
      _FunctionBehavior.qualifiedName = "qualifiedName";
      _FunctionBehavior.visibility = "visibility";
      _FunctionBehavior.ownedComment = "ownedComment";
      _FunctionBehavior.ownedElement = "ownedElement";
      _FunctionBehavior.owner = "owner";
      _FunctionBehavior._package_ = "package";
      _FunctionBehavior.owningTemplateParameter = "owningTemplateParameter";
      _FunctionBehavior.templateBinding = "templateBinding";
      _FunctionBehavior.isLeaf = "isLeaf";
      _FunctionBehavior.redefinedElement = "redefinedElement";
      _FunctionBehavior.redefinitionContext = "redefinitionContext";
      _FunctionBehavior.ownedPort = "ownedPort";
      _FunctionBehavior.ownedConnector = "ownedConnector";
      _FunctionBehavior.part = "part";
      _FunctionBehavior.role = "role";
      _CommonBehavior2._FunctionBehavior = _FunctionBehavior;
      _CommonBehavior2.__FunctionBehavior_Uri = "dm:///_internal/model/uml#FunctionBehavior";
      class _MessageEvent {
      }
      _MessageEvent.visibility = "visibility";
      _MessageEvent.owningTemplateParameter = "owningTemplateParameter";
      _MessageEvent.templateParameter = "templateParameter";
      _MessageEvent.ownedComment = "ownedComment";
      _MessageEvent.ownedElement = "ownedElement";
      _MessageEvent.owner = "owner";
      _MessageEvent.clientDependency = "clientDependency";
      _MessageEvent._name_ = "name";
      _MessageEvent.nameExpression = "nameExpression";
      _MessageEvent.namespace = "namespace";
      _MessageEvent.qualifiedName = "qualifiedName";
      _CommonBehavior2._MessageEvent = _MessageEvent;
      _CommonBehavior2.__MessageEvent_Uri = "dm:///_internal/model/uml#MessageEvent";
      class _OpaqueBehavior {
      }
      _OpaqueBehavior.body = "body";
      _OpaqueBehavior.language = "language";
      _OpaqueBehavior.context = "context";
      _OpaqueBehavior.isReentrant = "isReentrant";
      _OpaqueBehavior.ownedParameter = "ownedParameter";
      _OpaqueBehavior.ownedParameterSet = "ownedParameterSet";
      _OpaqueBehavior.postcondition = "postcondition";
      _OpaqueBehavior.precondition = "precondition";
      _OpaqueBehavior.specification = "specification";
      _OpaqueBehavior.redefinedBehavior = "redefinedBehavior";
      _OpaqueBehavior.extension = "extension";
      _OpaqueBehavior.isAbstract = "isAbstract";
      _OpaqueBehavior.isActive = "isActive";
      _OpaqueBehavior.nestedClassifier = "nestedClassifier";
      _OpaqueBehavior.ownedAttribute = "ownedAttribute";
      _OpaqueBehavior.ownedOperation = "ownedOperation";
      _OpaqueBehavior.ownedReception = "ownedReception";
      _OpaqueBehavior.superClass = "superClass";
      _OpaqueBehavior.classifierBehavior = "classifierBehavior";
      _OpaqueBehavior.interfaceRealization = "interfaceRealization";
      _OpaqueBehavior.ownedBehavior = "ownedBehavior";
      _OpaqueBehavior.attribute = "attribute";
      _OpaqueBehavior.collaborationUse = "collaborationUse";
      _OpaqueBehavior.feature = "feature";
      _OpaqueBehavior.general = "general";
      _OpaqueBehavior.generalization = "generalization";
      _OpaqueBehavior.inheritedMember = "inheritedMember";
      _OpaqueBehavior.isFinalSpecialization = "isFinalSpecialization";
      _OpaqueBehavior.ownedTemplateSignature = "ownedTemplateSignature";
      _OpaqueBehavior.ownedUseCase = "ownedUseCase";
      _OpaqueBehavior.powertypeExtent = "powertypeExtent";
      _OpaqueBehavior.redefinedClassifier = "redefinedClassifier";
      _OpaqueBehavior.representation = "representation";
      _OpaqueBehavior.substitution = "substitution";
      _OpaqueBehavior.templateParameter = "templateParameter";
      _OpaqueBehavior.useCase = "useCase";
      _OpaqueBehavior.elementImport = "elementImport";
      _OpaqueBehavior.importedMember = "importedMember";
      _OpaqueBehavior.member = "member";
      _OpaqueBehavior.ownedMember = "ownedMember";
      _OpaqueBehavior.ownedRule = "ownedRule";
      _OpaqueBehavior.packageImport = "packageImport";
      _OpaqueBehavior.clientDependency = "clientDependency";
      _OpaqueBehavior._name_ = "name";
      _OpaqueBehavior.nameExpression = "nameExpression";
      _OpaqueBehavior.namespace = "namespace";
      _OpaqueBehavior.qualifiedName = "qualifiedName";
      _OpaqueBehavior.visibility = "visibility";
      _OpaqueBehavior.ownedComment = "ownedComment";
      _OpaqueBehavior.ownedElement = "ownedElement";
      _OpaqueBehavior.owner = "owner";
      _OpaqueBehavior._package_ = "package";
      _OpaqueBehavior.owningTemplateParameter = "owningTemplateParameter";
      _OpaqueBehavior.templateBinding = "templateBinding";
      _OpaqueBehavior.isLeaf = "isLeaf";
      _OpaqueBehavior.redefinedElement = "redefinedElement";
      _OpaqueBehavior.redefinitionContext = "redefinitionContext";
      _OpaqueBehavior.ownedPort = "ownedPort";
      _OpaqueBehavior.ownedConnector = "ownedConnector";
      _OpaqueBehavior.part = "part";
      _OpaqueBehavior.role = "role";
      _CommonBehavior2._OpaqueBehavior = _OpaqueBehavior;
      _CommonBehavior2.__OpaqueBehavior_Uri = "dm:///_internal/model/uml#OpaqueBehavior";
      class _SignalEvent {
      }
      _SignalEvent.signal = "signal";
      _SignalEvent.visibility = "visibility";
      _SignalEvent.owningTemplateParameter = "owningTemplateParameter";
      _SignalEvent.templateParameter = "templateParameter";
      _SignalEvent.ownedComment = "ownedComment";
      _SignalEvent.ownedElement = "ownedElement";
      _SignalEvent.owner = "owner";
      _SignalEvent.clientDependency = "clientDependency";
      _SignalEvent._name_ = "name";
      _SignalEvent.nameExpression = "nameExpression";
      _SignalEvent.namespace = "namespace";
      _SignalEvent.qualifiedName = "qualifiedName";
      _CommonBehavior2._SignalEvent = _SignalEvent;
      _CommonBehavior2.__SignalEvent_Uri = "dm:///_internal/model/uml#SignalEvent";
      class _TimeEvent {
      }
      _TimeEvent.isRelative = "isRelative";
      _TimeEvent.when = "when";
      _TimeEvent.visibility = "visibility";
      _TimeEvent.owningTemplateParameter = "owningTemplateParameter";
      _TimeEvent.templateParameter = "templateParameter";
      _TimeEvent.ownedComment = "ownedComment";
      _TimeEvent.ownedElement = "ownedElement";
      _TimeEvent.owner = "owner";
      _TimeEvent.clientDependency = "clientDependency";
      _TimeEvent._name_ = "name";
      _TimeEvent.nameExpression = "nameExpression";
      _TimeEvent.namespace = "namespace";
      _TimeEvent.qualifiedName = "qualifiedName";
      _CommonBehavior2._TimeEvent = _TimeEvent;
      _CommonBehavior2.__TimeEvent_Uri = "dm:///_internal/model/uml#TimeEvent";
      class _Trigger {
      }
      _Trigger.event = "event";
      _Trigger.port = "port";
      _Trigger.clientDependency = "clientDependency";
      _Trigger._name_ = "name";
      _Trigger.nameExpression = "nameExpression";
      _Trigger.namespace = "namespace";
      _Trigger.qualifiedName = "qualifiedName";
      _Trigger.visibility = "visibility";
      _Trigger.ownedComment = "ownedComment";
      _Trigger.ownedElement = "ownedElement";
      _Trigger.owner = "owner";
      _CommonBehavior2._Trigger = _Trigger;
      _CommonBehavior2.__Trigger_Uri = "dm:///_internal/model/uml#Trigger";
    })(_CommonBehavior = _UML2._CommonBehavior || (_UML2._CommonBehavior = {}));
    let _Classification;
    (function(_Classification2) {
      class _Substitution {
      }
      _Substitution.contract = "contract";
      _Substitution.substitutingClassifier = "substitutingClassifier";
      _Substitution.mapping = "mapping";
      _Substitution.client = "client";
      _Substitution.supplier = "supplier";
      _Substitution.source = "source";
      _Substitution.target = "target";
      _Substitution.relatedElement = "relatedElement";
      _Substitution.ownedComment = "ownedComment";
      _Substitution.ownedElement = "ownedElement";
      _Substitution.owner = "owner";
      _Substitution.visibility = "visibility";
      _Substitution.owningTemplateParameter = "owningTemplateParameter";
      _Substitution.templateParameter = "templateParameter";
      _Substitution.clientDependency = "clientDependency";
      _Substitution._name_ = "name";
      _Substitution.nameExpression = "nameExpression";
      _Substitution.namespace = "namespace";
      _Substitution.qualifiedName = "qualifiedName";
      _Classification2._Substitution = _Substitution;
      _Classification2.__Substitution_Uri = "dm:///_internal/model/uml#Substitution";
      class _BehavioralFeature {
      }
      _BehavioralFeature.concurrency = "concurrency";
      _BehavioralFeature.isAbstract = "isAbstract";
      _BehavioralFeature.method = "method";
      _BehavioralFeature.ownedParameter = "ownedParameter";
      _BehavioralFeature.ownedParameterSet = "ownedParameterSet";
      _BehavioralFeature.raisedException = "raisedException";
      _BehavioralFeature.featuringClassifier = "featuringClassifier";
      _BehavioralFeature.isStatic = "isStatic";
      _BehavioralFeature.isLeaf = "isLeaf";
      _BehavioralFeature.redefinedElement = "redefinedElement";
      _BehavioralFeature.redefinitionContext = "redefinitionContext";
      _BehavioralFeature.clientDependency = "clientDependency";
      _BehavioralFeature._name_ = "name";
      _BehavioralFeature.nameExpression = "nameExpression";
      _BehavioralFeature.namespace = "namespace";
      _BehavioralFeature.qualifiedName = "qualifiedName";
      _BehavioralFeature.visibility = "visibility";
      _BehavioralFeature.ownedComment = "ownedComment";
      _BehavioralFeature.ownedElement = "ownedElement";
      _BehavioralFeature.owner = "owner";
      _BehavioralFeature.elementImport = "elementImport";
      _BehavioralFeature.importedMember = "importedMember";
      _BehavioralFeature.member = "member";
      _BehavioralFeature.ownedMember = "ownedMember";
      _BehavioralFeature.ownedRule = "ownedRule";
      _BehavioralFeature.packageImport = "packageImport";
      _Classification2._BehavioralFeature = _BehavioralFeature;
      _Classification2.__BehavioralFeature_Uri = "dm:///_internal/model/uml#BehavioralFeature";
      class _Classifier {
      }
      _Classifier.attribute = "attribute";
      _Classifier.collaborationUse = "collaborationUse";
      _Classifier.feature = "feature";
      _Classifier.general = "general";
      _Classifier.generalization = "generalization";
      _Classifier.inheritedMember = "inheritedMember";
      _Classifier.isAbstract = "isAbstract";
      _Classifier.isFinalSpecialization = "isFinalSpecialization";
      _Classifier.ownedTemplateSignature = "ownedTemplateSignature";
      _Classifier.ownedUseCase = "ownedUseCase";
      _Classifier.powertypeExtent = "powertypeExtent";
      _Classifier.redefinedClassifier = "redefinedClassifier";
      _Classifier.representation = "representation";
      _Classifier.substitution = "substitution";
      _Classifier.templateParameter = "templateParameter";
      _Classifier.useCase = "useCase";
      _Classifier.elementImport = "elementImport";
      _Classifier.importedMember = "importedMember";
      _Classifier.member = "member";
      _Classifier.ownedMember = "ownedMember";
      _Classifier.ownedRule = "ownedRule";
      _Classifier.packageImport = "packageImport";
      _Classifier.clientDependency = "clientDependency";
      _Classifier._name_ = "name";
      _Classifier.nameExpression = "nameExpression";
      _Classifier.namespace = "namespace";
      _Classifier.qualifiedName = "qualifiedName";
      _Classifier.visibility = "visibility";
      _Classifier.ownedComment = "ownedComment";
      _Classifier.ownedElement = "ownedElement";
      _Classifier.owner = "owner";
      _Classifier._package_ = "package";
      _Classifier.owningTemplateParameter = "owningTemplateParameter";
      _Classifier.templateBinding = "templateBinding";
      _Classifier.isLeaf = "isLeaf";
      _Classifier.redefinedElement = "redefinedElement";
      _Classifier.redefinitionContext = "redefinitionContext";
      _Classification2._Classifier = _Classifier;
      _Classification2.__Classifier_Uri = "dm:///_internal/model/uml#Classifier";
      class _ClassifierTemplateParameter {
      }
      _ClassifierTemplateParameter.allowSubstitutable = "allowSubstitutable";
      _ClassifierTemplateParameter.constrainingClassifier = "constrainingClassifier";
      _ClassifierTemplateParameter.parameteredElement = "parameteredElement";
      _ClassifierTemplateParameter._default_ = "default";
      _ClassifierTemplateParameter.ownedDefault = "ownedDefault";
      _ClassifierTemplateParameter.ownedParameteredElement = "ownedParameteredElement";
      _ClassifierTemplateParameter.signature = "signature";
      _ClassifierTemplateParameter.ownedComment = "ownedComment";
      _ClassifierTemplateParameter.ownedElement = "ownedElement";
      _ClassifierTemplateParameter.owner = "owner";
      _Classification2._ClassifierTemplateParameter = _ClassifierTemplateParameter;
      _Classification2.__ClassifierTemplateParameter_Uri = "dm:///_internal/model/uml#ClassifierTemplateParameter";
      class _Feature {
      }
      _Feature.featuringClassifier = "featuringClassifier";
      _Feature.isStatic = "isStatic";
      _Feature.isLeaf = "isLeaf";
      _Feature.redefinedElement = "redefinedElement";
      _Feature.redefinitionContext = "redefinitionContext";
      _Feature.clientDependency = "clientDependency";
      _Feature._name_ = "name";
      _Feature.nameExpression = "nameExpression";
      _Feature.namespace = "namespace";
      _Feature.qualifiedName = "qualifiedName";
      _Feature.visibility = "visibility";
      _Feature.ownedComment = "ownedComment";
      _Feature.ownedElement = "ownedElement";
      _Feature.owner = "owner";
      _Classification2._Feature = _Feature;
      _Classification2.__Feature_Uri = "dm:///_internal/model/uml#Feature";
      class _Generalization {
      }
      _Generalization.general = "general";
      _Generalization.generalizationSet = "generalizationSet";
      _Generalization.isSubstitutable = "isSubstitutable";
      _Generalization.specific = "specific";
      _Generalization.source = "source";
      _Generalization.target = "target";
      _Generalization.relatedElement = "relatedElement";
      _Generalization.ownedComment = "ownedComment";
      _Generalization.ownedElement = "ownedElement";
      _Generalization.owner = "owner";
      _Classification2._Generalization = _Generalization;
      _Classification2.__Generalization_Uri = "dm:///_internal/model/uml#Generalization";
      class _GeneralizationSet {
      }
      _GeneralizationSet.generalization = "generalization";
      _GeneralizationSet.isCovering = "isCovering";
      _GeneralizationSet.isDisjoint = "isDisjoint";
      _GeneralizationSet.powertype = "powertype";
      _GeneralizationSet.visibility = "visibility";
      _GeneralizationSet.owningTemplateParameter = "owningTemplateParameter";
      _GeneralizationSet.templateParameter = "templateParameter";
      _GeneralizationSet.ownedComment = "ownedComment";
      _GeneralizationSet.ownedElement = "ownedElement";
      _GeneralizationSet.owner = "owner";
      _GeneralizationSet.clientDependency = "clientDependency";
      _GeneralizationSet._name_ = "name";
      _GeneralizationSet.nameExpression = "nameExpression";
      _GeneralizationSet.namespace = "namespace";
      _GeneralizationSet.qualifiedName = "qualifiedName";
      _Classification2._GeneralizationSet = _GeneralizationSet;
      _Classification2.__GeneralizationSet_Uri = "dm:///_internal/model/uml#GeneralizationSet";
      class _InstanceSpecification {
      }
      _InstanceSpecification.classifier = "classifier";
      _InstanceSpecification.slot = "slot";
      _InstanceSpecification.specification = "specification";
      _InstanceSpecification.deployedElement = "deployedElement";
      _InstanceSpecification.deployment = "deployment";
      _InstanceSpecification.clientDependency = "clientDependency";
      _InstanceSpecification._name_ = "name";
      _InstanceSpecification.nameExpression = "nameExpression";
      _InstanceSpecification.namespace = "namespace";
      _InstanceSpecification.qualifiedName = "qualifiedName";
      _InstanceSpecification.visibility = "visibility";
      _InstanceSpecification.ownedComment = "ownedComment";
      _InstanceSpecification.ownedElement = "ownedElement";
      _InstanceSpecification.owner = "owner";
      _InstanceSpecification.owningTemplateParameter = "owningTemplateParameter";
      _InstanceSpecification.templateParameter = "templateParameter";
      _Classification2._InstanceSpecification = _InstanceSpecification;
      _Classification2.__InstanceSpecification_Uri = "dm:///_internal/model/uml#InstanceSpecification";
      class _InstanceValue {
      }
      _InstanceValue.instance = "instance";
      _InstanceValue.type = "type";
      _InstanceValue.clientDependency = "clientDependency";
      _InstanceValue._name_ = "name";
      _InstanceValue.nameExpression = "nameExpression";
      _InstanceValue.namespace = "namespace";
      _InstanceValue.qualifiedName = "qualifiedName";
      _InstanceValue.visibility = "visibility";
      _InstanceValue.ownedComment = "ownedComment";
      _InstanceValue.ownedElement = "ownedElement";
      _InstanceValue.owner = "owner";
      _InstanceValue.owningTemplateParameter = "owningTemplateParameter";
      _InstanceValue.templateParameter = "templateParameter";
      _Classification2._InstanceValue = _InstanceValue;
      _Classification2.__InstanceValue_Uri = "dm:///_internal/model/uml#InstanceValue";
      class _Operation {
      }
      _Operation.bodyCondition = "bodyCondition";
      _Operation._class_ = "class";
      _Operation.datatype = "datatype";
      _Operation._interface_ = "interface";
      _Operation.isOrdered = "isOrdered";
      _Operation.isQuery = "isQuery";
      _Operation.isUnique = "isUnique";
      _Operation.lower = "lower";
      _Operation.ownedParameter = "ownedParameter";
      _Operation.postcondition = "postcondition";
      _Operation.precondition = "precondition";
      _Operation.raisedException = "raisedException";
      _Operation.redefinedOperation = "redefinedOperation";
      _Operation.templateParameter = "templateParameter";
      _Operation.type = "type";
      _Operation.upper = "upper";
      _Operation.ownedTemplateSignature = "ownedTemplateSignature";
      _Operation.templateBinding = "templateBinding";
      _Operation.ownedComment = "ownedComment";
      _Operation.ownedElement = "ownedElement";
      _Operation.owner = "owner";
      _Operation.owningTemplateParameter = "owningTemplateParameter";
      _Operation.concurrency = "concurrency";
      _Operation.isAbstract = "isAbstract";
      _Operation.method = "method";
      _Operation.ownedParameterSet = "ownedParameterSet";
      _Operation.featuringClassifier = "featuringClassifier";
      _Operation.isStatic = "isStatic";
      _Operation.isLeaf = "isLeaf";
      _Operation.redefinedElement = "redefinedElement";
      _Operation.redefinitionContext = "redefinitionContext";
      _Operation.clientDependency = "clientDependency";
      _Operation._name_ = "name";
      _Operation.nameExpression = "nameExpression";
      _Operation.namespace = "namespace";
      _Operation.qualifiedName = "qualifiedName";
      _Operation.visibility = "visibility";
      _Operation.elementImport = "elementImport";
      _Operation.importedMember = "importedMember";
      _Operation.member = "member";
      _Operation.ownedMember = "ownedMember";
      _Operation.ownedRule = "ownedRule";
      _Operation.packageImport = "packageImport";
      _Classification2._Operation = _Operation;
      _Classification2.__Operation_Uri = "dm:///_internal/model/uml#Operation";
      class _OperationTemplateParameter {
      }
      _OperationTemplateParameter.parameteredElement = "parameteredElement";
      _OperationTemplateParameter._default_ = "default";
      _OperationTemplateParameter.ownedDefault = "ownedDefault";
      _OperationTemplateParameter.ownedParameteredElement = "ownedParameteredElement";
      _OperationTemplateParameter.signature = "signature";
      _OperationTemplateParameter.ownedComment = "ownedComment";
      _OperationTemplateParameter.ownedElement = "ownedElement";
      _OperationTemplateParameter.owner = "owner";
      _Classification2._OperationTemplateParameter = _OperationTemplateParameter;
      _Classification2.__OperationTemplateParameter_Uri = "dm:///_internal/model/uml#OperationTemplateParameter";
      class _Parameter {
      }
      _Parameter._default_ = "default";
      _Parameter.defaultValue = "defaultValue";
      _Parameter.direction = "direction";
      _Parameter.effect = "effect";
      _Parameter.isException = "isException";
      _Parameter.isStream = "isStream";
      _Parameter.operation = "operation";
      _Parameter.parameterSet = "parameterSet";
      _Parameter.isOrdered = "isOrdered";
      _Parameter.isUnique = "isUnique";
      _Parameter.lower = "lower";
      _Parameter.lowerValue = "lowerValue";
      _Parameter.upper = "upper";
      _Parameter.upperValue = "upperValue";
      _Parameter.ownedComment = "ownedComment";
      _Parameter.ownedElement = "ownedElement";
      _Parameter.owner = "owner";
      _Parameter.end = "end";
      _Parameter.templateParameter = "templateParameter";
      _Parameter.type = "type";
      _Parameter.clientDependency = "clientDependency";
      _Parameter._name_ = "name";
      _Parameter.nameExpression = "nameExpression";
      _Parameter.namespace = "namespace";
      _Parameter.qualifiedName = "qualifiedName";
      _Parameter.visibility = "visibility";
      _Parameter.owningTemplateParameter = "owningTemplateParameter";
      _Classification2._Parameter = _Parameter;
      _Classification2.__Parameter_Uri = "dm:///_internal/model/uml#Parameter";
      class _ParameterSet {
      }
      _ParameterSet.condition = "condition";
      _ParameterSet.parameter = "parameter";
      _ParameterSet.clientDependency = "clientDependency";
      _ParameterSet._name_ = "name";
      _ParameterSet.nameExpression = "nameExpression";
      _ParameterSet.namespace = "namespace";
      _ParameterSet.qualifiedName = "qualifiedName";
      _ParameterSet.visibility = "visibility";
      _ParameterSet.ownedComment = "ownedComment";
      _ParameterSet.ownedElement = "ownedElement";
      _ParameterSet.owner = "owner";
      _Classification2._ParameterSet = _ParameterSet;
      _Classification2.__ParameterSet_Uri = "dm:///_internal/model/uml#ParameterSet";
      class _Property {
      }
      _Property.aggregation = "aggregation";
      _Property.association = "association";
      _Property.associationEnd = "associationEnd";
      _Property._class_ = "class";
      _Property.datatype = "datatype";
      _Property.defaultValue = "defaultValue";
      _Property._interface_ = "interface";
      _Property.isComposite = "isComposite";
      _Property.isDerived = "isDerived";
      _Property.isDerivedUnion = "isDerivedUnion";
      _Property.isID = "isID";
      _Property.opposite = "opposite";
      _Property.owningAssociation = "owningAssociation";
      _Property.qualifier = "qualifier";
      _Property.redefinedProperty = "redefinedProperty";
      _Property.subsettedProperty = "subsettedProperty";
      _Property.end = "end";
      _Property.templateParameter = "templateParameter";
      _Property.type = "type";
      _Property.clientDependency = "clientDependency";
      _Property._name_ = "name";
      _Property.nameExpression = "nameExpression";
      _Property.namespace = "namespace";
      _Property.qualifiedName = "qualifiedName";
      _Property.visibility = "visibility";
      _Property.ownedComment = "ownedComment";
      _Property.ownedElement = "ownedElement";
      _Property.owner = "owner";
      _Property.owningTemplateParameter = "owningTemplateParameter";
      _Property.deployedElement = "deployedElement";
      _Property.deployment = "deployment";
      _Property.isReadOnly = "isReadOnly";
      _Property.isOrdered = "isOrdered";
      _Property.isUnique = "isUnique";
      _Property.lower = "lower";
      _Property.lowerValue = "lowerValue";
      _Property.upper = "upper";
      _Property.upperValue = "upperValue";
      _Property.featuringClassifier = "featuringClassifier";
      _Property.isStatic = "isStatic";
      _Property.isLeaf = "isLeaf";
      _Property.redefinedElement = "redefinedElement";
      _Property.redefinitionContext = "redefinitionContext";
      _Classification2._Property = _Property;
      _Classification2.__Property_Uri = "dm:///_internal/model/uml#Property";
      class _RedefinableElement {
      }
      _RedefinableElement.isLeaf = "isLeaf";
      _RedefinableElement.redefinedElement = "redefinedElement";
      _RedefinableElement.redefinitionContext = "redefinitionContext";
      _RedefinableElement.clientDependency = "clientDependency";
      _RedefinableElement._name_ = "name";
      _RedefinableElement.nameExpression = "nameExpression";
      _RedefinableElement.namespace = "namespace";
      _RedefinableElement.qualifiedName = "qualifiedName";
      _RedefinableElement.visibility = "visibility";
      _RedefinableElement.ownedComment = "ownedComment";
      _RedefinableElement.ownedElement = "ownedElement";
      _RedefinableElement.owner = "owner";
      _Classification2._RedefinableElement = _RedefinableElement;
      _Classification2.__RedefinableElement_Uri = "dm:///_internal/model/uml#RedefinableElement";
      class _RedefinableTemplateSignature {
      }
      _RedefinableTemplateSignature.classifier = "classifier";
      _RedefinableTemplateSignature.extendedSignature = "extendedSignature";
      _RedefinableTemplateSignature.inheritedParameter = "inheritedParameter";
      _RedefinableTemplateSignature.isLeaf = "isLeaf";
      _RedefinableTemplateSignature.redefinedElement = "redefinedElement";
      _RedefinableTemplateSignature.redefinitionContext = "redefinitionContext";
      _RedefinableTemplateSignature.clientDependency = "clientDependency";
      _RedefinableTemplateSignature._name_ = "name";
      _RedefinableTemplateSignature.nameExpression = "nameExpression";
      _RedefinableTemplateSignature.namespace = "namespace";
      _RedefinableTemplateSignature.qualifiedName = "qualifiedName";
      _RedefinableTemplateSignature.visibility = "visibility";
      _RedefinableTemplateSignature.ownedComment = "ownedComment";
      _RedefinableTemplateSignature.ownedElement = "ownedElement";
      _RedefinableTemplateSignature.owner = "owner";
      _RedefinableTemplateSignature.ownedParameter = "ownedParameter";
      _RedefinableTemplateSignature.parameter = "parameter";
      _RedefinableTemplateSignature.template = "template";
      _Classification2._RedefinableTemplateSignature = _RedefinableTemplateSignature;
      _Classification2.__RedefinableTemplateSignature_Uri = "dm:///_internal/model/uml#RedefinableTemplateSignature";
      class _Slot {
      }
      _Slot.definingFeature = "definingFeature";
      _Slot.owningInstance = "owningInstance";
      _Slot.value = "value";
      _Slot.ownedComment = "ownedComment";
      _Slot.ownedElement = "ownedElement";
      _Slot.owner = "owner";
      _Classification2._Slot = _Slot;
      _Classification2.__Slot_Uri = "dm:///_internal/model/uml#Slot";
      class _StructuralFeature {
      }
      _StructuralFeature.isReadOnly = "isReadOnly";
      _StructuralFeature.isOrdered = "isOrdered";
      _StructuralFeature.isUnique = "isUnique";
      _StructuralFeature.lower = "lower";
      _StructuralFeature.lowerValue = "lowerValue";
      _StructuralFeature.upper = "upper";
      _StructuralFeature.upperValue = "upperValue";
      _StructuralFeature.ownedComment = "ownedComment";
      _StructuralFeature.ownedElement = "ownedElement";
      _StructuralFeature.owner = "owner";
      _StructuralFeature.type = "type";
      _StructuralFeature.clientDependency = "clientDependency";
      _StructuralFeature._name_ = "name";
      _StructuralFeature.nameExpression = "nameExpression";
      _StructuralFeature.namespace = "namespace";
      _StructuralFeature.qualifiedName = "qualifiedName";
      _StructuralFeature.visibility = "visibility";
      _StructuralFeature.featuringClassifier = "featuringClassifier";
      _StructuralFeature.isStatic = "isStatic";
      _StructuralFeature.isLeaf = "isLeaf";
      _StructuralFeature.redefinedElement = "redefinedElement";
      _StructuralFeature.redefinitionContext = "redefinitionContext";
      _Classification2._StructuralFeature = _StructuralFeature;
      _Classification2.__StructuralFeature_Uri = "dm:///_internal/model/uml#StructuralFeature";
      let _AggregationKind;
      (function(_AggregationKind2) {
        _AggregationKind2.none = "none";
        _AggregationKind2.shared = "shared";
        _AggregationKind2.composite = "composite";
      })(_AggregationKind = _Classification2._AggregationKind || (_Classification2._AggregationKind = {}));
      let ___AggregationKind;
      (function(___AggregationKind2) {
        ___AggregationKind2[___AggregationKind2["none"] = 0] = "none";
        ___AggregationKind2[___AggregationKind2["shared"] = 1] = "shared";
        ___AggregationKind2[___AggregationKind2["composite"] = 2] = "composite";
      })(___AggregationKind = _Classification2.___AggregationKind || (_Classification2.___AggregationKind = {}));
      let _CallConcurrencyKind;
      (function(_CallConcurrencyKind2) {
        _CallConcurrencyKind2.sequential = "sequential";
        _CallConcurrencyKind2.guarded = "guarded";
        _CallConcurrencyKind2.concurrent = "concurrent";
      })(_CallConcurrencyKind = _Classification2._CallConcurrencyKind || (_Classification2._CallConcurrencyKind = {}));
      let ___CallConcurrencyKind;
      (function(___CallConcurrencyKind2) {
        ___CallConcurrencyKind2[___CallConcurrencyKind2["sequential"] = 0] = "sequential";
        ___CallConcurrencyKind2[___CallConcurrencyKind2["guarded"] = 1] = "guarded";
        ___CallConcurrencyKind2[___CallConcurrencyKind2["concurrent"] = 2] = "concurrent";
      })(___CallConcurrencyKind = _Classification2.___CallConcurrencyKind || (_Classification2.___CallConcurrencyKind = {}));
      let _ParameterDirectionKind;
      (function(_ParameterDirectionKind2) {
        _ParameterDirectionKind2._in_ = "in";
        _ParameterDirectionKind2.inout = "inout";
        _ParameterDirectionKind2.out = "out";
        _ParameterDirectionKind2._return_ = "return";
      })(_ParameterDirectionKind = _Classification2._ParameterDirectionKind || (_Classification2._ParameterDirectionKind = {}));
      let ___ParameterDirectionKind;
      (function(___ParameterDirectionKind2) {
        ___ParameterDirectionKind2[___ParameterDirectionKind2["_in_"] = 0] = "_in_";
        ___ParameterDirectionKind2[___ParameterDirectionKind2["inout"] = 1] = "inout";
        ___ParameterDirectionKind2[___ParameterDirectionKind2["out"] = 2] = "out";
        ___ParameterDirectionKind2[___ParameterDirectionKind2["_return_"] = 3] = "_return_";
      })(___ParameterDirectionKind = _Classification2.___ParameterDirectionKind || (_Classification2.___ParameterDirectionKind = {}));
      let _ParameterEffectKind;
      (function(_ParameterEffectKind2) {
        _ParameterEffectKind2.create = "create";
        _ParameterEffectKind2.read = "read";
        _ParameterEffectKind2.update = "update";
        _ParameterEffectKind2._delete_ = "delete";
      })(_ParameterEffectKind = _Classification2._ParameterEffectKind || (_Classification2._ParameterEffectKind = {}));
      let ___ParameterEffectKind;
      (function(___ParameterEffectKind2) {
        ___ParameterEffectKind2[___ParameterEffectKind2["create"] = 0] = "create";
        ___ParameterEffectKind2[___ParameterEffectKind2["read"] = 1] = "read";
        ___ParameterEffectKind2[___ParameterEffectKind2["update"] = 2] = "update";
        ___ParameterEffectKind2[___ParameterEffectKind2["_delete_"] = 3] = "_delete_";
      })(___ParameterEffectKind = _Classification2.___ParameterEffectKind || (_Classification2.___ParameterEffectKind = {}));
    })(_Classification = _UML2._Classification || (_UML2._Classification = {}));
    let _Actions2;
    (function(_Actions3) {
      class _ValueSpecificationAction {
      }
      _ValueSpecificationAction.result = "result";
      _ValueSpecificationAction.value = "value";
      _ValueSpecificationAction.context = "context";
      _ValueSpecificationAction.input = "input";
      _ValueSpecificationAction.isLocallyReentrant = "isLocallyReentrant";
      _ValueSpecificationAction.localPostcondition = "localPostcondition";
      _ValueSpecificationAction.localPrecondition = "localPrecondition";
      _ValueSpecificationAction.output = "output";
      _ValueSpecificationAction.handler = "handler";
      _ValueSpecificationAction.activity = "activity";
      _ValueSpecificationAction.inGroup = "inGroup";
      _ValueSpecificationAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ValueSpecificationAction.inPartition = "inPartition";
      _ValueSpecificationAction.inStructuredNode = "inStructuredNode";
      _ValueSpecificationAction.incoming = "incoming";
      _ValueSpecificationAction.outgoing = "outgoing";
      _ValueSpecificationAction.redefinedNode = "redefinedNode";
      _ValueSpecificationAction.isLeaf = "isLeaf";
      _ValueSpecificationAction.redefinedElement = "redefinedElement";
      _ValueSpecificationAction.redefinitionContext = "redefinitionContext";
      _ValueSpecificationAction.clientDependency = "clientDependency";
      _ValueSpecificationAction._name_ = "name";
      _ValueSpecificationAction.nameExpression = "nameExpression";
      _ValueSpecificationAction.namespace = "namespace";
      _ValueSpecificationAction.qualifiedName = "qualifiedName";
      _ValueSpecificationAction.visibility = "visibility";
      _ValueSpecificationAction.ownedComment = "ownedComment";
      _ValueSpecificationAction.ownedElement = "ownedElement";
      _ValueSpecificationAction.owner = "owner";
      _Actions3._ValueSpecificationAction = _ValueSpecificationAction;
      _Actions3.__ValueSpecificationAction_Uri = "dm:///_internal/model/uml#ValueSpecificationAction";
      class _VariableAction {
      }
      _VariableAction.variable = "variable";
      _VariableAction.context = "context";
      _VariableAction.input = "input";
      _VariableAction.isLocallyReentrant = "isLocallyReentrant";
      _VariableAction.localPostcondition = "localPostcondition";
      _VariableAction.localPrecondition = "localPrecondition";
      _VariableAction.output = "output";
      _VariableAction.handler = "handler";
      _VariableAction.activity = "activity";
      _VariableAction.inGroup = "inGroup";
      _VariableAction.inInterruptibleRegion = "inInterruptibleRegion";
      _VariableAction.inPartition = "inPartition";
      _VariableAction.inStructuredNode = "inStructuredNode";
      _VariableAction.incoming = "incoming";
      _VariableAction.outgoing = "outgoing";
      _VariableAction.redefinedNode = "redefinedNode";
      _VariableAction.isLeaf = "isLeaf";
      _VariableAction.redefinedElement = "redefinedElement";
      _VariableAction.redefinitionContext = "redefinitionContext";
      _VariableAction.clientDependency = "clientDependency";
      _VariableAction._name_ = "name";
      _VariableAction.nameExpression = "nameExpression";
      _VariableAction.namespace = "namespace";
      _VariableAction.qualifiedName = "qualifiedName";
      _VariableAction.visibility = "visibility";
      _VariableAction.ownedComment = "ownedComment";
      _VariableAction.ownedElement = "ownedElement";
      _VariableAction.owner = "owner";
      _Actions3._VariableAction = _VariableAction;
      _Actions3.__VariableAction_Uri = "dm:///_internal/model/uml#VariableAction";
      class _WriteLinkAction {
      }
      _WriteLinkAction.endData = "endData";
      _WriteLinkAction.inputValue = "inputValue";
      _WriteLinkAction.context = "context";
      _WriteLinkAction.input = "input";
      _WriteLinkAction.isLocallyReentrant = "isLocallyReentrant";
      _WriteLinkAction.localPostcondition = "localPostcondition";
      _WriteLinkAction.localPrecondition = "localPrecondition";
      _WriteLinkAction.output = "output";
      _WriteLinkAction.handler = "handler";
      _WriteLinkAction.activity = "activity";
      _WriteLinkAction.inGroup = "inGroup";
      _WriteLinkAction.inInterruptibleRegion = "inInterruptibleRegion";
      _WriteLinkAction.inPartition = "inPartition";
      _WriteLinkAction.inStructuredNode = "inStructuredNode";
      _WriteLinkAction.incoming = "incoming";
      _WriteLinkAction.outgoing = "outgoing";
      _WriteLinkAction.redefinedNode = "redefinedNode";
      _WriteLinkAction.isLeaf = "isLeaf";
      _WriteLinkAction.redefinedElement = "redefinedElement";
      _WriteLinkAction.redefinitionContext = "redefinitionContext";
      _WriteLinkAction.clientDependency = "clientDependency";
      _WriteLinkAction._name_ = "name";
      _WriteLinkAction.nameExpression = "nameExpression";
      _WriteLinkAction.namespace = "namespace";
      _WriteLinkAction.qualifiedName = "qualifiedName";
      _WriteLinkAction.visibility = "visibility";
      _WriteLinkAction.ownedComment = "ownedComment";
      _WriteLinkAction.ownedElement = "ownedElement";
      _WriteLinkAction.owner = "owner";
      _Actions3._WriteLinkAction = _WriteLinkAction;
      _Actions3.__WriteLinkAction_Uri = "dm:///_internal/model/uml#WriteLinkAction";
      class _WriteStructuralFeatureAction {
      }
      _WriteStructuralFeatureAction.result = "result";
      _WriteStructuralFeatureAction.value = "value";
      _WriteStructuralFeatureAction.object = "object";
      _WriteStructuralFeatureAction.structuralFeature = "structuralFeature";
      _WriteStructuralFeatureAction.context = "context";
      _WriteStructuralFeatureAction.input = "input";
      _WriteStructuralFeatureAction.isLocallyReentrant = "isLocallyReentrant";
      _WriteStructuralFeatureAction.localPostcondition = "localPostcondition";
      _WriteStructuralFeatureAction.localPrecondition = "localPrecondition";
      _WriteStructuralFeatureAction.output = "output";
      _WriteStructuralFeatureAction.handler = "handler";
      _WriteStructuralFeatureAction.activity = "activity";
      _WriteStructuralFeatureAction.inGroup = "inGroup";
      _WriteStructuralFeatureAction.inInterruptibleRegion = "inInterruptibleRegion";
      _WriteStructuralFeatureAction.inPartition = "inPartition";
      _WriteStructuralFeatureAction.inStructuredNode = "inStructuredNode";
      _WriteStructuralFeatureAction.incoming = "incoming";
      _WriteStructuralFeatureAction.outgoing = "outgoing";
      _WriteStructuralFeatureAction.redefinedNode = "redefinedNode";
      _WriteStructuralFeatureAction.isLeaf = "isLeaf";
      _WriteStructuralFeatureAction.redefinedElement = "redefinedElement";
      _WriteStructuralFeatureAction.redefinitionContext = "redefinitionContext";
      _WriteStructuralFeatureAction.clientDependency = "clientDependency";
      _WriteStructuralFeatureAction._name_ = "name";
      _WriteStructuralFeatureAction.nameExpression = "nameExpression";
      _WriteStructuralFeatureAction.namespace = "namespace";
      _WriteStructuralFeatureAction.qualifiedName = "qualifiedName";
      _WriteStructuralFeatureAction.visibility = "visibility";
      _WriteStructuralFeatureAction.ownedComment = "ownedComment";
      _WriteStructuralFeatureAction.ownedElement = "ownedElement";
      _WriteStructuralFeatureAction.owner = "owner";
      _Actions3._WriteStructuralFeatureAction = _WriteStructuralFeatureAction;
      _Actions3.__WriteStructuralFeatureAction_Uri = "dm:///_internal/model/uml#WriteStructuralFeatureAction";
      class _WriteVariableAction {
      }
      _WriteVariableAction.value = "value";
      _WriteVariableAction.variable = "variable";
      _WriteVariableAction.context = "context";
      _WriteVariableAction.input = "input";
      _WriteVariableAction.isLocallyReentrant = "isLocallyReentrant";
      _WriteVariableAction.localPostcondition = "localPostcondition";
      _WriteVariableAction.localPrecondition = "localPrecondition";
      _WriteVariableAction.output = "output";
      _WriteVariableAction.handler = "handler";
      _WriteVariableAction.activity = "activity";
      _WriteVariableAction.inGroup = "inGroup";
      _WriteVariableAction.inInterruptibleRegion = "inInterruptibleRegion";
      _WriteVariableAction.inPartition = "inPartition";
      _WriteVariableAction.inStructuredNode = "inStructuredNode";
      _WriteVariableAction.incoming = "incoming";
      _WriteVariableAction.outgoing = "outgoing";
      _WriteVariableAction.redefinedNode = "redefinedNode";
      _WriteVariableAction.isLeaf = "isLeaf";
      _WriteVariableAction.redefinedElement = "redefinedElement";
      _WriteVariableAction.redefinitionContext = "redefinitionContext";
      _WriteVariableAction.clientDependency = "clientDependency";
      _WriteVariableAction._name_ = "name";
      _WriteVariableAction.nameExpression = "nameExpression";
      _WriteVariableAction.namespace = "namespace";
      _WriteVariableAction.qualifiedName = "qualifiedName";
      _WriteVariableAction.visibility = "visibility";
      _WriteVariableAction.ownedComment = "ownedComment";
      _WriteVariableAction.ownedElement = "ownedElement";
      _WriteVariableAction.owner = "owner";
      _Actions3._WriteVariableAction = _WriteVariableAction;
      _Actions3.__WriteVariableAction_Uri = "dm:///_internal/model/uml#WriteVariableAction";
      let _ExpansionKind;
      (function(_ExpansionKind2) {
        _ExpansionKind2.parallel = "parallel";
        _ExpansionKind2.iterative = "iterative";
        _ExpansionKind2.stream = "stream";
      })(_ExpansionKind = _Actions3._ExpansionKind || (_Actions3._ExpansionKind = {}));
      let ___ExpansionKind;
      (function(___ExpansionKind2) {
        ___ExpansionKind2[___ExpansionKind2["parallel"] = 0] = "parallel";
        ___ExpansionKind2[___ExpansionKind2["iterative"] = 1] = "iterative";
        ___ExpansionKind2[___ExpansionKind2["stream"] = 2] = "stream";
      })(___ExpansionKind = _Actions3.___ExpansionKind || (_Actions3.___ExpansionKind = {}));
      class _AcceptCallAction {
      }
      _AcceptCallAction.returnInformation = "returnInformation";
      _AcceptCallAction.isUnmarshall = "isUnmarshall";
      _AcceptCallAction.result = "result";
      _AcceptCallAction.trigger = "trigger";
      _AcceptCallAction.context = "context";
      _AcceptCallAction.input = "input";
      _AcceptCallAction.isLocallyReentrant = "isLocallyReentrant";
      _AcceptCallAction.localPostcondition = "localPostcondition";
      _AcceptCallAction.localPrecondition = "localPrecondition";
      _AcceptCallAction.output = "output";
      _AcceptCallAction.handler = "handler";
      _AcceptCallAction.activity = "activity";
      _AcceptCallAction.inGroup = "inGroup";
      _AcceptCallAction.inInterruptibleRegion = "inInterruptibleRegion";
      _AcceptCallAction.inPartition = "inPartition";
      _AcceptCallAction.inStructuredNode = "inStructuredNode";
      _AcceptCallAction.incoming = "incoming";
      _AcceptCallAction.outgoing = "outgoing";
      _AcceptCallAction.redefinedNode = "redefinedNode";
      _AcceptCallAction.isLeaf = "isLeaf";
      _AcceptCallAction.redefinedElement = "redefinedElement";
      _AcceptCallAction.redefinitionContext = "redefinitionContext";
      _AcceptCallAction.clientDependency = "clientDependency";
      _AcceptCallAction._name_ = "name";
      _AcceptCallAction.nameExpression = "nameExpression";
      _AcceptCallAction.namespace = "namespace";
      _AcceptCallAction.qualifiedName = "qualifiedName";
      _AcceptCallAction.visibility = "visibility";
      _AcceptCallAction.ownedComment = "ownedComment";
      _AcceptCallAction.ownedElement = "ownedElement";
      _AcceptCallAction.owner = "owner";
      _Actions3._AcceptCallAction = _AcceptCallAction;
      _Actions3.__AcceptCallAction_Uri = "dm:///_internal/model/uml#AcceptCallAction";
      class _AcceptEventAction {
      }
      _AcceptEventAction.isUnmarshall = "isUnmarshall";
      _AcceptEventAction.result = "result";
      _AcceptEventAction.trigger = "trigger";
      _AcceptEventAction.context = "context";
      _AcceptEventAction.input = "input";
      _AcceptEventAction.isLocallyReentrant = "isLocallyReentrant";
      _AcceptEventAction.localPostcondition = "localPostcondition";
      _AcceptEventAction.localPrecondition = "localPrecondition";
      _AcceptEventAction.output = "output";
      _AcceptEventAction.handler = "handler";
      _AcceptEventAction.activity = "activity";
      _AcceptEventAction.inGroup = "inGroup";
      _AcceptEventAction.inInterruptibleRegion = "inInterruptibleRegion";
      _AcceptEventAction.inPartition = "inPartition";
      _AcceptEventAction.inStructuredNode = "inStructuredNode";
      _AcceptEventAction.incoming = "incoming";
      _AcceptEventAction.outgoing = "outgoing";
      _AcceptEventAction.redefinedNode = "redefinedNode";
      _AcceptEventAction.isLeaf = "isLeaf";
      _AcceptEventAction.redefinedElement = "redefinedElement";
      _AcceptEventAction.redefinitionContext = "redefinitionContext";
      _AcceptEventAction.clientDependency = "clientDependency";
      _AcceptEventAction._name_ = "name";
      _AcceptEventAction.nameExpression = "nameExpression";
      _AcceptEventAction.namespace = "namespace";
      _AcceptEventAction.qualifiedName = "qualifiedName";
      _AcceptEventAction.visibility = "visibility";
      _AcceptEventAction.ownedComment = "ownedComment";
      _AcceptEventAction.ownedElement = "ownedElement";
      _AcceptEventAction.owner = "owner";
      _Actions3._AcceptEventAction = _AcceptEventAction;
      _Actions3.__AcceptEventAction_Uri = "dm:///_internal/model/uml#AcceptEventAction";
      class _Action {
      }
      _Action.context = "context";
      _Action.input = "input";
      _Action.isLocallyReentrant = "isLocallyReentrant";
      _Action.localPostcondition = "localPostcondition";
      _Action.localPrecondition = "localPrecondition";
      _Action.output = "output";
      _Action.handler = "handler";
      _Action.activity = "activity";
      _Action.inGroup = "inGroup";
      _Action.inInterruptibleRegion = "inInterruptibleRegion";
      _Action.inPartition = "inPartition";
      _Action.inStructuredNode = "inStructuredNode";
      _Action.incoming = "incoming";
      _Action.outgoing = "outgoing";
      _Action.redefinedNode = "redefinedNode";
      _Action.isLeaf = "isLeaf";
      _Action.redefinedElement = "redefinedElement";
      _Action.redefinitionContext = "redefinitionContext";
      _Action.clientDependency = "clientDependency";
      _Action._name_ = "name";
      _Action.nameExpression = "nameExpression";
      _Action.namespace = "namespace";
      _Action.qualifiedName = "qualifiedName";
      _Action.visibility = "visibility";
      _Action.ownedComment = "ownedComment";
      _Action.ownedElement = "ownedElement";
      _Action.owner = "owner";
      _Actions3._Action = _Action;
      _Actions3.__Action_Uri = "dm:///_internal/model/uml#Action";
      class _ActionInputPin {
      }
      _ActionInputPin.fromAction = "fromAction";
      _ActionInputPin.isControl = "isControl";
      _ActionInputPin.inState = "inState";
      _ActionInputPin.isControlType = "isControlType";
      _ActionInputPin.ordering = "ordering";
      _ActionInputPin.selection = "selection";
      _ActionInputPin.upperBound = "upperBound";
      _ActionInputPin.type = "type";
      _ActionInputPin.clientDependency = "clientDependency";
      _ActionInputPin._name_ = "name";
      _ActionInputPin.nameExpression = "nameExpression";
      _ActionInputPin.namespace = "namespace";
      _ActionInputPin.qualifiedName = "qualifiedName";
      _ActionInputPin.visibility = "visibility";
      _ActionInputPin.ownedComment = "ownedComment";
      _ActionInputPin.ownedElement = "ownedElement";
      _ActionInputPin.owner = "owner";
      _ActionInputPin.activity = "activity";
      _ActionInputPin.inGroup = "inGroup";
      _ActionInputPin.inInterruptibleRegion = "inInterruptibleRegion";
      _ActionInputPin.inPartition = "inPartition";
      _ActionInputPin.inStructuredNode = "inStructuredNode";
      _ActionInputPin.incoming = "incoming";
      _ActionInputPin.outgoing = "outgoing";
      _ActionInputPin.redefinedNode = "redefinedNode";
      _ActionInputPin.isLeaf = "isLeaf";
      _ActionInputPin.redefinedElement = "redefinedElement";
      _ActionInputPin.redefinitionContext = "redefinitionContext";
      _ActionInputPin.isOrdered = "isOrdered";
      _ActionInputPin.isUnique = "isUnique";
      _ActionInputPin.lower = "lower";
      _ActionInputPin.lowerValue = "lowerValue";
      _ActionInputPin.upper = "upper";
      _ActionInputPin.upperValue = "upperValue";
      _Actions3._ActionInputPin = _ActionInputPin;
      _Actions3.__ActionInputPin_Uri = "dm:///_internal/model/uml#ActionInputPin";
      class _AddStructuralFeatureValueAction {
      }
      _AddStructuralFeatureValueAction.insertAt = "insertAt";
      _AddStructuralFeatureValueAction.isReplaceAll = "isReplaceAll";
      _AddStructuralFeatureValueAction.result = "result";
      _AddStructuralFeatureValueAction.value = "value";
      _AddStructuralFeatureValueAction.object = "object";
      _AddStructuralFeatureValueAction.structuralFeature = "structuralFeature";
      _AddStructuralFeatureValueAction.context = "context";
      _AddStructuralFeatureValueAction.input = "input";
      _AddStructuralFeatureValueAction.isLocallyReentrant = "isLocallyReentrant";
      _AddStructuralFeatureValueAction.localPostcondition = "localPostcondition";
      _AddStructuralFeatureValueAction.localPrecondition = "localPrecondition";
      _AddStructuralFeatureValueAction.output = "output";
      _AddStructuralFeatureValueAction.handler = "handler";
      _AddStructuralFeatureValueAction.activity = "activity";
      _AddStructuralFeatureValueAction.inGroup = "inGroup";
      _AddStructuralFeatureValueAction.inInterruptibleRegion = "inInterruptibleRegion";
      _AddStructuralFeatureValueAction.inPartition = "inPartition";
      _AddStructuralFeatureValueAction.inStructuredNode = "inStructuredNode";
      _AddStructuralFeatureValueAction.incoming = "incoming";
      _AddStructuralFeatureValueAction.outgoing = "outgoing";
      _AddStructuralFeatureValueAction.redefinedNode = "redefinedNode";
      _AddStructuralFeatureValueAction.isLeaf = "isLeaf";
      _AddStructuralFeatureValueAction.redefinedElement = "redefinedElement";
      _AddStructuralFeatureValueAction.redefinitionContext = "redefinitionContext";
      _AddStructuralFeatureValueAction.clientDependency = "clientDependency";
      _AddStructuralFeatureValueAction._name_ = "name";
      _AddStructuralFeatureValueAction.nameExpression = "nameExpression";
      _AddStructuralFeatureValueAction.namespace = "namespace";
      _AddStructuralFeatureValueAction.qualifiedName = "qualifiedName";
      _AddStructuralFeatureValueAction.visibility = "visibility";
      _AddStructuralFeatureValueAction.ownedComment = "ownedComment";
      _AddStructuralFeatureValueAction.ownedElement = "ownedElement";
      _AddStructuralFeatureValueAction.owner = "owner";
      _Actions3._AddStructuralFeatureValueAction = _AddStructuralFeatureValueAction;
      _Actions3.__AddStructuralFeatureValueAction_Uri = "dm:///_internal/model/uml#AddStructuralFeatureValueAction";
      class _AddVariableValueAction {
      }
      _AddVariableValueAction.insertAt = "insertAt";
      _AddVariableValueAction.isReplaceAll = "isReplaceAll";
      _AddVariableValueAction.value = "value";
      _AddVariableValueAction.variable = "variable";
      _AddVariableValueAction.context = "context";
      _AddVariableValueAction.input = "input";
      _AddVariableValueAction.isLocallyReentrant = "isLocallyReentrant";
      _AddVariableValueAction.localPostcondition = "localPostcondition";
      _AddVariableValueAction.localPrecondition = "localPrecondition";
      _AddVariableValueAction.output = "output";
      _AddVariableValueAction.handler = "handler";
      _AddVariableValueAction.activity = "activity";
      _AddVariableValueAction.inGroup = "inGroup";
      _AddVariableValueAction.inInterruptibleRegion = "inInterruptibleRegion";
      _AddVariableValueAction.inPartition = "inPartition";
      _AddVariableValueAction.inStructuredNode = "inStructuredNode";
      _AddVariableValueAction.incoming = "incoming";
      _AddVariableValueAction.outgoing = "outgoing";
      _AddVariableValueAction.redefinedNode = "redefinedNode";
      _AddVariableValueAction.isLeaf = "isLeaf";
      _AddVariableValueAction.redefinedElement = "redefinedElement";
      _AddVariableValueAction.redefinitionContext = "redefinitionContext";
      _AddVariableValueAction.clientDependency = "clientDependency";
      _AddVariableValueAction._name_ = "name";
      _AddVariableValueAction.nameExpression = "nameExpression";
      _AddVariableValueAction.namespace = "namespace";
      _AddVariableValueAction.qualifiedName = "qualifiedName";
      _AddVariableValueAction.visibility = "visibility";
      _AddVariableValueAction.ownedComment = "ownedComment";
      _AddVariableValueAction.ownedElement = "ownedElement";
      _AddVariableValueAction.owner = "owner";
      _Actions3._AddVariableValueAction = _AddVariableValueAction;
      _Actions3.__AddVariableValueAction_Uri = "dm:///_internal/model/uml#AddVariableValueAction";
      class _BroadcastSignalAction {
      }
      _BroadcastSignalAction.signal = "signal";
      _BroadcastSignalAction.argument = "argument";
      _BroadcastSignalAction.onPort = "onPort";
      _BroadcastSignalAction.context = "context";
      _BroadcastSignalAction.input = "input";
      _BroadcastSignalAction.isLocallyReentrant = "isLocallyReentrant";
      _BroadcastSignalAction.localPostcondition = "localPostcondition";
      _BroadcastSignalAction.localPrecondition = "localPrecondition";
      _BroadcastSignalAction.output = "output";
      _BroadcastSignalAction.handler = "handler";
      _BroadcastSignalAction.activity = "activity";
      _BroadcastSignalAction.inGroup = "inGroup";
      _BroadcastSignalAction.inInterruptibleRegion = "inInterruptibleRegion";
      _BroadcastSignalAction.inPartition = "inPartition";
      _BroadcastSignalAction.inStructuredNode = "inStructuredNode";
      _BroadcastSignalAction.incoming = "incoming";
      _BroadcastSignalAction.outgoing = "outgoing";
      _BroadcastSignalAction.redefinedNode = "redefinedNode";
      _BroadcastSignalAction.isLeaf = "isLeaf";
      _BroadcastSignalAction.redefinedElement = "redefinedElement";
      _BroadcastSignalAction.redefinitionContext = "redefinitionContext";
      _BroadcastSignalAction.clientDependency = "clientDependency";
      _BroadcastSignalAction._name_ = "name";
      _BroadcastSignalAction.nameExpression = "nameExpression";
      _BroadcastSignalAction.namespace = "namespace";
      _BroadcastSignalAction.qualifiedName = "qualifiedName";
      _BroadcastSignalAction.visibility = "visibility";
      _BroadcastSignalAction.ownedComment = "ownedComment";
      _BroadcastSignalAction.ownedElement = "ownedElement";
      _BroadcastSignalAction.owner = "owner";
      _Actions3._BroadcastSignalAction = _BroadcastSignalAction;
      _Actions3.__BroadcastSignalAction_Uri = "dm:///_internal/model/uml#BroadcastSignalAction";
      class _CallAction {
      }
      _CallAction.isSynchronous = "isSynchronous";
      _CallAction.result = "result";
      _CallAction.argument = "argument";
      _CallAction.onPort = "onPort";
      _CallAction.context = "context";
      _CallAction.input = "input";
      _CallAction.isLocallyReentrant = "isLocallyReentrant";
      _CallAction.localPostcondition = "localPostcondition";
      _CallAction.localPrecondition = "localPrecondition";
      _CallAction.output = "output";
      _CallAction.handler = "handler";
      _CallAction.activity = "activity";
      _CallAction.inGroup = "inGroup";
      _CallAction.inInterruptibleRegion = "inInterruptibleRegion";
      _CallAction.inPartition = "inPartition";
      _CallAction.inStructuredNode = "inStructuredNode";
      _CallAction.incoming = "incoming";
      _CallAction.outgoing = "outgoing";
      _CallAction.redefinedNode = "redefinedNode";
      _CallAction.isLeaf = "isLeaf";
      _CallAction.redefinedElement = "redefinedElement";
      _CallAction.redefinitionContext = "redefinitionContext";
      _CallAction.clientDependency = "clientDependency";
      _CallAction._name_ = "name";
      _CallAction.nameExpression = "nameExpression";
      _CallAction.namespace = "namespace";
      _CallAction.qualifiedName = "qualifiedName";
      _CallAction.visibility = "visibility";
      _CallAction.ownedComment = "ownedComment";
      _CallAction.ownedElement = "ownedElement";
      _CallAction.owner = "owner";
      _Actions3._CallAction = _CallAction;
      _Actions3.__CallAction_Uri = "dm:///_internal/model/uml#CallAction";
      class _CallBehaviorAction {
      }
      _CallBehaviorAction.behavior = "behavior";
      _CallBehaviorAction.isSynchronous = "isSynchronous";
      _CallBehaviorAction.result = "result";
      _CallBehaviorAction.argument = "argument";
      _CallBehaviorAction.onPort = "onPort";
      _CallBehaviorAction.context = "context";
      _CallBehaviorAction.input = "input";
      _CallBehaviorAction.isLocallyReentrant = "isLocallyReentrant";
      _CallBehaviorAction.localPostcondition = "localPostcondition";
      _CallBehaviorAction.localPrecondition = "localPrecondition";
      _CallBehaviorAction.output = "output";
      _CallBehaviorAction.handler = "handler";
      _CallBehaviorAction.activity = "activity";
      _CallBehaviorAction.inGroup = "inGroup";
      _CallBehaviorAction.inInterruptibleRegion = "inInterruptibleRegion";
      _CallBehaviorAction.inPartition = "inPartition";
      _CallBehaviorAction.inStructuredNode = "inStructuredNode";
      _CallBehaviorAction.incoming = "incoming";
      _CallBehaviorAction.outgoing = "outgoing";
      _CallBehaviorAction.redefinedNode = "redefinedNode";
      _CallBehaviorAction.isLeaf = "isLeaf";
      _CallBehaviorAction.redefinedElement = "redefinedElement";
      _CallBehaviorAction.redefinitionContext = "redefinitionContext";
      _CallBehaviorAction.clientDependency = "clientDependency";
      _CallBehaviorAction._name_ = "name";
      _CallBehaviorAction.nameExpression = "nameExpression";
      _CallBehaviorAction.namespace = "namespace";
      _CallBehaviorAction.qualifiedName = "qualifiedName";
      _CallBehaviorAction.visibility = "visibility";
      _CallBehaviorAction.ownedComment = "ownedComment";
      _CallBehaviorAction.ownedElement = "ownedElement";
      _CallBehaviorAction.owner = "owner";
      _Actions3._CallBehaviorAction = _CallBehaviorAction;
      _Actions3.__CallBehaviorAction_Uri = "dm:///_internal/model/uml#CallBehaviorAction";
      class _CallOperationAction {
      }
      _CallOperationAction.operation = "operation";
      _CallOperationAction.target = "target";
      _CallOperationAction.isSynchronous = "isSynchronous";
      _CallOperationAction.result = "result";
      _CallOperationAction.argument = "argument";
      _CallOperationAction.onPort = "onPort";
      _CallOperationAction.context = "context";
      _CallOperationAction.input = "input";
      _CallOperationAction.isLocallyReentrant = "isLocallyReentrant";
      _CallOperationAction.localPostcondition = "localPostcondition";
      _CallOperationAction.localPrecondition = "localPrecondition";
      _CallOperationAction.output = "output";
      _CallOperationAction.handler = "handler";
      _CallOperationAction.activity = "activity";
      _CallOperationAction.inGroup = "inGroup";
      _CallOperationAction.inInterruptibleRegion = "inInterruptibleRegion";
      _CallOperationAction.inPartition = "inPartition";
      _CallOperationAction.inStructuredNode = "inStructuredNode";
      _CallOperationAction.incoming = "incoming";
      _CallOperationAction.outgoing = "outgoing";
      _CallOperationAction.redefinedNode = "redefinedNode";
      _CallOperationAction.isLeaf = "isLeaf";
      _CallOperationAction.redefinedElement = "redefinedElement";
      _CallOperationAction.redefinitionContext = "redefinitionContext";
      _CallOperationAction.clientDependency = "clientDependency";
      _CallOperationAction._name_ = "name";
      _CallOperationAction.nameExpression = "nameExpression";
      _CallOperationAction.namespace = "namespace";
      _CallOperationAction.qualifiedName = "qualifiedName";
      _CallOperationAction.visibility = "visibility";
      _CallOperationAction.ownedComment = "ownedComment";
      _CallOperationAction.ownedElement = "ownedElement";
      _CallOperationAction.owner = "owner";
      _Actions3._CallOperationAction = _CallOperationAction;
      _Actions3.__CallOperationAction_Uri = "dm:///_internal/model/uml#CallOperationAction";
      class _Clause {
      }
      _Clause.body = "body";
      _Clause.bodyOutput = "bodyOutput";
      _Clause.decider = "decider";
      _Clause.predecessorClause = "predecessorClause";
      _Clause.successorClause = "successorClause";
      _Clause.test = "test";
      _Clause.ownedComment = "ownedComment";
      _Clause.ownedElement = "ownedElement";
      _Clause.owner = "owner";
      _Actions3._Clause = _Clause;
      _Actions3.__Clause_Uri = "dm:///_internal/model/uml#Clause";
      class _ClearAssociationAction {
      }
      _ClearAssociationAction.association = "association";
      _ClearAssociationAction.object = "object";
      _ClearAssociationAction.context = "context";
      _ClearAssociationAction.input = "input";
      _ClearAssociationAction.isLocallyReentrant = "isLocallyReentrant";
      _ClearAssociationAction.localPostcondition = "localPostcondition";
      _ClearAssociationAction.localPrecondition = "localPrecondition";
      _ClearAssociationAction.output = "output";
      _ClearAssociationAction.handler = "handler";
      _ClearAssociationAction.activity = "activity";
      _ClearAssociationAction.inGroup = "inGroup";
      _ClearAssociationAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ClearAssociationAction.inPartition = "inPartition";
      _ClearAssociationAction.inStructuredNode = "inStructuredNode";
      _ClearAssociationAction.incoming = "incoming";
      _ClearAssociationAction.outgoing = "outgoing";
      _ClearAssociationAction.redefinedNode = "redefinedNode";
      _ClearAssociationAction.isLeaf = "isLeaf";
      _ClearAssociationAction.redefinedElement = "redefinedElement";
      _ClearAssociationAction.redefinitionContext = "redefinitionContext";
      _ClearAssociationAction.clientDependency = "clientDependency";
      _ClearAssociationAction._name_ = "name";
      _ClearAssociationAction.nameExpression = "nameExpression";
      _ClearAssociationAction.namespace = "namespace";
      _ClearAssociationAction.qualifiedName = "qualifiedName";
      _ClearAssociationAction.visibility = "visibility";
      _ClearAssociationAction.ownedComment = "ownedComment";
      _ClearAssociationAction.ownedElement = "ownedElement";
      _ClearAssociationAction.owner = "owner";
      _Actions3._ClearAssociationAction = _ClearAssociationAction;
      _Actions3.__ClearAssociationAction_Uri = "dm:///_internal/model/uml#ClearAssociationAction";
      class _ClearStructuralFeatureAction {
      }
      _ClearStructuralFeatureAction.result = "result";
      _ClearStructuralFeatureAction.object = "object";
      _ClearStructuralFeatureAction.structuralFeature = "structuralFeature";
      _ClearStructuralFeatureAction.context = "context";
      _ClearStructuralFeatureAction.input = "input";
      _ClearStructuralFeatureAction.isLocallyReentrant = "isLocallyReentrant";
      _ClearStructuralFeatureAction.localPostcondition = "localPostcondition";
      _ClearStructuralFeatureAction.localPrecondition = "localPrecondition";
      _ClearStructuralFeatureAction.output = "output";
      _ClearStructuralFeatureAction.handler = "handler";
      _ClearStructuralFeatureAction.activity = "activity";
      _ClearStructuralFeatureAction.inGroup = "inGroup";
      _ClearStructuralFeatureAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ClearStructuralFeatureAction.inPartition = "inPartition";
      _ClearStructuralFeatureAction.inStructuredNode = "inStructuredNode";
      _ClearStructuralFeatureAction.incoming = "incoming";
      _ClearStructuralFeatureAction.outgoing = "outgoing";
      _ClearStructuralFeatureAction.redefinedNode = "redefinedNode";
      _ClearStructuralFeatureAction.isLeaf = "isLeaf";
      _ClearStructuralFeatureAction.redefinedElement = "redefinedElement";
      _ClearStructuralFeatureAction.redefinitionContext = "redefinitionContext";
      _ClearStructuralFeatureAction.clientDependency = "clientDependency";
      _ClearStructuralFeatureAction._name_ = "name";
      _ClearStructuralFeatureAction.nameExpression = "nameExpression";
      _ClearStructuralFeatureAction.namespace = "namespace";
      _ClearStructuralFeatureAction.qualifiedName = "qualifiedName";
      _ClearStructuralFeatureAction.visibility = "visibility";
      _ClearStructuralFeatureAction.ownedComment = "ownedComment";
      _ClearStructuralFeatureAction.ownedElement = "ownedElement";
      _ClearStructuralFeatureAction.owner = "owner";
      _Actions3._ClearStructuralFeatureAction = _ClearStructuralFeatureAction;
      _Actions3.__ClearStructuralFeatureAction_Uri = "dm:///_internal/model/uml#ClearStructuralFeatureAction";
      class _ClearVariableAction {
      }
      _ClearVariableAction.variable = "variable";
      _ClearVariableAction.context = "context";
      _ClearVariableAction.input = "input";
      _ClearVariableAction.isLocallyReentrant = "isLocallyReentrant";
      _ClearVariableAction.localPostcondition = "localPostcondition";
      _ClearVariableAction.localPrecondition = "localPrecondition";
      _ClearVariableAction.output = "output";
      _ClearVariableAction.handler = "handler";
      _ClearVariableAction.activity = "activity";
      _ClearVariableAction.inGroup = "inGroup";
      _ClearVariableAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ClearVariableAction.inPartition = "inPartition";
      _ClearVariableAction.inStructuredNode = "inStructuredNode";
      _ClearVariableAction.incoming = "incoming";
      _ClearVariableAction.outgoing = "outgoing";
      _ClearVariableAction.redefinedNode = "redefinedNode";
      _ClearVariableAction.isLeaf = "isLeaf";
      _ClearVariableAction.redefinedElement = "redefinedElement";
      _ClearVariableAction.redefinitionContext = "redefinitionContext";
      _ClearVariableAction.clientDependency = "clientDependency";
      _ClearVariableAction._name_ = "name";
      _ClearVariableAction.nameExpression = "nameExpression";
      _ClearVariableAction.namespace = "namespace";
      _ClearVariableAction.qualifiedName = "qualifiedName";
      _ClearVariableAction.visibility = "visibility";
      _ClearVariableAction.ownedComment = "ownedComment";
      _ClearVariableAction.ownedElement = "ownedElement";
      _ClearVariableAction.owner = "owner";
      _Actions3._ClearVariableAction = _ClearVariableAction;
      _Actions3.__ClearVariableAction_Uri = "dm:///_internal/model/uml#ClearVariableAction";
      class _ConditionalNode {
      }
      _ConditionalNode.clause = "clause";
      _ConditionalNode.isAssured = "isAssured";
      _ConditionalNode.isDeterminate = "isDeterminate";
      _ConditionalNode.result = "result";
      _ConditionalNode.activity = "activity";
      _ConditionalNode.edge = "edge";
      _ConditionalNode.mustIsolate = "mustIsolate";
      _ConditionalNode.node = "node";
      _ConditionalNode.structuredNodeInput = "structuredNodeInput";
      _ConditionalNode.structuredNodeOutput = "structuredNodeOutput";
      _ConditionalNode.variable = "variable";
      _ConditionalNode.elementImport = "elementImport";
      _ConditionalNode.importedMember = "importedMember";
      _ConditionalNode.member = "member";
      _ConditionalNode.ownedMember = "ownedMember";
      _ConditionalNode.ownedRule = "ownedRule";
      _ConditionalNode.packageImport = "packageImport";
      _ConditionalNode.clientDependency = "clientDependency";
      _ConditionalNode._name_ = "name";
      _ConditionalNode.nameExpression = "nameExpression";
      _ConditionalNode.namespace = "namespace";
      _ConditionalNode.qualifiedName = "qualifiedName";
      _ConditionalNode.visibility = "visibility";
      _ConditionalNode.ownedComment = "ownedComment";
      _ConditionalNode.ownedElement = "ownedElement";
      _ConditionalNode.owner = "owner";
      _ConditionalNode.containedEdge = "containedEdge";
      _ConditionalNode.containedNode = "containedNode";
      _ConditionalNode.inActivity = "inActivity";
      _ConditionalNode.subgroup = "subgroup";
      _ConditionalNode.superGroup = "superGroup";
      _ConditionalNode.context = "context";
      _ConditionalNode.input = "input";
      _ConditionalNode.isLocallyReentrant = "isLocallyReentrant";
      _ConditionalNode.localPostcondition = "localPostcondition";
      _ConditionalNode.localPrecondition = "localPrecondition";
      _ConditionalNode.output = "output";
      _ConditionalNode.handler = "handler";
      _ConditionalNode.inGroup = "inGroup";
      _ConditionalNode.inInterruptibleRegion = "inInterruptibleRegion";
      _ConditionalNode.inPartition = "inPartition";
      _ConditionalNode.inStructuredNode = "inStructuredNode";
      _ConditionalNode.incoming = "incoming";
      _ConditionalNode.outgoing = "outgoing";
      _ConditionalNode.redefinedNode = "redefinedNode";
      _ConditionalNode.isLeaf = "isLeaf";
      _ConditionalNode.redefinedElement = "redefinedElement";
      _ConditionalNode.redefinitionContext = "redefinitionContext";
      _Actions3._ConditionalNode = _ConditionalNode;
      _Actions3.__ConditionalNode_Uri = "dm:///_internal/model/uml#ConditionalNode";
      class _CreateLinkAction {
      }
      _CreateLinkAction.endData = "endData";
      _CreateLinkAction.inputValue = "inputValue";
      _CreateLinkAction.context = "context";
      _CreateLinkAction.input = "input";
      _CreateLinkAction.isLocallyReentrant = "isLocallyReentrant";
      _CreateLinkAction.localPostcondition = "localPostcondition";
      _CreateLinkAction.localPrecondition = "localPrecondition";
      _CreateLinkAction.output = "output";
      _CreateLinkAction.handler = "handler";
      _CreateLinkAction.activity = "activity";
      _CreateLinkAction.inGroup = "inGroup";
      _CreateLinkAction.inInterruptibleRegion = "inInterruptibleRegion";
      _CreateLinkAction.inPartition = "inPartition";
      _CreateLinkAction.inStructuredNode = "inStructuredNode";
      _CreateLinkAction.incoming = "incoming";
      _CreateLinkAction.outgoing = "outgoing";
      _CreateLinkAction.redefinedNode = "redefinedNode";
      _CreateLinkAction.isLeaf = "isLeaf";
      _CreateLinkAction.redefinedElement = "redefinedElement";
      _CreateLinkAction.redefinitionContext = "redefinitionContext";
      _CreateLinkAction.clientDependency = "clientDependency";
      _CreateLinkAction._name_ = "name";
      _CreateLinkAction.nameExpression = "nameExpression";
      _CreateLinkAction.namespace = "namespace";
      _CreateLinkAction.qualifiedName = "qualifiedName";
      _CreateLinkAction.visibility = "visibility";
      _CreateLinkAction.ownedComment = "ownedComment";
      _CreateLinkAction.ownedElement = "ownedElement";
      _CreateLinkAction.owner = "owner";
      _Actions3._CreateLinkAction = _CreateLinkAction;
      _Actions3.__CreateLinkAction_Uri = "dm:///_internal/model/uml#CreateLinkAction";
      class _CreateLinkObjectAction {
      }
      _CreateLinkObjectAction.result = "result";
      _CreateLinkObjectAction.endData = "endData";
      _CreateLinkObjectAction.inputValue = "inputValue";
      _CreateLinkObjectAction.context = "context";
      _CreateLinkObjectAction.input = "input";
      _CreateLinkObjectAction.isLocallyReentrant = "isLocallyReentrant";
      _CreateLinkObjectAction.localPostcondition = "localPostcondition";
      _CreateLinkObjectAction.localPrecondition = "localPrecondition";
      _CreateLinkObjectAction.output = "output";
      _CreateLinkObjectAction.handler = "handler";
      _CreateLinkObjectAction.activity = "activity";
      _CreateLinkObjectAction.inGroup = "inGroup";
      _CreateLinkObjectAction.inInterruptibleRegion = "inInterruptibleRegion";
      _CreateLinkObjectAction.inPartition = "inPartition";
      _CreateLinkObjectAction.inStructuredNode = "inStructuredNode";
      _CreateLinkObjectAction.incoming = "incoming";
      _CreateLinkObjectAction.outgoing = "outgoing";
      _CreateLinkObjectAction.redefinedNode = "redefinedNode";
      _CreateLinkObjectAction.isLeaf = "isLeaf";
      _CreateLinkObjectAction.redefinedElement = "redefinedElement";
      _CreateLinkObjectAction.redefinitionContext = "redefinitionContext";
      _CreateLinkObjectAction.clientDependency = "clientDependency";
      _CreateLinkObjectAction._name_ = "name";
      _CreateLinkObjectAction.nameExpression = "nameExpression";
      _CreateLinkObjectAction.namespace = "namespace";
      _CreateLinkObjectAction.qualifiedName = "qualifiedName";
      _CreateLinkObjectAction.visibility = "visibility";
      _CreateLinkObjectAction.ownedComment = "ownedComment";
      _CreateLinkObjectAction.ownedElement = "ownedElement";
      _CreateLinkObjectAction.owner = "owner";
      _Actions3._CreateLinkObjectAction = _CreateLinkObjectAction;
      _Actions3.__CreateLinkObjectAction_Uri = "dm:///_internal/model/uml#CreateLinkObjectAction";
      class _CreateObjectAction {
      }
      _CreateObjectAction.classifier = "classifier";
      _CreateObjectAction.result = "result";
      _CreateObjectAction.context = "context";
      _CreateObjectAction.input = "input";
      _CreateObjectAction.isLocallyReentrant = "isLocallyReentrant";
      _CreateObjectAction.localPostcondition = "localPostcondition";
      _CreateObjectAction.localPrecondition = "localPrecondition";
      _CreateObjectAction.output = "output";
      _CreateObjectAction.handler = "handler";
      _CreateObjectAction.activity = "activity";
      _CreateObjectAction.inGroup = "inGroup";
      _CreateObjectAction.inInterruptibleRegion = "inInterruptibleRegion";
      _CreateObjectAction.inPartition = "inPartition";
      _CreateObjectAction.inStructuredNode = "inStructuredNode";
      _CreateObjectAction.incoming = "incoming";
      _CreateObjectAction.outgoing = "outgoing";
      _CreateObjectAction.redefinedNode = "redefinedNode";
      _CreateObjectAction.isLeaf = "isLeaf";
      _CreateObjectAction.redefinedElement = "redefinedElement";
      _CreateObjectAction.redefinitionContext = "redefinitionContext";
      _CreateObjectAction.clientDependency = "clientDependency";
      _CreateObjectAction._name_ = "name";
      _CreateObjectAction.nameExpression = "nameExpression";
      _CreateObjectAction.namespace = "namespace";
      _CreateObjectAction.qualifiedName = "qualifiedName";
      _CreateObjectAction.visibility = "visibility";
      _CreateObjectAction.ownedComment = "ownedComment";
      _CreateObjectAction.ownedElement = "ownedElement";
      _CreateObjectAction.owner = "owner";
      _Actions3._CreateObjectAction = _CreateObjectAction;
      _Actions3.__CreateObjectAction_Uri = "dm:///_internal/model/uml#CreateObjectAction";
      class _DestroyLinkAction {
      }
      _DestroyLinkAction.endData = "endData";
      _DestroyLinkAction.inputValue = "inputValue";
      _DestroyLinkAction.context = "context";
      _DestroyLinkAction.input = "input";
      _DestroyLinkAction.isLocallyReentrant = "isLocallyReentrant";
      _DestroyLinkAction.localPostcondition = "localPostcondition";
      _DestroyLinkAction.localPrecondition = "localPrecondition";
      _DestroyLinkAction.output = "output";
      _DestroyLinkAction.handler = "handler";
      _DestroyLinkAction.activity = "activity";
      _DestroyLinkAction.inGroup = "inGroup";
      _DestroyLinkAction.inInterruptibleRegion = "inInterruptibleRegion";
      _DestroyLinkAction.inPartition = "inPartition";
      _DestroyLinkAction.inStructuredNode = "inStructuredNode";
      _DestroyLinkAction.incoming = "incoming";
      _DestroyLinkAction.outgoing = "outgoing";
      _DestroyLinkAction.redefinedNode = "redefinedNode";
      _DestroyLinkAction.isLeaf = "isLeaf";
      _DestroyLinkAction.redefinedElement = "redefinedElement";
      _DestroyLinkAction.redefinitionContext = "redefinitionContext";
      _DestroyLinkAction.clientDependency = "clientDependency";
      _DestroyLinkAction._name_ = "name";
      _DestroyLinkAction.nameExpression = "nameExpression";
      _DestroyLinkAction.namespace = "namespace";
      _DestroyLinkAction.qualifiedName = "qualifiedName";
      _DestroyLinkAction.visibility = "visibility";
      _DestroyLinkAction.ownedComment = "ownedComment";
      _DestroyLinkAction.ownedElement = "ownedElement";
      _DestroyLinkAction.owner = "owner";
      _Actions3._DestroyLinkAction = _DestroyLinkAction;
      _Actions3.__DestroyLinkAction_Uri = "dm:///_internal/model/uml#DestroyLinkAction";
      class _DestroyObjectAction {
      }
      _DestroyObjectAction.isDestroyLinks = "isDestroyLinks";
      _DestroyObjectAction.isDestroyOwnedObjects = "isDestroyOwnedObjects";
      _DestroyObjectAction.target = "target";
      _DestroyObjectAction.context = "context";
      _DestroyObjectAction.input = "input";
      _DestroyObjectAction.isLocallyReentrant = "isLocallyReentrant";
      _DestroyObjectAction.localPostcondition = "localPostcondition";
      _DestroyObjectAction.localPrecondition = "localPrecondition";
      _DestroyObjectAction.output = "output";
      _DestroyObjectAction.handler = "handler";
      _DestroyObjectAction.activity = "activity";
      _DestroyObjectAction.inGroup = "inGroup";
      _DestroyObjectAction.inInterruptibleRegion = "inInterruptibleRegion";
      _DestroyObjectAction.inPartition = "inPartition";
      _DestroyObjectAction.inStructuredNode = "inStructuredNode";
      _DestroyObjectAction.incoming = "incoming";
      _DestroyObjectAction.outgoing = "outgoing";
      _DestroyObjectAction.redefinedNode = "redefinedNode";
      _DestroyObjectAction.isLeaf = "isLeaf";
      _DestroyObjectAction.redefinedElement = "redefinedElement";
      _DestroyObjectAction.redefinitionContext = "redefinitionContext";
      _DestroyObjectAction.clientDependency = "clientDependency";
      _DestroyObjectAction._name_ = "name";
      _DestroyObjectAction.nameExpression = "nameExpression";
      _DestroyObjectAction.namespace = "namespace";
      _DestroyObjectAction.qualifiedName = "qualifiedName";
      _DestroyObjectAction.visibility = "visibility";
      _DestroyObjectAction.ownedComment = "ownedComment";
      _DestroyObjectAction.ownedElement = "ownedElement";
      _DestroyObjectAction.owner = "owner";
      _Actions3._DestroyObjectAction = _DestroyObjectAction;
      _Actions3.__DestroyObjectAction_Uri = "dm:///_internal/model/uml#DestroyObjectAction";
      class _ExpansionNode {
      }
      _ExpansionNode.regionAsInput = "regionAsInput";
      _ExpansionNode.regionAsOutput = "regionAsOutput";
      _ExpansionNode.inState = "inState";
      _ExpansionNode.isControlType = "isControlType";
      _ExpansionNode.ordering = "ordering";
      _ExpansionNode.selection = "selection";
      _ExpansionNode.upperBound = "upperBound";
      _ExpansionNode.type = "type";
      _ExpansionNode.clientDependency = "clientDependency";
      _ExpansionNode._name_ = "name";
      _ExpansionNode.nameExpression = "nameExpression";
      _ExpansionNode.namespace = "namespace";
      _ExpansionNode.qualifiedName = "qualifiedName";
      _ExpansionNode.visibility = "visibility";
      _ExpansionNode.ownedComment = "ownedComment";
      _ExpansionNode.ownedElement = "ownedElement";
      _ExpansionNode.owner = "owner";
      _ExpansionNode.activity = "activity";
      _ExpansionNode.inGroup = "inGroup";
      _ExpansionNode.inInterruptibleRegion = "inInterruptibleRegion";
      _ExpansionNode.inPartition = "inPartition";
      _ExpansionNode.inStructuredNode = "inStructuredNode";
      _ExpansionNode.incoming = "incoming";
      _ExpansionNode.outgoing = "outgoing";
      _ExpansionNode.redefinedNode = "redefinedNode";
      _ExpansionNode.isLeaf = "isLeaf";
      _ExpansionNode.redefinedElement = "redefinedElement";
      _ExpansionNode.redefinitionContext = "redefinitionContext";
      _Actions3._ExpansionNode = _ExpansionNode;
      _Actions3.__ExpansionNode_Uri = "dm:///_internal/model/uml#ExpansionNode";
      class _ExpansionRegion {
      }
      _ExpansionRegion.inputElement = "inputElement";
      _ExpansionRegion.mode = "mode";
      _ExpansionRegion.outputElement = "outputElement";
      _ExpansionRegion.activity = "activity";
      _ExpansionRegion.edge = "edge";
      _ExpansionRegion.mustIsolate = "mustIsolate";
      _ExpansionRegion.node = "node";
      _ExpansionRegion.structuredNodeInput = "structuredNodeInput";
      _ExpansionRegion.structuredNodeOutput = "structuredNodeOutput";
      _ExpansionRegion.variable = "variable";
      _ExpansionRegion.elementImport = "elementImport";
      _ExpansionRegion.importedMember = "importedMember";
      _ExpansionRegion.member = "member";
      _ExpansionRegion.ownedMember = "ownedMember";
      _ExpansionRegion.ownedRule = "ownedRule";
      _ExpansionRegion.packageImport = "packageImport";
      _ExpansionRegion.clientDependency = "clientDependency";
      _ExpansionRegion._name_ = "name";
      _ExpansionRegion.nameExpression = "nameExpression";
      _ExpansionRegion.namespace = "namespace";
      _ExpansionRegion.qualifiedName = "qualifiedName";
      _ExpansionRegion.visibility = "visibility";
      _ExpansionRegion.ownedComment = "ownedComment";
      _ExpansionRegion.ownedElement = "ownedElement";
      _ExpansionRegion.owner = "owner";
      _ExpansionRegion.containedEdge = "containedEdge";
      _ExpansionRegion.containedNode = "containedNode";
      _ExpansionRegion.inActivity = "inActivity";
      _ExpansionRegion.subgroup = "subgroup";
      _ExpansionRegion.superGroup = "superGroup";
      _ExpansionRegion.context = "context";
      _ExpansionRegion.input = "input";
      _ExpansionRegion.isLocallyReentrant = "isLocallyReentrant";
      _ExpansionRegion.localPostcondition = "localPostcondition";
      _ExpansionRegion.localPrecondition = "localPrecondition";
      _ExpansionRegion.output = "output";
      _ExpansionRegion.handler = "handler";
      _ExpansionRegion.inGroup = "inGroup";
      _ExpansionRegion.inInterruptibleRegion = "inInterruptibleRegion";
      _ExpansionRegion.inPartition = "inPartition";
      _ExpansionRegion.inStructuredNode = "inStructuredNode";
      _ExpansionRegion.incoming = "incoming";
      _ExpansionRegion.outgoing = "outgoing";
      _ExpansionRegion.redefinedNode = "redefinedNode";
      _ExpansionRegion.isLeaf = "isLeaf";
      _ExpansionRegion.redefinedElement = "redefinedElement";
      _ExpansionRegion.redefinitionContext = "redefinitionContext";
      _Actions3._ExpansionRegion = _ExpansionRegion;
      _Actions3.__ExpansionRegion_Uri = "dm:///_internal/model/uml#ExpansionRegion";
      class _InputPin {
      }
      _InputPin.isControl = "isControl";
      _InputPin.inState = "inState";
      _InputPin.isControlType = "isControlType";
      _InputPin.ordering = "ordering";
      _InputPin.selection = "selection";
      _InputPin.upperBound = "upperBound";
      _InputPin.type = "type";
      _InputPin.clientDependency = "clientDependency";
      _InputPin._name_ = "name";
      _InputPin.nameExpression = "nameExpression";
      _InputPin.namespace = "namespace";
      _InputPin.qualifiedName = "qualifiedName";
      _InputPin.visibility = "visibility";
      _InputPin.ownedComment = "ownedComment";
      _InputPin.ownedElement = "ownedElement";
      _InputPin.owner = "owner";
      _InputPin.activity = "activity";
      _InputPin.inGroup = "inGroup";
      _InputPin.inInterruptibleRegion = "inInterruptibleRegion";
      _InputPin.inPartition = "inPartition";
      _InputPin.inStructuredNode = "inStructuredNode";
      _InputPin.incoming = "incoming";
      _InputPin.outgoing = "outgoing";
      _InputPin.redefinedNode = "redefinedNode";
      _InputPin.isLeaf = "isLeaf";
      _InputPin.redefinedElement = "redefinedElement";
      _InputPin.redefinitionContext = "redefinitionContext";
      _InputPin.isOrdered = "isOrdered";
      _InputPin.isUnique = "isUnique";
      _InputPin.lower = "lower";
      _InputPin.lowerValue = "lowerValue";
      _InputPin.upper = "upper";
      _InputPin.upperValue = "upperValue";
      _Actions3._InputPin = _InputPin;
      _Actions3.__InputPin_Uri = "dm:///_internal/model/uml#InputPin";
      class _InvocationAction {
      }
      _InvocationAction.argument = "argument";
      _InvocationAction.onPort = "onPort";
      _InvocationAction.context = "context";
      _InvocationAction.input = "input";
      _InvocationAction.isLocallyReentrant = "isLocallyReentrant";
      _InvocationAction.localPostcondition = "localPostcondition";
      _InvocationAction.localPrecondition = "localPrecondition";
      _InvocationAction.output = "output";
      _InvocationAction.handler = "handler";
      _InvocationAction.activity = "activity";
      _InvocationAction.inGroup = "inGroup";
      _InvocationAction.inInterruptibleRegion = "inInterruptibleRegion";
      _InvocationAction.inPartition = "inPartition";
      _InvocationAction.inStructuredNode = "inStructuredNode";
      _InvocationAction.incoming = "incoming";
      _InvocationAction.outgoing = "outgoing";
      _InvocationAction.redefinedNode = "redefinedNode";
      _InvocationAction.isLeaf = "isLeaf";
      _InvocationAction.redefinedElement = "redefinedElement";
      _InvocationAction.redefinitionContext = "redefinitionContext";
      _InvocationAction.clientDependency = "clientDependency";
      _InvocationAction._name_ = "name";
      _InvocationAction.nameExpression = "nameExpression";
      _InvocationAction.namespace = "namespace";
      _InvocationAction.qualifiedName = "qualifiedName";
      _InvocationAction.visibility = "visibility";
      _InvocationAction.ownedComment = "ownedComment";
      _InvocationAction.ownedElement = "ownedElement";
      _InvocationAction.owner = "owner";
      _Actions3._InvocationAction = _InvocationAction;
      _Actions3.__InvocationAction_Uri = "dm:///_internal/model/uml#InvocationAction";
      class _LinkAction {
      }
      _LinkAction.endData = "endData";
      _LinkAction.inputValue = "inputValue";
      _LinkAction.context = "context";
      _LinkAction.input = "input";
      _LinkAction.isLocallyReentrant = "isLocallyReentrant";
      _LinkAction.localPostcondition = "localPostcondition";
      _LinkAction.localPrecondition = "localPrecondition";
      _LinkAction.output = "output";
      _LinkAction.handler = "handler";
      _LinkAction.activity = "activity";
      _LinkAction.inGroup = "inGroup";
      _LinkAction.inInterruptibleRegion = "inInterruptibleRegion";
      _LinkAction.inPartition = "inPartition";
      _LinkAction.inStructuredNode = "inStructuredNode";
      _LinkAction.incoming = "incoming";
      _LinkAction.outgoing = "outgoing";
      _LinkAction.redefinedNode = "redefinedNode";
      _LinkAction.isLeaf = "isLeaf";
      _LinkAction.redefinedElement = "redefinedElement";
      _LinkAction.redefinitionContext = "redefinitionContext";
      _LinkAction.clientDependency = "clientDependency";
      _LinkAction._name_ = "name";
      _LinkAction.nameExpression = "nameExpression";
      _LinkAction.namespace = "namespace";
      _LinkAction.qualifiedName = "qualifiedName";
      _LinkAction.visibility = "visibility";
      _LinkAction.ownedComment = "ownedComment";
      _LinkAction.ownedElement = "ownedElement";
      _LinkAction.owner = "owner";
      _Actions3._LinkAction = _LinkAction;
      _Actions3.__LinkAction_Uri = "dm:///_internal/model/uml#LinkAction";
      class _LinkEndCreationData {
      }
      _LinkEndCreationData.insertAt = "insertAt";
      _LinkEndCreationData.isReplaceAll = "isReplaceAll";
      _LinkEndCreationData.end = "end";
      _LinkEndCreationData.qualifier = "qualifier";
      _LinkEndCreationData.value = "value";
      _LinkEndCreationData.ownedComment = "ownedComment";
      _LinkEndCreationData.ownedElement = "ownedElement";
      _LinkEndCreationData.owner = "owner";
      _Actions3._LinkEndCreationData = _LinkEndCreationData;
      _Actions3.__LinkEndCreationData_Uri = "dm:///_internal/model/uml#LinkEndCreationData";
      class _LinkEndData {
      }
      _LinkEndData.end = "end";
      _LinkEndData.qualifier = "qualifier";
      _LinkEndData.value = "value";
      _LinkEndData.ownedComment = "ownedComment";
      _LinkEndData.ownedElement = "ownedElement";
      _LinkEndData.owner = "owner";
      _Actions3._LinkEndData = _LinkEndData;
      _Actions3.__LinkEndData_Uri = "dm:///_internal/model/uml#LinkEndData";
      class _LinkEndDestructionData {
      }
      _LinkEndDestructionData.destroyAt = "destroyAt";
      _LinkEndDestructionData.isDestroyDuplicates = "isDestroyDuplicates";
      _LinkEndDestructionData.end = "end";
      _LinkEndDestructionData.qualifier = "qualifier";
      _LinkEndDestructionData.value = "value";
      _LinkEndDestructionData.ownedComment = "ownedComment";
      _LinkEndDestructionData.ownedElement = "ownedElement";
      _LinkEndDestructionData.owner = "owner";
      _Actions3._LinkEndDestructionData = _LinkEndDestructionData;
      _Actions3.__LinkEndDestructionData_Uri = "dm:///_internal/model/uml#LinkEndDestructionData";
      class _LoopNode {
      }
      _LoopNode.bodyOutput = "bodyOutput";
      _LoopNode.bodyPart = "bodyPart";
      _LoopNode.decider = "decider";
      _LoopNode.isTestedFirst = "isTestedFirst";
      _LoopNode.loopVariable = "loopVariable";
      _LoopNode.loopVariableInput = "loopVariableInput";
      _LoopNode.result = "result";
      _LoopNode.setupPart = "setupPart";
      _LoopNode.test = "test";
      _LoopNode.activity = "activity";
      _LoopNode.edge = "edge";
      _LoopNode.mustIsolate = "mustIsolate";
      _LoopNode.node = "node";
      _LoopNode.structuredNodeInput = "structuredNodeInput";
      _LoopNode.structuredNodeOutput = "structuredNodeOutput";
      _LoopNode.variable = "variable";
      _LoopNode.elementImport = "elementImport";
      _LoopNode.importedMember = "importedMember";
      _LoopNode.member = "member";
      _LoopNode.ownedMember = "ownedMember";
      _LoopNode.ownedRule = "ownedRule";
      _LoopNode.packageImport = "packageImport";
      _LoopNode.clientDependency = "clientDependency";
      _LoopNode._name_ = "name";
      _LoopNode.nameExpression = "nameExpression";
      _LoopNode.namespace = "namespace";
      _LoopNode.qualifiedName = "qualifiedName";
      _LoopNode.visibility = "visibility";
      _LoopNode.ownedComment = "ownedComment";
      _LoopNode.ownedElement = "ownedElement";
      _LoopNode.owner = "owner";
      _LoopNode.containedEdge = "containedEdge";
      _LoopNode.containedNode = "containedNode";
      _LoopNode.inActivity = "inActivity";
      _LoopNode.subgroup = "subgroup";
      _LoopNode.superGroup = "superGroup";
      _LoopNode.context = "context";
      _LoopNode.input = "input";
      _LoopNode.isLocallyReentrant = "isLocallyReentrant";
      _LoopNode.localPostcondition = "localPostcondition";
      _LoopNode.localPrecondition = "localPrecondition";
      _LoopNode.output = "output";
      _LoopNode.handler = "handler";
      _LoopNode.inGroup = "inGroup";
      _LoopNode.inInterruptibleRegion = "inInterruptibleRegion";
      _LoopNode.inPartition = "inPartition";
      _LoopNode.inStructuredNode = "inStructuredNode";
      _LoopNode.incoming = "incoming";
      _LoopNode.outgoing = "outgoing";
      _LoopNode.redefinedNode = "redefinedNode";
      _LoopNode.isLeaf = "isLeaf";
      _LoopNode.redefinedElement = "redefinedElement";
      _LoopNode.redefinitionContext = "redefinitionContext";
      _Actions3._LoopNode = _LoopNode;
      _Actions3.__LoopNode_Uri = "dm:///_internal/model/uml#LoopNode";
      class _OpaqueAction {
      }
      _OpaqueAction.body = "body";
      _OpaqueAction.inputValue = "inputValue";
      _OpaqueAction.language = "language";
      _OpaqueAction.outputValue = "outputValue";
      _OpaqueAction.context = "context";
      _OpaqueAction.input = "input";
      _OpaqueAction.isLocallyReentrant = "isLocallyReentrant";
      _OpaqueAction.localPostcondition = "localPostcondition";
      _OpaqueAction.localPrecondition = "localPrecondition";
      _OpaqueAction.output = "output";
      _OpaqueAction.handler = "handler";
      _OpaqueAction.activity = "activity";
      _OpaqueAction.inGroup = "inGroup";
      _OpaqueAction.inInterruptibleRegion = "inInterruptibleRegion";
      _OpaqueAction.inPartition = "inPartition";
      _OpaqueAction.inStructuredNode = "inStructuredNode";
      _OpaqueAction.incoming = "incoming";
      _OpaqueAction.outgoing = "outgoing";
      _OpaqueAction.redefinedNode = "redefinedNode";
      _OpaqueAction.isLeaf = "isLeaf";
      _OpaqueAction.redefinedElement = "redefinedElement";
      _OpaqueAction.redefinitionContext = "redefinitionContext";
      _OpaqueAction.clientDependency = "clientDependency";
      _OpaqueAction._name_ = "name";
      _OpaqueAction.nameExpression = "nameExpression";
      _OpaqueAction.namespace = "namespace";
      _OpaqueAction.qualifiedName = "qualifiedName";
      _OpaqueAction.visibility = "visibility";
      _OpaqueAction.ownedComment = "ownedComment";
      _OpaqueAction.ownedElement = "ownedElement";
      _OpaqueAction.owner = "owner";
      _Actions3._OpaqueAction = _OpaqueAction;
      _Actions3.__OpaqueAction_Uri = "dm:///_internal/model/uml#OpaqueAction";
      class _OutputPin {
      }
      _OutputPin.isControl = "isControl";
      _OutputPin.inState = "inState";
      _OutputPin.isControlType = "isControlType";
      _OutputPin.ordering = "ordering";
      _OutputPin.selection = "selection";
      _OutputPin.upperBound = "upperBound";
      _OutputPin.type = "type";
      _OutputPin.clientDependency = "clientDependency";
      _OutputPin._name_ = "name";
      _OutputPin.nameExpression = "nameExpression";
      _OutputPin.namespace = "namespace";
      _OutputPin.qualifiedName = "qualifiedName";
      _OutputPin.visibility = "visibility";
      _OutputPin.ownedComment = "ownedComment";
      _OutputPin.ownedElement = "ownedElement";
      _OutputPin.owner = "owner";
      _OutputPin.activity = "activity";
      _OutputPin.inGroup = "inGroup";
      _OutputPin.inInterruptibleRegion = "inInterruptibleRegion";
      _OutputPin.inPartition = "inPartition";
      _OutputPin.inStructuredNode = "inStructuredNode";
      _OutputPin.incoming = "incoming";
      _OutputPin.outgoing = "outgoing";
      _OutputPin.redefinedNode = "redefinedNode";
      _OutputPin.isLeaf = "isLeaf";
      _OutputPin.redefinedElement = "redefinedElement";
      _OutputPin.redefinitionContext = "redefinitionContext";
      _OutputPin.isOrdered = "isOrdered";
      _OutputPin.isUnique = "isUnique";
      _OutputPin.lower = "lower";
      _OutputPin.lowerValue = "lowerValue";
      _OutputPin.upper = "upper";
      _OutputPin.upperValue = "upperValue";
      _Actions3._OutputPin = _OutputPin;
      _Actions3.__OutputPin_Uri = "dm:///_internal/model/uml#OutputPin";
      class _Pin {
      }
      _Pin.isControl = "isControl";
      _Pin.inState = "inState";
      _Pin.isControlType = "isControlType";
      _Pin.ordering = "ordering";
      _Pin.selection = "selection";
      _Pin.upperBound = "upperBound";
      _Pin.type = "type";
      _Pin.clientDependency = "clientDependency";
      _Pin._name_ = "name";
      _Pin.nameExpression = "nameExpression";
      _Pin.namespace = "namespace";
      _Pin.qualifiedName = "qualifiedName";
      _Pin.visibility = "visibility";
      _Pin.ownedComment = "ownedComment";
      _Pin.ownedElement = "ownedElement";
      _Pin.owner = "owner";
      _Pin.activity = "activity";
      _Pin.inGroup = "inGroup";
      _Pin.inInterruptibleRegion = "inInterruptibleRegion";
      _Pin.inPartition = "inPartition";
      _Pin.inStructuredNode = "inStructuredNode";
      _Pin.incoming = "incoming";
      _Pin.outgoing = "outgoing";
      _Pin.redefinedNode = "redefinedNode";
      _Pin.isLeaf = "isLeaf";
      _Pin.redefinedElement = "redefinedElement";
      _Pin.redefinitionContext = "redefinitionContext";
      _Pin.isOrdered = "isOrdered";
      _Pin.isUnique = "isUnique";
      _Pin.lower = "lower";
      _Pin.lowerValue = "lowerValue";
      _Pin.upper = "upper";
      _Pin.upperValue = "upperValue";
      _Actions3._Pin = _Pin;
      _Actions3.__Pin_Uri = "dm:///_internal/model/uml#Pin";
      class _QualifierValue {
      }
      _QualifierValue.qualifier = "qualifier";
      _QualifierValue.value = "value";
      _QualifierValue.ownedComment = "ownedComment";
      _QualifierValue.ownedElement = "ownedElement";
      _QualifierValue.owner = "owner";
      _Actions3._QualifierValue = _QualifierValue;
      _Actions3.__QualifierValue_Uri = "dm:///_internal/model/uml#QualifierValue";
      class _RaiseExceptionAction {
      }
      _RaiseExceptionAction.exception = "exception";
      _RaiseExceptionAction.context = "context";
      _RaiseExceptionAction.input = "input";
      _RaiseExceptionAction.isLocallyReentrant = "isLocallyReentrant";
      _RaiseExceptionAction.localPostcondition = "localPostcondition";
      _RaiseExceptionAction.localPrecondition = "localPrecondition";
      _RaiseExceptionAction.output = "output";
      _RaiseExceptionAction.handler = "handler";
      _RaiseExceptionAction.activity = "activity";
      _RaiseExceptionAction.inGroup = "inGroup";
      _RaiseExceptionAction.inInterruptibleRegion = "inInterruptibleRegion";
      _RaiseExceptionAction.inPartition = "inPartition";
      _RaiseExceptionAction.inStructuredNode = "inStructuredNode";
      _RaiseExceptionAction.incoming = "incoming";
      _RaiseExceptionAction.outgoing = "outgoing";
      _RaiseExceptionAction.redefinedNode = "redefinedNode";
      _RaiseExceptionAction.isLeaf = "isLeaf";
      _RaiseExceptionAction.redefinedElement = "redefinedElement";
      _RaiseExceptionAction.redefinitionContext = "redefinitionContext";
      _RaiseExceptionAction.clientDependency = "clientDependency";
      _RaiseExceptionAction._name_ = "name";
      _RaiseExceptionAction.nameExpression = "nameExpression";
      _RaiseExceptionAction.namespace = "namespace";
      _RaiseExceptionAction.qualifiedName = "qualifiedName";
      _RaiseExceptionAction.visibility = "visibility";
      _RaiseExceptionAction.ownedComment = "ownedComment";
      _RaiseExceptionAction.ownedElement = "ownedElement";
      _RaiseExceptionAction.owner = "owner";
      _Actions3._RaiseExceptionAction = _RaiseExceptionAction;
      _Actions3.__RaiseExceptionAction_Uri = "dm:///_internal/model/uml#RaiseExceptionAction";
      class _ReadExtentAction {
      }
      _ReadExtentAction.classifier = "classifier";
      _ReadExtentAction.result = "result";
      _ReadExtentAction.context = "context";
      _ReadExtentAction.input = "input";
      _ReadExtentAction.isLocallyReentrant = "isLocallyReentrant";
      _ReadExtentAction.localPostcondition = "localPostcondition";
      _ReadExtentAction.localPrecondition = "localPrecondition";
      _ReadExtentAction.output = "output";
      _ReadExtentAction.handler = "handler";
      _ReadExtentAction.activity = "activity";
      _ReadExtentAction.inGroup = "inGroup";
      _ReadExtentAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReadExtentAction.inPartition = "inPartition";
      _ReadExtentAction.inStructuredNode = "inStructuredNode";
      _ReadExtentAction.incoming = "incoming";
      _ReadExtentAction.outgoing = "outgoing";
      _ReadExtentAction.redefinedNode = "redefinedNode";
      _ReadExtentAction.isLeaf = "isLeaf";
      _ReadExtentAction.redefinedElement = "redefinedElement";
      _ReadExtentAction.redefinitionContext = "redefinitionContext";
      _ReadExtentAction.clientDependency = "clientDependency";
      _ReadExtentAction._name_ = "name";
      _ReadExtentAction.nameExpression = "nameExpression";
      _ReadExtentAction.namespace = "namespace";
      _ReadExtentAction.qualifiedName = "qualifiedName";
      _ReadExtentAction.visibility = "visibility";
      _ReadExtentAction.ownedComment = "ownedComment";
      _ReadExtentAction.ownedElement = "ownedElement";
      _ReadExtentAction.owner = "owner";
      _Actions3._ReadExtentAction = _ReadExtentAction;
      _Actions3.__ReadExtentAction_Uri = "dm:///_internal/model/uml#ReadExtentAction";
      class _ReadIsClassifiedObjectAction {
      }
      _ReadIsClassifiedObjectAction.classifier = "classifier";
      _ReadIsClassifiedObjectAction.isDirect = "isDirect";
      _ReadIsClassifiedObjectAction.object = "object";
      _ReadIsClassifiedObjectAction.result = "result";
      _ReadIsClassifiedObjectAction.context = "context";
      _ReadIsClassifiedObjectAction.input = "input";
      _ReadIsClassifiedObjectAction.isLocallyReentrant = "isLocallyReentrant";
      _ReadIsClassifiedObjectAction.localPostcondition = "localPostcondition";
      _ReadIsClassifiedObjectAction.localPrecondition = "localPrecondition";
      _ReadIsClassifiedObjectAction.output = "output";
      _ReadIsClassifiedObjectAction.handler = "handler";
      _ReadIsClassifiedObjectAction.activity = "activity";
      _ReadIsClassifiedObjectAction.inGroup = "inGroup";
      _ReadIsClassifiedObjectAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReadIsClassifiedObjectAction.inPartition = "inPartition";
      _ReadIsClassifiedObjectAction.inStructuredNode = "inStructuredNode";
      _ReadIsClassifiedObjectAction.incoming = "incoming";
      _ReadIsClassifiedObjectAction.outgoing = "outgoing";
      _ReadIsClassifiedObjectAction.redefinedNode = "redefinedNode";
      _ReadIsClassifiedObjectAction.isLeaf = "isLeaf";
      _ReadIsClassifiedObjectAction.redefinedElement = "redefinedElement";
      _ReadIsClassifiedObjectAction.redefinitionContext = "redefinitionContext";
      _ReadIsClassifiedObjectAction.clientDependency = "clientDependency";
      _ReadIsClassifiedObjectAction._name_ = "name";
      _ReadIsClassifiedObjectAction.nameExpression = "nameExpression";
      _ReadIsClassifiedObjectAction.namespace = "namespace";
      _ReadIsClassifiedObjectAction.qualifiedName = "qualifiedName";
      _ReadIsClassifiedObjectAction.visibility = "visibility";
      _ReadIsClassifiedObjectAction.ownedComment = "ownedComment";
      _ReadIsClassifiedObjectAction.ownedElement = "ownedElement";
      _ReadIsClassifiedObjectAction.owner = "owner";
      _Actions3._ReadIsClassifiedObjectAction = _ReadIsClassifiedObjectAction;
      _Actions3.__ReadIsClassifiedObjectAction_Uri = "dm:///_internal/model/uml#ReadIsClassifiedObjectAction";
      class _ReadLinkAction {
      }
      _ReadLinkAction.result = "result";
      _ReadLinkAction.endData = "endData";
      _ReadLinkAction.inputValue = "inputValue";
      _ReadLinkAction.context = "context";
      _ReadLinkAction.input = "input";
      _ReadLinkAction.isLocallyReentrant = "isLocallyReentrant";
      _ReadLinkAction.localPostcondition = "localPostcondition";
      _ReadLinkAction.localPrecondition = "localPrecondition";
      _ReadLinkAction.output = "output";
      _ReadLinkAction.handler = "handler";
      _ReadLinkAction.activity = "activity";
      _ReadLinkAction.inGroup = "inGroup";
      _ReadLinkAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReadLinkAction.inPartition = "inPartition";
      _ReadLinkAction.inStructuredNode = "inStructuredNode";
      _ReadLinkAction.incoming = "incoming";
      _ReadLinkAction.outgoing = "outgoing";
      _ReadLinkAction.redefinedNode = "redefinedNode";
      _ReadLinkAction.isLeaf = "isLeaf";
      _ReadLinkAction.redefinedElement = "redefinedElement";
      _ReadLinkAction.redefinitionContext = "redefinitionContext";
      _ReadLinkAction.clientDependency = "clientDependency";
      _ReadLinkAction._name_ = "name";
      _ReadLinkAction.nameExpression = "nameExpression";
      _ReadLinkAction.namespace = "namespace";
      _ReadLinkAction.qualifiedName = "qualifiedName";
      _ReadLinkAction.visibility = "visibility";
      _ReadLinkAction.ownedComment = "ownedComment";
      _ReadLinkAction.ownedElement = "ownedElement";
      _ReadLinkAction.owner = "owner";
      _Actions3._ReadLinkAction = _ReadLinkAction;
      _Actions3.__ReadLinkAction_Uri = "dm:///_internal/model/uml#ReadLinkAction";
      class _ReadLinkObjectEndAction {
      }
      _ReadLinkObjectEndAction.end = "end";
      _ReadLinkObjectEndAction.object = "object";
      _ReadLinkObjectEndAction.result = "result";
      _ReadLinkObjectEndAction.context = "context";
      _ReadLinkObjectEndAction.input = "input";
      _ReadLinkObjectEndAction.isLocallyReentrant = "isLocallyReentrant";
      _ReadLinkObjectEndAction.localPostcondition = "localPostcondition";
      _ReadLinkObjectEndAction.localPrecondition = "localPrecondition";
      _ReadLinkObjectEndAction.output = "output";
      _ReadLinkObjectEndAction.handler = "handler";
      _ReadLinkObjectEndAction.activity = "activity";
      _ReadLinkObjectEndAction.inGroup = "inGroup";
      _ReadLinkObjectEndAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReadLinkObjectEndAction.inPartition = "inPartition";
      _ReadLinkObjectEndAction.inStructuredNode = "inStructuredNode";
      _ReadLinkObjectEndAction.incoming = "incoming";
      _ReadLinkObjectEndAction.outgoing = "outgoing";
      _ReadLinkObjectEndAction.redefinedNode = "redefinedNode";
      _ReadLinkObjectEndAction.isLeaf = "isLeaf";
      _ReadLinkObjectEndAction.redefinedElement = "redefinedElement";
      _ReadLinkObjectEndAction.redefinitionContext = "redefinitionContext";
      _ReadLinkObjectEndAction.clientDependency = "clientDependency";
      _ReadLinkObjectEndAction._name_ = "name";
      _ReadLinkObjectEndAction.nameExpression = "nameExpression";
      _ReadLinkObjectEndAction.namespace = "namespace";
      _ReadLinkObjectEndAction.qualifiedName = "qualifiedName";
      _ReadLinkObjectEndAction.visibility = "visibility";
      _ReadLinkObjectEndAction.ownedComment = "ownedComment";
      _ReadLinkObjectEndAction.ownedElement = "ownedElement";
      _ReadLinkObjectEndAction.owner = "owner";
      _Actions3._ReadLinkObjectEndAction = _ReadLinkObjectEndAction;
      _Actions3.__ReadLinkObjectEndAction_Uri = "dm:///_internal/model/uml#ReadLinkObjectEndAction";
      class _ReadLinkObjectEndQualifierAction {
      }
      _ReadLinkObjectEndQualifierAction.object = "object";
      _ReadLinkObjectEndQualifierAction.qualifier = "qualifier";
      _ReadLinkObjectEndQualifierAction.result = "result";
      _ReadLinkObjectEndQualifierAction.context = "context";
      _ReadLinkObjectEndQualifierAction.input = "input";
      _ReadLinkObjectEndQualifierAction.isLocallyReentrant = "isLocallyReentrant";
      _ReadLinkObjectEndQualifierAction.localPostcondition = "localPostcondition";
      _ReadLinkObjectEndQualifierAction.localPrecondition = "localPrecondition";
      _ReadLinkObjectEndQualifierAction.output = "output";
      _ReadLinkObjectEndQualifierAction.handler = "handler";
      _ReadLinkObjectEndQualifierAction.activity = "activity";
      _ReadLinkObjectEndQualifierAction.inGroup = "inGroup";
      _ReadLinkObjectEndQualifierAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReadLinkObjectEndQualifierAction.inPartition = "inPartition";
      _ReadLinkObjectEndQualifierAction.inStructuredNode = "inStructuredNode";
      _ReadLinkObjectEndQualifierAction.incoming = "incoming";
      _ReadLinkObjectEndQualifierAction.outgoing = "outgoing";
      _ReadLinkObjectEndQualifierAction.redefinedNode = "redefinedNode";
      _ReadLinkObjectEndQualifierAction.isLeaf = "isLeaf";
      _ReadLinkObjectEndQualifierAction.redefinedElement = "redefinedElement";
      _ReadLinkObjectEndQualifierAction.redefinitionContext = "redefinitionContext";
      _ReadLinkObjectEndQualifierAction.clientDependency = "clientDependency";
      _ReadLinkObjectEndQualifierAction._name_ = "name";
      _ReadLinkObjectEndQualifierAction.nameExpression = "nameExpression";
      _ReadLinkObjectEndQualifierAction.namespace = "namespace";
      _ReadLinkObjectEndQualifierAction.qualifiedName = "qualifiedName";
      _ReadLinkObjectEndQualifierAction.visibility = "visibility";
      _ReadLinkObjectEndQualifierAction.ownedComment = "ownedComment";
      _ReadLinkObjectEndQualifierAction.ownedElement = "ownedElement";
      _ReadLinkObjectEndQualifierAction.owner = "owner";
      _Actions3._ReadLinkObjectEndQualifierAction = _ReadLinkObjectEndQualifierAction;
      _Actions3.__ReadLinkObjectEndQualifierAction_Uri = "dm:///_internal/model/uml#ReadLinkObjectEndQualifierAction";
      class _ReadSelfAction {
      }
      _ReadSelfAction.result = "result";
      _ReadSelfAction.context = "context";
      _ReadSelfAction.input = "input";
      _ReadSelfAction.isLocallyReentrant = "isLocallyReentrant";
      _ReadSelfAction.localPostcondition = "localPostcondition";
      _ReadSelfAction.localPrecondition = "localPrecondition";
      _ReadSelfAction.output = "output";
      _ReadSelfAction.handler = "handler";
      _ReadSelfAction.activity = "activity";
      _ReadSelfAction.inGroup = "inGroup";
      _ReadSelfAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReadSelfAction.inPartition = "inPartition";
      _ReadSelfAction.inStructuredNode = "inStructuredNode";
      _ReadSelfAction.incoming = "incoming";
      _ReadSelfAction.outgoing = "outgoing";
      _ReadSelfAction.redefinedNode = "redefinedNode";
      _ReadSelfAction.isLeaf = "isLeaf";
      _ReadSelfAction.redefinedElement = "redefinedElement";
      _ReadSelfAction.redefinitionContext = "redefinitionContext";
      _ReadSelfAction.clientDependency = "clientDependency";
      _ReadSelfAction._name_ = "name";
      _ReadSelfAction.nameExpression = "nameExpression";
      _ReadSelfAction.namespace = "namespace";
      _ReadSelfAction.qualifiedName = "qualifiedName";
      _ReadSelfAction.visibility = "visibility";
      _ReadSelfAction.ownedComment = "ownedComment";
      _ReadSelfAction.ownedElement = "ownedElement";
      _ReadSelfAction.owner = "owner";
      _Actions3._ReadSelfAction = _ReadSelfAction;
      _Actions3.__ReadSelfAction_Uri = "dm:///_internal/model/uml#ReadSelfAction";
      class _ReadStructuralFeatureAction {
      }
      _ReadStructuralFeatureAction.result = "result";
      _ReadStructuralFeatureAction.object = "object";
      _ReadStructuralFeatureAction.structuralFeature = "structuralFeature";
      _ReadStructuralFeatureAction.context = "context";
      _ReadStructuralFeatureAction.input = "input";
      _ReadStructuralFeatureAction.isLocallyReentrant = "isLocallyReentrant";
      _ReadStructuralFeatureAction.localPostcondition = "localPostcondition";
      _ReadStructuralFeatureAction.localPrecondition = "localPrecondition";
      _ReadStructuralFeatureAction.output = "output";
      _ReadStructuralFeatureAction.handler = "handler";
      _ReadStructuralFeatureAction.activity = "activity";
      _ReadStructuralFeatureAction.inGroup = "inGroup";
      _ReadStructuralFeatureAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReadStructuralFeatureAction.inPartition = "inPartition";
      _ReadStructuralFeatureAction.inStructuredNode = "inStructuredNode";
      _ReadStructuralFeatureAction.incoming = "incoming";
      _ReadStructuralFeatureAction.outgoing = "outgoing";
      _ReadStructuralFeatureAction.redefinedNode = "redefinedNode";
      _ReadStructuralFeatureAction.isLeaf = "isLeaf";
      _ReadStructuralFeatureAction.redefinedElement = "redefinedElement";
      _ReadStructuralFeatureAction.redefinitionContext = "redefinitionContext";
      _ReadStructuralFeatureAction.clientDependency = "clientDependency";
      _ReadStructuralFeatureAction._name_ = "name";
      _ReadStructuralFeatureAction.nameExpression = "nameExpression";
      _ReadStructuralFeatureAction.namespace = "namespace";
      _ReadStructuralFeatureAction.qualifiedName = "qualifiedName";
      _ReadStructuralFeatureAction.visibility = "visibility";
      _ReadStructuralFeatureAction.ownedComment = "ownedComment";
      _ReadStructuralFeatureAction.ownedElement = "ownedElement";
      _ReadStructuralFeatureAction.owner = "owner";
      _Actions3._ReadStructuralFeatureAction = _ReadStructuralFeatureAction;
      _Actions3.__ReadStructuralFeatureAction_Uri = "dm:///_internal/model/uml#ReadStructuralFeatureAction";
      class _ReadVariableAction {
      }
      _ReadVariableAction.result = "result";
      _ReadVariableAction.variable = "variable";
      _ReadVariableAction.context = "context";
      _ReadVariableAction.input = "input";
      _ReadVariableAction.isLocallyReentrant = "isLocallyReentrant";
      _ReadVariableAction.localPostcondition = "localPostcondition";
      _ReadVariableAction.localPrecondition = "localPrecondition";
      _ReadVariableAction.output = "output";
      _ReadVariableAction.handler = "handler";
      _ReadVariableAction.activity = "activity";
      _ReadVariableAction.inGroup = "inGroup";
      _ReadVariableAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReadVariableAction.inPartition = "inPartition";
      _ReadVariableAction.inStructuredNode = "inStructuredNode";
      _ReadVariableAction.incoming = "incoming";
      _ReadVariableAction.outgoing = "outgoing";
      _ReadVariableAction.redefinedNode = "redefinedNode";
      _ReadVariableAction.isLeaf = "isLeaf";
      _ReadVariableAction.redefinedElement = "redefinedElement";
      _ReadVariableAction.redefinitionContext = "redefinitionContext";
      _ReadVariableAction.clientDependency = "clientDependency";
      _ReadVariableAction._name_ = "name";
      _ReadVariableAction.nameExpression = "nameExpression";
      _ReadVariableAction.namespace = "namespace";
      _ReadVariableAction.qualifiedName = "qualifiedName";
      _ReadVariableAction.visibility = "visibility";
      _ReadVariableAction.ownedComment = "ownedComment";
      _ReadVariableAction.ownedElement = "ownedElement";
      _ReadVariableAction.owner = "owner";
      _Actions3._ReadVariableAction = _ReadVariableAction;
      _Actions3.__ReadVariableAction_Uri = "dm:///_internal/model/uml#ReadVariableAction";
      class _ReclassifyObjectAction {
      }
      _ReclassifyObjectAction.isReplaceAll = "isReplaceAll";
      _ReclassifyObjectAction.newClassifier = "newClassifier";
      _ReclassifyObjectAction.object = "object";
      _ReclassifyObjectAction.oldClassifier = "oldClassifier";
      _ReclassifyObjectAction.context = "context";
      _ReclassifyObjectAction.input = "input";
      _ReclassifyObjectAction.isLocallyReentrant = "isLocallyReentrant";
      _ReclassifyObjectAction.localPostcondition = "localPostcondition";
      _ReclassifyObjectAction.localPrecondition = "localPrecondition";
      _ReclassifyObjectAction.output = "output";
      _ReclassifyObjectAction.handler = "handler";
      _ReclassifyObjectAction.activity = "activity";
      _ReclassifyObjectAction.inGroup = "inGroup";
      _ReclassifyObjectAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReclassifyObjectAction.inPartition = "inPartition";
      _ReclassifyObjectAction.inStructuredNode = "inStructuredNode";
      _ReclassifyObjectAction.incoming = "incoming";
      _ReclassifyObjectAction.outgoing = "outgoing";
      _ReclassifyObjectAction.redefinedNode = "redefinedNode";
      _ReclassifyObjectAction.isLeaf = "isLeaf";
      _ReclassifyObjectAction.redefinedElement = "redefinedElement";
      _ReclassifyObjectAction.redefinitionContext = "redefinitionContext";
      _ReclassifyObjectAction.clientDependency = "clientDependency";
      _ReclassifyObjectAction._name_ = "name";
      _ReclassifyObjectAction.nameExpression = "nameExpression";
      _ReclassifyObjectAction.namespace = "namespace";
      _ReclassifyObjectAction.qualifiedName = "qualifiedName";
      _ReclassifyObjectAction.visibility = "visibility";
      _ReclassifyObjectAction.ownedComment = "ownedComment";
      _ReclassifyObjectAction.ownedElement = "ownedElement";
      _ReclassifyObjectAction.owner = "owner";
      _Actions3._ReclassifyObjectAction = _ReclassifyObjectAction;
      _Actions3.__ReclassifyObjectAction_Uri = "dm:///_internal/model/uml#ReclassifyObjectAction";
      class _ReduceAction {
      }
      _ReduceAction.collection = "collection";
      _ReduceAction.isOrdered = "isOrdered";
      _ReduceAction.reducer = "reducer";
      _ReduceAction.result = "result";
      _ReduceAction.context = "context";
      _ReduceAction.input = "input";
      _ReduceAction.isLocallyReentrant = "isLocallyReentrant";
      _ReduceAction.localPostcondition = "localPostcondition";
      _ReduceAction.localPrecondition = "localPrecondition";
      _ReduceAction.output = "output";
      _ReduceAction.handler = "handler";
      _ReduceAction.activity = "activity";
      _ReduceAction.inGroup = "inGroup";
      _ReduceAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReduceAction.inPartition = "inPartition";
      _ReduceAction.inStructuredNode = "inStructuredNode";
      _ReduceAction.incoming = "incoming";
      _ReduceAction.outgoing = "outgoing";
      _ReduceAction.redefinedNode = "redefinedNode";
      _ReduceAction.isLeaf = "isLeaf";
      _ReduceAction.redefinedElement = "redefinedElement";
      _ReduceAction.redefinitionContext = "redefinitionContext";
      _ReduceAction.clientDependency = "clientDependency";
      _ReduceAction._name_ = "name";
      _ReduceAction.nameExpression = "nameExpression";
      _ReduceAction.namespace = "namespace";
      _ReduceAction.qualifiedName = "qualifiedName";
      _ReduceAction.visibility = "visibility";
      _ReduceAction.ownedComment = "ownedComment";
      _ReduceAction.ownedElement = "ownedElement";
      _ReduceAction.owner = "owner";
      _Actions3._ReduceAction = _ReduceAction;
      _Actions3.__ReduceAction_Uri = "dm:///_internal/model/uml#ReduceAction";
      class _RemoveStructuralFeatureValueAction {
      }
      _RemoveStructuralFeatureValueAction.isRemoveDuplicates = "isRemoveDuplicates";
      _RemoveStructuralFeatureValueAction.removeAt = "removeAt";
      _RemoveStructuralFeatureValueAction.result = "result";
      _RemoveStructuralFeatureValueAction.value = "value";
      _RemoveStructuralFeatureValueAction.object = "object";
      _RemoveStructuralFeatureValueAction.structuralFeature = "structuralFeature";
      _RemoveStructuralFeatureValueAction.context = "context";
      _RemoveStructuralFeatureValueAction.input = "input";
      _RemoveStructuralFeatureValueAction.isLocallyReentrant = "isLocallyReentrant";
      _RemoveStructuralFeatureValueAction.localPostcondition = "localPostcondition";
      _RemoveStructuralFeatureValueAction.localPrecondition = "localPrecondition";
      _RemoveStructuralFeatureValueAction.output = "output";
      _RemoveStructuralFeatureValueAction.handler = "handler";
      _RemoveStructuralFeatureValueAction.activity = "activity";
      _RemoveStructuralFeatureValueAction.inGroup = "inGroup";
      _RemoveStructuralFeatureValueAction.inInterruptibleRegion = "inInterruptibleRegion";
      _RemoveStructuralFeatureValueAction.inPartition = "inPartition";
      _RemoveStructuralFeatureValueAction.inStructuredNode = "inStructuredNode";
      _RemoveStructuralFeatureValueAction.incoming = "incoming";
      _RemoveStructuralFeatureValueAction.outgoing = "outgoing";
      _RemoveStructuralFeatureValueAction.redefinedNode = "redefinedNode";
      _RemoveStructuralFeatureValueAction.isLeaf = "isLeaf";
      _RemoveStructuralFeatureValueAction.redefinedElement = "redefinedElement";
      _RemoveStructuralFeatureValueAction.redefinitionContext = "redefinitionContext";
      _RemoveStructuralFeatureValueAction.clientDependency = "clientDependency";
      _RemoveStructuralFeatureValueAction._name_ = "name";
      _RemoveStructuralFeatureValueAction.nameExpression = "nameExpression";
      _RemoveStructuralFeatureValueAction.namespace = "namespace";
      _RemoveStructuralFeatureValueAction.qualifiedName = "qualifiedName";
      _RemoveStructuralFeatureValueAction.visibility = "visibility";
      _RemoveStructuralFeatureValueAction.ownedComment = "ownedComment";
      _RemoveStructuralFeatureValueAction.ownedElement = "ownedElement";
      _RemoveStructuralFeatureValueAction.owner = "owner";
      _Actions3._RemoveStructuralFeatureValueAction = _RemoveStructuralFeatureValueAction;
      _Actions3.__RemoveStructuralFeatureValueAction_Uri = "dm:///_internal/model/uml#RemoveStructuralFeatureValueAction";
      class _RemoveVariableValueAction {
      }
      _RemoveVariableValueAction.isRemoveDuplicates = "isRemoveDuplicates";
      _RemoveVariableValueAction.removeAt = "removeAt";
      _RemoveVariableValueAction.value = "value";
      _RemoveVariableValueAction.variable = "variable";
      _RemoveVariableValueAction.context = "context";
      _RemoveVariableValueAction.input = "input";
      _RemoveVariableValueAction.isLocallyReentrant = "isLocallyReentrant";
      _RemoveVariableValueAction.localPostcondition = "localPostcondition";
      _RemoveVariableValueAction.localPrecondition = "localPrecondition";
      _RemoveVariableValueAction.output = "output";
      _RemoveVariableValueAction.handler = "handler";
      _RemoveVariableValueAction.activity = "activity";
      _RemoveVariableValueAction.inGroup = "inGroup";
      _RemoveVariableValueAction.inInterruptibleRegion = "inInterruptibleRegion";
      _RemoveVariableValueAction.inPartition = "inPartition";
      _RemoveVariableValueAction.inStructuredNode = "inStructuredNode";
      _RemoveVariableValueAction.incoming = "incoming";
      _RemoveVariableValueAction.outgoing = "outgoing";
      _RemoveVariableValueAction.redefinedNode = "redefinedNode";
      _RemoveVariableValueAction.isLeaf = "isLeaf";
      _RemoveVariableValueAction.redefinedElement = "redefinedElement";
      _RemoveVariableValueAction.redefinitionContext = "redefinitionContext";
      _RemoveVariableValueAction.clientDependency = "clientDependency";
      _RemoveVariableValueAction._name_ = "name";
      _RemoveVariableValueAction.nameExpression = "nameExpression";
      _RemoveVariableValueAction.namespace = "namespace";
      _RemoveVariableValueAction.qualifiedName = "qualifiedName";
      _RemoveVariableValueAction.visibility = "visibility";
      _RemoveVariableValueAction.ownedComment = "ownedComment";
      _RemoveVariableValueAction.ownedElement = "ownedElement";
      _RemoveVariableValueAction.owner = "owner";
      _Actions3._RemoveVariableValueAction = _RemoveVariableValueAction;
      _Actions3.__RemoveVariableValueAction_Uri = "dm:///_internal/model/uml#RemoveVariableValueAction";
      class _ReplyAction {
      }
      _ReplyAction.replyToCall = "replyToCall";
      _ReplyAction.replyValue = "replyValue";
      _ReplyAction.returnInformation = "returnInformation";
      _ReplyAction.context = "context";
      _ReplyAction.input = "input";
      _ReplyAction.isLocallyReentrant = "isLocallyReentrant";
      _ReplyAction.localPostcondition = "localPostcondition";
      _ReplyAction.localPrecondition = "localPrecondition";
      _ReplyAction.output = "output";
      _ReplyAction.handler = "handler";
      _ReplyAction.activity = "activity";
      _ReplyAction.inGroup = "inGroup";
      _ReplyAction.inInterruptibleRegion = "inInterruptibleRegion";
      _ReplyAction.inPartition = "inPartition";
      _ReplyAction.inStructuredNode = "inStructuredNode";
      _ReplyAction.incoming = "incoming";
      _ReplyAction.outgoing = "outgoing";
      _ReplyAction.redefinedNode = "redefinedNode";
      _ReplyAction.isLeaf = "isLeaf";
      _ReplyAction.redefinedElement = "redefinedElement";
      _ReplyAction.redefinitionContext = "redefinitionContext";
      _ReplyAction.clientDependency = "clientDependency";
      _ReplyAction._name_ = "name";
      _ReplyAction.nameExpression = "nameExpression";
      _ReplyAction.namespace = "namespace";
      _ReplyAction.qualifiedName = "qualifiedName";
      _ReplyAction.visibility = "visibility";
      _ReplyAction.ownedComment = "ownedComment";
      _ReplyAction.ownedElement = "ownedElement";
      _ReplyAction.owner = "owner";
      _Actions3._ReplyAction = _ReplyAction;
      _Actions3.__ReplyAction_Uri = "dm:///_internal/model/uml#ReplyAction";
      class _SendObjectAction {
      }
      _SendObjectAction.request = "request";
      _SendObjectAction.target = "target";
      _SendObjectAction.argument = "argument";
      _SendObjectAction.onPort = "onPort";
      _SendObjectAction.context = "context";
      _SendObjectAction.input = "input";
      _SendObjectAction.isLocallyReentrant = "isLocallyReentrant";
      _SendObjectAction.localPostcondition = "localPostcondition";
      _SendObjectAction.localPrecondition = "localPrecondition";
      _SendObjectAction.output = "output";
      _SendObjectAction.handler = "handler";
      _SendObjectAction.activity = "activity";
      _SendObjectAction.inGroup = "inGroup";
      _SendObjectAction.inInterruptibleRegion = "inInterruptibleRegion";
      _SendObjectAction.inPartition = "inPartition";
      _SendObjectAction.inStructuredNode = "inStructuredNode";
      _SendObjectAction.incoming = "incoming";
      _SendObjectAction.outgoing = "outgoing";
      _SendObjectAction.redefinedNode = "redefinedNode";
      _SendObjectAction.isLeaf = "isLeaf";
      _SendObjectAction.redefinedElement = "redefinedElement";
      _SendObjectAction.redefinitionContext = "redefinitionContext";
      _SendObjectAction.clientDependency = "clientDependency";
      _SendObjectAction._name_ = "name";
      _SendObjectAction.nameExpression = "nameExpression";
      _SendObjectAction.namespace = "namespace";
      _SendObjectAction.qualifiedName = "qualifiedName";
      _SendObjectAction.visibility = "visibility";
      _SendObjectAction.ownedComment = "ownedComment";
      _SendObjectAction.ownedElement = "ownedElement";
      _SendObjectAction.owner = "owner";
      _Actions3._SendObjectAction = _SendObjectAction;
      _Actions3.__SendObjectAction_Uri = "dm:///_internal/model/uml#SendObjectAction";
      class _SendSignalAction {
      }
      _SendSignalAction.signal = "signal";
      _SendSignalAction.target = "target";
      _SendSignalAction.argument = "argument";
      _SendSignalAction.onPort = "onPort";
      _SendSignalAction.context = "context";
      _SendSignalAction.input = "input";
      _SendSignalAction.isLocallyReentrant = "isLocallyReentrant";
      _SendSignalAction.localPostcondition = "localPostcondition";
      _SendSignalAction.localPrecondition = "localPrecondition";
      _SendSignalAction.output = "output";
      _SendSignalAction.handler = "handler";
      _SendSignalAction.activity = "activity";
      _SendSignalAction.inGroup = "inGroup";
      _SendSignalAction.inInterruptibleRegion = "inInterruptibleRegion";
      _SendSignalAction.inPartition = "inPartition";
      _SendSignalAction.inStructuredNode = "inStructuredNode";
      _SendSignalAction.incoming = "incoming";
      _SendSignalAction.outgoing = "outgoing";
      _SendSignalAction.redefinedNode = "redefinedNode";
      _SendSignalAction.isLeaf = "isLeaf";
      _SendSignalAction.redefinedElement = "redefinedElement";
      _SendSignalAction.redefinitionContext = "redefinitionContext";
      _SendSignalAction.clientDependency = "clientDependency";
      _SendSignalAction._name_ = "name";
      _SendSignalAction.nameExpression = "nameExpression";
      _SendSignalAction.namespace = "namespace";
      _SendSignalAction.qualifiedName = "qualifiedName";
      _SendSignalAction.visibility = "visibility";
      _SendSignalAction.ownedComment = "ownedComment";
      _SendSignalAction.ownedElement = "ownedElement";
      _SendSignalAction.owner = "owner";
      _Actions3._SendSignalAction = _SendSignalAction;
      _Actions3.__SendSignalAction_Uri = "dm:///_internal/model/uml#SendSignalAction";
      class _SequenceNode {
      }
      _SequenceNode.executableNode = "executableNode";
      _SequenceNode.activity = "activity";
      _SequenceNode.edge = "edge";
      _SequenceNode.mustIsolate = "mustIsolate";
      _SequenceNode.node = "node";
      _SequenceNode.structuredNodeInput = "structuredNodeInput";
      _SequenceNode.structuredNodeOutput = "structuredNodeOutput";
      _SequenceNode.variable = "variable";
      _SequenceNode.elementImport = "elementImport";
      _SequenceNode.importedMember = "importedMember";
      _SequenceNode.member = "member";
      _SequenceNode.ownedMember = "ownedMember";
      _SequenceNode.ownedRule = "ownedRule";
      _SequenceNode.packageImport = "packageImport";
      _SequenceNode.clientDependency = "clientDependency";
      _SequenceNode._name_ = "name";
      _SequenceNode.nameExpression = "nameExpression";
      _SequenceNode.namespace = "namespace";
      _SequenceNode.qualifiedName = "qualifiedName";
      _SequenceNode.visibility = "visibility";
      _SequenceNode.ownedComment = "ownedComment";
      _SequenceNode.ownedElement = "ownedElement";
      _SequenceNode.owner = "owner";
      _SequenceNode.containedEdge = "containedEdge";
      _SequenceNode.containedNode = "containedNode";
      _SequenceNode.inActivity = "inActivity";
      _SequenceNode.subgroup = "subgroup";
      _SequenceNode.superGroup = "superGroup";
      _SequenceNode.context = "context";
      _SequenceNode.input = "input";
      _SequenceNode.isLocallyReentrant = "isLocallyReentrant";
      _SequenceNode.localPostcondition = "localPostcondition";
      _SequenceNode.localPrecondition = "localPrecondition";
      _SequenceNode.output = "output";
      _SequenceNode.handler = "handler";
      _SequenceNode.inGroup = "inGroup";
      _SequenceNode.inInterruptibleRegion = "inInterruptibleRegion";
      _SequenceNode.inPartition = "inPartition";
      _SequenceNode.inStructuredNode = "inStructuredNode";
      _SequenceNode.incoming = "incoming";
      _SequenceNode.outgoing = "outgoing";
      _SequenceNode.redefinedNode = "redefinedNode";
      _SequenceNode.isLeaf = "isLeaf";
      _SequenceNode.redefinedElement = "redefinedElement";
      _SequenceNode.redefinitionContext = "redefinitionContext";
      _Actions3._SequenceNode = _SequenceNode;
      _Actions3.__SequenceNode_Uri = "dm:///_internal/model/uml#SequenceNode";
      class _StartClassifierBehaviorAction {
      }
      _StartClassifierBehaviorAction.object = "object";
      _StartClassifierBehaviorAction.context = "context";
      _StartClassifierBehaviorAction.input = "input";
      _StartClassifierBehaviorAction.isLocallyReentrant = "isLocallyReentrant";
      _StartClassifierBehaviorAction.localPostcondition = "localPostcondition";
      _StartClassifierBehaviorAction.localPrecondition = "localPrecondition";
      _StartClassifierBehaviorAction.output = "output";
      _StartClassifierBehaviorAction.handler = "handler";
      _StartClassifierBehaviorAction.activity = "activity";
      _StartClassifierBehaviorAction.inGroup = "inGroup";
      _StartClassifierBehaviorAction.inInterruptibleRegion = "inInterruptibleRegion";
      _StartClassifierBehaviorAction.inPartition = "inPartition";
      _StartClassifierBehaviorAction.inStructuredNode = "inStructuredNode";
      _StartClassifierBehaviorAction.incoming = "incoming";
      _StartClassifierBehaviorAction.outgoing = "outgoing";
      _StartClassifierBehaviorAction.redefinedNode = "redefinedNode";
      _StartClassifierBehaviorAction.isLeaf = "isLeaf";
      _StartClassifierBehaviorAction.redefinedElement = "redefinedElement";
      _StartClassifierBehaviorAction.redefinitionContext = "redefinitionContext";
      _StartClassifierBehaviorAction.clientDependency = "clientDependency";
      _StartClassifierBehaviorAction._name_ = "name";
      _StartClassifierBehaviorAction.nameExpression = "nameExpression";
      _StartClassifierBehaviorAction.namespace = "namespace";
      _StartClassifierBehaviorAction.qualifiedName = "qualifiedName";
      _StartClassifierBehaviorAction.visibility = "visibility";
      _StartClassifierBehaviorAction.ownedComment = "ownedComment";
      _StartClassifierBehaviorAction.ownedElement = "ownedElement";
      _StartClassifierBehaviorAction.owner = "owner";
      _Actions3._StartClassifierBehaviorAction = _StartClassifierBehaviorAction;
      _Actions3.__StartClassifierBehaviorAction_Uri = "dm:///_internal/model/uml#StartClassifierBehaviorAction";
      class _StartObjectBehaviorAction {
      }
      _StartObjectBehaviorAction.object = "object";
      _StartObjectBehaviorAction.isSynchronous = "isSynchronous";
      _StartObjectBehaviorAction.result = "result";
      _StartObjectBehaviorAction.argument = "argument";
      _StartObjectBehaviorAction.onPort = "onPort";
      _StartObjectBehaviorAction.context = "context";
      _StartObjectBehaviorAction.input = "input";
      _StartObjectBehaviorAction.isLocallyReentrant = "isLocallyReentrant";
      _StartObjectBehaviorAction.localPostcondition = "localPostcondition";
      _StartObjectBehaviorAction.localPrecondition = "localPrecondition";
      _StartObjectBehaviorAction.output = "output";
      _StartObjectBehaviorAction.handler = "handler";
      _StartObjectBehaviorAction.activity = "activity";
      _StartObjectBehaviorAction.inGroup = "inGroup";
      _StartObjectBehaviorAction.inInterruptibleRegion = "inInterruptibleRegion";
      _StartObjectBehaviorAction.inPartition = "inPartition";
      _StartObjectBehaviorAction.inStructuredNode = "inStructuredNode";
      _StartObjectBehaviorAction.incoming = "incoming";
      _StartObjectBehaviorAction.outgoing = "outgoing";
      _StartObjectBehaviorAction.redefinedNode = "redefinedNode";
      _StartObjectBehaviorAction.isLeaf = "isLeaf";
      _StartObjectBehaviorAction.redefinedElement = "redefinedElement";
      _StartObjectBehaviorAction.redefinitionContext = "redefinitionContext";
      _StartObjectBehaviorAction.clientDependency = "clientDependency";
      _StartObjectBehaviorAction._name_ = "name";
      _StartObjectBehaviorAction.nameExpression = "nameExpression";
      _StartObjectBehaviorAction.namespace = "namespace";
      _StartObjectBehaviorAction.qualifiedName = "qualifiedName";
      _StartObjectBehaviorAction.visibility = "visibility";
      _StartObjectBehaviorAction.ownedComment = "ownedComment";
      _StartObjectBehaviorAction.ownedElement = "ownedElement";
      _StartObjectBehaviorAction.owner = "owner";
      _Actions3._StartObjectBehaviorAction = _StartObjectBehaviorAction;
      _Actions3.__StartObjectBehaviorAction_Uri = "dm:///_internal/model/uml#StartObjectBehaviorAction";
      class _StructuralFeatureAction {
      }
      _StructuralFeatureAction.object = "object";
      _StructuralFeatureAction.structuralFeature = "structuralFeature";
      _StructuralFeatureAction.context = "context";
      _StructuralFeatureAction.input = "input";
      _StructuralFeatureAction.isLocallyReentrant = "isLocallyReentrant";
      _StructuralFeatureAction.localPostcondition = "localPostcondition";
      _StructuralFeatureAction.localPrecondition = "localPrecondition";
      _StructuralFeatureAction.output = "output";
      _StructuralFeatureAction.handler = "handler";
      _StructuralFeatureAction.activity = "activity";
      _StructuralFeatureAction.inGroup = "inGroup";
      _StructuralFeatureAction.inInterruptibleRegion = "inInterruptibleRegion";
      _StructuralFeatureAction.inPartition = "inPartition";
      _StructuralFeatureAction.inStructuredNode = "inStructuredNode";
      _StructuralFeatureAction.incoming = "incoming";
      _StructuralFeatureAction.outgoing = "outgoing";
      _StructuralFeatureAction.redefinedNode = "redefinedNode";
      _StructuralFeatureAction.isLeaf = "isLeaf";
      _StructuralFeatureAction.redefinedElement = "redefinedElement";
      _StructuralFeatureAction.redefinitionContext = "redefinitionContext";
      _StructuralFeatureAction.clientDependency = "clientDependency";
      _StructuralFeatureAction._name_ = "name";
      _StructuralFeatureAction.nameExpression = "nameExpression";
      _StructuralFeatureAction.namespace = "namespace";
      _StructuralFeatureAction.qualifiedName = "qualifiedName";
      _StructuralFeatureAction.visibility = "visibility";
      _StructuralFeatureAction.ownedComment = "ownedComment";
      _StructuralFeatureAction.ownedElement = "ownedElement";
      _StructuralFeatureAction.owner = "owner";
      _Actions3._StructuralFeatureAction = _StructuralFeatureAction;
      _Actions3.__StructuralFeatureAction_Uri = "dm:///_internal/model/uml#StructuralFeatureAction";
      class _StructuredActivityNode {
      }
      _StructuredActivityNode.activity = "activity";
      _StructuredActivityNode.edge = "edge";
      _StructuredActivityNode.mustIsolate = "mustIsolate";
      _StructuredActivityNode.node = "node";
      _StructuredActivityNode.structuredNodeInput = "structuredNodeInput";
      _StructuredActivityNode.structuredNodeOutput = "structuredNodeOutput";
      _StructuredActivityNode.variable = "variable";
      _StructuredActivityNode.elementImport = "elementImport";
      _StructuredActivityNode.importedMember = "importedMember";
      _StructuredActivityNode.member = "member";
      _StructuredActivityNode.ownedMember = "ownedMember";
      _StructuredActivityNode.ownedRule = "ownedRule";
      _StructuredActivityNode.packageImport = "packageImport";
      _StructuredActivityNode.clientDependency = "clientDependency";
      _StructuredActivityNode._name_ = "name";
      _StructuredActivityNode.nameExpression = "nameExpression";
      _StructuredActivityNode.namespace = "namespace";
      _StructuredActivityNode.qualifiedName = "qualifiedName";
      _StructuredActivityNode.visibility = "visibility";
      _StructuredActivityNode.ownedComment = "ownedComment";
      _StructuredActivityNode.ownedElement = "ownedElement";
      _StructuredActivityNode.owner = "owner";
      _StructuredActivityNode.containedEdge = "containedEdge";
      _StructuredActivityNode.containedNode = "containedNode";
      _StructuredActivityNode.inActivity = "inActivity";
      _StructuredActivityNode.subgroup = "subgroup";
      _StructuredActivityNode.superGroup = "superGroup";
      _StructuredActivityNode.context = "context";
      _StructuredActivityNode.input = "input";
      _StructuredActivityNode.isLocallyReentrant = "isLocallyReentrant";
      _StructuredActivityNode.localPostcondition = "localPostcondition";
      _StructuredActivityNode.localPrecondition = "localPrecondition";
      _StructuredActivityNode.output = "output";
      _StructuredActivityNode.handler = "handler";
      _StructuredActivityNode.inGroup = "inGroup";
      _StructuredActivityNode.inInterruptibleRegion = "inInterruptibleRegion";
      _StructuredActivityNode.inPartition = "inPartition";
      _StructuredActivityNode.inStructuredNode = "inStructuredNode";
      _StructuredActivityNode.incoming = "incoming";
      _StructuredActivityNode.outgoing = "outgoing";
      _StructuredActivityNode.redefinedNode = "redefinedNode";
      _StructuredActivityNode.isLeaf = "isLeaf";
      _StructuredActivityNode.redefinedElement = "redefinedElement";
      _StructuredActivityNode.redefinitionContext = "redefinitionContext";
      _Actions3._StructuredActivityNode = _StructuredActivityNode;
      _Actions3.__StructuredActivityNode_Uri = "dm:///_internal/model/uml#StructuredActivityNode";
      class _TestIdentityAction {
      }
      _TestIdentityAction.first = "first";
      _TestIdentityAction.result = "result";
      _TestIdentityAction.second = "second";
      _TestIdentityAction.context = "context";
      _TestIdentityAction.input = "input";
      _TestIdentityAction.isLocallyReentrant = "isLocallyReentrant";
      _TestIdentityAction.localPostcondition = "localPostcondition";
      _TestIdentityAction.localPrecondition = "localPrecondition";
      _TestIdentityAction.output = "output";
      _TestIdentityAction.handler = "handler";
      _TestIdentityAction.activity = "activity";
      _TestIdentityAction.inGroup = "inGroup";
      _TestIdentityAction.inInterruptibleRegion = "inInterruptibleRegion";
      _TestIdentityAction.inPartition = "inPartition";
      _TestIdentityAction.inStructuredNode = "inStructuredNode";
      _TestIdentityAction.incoming = "incoming";
      _TestIdentityAction.outgoing = "outgoing";
      _TestIdentityAction.redefinedNode = "redefinedNode";
      _TestIdentityAction.isLeaf = "isLeaf";
      _TestIdentityAction.redefinedElement = "redefinedElement";
      _TestIdentityAction.redefinitionContext = "redefinitionContext";
      _TestIdentityAction.clientDependency = "clientDependency";
      _TestIdentityAction._name_ = "name";
      _TestIdentityAction.nameExpression = "nameExpression";
      _TestIdentityAction.namespace = "namespace";
      _TestIdentityAction.qualifiedName = "qualifiedName";
      _TestIdentityAction.visibility = "visibility";
      _TestIdentityAction.ownedComment = "ownedComment";
      _TestIdentityAction.ownedElement = "ownedElement";
      _TestIdentityAction.owner = "owner";
      _Actions3._TestIdentityAction = _TestIdentityAction;
      _Actions3.__TestIdentityAction_Uri = "dm:///_internal/model/uml#TestIdentityAction";
      class _UnmarshallAction {
      }
      _UnmarshallAction.object = "object";
      _UnmarshallAction.result = "result";
      _UnmarshallAction.unmarshallType = "unmarshallType";
      _UnmarshallAction.context = "context";
      _UnmarshallAction.input = "input";
      _UnmarshallAction.isLocallyReentrant = "isLocallyReentrant";
      _UnmarshallAction.localPostcondition = "localPostcondition";
      _UnmarshallAction.localPrecondition = "localPrecondition";
      _UnmarshallAction.output = "output";
      _UnmarshallAction.handler = "handler";
      _UnmarshallAction.activity = "activity";
      _UnmarshallAction.inGroup = "inGroup";
      _UnmarshallAction.inInterruptibleRegion = "inInterruptibleRegion";
      _UnmarshallAction.inPartition = "inPartition";
      _UnmarshallAction.inStructuredNode = "inStructuredNode";
      _UnmarshallAction.incoming = "incoming";
      _UnmarshallAction.outgoing = "outgoing";
      _UnmarshallAction.redefinedNode = "redefinedNode";
      _UnmarshallAction.isLeaf = "isLeaf";
      _UnmarshallAction.redefinedElement = "redefinedElement";
      _UnmarshallAction.redefinitionContext = "redefinitionContext";
      _UnmarshallAction.clientDependency = "clientDependency";
      _UnmarshallAction._name_ = "name";
      _UnmarshallAction.nameExpression = "nameExpression";
      _UnmarshallAction.namespace = "namespace";
      _UnmarshallAction.qualifiedName = "qualifiedName";
      _UnmarshallAction.visibility = "visibility";
      _UnmarshallAction.ownedComment = "ownedComment";
      _UnmarshallAction.ownedElement = "ownedElement";
      _UnmarshallAction.owner = "owner";
      _Actions3._UnmarshallAction = _UnmarshallAction;
      _Actions3.__UnmarshallAction_Uri = "dm:///_internal/model/uml#UnmarshallAction";
      class _ValuePin {
      }
      _ValuePin.value = "value";
      _ValuePin.isControl = "isControl";
      _ValuePin.inState = "inState";
      _ValuePin.isControlType = "isControlType";
      _ValuePin.ordering = "ordering";
      _ValuePin.selection = "selection";
      _ValuePin.upperBound = "upperBound";
      _ValuePin.type = "type";
      _ValuePin.clientDependency = "clientDependency";
      _ValuePin._name_ = "name";
      _ValuePin.nameExpression = "nameExpression";
      _ValuePin.namespace = "namespace";
      _ValuePin.qualifiedName = "qualifiedName";
      _ValuePin.visibility = "visibility";
      _ValuePin.ownedComment = "ownedComment";
      _ValuePin.ownedElement = "ownedElement";
      _ValuePin.owner = "owner";
      _ValuePin.activity = "activity";
      _ValuePin.inGroup = "inGroup";
      _ValuePin.inInterruptibleRegion = "inInterruptibleRegion";
      _ValuePin.inPartition = "inPartition";
      _ValuePin.inStructuredNode = "inStructuredNode";
      _ValuePin.incoming = "incoming";
      _ValuePin.outgoing = "outgoing";
      _ValuePin.redefinedNode = "redefinedNode";
      _ValuePin.isLeaf = "isLeaf";
      _ValuePin.redefinedElement = "redefinedElement";
      _ValuePin.redefinitionContext = "redefinitionContext";
      _ValuePin.isOrdered = "isOrdered";
      _ValuePin.isUnique = "isUnique";
      _ValuePin.lower = "lower";
      _ValuePin.lowerValue = "lowerValue";
      _ValuePin.upper = "upper";
      _ValuePin.upperValue = "upperValue";
      _Actions3._ValuePin = _ValuePin;
      _Actions3.__ValuePin_Uri = "dm:///_internal/model/uml#ValuePin";
    })(_Actions2 = _UML2._Actions || (_UML2._Actions = {}));
  })(_UML || (_UML = {}));

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/SubElementField.js
  var Control = class {
    /**
     * Initializes the container that holds the rendered subelement list and controls.
     */
    constructor(context) {
      this._list = $("<div></div>");
      this.isReadOnly = context.isReadOnly;
      this.itemUrl = context.itemUrl ?? "";
      this.form = context.form;
      if (context.configuration !== void 0) {
        this.configuration = context.configuration;
      }
    }
    /**
     * Creates the DOM for the subelement list based on the given field value.
     *
     * Relevant steps:
     * - For read-only mode, render a simple list with optional click actions.
     * - For edit mode, render a table with fields and row actions (delete / move).
     * - Attach creation/attach and refresh actions and return the root element.
     *
     * @param fieldValue The current property value, expected to be an array of referenced subelements.
     */
    async createDomByFieldValue(fieldValue) {
      const tthis = this;
      this._list.empty();
      if (this.isReadOnly) {
        if (!Array.isArray(fieldValue)) {
          this._list.add($("<div><em>Element is not an Array</em></div>"));
        } else {
          let ul = $("<ul class='list-unstyled'></ul>");
          let foundElements = 0;
          for (let m in fieldValue) {
            if (Object.prototype.hasOwnProperty.call(fieldValue, m)) {
              let innerValue = fieldValue[m];
              const item = $("<li></li>");
              const injectParams = {};
              if (this.itemActionName !== void 0 && this.itemActionName !== null) {
                injectParams.onClick = async (x) => {
                  const readObject = await getObjectByUri(x.workspace, x.uri);
                  await execute(this.itemActionName, tthis.form, readObject);
                  return false;
                };
              }
              let _ = injectNameByUri(item, innerValue.workspace, innerValue.uri, injectParams);
              ul.append(item);
              foundElements++;
            }
          }
          if (foundElements === 0) {
            ul = $("<em>No items</em>");
          }
          this._list.append(ul);
        }
      } else {
        if (!Array.isArray(fieldValue)) {
          fieldValue = [];
        }
        const table = $("<table><tbody></tbody></table>");
        this._list.append(table);
        let fields = this.getFieldDefinitions();
        let fieldsData = fields ?? new Array();
        if (fields === void 0) {
          const nameField = new DmObject();
          nameField.setMetaClassByUri(_Forms._FieldTypes.__TextFieldData_Uri, "Types");
          nameField.set("name", "name");
          nameField.set("title", "Name");
          nameField.set("isReadOnly", true);
          fieldsData.push(nameField);
        }
        const tBody = $("tbody", table);
        const tr = $("<tr></tr>");
        for (let fieldDataKey in fieldsData) {
          let fieldData = fieldsData[fieldDataKey];
          let header = $("<th></th>");
          header.text(fieldData.get("title"));
          tr.append(header);
        }
        let deleteHeader = $("<th>Actions</th>");
        tr.append(deleteHeader);
        tBody.append(tr);
        for (let m in fieldValue) {
          if (Object.prototype.hasOwnProperty.call(fieldValue, m)) {
            const tr2 = $("<tr></tr>");
            let innerValue = fieldValue[m];
            for (let fieldDataKey in fieldsData) {
              const td = $("<td></td>");
              let fieldData = fieldsData[fieldDataKey];
              const field = createField({
                field: fieldData,
                isReadOnly: true,
                itemUrl: innerValue.uri,
                configuration: {
                  formType: this.configuration?.formType ?? FormType.Object,
                  isReadOnly: false,
                  formElement: tthis.form.formElement
                },
                form: tthis.form
              });
              const dom = await field.createDom(innerValue);
              td.append(dom);
              tr2.append(td);
            }
            const moveUp = $("<btn class='btn btn-secondary dm-item-moveup-button'>\u2B06\uFE0F</btn>");
            const moveDown = $("<btn class='btn btn-secondary dm-item-movedown-button'>\u2B07\uFE0F</btn>");
            moveUp.on("click", async () => {
              await moveItemInCollectionUp(this.form.workspace, this.itemUrl, this.propertyName, innerValue.uri);
              await this.reloadValuesFromServer();
            });
            moveDown.on("click", async () => {
              await moveItemInCollectionDown(this.form.workspace, this.itemUrl, this.propertyName, innerValue.uri);
              await this.reloadValuesFromServer();
            });
            let deleteCell = $("<td><btn class='btn btn-secondary'>Delete</btn></td>");
            const deleteButton = $("btn", deleteCell);
            deleteButton.on("click", () => {
              if (deleteButton.attr("data-confirmdelete") !== "1") {
                deleteButton.text("Sure?");
                deleteButton.attr("data-confirmdelete", "1");
              } else {
                removeReferenceFromCollection(tthis.form.workspace, tthis.itemUrl, {
                  property: tthis.propertyName,
                  referenceUri: innerValue.uri,
                  referenceWorkspaceId: innerValue.workspace
                }).then(() => {
                  tthis.reloadValuesFromServer();
                });
              }
            });
            tr2.append(deleteCell);
            deleteCell.append(moveUp);
            deleteCell.append(moveDown);
            table.append(tr2);
          }
        }
      }
      if (this.configuration?.formType !== FormType.Collection) {
        await this.createAndAttachModificationButtons(tthis);
      }
      const refreshBtn = $("<div><btn class='dm-subelements-refresh-btn'><img src='/img/refresh-16.png' alt='Refresh' /></btn></div>");
      $(".dm-subelements-refresh-btn", refreshBtn).on("click", () => {
        tthis.reloadValuesFromServer();
      });
      this._list.append(refreshBtn);
      return this._list;
    }
    /**
     * Creates and attaches controls for attaching existing items and creating new subelements.
     *
     * Relevant steps:
     * - Add an attach button that opens a selector and appends references to the collection.
     * - Add a create button that navigates to item creation based on selected type.
     * - Render optional shortcut buttons for preconfigured additional creation types.
     *
     * @param tthis Captured instance reference used inside asynchronous callbacks.
     */
    async createAndAttachModificationButtons(tthis) {
      const attachItem = $("<div><div><btn class='btn btn-secondary dm-subelements-attachitem-btn'>Attach</btn><btn class='btn btn-secondary dm-subelements-createitem-btn'>Create</btn><span class='dm-subelements-createadditional'></span></div><div class='dm-subelements-attachitem-box'></div><div class='dm-subelements-createitem-box'></div></div>");
      $(".dm-subelements-attachitem-btn", attachItem).on("click", async () => {
        const containerDiv = $(".dm-subelements-attachitem-box", attachItem);
        containerDiv.empty();
        const selectItem2 = new SelectItemControl();
        const settings = new ContainerSettings();
        settings.browseSettings.showWorkspaceInBreadcrumb = true;
        settings.browseSettings.showExtentInBreadcrumb = true;
        selectItem2.itemSelected.addListener((selectedItem) => {
          addReferenceToCollection(tthis.form.workspace, tthis.itemUrl, {
            property: tthis.propertyName,
            referenceUri: selectedItem.uri,
            workspaceId: selectItem2.getCurrentlySelectedWorkspace()
          }).then(() => {
            this.reloadValuesFromServer();
          });
        });
        await selectItem2.initAsync(containerDiv, settings);
        return false;
      });
      $(".dm-subelements-createitem-btn", attachItem).on("click", async () => {
        const container = $(".dm-subelements-createitem-box", attachItem);
        container.empty();
        const control = new TypeSelectionControl(container);
        if (this.propertyType !== void 0) {
          control.setCurrentTypeUrl(this.propertyType);
        }
        control.typeSelected.addListener(async (x) => {
          if (x === void 0 || x.selectedType === void 0 || x.selectedType.uri === void 0) {
            alert("Nothing is selected.");
            return;
          }
          document.location.href = getLinkForNavigateToCreateItemInProperty(tthis.form.workspace, tthis.itemUrl, tthis.propertyName, x.selectedType.uri, x.selectedType.workspace);
        });
        await control.createControl();
      });
      const containerAdditional = $(".dm-subelements-createadditional", attachItem);
      if (this.additionalTypes !== void 0) {
        for (const m in this.additionalTypes) {
          const additionalTypeTemp = this.additionalTypes[m];
          let additionalType = await resolve(additionalTypeTemp);
          if (additionalType === void 0)
            continue;
          let name;
          let metaClassUri;
          let metaClassWorkspace;
          if (additionalType.metaClass?.uri === _Forms.__DefaultTypeForNewElement_Uri) {
            name = additionalType.get(_Forms._DefaultTypeForNewElement._name_, ObjectType.String);
            const metaClass = await resolve(additionalType.get(_Forms._DefaultTypeForNewElement.metaClass, ObjectType.Object));
            metaClassUri = metaClass.uri;
            metaClassWorkspace = metaClass.workspace;
            name ?? (name = metaClass.get(_UML._CommonStructure._NamedElement._name_, ObjectType.String));
          } else {
            name = additionalType.get(_UML._CommonStructure._NamedElement._name_, ObjectType.String);
            metaClassUri = additionalType.uri;
            metaClassWorkspace = additionalType.workspace;
          }
          const buttonAdditionalType = $("<button type='button' class='btn btn-secondary'></button>");
          buttonAdditionalType.text("Create " + name);
          buttonAdditionalType.on("click", () => {
            document.location.href = getLinkForNavigateToCreateItemInProperty(tthis.form.workspace, tthis.itemUrl, tthis.propertyName, metaClassUri, metaClassWorkspace);
          });
          containerAdditional.append(buttonAdditionalType);
        }
      }
      this._list.append(attachItem);
    }
    /**
     * Reloads subelement values from server-side data.
     * Must be overridden by concrete field implementations.
     */
    reloadValuesFromServer() {
      alert("reloadValuesFromServer is not overridden.");
    }
    /**
     * Returns the field definitions used for rendering each row column.
     * Can be overridden by implementations that provide custom field definitions.
     */
    getFieldDefinitions() {
      return void 0;
    }
  };
  var Field10 = class extends Control {
    get field() {
      return this.context.field;
    }
    constructor(context) {
      super(context);
      this.context = context;
    }
    /**
     * Reloads the current property from the backend and re-renders the subelement list.
     */
    reloadValuesFromServer() {
      const tthis = this;
      const url = this._element.uri;
      getProperty(this.form.workspace, url, this.propertyName).then((x) => tthis.createDomByFieldValue(x));
    }
    /**
     * Resolves row field definitions from the current form definition.
     */
    getFieldDefinitions() {
      return this.field.get("form", ObjectType.Single)?.get("field", ObjectType.Array);
    }
    /**
     * Creates the subelement field DOM for the provided element.
     *
     * Relevant steps:
     * - Read field configuration (property name, action, additional default types).
     * - For new items, return a hint because subelements cannot be edited yet.
     * - For persisted items, optionally resolve property type metadata and render values.
     *
     * @param dmElement The element for which this field is rendered.
     */
    async createDom(dmElement) {
      this.propertyName = this.field.get(_Forms._FieldTypes._SubElementFieldData._name_, ObjectType.String);
      this.itemActionName = this.field.get(_Forms._FieldTypes._SubElementFieldData.actionName, ObjectType.String);
      this.additionalTypes = this.field.get(_Forms._FieldTypes._SubElementFieldData.defaultTypesForNewElements, ObjectType.Array);
      if (this.configuration?.isNewItem) {
        return $("<div><em>New item, content can only be set after saving</em></div>");
      }
      this._element = dmElement;
      const value = dmElement.get(this.propertyName);
      if (this._element.metaClass?.uri !== void 0 && this.propertyName !== void 0 && !this.isReadOnly) {
        this.propertyType = await getPropertyType(this._element.metaClass.workspace, this._element.metaClass.uri, this.propertyName);
      }
      await this.createDomByFieldValue(value);
      return this._list;
    }
    /**
     * Evaluates and persists DOM changes back to the model.
     * This field currently performs all write operations immediately via UI actions,
     * therefore no additional evaluation step is required here.
     *
     * @param dmElement The element whose field value would be evaluated.
     */
    async evaluateDom(dmElement) {
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/ReferenceField.js
  var Control2 = class extends BaseField {
    /** Initializes a new instance
     *
     * @param context The render context according IFieldRenderContext.
     * */
    constructor(context) {
      super(context);
      this._list = $("<span></span>");
    }
    /** Creates the overall DOM-elements by getting the object */
    async createDomByValue(value) {
      this._list.empty();
      const tthis = this;
      const asMofDmObject = value;
      if (this.configuration?.isNewItem) {
        return $("<div><em>New item, content can only be set after saving</em></div>");
      }
      const isSelectionInline = this.field?.get("isSelectionInline", ObjectType.Boolean);
      if (!isSelectionInline) {
        if (typeof value !== "object" && typeof value !== "function" || value === null || value === void 0) {
          const div = $("<div><em class='dm-undefined'>undefined</em></div>");
          this._list.append(div);
        } else {
          const div = $("<div />");
          let _ = injectNameByUri(div, asMofDmObject.workspace, asMofDmObject.uri);
          this._list.append(div);
        }
      }
      if (!this.isReadOnly) {
        const containerChangeCell = $("<div></div>");
        if (!(this.inhibitInline !== true && isSelectionInline)) {
          const createCell = $("<btn class='btn btn-secondary'>Create</btn>");
          const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
          const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
          createCell.on("click", async () => {
            const packageItem = await selectPackage(containerChangeCell);
            const typeItem = await selectType(containerChangeCell);
            const result = await addToContainer(packageItem.workspace, packageItem.uri, {
              metaClass: typeItem.uri
            });
            if (tthis.referenceSetCall !== void 0) {
              await this.referenceSetCall({
                workspace: result.workspace,
                uri: result.itemUri
              });
            }
            await setPropertyReference(tthis.form.workspace, tthis.itemUrl, {
              property: tthis.propertyName,
              referenceUri: result.itemUri,
              workspaceId: result.workspace
            });
            await tthis.reloadValuesFromServer();
            return false;
          });
          changeCell.on("click", async () => {
            await this.createSelectFields(containerChangeCell, value);
            return false;
          });
          unsetCell.on("click", () => {
            unsetProperty(tthis.form.workspace, tthis.itemUrl, tthis.propertyName).then(async () => {
              await tthis.reloadValuesFromServer();
            });
          });
          this._list.append(createCell);
          this._list.append(changeCell);
          this._list.append(unsetCell);
        } else {
          await this.createSelectFields(containerChangeCell, value);
        }
        this._list.append(containerChangeCell);
      }
      return this._list;
    }
    /** Creates the GUI elements in which the user is capable to select the items to be reference
     * @param containerChangeCell The cell which will contain the GUI elements. This cell will be emptied
     * @param value The value that is currently selected*/
    async createSelectFields(containerChangeCell, value) {
      const tthis = this;
      containerChangeCell.empty();
      const isSelectionInline = this.field?.get("isSelectionInline", ObjectType.Boolean);
      const selectItem2 = new SelectItemControl();
      const settings = new ContainerSettings();
      settings.browseSettings.showWorkspaceInBreadcrumb = true;
      settings.browseSettings.showExtentInBreadcrumb = true;
      settings.hideButtonRow = isSelectionInline;
      const eventType = isSelectionInline ? selectItem2.itemClicked : selectItem2.itemSelected;
      eventType.addListener(async (selectedItem) => {
        if (tthis.referenceSetCall !== void 0) {
          await this.referenceSetCall(selectedItem);
        }
        await setPropertyReference(tthis.form.workspace, tthis.itemUrl, {
          property: tthis.propertyName,
          referenceUri: selectedItem.uri,
          workspaceId: selectedItem.workspace
        });
        if (this.callbackUpdateField !== void 0) {
          this.callbackUpdateField();
        }
        if (!isSelectionInline) {
          containerChangeCell.empty();
          tthis.inhibitInline = true;
          await this.reloadValuesFromServer();
        }
      });
      await selectItem2.initAsync(containerChangeCell, settings);
      if (value !== void 0 && value !== null && (typeof value === "object" || typeof value === "function")) {
        const valueAsMofDmObject = value;
        await selectItem2.setItemByUri(valueAsMofDmObject.workspace, valueAsMofDmObject.uri);
      } else {
        const workspaceId = this.field?.get("defaultWorkspace", ObjectType.Single);
        const itemUri = this.field?.get("defaultItemUri", ObjectType.Single);
        if (workspaceId !== void 0 && workspaceId !== null) {
          if (itemUri === null || itemUri === void 0) {
            await selectItem2.setWorkspaceById(workspaceId);
          } else {
            await selectItem2.setItemByUri(workspaceId, itemUri);
          }
        } else if (this.form.workspace !== void 0 && this.form.extentUri !== void 0) {
          await selectItem2.setExtentByUri(this.form.workspace, this.form.extentUri);
        }
      }
    }
    async reloadValuesFromServer() {
      alert("reloadValuesFromServer is not overridden.");
    }
  };
  var Field11 = class extends Control2 {
    constructor(context) {
      super(context);
    }
    async createDom(dmElement) {
      this.element = dmElement;
      this.referenceSetCall = this.callbackSetReference;
      this._list.empty();
      this.fieldName = this.field.get("name");
      let value = dmElement.get(this.fieldName);
      if (Array.isArray(value)) {
        if (value.length === 1) {
          value = value[0];
        } else {
          this._list.append($("<em>The value is an array and not supported by the referencefield</em>"));
          return this._list;
        }
      }
      this.propertyName = this.fieldName;
      if (this.isReadOnly === true) {
        if (value === void 0 || value === null) {
          this._list.html("<em class='dm-undefined'>undefined</em>");
        } else if (value.get === void 0) {
          this._list.text(value.toString());
        } else {
          await injectNameByObject(this._list, value);
        }
      } else {
        return await this.createDomByValue(value);
      }
      return this._list;
    }
    async callbackSetReference(selectedElement) {
      this.element.set(this.fieldName, DmObject.createFromItemWithNameAndId(selectedElement));
      if (this.callbackUpdateField !== void 0) {
        this.callbackUpdateField();
      }
    }
    async evaluateDom(dmElement) {
    }
    async reloadValuesFromServer() {
      let value = await getProperty(this.form.workspace, this.element.uri, this.fieldName);
      if (Array.isArray(value)) {
        if (value.length === 1) {
          value = value[0];
        } else {
          this._list.empty();
          this._list.append($("<em>The value is an array and not supported by the referencefield</em>"));
          return;
        }
      }
      this.element.set(this.fieldName, value);
      await this.createDomByValue(value);
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/AnyDataField.js
  var ModeValue;
  (function(ModeValue2) {
    ModeValue2[ModeValue2["Value"] = 0] = "Value";
    ModeValue2[ModeValue2["Collection"] = 1] = "Collection";
    ModeValue2[ModeValue2["Reference"] = 2] = "Reference";
  })(ModeValue || (ModeValue = {}));
  var Field12 = class extends BaseField {
    constructor(context) {
      super(context);
    }
    // Creates the overall DOM
    async createDom(dmElement) {
      if (this.configuration?.isNewItem) {
        return $("<div><em>New item, content can only be set after saving</em></div>");
      }
      const tthis = this;
      this._element = dmElement;
      const result = $("<div>");
      const headLine = $("<div class='dm-anydatafield-headline'><a class='dm-anydatafield-headline-value'>Value</a> | <a class='dm-anydatafield-headline-collection'>Collection</a> | <a class='dm-anydatafield-headline-reference'>Reference</a></div>");
      this._aValue = $(".dm-anydatafield-headline-value", headLine);
      this._aCollection = $(".dm-anydatafield-headline-collection", headLine);
      this._aReference = $(".dm-anydatafield-headline-reference", headLine);
      this._domElement = $("<div></div>");
      this._aValue.on("click", () => {
        tthis.highlightValue();
      });
      this._aCollection.on("click", () => {
        tthis.highlightCollection();
      });
      this._aReference.on("click", () => {
        tthis.highlightReference();
      });
      result.append(headLine);
      result.append(this._domElement);
      const fieldName = this.field.get("name").toString();
      this._fieldValue = this._element.get(fieldName);
      if (this._fieldValue === null || this._fieldValue === void 0) {
        this.highlightReference();
      } else if (typeof this._fieldValue === "object" || typeof this._fieldValue === "function") {
        this.highlightReference();
      } else {
        this.highlightValue();
      }
      return result;
    }
    async evaluateDom(dmElement) {
      if (this._mode === ModeValue.Value) {
        if (this._textBox !== void 0 && this._textBox !== null) {
          const fieldName = this.field.get("name").toString();
          dmElement.set(fieldName, this._textBox.val());
        }
      }
    }
    /*
     * These helper methods are to be called when one of the three types were selected and
     * shall be switched to
     */
    highlightValue() {
      this._aValue.addClass("active");
      this._aCollection.removeClass("active");
      this._aReference.removeClass("active");
      this._mode = ModeValue.Value;
      this.updateDomContent();
    }
    highlightCollection() {
      this._aValue.removeClass("active");
      this._aCollection.addClass("active");
      this._aReference.removeClass("active");
      this._mode = ModeValue.Collection;
      this.updateDomContent();
    }
    highlightReference() {
      this._aValue.removeClass("active");
      this._aCollection.removeClass("active");
      this._aReference.addClass("active");
      this._mode = ModeValue.Reference;
      this.updateDomContent();
    }
    async reloadAndUpdateDomContent() {
      const tthis = this;
      tthis._fieldValue = await getProperty(this.form.workspace, this.itemUrl, this.field.get("name").toString());
      await tthis.updateDomContent();
    }
    // Performs a 'reload' of the complete DOM
    async updateDomContent() {
      this._domElement.empty();
      if (this.isReadOnly) {
        await this.updateDomContentReadOnly();
      } else {
        await this.updateDomContentEditable();
      }
    }
    // Rebuilds the BOM in the read-only mode
    async updateDomContentReadOnly() {
      let value = this._fieldValue;
      if (value === null || value === void 0 || this._mode === ModeValue.Reference && (typeof value !== "object" && typeof value !== "function")) {
        const div = $("<div><em class='dm-undefined'>Undefined</em></div>");
        this._domElement.append(div);
      } else if (this._mode === ModeValue.Reference) {
        if (Array.isArray(value)) {
          value = value[0];
        }
        const field = this.createReferenceFieldInstance();
        const element = await field.createDomByValue(value);
        this._domElement.append(element);
      } else if (this._mode === ModeValue.Value) {
        const div = $("<div />");
        div.text(value?.toString() ?? "");
        this._domElement.append(div);
      } else if (this._mode === ModeValue.Collection) {
        const field = this.createSubElementFieldInstance();
        const element = await field.createDomByFieldValue(value);
        this._domElement.append(element);
      }
    }
    // Rebuilds the DOM in the edit mode
    async updateDomContentEditable() {
      if (this._mode === ModeValue.Reference) {
        const tthis = this;
        let value = this._fieldValue;
        if (Array.isArray(value)) {
          value = value[0];
        }
        const fieldName = this.field.get("name").toString();
        if (typeof value !== "object" && typeof value !== "function" || value === null || value === void 0) {
          const div = $("<div><em class='dm-undefined'>undefined</em></null>");
          this._domElement.append(div);
        } else {
          const asDmObject = value;
          const div = $("<div />");
          injectNameByUri(div, asDmObject.workspace, asDmObject.uri);
          this._domElement.append(div);
        }
        if (this.configuration?.isNewItem) {
          const div = $("<em>Element needs to be saved first</em>");
          this._domElement.append(div);
        } else {
          const changeCell = $("<btn class='btn btn-secondary'>Change</btn>");
          const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");
          const containerChangeCell = $("<div></div>");
          unsetCell.on("click", async () => {
            await unsetProperty(tthis.form.workspace, tthis.itemUrl, fieldName);
            tthis.updateDomContent();
          });
          changeCell.on("click", async () => {
            containerChangeCell.empty();
            const selectItem2 = new SelectItemControl();
            const settings = new ContainerSettings();
            settings.browseSettings.showWorkspaceInBreadcrumb = true;
            settings.browseSettings.showExtentInBreadcrumb = true;
            settings.hideAtStartup = true;
            selectItem2.itemSelected.addListener(async (selectedItem) => {
              await setPropertyReference(tthis.form.workspace, tthis.itemUrl, {
                property: tthis.field.get("name"),
                referenceUri: selectedItem.uri,
                workspaceId: selectItem2.getCurrentlySelectedWorkspace()
              });
              if (tthis.callbackUpdateField !== void 0 && tthis.callbackUpdateField !== null)
                tthis.callbackUpdateField();
              await tthis.reloadAndUpdateDomContent();
            });
            containerChangeCell.empty();
            await selectItem2.initAsync(containerChangeCell, settings);
            if (value?.workspace !== void 0 && value.uri !== void 0) {
              await selectItem2.setItemByUri(value.workspace, value.uri);
            } else {
              if (value?.workspace !== void 0) {
                await selectItem2.setWorkspaceById(value.workspace);
              } else if (this._element?.workspace !== void 0) {
                await selectItem2.setWorkspaceById(tthis._element.workspace);
              }
              if (value?.extentUri !== void 0) {
                await selectItem2.setWorkspaceById(value.workspace);
              } else if (this._element?.extentUri !== void 0 && this._element.workspace !== void 0) {
                await selectItem2.setExtentByUri(this._element.workspace, tthis._element.extentUri);
              }
            }
            selectItem2.showControl();
            return false;
          });
          this._domElement.append(changeCell);
          this._domElement.append(unsetCell);
          this._domElement.append(containerChangeCell);
        }
      } else if (this._mode === ModeValue.Value) {
        const value = this._fieldValue;
        this._textBox = $("<input />");
        this._textBox.val(value?.toString() ?? "");
        this._textBox.on("input change", () => {
          if (this.callbackUpdateField !== void 0 && this.callbackUpdateField !== null)
            this.callbackUpdateField();
        });
        this._domElement.append(this._textBox);
      } else if (this._mode === ModeValue.Collection) {
        const value = this._fieldValue;
        if (this.configuration?.isNewItem) {
          const div = $("<em>Element needs to be saved first</em>");
          this._domElement.append(div);
        } else {
          const field = this.createSubElementFieldInstance();
          const element = await field.createDomByFieldValue(value);
          this._domElement.append(element);
        }
      }
    }
    createReferenceFieldInstance() {
      const element = new Field11({
        field: this.field,
        isReadOnly: this.isReadOnly,
        itemUrl: this.itemUrl,
        form: this.form,
        configuration: this.configuration
      });
      element.propertyName = this.field.get("name").toString();
      return element;
    }
    createSubElementFieldInstance() {
      const element = new Control({
        field: this.field,
        isReadOnly: this.isReadOnly,
        itemUrl: this.itemUrl,
        form: this.form,
        configuration: this.configuration
      });
      element.propertyName = this.field.get("name").toString();
      return element;
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/SeparatorLineField.js
  var Field13 = class extends BaseField {
    constructor(context) {
      super(context);
    }
    async createDom(dmElement) {
      return $("<hr class='dm-separatorline'/>");
    }
    async evaluateDom(dmElement) {
    }
    showNameField() {
      return true;
    }
    showValue() {
      return false;
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/DropDownByCollection.js
  var Field14 = class extends DropDownBaseField {
    constructor(context) {
      super(context);
      this.fieldType = FieldType.References;
    }
    async loadFields() {
      const workspace = this.field.get(_Forms._FieldTypes._DropDownByCollection.defaultWorkspace, ObjectType.String) ?? "Data";
      const path = this.field.get(_Forms._FieldTypes._DropDownByCollection.collection, ObjectType.String);
      const queryBuilder = new QueryBuilder();
      getElementsByPath(queryBuilder, workspace, path);
      const serverResult = await queryObject(queryBuilder.queryStatement);
      return serverResult.result.map((x) => {
        return {
          title: x.get("name", ObjectType.String) ?? x.id,
          workspace: x.workspace,
          itemUrl: x.uri
        };
      });
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/DropDownByQuery.js
  var Field15 = class extends DropDownBaseField {
    constructor(context) {
      super(context);
      this.fieldType = FieldType.References;
    }
    async loadFields() {
      let query = this.field.get(_Forms._FieldTypes._DropDownByQueryData.query, ObjectType.Single);
      query = await forceResolve(query);
      if (query === void 0 || query === null) {
        return [
          {
            title: "-- ERROR: Parameter Query is not set",
            value: ""
          }
        ];
      }
      const serverResult = await queryObject(query);
      return serverResult.result.map((x) => {
        return {
          title: x.get("name", ObjectType.String) ?? x.id,
          workspace: x.workspace,
          itemUrl: x.uri
        };
      });
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/UriReferenceFieldData.js
  var _UriReferenceFieldData = _Forms._FieldTypes._UriReferenceFieldData;
  var Field16 = class extends BaseField {
    constructor(context) {
      super(context);
    }
    async createDom(dmElement) {
      if (this.configuration?.isNewItem) {
        return $("<div><em>New item, content can only be set after saving</em></div>");
      }
      const fieldName = this.field.get("name")?.toString() ?? "";
      let value = dmElement.get(fieldName, ObjectType.String) ?? "";
      const originalValue = value;
      if (this.isReadOnly) {
        const divContainer = $("<div class='dm-urireference-container-readonly'></div>");
        const div = $("<div class='dm-textfield'/>");
        if (value === void 0) {
          div.append($("<em class='dm-undefined'>undefined</em>"));
        } else {
          div.text(value ?? "undefined");
        }
        divContainer.append(div);
        return divContainer;
      } else {
        const domContainer = $("<div class='dm-urireference-container'></div>");
        const textUri = "<div><strong>Uri:</strong></div>";
        domContainer.append(textUri);
        const defaultExtent = this.field.get(_UriReferenceFieldData.defaultExtent, ObjectType.String);
        const defaultWorkspace = this.field.get(_UriReferenceFieldData.defaultWorkspace, ObjectType.String);
        this._textBox = $("<input size='80' class='dm-textfield' />");
        this._textBox.val(value);
        this._textBox.on("input change", () => {
          if (this.callbackUpdateField !== void 0) {
            this.callbackUpdateField();
          }
        });
        domContainer.append(this._textBox);
        const builderText = $("<div><strong>Build your Uri:</strong></div>");
        domContainer.append(builderText);
        this._selectField = new SelectItemControl();
        if (defaultExtent !== void 0 && defaultExtent !== "") {
          await this._selectField.setWorkspaceById(defaultWorkspace);
        }
        if (defaultExtent !== void 0 && defaultExtent !== "") {
          await this._selectField.setWorkspaceById(defaultWorkspace);
        }
        const table = $("<table><tr><td>Element:</td><td><div class='dm-urireference-element' /></div></td></tr><tr><td>Property:</td><td><input type='text' class='dm-urireference-property' /></td></tr><tr><td>Full Name:</td><td><input type='text' class='dm-urireference-fullname' /></td></tr><tr><td>Composite:</td><td>  <select class='dm-urireference-composites' >    <option value=''>Only Self</option>    <option value='includingSelf'>Composites and self</option>    <option value='onlyComposites'>Composites</option>    <option value='allReferenced'>Referenced</option>    <option value='allReferencedIncludingSelf'>Referenced and self</option>  </select></td></tr><tr><td> </td><td><button class='dm-urireference-buildUri' class='btn btn-secondary'>Build Uri</button></td></tr></table>");
        domContainer.append(table);
        this._elementSelection = $(".dm-urireference-element", table);
        await this._selectField.initAsync(this._elementSelection);
        this._propertyField = $(".dm-urireference-property", table);
        this._fullNameField = $(".dm-urireference-fullname", table);
        this._compositeSelection = $(".dm-urireference-composites", table);
        this._buildUriButton = $(".dm-urireference-buildUri", table);
        this._buildUriButton.on("click", async () => {
          await this.buildUrl();
          if (this.callbackUpdateField !== void 0) {
            this.callbackUpdateField();
          }
        });
        return domContainer;
      }
    }
    async buildUrl() {
      const selectedItem = this._selectField.getSelectedItem();
      if (selectedItem === null || selectedItem === void 0) {
        this._textBox.val("No item is selected");
        return;
      }
      let url = this._selectField.getSelectedItem()?.uri;
      if (url === void 0) {
        this._textBox.val("");
        return;
      }
      let objectId = "";
      const hashIndex = url.indexOf("#");
      if (hashIndex >= 0) {
        objectId = url.substring(hashIndex + 1);
        url = url.substring(0, hashIndex);
      }
      let ampersand = "?";
      var propertyFieldValue = this._propertyField.val();
      if (propertyFieldValue !== void 0 && propertyFieldValue !== null && propertyFieldValue !== "") {
        url += ampersand + "prop=" + encodeURIComponent(this._propertyField.val()?.toString() ?? "");
        ampersand = "&";
      }
      var fullNameFieldValue = this._fullNameField.val();
      if (fullNameFieldValue !== void 0 && fullNameFieldValue !== null && fullNameFieldValue !== "") {
        url += ampersand + "fn=" + encodeURIComponent(this._fullNameField.val()?.toString() ?? "");
        ampersand = "&";
      }
      var compositeFieldValue = this._compositeSelection.val();
      if (compositeFieldValue !== void 0 && compositeFieldValue !== null && compositeFieldValue !== "") {
        url += ampersand + "composites=" + encodeURIComponent(this._compositeSelection.val()?.toString() ?? "");
        ampersand = "&";
      }
      if (objectId !== "") {
        url += "#" + objectId;
      }
      this._textBox.val(url);
    }
    async fillFieldsFromUrl(url) {
      let objectId = "";
      const hashIndex = url.indexOf("#");
      if (hashIndex >= 0) {
        objectId = url.substring(hashIndex + 1);
      }
      let prop = "";
      let fn = "";
      let composites = "";
      let ampersandIndex = url.indexOf("?");
      if (ampersandIndex >= 0) {
        const parameters = new URLSearchParams(url.substring(ampersandIndex + 1));
        prop = parameters.get("prop") ?? "";
        fn = parameters.get("fn") ?? "";
        composites = parameters.get("composites") ?? "";
      }
      let extent = url;
      const first = ampersandIndex > 0 && hashIndex > 0 ? Math.min(url.indexOf("#"), url.indexOf("?")) : ampersandIndex > 0 ? ampersandIndex : hashIndex;
      if (first >= 0) {
        extent = url.substring(0, hashIndex);
      }
      let objectUrl = extent;
      if (objectId !== "") {
        objectUrl += "#" + objectId;
      }
      this._fullNameField.val(fn);
      this._propertyField.val(prop);
      this._compositeSelection.val(composites);
      await this._selectField.setItemByUri("Data", objectUrl);
    }
    async evaluateDom(dmElement) {
      if (this._textBox !== void 0 && this._textBox !== null) {
        const fieldName = this.field.get("name")?.toString() ?? "";
        dmElement.set(fieldName, this._textBox.val());
      }
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/fields/UnknownField.js
  var Field17 = class extends BaseField {
    constructor(unknownFieldUri, context) {
      super(context);
      this.unknownFieldUri = unknownFieldUri;
    }
    async createDom(dmElement) {
      const result = $("<em></em>");
      result.text(this.unknownFieldUri ?? "unknown");
      return result;
    }
    async evaluateDom(dmElement) {
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/forms/FieldFactory.js
  var registeredFieldFactories = /* @__PURE__ */ new Map();
  function registerField(metaClassFieldData, factoryMethod) {
    registeredFieldFactories.set(metaClassFieldData, factoryMethod);
  }
  function registerDefaultFields() {
    registerField(_Forms._FieldTypes.__TextFieldData_Uri, (ctx) => new Field(ctx));
    registerField(_Forms._FieldTypes.__MetaClassElementFieldData_Uri, (ctx) => new Field7(ctx));
    registerField(_Forms._FieldTypes.__ReferenceFieldData_Uri, (ctx) => new Field11(ctx));
    registerField(_Forms._FieldTypes.__DropDownByCollection_Uri, (ctx) => new Field14(ctx));
    registerField(_Forms._FieldTypes.__DropDownByQueryData_Uri, (ctx) => new Field15(ctx));
    registerField(_Forms._FieldTypes.__CheckboxFieldData_Uri, (ctx) => new Field2(ctx));
    registerField(_Forms._FieldTypes.__CompositeFieldData_Uri, (ctx) => new Field4(ctx));
    registerField(_Forms._FieldTypes.__DateTimeFieldData_Uri, (ctx) => new Field5(ctx));
    registerField(_Forms._FieldTypes.__CheckboxListTaggingFieldData_Uri, (ctx) => new Field3(ctx));
    registerField(_Forms._FieldTypes.__DropDownFieldData_Uri, (ctx) => new Field6(ctx));
    registerField(_Forms._FieldTypes.__ActionFieldData_Uri, (ctx) => new Field8(ctx));
    registerField(_Forms._FieldTypes.__MergedFieldsInCellData_Uri, (ctx) => new Field9(ctx));
    registerField(_Forms._FieldTypes.__SubElementFieldData_Uri, (ctx) => new Field10(ctx));
    registerField(_Forms._FieldTypes.__AnyDataFieldData_Uri, (ctx) => new Field12(ctx));
    registerField(_Forms._FieldTypes.__SeparatorLineFieldData_Uri, (ctx) => new Field13(ctx));
    registerField(_Forms._FieldTypes.__UriReferenceFieldData_Uri, (ctx) => new Field16(ctx));
  }
  registerDefaultFields();
  function canBeSorted(field) {
    const metaClassUri = field.metaClass?.uri;
    if (metaClassUri === _Forms._FieldTypes.__TextFieldData_Uri || metaClassUri === _Forms._FieldTypes.__DateTimeFieldData_Uri) {
      return true;
    }
    return false;
  }
  function createField(context) {
    const fieldMetaClassUri = context.field?.metaClass?.uri ?? "";
    const factory = registeredFieldFactories.get(fieldMetaClassUri);
    if (factory !== void 0) {
      return factory(context);
    } else {
      return new Field17(fieldMetaClassUri, context);
    }
  }

  // ../DatenMeister.WebServer/Assets/js/burnsystems/burnJsPopup.js
  var PopupResult = class {
    // Method to close the popup by removing the outer popup window element
    closePopup() {
      this.htmlPopupWindow.remove();
    }
  };
  function createPopup() {
    const result = new PopupResult();
    const popup = document.createElement("div");
    popup.classList.add("burn-popup");
    document.body.appendChild(popup);
    const innerPopup = document.createElement("div");
    innerPopup.classList.add("burn-popup-inner");
    popup.appendChild(innerPopup);
    const closeButton = document.createElement("span");
    closeButton.classList.add("burn-popup-close");
    closeButton.innerHTML = "&times;";
    closeButton.onclick = () => result.closePopup();
    innerPopup.appendChild(closeButton);
    result.htmlPopupWindow = popup;
    result.htmlContent = innerPopup;
    return result;
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/forms/TableForm.ContextMenus.js
  var _FieldData = _Forms._FieldTypes._FieldData;
  var _TableForm = _Forms._FormTypes._TableForm;
  var _NamedElement = _UML._CommonStructure._NamedElement;
  function createFunctionToRemoveAllProperties(tableForm, field) {
    return {
      cellKeyTitle: "Clear",
      onCreateDom: (popup, jquery) => {
        const button = $("<button class='btn btn-secondary' type='button'>Clear Properties</button>");
        button.on("click", async () => {
          const propertyName = field.get(_FieldData._name_, ObjectType.String);
          const dataUrl = tableForm.getFormElement().get(_TableForm.dataUrl, ObjectType.String);
          const action = new DmObject(_Actions.__DeletePropertyFromCollectionAction_Uri);
          action.set(_Actions._DeletePropertyFromCollectionAction.collectionUrl, dataUrl);
          action.set(_Actions._DeletePropertyFromCollectionAction.propertyName, propertyName);
          const parameter = {
            parameter: action
          };
          await executeActionDirectly("Execute", parameter);
          await tableForm.refreshForm();
        });
        jquery.append(button);
      },
      requireConfirmation: true
    };
  }
  function createFunctionToFilterInProperty(tthis, field) {
    const dropDown = $("<select class=''></select>");
    const propertyName = field.get(_FieldData._name_, ObjectType.String);
    return {
      cellKeyTitle: "Filter in Property",
      /**
       * Creates a dropdown menu for filtering the values of a specific property.
       * @param popup - The popup result object.
       * @param jquery - The jQuery element to which the dropdown menu will be appended.
       */
      onCreateDom: (popup, jquery) => {
        const propertyValues = /* @__PURE__ */ new Set();
        tthis.elements.forEach((x) => {
          const propertyValue = x.get(propertyName, ObjectType.String);
          if (propertyValue !== void 0) {
            propertyValues.add(propertyValue);
          }
          if (propertyValues.size > 100) {
            jquery.append($("<span>Too many values to show</span>"));
            return;
          }
        });
        const sortedPropertyValues = Array.from(propertyValues).sort();
        dropDown.empty();
        const noFilter = $("<option></option>");
        noFilter.val("");
        noFilter.text("-- No Filter --");
        dropDown.append(noFilter);
        const currentValue = tthis.tableState.getFilterByProperty(propertyName);
        sortedPropertyValues.forEach((value) => {
          const option = $("<option></option>");
          option.val(value);
          option.text(truncateText(value, { maxLength: 20 }));
          dropDown.append(option);
          if (value === currentValue) {
            option.prop("selected", true);
          }
        });
        jquery.append(dropDown);
      },
      onSubmitForm: async () => {
        const value = dropDown.val()?.toString();
        if (value !== "" && value !== void 0) {
          tthis.tableState.setFilterByProperty(propertyName, value);
        } else {
          tthis.tableState.removeFilterByProperty(propertyName);
        }
        await tthis.reloadTable();
      },
      callbackButtonText: (query) => {
        const val = tthis.tableState.getFilterByProperty(propertyName);
        if (val !== void 0 && val !== "") {
          query.append($("<span>F</span>"));
          return true;
        }
        return false;
      }
    };
  }
  function createPopupButton(buttonText, container, createDom) {
    const buttonContainer = $("<div class='dm-tableform-popup-buttonpopup'></div>");
    const button = $("<button class='btn btn-secondary' type='button'></button>");
    const content = $("<div class='dm-tableform-popup-button-popup-content'></div>");
    buttonContainer.append(button);
    buttonContainer.append(content);
    content.hide();
    button.text(buttonText);
    button.on("click", () => {
      content.empty();
      content.show();
      createDom(content);
    });
    container.append(buttonContainer);
    return buttonContainer;
  }
  function createFunctionToLoadCurrentView(tableForm) {
    return {
      cellKeyTitle: "Load View",
      onCreateDom: async (popup, jquery) => {
        createPopupButton("Load View", jquery, async (innerQuery) => {
          const selectField = $("<div class='dm-tableform-load-currentview-select'></div>'");
          const selectItemControl = new SelectItemControl();
          await selectItemControl.setExtentByUri(WorkspaceManagement, UriExtentUserForm);
          const selectItemControlSettings = new ContainerSettings();
          selectItemControlSettings.showCancelButton = false;
          selectItemControlSettings.headline = "Select View";
          await selectItemControl.initAsync(selectField, selectItemControlSettings);
          selectItemControl.itemSelected.addListener(async (item) => {
            if (item.workspace === void 0) {
              throw new Error("Workspace is undefined");
            }
            tableForm.tableState.queryStatement = await getObjectByUri(item.workspace, item.uri);
            popup.closePopup();
            await tableForm.reloadTable();
          });
          innerQuery.append(selectField);
          const removeOverrideForm = $("<div><button class='btn btn-secondary' type='button'>Switch to Default View</button></div>");
          removeOverrideForm.on("click", async () => {
            tableForm.tableState.overrideQueryWorkspace = void 0;
            tableForm.tableState.overrideQueryItem = void 0;
            await tableForm.reloadTable();
          });
          innerQuery.append(removeOverrideForm);
        });
      }
    };
  }
  function createFunctionToStoreCurrentView(tableForm) {
    return {
      cellKeyTitle: "Store View",
      onCreateDom: async (popup, jquery) => {
        createPopupButton("Store View", jquery, async (innerQuery) => {
          const storeTable = $("<table><tr><td>Name of View:</td><td><input class='dm-tableform-store-currentview-name' type='text'></td></tr><tr><td title='Define the package under which the new package storing the overall dataview will be stored'>Package:</td><td class='dm-tableform-store-currentview-package'></td></tr><tr><td></td><td><button class='btn btn-primary dm-tableform-store-currentview-submit' type='button'>StoreView</button></td></tr><tr><td></td><td class='dm-tableform-store-currentview-result'></tr></table>");
          const nameTextField = $(".dm-tableform-store-currentview-name", storeTable);
          const packageField = $(".dm-tableform-store-currentview-package", storeTable);
          const submitButton = $(".dm-tableform-store-currentview-submit", storeTable);
          const resultCell = $(".dm-tableform-store-currentview-result", storeTable);
          const selectItemControl = new SelectItemControl();
          await selectItemControl.setExtentByUri(WorkspaceManagement, UriExtentUserForm);
          const selectItemControlSettings = new ContainerSettings();
          selectItemControlSettings.hideButtonRow = true;
          selectItemControlSettings.showCancelButton = false;
          selectItemControlSettings.headline = "Select Package";
          await selectItemControl.initAsync(packageField, selectItemControlSettings);
          submitButton.on("click", async () => {
            const name = nameTextField.val();
            if (name === void 0 || name === "") {
              alert("Please provide a name for the view");
              return;
            }
            const packageUrl = selectItemControl.getSelectedItem();
            if (packageUrl === void 0) {
              alert("No item selected");
              return;
            }
            const actionParameter = new DmObject(_Actions.__StoreElementAction_Uri);
            actionParameter.set(_Actions._StoreElementAction.name, name);
            actionParameter.set(_Actions._StoreElementAction.url, packageUrl.uri);
            actionParameter.set(_Actions._StoreElementAction.workspace, packageUrl.workspace);
            const queryBuilder = tableForm.tableState.queryStatement;
            queryBuilder.set(_NamedElement._name_, name);
            actionParameter.set(_Actions._StoreElementAction.element, queryBuilder);
            const result = await executeActionDirectly("Execute", {
              parameter: actionParameter
            });
            if (result.resultAsDmObject === void 0)
              throw Error("Result is not set");
            const uri = result.resultAsDmObject.get(_Actions._TargetReferenceResult.targetUrl, ObjectType.String);
            const workspace = result.resultAsDmObject.get(_Actions._TargetReferenceResult.targetWorkspace, ObjectType.String);
            selectItemControl.removeControl();
            const resultText = $("<span>View is created: <a>Click here to navigate to the view</a></span>");
            const anchor = resultText.find("a");
            anchor.attr("href", getLinkForNavigateToItemByUrl(workspace, uri));
            resultCell.empty();
            resultCell.append(resultText);
          });
          innerQuery.append(storeTable);
        });
      }
    };
  }

  // ../DatenMeister.WebServer/Assets/js/datenmeister/forms/TableState.js
  var TableState = class {
    constructor() {
      this.queryStatement = new DmObject(_DataViews.__QueryStatement_Uri);
      this.overrideQueryWorkspace = void 0;
      this.overrideQueryItem = void 0;
    }
    getOrderBy() {
      const node = this.findNodeByMetaClass(_DataViews._Row.__RowOrderByNode_Uri);
      if (node) {
        return {
          property: node.get(_DataViews._Row._RowOrderByNode.propertyName, ObjectType.String) ?? "",
          descending: node.get(_DataViews._Row._RowOrderByNode.orderDescending, ObjectType.Boolean) ?? false
        };
      }
      return void 0;
    }
    setOrderBy(property, descending) {
      let node = this.findNodeByMetaClass(_DataViews._Row.__RowOrderByNode_Uri);
      if (!node) {
        node = createForOrderByProperty(void 0, property, descending);
        this.addNode(node);
      } else {
        node.set(_DataViews._Row._RowOrderByNode.name, "Order by '" + property + "' " + (descending ? "descending" : "ascending"));
        node.set(_DataViews._Row._RowOrderByNode.propertyName, property);
        node.set(_DataViews._Row._RowOrderByNode.orderDescending, descending);
      }
    }
    removeOrderBy() {
      this.removeNodeByMetaClass(_DataViews._Row.__RowOrderByNode_Uri);
    }
    getFreeTextFilter() {
      const node = this.findNodeByMetaClass(_DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
      return node?.get(_DataViews._Row._RowFilterByFreeTextAnywhere.freeText, ObjectType.String);
    }
    setFreeTextFilter(text) {
      let node = this.findNodeByMetaClass(_DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
      if (!node) {
        node = createForFilterByFreetext(void 0, text);
        this.addNode(node);
      } else {
        node.set(_DataViews._Row._RowFilterByFreeTextAnywhere.name, "Filter by free text '" + text + "'");
        node.set(_DataViews._Row._RowFilterByFreeTextAnywhere.freeText, text);
      }
    }
    removeFreeTextFilter() {
      this.removeNodeByMetaClass(_DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
    }
    getFilterByProperty(property) {
      const node = this.findFilterByPropertyNode(property);
      return node?.get(_DataViews._Row._RowFilterByPropertyValueNode.value, ObjectType.String);
    }
    setFilterByProperty(property, value) {
      let node = this.findFilterByPropertyNode(property);
      const comparisonMode = _DataViews.___ComparisonMode.Equal;
      if (!node) {
        node = createForFilterByProperty(void 0, property, value, comparisonMode);
        this.addNode(node);
      } else {
        node.set(_DataViews._Row._RowFilterByPropertyValueNode.name, "Filter by property '" + property + "' " + comparisonMode + " '" + value + "'");
        node.set(_DataViews._Row._RowFilterByPropertyValueNode.value, value);
      }
    }
    removeFilterByProperty(property) {
      const node = this.findFilterByPropertyNode(property);
      if (node) {
        this.removeNode(node);
      }
    }
    getLimit() {
      const node = this.findNodeByMetaClass(_DataViews._Row.__RowFilterOnPositionNode_Uri);
      return node?.get(_DataViews._Row._RowFilterOnPositionNode.amount, ObjectType.Number);
    }
    setLimit(limit2) {
      let node = this.findNodeByMetaClass(_DataViews._Row.__RowFilterOnPositionNode_Uri);
      if (node === void 0) {
        node = createForLimit(void 0, limit2);
        node.set(_DataViews._Row._RowFilterOnPositionNode.position, 0);
        this.addNode(node);
      } else {
        node.set(_DataViews._Row._RowFilterOnPositionNode.name, "Limit to " + limit2 + " items");
        node.set(_DataViews._Row._RowFilterOnPositionNode.amount, limit2);
      }
    }
    removeLimit() {
      this.removeNodeByMetaClass(_DataViews._Row.__RowFilterOnPositionNode_Uri);
    }
    getFilterByProperties() {
      const result = {};
      for (const node of this.getViewNodes()) {
        if (node.metaClass?.uri === _DataViews._Row.__RowFilterByPropertyValueNode_Uri) {
          const prop = node.get(_DataViews._Row._RowFilterByPropertyValueNode.property, ObjectType.String);
          const val = node.get(_DataViews._Row._RowFilterByPropertyValueNode.value, ObjectType.String);
          if (prop) {
            result[prop] = val;
          }
        }
      }
      return result;
    }
    /**
     * Gets the nodes of the query statement in the order from source to result
     */
    getViewNodes() {
      const result = [];
      let currentNode = this.queryStatement.get(_DataViews._QueryStatement.resultNode, ObjectType.Object);
      while (currentNode) {
        result.push(currentNode);
        currentNode = currentNode.get("input", ObjectType.Object);
      }
      return result;
    }
    findNodeByMetaClass(metaClassUri) {
      return this.getViewNodes().find((n) => n.metaClass?.uri === metaClassUri);
    }
    findFilterByPropertyNode(property) {
      return this.getViewNodes().find((n) => n.metaClass?.uri === _DataViews._Row.__RowFilterByPropertyValueNode_Uri && n.get(_DataViews._Row._RowFilterByPropertyValueNode.property, ObjectType.String) === property);
    }
    addNode(node) {
      const limitNode = this.findNodeByMetaClass(_DataViews._Row.__RowFilterOnPositionNode_Uri);
      if (limitNode && node !== limitNode) {
        const inputOfLimit = limitNode.get("input", ObjectType.Object);
        if (inputOfLimit) {
          node.set("input", DmObject.createAsReferenceFromLocalId(inputOfLimit));
        }
        limitNode.set("input", DmObject.createAsReferenceFromLocalId(node));
        this.queryStatement.appendToArray(_DataViews._QueryStatement.nodes, node);
      } else {
        const resultNode = this.queryStatement.get(_DataViews._QueryStatement.resultNode, ObjectType.Object);
        if (resultNode) {
          node.set("input", DmObject.createAsReferenceFromLocalId(resultNode));
        }
        this.queryStatement.appendToArray(_DataViews._QueryStatement.nodes, node);
        this.queryStatement.set(_DataViews._QueryStatement.resultNode, node);
      }
    }
    removeNodeByMetaClass(metaClassUri) {
      const node = this.findNodeByMetaClass(metaClassUri);
      if (node) {
        this.removeNode(node);
      }
    }
    removeNode(node) {
      const nodes = this.queryStatement.getAsArray(_DataViews._QueryStatement.nodes);
      const index = nodes.indexOf(node);
      if (index === -1)
        return;
      const childNode = nodes.find((n) => n.get("input", ObjectType.Object) === node);
      const inputNode = node.get("input", ObjectType.Object);
      if (childNode) {
        if (inputNode) {
          childNode.set("input", inputNode);
        } else {
          childNode.unset("input");
        }
      } else {
        if (inputNode) {
          this.queryStatement.set(_DataViews._QueryStatement.resultNode, inputNode);
        } else {
          this.queryStatement.unset(_DataViews._QueryStatement.resultNode);
        }
      }
      nodes.splice(index, 1);
      this.queryStatement.set(_DataViews._QueryStatement.nodes, nodes);
    }
    initialize(limit2) {
      const builder = new QueryBuilder();
      if (this.overrideQueryWorkspace !== void 0 && this.overrideQueryItem !== void 0) {
        referenceExistingNode(builder, this.overrideQueryWorkspace, this.overrideQueryItem);
      } else {
        addDynamicSource(builder, "input");
      }
      if (limit2 === void 0) {
        limit(builder, 101);
      } else if (limit2 > 0) {
        limit(builder, limit2);
      }
      this.queryStatement = builder.queryStatement;
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/forms/TableForm.js
  var _FieldData2 = _Forms._FieldTypes._FieldData;
  var TableFormParameter = class {
    constructor() {
      this.shortenFullText = true;
      this.allowSortingOfColumn = true;
      this.allowFilteringOnProperty = true;
      this.allowFreeTextFiltering = true;
      this.showFilterQuery = true;
      this.showSettingsButtons = true;
      this.showColumnSettingsButtons = true;
      this.hideButtonsForNewElements = false;
    }
  };
  var TableJQueryCaches = class {
  };
  var TableForm = class {
    getFormElement() {
      if (this.formElement === void 0) {
        throw new Error("Form element is undefined");
      }
      return this.formElement;
    }
    constructor(configuration) {
      this.isDebug = false;
      this.tableParameter = new TableFormParameter();
      this.tableState = new TableState();
      this.tableCache = new TableJQueryCaches();
      this.firstRun = true;
      this.configuration = configuration;
      this.formElement = configuration.formElement;
    }
    /**
     * Refreshes the complete form including the parent item which might contain multiple tables
     */
    async refreshForm() {
      if (this.configuration.refreshForm !== void 0) {
        await this.configuration.refreshForm();
      } else {
        await this.createFormByCollection(this.tableCache.parentHtml);
      }
    }
    /**
     * Just refreshes the form and performs a reloading of items from the server
     * The query parameters are taken into consideration
     */
    async reloadTable() {
      const _ = this.updateFilterQueryText();
      await this.createFormByCollection(this.tableCache.parentHtml);
    }
    async refreshTable() {
      const _ = this.updateFilterQueryText();
      await this.createTable();
    }
    /**
     * Updates the informational text displayed in the table cache UI element.
     * If the provided message is null, undefined, or an empty string, the text is hidden.
     * Otherwise, the text is updated and displayed.
     *
     * @param {string} message The informational text to display. Passing null, undefined, or an empty string will hide the text.
     * @return {void}
     */
    setInfoText(message) {
      if (message === null || message === void 0 || message === "") {
        this.tableCache.cacheLoadingInfoText.hide();
      } else {
        this.tableCache.cacheLoadingInfoText.show();
        this.tableCache.cacheLoadingInfoText.text(message);
      }
    }
    /**
     * This method just calls the createFormByCollection since a TableForm can
     * show the extent's elements directly or just the properties of an elemnet
     * @param parent The Html to which the table shall be added
     */
    async createFormByObject(parent) {
      const tthis = this;
      if (this.configuration.isNewItem) {
        parent.append($("<div><em>For new items, the table form will not be shown. </em></div>"));
        return;
      }
      this.callbackLoadItems = async (query) => {
        this.tableParameter.allowSortingOfColumn = false;
        this.tableParameter.allowFilteringOnProperty = false;
        const property = this.getFormElement().get(_Forms._FormTypes._TableForm.property, ObjectType.String);
        const result = await getProperty(tthis.workspace, tthis.itemUrl, property);
        if (Array.isArray(result)) {
          return result;
        } else {
          return [];
        }
      };
      return await this.createFormByCollection(parent);
    }
    /**
     * Creates the form by the given collection
     * @param parent The parent html element
     */
    async createFormByCollection(parent) {
      this.tableCache.parentHtml = parent;
      const formElement = this.getFormElement();
      this.tableParameter.metaClass = formElement.get("metaClass")?.uri;
      if (this.configuration.isReadOnly === void 0) {
        this.configuration.isReadOnly = true;
      }
      if (this.callbackLoadItems === void 0) {
        throw "No callbackLoadItems is set";
      }
      if (this.firstRun) {
        this.firstRun = false;
        const urlParams = new URLSearchParams(window.location.search);
        this.isDebug = urlParams.get("debug") === "true" || urlParams.get("debug") === "1";
        this.tableCache.cacheContainer = $("<div class='dm-tableform-container'></div>");
        parent.append(this.tableCache.cacheContainer);
        this.tableCache.cacheHeadline = $("<h2><a></a></h2>");
        this.tableCache.cacheHeadline.hide();
        this.tableCache.cacheContainer.append(this.tableCache.cacheHeadline);
        this.tableCache.cacheLoadingInfoText = $("<div class='dm-tableform-loadinginfotext'></div>");
        this.tableCache.cacheLoadingInfoText.hide();
        this.tableCache.cacheContainer.append(this.tableCache.cacheLoadingInfoText);
        this.tableCache.cacheFreeTextField = $("<div class='dm-tableform-freetextform'></div>");
        this.tableCache.cacheContainer.append(this.tableCache.cacheFreeTextField);
        this.tableCache.cacheButtons = $("<div class='dm-tableform-buttons'></div>");
        this.tableCache.cacheContainer.append(this.tableCache.cacheButtons);
        this.tableCache.cacheButtonsTypeSelection = $("<div class='dm-tableform-button-typeselection'></div>");
        this.tableCache.cacheContainer.append(this.tableCache.cacheButtonsTypeSelection);
        this.tableCache.cacheSettings = $("<div class='dm-tableform-settings'><a class='btn btn-secondary'>Tableform Settings</a></div>");
        if (this.tableParameter.showSettingsButtons) {
          this.tableCache.cacheContainer.append(this.tableCache.cacheSettings);
          this.tableCache.cacheSettingsButton = $("<div class='dm-tableform-settings-button'></div>");
          this.tableCache.cacheSettings.append(this.tableCache.cacheSettingsButton);
          await this.initializeTableSettingsButton();
        }
        this.tableCache.cacheQueryText = $('<div class="dm-tableform-querytext"></div>');
        this.tableCache.cacheSettings.append(this.tableCache.cacheQueryText);
        this.tableCache.cacheQueryDebugLink = $('<a class="dm-tableform-debugquery" href="#">Debug Query</a>');
        this.tableCache.cacheQueryDebugLink.on("click", (e) => {
          e.preventDefault();
          this.ShowQueryStatementForDebug();
        });
        if (this.isDebug) {
          this.tableCache.cacheSettings.append(this.tableCache.cacheQueryDebugLink);
        }
        this.tableCache.cacheSettings.append(this.tableCache.cacheQueryDebugOutput = $('<span class="dm-tableform-debugquery-content"></span>'));
        this.tableCache.cacheEmptyDiv = $("<div></div>");
        this.tableCache.cacheContainer.append(this.tableCache.cacheEmptyDiv);
        this.tableCache.cacheTableContainer = $("<div class='dm-tableform-tablecontainer'></div>");
        this.tableCache.cacheContainer.append(this.tableCache.cacheTableContainer);
        this.tableCache.cacheTable = $("<table class='table table-striped table-bordered align-top dm-tableform'></table>");
        this.tableCache.cacheTableContainer.append(this.tableCache.cacheTable);
        if (this.tableParameter.allowFreeTextFiltering) {
          this.createFreeTextField();
        }
        this.tableState.initialize();
      }
      this.elements = await this.callbackLoadItems(this.tableState.queryStatement);
      const headLineText = formElement.get("title") ?? formElement.get("name");
      if (headLineText === "" || headLineText === void 0) {
        this.tableCache.cacheHeadline.hide();
      } else {
        this.tableCache.cacheHeadline.show();
        const headLineLink = $("a", this.tableCache.cacheHeadline);
        headLineLink.text(headLineText);
        if (this.workspace !== void 0 && this.extentUri !== void 0) {
          const link = getLinkForNavigateToExtentItems(this.workspace, this.extentUri, { metaClass: this.tableParameter.metaClass });
          if (link !== null) {
            headLineLink.attr("href", link);
          }
        }
      }
      this.tableCache.cacheButtons.empty();
      if (this.tableParameter.hideButtonsForNewElements) {
        this.tableCache.cacheButtons.hide();
      } else {
        this.createButtonsForNewInstance();
      }
      const _ = this.updateFilterQueryText();
      if (this.elements === void 0) {
        this.elements = [];
      }
      if (!Array.isArray(this.elements)) {
        this.tableCache.cacheEmptyDiv.empty();
        this.tableCache.cacheEmptyDiv.text("Non-Array elements for ListForm: ");
        this.tableCache.cacheEmptyDiv.append($("<em></em>").text(this.elements.toString()));
      } else {
        await this.createTable();
      }
    }
    /**
     * Creates the buttons for the new instance
     */
    createButtonsForNewInstance() {
      const formElement = this.getFormElement();
      const property = formElement.get("property");
      const tthis = this;
      if (formElement.get(_Forms._FormTypes._TableForm.inhibitNewUnclassifiedItems, ObjectType.Boolean) !== true) {
        createUnclassifiedButton();
      }
      const defaultTypesForNewElements = formElement.getAsArray("defaultTypesForNewElements");
      if (defaultTypesForNewElements !== void 0) {
        for (let n in defaultTypesForNewElements) {
          const inner = defaultTypesForNewElements[n];
          createButton(inner.get("name", ObjectType.String), inner.get("metaClass", ObjectType.Object));
        }
      }
      function createUnclassifiedButton() {
        const btn = $("<btn class='btn btn-secondary'></btn>");
        const typeSelection = $("<div></div>");
        btn.text("Create new Item");
        btn.on("click", async () => {
          typeSelection.empty();
          const selectItem2 = new SelectItemControl();
          const settings = new ContainerSettings();
          settings.browseSettings.showWorkspaceInBreadcrumb = true;
          settings.browseSettings.showExtentInBreadcrumb = true;
          settings.setButtonText = "Create new Item";
          selectItem2.itemSelected.addListener((selectedItem) => {
            if (tthis.itemUrl === void 0) {
              document.location.href = getLinkForNavigateToCreateNewItemInExtent(tthis.workspace, tthis.extentUri, selectedItem === void 0 ? void 0 : selectedItem.uri, selectedItem === void 0 ? void 0 : selectedItem.workspace);
            } else {
              document.location.href = getLinkForNavigateToCreateItemInProperty(tthis.workspace, tthis.itemUrl, property, selectedItem === void 0 ? void 0 : selectedItem.uri, selectedItem === void 0 ? void 0 : selectedItem.workspace);
            }
          });
          await selectItem2.setWorkspaceById("Types");
          await selectItem2.setExtentByUri("Types", "dm:///_internal/types/internal");
          await selectItem2.initAsync(typeSelection, settings);
        });
        tthis.tableCache.cacheButtons.append(btn);
        tthis.tableCache.cacheButtonsTypeSelection.append(typeSelection);
      }
      function createButton(name, metaClass) {
        const metaClassUri = metaClass?.uri;
        const metaClassWorkspace = metaClass?.workspace;
        const btn = $("<btn class='btn btn-secondary'></btn>");
        btn.text("Create " + name);
        btn.on("click", () => {
          if (property === void 0 || property === null) {
            document.location.href = getLinkForNavigateToCreateNewItemInExtent(tthis.workspace, tthis.extentUri, metaClassUri, metaClassWorkspace);
          } else {
            document.location.href = getLinkForNavigateToCreateItemInProperty(tthis.workspace, tthis.itemUrl, property, metaClassUri, "Types");
          }
        });
        tthis.tableCache.cacheButtons.append(btn);
      }
    }
    /**
     * Creates the free text field for filtering
     */
    createFreeTextField() {
      const tthis = this;
      const inputField = $('<input type="text" placeholder="Filter by Text"></input>');
      inputField.on("input", async () => {
        const value = inputField.val()?.toString();
        if (value === "" || value === void 0 || value === null) {
          this.tableState.removeFreeTextFilter();
        } else {
          tthis.tableState.setFreeTextFilter(value);
        }
        await tthis.reloadTable();
      });
      this.tableCache.cacheFreeTextField.append(inputField);
    }
    /**
     * Initializes the table settings button and provides the mechanism to create the table
     * @private
     */
    async initializeTableSettingsButton() {
      $(".btn", this.tableCache.cacheSettings).on("click", async () => {
        const popup = createPopup();
        const table = $("<table class='table table-bordered dm-table-nofullwidth align-top dm-tableform'><th>Action</th><th>Parameter</th></table>");
        $(popup.htmlContent).append(table);
        const menuItems = this.getMenuItemsForTableSettings();
        menuItems.forEach((menuItem) => {
          const tableRow = $("<tr><td class='dm-key'></td><td class='dm-value'></td></tr>");
          const cellKey = $(".dm-key", tableRow);
          const cellValue = $(".dm-value", tableRow);
          menuItem.onCreateDom(popup, cellValue);
          if (menuItem.cellKeyTitle !== void 0) {
            cellKey.text(menuItem.cellKeyTitle);
          }
          table.append(tableRow);
        });
        const submitButton = $("<button class='btn btn-primary' type='button'>Close</button>");
        submitButton.on("click", () => {
          menuItems.forEach((menuItem) => {
            if (menuItem.onSubmitForm) {
              menuItem.onSubmitForm();
            }
          });
          popup.closePopup();
        });
        const submitRow = $("<tr><td></td><td class='dm-value'></td></tr>").append(submitButton);
        $("td.dm-value", submitRow).append(submitButton);
        table.append(submitRow);
      });
    }
    /**
     * Gets the menu items for the table settings
     * @private
     */
    getMenuItemsForTableSettings() {
      return [
        createFunctionToLoadCurrentView(this),
        createFunctionToStoreCurrentView(this)
      ];
    }
    /**
     * Updates the filter query text which describes the current filter settings
     */
    async updateFilterQueryText() {
      if (this.tableParameter.showFilterQuery) {
        const queryText = await this.getSummaryOfQuery();
        this.tableCache.cacheQueryText.text(queryText);
      }
    }
    /**
    * Creates the table itself that shall be shown
    */
    async createTable() {
      const formElement = this.getFormElement();
      if (this.tableCache?.cacheTable === void 0)
        return;
      this.tableCache.cacheTable.empty();
      const fields = formElement.getAsArray("field");
      const headerRow = $("<tbody><tr></tr></tbody>");
      const innerRow = $("tr", headerRow);
      for (const field of fields) {
        let cell = $("<th></th>");
        const innerTable = $("<table class='dm-tablerow-columnheader'><tr><td class='dm-tablerow-columntitle'></td><td class='dm-tablerow-columnbuttons'></td></tr></table>");
        cell.append(innerTable);
        const titleCell = $(".dm-tablerow-columntitle", innerTable);
        const titleButtons = $(".dm-tablerow-columnbuttons", innerTable);
        titleCell.text(field.get(_FieldData2.title) ?? field.get(_FieldData2._name_));
        if (this.tableParameter.allowSortingOfColumn) {
          this.createSortingButton(field, titleButtons);
        }
        if (this.tableParameter.showColumnSettingsButtons) {
          await this.appendColumnMenus(field, titleButtons);
        }
        innerRow.append(cell);
      }
      this.tableCache.cacheTable.append(headerRow);
      let noItemsWithMetaClass = formElement.get("noItemsWithMetaClass");
      const metaClassFilter = this.tableParameter.metaClass;
      for (const element of this.elements) {
        const elementsMetaClass = element.metaClass?.uri;
        if (elementsMetaClass && noItemsWithMetaClass || metaClassFilter && elementsMetaClass !== metaClassFilter) {
          continue;
        }
        const row = $("<tr></tr>");
        for (const field of fields) {
          let cell = $("<td></td>");
          const fieldElement = createField({
            configuration: this.configuration,
            field,
            itemUrl: element.uri,
            isReadOnly: this.configuration.isReadOnly === true,
            form: this
          });
          let dom;
          if (fieldElement === void 0) {
            dom = $("<span></span>");
            dom.text("Field for " + field.get("name", ObjectType.String) + " not found");
          } else {
            dom = await fieldElement.createDom(element);
          }
          cell.append(dom);
          row.append(cell);
        }
        this.tableCache.cacheTable.append(row);
      }
    }
    /**
     * Creates a sorting button for the specified field and appends it to the title buttons.
     * The button allows toggling between ascending, descending, and no sorting states.
     *
     * @param field - The field for which the sorting button is created.
     * @param titleButtons - The jQuery element to which the sorting button will be appended.
     */
    createSortingButton(field, titleButtons) {
      const fieldCanBeSorted = canBeSorted(field);
      if (!fieldCanBeSorted)
        return;
      const fieldName = field.get(_FieldData2._name_);
      const orderBy = this.tableState.getOrderBy();
      const isSorted = orderBy?.property === fieldName;
      const isSortedDescending = orderBy?.descending;
      let sortingArrow;
      let onClick;
      if (!isSorted) {
        sortingArrow = $('<span class="dm-tableform-sortbutton">\u21C5</span>');
        onClick = async () => {
          this.tableState.setOrderBy(fieldName, false);
          await this.reloadTable();
        };
      } else if (isSorted && !isSortedDescending) {
        sortingArrow = $('<span class="dm-tableform-sortbutton">\u2193</span>');
        onClick = async () => {
          this.tableState.setOrderBy(fieldName, true);
          await this.reloadTable();
        };
      } else if (isSorted && isSortedDescending) {
        sortingArrow = $('<span class="dm-tableform-sortbutton">\u2191</span>');
        onClick = async () => {
          this.tableState.removeOrderBy();
          await this.reloadTable();
        };
      } else {
        throw new Error("Unknown sorting state");
      }
      sortingArrow.on("click", onClick);
      titleButtons.append(sortingArrow);
    }
    async appendColumnMenus(field, cell) {
      const propertyMenuItems = await this.createPropertyMenuItems(field);
      if (propertyMenuItems.length === 0) {
        return;
      }
      let manualButtonContent = false;
      let contextItem = $("<div class='dm-tableform-sortbutton'></div>");
      cell.append(contextItem);
      propertyMenuItems.forEach((menuItem) => {
        if (menuItem.callbackButtonText) {
          if (menuItem.callbackButtonText(contextItem)) {
            manualButtonContent = true;
          }
        }
      });
      if (!manualButtonContent) {
        contextItem.text("...");
      }
      contextItem.on("click", () => {
        const popup = createPopup();
        const table = $("<table class='table table-bordered dm-table-nofullwidth align-top dm-tableform'><th>Action</th><th>Parameter</th></table>");
        $(popup.htmlContent).append(table);
        propertyMenuItems.forEach((menuItem) => {
          const tableRow = $("<tr><td class='dm-key'></td><td class='dm-value'></td></tr>");
          const cellKey = $(".dm-key", tableRow);
          const cellValue = $(".dm-value", tableRow);
          menuItem.onCreateDom(popup, cellValue);
          if (menuItem.cellKeyTitle !== void 0) {
            cellKey.text(menuItem.cellKeyTitle);
          }
          table.append(tableRow);
        });
        const submitButton = $("<button class='btn btn-primary' type='button'>Submit</button>");
        submitButton.on("click", () => {
          propertyMenuItems.forEach((menuItem) => {
            if (menuItem.onSubmitForm) {
              menuItem.onSubmitForm();
            }
          });
          popup.closePopup();
        });
        const submitRow = $("<tr><td></td><td class='dm-value'></td></tr>").append(submitButton);
        $("td.dm-value", submitRow).append(submitButton);
        table.append(submitRow);
      });
    }
    async createPropertyMenuItems(field) {
      let result = [];
      if (field.metaClass !== void 0 && field.metaClass !== null && field.metaClass.uri !== _Forms._FieldTypes.__ActionFieldData_Uri && field.metaClass.uri !== _Forms._FieldTypes.__MetaClassElementFieldData_Uri) {
        result.push(createFunctionToRemoveAllProperties(this, field));
        if (this.tableParameter.allowFilteringOnProperty) {
          result.push(createFunctionToFilterInProperty(this, field));
        }
      }
      return result;
    }
    /**
     * Shows the query statement for debugging purposes
     */
    ShowQueryStatementForDebug() {
      debugElementToDom(this.tableState.queryStatement, this.tableCache.cacheQueryDebugOutput);
    }
    /**
     * Gets the summary text which is read by the user to understand the effective filtering
     * @returns The summary text
     */
    async getSummaryOfQuery() {
      let result = "";
      let andText = "";
      if (this.tableState.overrideQueryItem !== void 0 && this.tableState.overrideQueryWorkspace !== void 0) {
        const itemResult = await getObjectByUri(this.tableState.overrideQueryWorkspace, this.tableState.overrideQueryItem);
        const name = itemResult.get("name", ObjectType.String) ?? "Unknown";
        result = "Stored Query: " + name;
        return result;
      }
      const orderBy = this.tableState.getOrderBy();
      if (orderBy !== void 0) {
        result += `Order By: ${orderBy.property}`;
        if (orderBy.descending) {
          result += " (Descending)";
        }
        andText = " AND ";
      }
      const freeTextFilter = this.tableState.getFreeTextFilter();
      if (freeTextFilter !== void 0 && freeTextFilter !== "") {
        result += `${andText}Free-Textfilter: ${freeTextFilter}`;
        andText = " AND ";
      }
      const filterByProperties = this.tableState.getFilterByProperties();
      if (filterByProperties !== void 0) {
        for (let key in filterByProperties) {
          const value = filterByProperties[key];
          result += `${andText + key} is '${value}'`;
          andText = " AND ";
        }
      }
      const limitFilter = this.tableState.getLimit();
      if (limitFilter !== void 0) {
        result += `${andText}Limit: ${limitFilter}`;
        andText = " AND ";
      }
      if (result === "") {
        return "No Filter";
      }
      return result;
    }
  };

  // ../DatenMeister.WebServer/Assets/js/datenmeister/forms/TableState.StaticFilter.js
  function filterAndSortTableData(tableForm, elementsAsDmObject) {
    return async (query) => {
      const tableState = tableForm.tableState;
      if (query && tableState) {
        tableState.queryStatement = query;
      }
      let result = [...elementsAsDmObject];
      if (!tableState) {
        return result;
      }
      const freeText = tableState.getFreeTextFilter();
      if (freeText && freeText.trim().length > 0) {
        const term = freeText.trim().toLowerCase();
        result = result.filter((item) => {
          const propertyValues = item.getPropertyValues();
          for (let prop2 in propertyValues) {
            if (propertyValues[prop2].toLowerCase().includes(term))
              return true;
          }
          return false;
        });
      }
      const propertyFilters = tableState.getFilterByProperties();
      if (propertyFilters) {
        for (var prop in propertyFilters) {
          const filterVal = propertyFilters[prop];
          result = result.filter((item) => item.get(prop, ObjectType.String) === filterVal);
        }
      }
      const orderBy = tableState.getOrderBy();
      if (orderBy && orderBy.property) {
        const prop2 = orderBy.property;
        const isDesc = orderBy.descending;
        result.sort((a, b) => {
          let aValue = a.get(prop2);
          let bValue = b.get(prop2);
          if (aValue === void 0) {
            return isDesc ? 1 : -1;
          }
          if (bValue === void 0) {
            return isDesc ? -1 : 1;
          }
          if (typeof aValue === "number" && typeof bValue === "number") {
            return aValue - bValue * (isDesc ? -1 : 1);
          }
          aValue = aValue.toString();
          bValue = bValue.toString();
          return aValue.localeCompare(bValue) * (isDesc ? -1 : 1);
        });
      }
      let limit2 = tableState.getLimit();
      if (limit2 === void 0) {
        limit2 = 100;
        if (result.length > limit2) {
          tableForm.setInfoText("Only " + limit2 + " items are shown");
        }
      }
      if (limit2 > 0) {
        result = result.slice(0, limit2);
      }
      return result;
    };
  }

  // Assets/Js/EntryPoint.js
  function testAlert(text) {
    alert(text);
  }
  function testEntryPoint() {
    alert("Test entry");
  }
  async function renderStaticDataPage(data) {
    const formAsDmObject = convertJsonObjectToDmObject(data.form);
    const elementsAsDmObject = new Array();
    for (let n in data.data) {
      const elementAsDmObject = convertJsonObjectToDmObject(data.data[n]);
      if (elementAsDmObject !== void 0) {
        elementsAsDmObject.push(elementAsDmObject);
      }
    }
    const tableFormParameter = new TableFormParameter();
    tableFormParameter.showSettingsButtons = false;
    const formConfiguration = {
      formType: "collection" /* Collection */,
      formElement: formAsDmObject,
      isReadOnly: true
    };
    const tableForm = new TableForm(formConfiguration);
    tableForm.tableParameter = tableFormParameter;
    tableForm.callbackLoadItems = filterAndSortTableData(tableForm, elementsAsDmObject);
    await tableForm.createFormByCollection(data.htmlContainer);
  }
  window.testEntryPoint = testEntryPoint;
  window.testAlert = testAlert;
  window.renderStaticDataPage = renderStaticDataPage;
})();
//# sourceMappingURL=EntryPoint.js.map
