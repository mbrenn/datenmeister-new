import * as SIC from "./SelectItemControl.js";
import * as Events from "../../burnsystems/Events.js";
import * as ClientItems from "../client/Items.js";
/**
 * Within this control, the user can select a type
 */
export class TypeSelectionControl {
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
    async createControl() {
        const result = $("<div>" +
            "<div class='dm-form-selection-control-select'></div>" +
            "</div>");
        const controlSelect = $(".dm-form-selection-control-select", result);
        // Creates the selection field
        this.selectionField = new SIC.SelectItemControl();
        this.selectionField.itemSelected.addListener(async (selectedItem) => {
            if (selectedItem !== undefined) {
                const foundItem = await ClientItems.getObjectByUri(selectedItem.workspace, selectedItem.uri);
                this.typeSelected.invoke({
                    selectedType: foundItem
                });
            }
            else {
                alert("No valid type has been selected");
            }
        });
        if (this._currentTypeUrl !== undefined) {
            await this.selectionField.setItemByUri(this._currentTypeUrl.workspace, this._currentTypeUrl.uri);
        }
        else {
            await this.selectionField.setExtentByUri("Types", "dm:///_internal/types/internal");
        }
        const settings = new SIC.Settings();
        settings.setButtonText = "Use Type";
        settings.headline = "Select Type:";
        await this.selectionField.initAsync(controlSelect, settings);
        // Finalize the GUI
        this._container.append(result);
    }
}
//# sourceMappingURL=TypeSelectionControl.js.map