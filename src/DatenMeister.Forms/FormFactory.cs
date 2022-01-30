using System;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
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
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms
{
    public class FormFactory : IFormFactory
    {
        /// <summary>
        ///     Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(FormFactory));

        /// <summary>
        ///     Defines the state for the form plugin
        /// </summary>
        private readonly FormsPluginState _formPluginState;

        private readonly FormsPlugin _plugin;
        private readonly IScopeStorage _scopeStorage;

        public FormFactory(FormsPlugin plugin, IScopeStorage scopeStorage)
        {
            _plugin = plugin;
            _scopeStorage = scopeStorage;
            _formPluginState = scopeStorage.Get<FormsPluginState>();
        }

        public IElement? CreateExtentFormForItem(IObject element, FormFactoryConfiguration configuration)
        {
            var metaClass = (element as IElement)?.getMetaClass();

            // Checks if the item to which the extent form is requested is an extent
            if (element is IExtent elementAsExtent) return CreateExtentFormForExtent(elementAsExtent, configuration);

            // Ok, not an extent now do the right things
            IElement? foundForm = null;

            var extent = (element as IHasExtent)?.Extent;
            if (extent == null)
                throw new InvalidOperationException("Item Tree for extent-less object can't be created");

            var extentType = extent.GetConfiguration().ExtentType;

            string? packageViewMode = null;
            // Checks if the current item is a package and if the viewmode
            if (DefaultClassifierHints.IsPackageLike(element))
                packageViewMode =
                    element.getOrDefault<string>(_DatenMeister._CommonTypes._Default._Package.defaultViewMode);

            packageViewMode = string.IsNullOrEmpty(packageViewMode) ? ViewModes.Default : packageViewMode;

            if (configuration.ViaFormFinder)
            {
                var viewFinder = CreateFormFinder();
                foundForm = viewFinder.FindFormsFor(new FindFormQuery
                {
                    extentType = extentType,
                    metaClass = metaClass,
                    FormType = _DatenMeister._Forms.___FormType.TreeItemDetail,
                    viewModeId = configuration.ViewModeId ?? packageViewMode
                }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("CreateExtentFormForItem: Found form: " + NamedElementMethods.GetFullName(foundForm));
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory] Found Form via FormFinder: " + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateExtentFormForObject(
                    element,
                    extent,
                    new FormFactoryConfiguration { IncludeOnlyCommonProperties = true });

                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory] Created Form via FormCreator");
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

                if (element is IElement asElement) EvaluateListFormsForAutogenerationByElement(asElement, foundForm);

                var formCreationContext = new FormCreationContext
                {
                    Configuration = configuration,
                    FormType = _DatenMeister._Forms.___FormType.TreeItemDetail,
                    MetaClass = (element as IElement)?.metaclass,
                    DetailElement = element
                };

                CallPluginsForExtentForm(formCreationContext, ref foundForm);
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
                    Logger.Info("CreateDetailFormForItem: Found form: " + NamedElementMethods.GetFullName(foundForm));
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory] Found Form via FormFinder: " + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, we have not found the form. So create one
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateDetailFormForItem(element);
                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory] Created Form via FormCreator");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);

                ExpandDropDownValuesOfValueReference(foundForm);

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
                    Logger.Info("CreateExtentFormForItemsMetaClass: Found form: " +
                                NamedElementMethods.GetFullName(foundForm));
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory] Found Form via FormFinder: " + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, we have not found the form. So create one
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateDetailFormByMetaClass(metaClass);
                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory] Found Form via FormCreator");
            }

            if (foundForm != null &&
                foundForm.equals(_DatenMeister.TheOne.Forms.__ExtentForm) != true)
            {
                foundForm = FormCreator.FormCreator.CreateExtentFormFromTabs(foundForm);
                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory] Transformed Form to Extent Form");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);

                var formCreationContext = new FormCreationContext
                {
                    Configuration = configuration,
                    MetaClass = metaClass,
                    FormType = _DatenMeister._Forms.___FormType.Detail
                };

                CallPluginsForExtentForm(formCreationContext, ref foundForm);
            }

            return foundForm;
        }

        public IElement? CreateListFormForCollection(IReflectiveCollection collection,
            FormFactoryConfiguration configuration)
        {
            configuration = configuration with { IsForListView = true };
            IElement? foundForm = null;
            if (configuration.ViaFormCreator)
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateListFormForCollection(collection, configuration);
                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory] Created Form via FormCreator");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                ExpandDropDownValuesOfValueReference(foundForm);

                CallFormsModificationPlugins(new FormCreationContext
                    {
                        Configuration = configuration,
                        FormType = _DatenMeister._Forms.___FormType.ObjectList
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        public IElement? CreateListFormForMetaClass(IElement metaClass, FormFactoryConfiguration configuration)
        {
            configuration = configuration with { IsForListView = true };
            IElement? foundForm = null;

            if (configuration.ViaFormFinder)
            {
                // Tries to find the form
                var viewFinder = new FormFinder.FormFinder(_plugin);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        metaClass = metaClass,
                        FormType = _DatenMeister._Forms.___FormType.ObjectList,
                        viewModeId = configuration.ViewModeId
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("CreateListFormForMetaClass: Found form: " +
                                NamedElementMethods.GetFullName(foundForm));

                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory] Found Form via FormFinder" + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, we have not found the form. So create one
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateListFormForMetaClass(metaClass, configuration);

                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory] Created Form via FormCreator");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                ExpandDropDownValuesOfValueReference(foundForm);

                CallFormsModificationPlugins(new FormCreationContext
                    {
                        Configuration = configuration,
                        MetaClass = metaClass,
                        FormType = _DatenMeister._Forms.___FormType.ObjectList
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        public IElement? CreateExtentFormForExtent(IExtent extent, FormFactoryConfiguration configuration)
        {
            var extentType = extent.GetConfiguration().ExtentType;
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
                    FormMethods.AddToFormCreationProtocol(
                        foundForm,
                        $"[FormFactory] Found Form via FormFinder {foundForm.GetUri()}");
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateExtentFormForExtent(
                    extent,
                    new FormFactoryConfiguration());

                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory] Created Form via FormCreator");
            }

            // Adds the extension forms to the found extent
            if (foundForm != null)
                AddExtensionFormsToExtentForm(
                    foundForm,
                    new FindFormQuery
                    {
                        extentType = extent.GetConfiguration().ExtentType,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtentExtension,
                        viewModeId = configuration.ViewModeId ?? ""
                    });

            // 
            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);

                EvaluateListFormsForAutogenerationByReflectiveCollection(extent.elements(), foundForm);

                var formCreationContext = new FormCreationContext
                {
                    DetailElement = extent,
                    Configuration = configuration,
                    FormType = _DatenMeister._Forms.___FormType.TreeItemExtent,
                    ExtentType = extent.GetConfiguration().ExtentType
                };

                CallPluginsForExtentForm(formCreationContext, ref foundForm);
            }

            return foundForm;
        }

        public IElement? CreateListFormForPropertyValues(IObject? parentElement, string propertyName,
            IElement? propertyType, FormFactoryConfiguration configuration)
        {
            configuration = configuration with { IsForListView = true };
            IElement? foundForm = null;
            propertyType ??=
                ClassifierMethods.GetPropertyTypeOfValuesProperty(parentElement as IElement, propertyName);

            if (configuration.ViaFormFinder)
            {
                var viewFinder = new FormFinder.FormFinder(_plugin);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        extentType = (parentElement as IHasExtent)?.Extent?.GetConfiguration().ExtentType ??
                                     string.Empty,
                        parentMetaClass = (parentElement as IElement)?.metaclass,
                        metaClass = propertyType,
                        FormType = _DatenMeister._Forms.___FormType.ObjectList,
                        parentProperty = propertyName
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetListFormForElementsProperty: Found form: " +
                                NamedElementMethods.GetFullName(foundForm));
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory] Found Form via FormFinder: " + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateListFormForCollection(
                    parentElement.get<IReflectiveCollection>(propertyName),
                    new FormFactoryConfiguration { IncludeOnlyCommonProperties = true });

                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory] Found Form via FormCreator");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                ExpandDropDownValuesOfValueReference(foundForm);

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
        ///     Calls all the plugins for the extent form
        /// </summary>
        /// <param name="formCreationContext">Form Creation context to be used</param>
        /// <param name="foundForm">The form being found</param>
        /// <returns>Element being called</returns>
        private void CallPluginsForExtentForm(FormCreationContext formCreationContext, ref IElement foundForm)
        {
            CallFormsModificationPlugins(
                formCreationContext,
                ref foundForm);

            var detailForms = FormMethods.GetDetailForms(foundForm);
            foreach (var detailForm in detailForms)
            {
                var listedForm = detailForm; // Get iterative

                ExpandDropDownValuesOfValueReference(listedForm);

                CallFormsModificationPlugins(
                    formCreationContext with { FormType = _DatenMeister._Forms.___FormType.Detail },
                    ref listedForm);
            }

            var listForms = FormMethods.GetListForms(foundForm);
            foreach (var listForm in listForms)
            {
                var listedForm = listForm; // Get iterative

                ExpandDropDownValuesOfValueReference(listedForm);

                CallFormsModificationPlugins(
                    formCreationContext with { FormType = _DatenMeister._Forms.___FormType.ObjectList },
                    ref listedForm);
            }
        }

        /// <summary>
        ///     Creates a new instance of the form reportCreator
        /// </summary>
        /// <returns>The created instance of the form reportCreator</returns>
        private FormCreator.FormCreator CreateFormCreator()
        {
            return FormCreator.FormCreator.Create(
                _plugin.WorkspaceLogic, _scopeStorage, this
            );
        }


        /// <summary>
        ///     Creates a new instance of the form reportCreator
        /// </summary>
        /// <returns>The created instance of the form reportCreator</returns>
        private FormFinder.FormFinder CreateFormFinder()
        {
            return new(_plugin);
        }

        /// <summary>
        ///     Calls all the form modification plugins, if allowed.
        /// </summary>
        /// <param name="formCreationContext">The creation context used by the plugins</param>
        /// <param name="form">The form that is evaluated</param>
        private void CallFormsModificationPlugins(FormCreationContext formCreationContext, ref IElement form)
        {
            if (formCreationContext.Configuration?.AllowFormModifications != true) return; // Nothing to do

            foreach (var plugin in _formPluginState.FormModificationPlugins)
                if (plugin.ModifyForm(formCreationContext, form))
                    FormMethods.AddToFormCreationProtocol(form, $"[FormFactory] Modified via plugin: {plugin}");
        }

        /// <summary>
        ///     Adds all found extension forms.
        ///     These extensions are required, so the user can also configure special tabs via the data
        /// </summary>
        /// <param name="form">
        ///     Gives the extent form that will be extended.
        ///     Must be of type ExtentForm.
        /// </param>
        /// <param name="query">Defines the query to be evaluated</param>
        private void AddExtensionFormsToExtentForm(
            IObject form,
            FindFormQuery query)
        {
            var viewFinder = CreateFormFinder();
            var foundForms = viewFinder.FindFormsFor(query);

            var tabs = form.get<IReflectiveSequence>(_DatenMeister._Forms._ExtentForm.tab);
            foreach (var listForm in foundForms) tabs.add(listForm);
        }

        /// <summary>
        ///     Goes through the tabs the extent form and checks whether the listform required an autogeneration
        /// </summary>
        /// <param name="reflectiveCollection">The reflective collection to be used</param>
        /// <param name="foundForm">The element that has been found</param>
        private void EvaluateListFormsForAutogenerationByReflectiveCollection(
            IReflectiveCollection reflectiveCollection, IElement foundForm)
        {
            // Go through the list forms and check if we need to auto-populate
            foreach (var tab in
                     foundForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab)
                         .OfType<IElement>())
            {
                var tabMetaClass = tab.getMetaClass();
                if (tabMetaClass == null ||
                    !tabMetaClass.equals(_DatenMeister.TheOne.Forms.__ListForm))
                    // Not a list tab
                    continue;

                var autoGenerate = tab.getOrDefault<bool>(_DatenMeister._Forms._ListForm.autoGenerateFields);
                if (autoGenerate)
                {
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        $"[FormFactory] Auto Creation of fields by Reflective Collection: {NamedElementMethods.GetName(tab)}");

                    var formCreator = CreateFormCreator();
                    formCreator.AddToListFormByElements(tab, reflectiveCollection,
                        new FormFactoryConfiguration());
                }
            }
        }

        /// <summary>
        ///     Expands the dropdown values of the the DropDownField.
        ///     The DropDownField supports a reference field which is not resolved by every Form Client.
        ///     So, the DropDownField can already be resolved on server side
        /// </summary>
        /// <param name="listOrDetailForm">The list form or the DetailForm being handled</param>
        public void ExpandDropDownValuesOfValueReference(IElement listOrDetailForm)
        {
            var factory = new MofFactory(listOrDetailForm);
            var fields = listOrDetailForm.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field);
            foreach (var field in fields.OfType<IElement>())
            {
                if (field.getMetaClass()?.equals(_DatenMeister.TheOne.Forms.__DropDownFieldData) != true) continue;

                var byEnumeration =
                    field.getOrDefault<IElement>(_DatenMeister._Forms._DropDownFieldData.valuesByEnumeration);
                var byValues =
                    field.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DropDownFieldData.values);
                if (byValues == null && byEnumeration != null)
                {
                    var enumeration = EnumerationMethods.GetEnumValues(byEnumeration);
                    foreach (var value in enumeration)
                    {
                        var element = factory.create(_DatenMeister.TheOne.Forms.__ValuePair);
                        element.set(_DatenMeister._Forms._ValuePair.name, value);
                        element.set(_DatenMeister._Forms._ValuePair.value, value);
                        field.AddCollectionItem(_DatenMeister._Forms._DropDownFieldData.values, element);
                    }

                    FormMethods.AddToFormCreationProtocol(listOrDetailForm,
                        $"[FormFactory] Expanded DropDown-Values for {NamedElementMethods.GetName(field)}");
                }
            }
        }

        /// <summary>
        ///     Goes through the tabs the extent form and checks whether the listform required an autogeneration.
        ///     Each tab within the list form can require an autogeneration by setting the field 'autoGenerateFields'.
        /// </summary>
        /// <param name="element">The element to be used</param>
        /// <param name="foundForm">The element that has been found</param>
        private void EvaluateListFormsForAutogenerationByElement(IObject element, IElement foundForm)
        {
            var listForms = FormMethods.GetListForms(foundForm);
            foreach (var tab in listForms)
            {
                var tabMetaClass = tab.getMetaClass();
                if (tabMetaClass == null ||
                    !tabMetaClass.equals(_DatenMeister.TheOne.Forms.__ListForm))
                    // Not a list tab
                    continue;

                var autoGenerate = tab.getOrDefault<bool>(_DatenMeister._Forms._ListForm.autoGenerateFields);
                if (autoGenerate)
                {
                    var formCreator = CreateFormCreator();
                    var propertyName = tab.getOrDefault<string>(_DatenMeister._Forms._ListForm.property);
                    if (propertyName == null || string.IsNullOrEmpty(propertyName))
                    {
                        FormMethods.AddToFormCreationProtocol(foundForm,
                            $"[FormFactory] Auto Creation of fields by Element: {NamedElementMethods.GetName(tab)}");

                        formCreator.AddToListFormByElements(
                            tab,
                            new PropertiesAsReflectiveCollection(element),
                            new FormFactoryConfiguration());
                    }
                    else
                    {
                        FormMethods.AddToFormCreationProtocol(foundForm,
                            $"[FormFactory] Auto Creation of fields by Element: {NamedElementMethods.GetName(tab)}");

                        var reflectiveSequence = element.getOrDefault<IReflectiveCollection>(propertyName);
                        if (reflectiveSequence != null)
                            formCreator.AddToListFormByElements(
                                tab,
                                reflectiveSequence,
                                new FormFactoryConfiguration());
                    }
                }
            }
        }

        private static IElement CloneForm(IElement foundForm)
        {
            var originalUrl = foundForm.GetUri();
            foundForm = ObjectCopier.Copy(InMemoryObject.TemporaryFactory, foundForm, new CopyOption());
            if (originalUrl != null) foundForm.set(_DatenMeister._Forms._Form.originalUri, originalUrl);

            return foundForm;
        }
    }
}