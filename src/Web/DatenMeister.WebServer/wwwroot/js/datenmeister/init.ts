import {executeSearchByText} from "./Search";

$(() => {

    $("#dm-search-btn").on(
        'click',
        () => {
            executeSearchByText($("#dm-search-textbox").val().toString());
        }
    )
});