import * as ZipCodeModules from "./ZipCodeModules"
import * as ExtentModules from "./ExtentModules"
import * as ItemModules from "./ItemModules"
import * as WorkspaceModules from "./WorkspaceModules"

let loaded = false;
export function loadDefaultModules() {
    if (!loaded) {
        ZipCodeModules.loadModules();
        ExtentModules.loadModules();
        ItemModules.loadModules();
        WorkspaceModules.loadModules();
    }
    
    loaded = true;
}