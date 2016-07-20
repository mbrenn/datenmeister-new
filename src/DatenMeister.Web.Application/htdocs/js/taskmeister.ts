import * as DMI from "./datenmeister-interfaces";

export function load(plugin: DMI.Api.PluginParameter): DMI.Api.IPluginResult {
    return {
        onLayoutChanged: (ev) => {
            var tab = ev.layout.getRibbon().getOrAddTab("Tasks");
            if (ev.extent !== undefined && ev.extent !== null) {
                tab.addIcon(
                    "Test",
                    "...",
                    () => {
                        alert('JO');
                    });
            }
        }
    };
}