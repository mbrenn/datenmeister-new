using DatenMeister.Plugins;

namespace DatenMeister.WPF.Modules.ReportManager;

public class ReportManagerPlugin : IDatenMeisterPlugin
{
    public void Start(PluginLoadingPosition position)
    {
        GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new DefaultReportManagerViewExtensions());
    }
}