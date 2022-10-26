define(["require", "exports", "./Search", "./modules/DefaultLoader"], function (require, exports, Search_1, DefaultLoader_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    $(() => {
        (0, DefaultLoader_1.loadDefaultModules)();
        $("#dm-search-btn").on('click', () => {
            (0, Search_1.executeSearchByText)($("#dm-search-textbox").val().toString());
        });
    });
});
//# sourceMappingURL=init.js.map