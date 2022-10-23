define(["require", "exports", "./ZipCodeModules"], function (require, exports, ZipCodeModules) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadDefaultModules = void 0;
    let loaded = false;
    function loadDefaultModules() {
        if (!loaded) {
            ZipCodeModules.loadModules();
        }
        loaded = true;
    }
    exports.loadDefaultModules = loadDefaultModules;
});
//# sourceMappingURL=DefaultLoader.js.map