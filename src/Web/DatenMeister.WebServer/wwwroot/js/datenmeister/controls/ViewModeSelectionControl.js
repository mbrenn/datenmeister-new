import * as VML from "../forms/ViewModeLogic.js";
import { ObjectType } from "../Mof.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import { UserEvent } from "../../burnsystems/Events.js";
export class ViewModeSelectionControl {
    constructor() {
        /**
         * Describes the event which is called when the user selects another viewMode
         */
        this.viewModeSelected = new UserEvent();
    }
    /**
     * Creates the form itself
     * @param overridedViewMode Can be set, if the view mode is overridden by the caller of the method.
     * Usually, the viewmode is retrieved by reading the one from the browser property. But if, the caller
     * requests another viewmode independent of the selected one, it can be injected here.
     */
    async createForm(overridedViewMode) {
        const container = $("<div></div>");
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
        clearOption.text('Switch to automatic');
        selectField.append(clearOption);
        // Now performs the action
        selectField.on("change", () => {
            const selectedElement = $("option:selected", selectField);
            // Checks, if the user has clicked on dataclear
            let viewModeId;
            const isDataClear = selectedElement.attr('data-clear') === '1';
            if (isDataClear) {
                VML.clearCurrentViewMode();
                viewModeId = VML.getCurrentViewMode();
            }
            else {
                viewModeId = selectedElement.attr("value");
                VML.setCurrentViewMode(viewModeId);
            }
            // Callback
            this.viewModeSelected.invoke(viewModeId);
        });
        container.append(selectField);
        // Sets the current view mode
        const current = $("<span class='dm-viewmode-current'>Current: <span class='dm-viewmode-current-text'></span></span>");
        $(".dm-viewmode-current-text", current).text(currentViewMode);
        container.append(current);
        return container;
    }
}
//# sourceMappingURL=ViewModeSelectionControl.js.map