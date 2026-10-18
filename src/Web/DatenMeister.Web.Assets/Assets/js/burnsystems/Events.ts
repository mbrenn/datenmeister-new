
/**
 * Registers a keyboard shortcut that triggers a click on the given button.
 * Also appends the shortcut label to the button's title attribute for hover display.
 */
export function addKeyBindingEvent(
    button: JQuery,
    bindingKey: string,
    bindingKeyModifierCtrl: boolean = false,
    bindingKeyModifierShift: boolean = false,
    bindingKeyModifierAlt: boolean = false
): void {
    const parts: string[] = [];
    if (bindingKeyModifierCtrl) parts.push('Ctrl');
    if (bindingKeyModifierShift) parts.push('Shift');
    if (bindingKeyModifierAlt) parts.push('Alt');
    parts.push(bindingKey);
    const shortcutLabel = "Shortcut: " + parts.join('+');

    const existingTitle = button.attr('title') ?? '';
    button.attr('title', existingTitle ? `${existingTitle} (${shortcutLabel})` : shortcutLabel);

    $(document).on('keydown', (e: JQuery.KeyDownEvent) => {
        const tag = (e.target as HTMLElement).tagName;
        const isEditable = tag === 'INPUT' || tag === 'TEXTAREA' || tag === 'SELECT'
            || (e.target as HTMLElement).isContentEditable;
        if (isEditable) return;

        if (
            e.key === bindingKey &&
            e.ctrlKey === bindingKeyModifierCtrl &&
            e.shiftKey === bindingKeyModifierShift &&
            e.altKey === bindingKeyModifierAlt
        ) {
            e.preventDefault();
            button.trigger('click');
        }
    });
}

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