using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms;

public static class FormCreation
{
    public static  bool IsInExtensionCreationMode(this FormCreationContext result)
    {
        return result.LocalScopeStorage.TryGet<ExtensionCreationMode>()?.IsActive == true;
    }
    
    /// <summary>
    /// Defines the logger being used
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(FormCreation)); 
    
    public static FormCreationResultOneForm CreateCollectionForm(
        CollectionFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm? result = null)
    {
        result ??= new FormCreationResultOneForm();
        WalkThroughManager(
            context.Global.CollectionFormFactories, 
            manager => 
                manager.CreateCollectionForm(parameter, context, result),
            result);
        
        // After we are having created the object form, we go through the rowforms and
        // table forms, so the system can create additional forms or updates, if requested
        if (result.IsMainContentCreated)
        {
            var tabs = result.Form.getOrDefault<IReflectiveCollection>(
                _Forms._ObjectForm.tab).OfType<IElement>();
            foreach (var tab in tabs)
            {
                var innerContext = context.Clone();
                innerContext.LocalScopeStorage.Add(new ExtensionCreationMode());

                var innerResult = new FormCreationResultMultipleForms()
                {
                    Forms = [tab],
                    IsMainContentCreated = true
                };
                
                if (tab.getMetaClass()?.equals(_Forms.TheOne.__RowForm) == true)
                {
                    var innerParameter = new RowFormFactoryParameter
                    {
                        Extent = parameter.Extent,
                        ExtentTypes = parameter.ExtentTypes,
                        MetaClass = parameter.MetaClass
                    };

                    CreateRowForm(innerParameter, innerContext, innerResult);
                }
                else 
                if (tab.getMetaClass()?.equals(_Forms.TheOne.__TableForm) == true)
                {
                    var innerParameter = new TableFormFactoryParameter
                    {
                        Extent = parameter.Extent,
                        ExtentTypes = parameter.ExtentTypes,
                        MetaClass = tab.getOrDefault<IElement>(_Forms._TableForm.metaClass),
                        ParentMetaClass = parameter.MetaClass
                    };

                    CreateTableForm(innerParameter, innerContext, innerResult);
                }   
            }
        }

        return result;
    }

    /// <summary>
    /// Creates an objectform for a specific item by walking through the context 
    /// </summary>
    /// <param name="parameter">Parameter to be evaluated</param>
    /// <param name="context">Context for that request</param>
    /// <param name="result">Optional storage for the final result, in case
    /// the element is not set, a new element will be created and returned by the
    /// function.</param>
    /// <returns>The created form result</returns>
    public static FormCreationResultOneForm CreateObjectForm(
        ObjectFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm? result = null)
    {
        parameter.MetaClass ??= parameter.Element?.AsIElement()?.getMetaClass();
        
        result ??= new FormCreationResultOneForm();
        WalkThroughManager(
            context.Global.ObjectFormFactories,
            manager =>
                manager.CreateObjectForm(parameter, context, result),
            result);
        
        // After we are having created the object form, we go through the rowforms and
        // table forms, so the system can create additional forms or updates, if requested
        if (result.IsMainContentCreated)
        {
            var tabs = result.Form.getOrDefault<IReflectiveCollection>(
                _Forms._ObjectForm.tab).OfType<IElement>();
            foreach (var tab in tabs)
            {
                var innerContext = context.Clone();
                innerContext.LocalScopeStorage.Add(new ExtensionCreationMode());

                var innerResult = new FormCreationResultMultipleForms()
                {
                    Forms = [tab],
                    IsMainContentCreated = true
                };
                
                if (tab.getMetaClass()?.equals(_Forms.TheOne.__RowForm) == true)
                {
                    var innerParameter = new RowFormFactoryParameter()
                    {
                        Extent = parameter.Extent,
                        ExtentTypes = parameter.ExtentTypes,
                        MetaClass = parameter.MetaClass,
                        Element = parameter.Element
                    };

                    CreateRowForm(innerParameter, innerContext, innerResult);
                }
                else 
                if (tab.getMetaClass()?.equals(_Forms.TheOne.__TableForm) == true)
                {
                    var innerParameter = new TableFormFactoryParameter
                    {
                        Extent = parameter.Extent,
                        ExtentTypes = parameter.ExtentTypes,
                        MetaClass = tab.getOrDefault<IElement>(_Forms._TableForm.metaClass)
                    };

                    CreateTableForm(innerParameter, innerContext, innerResult);
                }   
            }
        }
        
        return result;
    }

    /// <summary>
    /// Evaluates all available managers to create a table view for a certain collection
    /// </summary>
    /// <param name="parameter">The parameter which is used to create the instances</param>
    /// <param name="context">Context to be used</param>
    /// <param name="result">Result of the creation which lead to the creation of the result</param>
    /// <returns></returns>
    public static FormCreationResultMultipleForms CreateTableForm(
        TableFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms? result = null)
    {
        result ??= new FormCreationResultMultipleForms();
        WalkThroughManager(
            context.Global.TableFormFactories, 
            manager => 
                manager.CreateTableForm(parameter, context, result),
            result);

        return result;
    }

    public static FormCreationResultMultipleForms CreateRowForm(
        RowFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms? result = null)
    {
        parameter.MetaClass ??= parameter.Element?.AsIElement()?.getMetaClass();

        result ??= new FormCreationResultMultipleForms();
        WalkThroughManager(
            context.Global.RowFormFactories,
            manager =>
                manager.CreateRowForm(parameter, context, result),
            result);

        return result;
    }

    public static FormCreationResultOneForm CreateField(
        FieldFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm? result = null)
    {
        result ??= new FormCreationResultOneForm();
        
        WalkThroughManager(
            context.Global.FieldFormFactories,
            manager =>
                manager.CreateField(parameter, context, result),
            result);
        
        return result;
    }

    /// <summary>
    /// Walks through the managers and performs the default actions
    /// </summary>
    /// <param name="managers">Managers to be called</param>
    /// <param name="action">Action to be executed upon each manager</param>
    /// <param name="result">The result of the managing action will be evaluated here</param>
    /// <typeparam name="T">Type of the manager</typeparam>
    private static void WalkThroughManager<T>(IEnumerable<T> managers, Action<T> action, FormCreationResult result)
    {
        var anybodyManaged = false;
        var anybodyCreatedMainForm = false;
        foreach (var manager in managers)
        {
            result.IsManaged = false;

            action(manager);

            if (result.IsManaged)
            {
                result.AddToFormCreationProtocol("Managed by: " + manager);
                anybodyManaged = true;
            }

            if (result.IsMainContentCreated)
            {
                anybodyCreatedMainForm = true;
            }
            
            if (result.IsFinalized)
            {
                // Hard stop by finalization
                result.AddToFormCreationProtocol("Finalized by: " + manager);  
                break;
            }
        }

        if (!anybodyManaged)
        {
            Logger.Warn("Nobody managed the form request. Unfortunately, I do not have further information. ");
        }

        if (!anybodyCreatedMainForm)
        {
            Logger.Info("Nobody created a form. Unfortunately, I do not have further information. ");
        }
    }
}