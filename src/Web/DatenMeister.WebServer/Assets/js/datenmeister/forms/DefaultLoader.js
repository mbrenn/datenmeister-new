import * as FormFactory from "./FormFactory.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import { RowForm } from "./RowForm.js";
import TableForm from "./TableForm.js";
export function loadDefaultForms() {
    FormFactory.registerObjectForm(_DatenMeister._Forms._FormTypes.__RowForm_Uri, (configuration) => new RowForm(configuration));
    FormFactory.registerObjectForm(_DatenMeister._Forms._FormTypes.__TableForm_Uri, (configuration) => new TableForm(configuration));
    FormFactory.registerCollectionForm(_DatenMeister._Forms._FormTypes.__TableForm_Uri, (configuration) => {
        const result = new TableForm(configuration);
        result.tableParameter.allowSortingOfColumn = true;
        return result;
    });
}
//# sourceMappingURL=DefaultLoader.js.map