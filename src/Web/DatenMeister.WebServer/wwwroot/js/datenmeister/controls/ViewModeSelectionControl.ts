import * as VML from "../forms/ViewModeLogic.js";
import {ObjectType} from "../Mof.js";
import {_DatenMeister} from "../models/DatenMeister.class.js";
import {UserEvent} from "../../burnsystems/Events.js";

export class ViewModeSelectionControl {

    /**
     * Describes the event which is called when the user selects another viewMode
     */
    viewModeSelected:UserEvent<string> = new UserEvent<string>();

    /**
     * Creates the form itself
     * @param overridedViewMode Can be set, if the view mode is overridden by the caller of the method. 
     * Usually, the viewmode is retrieved by reading the one from the browser property. But if, the caller
     * requests another viewmode independent of the selected one, it can be injected here. 
     */
    async createForm(overridedViewMode?: string): Promise<JQuery> {

        const container = $("<div class='dm-viewmode-selection'></div>");
        const selectField = $("<select></select>");

        const viewModes = await VML.getViewModesFromServer();
        const currentViewMode = overridedViewMode ?? VML.getCurrentViewMode();

        for (let n in viewModes) {
            const v = viewModes[n];
            const option = $("<option></option>");
            const id = v.get(_DatenMeister._Forms._ViewMode.id, ObjectType.Single);
            option.attr("value", id);
            option.text(v.get(_DatenMeister._Forms._ViewMode._name_, ObjectType.Single));

            if (id === currentViewMode) {
                option.attr("selected", "selected");
            }

            selectField.append(option);
        }
        
        // Adds the clearing option
        const clearOption = $("<option></option>");
        clearOption.attr('data-clear', '1');
        clearOption.text('<< Default View >>');
        selectField.append(clearOption);

        // Now performs the action
        selectField.on("change", () => {
            const selectedElement = $("option:selected", selectField);

            // Checks, if the user has clicked on the field which clears the current viewmode
            let viewModeId;
            const isDataClear = selectedElement.attr('data-clear') === '1';
            if (isDataClear) {
                VML.clearCurrentViewMode();
                viewModeId = VML.getCurrentViewMode();
            } else {
                viewModeId = selectedElement.attr("value");
                VML.setCurrentViewMode(viewModeId);
            }
            
            // Callback
            if (this.viewModeSelected !== undefined) {
                this.viewModeSelected.invoke(viewModeId);
            }
        });

        container.append(selectField);
        
        // Sets the current view mode
        container.attr('title', 'Viewmode: ' + currentViewMode);
        return container;
    }
}