
import * as FormActions from "../FormActions.js";
import {BaseField, IFormField} from "./Interfaces.js";
import * as Mof from "../Mof.js";
import * as ClientItems from "../client/Items.js";
import * as MofSync from "../MofSync.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";

export class Field extends BaseField implements IFormField {

    button: JQuery;

    private inConfirmation: boolean;

    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {

        const tthis = this;
        const title = this.field.get(_DatenMeister._Forms._ActionFieldData.title, Mof.ObjectType.String);
        const action = this.field.get(_DatenMeister._Forms._ActionFieldData.actionName, Mof.ObjectType.String);

        const parameter = this.field.get(_DatenMeister._Forms._ActionFieldData.parameter, Mof.ObjectType.Single);
        const buttonText = this.field.get(_DatenMeister._Forms._ActionFieldData.buttonText, Mof.ObjectType.String);


        const module = FormActions.getModule(action);
        this.inConfirmation = false;
        const requireConfirmation = module?.requiresConfirmation === true;

        this.button = $("<button class='btn btn-secondary' type='button'></button>");
        this.button.text(buttonText ?? title ?? action);

        this.button.on('click',
            async () => {
                // There is the option whether a form action requires a separate confirmation
                // If this is the case, then the button itself is asking for confirmation upon the first 
                // click. Only then, the DetailForm itself is executed. 
                if (!requireConfirmation || tthis.inConfirmation) {
                    if (tthis?.form?.storeFormValuesIntoDom !== undefined) {
                        await tthis.form.storeFormValuesIntoDom(true);
                    }

                    const mofWithSync = dmElement as Mof.DmObjectWithSync;
                    if (module?.skipSaving !== true && mofWithSync.propertiesSet !== undefined) {
                        // We need to set the properties of the item, so the action handler can directly work on the item
                        await MofSync.sync(mofWithSync);
                    }

                    await FormActions.execute(action, tthis.form, dmElement, parameter);
                }             
                
                if (requireConfirmation && !tthis.inConfirmation) {
                    this.button.text("Are you sure?");
                    tthis.inConfirmation = true;
                }
            });

        return this.button;
    }

    async evaluateDom(dmElement: Mof.DmObject) : Promise<void> {

    }

    showValue(): boolean {
        return false;
    }
}