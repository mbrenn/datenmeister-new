import {BaseField, IFormField} from "./Interfaces.js";
import * as Mof from "../Mof.js";
import * as ClientItem from "../client/Items.js";
import {injectNameByUri} from "../DomHelper.js";
import * as SIC from "../controls/SelectItemControl.js";
import * as ClientItems from "../client/Items.js";
import * as Settings from "../Settings.js";
import * as Navigator from "../Navigator.js";

export class Field extends BaseField implements IFormField {

    element: Mof.DmObject;
    
    resultingDom: JQuery;
    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {
        this.element = dmElement;
        this.resultingDom = $(
            '<div class="dm-field-composite">' +
            '  <div class="dm-field-composite-value"></div>' +
            '  <div class="dm-field-composite-button-container">' +
            '    <div class="dm-field-composite-buttons"></div>' +
            '    <div class="dm-compositefield-attachitem-box"></div>' +
            '  </div>' +
            '</div>');
        
        await this.reloadValuesFromServer();

        return this.resultingDom;
    }
    
    reloadValuesFromServer = async () => {
        const valueDom = $('.dm-field-composite-value', this.resultingDom);
        const buttonsDom = $('.dm-field-composite-buttons', this.resultingDom);
        valueDom.empty();
        buttonsDom.empty();

        const propertyName = this.field.get('name');

        let value = await ClientItem.getProperty(this.form.workspace, this.element.uri, propertyName);
        if(Array.isArray(value))
        {
            value = value[0];
        }
        
        // Sets the buttons
        let isUndefined = false;
        if (value === undefined || value === null) {
            valueDom.append($("<em class='dm-undefined'>undefined</em>"));
            isUndefined = true;
        }
        else
        {
            let _ = injectNameByUri(valueDom, value.workspace, value.uri);
        }        
        
        const tthis = this;
        // Add the buttons
        if (!this.isReadOnly)
        {
            if(isUndefined)
            {
                const createButton = $("<btn class='btn btn-secondary'>Create</btn>");
                // Adds the button which allows the user to attach an existing item
                createButton.on("click", () => {
                    const containerDiv = $(".dm-compositefield-attachitem-box", this.resultingDom);
                    containerDiv.empty();
                    const selectItem = new SIC.SelectItemControl();
                    const settings = new SIC.Settings();
                    settings.showWorkspaceInBreadcrumb = true;
                    settings.showExtentInBreadcrumb = true;
                    selectItem.setWorkspaceById(Settings.WorkspaceTypes);
                    selectItem.itemSelected.addListener(
                        selectedItem => {

                            if (selectedItem === undefined ||
                                selectedItem.uri === undefined) {
                                alert("Nothing is selected.");
                                return;
                            }

                            document.location.href = Navigator.getLinkForNavigateToCreateItemInProperty(
                                tthis.form.workspace,
                                tthis.itemUrl,
                                selectedItem.uri,
                                selectedItem.workspace,
                                propertyName,
                                false);
                        });

                    selectItem.init(containerDiv, settings);

                    return false;
                });

                buttonsDom.append(createButton);
            }
            else {
                const unsetCell = $("<btn class='btn btn-secondary'>Unset</btn>");

                unsetCell.on('click', () => {
                    ClientItem.unsetProperty(tthis.form.workspace, tthis.itemUrl, propertyName).then(
                        async () => {
                            await tthis.reloadValuesFromServer();
                        }
                    );
                });

                buttonsDom.append(unsetCell);
            }
        }
    }

    async evaluateDom(dmElement: Mof.DmObject): Promise<void> {
    }
}
