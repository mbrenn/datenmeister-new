import * as Mof from "/js/datenmeister/Mof.js"
import * as DatenMeister from "/js/datenmeister/models/DatenMeister.class.js"
import * as TableForm from "/js/datenmeister/forms/TableForm.js"
import {FormType} from "/js/datenmeister/forms/Interfaces";

declare global {
    interface Window {
        testEntryPoint: typeof testEntryPoint;
    }
}

export function testEntryPoint()
{
    alert('Test entry');
}

window.testEntryPoint = testEntryPoint;