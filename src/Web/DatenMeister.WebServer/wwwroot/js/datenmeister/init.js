define(["require", "exports", "./Search"], function (require, exports, Search_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    $(() => {
        $("#dm-search-btn").on('click', () => {
            (0, Search_1.executeSearchByText)($("#dm-search-textbox").val().toString());
        });
    });
});
//# sourceMappingURL=init.js.map