define(["require", "exports", "./ZipCodeModules", "./ExtentModules", "./ItemModules", "./WorkspaceModules", "./FormModules", "./ActionModules"], function (require, exports, ZipCodeModules, ExtentModules, ItemModules, WorkspaceModules, FormModules, ActionModules) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadDefaultModules = void 0;
    let loaded = false;
    function loadDefaultModules() {
        if (!loaded) {
            ZipCodeModules.loadModules();
            ExtentModules.loadModules();
            ItemModules.loadModules();
            WorkspaceModules.loadModules();
            FormModules.loadModules();
            ActionModules.loadModules();
        }
        loaded = true;
    }
    exports.loadDefaultModules = loadDefaultModules;
});
//# sourceMappingURL=DefaultLoader.js.map