export class UserEvent {
    constructor() {
        this.assigned = new Array();
    }
    // Adds a new listener to the event handler
    addListener(func) {
        const result = {
            func: func
        };
        this.assigned.push(result);
        return result;
    }
    // Removes a listener from the event handler
    removeListener(handle) {
        this.assigned = this.assigned.filter(x => x !== handle);
    }
    // Calls the events
    invoke(data) {
        for (let n in this.assigned) {
            if (Object.prototype.hasOwnProperty.call(this.assigned, n)) {
                const handler = this.assigned[n];
                if (handler !== null && handler !== undefined) {
                    handler.func(data);
                }
            }
        }
    }
}
//# sourceMappingURL=Events.js.map