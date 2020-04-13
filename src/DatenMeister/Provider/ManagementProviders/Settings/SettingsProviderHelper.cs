using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Integration;
using DatenMeister.Runtime.Extents.Configuration;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.ManagementProviders.Settings
{
    public class SettingsProviderHelper
    {
        /// <summary>
        /// Initializes the ExtentHelper and creates the extent for the workspaces
        /// </summary>
        /// <param name="scope">Scope of the datenmeister to be added</param>
        /// <param name="workspaceLogic">The workspace logic to be used</param>
        public static void Initialize(ILifetimeScope scope, IWorkspaceLogic workspaceLogic)
        {
            var settingsExtent =
                new MofUriExtent(new ManagementSettingsProvider(), WorkspaceNames.ManagementSettingExtentUri);
            
            // Adds the extent containing the settings
            workspaceLogic.GetManagementWorkspace().AddExtent(settingsExtent);

            var settings = scope.Resolve<ExtentSettings>();
            settingsExtent.elements().add(settings);
        }

    }
}