﻿using DatenMeister.Excel.Properties;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Uml.Helper;
using DatenMeister.Uml.Plugin;

namespace DatenMeister.WPF.Modules.FormManager
{
    /// <summary>
    /// Defines the plugin for the view manager
    /// </summary>
    [UsedImplicitly]
    public class FormManagerPlugin : IDatenMeisterPlugin
    {
        private readonly PackageMethods _packageMethods;

        private readonly ViewLogic _viewLogic;

        public const string PackageName = "FormManager";

        public FormManagerPlugin(PackageMethods packageMethods, ViewLogic viewLogic)
        {
            _packageMethods = packageMethods;
            _viewLogic = viewLogic;
        }

        /// <inheritdoc />
        public void Start(PluginLoadingPosition position)
        {
            GuiObjectCollection.TheOne.ViewExtensionFactories.Add(new FormManagerViewExtension());

            _packageMethods.ImportByManifest(
                typeof(UmlPlugin),
                "DatenMeister.XmiFiles.Views.Formmanager.xmi",
                PackageName,
                _viewLogic.GetInternalViewExtent(),
                PackageName);
        }
    }
}