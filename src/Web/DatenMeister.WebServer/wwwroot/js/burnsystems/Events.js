define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.UserEvent = void 0;
    class UserEvent {
        constructor() {
            this._assigned = new Array();
        }
        // Adds a new listener to the event handler
        addListener(func) {
            const result = {
                func: func
            };
            this._assigned.push(result);
            return result;
        }
        // Removes a listener from the event handler
        removeListener(handle) {
            this._assigned = this._assigned.filter(x => x !== handle);
        }
        // Calls the events
        invoke(data) {
            for (let n in this._assigned) {
                const handler = this._assigned[n];
                if (handler !== null) {
                    handler.func(data);
                }
            }
        }
    }
    exports.UserEvent = UserEvent;
});
//# sourceMappingURL=Events.js.map