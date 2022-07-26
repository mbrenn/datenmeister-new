var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./SelectItemControl", "../../burnsystems/Events"], function (require, exports, SelectItemControl_1, Events) {
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
        }
        /**
         * Creates the form in which the user can select a specific form
         * @param control Parent control in which the control shall be updated
         */
        createControl(control) {
            return __awaiter(this, void 0, void 0, function* () {
                this._selectionField = new SelectItemControl_1.SelectItemControl();
                this._selectionField.itemSelected.addListener(selectedItem => {
                    this.formSelected.invoke({
                        selectedForm: selectedItem
                    });
                });
                yield this._selectionField.setWorkspaceById("Management");
                yield this._selectionField.setExtentByUri("dm:///_internal/forms/user");
                yield this._selectionField.init(control);
            });
        }
    }
    exports.FormSelectionControl = FormSelectionControl;
});
//# sourceMappingURL=FormSelectionControl.js.map