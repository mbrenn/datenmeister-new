using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Forms.FormModifications;
using DatenMeister.Forms.Helper;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms
{
    // ReSharper disable once UnusedType.Global
    /// <summary>
    /// Defines the default form extensions which are used to navigate through the
    /// items, extens and also offers the simple creation and deletion of items. 
    /// </summary>
    public class TypesFormsPlugin : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly ExtentSettings _extentSettings;

        public TypesFormsPlugin(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
            _extentSettings = scopeStorage.Get<ExtentSettings>();
        }

        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterLoadingOfExtents:

                    var formsPlugin = _scopeStorage.Get<FormsPluginState>();
                    formsPlugin.FormModificationPlugins.Add(new TypeFormModification(_extentSettings));

                    break;
            }
        }

        private class TypeFormModification : IFormModificationPlugin
        {
            private readonly ExtentSettings _extentSettings;

            public TypeFormModification(ExtentSettings extentSettings)
            {
                _extentSettings = extentSettings;
            }

            public bool ModifyForm(FormCreationContext context, IElement form)
            {
                var foundExtentType =
                    _extentSettings.extentTypeSettings.FirstOrDefault(x => x.name == context.ExtentType);
                if (foundExtentType != null && context.FormType == _DatenMeister._Forms.___FormType.ObjectList)
                {
                    foreach (var rootElement in foundExtentType.rootElementMetaClasses)
                    {
                        var defaultTypesForNewElements =
                            form.get<IReflectiveSequence>(_DatenMeister._Forms._ListForm.defaultTypesForNewElements);
                        defaultTypesForNewElements.add(rootElement);
                    }
                }

                return false;
            }
        }
    }
}