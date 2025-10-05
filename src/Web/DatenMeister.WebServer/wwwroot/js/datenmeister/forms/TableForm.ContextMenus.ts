import * as burnJsPopup from "../../burnJsPopup";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as Actions from "../client/Actions.js";
import _FieldData = _DatenMeister._Forms._FieldData;
import _TableForm = _DatenMeister._Forms._TableForm;
import * as Mof from "../Mof.js";
import {truncateText} from "../../burnsystems/StringManipulation.js";
import {TableForm} from "./TableForm.js";
import * as SelectItemControl from "../controls/SelectItemControl.js";
import * as Settings from "../Settings.js";
import * as CollectionForm from "./CollectionForm.js";
import * as Navigator from "../Navigator.js"
import {_UML} from "../models/uml.js";
import _NamedElement = _UML._CommonStructure._NamedElement;
import {getLinkForNavigateToItemByUrl} from "../Navigator.js";

/**
 * Creates the function which allows to remove all properties of all items within the extent
 * @param field Field for which the properties shall be removed
 */
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

        onSubmitForm: async () =>  {
            const value = dropDown.val();

            if (value !== "" && value !== undefined) {
                tthis.tableState.filterByProperty[propertyName] = dropDown.val();
            }
            else {
                delete tthis.tableState.filterByProperty[propertyName];
            }
            await tthis.reloadTable();
        },

        callbackButtonText: (query: JQuery) => {
            if (tthis.tableState.filterByProperty[propertyName] !== undefined && tthis.tableState.filterByProperty[propertyName] !== "") {
                query.append($("<span>F</span>"));
                return true;
            }
        }
    };
}

/**
 * Creates a function which allows to store the current view of the table
 * in a certain package on the server.
 * @param tableForm
 */
export function createFunctionToStoreCurrentView(tableForm: TableForm) {
    return {
        cellKeyTitle: "Store View",
        onCreateDom: async (popup: burnJsPopup.PopupResult, jquery: JQuery) => {
            const storeTable = $("<table>" +
                "<tr><td>Name of View:</td><td><input class='dm-tableform-store-currentview-name' type='text'></td></tr>" +
                "<tr><td title='Define the package under which the new package storing the overall dataview will be stored'>Package:</td>" +
                "<td class='dm-tableform-store-currentview-package'></td></tr>" +
                "<tr><td></td><td><button class='btn btn-primary dm-tableform-store-currentview-submit' type='button'>StoreView</button></td></tr>" +
                "<tr><td></td><td class='dm-tableform-store-currentview-result'></tr>" +
                "</table>");

            const nameTextField = $('.dm-tableform-store-currentview-name', storeTable);
            const packageField = $('.dm-tableform-store-currentview-package', storeTable);
            const submitButton = $('.dm-tableform-store-currentview-submit', storeTable);
            const resultCell = $('.dm-tableform-store-currentview-result', storeTable);

            const selectItemControl = new SelectItemControl.SelectItemControl();
            await selectItemControl.setExtentByUri(Settings.WorkspaceManagement, Settings.UriExtentUserForm);

            const selectItemControlSettings = new SelectItemControl.Settings();
            selectItemControlSettings.hideButtonRow = true;
            selectItemControlSettings.showCancelButton = false;
            selectItemControlSettings.headline = "Select Package";

            selectItemControl.init(packageField, selectItemControlSettings);

            submitButton.on('click', async () => {
                const name = nameTextField.val();
                if (name === undefined || name === "") {
                    alert("Please provide a name for the view");
                    return;
                }

                // Ok, get the package url
                const packageUrl = selectItemControl.getSelectedItem();

                // Prepare the action
                const actionParameter = new Mof.DmObject(_DatenMeister._Actions._Forms.__CreateFormUponViewAction_Uri);
                actionParameter.set(
                    _DatenMeister._Actions._Forms._CreateFormUponViewAction.name, name);
                actionParameter.set(
                    _DatenMeister._Actions._Forms._CreateFormUponViewAction.targetPackageUri, packageUrl.uri);
                actionParameter.set(
                    _DatenMeister._Actions._Forms._CreateFormUponViewAction.targetPackageWorkspace, packageUrl.workspace);
                
                const queryBuilder = 
                    CollectionForm.createQueryBuilder(tableForm.getQueryParameter(), -1).queryStatement;
                queryBuilder.set(_NamedElement._name_, name);
                actionParameter.set(
                    _DatenMeister._Actions._Forms._CreateFormUponViewAction.query, queryBuilder);

                const result = 
                    await Actions.executeActionDirectly("Execute", {
                    parameter: actionParameter
                });
                
                const workspace = packageUrl.workspace;
                const item = result.resultAsDmObject.get(
                    _DatenMeister._Actions._ParameterTypes._CreateFormUponViewResult.resultingPackageUrl, Mof.ObjectType.String);
                
                // If we have the result, we show it with a link to navigate
                if (result.resultAsDmObject !== undefined) {
                    const resultText = $("<span>View is created: <a>Click here to navigate to the view</a></span>");
                    const anchor = resultText.find("a");
                    anchor.attr("href",
                        Navigator.getLinkForNavigateToItemByUrl(
                            workspace,
                            item));
                    resultCell.empty();
                    resultCell.append(resultText);
                }
                else{
                    alert('Something went wrong.');
                }
            });

            jquery.append(storeTable);
        }
    };
}