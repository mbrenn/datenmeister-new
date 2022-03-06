using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Forms.FormModifications;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms
{
    // ReSharper disable once UnusedType.Global
    /// <summary>
    /// Defines the default form extensions which are used to navigate through the
    /// items, extents and also offers the simple creation and deletion of items. 
    /// </summary>
    public class TypesFormsPlugin : IDatenMeisterPlugin
    {
        private readonly ExtentSettings _extentSettings;
        private readonly IScopeStorage _scopeStorage;

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
                var mofFactory = new MofFactory(form);
                if (foundExtentType != null && context.FormType == _DatenMeister._Forms.___FormType.TreeItemExtent)
                {
                    
                    foreach (var listForm in FormMethods.GetListForms(form))
                    {
                        if (listForm.getOrDefault<IElement>(_DatenMeister._Forms._ListForm.metaClass) != null)
                        {
                            continue;
                        }

                        foreach (var rootElement in foundExtentType.rootElementMetaClasses)
                        {
                            var defaultTypesForNewElements =
                                listForm.get<IReflectiveSequence>(
                                    _DatenMeister._Forms._ListForm
                                        .defaultTypesForNewElements);

                            var defaultType = mofFactory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
                            defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, rootElement);
                            defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.name,
                                NamedElementMethods.GetName(rootElement));
                            defaultTypesForNewElements.add(defaultType);

                            FormMethods.AddToFormCreationProtocol(listForm,
                                $"TypesFormsPlugin: Added {NamedElementMethods.GetName(rootElement)} by ExtentType '{foundExtentType.name}'");
                        }
                    }
                }

                return false;
            }
        }
    }
}