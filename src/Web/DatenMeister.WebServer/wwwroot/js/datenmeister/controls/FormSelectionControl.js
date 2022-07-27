var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./SelectItemControl", "../../burnsystems/Events"], function (require, exports, SIC, Events) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.FormSelectionControl = void 0;
    /**
     * Creates control in which the user can select a specific form
     */
    class FormSelectionControl {
        constructor() {
            /**
             * This event is thrown when the user has selected a specific form
             */
            this.formSelected = new Events.UserEvent();
            /**
             * This event is thrown when the user has selected a specific form
             */
            this.formResetted = new Events.UserEvent();
        }
        /**
         * Creates the form in which the user can select a specific form
         * @param control Parent control in which the control shall be updated
         */
        createControl(control) {
            return __awaiter(this, void 0, void 0, function* () {
                const result = $("<div><div class='dm-form-selection-control-select'></div><div class='dm_form-selection-control-reset'></div></div>");
                const controlSelect = $(".dm-form-selection-control-select", result);
                const controlReset = $(".dm_form-selection-control-reset", result);
                // Creates the selection field
                this._selectionField = new SIC.SelectItemControl();
                this._selectionField.itemSelected.addListener(selectedItem => {
                    if (selectedItem !== undefined && selectedItem.selectedItem !== undefined) {
                        this.formSelected.invoke({
                            selectedForm: selectedItem
                        });
                    }
                    else {
                        alert('Not a valid form has been selected');
                    }
                });
                const t2 = this._selectionField.setWorkspaceById("Management")
                    .then(() => __awaiter(this, void 0, void 0, function* () {
                    yield this._selectionField.setExtentByUri("dm:///_internal/forms/user");
                    const settings = new SIC.Settings();
                    settings.setButtonText = "Change Form";
                    yield this._selectionField.init(controlSelect, settings);
                }));
                // Creates the reset button
                const resetButton = $("<button class='btn btn-secondary'>Reset form</button>");
                resetButton.on('click', () => {
                    this.formResetted.invoke();
                });
                controlReset.append(resetButton);
                // Finalize the GUI
                control.append(result);
                // Now wait for the task 
                yield t2;
            });
        }
    }
    exports.FormSelectionControl = FormSelectionControl;
});
//# sourceMappingURL=FormSelectionControl.js.map