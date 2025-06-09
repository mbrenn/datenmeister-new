using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms.FormFactory;

public class RowFormFactory : FormFactoryBase, IRowFormFactory
{
    public RowFormFactory(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        :base(workspaceLogic, scopeStorage)
    {
        
    }
    /// <summary>
    ///     Defines the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(CollectionFormFactory));
    
    
    /// <summary>
    /// Creates a row form by the given metaclass 
    /// </summary>
    /// <param name="metaClass">Metaclass to be evaluated</param>
    /// <param name="configuration">Form configuration to be used</param>
    /// <returns></returns>
    public IElement? CreateRowFormByMetaClass(IElement metaClass, FormFactoryContext? configuration)
    {
        using var _ = new StopWatchLogger(Logger, "Timing for CreateRowFormByMetaClass: ", LogLevel.Trace);
        // Ok, not an extent now do the right things
        IElement? rowForm = null;
        configuration ??= new FormFactoryContext();

        if (configuration.ViaFormFinder)
        {
            var viewFinder = CreateFormFinder();
            rowForm = viewFinder.FindFormsFor(new FindFormQuery
            {
                metaClass = metaClass,
                FormType = _Forms.___FormType.Row,
                viewModeId = configuration.ViewModeId ?? ViewModes.Default
            }).FirstOrDefault();

            if (rowForm != null)
            {
                rowForm = FormMethods.CloneForm(rowForm);
                Logger.Info("CreateRowFormByMetaClass: Found form: " + NamedElementMethods.GetFullName(rowForm));
                FormMethods.AddToFormCreationProtocol(rowForm,
                    "[FormFactory.CreateRowFormByMetaClass] Found Form via FormFinder: " + rowForm.GetUri());
            }
        }

        if (rowForm == null && configuration.ViaFormCreator)
        {
            var formCreator = new RowFormFactory(WorkspaceLogic, ScopeStorage);
            rowForm = formCreator.CreateRowFormByMetaClass(
                metaClass,
                new FormFactoryContext { IncludeOnlyCommonProperties = true, AllowFormModifications = false});

            FormMethods.AddToFormCreationProtocol(rowForm, "[FormFactory.CreateRowFormByMetaClass] Created Form via FormCreator");
        }

        if (rowForm != null)
        {
            // Adds the extension forms to the found extent
            AddExtensionFieldsToRowOrTableForm(
                rowForm,
                new FindFormQuery
                {
                    metaClass = metaClass,
                    FormType = _Forms.___FormType.RowExtension,
                    viewModeId = configuration.ViewModeId ?? ViewModes.Default
                });
                
            var formCreationContext = new FormCreationContext
            {
                FormType = _Forms.___FormType.Row,
                MetaClass = metaClass,
                IsReadOnly = configuration.IsReadOnly
            };

            FormsState.CallFormsModificationPlugins(configuration, formCreationContext, ref rowForm);
                
            CleanupRowForm(rowForm);
        }

        // No Form
        return rowForm;
    }

    /// <summary>
    /// Creates a row form for the specific item
    /// Here, the metaclass or the included properties are used
    /// </summary>
    /// <param name="element">Element to be evaluted</param>
    /// <param name="context">Configuration how this element shall be evaluated</param>
    /// <returns>The created element</returns>
    public IElement? CreateRowFormForItem(IObject element, FormFactoryContext context)
    {
        ArgumentNullException.ThrowIfNull(element);
        
        using var _ = new StopWatchLogger(Logger, "Timing for CreateRowFormForItem: ", LogLevel.Trace);
        IElement? foundForm = null;
        var extent = (element as IHasExtent)?.Extent;

        if (context.ViaFormFinder)
        {
            // Tries to find the form
            var viewFinder = new FormFinder.FormFinder(FormMethods);
            foundForm = viewFinder.FindFormsFor(
                new FindFormQuery
                {
                    extentUri = extent?.GetUri() ?? string.Empty,
                    workspaceId = extent?.GetWorkspace()?.id ?? string.Empty,
                    metaClass = (element as IElement)?.getMetaClass(),
                    FormType = _Forms.___FormType.Row,
                    extentTypes = extent == null ? [] : extent.GetConfiguration().ExtentTypes,
                    viewModeId = context.ViewModeId
                }).FirstOrDefault();

            if (foundForm != null)
            {
                foundForm = FormMethods.CloneForm(foundForm);
                Logger.Info("CreateRowFormForItem: Found form: " + NamedElementMethods.GetFullName(foundForm));
                FormMethods.AddToFormCreationProtocol(foundForm,
                    "[FormFactory.CreateRowFormForItem] Found Form via FormFinder: " + foundForm.GetUri());
            }
        }

        if (foundForm == null && context.ViaFormCreator)
        {
            // Ok, we have not found the form. So create one
            var formCreator = new RowFormFactory(WorkspaceLogic, ScopeStorage);
            foundForm = formCreator.CreateRowFormForItem(element, context);
            FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateRowFormForItem] Created Form via FormCreator");
        }

        if (foundForm != null)
        {
            FormsState.CallFormsModificationPlugins(
                context,
                new FormCreationContext
                {
                    MetaClass = (element as IElement)?.getMetaClass(),
                    FormType = _Forms.___FormType.Row,
                    ExtentTypes = extent?.GetConfiguration().ExtentTypes ?? [],
                    DetailElement = element,
                    IsReadOnly = context.IsReadOnly
                },
                ref foundForm);
                
            CleanupRowForm(foundForm);
        }

        return foundForm;
    }
   
}