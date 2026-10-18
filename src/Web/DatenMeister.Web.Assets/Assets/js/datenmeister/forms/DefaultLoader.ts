
import * as FormFactory from  "./FormFactory.js"
import * as _DatenMeister from "../models/DatenMeister.class.js";
import {RowForm} from "./RowForm.js";
import TableForm from "./TableForm.js";
import {IFormConfiguration} from "./IFormConfiguration.js";

export function loadDefaultForms()
{
    FormFactory.registerObjectForm(
        _DatenMeister._Forms._FormTypes.__RowForm_Uri,
        (configuration: IFormConfiguration) => new RowForm(configuration));
    
    FormFactory.registerObjectForm(
        _DatenMeister._Forms._FormTypes.__TableForm_Uri,
        (configuration: IFormConfiguration) => new TableForm(configuration));
    
    FormFactory.registerCollectionForm(
        _DatenMeister._Forms._FormTypes.__TableForm_Uri,
        (configuration: IFormConfiguration) => {
            const result = new TableForm(configuration);
            result.tableParameter.allowSortingOfColumn = true;
            return result;
        }
    );
}