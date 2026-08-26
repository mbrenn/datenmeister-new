/**
 * Base class for field implementations, providing the standard properties of IFormField
 */
export class BaseField {
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