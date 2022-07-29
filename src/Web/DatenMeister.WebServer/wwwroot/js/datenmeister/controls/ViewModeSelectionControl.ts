import * as VML from "../forms/ViewModeLogic";
import {ObjectType} from "../Mof";
import {_DatenMeister} from "../models/DatenMeister.class";
import {UserEvent} from "../../burnsystems/Events";

export class ViewModeSelectionControl {

    /**
     * Describes the event which is called when the user selects another viewMode
     */
    viewModeSelected:UserEvent<string> = new UserEvent<string>();
    
    createForm(): JQuery {

        const selectField = $("<select></select>");

        const viewModes = VML.getViewModesFromServer();
        const currentViewMode = VML.getCurrentViewMode();
        viewModes.then((result) => {
            for (let n in result) {
                const v = result[n];
                const option = $("<option></option>");
                const id = v.get(_DatenMeister._Forms._ViewMode.id, ObjectType.Single);
                option.attr('value', id);
                option.text(v.get(_DatenMeister._Forms._ViewMode._name_, ObjectType.Single));

                if (id === currentViewMode) {
                    option.attr('selected', 'selected');
                }

                selectField.append(option);
            }

            selectField.on('change', () => {
                const selectedElement = $("option:selected", selectField);
                const viewModeId = selectedElement.attr('value');
                VML.setCurrentViewMode(viewModeId);
                this.viewModeSelected.invoke(viewModeId);
            });
        });

        return selectField;
    }

}