var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./SelectItemControl", "../../burnsystems/Events", "../client/Items"], function (require, exports, SIC, Events, ClientItems) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.TypeSelectionControl = void 0;
    /**
     * Within this control, the user can select a type
     */
    class TypeSelectionControl {
        /**
         * The constructor for this control
         * @param container The element in which the element shall be contained
         */
        constructor(container) {
            /**
             * This event is thrown when the user has selected a specific form
             */
            this.typeSelected = new Events.UserEvent();
            this._container = container;
        }
        /**
         * Sets the current type url..
         * This method must be called before calling createControl
         * @param formUrl
         */
        setCurrentTypeUrl(formUrl) {
            this._currentTypeUrl = formUrl;
        }
        createControl() {
            return __awaiter(this, void 0, void 0, function* () {
                const result = $("<div>" +
                    "<div class='dm-form-selection-control-select'></div>" +
                    "</div>");
                const controlSelect = $(".dm-form-selection-control-select", result);
                // Creates the selection field
                this._selectionField = new SIC.SelectItemControl();
                this._selectionField.itemSelected.addListener((selectedItem) => __awaiter(this, void 0, void 0, function* () {
                    if (selectedItem !== undefined) {
                        const foundItem = yield ClientItems.getObjectByUri(selectedItem.workspace, selectedItem.uri);
                        this.typeSelected.invoke({
                            selectedType: foundItem
                        });
                    }
                    else {
                        alert('Not a valid form has been selected');
                    }
                }));
                if (this._currentTypeUrl !== undefined) {
                    yield this._selectionField.setItemByUri("Types", this._currentTypeUrl.itemUrl);
                }
                else {
                    yield this._selectionField.setExtentByUri("Types", "dm:///_internal/types/internal");
                }
                const settings = new SIC.Settings();
                settings.setButtonText = "Use Type";
                settings.headline = "Select Type:";
                yield this._selectionField.initAsync(controlSelect, settings);
                // Finalize the GUI
                this._container.append(result);
            });
        }
    }
    exports.TypeSelectionControl = TypeSelectionControl;
});
//# sourceMappingURL=TypeSelectionControl.js.map