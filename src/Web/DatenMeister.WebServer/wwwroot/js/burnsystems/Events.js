/**
 * Registers a keyboard shortcut that triggers a click on the given button.
 * Also appends the shortcut label to the button's title attribute for hover display.
 */
export function addKeyBindingEvent(button, bindingKey, bindingKeyModifierCtrl = false, bindingKeyModifierShift = false, bindingKeyModifierAlt = false) {
    const parts = [];
    if (bindingKeyModifierCtrl)
        parts.push('Ctrl');
    if (bindingKeyModifierShift)
        parts.push('Shift');
    if (bindingKeyModifierAlt)
        parts.push('Alt');
    parts.push(bindingKey);
    const shortcutLabel = "Shortcut: " + parts.join('+');
    const existingTitle = button.attr('title') ?? '';
    button.attr('title', existingTitle ? `${existingTitle} (${shortcutLabel})` : shortcutLabel);
    $(document).on('keydown', (e) => {
        const tag = e.target.tagName;
        const isEditable = tag === 'INPUT' || tag === 'TEXTAREA' || tag === 'SELECT'
            || e.target.isContentEditable;
        if (isEditable)
            return;
        if (e.key === bindingKey &&
            e.ctrlKey === bindingKeyModifierCtrl &&
            e.shiftKey === bindingKeyModifierShift &&
            e.altKey === bindingKeyModifierAlt) {
            e.preventDefault();
            button.trigger('click');
        }
    });
}
export class UserEvent {
    assigned = new Array();
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