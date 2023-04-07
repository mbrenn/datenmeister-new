define(["require", "exports", "./Search", "./modules/DefaultLoader", "./forms/DefaultLoader"], function (require, exports, Search_1, ModuleLoader, FormLoader) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    $(() => {
        ModuleLoader.loadDefaultModules();
        FormLoader.loadDefaultForms();
        $("#dm-search-btn").on('click', () => {
            (0, Search_1.executeSearchByText)($("#dm-search-textbox").val().toString());
        });
    });
});
//# sourceMappingURL=init.js.map