using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.TypeIndexAssembly.Helper;

namespace DatenMeister.DataView.Evaluation.Transformation;

public class SelectByPropertyEvaluation : IDataViewNodeEvaluation
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(SelectByPropertyEvaluation));
    
    /// <inheritdoc />
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Transformation.__SelectByProperty);
    }

    /// <inheritdoc />
    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        
        var inputNode = viewNode.getOrDefault<IElement?>(_DataViews._Transformation._SelectByProperty.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            throw new InvalidOperationException("Input node not found");
        }

        var input = evaluation.GetElementsForViewNode(inputNode);

        var propertyName = viewNode.getOrDefault<string?>(_DataViews._Transformation._SelectByProperty.propertyName);
        if (propertyName == null)
        {
            Logger.Warn("propertyName is not set");
            throw new InvalidOperationException("propertyName is not set");
        }
        
        var result = new List<IElement>();
        foreach (var item in input.OfType<IElement>())
        {
            result.AddRange(item.ForceAsEnumerable(propertyName).OfType<IElement>());
        }
        
        return new TemporaryReflectiveSequence(result);
    }
}