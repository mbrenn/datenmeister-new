import {executeSearchByText} from "./Search";

$(function () {

    $("#dm-search-btn").on(
        'click',
        () => {
            executeSearchByText($("#dm-search-textbox").val().toString());
        }
    )
});