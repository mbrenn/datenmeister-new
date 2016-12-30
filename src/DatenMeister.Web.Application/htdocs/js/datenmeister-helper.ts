

// Helper function out of http://stackoverflow.com/questions/901115/how-can-i-get-query-string-values-in-javascript
export function getParameterByNameFromHash(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&#]" + name + "=([^&#]*)"),
        results = regex.exec(location.hash);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

export const simpleEvent = <T extends Function>(context = null) => {
    let cbs: T[] = [];
    return {
        addListener: (cb: T) => {
            cbs.push(cb);
        },

        removeListener: (cb: T) => {
            let i = cbs.indexOf(cb);
            cbs.splice(i, Math.max(i, 0));
        },

        trigger: (...args: any[]) => {
            cbs.forEach(cb => cb.apply(context, args));
        }
    };
};

export class SimpleEventClass<T extends Function> {
    cbs: T[];
    context: any;

    constructor(context:any = null) {
        this.context = context;
        this.cbs = new Array<T>();
    }

    addListener(cb: T) {
        this.cbs.push(cb);
    }

    removeListener(cb: T) {
        let i = this.cbs.indexOf(cb);
        this.cbs.splice(i, Math.max(i, 0));
    }

    trigger(...args: any[]) {
        this.cbs.forEach(cb => cb.apply(this.context, args));
    }
}