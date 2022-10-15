
export interface SubscriptionHandle<T> {

}

interface Subscription<T> {
    func: (T) => void;
}

export class UserEvent<T> {
    private assigned: Array<Subscription<T>> = new Array<Subscription<T>>();

    // Adds a new listener to the event handler
    addListener(func: (argument:T) => void): SubscriptionHandle<T> {
        const result =
            {
                func: func
            };

        this.assigned.push(result);
        return result;
    }

    // Removes a listener from the event handler
    removeListener(handle: SubscriptionHandle<T>) {
        this.assigned = this.assigned.filter(x => x !== handle);
    }

    // Calls the events
    invoke(data: T) {
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