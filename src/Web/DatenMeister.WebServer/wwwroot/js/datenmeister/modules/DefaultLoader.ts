import * as ZipCodeModules from "./ZipCodeModules.js"
import * as ExtentModules from "./ExtentModules.js"
import * as ItemModules from "./ItemModules.js"
import * as WorkspaceModules from "./WorkspaceModules.js"
import * as FormModules from "./FormModules.js"
import * as ActionModules from "./ActionModules.js"

let loaded = false;
export function loadDefaultModules() {
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