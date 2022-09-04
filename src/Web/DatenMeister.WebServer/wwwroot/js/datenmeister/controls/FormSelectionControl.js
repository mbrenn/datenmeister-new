var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./SelectItemControl", "../../burnsystems/Events", "../client/Items", "../DomHelper"], function (require, exports, SIC, Events, ClientItems, DomHelper) {
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
         * Sets the current form url..
         * This method must be called before calling createControl
         * @param formUrl
         */
        setCurrentFormUrl(formUrl) {
            this.currentFormUrl = formUrl;
        }
        /**
         * Creates the form in which the user can select a specific form
         * @param control Parent control in which the control shall be updated
         */
        createControl(control) {
            return __awaiter(this, void 0, void 0, function* () {
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
                }
                else {
                    currentForm.append($("<em>Auto-Generated</em>"));
                }
                // Creates the selection field
                this.selectionField = new SIC.SelectItemControl();
                this.selectionField.itemSelected.addListener((selectedItem) => __awaiter(this, void 0, void 0, function* () {
                    if (selectedItem !== undefined) {
                        const foundItem = yield ClientItems.getObjectByUri(selectedItem.workspace, selectedItem.uri);
                        this.formSelected.invoke({
                            selectedForm: foundItem
                        });
                    }
                    else {
                        alert("Not a valid form has been selected");
                    }
                }));
                const t2 = this.selectionField.setWorkspaceById("Management")
                    .then(() => __awaiter(this, void 0, void 0, function* () {
                    const settings = new SIC.Settings();
                    settings.setButtonText = "Change Form";
                    settings.headline = "Select Form:";
                    yield this.selectionField.initAsync(controlSelect, settings);
                    if (this.currentFormUrl !== undefined) {
                        yield this.selectionField.setItemByUri("Management", this.currentFormUrl.uri);
                    }
                    else {
                        yield this.selectionField.setExtentByUri("Management", "dm:///_internal/forms/internal");
                    }
                }));
                // Creates the reset button
                const resetButton = $("<button class='btn btn-secondary'>Reset form</button>");
                resetButton.on("click", () => {
                    this.formResetted.invoke(null);
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