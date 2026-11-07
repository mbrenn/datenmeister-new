import * as FormActions from "../FormActions.js";
import {BaseField, IFieldRenderContext, IFormField} from "./Interfaces.js";
import * as Mof from "../Mof.js";
import * as ClientItems from "../client/Items.js";
import * as MofSync from "../MofSync.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import {addKeyBindingEvent} from "../../burnsystems/Events.js";

export class Field extends BaseField implements IFormField {

    button: JQuery;

    private inConfirmation: boolean;
    /**
     * Stores the currently active status message element displayed next to the button.
     */
    private statusElement?: JQuery;
    /**
     * Stores the timeout identifier for automatically hiding the status message.
     */
    private statusTimeout?: number;

    constructor(context: IFieldRenderContext) {
        super(context);
    }

    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {

        const tthis = this;
        const title = this.field.get(_DatenMeister._Forms._FieldTypes._ActionFieldData.title, Mof.ObjectType.String);
        const action = this.field.get(_DatenMeister._Forms._FieldTypes._ActionFieldData.actionName, Mof.ObjectType.String);

        const parameter = this.field.get(_DatenMeister._Forms._FieldTypes._ActionFieldData.parameter, Mof.ObjectType.Single);
        const buttonText = this.field.get(_DatenMeister._Forms._FieldTypes._ActionFieldData.buttonText, Mof.ObjectType.String);


        const module = FormActions.getModule(action);
        this.inConfirmation = false;
        const requireConfirmation = module?.requiresConfirmation === true;

        this.button = $("<button class='btn btn-secondary' type='button'></button>");
        this.button.text(buttonText ?? title ?? action);

        const bindingKey = this.field.get(_DatenMeister._Forms._FieldTypes._ActionFieldData.bindingKey, Mof.ObjectType.String);
        if (bindingKey) {
            const bindingKeyModifierCtrl = this.field.get(_DatenMeister._Forms._FieldTypes._ActionFieldData.bindingKeyModifierCtrl, Mof.ObjectType.Boolean);
            const bindingKeyModifierShift = this.field.get(_DatenMeister._Forms._FieldTypes._ActionFieldData.bindingKeyModifierShift, Mof.ObjectType.Boolean);
            const bindingKeyModifierAlt = this.field.get(_DatenMeister._Forms._FieldTypes._ActionFieldData.bindingKeyModifierAlt, Mof.ObjectType.Boolean);
            addKeyBindingEvent(this.button, bindingKey, bindingKeyModifierCtrl, bindingKeyModifierShift, bindingKeyModifierAlt);
        }

        this.button.on('click',
            async () => {
                // There is the option whether a form action requires a separate confirmation
                // If this is the case, then the button itself is asking for confirmation upon the first 
                // click. Only then, the DetailForm itself is executed. 
                if (!requireConfirmation || tthis.inConfirmation) {
                    if (tthis.form.storeFormValuesIntoDom !== undefined) {
                        await tthis.form.storeFormValuesIntoDom(true);
                    }

                    const mofWithSync = dmElement as Mof.DmObjectWithSync;
                    if (module?.skipSaving !== true && mofWithSync.propertiesSet !== undefined) {
                        // We need to set the properties of the item, so the action handler can directly work on the item
                        await MofSync.sync(mofWithSync);
                    }

                    this.showButtonStatusMessage("Executing action...", false);
                    await FormActions.execute(action, tthis.form, dmElement, parameter);
                    this.showButtonStatusMessage("...done", true);
                }
                
                if (requireConfirmation && !tthis.inConfirmation) {
                    this.button.text("Are you sure?");
                    tthis.inConfirmation = true;
                }
            });

        return this.button;
    }
    
    /**
     * Displays a status message next to the action button, replacing any existing status message.
     * @param text The status message text to display.
     * @param autoHide If true, the message will fade out and be removed after 1 second.
     */
    showButtonStatusMessage(text: string, autoHide: boolean = false) {
        if (this.statusTimeout !== undefined) {
            window.clearTimeout(this.statusTimeout);
            this.statusTimeout = undefined;
        }

        if (this.statusElement) {
            this.statusElement.remove();
            this.statusElement = undefined;
        }

        const status = $("<span class='ms-2'></span>").text(text);
        this.button.after(status);
        this.statusElement = status;

        if (autoHide) {
            this.statusTimeout = window.setTimeout(() => {
                status.fadeOut('slow', () => {
                    status.remove();
                    if (this.statusElement === status) {
                        this.statusElement = undefined;
                    }
                });
            }, 1000);
        }
    }

    async evaluateDom(dmElement: Mof.DmObject) : Promise<void> {

    }

    showValue(): boolean {
        return false;
    }
}
