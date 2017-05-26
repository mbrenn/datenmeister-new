import * as DMI from "./datenmeister-interfaces";
import * as DMClient from "./datenmeister-client";
import * as DMCI from "./datenmeister-clientinterface";
import * as DMView from "./datenmeister-view";


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
                                DMView.ItemDetail.navigateToItem(ev.layout.mainViewPort, ev.viewState.workspace, ev.viewState.extent, innerData.newuri);
                            });
                    });

                tab.addIcon(
                    "Show Tasks",
                    "...,",
                    () => {
                        DMView.ItemList.navigateToItems(ev.layout.mainViewPort, ev.viewState.workspace, ev.viewState.extent, "dm:///management/views#Views.Activity.Detail");
                    });
            }
        },
        onViewPortChanged: (ev) => {
            
        }
    };
}