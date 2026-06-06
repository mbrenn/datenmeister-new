/**
 * Base class for field implementations, providing the standard properties of IFormField
 */
export class BaseField {
    /**
     * The configuration for the form
     */
    configuration;
    /**
     * Defines the field definition (metamodel)
     */
    field;
    /**
     * Indicates whether the field is in read-only mode
     */
    isReadOnly;
    /**
     * The navigation interface for the parent form
     */
    form;
    /**
     * The URL of the item to which this field is connected
     */
    itemUrl;
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