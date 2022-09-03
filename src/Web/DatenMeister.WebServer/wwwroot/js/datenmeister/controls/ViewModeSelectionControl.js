define(["require", "exports", "../forms/ViewModeLogic", "../Mof", "../models/DatenMeister.class", "../../burnsystems/Events"], function (require, exports, VML, Mof_1, DatenMeister_class_1, Events_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ViewModeSelectionControl = void 0;
    class ViewModeSelectionControl {
        constructor() {
            /**
             * Describes the event which is called when the user selects another viewMode
             */
            this.viewModeSelected = new Events_1.UserEvent();
        }
        createForm() {
            const selectField = $("<select></select>");
            const viewModes = VML.getViewModesFromServer();
            const currentViewMode = VML.getCurrentViewMode();
            viewModes.then((result) => {
                for (let n in result) {
                    const v = result[n];
                    const option = $("<option></option>");
                    const id = v.get(DatenMeister_class_1._DatenMeister._Forms._ViewMode.id, Mof_1.ObjectType.Single);
                    option.attr("value", id);
                    option.text(v.get(DatenMeister_class_1._DatenMeister._Forms._ViewMode._name_, Mof_1.ObjectType.Single));
                    if (id === currentViewMode) {
                        option.attr("selected", "selected");
                    }
                    selectField.append(option);
                }
                selectField.on("change", () => {
                    const selectedElement = $("option:selected", selectField);
                    const viewModeId = selectedElement.attr("value");
                    VML.setCurrentViewMode(viewModeId);
                    this.viewModeSelected.invoke(viewModeId);
                });
            });
            return selectField;
        }
    }
    exports.ViewModeSelectionControl = ViewModeSelectionControl;
});
//# sourceMappingURL=ViewModeSelectionControl.js.map