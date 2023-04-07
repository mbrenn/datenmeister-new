
import * as FormFactory from  "./FormFactory"
import {_DatenMeister} from "../models/DatenMeister.class";
import {RowForm} from "./RowForm";
import {TableForm} from "./TableForm";

export function loadDefaultForms()
{
    FormFactory.registerObjectForm(
        _DatenMeister._Forms.__RowForm_Uri,
        () => new RowForm());
    
    FormFactory.registerObjectForm(
        _DatenMeister._Forms.__TableForm_Uri,
        () => new TableForm());
    
    FormFactory.registerCollectionForm(
        _DatenMeister._Forms.__TableForm_Uri,
        () => new TableForm());
}