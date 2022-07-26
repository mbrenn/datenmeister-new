import {SelectItemControl} from "./SelectItemControl";
import * as Events from "../../burnsystems/Events"
import * as Mof from "../Mof" 

/**
 * This event is thrown when the user has selected a form
 */
export interface FormSelectedEvent{

    /**
     * Defines the form which has been selected
     */
    selectedForm: Mof.DmObject;
}

/**
 * Creates control in which the user can select a specific form
 */
export class FormSelectionControl {

    private _selectionField: SelectItemControl;

    /**
     * This event is thrown when the user has selected a specific form
     */
    formSelected: Events.UserEvent<FormSelectedEvent> = new Events.UserEvent<FormSelectedEvent>();

    /**
     * Creates the form in which the user can select a specific form
     * @param control Parent control in which the control shall be updated
     */
    async createControl(control: JQuery){
        this._selectionField = new SelectItemControl();

        this._selectionField.itemSelected.addListener(
            selectedItem => {this.formSelected.invoke(
                {
                    selectedForm: selectedItem
                }
            )}
        )
        await this._selectionField.setWorkspaceById("Management");
        await this._selectionField.setExtentByUri("dm:///_internal/forms/user");
        await this._selectionField.init(control);
    }
}