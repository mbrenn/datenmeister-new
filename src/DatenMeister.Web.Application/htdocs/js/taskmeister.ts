import * as DMI from "./datenmeister-interfaces";

export function Load(plugin: DMI.Api.PluginParameter ) {
    alert(plugin.version);
}