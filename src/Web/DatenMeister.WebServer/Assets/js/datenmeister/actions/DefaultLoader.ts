import * as ZipCodeModules from "./ZipCodeModules.js"
import * as ExtentModules from "./ExtentModules.js"
import * as ItemModules from "./ItemModules.js"
import * as WorkspaceModules from "./WorkspaceModules.js"
import * as FormModules from "./FormModules.js"
import * as ActionModules from "./ActionModules.js"
import * as NavigationModules from "./NavigationModules.js"
import * as ClientActionsRender from "./ClientActions.Render.js"

let loaded = false;
export function loadDefaultModules() {
    if (!loaded) {
        ZipCodeModules.loadModules();
        ExtentModules.loadModules();
        ItemModules.loadModules();
        WorkspaceModules.loadModules();
        FormModules.loadModules();
        ActionModules.loadModules();
        NavigationModules.loadModules();
        ClientActionsRender.loadModules();
    }
    
    loaded = true;
}