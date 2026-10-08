import * as Mof from "/js/datenmeister/Mof.js"
import * as DatenMeister from "/js/datenmeister/models/DatenMeister.class.js"
import * as TableForm from "/js/datenmeister/forms/TableForm.js"
import {FormType} from "/js/datenmeister/forms/Interfaces";;

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
    }
}

export function testAlert(text: string) {
    alert(text);
}

export function testEntryPoint() {
    alert('Test entry');
}

export function renderStaticDataPage(data: IRenderStaticDataPageParams ) {
    alert(data.form);
}

window.testEntryPoint = testEntryPoint;
window.testAlert = testAlert;
window.renderStaticDataPage = renderStaticDataPage;