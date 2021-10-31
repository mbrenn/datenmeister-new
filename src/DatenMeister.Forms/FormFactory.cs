using System;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormCreator;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms
{
    public class FormFactory : IFormFactory
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(FormFactory));
        
        private readonly FormsPlugin _plugin;
        private readonly IScopeStorage _scopeStorage;

        /// <summary>
        /// Defines the state for the form plugin
        /// </summary>
        private readonly FormsPluginState _formPluginState;

        public FormFactory(FormsPlugin plugin, IScopeStorage scopeStorage)
        {
            _plugin = plugin;
            _scopeStorage = scopeStorage;
            _formPluginState = scopeStorage.Get<FormsPluginState>();
        }

        /// <summary>
        /// Creates a new instance of the form reportCreator
        /// </summary>
        /// <returns>The created instance of the form reportCreator</returns>
        private FormCreator.FormCreator CreateFormCreator()
            =>  FormCreator.FormCreator.Create(
                _plugin.WorkspaceLogic, _scopeStorage
            );
        

        /// <summary>
        /// Creates a new instance of the form reportCreator
        /// </summary>
        /// <returns>The created instance of the form reportCreator</returns>
        private FormFinder.FormFinder CreateFormFinder()
            =>  new FormFinder.FormFinder(_plugin);
        
        public IElement? CreateExtentFormForItem(IObject element, FormFactoryConfiguration configuration)
        {
            // Checks if the item to which the extent form is requested is an extent
            if (element is IExtent elementAsExtent)
            {
                return CreateExtentFormForExtent(elementAsExtent, configuration);
            }
            
            // Ok, not an extent now do the right things
            IElement? foundForm = null;

            var extent = (element as IHasExtent)?.Extent;
            if (extent == null)
            {
                throw new InvalidOperationException("Item Tree for extent-less object can't be created");
            }

            string? packageViewMode = null;
            // Checks if the current item is a package and if the viewmode
            if (DefaultClassifierHints.IsPackageLike(element))
            {
                packageViewMode = element.getOrDefault<string>(_DatenMeister._CommonTypes._Default._Package.defaultViewMode);
            }

            packageViewMode ??= ViewModes.Default;
            
            if (configuration.ViaFormFinder)
            {
                var viewFinder = CreateFormFinder();
                foundForm = viewFinder.FindFormsFor(new FindFormQuery
                {
                    extentType = extent.GetConfiguration().ExtentType,
                    metaClass = (element as IElement)?.getMetaClass(),
                    FormType = _DatenMeister._Forms.___FormType.TreeItemDetail,
                    viewModeId = configuration.ViewModeId ?? packageViewMode
                }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetItemTreeFormForObject: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateExtentFormForObject(
                    element,
                    extent,
                    new FormFactoryConfiguration() { IncludeOnlyCommonProperties = true });
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                // Adds the extension forms to the found extent
                AddExtensionFormsToExtentForm(
                    foundForm,
                    new FindFormQuery
                    {
                        extentType = extent.GetConfiguration().ExtentType,
                        metaClass = (element as IElement)?.getMetaClass(),
                        FormType = _DatenMeister._Forms.___FormType.TreeItemDetailExtension,
                        viewModeId = configuration.ViewModeId ?? ViewModes.Default
                    });

                if (element is IElement asElement)
                {
                    EvaluateListFormsForAutogenerationByElement(asElement, foundForm);
                }

                CallFormsModificationPlugins(new FormCreationContext
                    {
                        Configuration = configuration,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemDetail,
                        MetaClass = (element as IElement)?.metaclass,
                        DetailElement = element
                    },
                    ref foundForm);
            }

            // No Form
            return foundForm;
        }

        public IElement? CreateDetailFormForItem(IObject element, FormFactoryConfiguration configuration)
        {
            IElement? foundForm = null;
            var extent = (element as IHasExtent)?.Extent;
            
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (configuration.ViaFormFinder)
            {
                // Tries to find the form
                var viewFinder = new FormFinder.FormFinder(_plugin);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        metaClass = (element as IElement)?.getMetaClass(),
                        FormType = _DatenMeister._Forms.___FormType.Detail,
                        extentType = extent == null ? string.Empty : extent.GetConfiguration().ExtentType,
                        viewModeId = configuration.ViewModeId
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetDetailForm: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, we have not found the form. So create one
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateDetailFormForItem(element);
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext
                    {
                        Configuration = configuration,
                        MetaClass = (element as IElement)?.getMetaClass(),
                        FormType = _DatenMeister._Forms.___FormType.Detail,
                        ExtentType = extent?.GetConfiguration().ExtentType ?? string.Empty,
                        DetailElement = element
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        public IElement? CreateExtentFormForItemsMetaClass(IElement metaClass, FormFactoryConfiguration configuration)
        {
            
            IElement? foundForm = null;

            if (configuration.ViaFormFinder)
            {
                // Tries to find the form
                var viewFinder = new FormFinder.FormFinder(_plugin);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        metaClass = metaClass,
                        FormType = _DatenMeister._Forms.___FormType.Detail,
                        viewModeId = configuration.ViewModeId
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetDetailForm: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, we have not found the form. So create one
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateDetailFormByMetaClass(metaClass);
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext
                    {
                        Configuration = configuration,
                        MetaClass = metaClass,
                        FormType = _DatenMeister._Forms.___FormType.Detail
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        public IElement? CreateListFormForCollection(IReflectiveCollection collection, FormFactoryConfiguration configuration)
        {
            IElement? foundForm = null;
            if (configuration.ViaFormCreator)
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                foundForm =  formCreator.CreateListFormForCollection(collection, configuration);
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext
                    {
                        Configuration = configuration,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtent,
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        public IElement? CreateExtentFormForExtent(IExtent extent, FormFactoryConfiguration configuration)
        {
            
            IElement? foundForm = null;
            if (configuration.ViaFormFinder)
            {
                var viewFinder = CreateFormFinder();
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        
                        extentType = extent.GetConfiguration().ExtentType,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtent,
                        viewModeId = configuration.ViewModeId ?? ""
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetExtentForm: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateExtentFormForExtent(
                    extent,
                    new FormFactoryConfiguration());
            }
            
            // Adds the extension forms to the found extent
            if (foundForm != null)
            {
                AddExtensionFormsToExtentForm(
                    foundForm,
                    new FindFormQuery
                    {
                        extentType = extent.GetConfiguration().ExtentType,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtentExtension,
                        viewModeId = configuration.ViewModeId ?? ""
                    });
            }
            
            // 
            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                EvaluateListFormsForAutogenerationByReflectiveCollection(extent.elements(), foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext
                    {
                        Configuration = configuration,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtent,
                        ExtentType = extent.GetConfiguration().ExtentType
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        public IElement? CreateListFormForPropertyValues(IObject parentElement, string propertyName, IElement? propertyType, FormFactoryConfiguration configuration)
        {
            IElement? foundForm = null;
            propertyType ??=
                ClassifierMethods.GetPropertyTypeOfValuesProperty(parentElement as IElement, propertyName);

            if (configuration.ViaFormFinder)
            {
                var viewFinder = new FormFinder.FormFinder(_plugin);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        extentType = (parentElement as IHasExtent)?.Extent?.GetConfiguration().ExtentType ?? string.Empty,
                        parentMetaClass = (parentElement as IElement)?.metaclass,
                        metaClass = propertyType,
                        FormType = _DatenMeister._Forms.___FormType.ObjectList,
                        parentProperty = propertyName
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetListFormForElementsProperty: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateListFormForCollection(
                    parentElement.get<IReflectiveCollection>(propertyName),
                    new FormFactoryConfiguration { IncludeOnlyCommonProperties = true });
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext
                    {
                        Configuration = configuration,
                        FormType = _DatenMeister._Forms.___FormType.ObjectList,
                        MetaClass = (parentElement as IElement)?.metaclass,
                        ParentPropertyName = propertyName,
                        DetailElement = parentElement
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        /// <summary>
        /// Calls all the form modification plugins, if allowed. 
        /// </summary>
        /// <param name="formCreationContext">The creation context used by the plugins</param>
        /// <param name="form">The form that is evaluated</param>
        private void CallFormsModificationPlugins(FormCreationContext formCreationContext, ref IElement form)
        {
            if (formCreationContext.Configuration?.AllowFormModifications != true)
            {
                return; // Nothing to do
            }

            foreach (var plugin in _formPluginState.FormModificationPlugins)
            {
                plugin.ModifyForm(formCreationContext, form);
            }
        }
        

        /// <summary>
        /// Adds all found extension forms.
        /// These extensions are required, so the user can also configure special tabs via the data 
        /// </summary>
        /// <param name="form">Gives the extent form that will be extended.
        /// Must be of type ExtentForm.</param>
        /// <param name="query">Defines the query to be evaluated</param>
        private void AddExtensionFormsToExtentForm(
            IObject form,
            FindFormQuery query)
        {
            var viewFinder = CreateFormFinder();
            var foundForms = viewFinder.FindFormsFor(query);

            var tabs = form.get<IReflectiveSequence>(_DatenMeister._Forms._ExtentForm.tab);
            foreach (var listForm in foundForms)
            {
                tabs.add(listForm);
            }
        }
        

        /// <summary>
        /// Goes through the tabs the extent form and checks whether the listform required an autogeneration
        /// </summary>
        /// <param name="reflectiveCollection">The reflective collection to be used</param>
        /// <param name="foundForm">The element that has been found</param>
        private void EvaluateListFormsForAutogenerationByReflectiveCollection(IReflectiveCollection reflectiveCollection, IElement foundForm)
        {
            // Go through the list forms and check if we need to auto-populate
            foreach (var tab in
                foundForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab).OfType<IElement>())
            {
                var tabMetaClass = tab.getMetaClass();
                if (tabMetaClass == null ||
                    !tabMetaClass.equals(_DatenMeister.TheOne.Forms.__ListForm))
                {
                    // Not a list tab
                    continue;
                }

                var autoGenerate = tab.getOrDefault<bool>(_DatenMeister._Forms._ListForm.autoGenerateFields);
                if (autoGenerate)
                {
                    var formCreator = CreateFormCreator();
                    formCreator.AddToListFormByElements(tab, reflectiveCollection,
                        new FormFactoryConfiguration());
                }
            }
        }

        /// <summary>
        /// Goes through the tabs the extent form and checks whether the listform required an autogeneration.
        /// Each tab within the list form can require an autogeneration by setting the field 'autoGenerateFields'.
        /// </summary>
        /// <param name="element">The element to be used</param>
        /// <param name="foundForm">The element that has been found</param>
        private void EvaluateListFormsForAutogenerationByElement(IElement element, IElement foundForm)
        {
            // Go through the list forms and check if we need to auto-populate
            foreach (var tab in
                foundForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab).OfType<IElement>())
            {
                var tabMetaClass = tab.getMetaClass();
                if (tabMetaClass == null ||
                    !tabMetaClass.equals(_DatenMeister.TheOne.Forms.__ListForm))
                {
                    // Not a list tab
                    continue;
                }

                var autoGenerate = tab.getOrDefault<bool>(_DatenMeister._Forms._ListForm.autoGenerateFields);
                if (autoGenerate)
                {
                    var formCreator = CreateFormCreator();
                    var propertyName = tab.getOrDefault<string>(_DatenMeister._Forms._ListForm.property);
                    if (propertyName == null || string.IsNullOrEmpty(propertyName))
                    {
                        formCreator.AddToListFormByElements(
                            tab,
                            new PropertiesAsReflectiveCollection(element), 
                            new FormFactoryConfiguration());
                    }
                    else
                    {
                        var reflectiveSequence = element.getOrDefault<IReflectiveCollection>(propertyName);
                        if (reflectiveSequence != null)
                        {
                            formCreator.AddToListFormByElements(
                                tab,
                                reflectiveSequence,
                                new FormFactoryConfiguration());
                        }
                    }
                }
            }
        }

        private static IElement CloneForm(IElement foundForm)
        {
            var originalUrl = foundForm.GetUri();
            foundForm = ObjectCopier.Copy(InMemoryObject.TemporaryFactory, foundForm, new CopyOption());
            if (originalUrl != null)
            {
                foundForm.set(_DatenMeister._Forms._Form.originalUri, originalUrl);
            }

            return foundForm;
        }
    }
}