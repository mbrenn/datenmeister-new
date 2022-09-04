import * as Mof from "../Mof"
import * as SIC from "./SelectItemControl";
import * as Events from "../../burnsystems/Events";
import {ItemLink} from "../ApiModels";
import * as ClientItems from "../client/Items";

/**
 * This parameter is given when the user has selected a certain type
 */
export interface TypeSelectedEvent {
    selectedType: Mof.DmObject;
}

/**
 * Within this control, the user can select a type 
 */
export class TypeSelectionControl {
    private selectionField: SIC.SelectItemControl;

    /**
     * This event is thrown when the user has selected a specific form
     */
    typeSelected: Events.UserEvent<TypeSelectedEvent> = new Events.UserEvent<TypeSelectedEvent>();

    /**
     * This JQuery element contains the control
     * @private
     */
    private _container: JQuery;

    /**
     * The constructor for this control
     * @param container The element in which the element shall be contained
     */
    constructor(container: JQuery) {
        this._container = container;
    }

    /**
     * Defines the current form url which is used to link the current form
     * @private
     */
    private _currentTypeUrl?: ItemLink;

    /**
     * Sets the current type url..
     * This method must be called before calling createControl
     * @param formUrl
     */
    setCurrentTypeUrl(formUrl: ItemLink) {
        this._currentTypeUrl = formUrl;
    }

    async createControl() {
        const result = $("<div>" +
            "<div class='dm-form-selection-control-select'></div>" +
            "</div>");
        const controlSelect = $(".dm-form-selection-control-select", result);

        // Creates the selection field
        this.selectionField = new SIC.SelectItemControl();

        this.selectionField.itemSelected.addListener(
            async selectedItem => {
                if (selectedItem !== undefined) {
                    const foundItem = await ClientItems.getObjectByUri(selectedItem.workspace, selectedItem.uri);
                    this.typeSelected.invoke(
                        {
                            selectedType: foundItem
                        });
                } else {
                    alert("No valid type has been selected")
                }
            }
        )

        if (this._currentTypeUrl !== undefined) {
            await this.selectionField.setItemByUri(this._currentTypeUrl.workspace, this._currentTypeUrl.uri);
        } else {
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