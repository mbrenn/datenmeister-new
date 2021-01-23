using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DatenMeister.Modules.PublicSettings;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Plugins;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;

namespace DatenMeister.WPF.Modules.PublicSettings
{
    public class PublicSettingsSupport : IDatenMeisterPlugin
    {
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new PublicSettingsViewExtension(this));
        }
    }

    public class PublicSettingsViewExtension : IViewExtensionFactory
    {
        private readonly PublicSettingsSupport _publicSettingsSupport;

        public PublicSettingsViewExtension(PublicSettingsSupport publicSettingsSupport)
        {
            _publicSettingsSupport = publicSettingsSupport;
        }

        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionInfo viewExtensionInfo)
        {
            var applicationWindow = viewExtensionInfo.GetMainApplicationWindow();
            if (applicationWindow != null)
            {
                yield return new ApplicationMenuButtonDefinition(
                    "Create Example Public Settings",
                    () =>
                    {
                        var creator = new PublicSettingsCreator();
                        if (creator.DoesFileExist())
                        {
                            MessageBox.Show("The public Settings file is already existing. ");
                        }
                        else
                        {
                            creator.CreateExampleFile();
                        }

                        DotNetHelper.OpenExplorer(PublicSettingsCreator.GetPublicSettingsPath());
                    },
                    null, 
                    "Admin");
            }
        }
    }
}