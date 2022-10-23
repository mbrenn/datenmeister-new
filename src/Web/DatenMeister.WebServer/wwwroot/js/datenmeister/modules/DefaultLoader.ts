import * as ZipCodeModules from "./ZipCodeModules"

let loaded = false;
export function loadDefaultModules() {
    if (!loaded) {
        ZipCodeModules.loadModules();
    }
    
    loaded = true;
}