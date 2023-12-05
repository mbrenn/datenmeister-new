import { executeSearchByText } from "./Search.js";
import * as ModuleLoader from "./modules/DefaultLoader.js";
import * as FormLoader from "./forms/DefaultLoader.js";
$(() => {
    ModuleLoader.loadDefaultModules();
    FormLoader.loadDefaultForms();
    $("#dm-search-btn").on('click', () => {
        executeSearchByText($("#dm-search-textbox").val().toString());
    });
});
//# sourceMappingURL=init.js.map