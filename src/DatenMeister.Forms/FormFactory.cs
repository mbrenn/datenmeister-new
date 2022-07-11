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
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
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
        private readonly FormMethods _plugin;
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public FormFactory(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _plugin = new FormMethods(workspaceLogic, scopeStorage);
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
            _formPluginState = scopeStorage.Get<FormsPluginState>();
        }

        public IElement? CreateObjectFormForItem(IObject element, FormFactoryConfiguration configuration)
        {
            var metaClass = (element as IElement)?.getMetaClass();

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
                    FormType = _DatenMeister._Forms.___FormType.Object,
                    viewModeId = configuration.ViewModeId ?? packageViewMode
                }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("CreateObjectFormForItem: Found form: " + NamedElementMethods.GetFullName(foundForm));
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory.CreateObjectFormForItem] Found Form via FormFinder: " + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateObjectFormForItem(
                    element,
                    extent,
                    new FormFactoryConfiguration { IncludeOnlyCommonProperties = true, AllowFormModifications = false});

                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateObjectFormForItem] Created Form via FormCreator");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);

                // Adds the extension forms to the found extent
                AddExtensionFormsToObjectForm(
                    foundForm,
                    new FindFormQuery
                    {
                        extentType = extent.GetConfiguration().ExtentType,
                        metaClass = (element as IElement)?.getMetaClass(),
                        FormType = _DatenMeister._Forms.___FormType.ObjectExtension,
                        viewModeId = configuration.ViewModeId ?? ViewModes.Default
                    });

                if (element is IElement asElement)
                {
                    EvaluateListFormsForAutogenerationByElement(asElement, foundForm);
                    
                    // This call is required to add the new buttons to the list form 
                    // in case the creator of the form did not have these buttons included
                    FormMethods.AddDefaultTypesInListFormByElementsProperty(foundForm, asElement);
                }
                
                var formCreationContext = new FormCreationContext
                {
                    FormType = _DatenMeister._Forms.___FormType.Object,
                    MetaClass = (element as IElement)?.metaclass,
                    DetailElement = element
                };

                CallPluginsForCollectionOrObjectForm(configuration, formCreationContext, ref foundForm);
                
                CleanupObjectForm(foundForm, true);
            }

            // No Form
            return foundForm;
        }

        public IElement? CreateRowFormByMetaClass(IElement metaClass, FormFactoryConfiguration? configuration)
        {
            // Ok, not an extent now do the right things
            IElement? rowForm = null;
            configuration ??= new FormFactoryConfiguration();

            if (configuration.ViaFormFinder)
            {
                var viewFinder = CreateFormFinder();
                rowForm = viewFinder.FindFormsFor(new FindFormQuery
                {
                    metaClass = metaClass,
                    FormType = _DatenMeister._Forms.___FormType.Row,
                    viewModeId = configuration.ViewModeId ?? ViewModes.Default
                }).FirstOrDefault();

                if (rowForm != null)
                {
                    Logger.Info("CreateRowFormByMetaClass: Found form: " + NamedElementMethods.GetFullName(rowForm));
                    FormMethods.AddToFormCreationProtocol(rowForm,
                        "[FormFactory.CreateRowFormByMetaClass] Found Form via FormFinder: " + rowForm.GetUri());
                }
            }

            if (rowForm == null && configuration.ViaFormCreator)
            {
                var formCreator = CreateFormCreator();
                rowForm = formCreator.CreateRowFormByMetaClass(
                    metaClass,
                    new FormFactoryConfiguration { IncludeOnlyCommonProperties = true, AllowFormModifications = false});

                FormMethods.AddToFormCreationProtocol(rowForm, "[FormFactory.CreateRowFormByMetaClass] Created Form via FormCreator");
            }

            if (rowForm != null)
            {
                rowForm = CloneForm(rowForm);

                // Adds the extension forms to the found extent
                AddExtensionFormsToObjectForm(
                    rowForm,
                    new FindFormQuery
                    {
                        metaClass = metaClass?.getMetaClass(),
                        FormType = _DatenMeister._Forms.___FormType.ObjectExtension,
                        viewModeId = configuration.ViewModeId ?? ViewModes.Default
                    });
                
                var formCreationContext = new FormCreationContext
                {
                    FormType = _DatenMeister._Forms.___FormType.Object,
                    MetaClass = metaClass
                };

                CallPluginsForRowOrTableForm(configuration, formCreationContext, ref rowForm);
                
                CleanupRowForm(rowForm);
            }

            // No Form
            return rowForm;
        }

        public IElement? CreateRowFormForItem(IObject element, FormFactoryConfiguration configuration)
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
                        FormType = _DatenMeister._Forms.___FormType.Row,
                        extentType = extent == null ? string.Empty : extent.GetConfiguration().ExtentType,
                        viewModeId = configuration.ViewModeId
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("CreateRowFormForItem: Found form: " + NamedElementMethods.GetFullName(foundForm));
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory.CreateRowFormForItem] Found Form via FormFinder: " + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, we have not found the form. So create one
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateRowFormForItem(element);
                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateRowFormForItem] Created Form via FormCreator");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);

                _formPluginState.CallFormsModificationPlugins(
                    configuration,
                    new FormCreationContext
                    {
                        MetaClass = (element as IElement)?.getMetaClass(),
                        FormType = _DatenMeister._Forms.___FormType.Row,
                        ExtentType = extent?.GetConfiguration().ExtentType ?? string.Empty,
                        DetailElement = element
                    },
                    ref foundForm);
                
                CleanupRowForm(foundForm);
            }

            return foundForm;
        }

        public IElement? CreateCollectionFormForItemsMetaClass(IElement? metaClass, FormFactoryConfiguration configuration)
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
                        FormType = _DatenMeister._Forms.___FormType.Collection,
                        viewModeId = configuration.ViewModeId
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("CreateCollectionFormForItemsMetaClass: Found form: " +
                                NamedElementMethods.GetFullName(foundForm));
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory.CreateCollectionFormForItemsMetaClass] Found Form via FormFinder: " + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, we have not found the form. So create one
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateCollectionFormForItemsMetaClass(
                    metaClass,
                    configuration with { AllowFormModifications = false});
                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateCollectionFormForItemsMetaClass] Found Form via FormCreator");
            }

            if (foundForm != null &&
                foundForm.equals(_DatenMeister.TheOne.Forms.__CollectionForm) != true && 
            foundForm.equals(_DatenMeister.TheOne.Forms.__ObjectForm) != true)
            {
                foundForm = FormCreator.FormCreator.CreateCollectionFormFromTabs(foundForm);
                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateCollectionFormForItemsMetaClass] Transformed Form to Extent Form");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);

                var formCreationContext = new FormCreationContext
                {
                    MetaClass = metaClass,
                    FormType = _DatenMeister._Forms.___FormType.Collection
                };

                CallPluginsForCollectionOrObjectForm(configuration, formCreationContext, ref foundForm);
                CleanupObjectForm(foundForm, true);
            }

            return foundForm;
        }

        public IElement? CreateTableFormForCollection(IReflectiveCollection collection,
            FormFactoryConfiguration configuration)
        {
            configuration = configuration with { IsForTableForm = true };
            IElement? foundForm = null;
            if (configuration.ViaFormCreator)
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateTableFormForCollection(collection,
                    configuration with { AllowFormModifications = false });
                FormMethods.AddToFormCreationProtocol(foundForm,
                    "[FormFactory.CreateListFormForCollection] Created Form via FormCreator");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                SetDefaultTypesByPackages(collection as IHasExtent, foundForm);

                _formPluginState.CallFormsModificationPlugins(
                    configuration, new FormCreationContext
                    {
                        FormType = _DatenMeister._Forms.___FormType.Object
                    },
                    ref foundForm);

                FormMethods.CleanupTableForm(foundForm);
            }

            return foundForm;
        }

        public IElement? CreateCollectionFormForExtent(IExtent extent, FormFactoryConfiguration configuration)
        {
            var extentType = extent.GetConfiguration().ExtentType;
            IElement? foundForm = null;
            if (configuration.ViaFormFinder)
            {
                var viewFinder = CreateFormFinder();
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        extentType = extentType,
                        FormType = _DatenMeister._Forms.___FormType.Collection,
                        viewModeId = configuration.ViewModeId ?? ""
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetExtentForm: Found form: " + NamedElementMethods.GetFullName(foundForm));
                    FormMethods.AddToFormCreationProtocol(
                        foundForm,
                        $"[FormFactory.CreateExtentFormForExtent] Found Form via FormFinder {foundForm.GetUri()}");
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateCollectionFormForExtent(
                    extent,
                    configuration);

                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateExtentFormForExtent] Created Form via FormCreator");
            }

            // Adds the extension forms to the found extent
            if (foundForm != null)
                AddExtensionFormsToObjectForm(
                    foundForm,
                    new FindFormQuery
                    {
                        extentType = extentType,
                        FormType = _DatenMeister._Forms.___FormType.CollectionExtension,
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
                    FormType = _DatenMeister._Forms.___FormType.Collection,
                    ExtentType = extentType
                };

                CallPluginsForCollectionOrObjectForm(configuration, formCreationContext, ref foundForm);
                
                CleanupObjectForm(foundForm, true);
            }

            return foundForm;
        }

        public IElement? CreateTableFormForMetaClass(
            IElement? metaClass,
            FormFactoryConfiguration configuration)
        {
            configuration = configuration with { IsForTableForm = true };
            IElement? foundForm = null;

            if (configuration.ViaFormFinder)
            {
                // Tries to find the form
                var viewFinder = new FormFinder.FormFinder(_plugin);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        metaClass = metaClass,
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        viewModeId = configuration.ViewModeId
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("CreateListFormForMetaClass: Found form: " +
                                NamedElementMethods.GetFullName(foundForm));

                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory.CreateListFormForMetaClass] Found Form via FormFinder" + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                // Ok, we have not found the form. So create one
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateTableFormForMetaClass(metaClass, configuration);

                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateListFormForMetaClass] Created Form via FormCreator");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                _formPluginState.CallFormsModificationPlugins(
                    configuration,
                    new FormCreationContext
                    {
                        MetaClass = metaClass,
                        FormType = _DatenMeister._Forms.___FormType.Table
                    },
                    ref foundForm);

                FormMethods.CleanupTableForm(foundForm);
            }

            return foundForm;
        }

        /// <summary>
        /// Creates a list form for a certain property which contains a collection
        /// </summary>
        /// <param name="parentElement">The element to which the property belows</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="propertyType">The type of the property</param>
        /// <param name="configuration">The configuration being used to creates the list form</param>
        /// <returns>The created listform</returns>
        public IElement? CreateTableFormForPropertyValues(IObject? parentElement, string propertyName,
            IElement? propertyType, FormFactoryConfiguration configuration)
        {
            configuration = configuration with { IsForTableForm = true };
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
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        parentProperty = propertyName
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetListFormForElementsProperty: Found form: " +
                                NamedElementMethods.GetFullName(foundForm));
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        "[FormFactory.CreateListFormForPropertyValues] Found Form via FormFinder: " + foundForm.GetUri());
                }
            }

            if (foundForm == null && configuration.ViaFormCreator)
            {
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateTableFormForCollection(
                    parentElement.get<IReflectiveCollection>(propertyName),
                    new FormFactoryConfiguration { IncludeOnlyCommonProperties = true });

                FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateListFormForPropertyValues] Found Form via FormCreator");
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                SetDefaultTypesByPackages(parentElement as IHasExtent, foundForm);

                _formPluginState.CallFormsModificationPlugins(configuration,
                    new FormCreationContext
                    {
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        MetaClass = (parentElement as IElement)?.metaclass,
                        ParentPropertyName = propertyName,
                        DetailElement = parentElement
                    },
                    ref foundForm);

                FormMethods.CleanupTableForm(foundForm);
            }

            return foundForm;
        }

        /// <summary>
        ///     Calls all the plugins for a possible row or table form
        /// </summary>
        /// <param name="configuration">Configuration of the formfactory</param>
        /// <param name="formCreationContext">Form Creation context to be used</param>
        /// <param name="foundForm">The form being found</param>
        /// <returns>Element being called</returns>
        private void CallPluginsForRowOrTableForm(
            FormFactoryConfiguration configuration,
            FormCreationContext formCreationContext,
            ref IElement foundForm)
        {
            _formPluginState.CallFormsModificationPlugins(
                configuration,
                formCreationContext,
                ref foundForm);
        }

        /// <summary>
        ///     Calls all the plugins for the extent form
        /// </summary>
        /// <param name="formCreationContext">Form Creation context to be used</param>
        /// <param name="foundForm">The form being found</param>
        /// <returns>Element being called</returns>
        private void CallPluginsForCollectionOrObjectForm(
            FormFactoryConfiguration configuration, 
            FormCreationContext formCreationContext, 
            ref IElement foundForm)
        {
            _formPluginState.CallFormsModificationPlugins(
                configuration,
                formCreationContext,
                ref foundForm);
            
            var rowForms = FormMethods.GetRowForms(foundForm);
            foreach (var rowForm in rowForms)
            {
                var rowFormInstance = rowForm; // Get iterative

                FormMethods.ExpandDropDownValuesOfValueReference(rowFormInstance);

                _formPluginState.CallFormsModificationPlugins(
                    configuration,
                    formCreationContext with { FormType = _DatenMeister._Forms.___FormType.Row },
                    ref rowFormInstance);
            }

            var tableForms = FormMethods.GetTableForms(foundForm);
            foreach (var tableForm in tableForms)
            {
                var tableFormInstance = tableForm; // Get iterative

                FormMethods.ExpandDropDownValuesOfValueReference(tableFormInstance);

                _formPluginState.CallFormsModificationPlugins(
                    configuration,
                    formCreationContext with
                    {
                        FormType = _DatenMeister._Forms.___FormType.Table,
                        ParentPropertyName = tableFormInstance.getOrDefault<string>(_DatenMeister._Forms._TableForm.property),
                        ParentMetaClass = formCreationContext.MetaClass,
                        MetaClass = tableFormInstance.getOrDefault<IElement>(_DatenMeister._Forms._TableForm.metaClass)
                    },
                    ref tableFormInstance);
            }
        }

        /// <summary>
        ///     Creates a new instance of the form reportCreator
        /// </summary>
        /// <returns>The created instance of the form reportCreator</returns>
        private FormCreator.FormCreator CreateFormCreator()
        {
            return FormCreator.FormCreator.Create(
                _workspaceLogic, _scopeStorage, this
            );
        }

        /// <summary>
        ///     Creates a new instance of the form reportCreator
        /// </summary>
        /// <returns>The created instance of the form reportCreator</returns>
        private FormFinder.FormFinder CreateFormFinder()
        {
            return new FormFinder.FormFinder(_plugin);
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
        private void AddExtensionFormsToObjectForm(
            IObject form,
            FindFormQuery query)
        {
            var viewFinder = CreateFormFinder();
            var foundForms = viewFinder.FindFormsFor(query);

            var tabs = form.get<IReflectiveSequence>(_DatenMeister._Forms._CollectionForm.tab);
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
                     foundForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._CollectionForm.tab)
                         .OfType<IElement>())
            {
                var tabMetaClass = tab.getMetaClass();
                if (tabMetaClass == null ||
                    !tabMetaClass.equals(_DatenMeister.TheOne.Forms.__TableForm))
                    // Not a list tab
                    continue;

                var autoGenerate = tab.getOrDefault<bool>(_DatenMeister._Forms._TableForm.autoGenerateFields);
                if (autoGenerate)
                {
                    FormMethods.AddToFormCreationProtocol(foundForm,
                        $"[FormFactory.EvaluateListFormsForAutogenerationByReflectiveCollection] Auto Creation of fields by Reflective Collection: {NamedElementMethods.GetName(tab)}");

                    var formCreator = CreateFormCreator();
                    formCreator.AddToListFormByElements(
                        tab,
                        reflectiveCollection,
                        new FormFactoryConfiguration());
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
            var listForms = FormMethods.GetTableForms(foundForm);
            foreach (var tab in listForms)
            {
                var tabMetaClass = tab.getMetaClass();
                if (tabMetaClass == null ||
                    !tabMetaClass.equals(_DatenMeister.TheOne.Forms.__TableForm))
                    // Not a list tab
                    continue;

                var autoGenerate = tab.getOrDefault<bool>(_DatenMeister._Forms._TableForm.autoGenerateFields);
                if (autoGenerate)
                {
                    var formCreator = CreateFormCreator();
                    var propertyName = tab.getOrDefault<string>(_DatenMeister._Forms._TableForm.property);
                    if (propertyName == null || string.IsNullOrEmpty(propertyName))
                    {
                        FormMethods.AddToFormCreationProtocol(foundForm,
                            $"[FormFactory.EvaluateListFormsForAutogenerationByElement] Auto Creation of fields by Element: {NamedElementMethods.GetName(tab)}");

                        formCreator.AddToListFormByElements(
                            tab,
                            new PropertiesAsReflectiveCollection(element),
                            new FormFactoryConfiguration());
                    }
                    else
                    {
                        FormMethods.AddToFormCreationProtocol(foundForm,
                            $"[FormFactory.EvaluateListFormsForAutogenerationByElement] Auto Creation of fields by Element: {NamedElementMethods.GetName(tab)}");

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

        /// <summary>
        /// Cleans up the extent form. 
        /// </summary>
        /// <param name="objectForm"></param>
        /// <param name="cleanUpTabs">Flag, whether the tabs shall also be cleaned up.
        /// Per default, the creator of the tabs should call the corresponding CleanupDetail and
        /// CleanupListForm</param>
        public void CleanupObjectForm(IElement objectForm, bool cleanUpTabs = false)
        {
            if (cleanUpTabs)
            {
                var rowForms = FormMethods.GetRowForms(objectForm);
                foreach (var detailForm in rowForms)
                {
                    CleanupRowForm(detailForm);
                }

                var tableForms = FormMethods.GetTableForms(objectForm);
                foreach (var tableForm in tableForms)
                {
                    FormMethods.CleanupTableForm(tableForm);
                }
            }
        }

        /// <summary>
        /// Cleans up the ist form by executing several default methods like, expanding the
        /// drop down values are removing duplicates
        /// </summary>
        /// <param name="rowForm">Detail form to be evaluated</param>
        public void CleanupRowForm(IElement rowForm)
        {
            FormMethods.ExpandDropDownValuesOfValueReference(rowForm);
        }


        // Some helper method which creates the button to create new elements by the extent being connected
        // to the enumeration of elements
        void SetDefaultTypesByPackages(IHasExtent? hasExtent, IObject listForm)
        {
            if (hasExtent is null) return;

            var extent = hasExtent.Extent;
            var defaultTypes = extent?.GetConfiguration().GetDefaultTypes();
            if (defaultTypes != null)
            {
                // Now go through the packages and pick the classifier and add them to the list
                foreach (var package in defaultTypes)
                {
                    var childItems =
                        package.getOrDefault<IReflectiveCollection>(_UML._Packages._Package.packagedElement);
                    if (childItems == null) continue;

                    foreach (var type in childItems.OfType<IElement>())
                    {
                        if (type.@equals(_UML.TheOne.StructuredClassifiers.__Class))
                        {
                            FormMethods.AddDefaultTypeForNewElement(listForm, package);

                            FormMethods.AddToFormCreationProtocol(
                                listForm,
                                "[FormCreator.SetDefaultTypesByPackages]: Add DefaultTypeForNewElement driven by ExtentType: " +
                                NamedElementMethods.GetName(package));
                        }
                    }
                }
            }
        }
    }
}