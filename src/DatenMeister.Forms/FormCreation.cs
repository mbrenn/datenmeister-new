using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms;

public static class FormCreation
{
    /// <summary>
    /// Defines the logger being used
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(FormCreation)); 
    
    public static FormCreationResult CreateCollectionForm(
        CollectionFormFactoryParameter parameter,
        NewFormCreationContext context,
        FormCreationResult? result = null)
    {
        result ??= new FormCreationResult();
        WalkThroughManager(
            context.Global.CollectionFormFactories, 
            manager => 
                manager.CreateCollectionFormForCollection(parameter, context, result),
            result);

        return result;
    }

    /// <summary>
    /// Evaluates all available managers to create a table view for a certain collection
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <param name="context">Context to be used</param>
    /// <param name="result">Result of the creation which lead to the creation of the result</param>
    /// <returns></returns>
    public static FormCreationResult CreateTableFormForCollection(
        IReflectiveCollection collection,
        NewFormCreationContext context,
        FormCreationResult? result = null)
    {
        result ??= new FormCreationResult();
        WalkThroughManager(
            context.Global.TableFormFactories, 
            manager => 
                manager.CreateTableFormForCollection(collection, context, result),
            result);

        return result;
    }

    /// <summary>
    /// Creates an objectform for a specific item by walking through the context 
    /// </summary>
    /// <param name="element">Element to be managed</param>
    /// <param name="context">Context for that request</param>
    /// <param name="result">Optional storage for the final result, in case
    /// the element is not set, a new element will be created and returned by the
    /// function.</param>
    /// <returns>The created form result</returns>
    public static FormCreationResult CreateObjectFormForItem(
        IObject element,
        NewFormCreationContext context,
        FormCreationResult? result = null)
    {
        result ??= new FormCreationResult();
        WalkThroughManager(
            context.Global.ObjectFormFactories,
            manager =>
                manager.CreateObjectFormForItem(element, context, result),
            result);
        return result;
    }

    /// <summary>
    /// Creates an objectform for a specific item by walking through the context 
    /// </summary>
    /// <param name="metaClass">MetaClass to be contained</param>
    /// <param name="context">Context for that request</param>
    /// <param name="result">Optional storage for the final result, in case
    /// the element is not set, a new element will be created and returned by the
    /// function.</param>
    /// <returns>The created form result</returns>
    public static FormCreationResult CreateObjectFormForMetaClass(
        IElement? metaClass,
        NewFormCreationContext context,
        FormCreationResult? result = null)
    {
        result ??= new FormCreationResult();
        WalkThroughManager(
            context.Global.ObjectFormFactories,
            manager =>
                manager.CreateObjectFormForMetaClass(metaClass, context, result),
            result);
        return result;
    }

    /// <summary>
    /// Evaluates all available managers to create a table view for a certain collection
    /// </summary>
    /// <param name="metaClass">The metaclass which is used to create the instances</param>
    /// <param name="context">Context to be used</param>
    /// <param name="result">Result of the creation which lead to the creation of the result</param>
    /// <returns></returns>
    public static FormCreationResult CreateTableFormForMetaClass(
        IElement metaClass,
        NewFormCreationContext context,
        FormCreationResult? result = null)
    {
        result ??= new FormCreationResult();
        WalkThroughManager(
            context.Global.TableFormFactories, 
            manager => 
                manager.CreateTableFormForMetaclass(metaClass, context, result),
            result);

        return result;
    }

    public static FormCreationResult CreateRowFormForMetaClass(
        IElement metaClass,
        NewFormCreationContext context,
        FormCreationResult? result = null)
    {
        result ??= new FormCreationResult();
        WalkThroughManager(
            context.Global.RowFormFactories,
            manager =>
                manager.CreateRowFormForMetaClass(metaClass, context, result),
            result);

        return result;
    }


    public static FormCreationResult CreateRowFormForItem(
        IObject item,
        NewFormCreationContext context,
        FormCreationResult? result = null)
    {
        result ??= new FormCreationResult();
        WalkThroughManager(
            context.Global.RowFormFactories,
            manager =>
                manager.CreateRowFormForItem(item, context, result),
            result);

        return result;
    }
    
    public static FormCreationResult CreateFieldForProperty(
        IObject? property,
        NewFormCreationContext context,
        FormCreationResult? result = null)
    {
        result ??= new FormCreationResult();
        
        WalkThroughManager(
            context.Global.FieldFormFactories,
            manager =>
                manager.CreateFieldForProperty(property, context, result),
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