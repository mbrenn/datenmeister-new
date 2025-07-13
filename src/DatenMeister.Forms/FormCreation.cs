using BurnSystems.Logging;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms;

public static class FormCreation
{
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
        result ??= new FormCreationResultOneForm();
        WalkThroughManager(
            context.Global.ObjectFormFactories,
            manager =>
                manager.CreateObjectForm(parameter, context, result),
            result);
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
        result ??= new FormCreationResultMultipleForms();
        WalkThroughManager(
            context.Global.RowFormFactories,
            manager =>
                manager.CreateRowForm(parameter, context, result),
            result);

        return result;
    }

    public static FormCreationResultOneForm CreateFieldForProperty(
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