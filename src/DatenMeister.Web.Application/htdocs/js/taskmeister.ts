import * as DMI from "./datenmeister-interfaces";
import * as DMClient from "./datenmeister-client";
import * as DMCI from "./datenmeister-clientinterface";


export function load(plugin: DMI.Plugin.PluginParameter): DMI.Plugin.IPluginResult {
    return {
        onRibbonUpdate: (ev) => {
            var tab = ev.layout.getRibbon().getOrAddTab("Tasks");
            if (ev.viewState.extent !== undefined && ev.viewState.extent !== null) {
                tab.addIcon(
                    "Add Task",
                    "...",
                    () => {
                        DMClient.ExtentApi.createItem(
                                ev.viewState.workspace,
                                ev.viewState.extent,
                                "datenmeister:///types#TaskMeisterLib.Model.IActivity")
                            .done((innerData: DMCI.In.ICreateItemResult) => {
                                ev.navigation.navigateToItem(ev.viewState.workspace, ev.viewState.extent, innerData.newuri);
                            });
                    });

                tab.addIcon(
                    "Show Tasks",
                    "...,",
                    () => {
                        ev.navigation.navigateToItems(ev.viewState.workspace, ev.viewState.extent, "dm:///management/views#Views.Activity.Detail");
                    });
            }
        },
        onViewPortChanged: (ev) => {
            
        }
    };
}