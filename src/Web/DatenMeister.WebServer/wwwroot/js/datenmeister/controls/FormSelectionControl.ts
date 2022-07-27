﻿import * as SIC from "./SelectItemControl";
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

    private _selectionField: SIC.SelectItemControl;

    /**
     * This event is thrown when the user has selected a specific form
     */
    formSelected: Events.UserEvent<FormSelectedEvent> = new Events.UserEvent<FormSelectedEvent>();


    /**
     * This event is thrown when the user has selected a specific form
     */
    formResetted: Events.UserEvent<void> = new Events.UserEvent<void>();

    /**
     * Creates the form in which the user can select a specific form
     * @param control Parent control in which the control shall be updated
     */
    async createControl(control: JQuery) {
        const result = $("<div><div class='dm-form-selection-control-select'></div><div class='dm_form-selection-control-reset'></div></div>");
        const controlSelect = $(".dm-form-selection-control-select", result);
        const controlReset = $(".dm_form-selection-control-reset", result);

        // Creates the selection field
        this._selectionField = new SIC.SelectItemControl();

        this._selectionField.itemSelected.addListener(
            selectedItem => {
                if (selectedItem !== undefined && selectedItem.selectedItem !== undefined) {
                    this.formSelected.invoke(
                        {
                            selectedForm: selectedItem
                        });
                } else {
                    alert('Not a valid form has been selected')
                }
            }
        );
        const t2 = this._selectionField.setWorkspaceById("Management")
            .then(async () => {
                await this._selectionField.setExtentByUri("dm:///_internal/forms/user");

                const settings = new SIC.Settings();
                settings.setButtonText = "Change Form";
                await this._selectionField.init(controlSelect, settings);
            });

        // Creates the reset button
        const resetButton = $("<button class='btn btn-secondary'>Reset form</button>");
        resetButton.on('click', () => {
            this.formResetted.invoke();
        });
        controlReset.append(resetButton)

        // Finalize the GUI
        control.append(result);

        // Now wait for the task 
        await t2;
    }
}