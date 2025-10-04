import * as burnJsPopup from "../../burnJsPopup";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as Actions from "../client/Actions.js";
import _FieldData = _DatenMeister._Forms._FieldData;
import _TableForm = _DatenMeister._Forms._TableForm;
import * as Mof from "../Mof.js";
import {truncateText} from "../../burnsystems/StringManipulation.js";
import {TableForm} from "./TableForm.js";

export function createFunctionToRemoveAllProperties(field: Mof.DmObject) {
    const tthis = this;
    return {
        cellKeyTitle: "Clear",
        onCreateDom: (popup: burnJsPopup.PopupResult, jquery: JQuery) => {
            const button = $("<button class='btn btn-secondary' type='button'>Clear Properties</button>");
            button.on('click', async () => {
                // Gets the data
                const propertyName = field.get(_FieldData._name_, Mof.ObjectType.String);
                const dataUrl = tthis.formElement.get(_TableForm.dataUrl, Mof.ObjectType.String);

                // Creates the action
                const action = new Mof.DmObject(_DatenMeister._Actions.__DeletePropertyFromCollectionAction_Uri);
                action.set(_DatenMeister._Actions._DeletePropertyFromCollectionAction.collectionUrl, dataUrl);
                action.set(_DatenMeister._Actions._DeletePropertyFromCollectionAction.propertyName, propertyName);

                const parameter: Actions.ExecuteActionParams = {
                    parameter: action
                };

                await Actions.executeActionDirectly("Execute", parameter);
                await tthis.refreshForm();
            });

            jquery.append(button);
        },
        requireConfirmation: true
    };
}

/**
 * Creates the dropdown which allows the user to filter within the properties
 */
export function createFunctionToFilterInProperty(tthis: TableForm, field: Mof.DmObject) {
    const dropDown = $("<select class=''></select>");
    const propertyName = field.get(_FieldData._name_, Mof.ObjectType.String);

    return {
        cellKeyTitle: "Filter in Property",
        /**
         * Creates a dropdown menu for filtering the values of a specific property.
         * @param popup - The popup result object.
         * @param jquery - The jQuery element to which the dropdown menu will be appended.
         */
        onCreateDom: (popup: burnJsPopup.PopupResult, jquery: JQuery) => {
            // Finds the unique values of the property
            const propertyValues = new Set<string>();
            tthis.elements.forEach(x => {
                const propertyValue = x.get(propertyName, Mof.ObjectType.String);
                if (propertyValue !== undefined) {
                    propertyValues.add(propertyValue);
                }

                // If there are too many values, then do not show them
                if (propertyValues.size > 100) {
                    jquery.append($("<span>Too many values to show</span>"));
                    return;
                }
            });

            // Sort propertyValues
            const sortedPropertyValues = Array.from(propertyValues).sort();

            // Adds the options to the dropdown
            dropDown.empty();

            const noFilter = $("<option></option>");
            noFilter.val("");
            noFilter.text("-- No Filter --");
            dropDown.append(noFilter);

            const currentValue = tthis.tableState.filterByProperty[propertyName];

            sortedPropertyValues.forEach(value => {
                const option = $("<option></option>");
                option.val(value);
                option.text(truncateText(value, { maxLength: 20 }));
                dropDown.append(option);

                if (value === currentValue) {
                    option.prop('selected', true);
                }
            });

            jquery.append(dropDown);
        },

        onSubmitForm: () => {
            const value = dropDown.val();

            if (value !== "" && value !== undefined) {
                tthis.tableState.filterByProperty[propertyName] = dropDown.val();
            }
            else {
                delete tthis.tableState.filterByProperty[propertyName];
            }
            tthis.reloadTable();
        },

        callbackButtonText: (query: JQuery) => {
            if (tthis.tableState.filterByProperty[propertyName] !== undefined && tthis.tableState.filterByProperty[propertyName] !== "") {
                query.append($("<span>F</span>"));
                return true;
            }
        }
    };
}