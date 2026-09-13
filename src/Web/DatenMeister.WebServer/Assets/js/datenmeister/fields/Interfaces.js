/**
 * Base class for field implementations, providing the standard properties and defaults
 */
export class BaseField {
    context;
    get configuration() {
        return this.context.configuration;
    }
    get field() {
        return this.context.field;
    }
    get isReadOnly() {
        return this.context.isReadOnly;
    }
    get form() {
        return this.context.form;
    }
    get itemUrl() {
        return this.context.itemUrl ?? "";
    }
    constructor(context) {
        this.context = context;
    }
    async evaluateDom(dmElement) {
        return;
    }
    /**
     * Gets a value indicating whether the value of the field is shown.
     * Default implementation returns true.
     * @returns True
     */
    showValue() {
        return true;
    }
    /**
     * This callback will be called in case of the content of the field is changed
     * This allows creator of a field to explicitly react upon changes within the field
     * Default implementation does nothing.
     */
    callbackUpdateField() {
        return;
    }
}
//# sourceMappingURL=Interfaces.js.map