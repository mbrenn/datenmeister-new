import * as FormFactory from "./FormFactory.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import { RowForm } from "./RowForm.js";
import { TableForm } from "./TableForm.js";
export function loadDefaultForms() {
    FormFactory.registerObjectForm(_DatenMeister._Forms.__RowForm_Uri, () => new RowForm());
    FormFactory.registerObjectForm(_DatenMeister._Forms.__TableForm_Uri, () => new TableForm());
    FormFactory.registerCollectionForm(_DatenMeister._Forms.__TableForm_Uri, () => new TableForm());
}
//# sourceMappingURL=DefaultLoader.js.map