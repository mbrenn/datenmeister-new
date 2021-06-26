define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.DmObject = void 0;
    class DmObject {
        constructor() {
            this._values = new Array();
        }
        set(key, value) {
            this._values[key] = value;
        }
        get(key) {
            return this._values[key];
        }
        isSet(key) {
            return this._values[key] !== undefined;
        }
        unset(key) {
            this._values[key] = undefined;
        }
    }
    exports.DmObject = DmObject;
});
//# sourceMappingURL=Mof.js.map