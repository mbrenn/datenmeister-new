import * as DMI from "./datenmeister-interfaces";
import * as DMClient from "./datenmeister-client";


export function load(plugin: DMI.Api.PluginParameter): DMI.Api.IPluginResult {
    return {
        onViewPortChanged: (ev) => {
            var tab = ev.layout.getRibbon().getOrAddTab("Tasks");
            if (ev.extent !== undefined && ev.extent !== null) {
                tab.addIcon(
                    "Add Task",
                    "...",
                    () => {
                        DMClient.ExtentApi.createItem(ev.workspace, ev.extent, undefined, "datenmeister:///types#TaskMeisterLib.Model.IActivity")
                            .done((innerData: DMI.ClientResponse.ICreateItemResult) => {
                                ev.layout.navigateToItem(ev.workspace, ev.extent, innerData.newuri);
                            });
                    });

                tab.addIcon(
                    "Show Tasks",
                    "...,",
                    () => {
                        ev.layout.navigateToItems(ev.workspace, ev.extent, "Views.Tasks.Default");
                    });
            }
        }
    };
}