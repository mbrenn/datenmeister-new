import * as Mof from "/js/datenmeister/Mof.js"
import * as TableForm from "/js/datenmeister/forms/TableForm.js"
import * as FormInterfaces from "/js/datenmeister/forms/Interfaces";
import {filterAndSortTableData} from "/js/datenmeister/forms/TableState.StaticFilter.js";

declare global {
    interface Window {
        testEntryPoint: typeof testEntryPoint;
        testAlert: typeof testAlert;
        renderStaticDataPage: typeof renderStaticDataPage;
    }
    
    interface IRenderStaticDataPageParams
    {
        data: any;
        form: any;
        cssClass: string;
        htmlContainer: JQuery<HTMLElement>
    }
}

export function testAlert(text: string) {
    alert(text);
}

export function testEntryPoint() {
    alert('Test entry');
}

export async function renderStaticDataPage(data: IRenderStaticDataPageParams) {
    // Converts the form as a DmObject
    const formAsDmObject = Mof.convertJsonObjectToDmObject(data.form);

    // Converts the elements as an array of DmObjects
    const elementsAsDmObject = new Array<Mof.DmObjectWithSync>();
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
    }

    const tableForm = new TableForm.TableForm(formConfiguration);
    tableForm.tableParameter = tableFormParameter;
    tableForm.callbackLoadItems = filterAndSortTableData(tableForm, elementsAsDmObject);
    
    await tableForm.createFormByCollection(data.htmlContainer);
}

window.testEntryPoint = testEntryPoint;
window.testAlert = testAlert;
window.renderStaticDataPage = renderStaticDataPage;