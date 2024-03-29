﻿import * as SIC from "./SelectItemControl.js";
import * as Events from "../../burnsystems/Events.js"
import * as Mof from "../Mof.js" 
import * as ClientItems from "../client/Items.js"
import {ItemLink} from "../ApiModels.js";
import * as DomHelper from "../DomHelper.js"

/**
 * This event is thrown when the user has selected a form
 */
export interface IFormSelectedEvent{

    /**
     * Defines the form which has been selected
     */
    selectedForm: Mof.DmObject;
}

/**
 * Creates control in which the user can select a specific form
 */
export class FormSelectionControl {

    private selectionField: SIC.SelectItemControl;

    /**
     * This event is thrown when the user has selected a specific form
     */
    formSelected: Events.UserEvent<IFormSelectedEvent> = new Events.UserEvent<IFormSelectedEvent>();

    /**
     * This event is thrown when the user has selected a specific form
     */
    formResetted: Events.UserEvent<void> = new Events.UserEvent<void>();

    /**
     * Defines the current form url which is used to link the current form
     * @private
     */
    private currentFormUrl?: ItemLink;

    /**
     * Sets the current form url..
     * This method must be called before calling createControl
     * @param formUrl
     */
    setCurrentFormUrl(formUrl: ItemLink) {
        this.currentFormUrl = formUrl;
    }

    /**
     * Creates the form in which the user can select a specific form
     * @param control Parent control in which the control shall be updated
     */
    async createControl(control: JQuery) {
        const result = $("<div>" +
            "<div class='dm-form-selection-control-current'>" +
            "Current Form: <span class='dm-form-selection-control-current-span'></span>" +
            "</div>" +
            "<div class='dm-form-selection-control-select'></div>" +
            "<div class='dm_form-selection-control-reset'></div>" +
            "</div>");
        const controlSelect = $(".dm-form-selection-control-select", result);
        const controlReset = $(".dm_form-selection-control-reset", result);
        const currentForm = $(".dm-form-selection-control-current-span", result);

        if (this.currentFormUrl !== undefined) {
            const _ = DomHelper.injectNameByUri(currentForm, this.currentFormUrl.workspace, this.currentFormUrl.uri);
        } else {
            currentForm.append($("<em>Auto-Generated</em>"));
        }

        // Creates the selection field
        this.selectionField = new SIC.SelectItemControl();

        this.selectionField.itemSelected.addListener(
            async selectedItem => {
                if (selectedItem !== undefined) {
                    const foundItem = await ClientItems.getObjectByUri(selectedItem.workspace, selectedItem.uri);
                    this.formSelected.invoke(
                        {
                            selectedForm: foundItem
                        });
                } else {
                    alert("Not a valid form has been selected")
                }
            }
        );

        const t2 = this.selectionField.setWorkspaceById("Management")
            .then(async () => {

                const settings = new SIC.Settings();
                settings.setButtonText = "Change Form";
                settings.headline = "Select Form:";
                await this.selectionField.initAsync(controlSelect, settings);

                if (this.currentFormUrl !== undefined) {
                    await this.selectionField.setItemByUri("Management", this.currentFormUrl.uri);
                } else {
                    await this.selectionField.setExtentByUri("Management", "dm:///_internal/forms/internal");
                }
            });

        // Creates the reset button
        const resetButton = $("<button class='btn btn-secondary'>Reset form</button>");
        resetButton.on("click", () => {
            this.formResetted.invoke(null);
        });
        controlReset.append(resetButton);

        // Finalize the GUI
        control.append(result);

        // Now wait for the task 
        await t2;
    }
}