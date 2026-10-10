import * as Mof from "/js/datenmeister/Mof.js";
import * as TableForm from "/js/datenmeister/forms/TableForm.js";
import * as FormInterfaces from "/js/datenmeister/forms/Interfaces";
;
export function testAlert(text) {
    alert(text);
}
export function testEntryPoint() {
    alert('Test entry');
}
export async function renderStaticDataPage(data) {
    // Converts the form as a DmObject
    const formAsDmObject = Mof.convertJsonObjectToDmObject(data.form);
    // Converts the elements as an array of DmObjects
    const elementsAsDmObject = new Array();
    for (let n in data.data) {
        const elementAsDmObject = Mof.convertJsonObjectToDmObject(data.data[n]);
        if (elementAsDmObject !== undefined) {
            elementsAsDmObject.push(elementAsDmObject);
        }
    }
    // Configures the Tableform and sets the callback
    const tableFormParameter = new TableForm.TableFormParameter();
    tableFormParameter.showSettingsButtons = false;
    const formConfiguration = {
        formType: FormInterfaces.FormType.Collection,
        formElement: formAsDmObject,
        isReadOnly: true
    };
    const tableForm = new TableForm.TableForm(formConfiguration);
    tableForm.tableParameter = tableFormParameter;
    tableForm.callbackLoadItems = async (query) => {
        const tableState = tableForm.tableState;
        if (query && tableState) {
            tableState.queryStatement = query;
        }
        let result = [...elementsAsDmObject];
        if (!tableState) {
            return result;
        }
        // 1. Free-Text Search via tableState.getFreeTextFilter()
        const freeText = tableState.getFreeTextFilter();
        if (freeText && freeText.trim().length > 0) {
            const term = freeText.trim().toLowerCase();
            result = result.filter((item) => {
                const propertyValues = item.getPropertyValues();
                for (let prop in propertyValues) {
                    if (propertyValues[prop].toLowerCase().includes(term))
                        return true;
                }
                return false;
            });
        }
        // 2. Property Filtering via tableState.getFilterByProperties()
        const propertyFilters = tableState.getFilterByProperties();
        if (propertyFilters) {
            for (const [prop, filterVal] of Object.entries(propertyFilters)) {
            }
        }
        // 3. Sorting via tableState.getOrderBy()
        const orderBy = tableState.getOrderBy();
        if (orderBy && orderBy.property) {
            const prop = orderBy.property;
            const isDesc = orderBy.descending;
            result.sort((a, b) => { return 0; });
        }
        // 4. Limiting / Pagination via tableState.getLimit()
        let limit = tableState.getLimit();
        if (limit === undefined) {
            limit = 100;
            if (result.length > limit) {
                tableForm.setInfoText("Only " + limit + " items are shown");
            }
        }
        if (limit > 0) {
            result = result.slice(0, limit);
        }
        return result;
    };
    await tableForm.createFormByCollection(data.htmlContainer);
}
window.testEntryPoint = testEntryPoint;
window.testAlert = testAlert;
window.renderStaticDataPage = renderStaticDataPage;
//# sourceMappingURL=EntryPoint.js.map