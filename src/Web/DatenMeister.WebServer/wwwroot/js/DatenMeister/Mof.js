define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createObjectFromJson = exports.DmObject = void 0;
    class DmObject {
        constructor() {
            this.values = {};
        }
        set(key, value) {
            this.values[key] = value;
        }
        get(key) {
            return this.values[key];
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
            else {
                result = item.toString();
            }
            return result;
        }
    }
    exports.DmObject = DmObject;
    // Creates the given object from the included json
    function createObjectFromJson(json, metaClass) {
        var result = new DmObject();
        var converted = JSON.parse(json);
        for (var key in converted) {
            if (Object.prototype.hasOwnProperty.call(converted, key)) {
                var value = converted[key];
                result.set(key, value);
            }
        }
        if (metaClass !== undefined && metaClass !== null) {
            result.metaClass = metaClass;
        }
        return result;
    }
    exports.createObjectFromJson = createObjectFromJson;
});
//# sourceMappingURL=Mof.js.map