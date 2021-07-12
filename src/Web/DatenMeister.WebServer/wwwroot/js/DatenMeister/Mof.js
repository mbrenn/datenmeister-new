define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createObjectFromJson = exports.DmObject = void 0;
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
        isSet(key) {
            return this.values[key] !== undefined;
        }
        unset(key) {
            this.values[key] = undefined;
        }
        toString() {
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
    exports.DmObject = DmObject;
    // Creates the given object from the included json
    function createObjectFromJson(json) {
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
    exports.createObjectFromJson = createObjectFromJson;
});
//# sourceMappingURL=Mof.js.map