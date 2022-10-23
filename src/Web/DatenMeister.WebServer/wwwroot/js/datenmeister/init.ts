import {executeSearchByText} from "./Search";
import {loadDefaultModules} from "./modules/DefaultLoader";

$(() => {
    loadDefaultModules();
    $("#dm-search-btn").on(
        'click',
        () => {
            executeSearchByText($("#dm-search-textbox").val().toString());
        }
    )
});