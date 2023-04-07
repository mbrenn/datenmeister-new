import {executeSearchByText} from "./Search";
import * as ModuleLoader from "./modules/DefaultLoader";

import * as FormLoader from "./forms/DefaultLoader";

$(() => {
    ModuleLoader.loadDefaultModules();
    FormLoader.loadDefaultForms();
    $("#dm-search-btn").on(
        'click',
        () => {
            executeSearchByText($("#dm-search-textbox").val().toString());
        }
    );
});