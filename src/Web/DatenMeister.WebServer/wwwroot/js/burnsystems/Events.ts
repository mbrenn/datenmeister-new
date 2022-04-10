
export interface SubscriptionHandle<T> {

}

interface Subscription<T> {
    func: (T) => void;
}

export class UserEvent<T> {
    private _assigned: Array<Subscription<T>> = new Array<Subscription<T>>();

    // Adds a new listener to the event handler
    addListener(func: (T) => void): SubscriptionHandle<T> {
        const result =
            {
                func: func
            };

        this._assigned.push(result);
        return result;
    }

    // Removes a listener from the event handler
    removeListener(handle: SubscriptionHandle<T>) {
        this._assigned = this._assigned.filter(x => x !== handle);
    }

    // Calls the events
    invoke(data: T) {
        for (let n in this._assigned) {
            const handler = this._assigned[n];
            if (handler !== null) {
                handler.func(data);
            }
        }
    }
}